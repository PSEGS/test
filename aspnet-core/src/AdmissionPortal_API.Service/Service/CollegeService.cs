using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using AdmissionPortal_API.Utility.MessageConfig;
using AdmissionPortal_API.Utility.Notification;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.Service
{
    public class CollegeService : ICollegeService
    {
        private readonly ICollegeRepository _collegeRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public CollegeService(ICollegeRepository collegeRepository, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _collegeRepository = collegeRepository;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }

        public async Task<ServiceResult> CreateCollegeAsync(AddCollege model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                var obj = _mapper.Map<AddCollege>(model);
                SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                string password = RandomStringGenerator.GenerateRandomString(8, "Capital");
                obj.password = Encryption.EncodeToBase64(password);
                var _result = _collegeRepository.AddColleges(obj);

                if (_result != null)
                {
                    serviceResult = _result.Result;
                    if (serviceResult.StatusCode == 1)
                    {
                        var PortalLink = _configuration.GetValue<string>("EmailConfiguration:Sender");
                        //StringBuilder strMail = new StringBuilder();

                        //strMail.AppendLine("<p>Dear College,</P>");
                        //strMail.AppendLine("<p>Your login details are as follows:</P>");
                        //strMail.AppendLine("<p>Link: " + PortalLink + " </P>");
                        //strMail.AppendLine("<p>User Name:" + serviceResult.ResultData + "</P>");
                        //strMail.AppendLine("<p>Password:" + password + "</P>");
                        //strMail.AppendLine("<p>Note: Please change password after login</P>");
                        //strMail.AppendLine("<p>Thanks</P>");

                        StringBuilder str = new StringBuilder();
                        str.AppendLine("<b>Dear</b> " + model.CollegeName + ",");
                        str.AppendLine("<br/>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<b>URL for Online Admission Portal, 2022-23 is https://admission.punjab.gov.in </b>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<br/>");
                        str.AppendLine("College User ID is<b> " + serviceResult.ResultData + "</b>");
                        str.AppendLine("<br/>");
                        str.AppendLine("Default password is<b> " + password + "</b>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<br/>");
                        str.AppendLine("It is advised to change the password at 1st time Login. The college Login credentials must be kept confidential.");
                        str.AppendLine("<br/>");
                        str.AppendLine("<br/>");
                        str.AppendLine("All future communications will take place on the College Principal Email ID and Mobile No. registered on the admission portal");
                        str.AppendLine("<br/>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<b>Note:</b> The sanctity of the College information under College Login will be the sole responsibility of College principal");
                        str.AppendLine("<br/>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<b>Dept. of Higher Education,</b>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<b>State of Punjab</b>");

                        string subj = "IMPORTANT: College Admission Portal Login Details";

                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        await emailUtility.SendEmailAsync(model.CollegeEmail, str.ToString(), subj);
                        await smsUtility.SendSmsAsync(model.CollegeContact, "Your College "+ model.CollegeName + " has been registered at Higher Education Portal.College User ID is "+ serviceResult.ResultData + ", Default Password is "+ password + ". -DHE, Punjab", "1407165526678907378");

                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.AlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Create College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetAllCollege(int? university, int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _collegeRepository.GetAllCollege(university, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get All Colleges : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> GetCollegeById(int collegeId)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetCollegesById(collegeId);
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        if (serviceResult.ResultData.prospectusRef != null)
                        {
                            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.prospectusRef);
                            serviceResult.ResultData.prospectusRef = Convert.ToString(photo.Result.ResultData);
                        }
                        if (serviceResult.ResultData.SFCancelledCheque != null)
                        {
                            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.SFCancelledCheque);
                            serviceResult.ResultData.SFCancelledCheque =Convert.ToString(photo.Result.ResultData);
                        }
                        if (serviceResult.ResultData.CancelledCheque != null)
                        {
                            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.CancelledCheque);
                            serviceResult.ResultData.CancelledCheque = Convert.ToString(photo.Result.ResultData);
                        }
                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get Employee By Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> UpdateCollege(UpdateCollege model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];

                //var obj = _mapper.Map<CollegeMaster>(model);
                var _result = _collegeRepository.Updatecollege(model);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.EmailAlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Create College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);

        }



        public async Task<ServiceResult> DeleteCollege(int CollegeId, int userid)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _collegeRepository.DeleteCollegeById(CollegeId, userid);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordDeleted;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Delete College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetCollegeByDistrictId(int districtId, int collegeTypeId, int admissionId, string type, string ugpg)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetCollegeByDistrictId(districtId, collegeTypeId, admissionId, type, ugpg);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> GetCollegeCourses(int collegeId, string CGtype)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetCollegeCourses(collegeId, CGtype);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }
        public async Task<ServiceResult> GetAllColleges()
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetAllColleges();

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> ResetCollegePassword(int collegeId, int Stataus)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.ResetCollegePassword(collegeId, Stataus);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result.ResultData;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Reset Password : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> CollegeInactiveActive(int collegeId, int Stataus, int modifiedBy)
        {
            ServiceResult serviceResult = new ServiceResult(); 
            EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
            SMSUtility smsUtility = new SMSUtility(_configuration, _logError);

            try
            {
                var _result = _collegeRepository.ActiveInActive(collegeId, Stataus, modifiedBy);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        string message = string.Empty;
                        string MsgTemplate = string.Empty;
                        if (Stataus == 0)
                        {
                            message= "You college ID has been successfully reactivated. -DHE, Punjab";
                            MsgTemplate = "1407165526706136192";
                        }
                        else
                        {
                            message = "You college ID has been disabled. Kindly contact admin for further details. -DHE, Punjab";
                            MsgTemplate = "1407165526715582604";

                        }
                        var PortalLink = _configuration.GetValue<string>("EmailConfiguration:Sender");

                        StringBuilder str = new StringBuilder();
                        str.AppendLine("<b>Dear</b> " + serviceResult.ResultData.CollegeName + ",");
                        str.AppendLine("<br/>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<b>"+ message.Replace("-DHE, Punjab","") + " </b>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<b>Dept. of Higher Education,</b>");
                        str.AppendLine("<br/>");
                        str.AppendLine("<b>State of Punjab</b>");

                        string subj = "IMPORTANT: Admission Portal Notification";

                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        await emailUtility.SendEmailAsync(serviceResult.ResultData.Email, str.ToString(), subj);
                        await smsUtility.SendSmsAsync(serviceResult.ResultData.Mobile, message, MsgTemplate);
                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("College Inactive Active : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> lockunlockCollegeInfo(int collegeId, int Status, int modifiedBy, string otp)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.lockunlockCollegeInfo(collegeId, Status, modifiedBy, otp);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("College Inactive Active : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> UpdateCollegeNew(UpdateCollegeModel model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<UpdateCollegeModel>(model);
                var _result = _collegeRepository.updateCollegeNew(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.EmailAlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Update College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> generateOTP(int collegeId)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                string otp = RandomStringGenerator.GenerateRandomString(6, "Capital");

                var _result = _collegeRepository.GenerateOTP(collegeId, otp);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != "0")
                    {
                        await smsUtility.SendSmsAsync(serviceResult.ResultData, "Your Higher Education Portal one time password (OTP) for bank account verification is " + otp + ".", "1407162148521946922");

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        serviceResult.ResultData = 1;
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("College Generate OTP : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> UploadProspectus(UploadCollegeProspectus model)
        {

            ServiceResult serviceResult = new ServiceResult();
            try
            {

                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                if (model.prospectus != null)
                {

                    if (StringExtensions.IsBase64String(model.prospectus))
                    {
                        var _blobResult = _blobService.UploadProspectusBlobUsingFileReferenceType(blobAccessKey, containerName, model.prospectus);
                        if (_blobResult.Result.StatusCode == 1)
                        {
                            model.prospectusRef = _blobResult.Result.ResultData;
                        }
                        else if (_blobResult.Result.StatusCode == 3)
                        {
                            serviceResult.Message = _blobResult.Result.Message;

                        }
                        else
                        {
                            serviceResult.Message = _blobResult.Result.Message;
                        }
                    }
                    else
                    {
                        model.prospectusRef = model.prospectus;
                    }
                }
                //var obj = _mapper.Map<UpdateCollegeModel>(model);
                var _result = _collegeRepository.UploadProspectus(model);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.EmailAlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Upload Prospectus : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> UploadCancelledCheque(UploadCancelledChequeModel model)
        {

            ServiceResult serviceResult = new ServiceResult();
            try
            {

                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                if (model.CancelledCheque != null)
                {

                    if (StringExtensions.IsBase64String(model.CancelledCheque))
                    {
                        var _blobResult = _blobService.UploadProspectusBlobUsingFileReferenceType(blobAccessKey, containerName, model.CancelledCheque, model.CollegeID + "_College_");
                        if (_blobResult.Result.StatusCode == 1)
                        {
                            model.CancelledCheque = _blobResult.Result.ResultData;
                        }
                        else if (_blobResult.Result.StatusCode == 3)
                        {
                            serviceResult.Message = _blobResult.Result.Message;

                        }
                        else
                        {
                            serviceResult.Message = _blobResult.Result.Message;
                        }
                    }
                    else
                    {
                        model.CancelledCheque = model.CancelledCheque;
                    }
                }

                var _result = _collegeRepository.UploadCancelledCheque(model);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.EmailAlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Upload Cancelled Cheque  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        //public ServiceResult GetAllCitizen(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        //{
        //    ServiceResult serviceResult = new ServiceResult();
        //    try
        //    {
        //        var _result = _universityRepository.GetAllCitizen(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);

        //        if (_result != null)
        //        {
        //            serviceResult.ResultData = _result.Result;
        //            if (serviceResult.ResultData != null)
        //            {
        //                serviceResult.Message = MessageConfig.Success;
        //                serviceResult.Status = true;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                serviceResult.Message = MessageConfig.ErrorOccurred;
        //                serviceResult.ResultData = null;
        //                serviceResult.Status = false;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            }
        //        }
        //        else
        //        {
        //            serviceResult.Message = MessageConfig.ErrorOccurred;
        //            serviceResult.ResultData = null;
        //            serviceResult.Status = false;
        //            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResult.Message = MessageConfig.ErrorOccurred;
        //        serviceResult.ResultData = null;
        //        serviceResult.Status = false;
        //        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        _logError.WriteTextToFile("Get All Citizen : " , ex.Message, ex.HResult, ex.StackTrace); ;
        //    }
        //    return serviceResult;
        //}

        //public ServiceResult GetCitizenById(int CitizenID)
        //{
        //    ServiceResult serviceResult = new ServiceResult();
        //    string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
        //    string containerName = _configuration["BlobContainers:ProfileImage"];
        //    try
        //    {
        //        var _result = _universityRepository.GetByIdAsync(CitizenID);

        //        if (_result != null)
        //        {
        //            serviceResult.ResultData = _result.Result;
        //            if (serviceResult.ResultData != null)
        //            {
        //                var result = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.ImageReference);
        //                if (result.Result.ResultData != null)
        //                {
        //                    serviceResult.ResultData.ImageCode = result.Result.ResultData;
        //                }
        //                else
        //                {
        //                    serviceResult.ResultData.ImageCode = null;
        //                }
        //                serviceResult.Message = MessageConfig.Success;
        //                serviceResult.Status = true;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                serviceResult.Message = MessageConfig.ErrorOccurred;
        //                serviceResult.ResultData = null;
        //                serviceResult.Status = false;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            }
        //        }
        //        else
        //        {
        //            serviceResult.Message = MessageConfig.ErrorOccurred;
        //            serviceResult.ResultData = null;
        //            serviceResult.Status = false;
        //            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResult.Message = MessageConfig.ErrorOccurred;
        //        serviceResult.ResultData = null;
        //        serviceResult.Status = false;
        //        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        _logError.WriteTextToFile("Get Citizen By Id : " , ex.Message, ex.HResult, ex.StackTrace); ;
        //    }
        //    return serviceResult;
        //}

        //public ServiceResult UpdateCitizen(UpdateCitizen model)
        //{
        //    ServiceResult serviceResult = new ServiceResult();
        //    try
        //    {
        //        var obj = _mapper.Map<CitizenMaster>(model);
        //        var _result = _universityRepository.UpdateAsync(obj);

        //        if (_result != null)
        //        {
        //            serviceResult.ResultData = _result.Result;
        //            if (serviceResult.ResultData == 1)
        //            {
        //                serviceResult.Message = MessageConfig.RecordUpdated;
        //                serviceResult.Status = true;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                serviceResult.Message = MessageConfig.ErrorOccurred;
        //                serviceResult.ResultData = null;
        //                serviceResult.Status = false;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            }
        //        }
        //        else
        //        {
        //            serviceResult.Message = MessageConfig.ErrorOccurred;
        //            serviceResult.ResultData = null;
        //            serviceResult.Status = false;
        //            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResult.Message = MessageConfig.ErrorOccurred;
        //        serviceResult.ResultData = null;
        //        serviceResult.Status = false;
        //        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        _logError.WriteTextToFile("Update Citizen : " , ex.Message, ex.HResult, ex.StackTrace); ;
        //    }
        //    return serviceResult;
        //}

        //public ServiceResult ForgotPassword(string emailId)
        //{
        //    ServiceResult serviceResult = new ServiceResult();
        //    try
        //    {
        //        var result = _universityRepository.ForgotPassword(emailId);

        //        if (result != null)
        //        {
        //            serviceResult.ResultData = result.Result;
        //            if (serviceResult.ResultData !=null)
        //            {
        //                serviceResult.Message = MessageConfig.Success;
        //                serviceResult.ResultData = result.Result;
        //                serviceResult.Status = true;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                serviceResult.Message = MessageConfig.InvalidRecord;
        //                serviceResult.ResultData = null;
        //                serviceResult.Status = false;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
        //            }
        //        }
        //        else
        //        {
        //            serviceResult.Message = MessageConfig.InvalidRecord;
        //            serviceResult.ResultData = null ;
        //            serviceResult.Status = false;
        //            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResult.Message = MessageConfig.ErrorOccurred;
        //        serviceResult.ResultData = null;
        //        serviceResult.Status = false;
        //        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        _logError.WriteTextToFile("Forgot Password : " , ex.Message, ex.HResult, ex.StackTrace); ;
        //    }
        //    return serviceResult;

        //}

        //public ServiceResult ResetPassword(string OTP, string password)
        //{
        //    ServiceResult serviceResult = new ServiceResult();
        //    try
        //    {
        //        var _result = _universityRepository.ResetPassword(OTP, password);

        //        if (_result != null)
        //        {
        //            serviceResult.ResultData = _result.Result;
        //            if (serviceResult.ResultData == 1)
        //            {
        //                serviceResult.Message = MessageConfig.PasswordUpdated;
        //                serviceResult.ResultData = _result.Result;
        //                serviceResult.Status = true;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                serviceResult.Message = MessageConfig.ErrorOccurred;
        //                serviceResult.ResultData = null;
        //                serviceResult.Status = false;
        //                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //            }
        //        }
        //        else
        //        {
        //            serviceResult.Message = MessageConfig.ErrorOccurred;
        //            serviceResult.ResultData = _result;
        //            serviceResult.Status = false;
        //            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResult.Message = MessageConfig.ErrorOccurred;
        //        serviceResult.ResultData = null;
        //        serviceResult.Status = false;
        //        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
        //        _logError.WriteTextToFile("Reset Password : " , ex.Message, ex.HResult, ex.StackTrace); ;
        //    }
        //    return serviceResult;

        //}
        //public ServiceResult UniversityLogin(CitizenLogin model)
        //{

        //    ServiceResult serviceResult = new ServiceResult();
        //    try
        //    {
        //        string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
        //        string containerName = _configuration["BlobContainers:ProfileImage"];
        //        var obj = _mapper.Map<CitizenLoginMaster>(model);
        //        var result = _universityRepository.GetUniversityLogin(obj);
        //        if (result.Result.CitizenID == 0)
        //        {
        //            serviceResult.Message = MessageConfig.InvalidEmailPassword;
        //            serviceResult.ResultData = result.Result;
        //            serviceResult.Status = false;
        //            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);

        //        }
        //        else
        //        {
        //            var data = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, result.Result.ImageReference);
        //            if (data.Result.ResultData != null)
        //            {
        //                result.Result.ImageCode = data.Result.ResultData;
        //            }
        //            else
        //            {
        //                result.Result.ImageCode = null;
        //            }
        //            result.Result.Token = JWTBearerAuthentication.GenerateJSONWebToken(model.Email, Convert.ToString(result.Result.CitizenID), nameof(UserType.Citizen), _configuration);
        //            serviceResult.Message = MessageConfig.Success;
        //            serviceResult.ResultData = result.Result;
        //            serviceResult.Status = true;
        //            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logError.WriteTextToFile("Citizen Login : " , ex.Message, ex.HResult, ex.StackTrace);
        //    }
        //    return serviceResult;
        //}

        public async Task<ServiceResult> GetDistrictCollege(int districtId, int collegeTypeId, int admissionId, string type)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetDistrictCollege(districtId, collegeTypeId, admissionId, type);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> GetDistrictCollegesByGender(int districtId, int collegeTypeId, int admissionId, int studentId, string type, string ugpg)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetDistrictCollegesByGender(districtId, collegeTypeId, admissionId, studentId, type, ugpg);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("GetDistrictColleges By Gender : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> GetCollegeByCGtype(string type)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetCollegeByCGtype(type);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }
        public async Task<ServiceResult> ReportsLogin(int userId, string username, string userType)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string Token = Guid.NewGuid().ToString();
                var _result = await _collegeRepository.ReportsLogin(userId, Token);

                //if (_result != null)
                //{
                serviceResult.ResultData = "https://edureports.psegs.in/Default.aspx?g=" + Token;
                if (serviceResult.ResultData != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
                //}
                //else
                //{
                //    serviceResult.Message = MessageConfig.ErrorOccurred;
                //    serviceResult.ResultData = null;
                //    serviceResult.Status = false;
                //    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                //}
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get college report login : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
        public async Task<ServiceResult> UnlockStudent(UnlockStudentModel model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string Token = Guid.NewGuid().ToString();
                var _result = await _collegeRepository.UnlockStudent(model);

                //if (_result != null)
                //{
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.STudentNoUnLock;
                        serviceResult.ResultData = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
                //}
                //else
                //{
                //    serviceResult.Message = MessageConfig.ErrorOccurred;
                //    serviceResult.ResultData = null;
                //    serviceResult.Status = false;
                //    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                //}
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("UnLock Student By College : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetStudentDetailsByRegId(string RegId, string type, int collegeId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _collegeRepository.GetStudentDetailsByRegId(RegId, type, collegeId);

                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get Subject By Student Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> CancelStudentAdmissionSeat(CancelAdmissionSeat model)
        {
            ServiceResult serviceResult = new ServiceResult();
            SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
            try
            {
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                if (model.Doc != null)
                {
                    if (StringExtensions.IsBase64String(model.Doc))
                    {
                        var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Doc);
                        if (_blobResult.Result.StatusCode == 1)
                        {
                            model.DocReference = _blobResult.Result.ResultData;
                            var _result = await _collegeRepository.CancelStudentAdmissionSeat(model);
                            if (_result != null)
                            {
                                serviceResult.ResultData = _result;
                                if (serviceResult.ResultData != null)
                                {
                                    EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                                    StringBuilder str = new StringBuilder();
                                    string sms = "";
                                    string smsId = "";
                                    str.AppendLine("<p>Dear " + serviceResult.ResultData.StudentName + ",</p>");
                                    str.AppendLine("</br>");
                                    str.AppendLine("<p>As per your request, your admission to " + serviceResult.ResultData.CollegeName + " for " + serviceResult.ResultData.CourseName + " has been cancelled. Please contact your college for refund.</p>");
                                    str.AppendLine("</br>");
                                    str.AppendLine("<p>DHE, Punjab</p>");

                                    sms = $"Dear " + serviceResult.ResultData.StudentName + ", As per your request, your admission to " + serviceResult.ResultData.CollegeName + " for " + serviceResult.ResultData.CourseName + " has been cancelled. Please contact your college for refund. DHE Punjab";
                                    smsId = "1407163168133440490";
                                    await smsUtility.SendSmsAsync(serviceResult.ResultData.Mobile, sms, smsId);
                                    await emailUtility.SendEmailAsync(serviceResult.ResultData.Email, str.ToString(), "DHE Punjab");
                                    serviceResult.Message = MessageConfig.Success;
                                    serviceResult.Status = true;
                                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                                }
                                else
                                {
                                    serviceResult.Message = MessageConfig.ErrorOccurred;
                                    serviceResult.ResultData = null;
                                    serviceResult.Status = false;
                                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                                }
                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.ErrorOccurred;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                            }
                        }
                        else
                        {
                            serviceResult.Message = _blobResult.Result.Message;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }

            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("CancelStudentAdmissionSeat: ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }



        public async Task<ServiceResult> GetStudentDocumentDownload(string CollegeID, string type)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetStudentDocumentDownload(CollegeID, type);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("GetStudentDocumentDownload : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> GetCollegeProspectus(string CollegeID)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            try
            {
                var _result = _collegeRepository.GetCollegeProspectus(CollegeID);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (_result != null)
                    {
                        serviceResult.ResultData = _result.Result;
                        if (serviceResult.ResultData != null)
                        {
                            var result = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData);
                            if (result.Result.ResultData != null)
                            {
                                serviceResult.ResultData = result.Result.ResultData;
                            }
                            else
                            {
                                serviceResult.ResultData = null;
                            }
                            serviceResult.Message = MessageConfig.Success;
                            serviceResult.Status = true;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ErrorOccurred;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get College Prospectus : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetCollegesIslock(int Admissiontype)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _collegeRepository.GetCollegesIslock(Admissiontype);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get All Islock colleges : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }
    }
}




