using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.University;
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
    public class UniversityService : IUniversityService
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public UniversityService(IUniversityRepository universityRepository, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _universityRepository = universityRepository;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }

        public async Task<ServiceResult> CreateUniversity(AddUniversity model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<UniversityMaster>(model);

                SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                string password = RandomStringGenerator.GenerateRandomString(8, "Capital");
                obj.Password = Encryption.EncodeToBase64(password);
                var _result = await _universityRepository.AddAsync(obj);

                serviceResult.ResultData = _result;
                if (serviceResult.ResultData == 1)
                {
                    var PortalLink = _configuration.GetValue<string>("EmailConfiguration:Sender");
                    StringBuilder strMail = new StringBuilder();
                    strMail.AppendLine("<p>Dear College,</P>");
                    strMail.AppendLine("<p>Your login details are as follows:</P>");
                    strMail.AppendLine("<p>Link: " + PortalLink + " </P>");
                    strMail.AppendLine("<p>User Name:" + model.UniversityEmail + "</P>");
                    strMail.AppendLine("<p>Password:" + password + "</P>");
                    strMail.AppendLine("<p>Note: Please change password after login</P>");
                    strMail.AppendLine("<p>Thanks</P>");
                    serviceResult.Message = MessageConfig.RecordSaved;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    await emailUtility.SendEmailAsync(model.UniversityEmail, strMail.ToString(), MessageConfig.UniversityMailSubject);
                    await smsUtility.SendSmsAsync(model.UniversityPhoneNumber, "Please check your Higher Education Portal login detail in your registered email.", "1407161820695085199");

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
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Create University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> DeleteUniversity(int UniversityId, int UserId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _universityRepository.DeleteUniversityById(UniversityId, UserId);
                serviceResult.ResultData = _result;
                if (serviceResult.ResultData != null)
                {
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordDeleted;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 0)
                    {
                        serviceResult.Message = MessageConfig.UniversityDeactivate;
                        serviceResult.Status = false;
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
                _logError.WriteTextToFile("Delete University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetAllUniversity(int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder,Boolean onBoard)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _universityRepository.GetAllUniversities(pageNumber, pageSize, searchKeyword, sortBy, sortOrder,onBoard);

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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
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
                _logError.WriteTextToFile("Get All University : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetAllUniversities()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _universityRepository.GetAllUniversity();

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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
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
                _logError.WriteTextToFile("Get All University : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetUniversityById(int universityId)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _universityRepository.GetUniversityById(universityId);

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
                _logError.WriteTextToFile("Get Employee By Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> UpdateUniversity(UpdateUniversity model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<updateuniversityMaster>(model);
                var _result = _universityRepository.UpdateUniversityAsync(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
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

        public async Task<ServiceResult> GetAllUniversitiesPG()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _universityRepository.GetAllUniversityPG();

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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
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
                _logError.WriteTextToFile("Get All University : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }
    }

}


