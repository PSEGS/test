using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using AdmissionPortal_API.Utility.MessageConfig;
using AdmissionPortal_API.Utility.Notification;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.UserTypeEnum;

namespace AdmissionPortal_API.Service.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public StudentService(IStudentRepository studentRepository, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }

        public async Task<ServiceResult> RegisterStudent(AddStudent model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                string password = RandomStringGenerator.GenerateRandomString(8, "Capital");
                model.Password = Encryption.EncodeToBase64(password);
                string BoardFirstCharacter = string.Empty;
                if (model.BoardName.ToLower().Contains("cbse"))
                {
                    BoardFirstCharacter = "C";
                }
                else if (model.BoardName.ToLower().Contains("punjab"))
                {
                    BoardFirstCharacter = "P";
                }
                else
                {
                    BoardFirstCharacter = "O";
                }
                model.RegistrationNumber = "U" + BoardFirstCharacter + model.PassingYear.Substring(model.PassingYear.Length - 2) + DateTime.Today.Year.ToString().Substring(DateTime.Today.Year.ToString().Length - 2) + RandomStringGenerator.GenerateRandomString(10, "Numeric");
                if (model.text != null)
                {
                    string imageBase64 = await ImageToBase64String.GetImageAsBase64Url(model.text);
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, imageBase64);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.ProfileImageReference = _blobResult.Result.ResultData;
                    }
                }
                else
                {
                    model.ProfileImageReference = null;
                }


                var _result = _studentRepository.AddAsync(model);
                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData.ResponseCode == 1)
                    {
                        StringBuilder str = new StringBuilder();
                        str.AppendLine("<p>Dear " + model.name + ",</p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p><b>Please check your login detail:</b></p>");
                        str.AppendLine("<p><b>User Name:</b> " + model.Email + "</p>");
                        str.AppendLine("<p><b>Password:</b> " + password + "</p>");
                        str.AppendLine("<p><b>Registration Number:</b> " + model.RegistrationNumber + "</p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p>Thanks</p>");
                        str.AppendLine("<p>DHE Punjab</p>");
                        await emailUtility.SendEmailAsync(model.Email, str.ToString(), "Digital Punjab Student Login Detail");

                        await smsUtility.SendSmsAsync(model.Mobile, "Dear " + model.name + " Registration is Successful. Your registration ID : " + model.RegistrationNumber + ", Password: " + password + ". -DHE, Punjab", "1407162633865778084");
                        serviceResult.Message = serviceResult.ResultData.SysMsg;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        serviceResult.ResultData = new { RegistrationNumber = model.RegistrationNumber, Password = password, status = 1 };
                    }
                    else
                    {
                        serviceResult.Message = serviceResult.ResultData.SysMsg;
                        serviceResult.ResultData = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
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
                _logError.WriteTextToFile("Register Student : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> StudentLogin(StudentLogin model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                var result = await _studentRepository.GetAsync(model);
                if (result.Student_ID == 0)
                {
                    serviceResult.Message = MessageConfig.InvalidOTP;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else if (result.Student_ID == -1)
                {
                    serviceResult.Message = MessageConfig.InvalidEmailPassword;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {
                    result.Token = JWTBearerAuthentication.GenerateJSONWebToken(result.Email, Convert.ToString(result.Student_ID), nameof(UserType.Student), "UG", string.Empty, model.UserName, string.Empty, _configuration, result.Name, result.Mobile);
                    //var data = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, result.ImageReference);
                    //if (data.Result.ResultData != null)
                    //{
                    //    result.ImageCode = data.Result.ResultData;
                    //}
                    //else
                    //{
                    //    result.ImageCode = null;
                    //}
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Student Login : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> UpdateStudentPersonalDetails(UpdatePersonalDetails model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UpdateStudentPersonalDetails(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Student Personal Details : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateBankDetails(UpdateBankDetails model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UpdateBankDetails(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Bank Details : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateAddressDetails(UpdateAddressDetails model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UpdateAddressDetails(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Address Details : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateAcademicDetails(UpdateAcademicDetails model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UpdateAcademicDetails(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Academic Details : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateWeightage(Weightage model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UpdateWeightage(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Weightage : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UploadDocuments(UploadDocuments model)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            bool tenth = false;
            bool twelveth = false;
            bool photo = false;
            bool signature = false;
            bool nss = false;
            bool ncc = false;
            bool youthWelfare = false;
            bool bc = false;
            bool sc = false;
            bool income = false;
            bool residence = false;
            bool migration = false;
            bool character = false;
            bool borderArea = false;
            bool IsFreedomFighter = false;
            bool IsSports = false;
            bool IsexServiceMan = false;
            bool Kashmiri = false;
            bool pwd = false;
            bool sizeOk = true;
            if (model.Character != null)
            {
                tenth = true;
                if (StringExtensions.IsBase64String(model.Character))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Character);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.CharacterReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.CharacterReference = model.Character;
                }
            }
            if (model.TenthCertificate != null)
            {
                tenth = true;
                if (StringExtensions.IsBase64String(model.TenthCertificate))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.TenthCertificate);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.TenthCertificateReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.TenthCertificateReference = model.TenthCertificate;
                }
            }
            if (model.TwelvethCertificate != null)
            {
                twelveth = true;
                if (StringExtensions.IsBase64String(model.TwelvethCertificate))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.TwelvethCertificate);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.TwelvethCertificateReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.TwelvethCertificateReference = model.TwelvethCertificate;
                }
            }
            if (model.Photo != null)
            {
                photo = true;
                if (StringExtensions.IsBase64String(model.Photo))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Photo);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.PhotoReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.PhotoReference = model.Photo;
                }
            }
            if (model.Signature != null)
            {
                signature = true;
                if (StringExtensions.IsBase64String(model.Signature))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Signature);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.SignatureReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.SignatureReference = model.Signature;
                }
            }
            if (model.NSS != null)
            {
                nss = true;
                if (StringExtensions.IsBase64String(model.NSS))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.NSS);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.NSSReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.NSSReference = model.NSS;
                }
            }
            if (model.NCC != null)
            {
                ncc = true;
                if (StringExtensions.IsBase64String(model.NCC))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.NCC);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.NCCReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.NCCReference = model.NCC;
                }
            }
            if (model.YouthWelfare != null)
            {
                youthWelfare = true;
                if (StringExtensions.IsBase64String(model.YouthWelfare))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.YouthWelfare);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.YouthWelfareReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.YouthWelfareReference = model.YouthWelfare;
                }
            }

            if (model.BC != null)
            {
                bc = true;
                if (StringExtensions.IsBase64String(model.BC))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.BC);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.BCReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.BCReference = model.BC;
                }
            }

            if (model.SC != null)
            {
                sc = true;
                if (StringExtensions.IsBase64String(model.SC))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.SC);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.SCReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.SCReference = model.SC;
                }
            }

            if (model.Income != null)
            {
                income = true;
                if (StringExtensions.IsBase64String(model.Income))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Income);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.IncomeReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.IncomeReference = model.Income;
                }
            }

            if (model.Residence != null)
            {
                residence = true;
                if (StringExtensions.IsBase64String(model.Residence))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Residence);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.ResidenceReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.ResidenceReference = model.Residence;
                }
            }
            if (model.Rural != null)
            {

                if (StringExtensions.IsBase64String(model.Rural))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Rural);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.RuralReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.RuralReference = model.Rural;
                }
            }
            if (model.Migration != null)
            {
                migration = true;
                if (StringExtensions.IsBase64String(model.Migration))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Migration);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.MigrationReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.MigrationReference = model.Migration;
                }
            }
            if (model.SchoolLeaving != null)
            {
                character = true;
                if (StringExtensions.IsBase64String(model.SchoolLeaving))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.SchoolLeaving);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.SchoolLeavingReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.SchoolLeavingReference = model.SchoolLeaving;
                }
            }
            if (model.BorderAreaCertificate != null)
            {
                borderArea = true;
                if (StringExtensions.IsBase64String(model.BorderAreaCertificate))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.BorderAreaCertificate);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.BorderAreaCertificateReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.BorderAreaCertificateReference = model.BorderAreaCertificate;
                }
            }
            if (model.KashmiriMigrant != null)
            {
                Kashmiri = true;
                if (StringExtensions.IsBase64String(model.KashmiriMigrant))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.KashmiriMigrant);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.KashmiriMigrantReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.KashmiriMigrantReference = model.KashmiriMigrant;
                }
            }
            if (model.FreedomFighter != null)
            {
                IsFreedomFighter = true;
                if (StringExtensions.IsBase64String(model.FreedomFighter))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.FreedomFighter);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.FreedomFighterReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.FreedomFighterReference = model.FreedomFighter;
                }
            }
            if (model.Sports != null)
            {
                IsSports = true;
                if (StringExtensions.IsBase64String(model.Sports))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Sports);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.SportsReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.SportsReference = model.Sports;
                }
            }
            if (model.ExServiceMan != null)
            {
                IsexServiceMan = true;
                if (StringExtensions.IsBase64String(model.ExServiceMan))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.ExServiceMan);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.ExServiceManReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.ExServiceManReference = model.ExServiceMan;
                }
            }
            if (model.physicalDisability != null)
            {
                pwd = true;
                if (StringExtensions.IsBase64String(model.physicalDisability))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.physicalDisability);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.physicalDisabilityReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.physicalDisabilityReference = model.physicalDisability;
                }
            }
            if (!sizeOk)
            {
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            else
            {
                if (tenth == true && photo == true && signature == true || (twelveth == true || ncc == true || nss == true || youthWelfare == true || bc == true || sc == true || income == true || residence == true || migration == true || character == true || borderArea || IsFreedomFighter || IsSports || IsexServiceMan || Kashmiri || pwd))
                {
                    try
                    {
                        var _result = await _studentRepository.UploadDocuments(model);
                        if (_result != 0)
                        {
                            serviceResult.ResultData = _result;
                            if (serviceResult.ResultData == 1)
                            {
                                serviceResult.Message = MessageConfig.DocumentsUploaded;
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
                        _logError.WriteTextToFile("Upload Documents : ", ex.Message, ex.HResult, ex.StackTrace);
                    }
                }
                else
                {

                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetStudentById(int studentID)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetStudentById(studentID);

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
                _logError.WriteTextToFile("Get Student By Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetSubjectByStudentId(int studentID)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetSubjectByStudentId(studentID);

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

        public async Task<ServiceResult> GetProgressBar(int studentID)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetProgressBar(studentID);

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
                _logError.WriteTextToFile("Get Progress Bar : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateDeclarations(Declaration model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UpdateDeclarations(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Declarations : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GenerateOTP(string userName)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                string otp = RandomStringGenerator.GenerateRandomString(6, "Capital");
                var _result = await _studentRepository.GenerateOTP(userName, otp);

                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != "2")
                    {
                        StringBuilder str = new StringBuilder();
                        str.AppendLine("<p>Dear User,</p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p><b>Please find your OTP:</b></p>");
                        str.AppendLine("<p><b>OTP: </b> " + otp + "</p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p>Thanks</p>");
                        str.AppendLine("<p>DHE Punjab</p>");
                        if (userName.Length > 10)
                        {
                            await emailUtility.SendEmailAsync(userName, str.ToString(), "Digital Punjab Student Login Detail");
                        }
                        else
                        {
                            await smsUtility.SendSmsAsync(userName, "Your One Time Password (OTP) for admission portal login is " + otp + ".", "1407161820681122056");
                        }

                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.Status = true;
                        serviceResult.ResultData = "OTP has been sent to your Registered Mobile No.";
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
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
                _logError.WriteTextToFile("Generate OTP : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateCourseChoice(CourseChoice model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UpdateCourseChoice(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Course Choice : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetUploadedDocuments(int studentID)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetUploadedDocuments(studentID);
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        if (serviceResult.ResultData.Photo != null)
                        {
                            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.Photo);
                            serviceResult.ResultData.base64Photo = photo.Result.ResultData.ToString();
                        }
                        else
                        {
                            serviceResult.ResultData.base64Photo = null;
                        }

                        if (serviceResult.ResultData.Signature != null)
                        {
                            var signature = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.Signature);
                            serviceResult.ResultData.base64Signature = signature.Result.ResultData.ToString();
                        }
                        else
                        {
                            serviceResult.ResultData.base64Signature = null;
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
                _logError.WriteTextToFile("Get Uploaded Documents : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public ServiceResult GetBlobDocument(string blobReference)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                //Download file from Blob
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                Dictionary<string, string> resData = new Dictionary<string, string>();

                var data = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, blobReference);
                string extension = StringExtensions.GetFileExtension(data.Result.ResultData.ToString());
                resData.Add("base64", data.Result.ResultData.ToString());
                resData.Add("type", extension);
                serviceResult.ResultData = resData;
                serviceResult.Status = true;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                serviceResult.Message = ex.Message;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = ex.HResult;
                _logError.WriteTextToFile("Get Blob Document : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;

        }

        public async Task<ServiceResult> GetCourseChoiceByStudentId(int studentID)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetCourseChoiceByStudentId(studentID);

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

        public async Task<ServiceResult> RegisterationFeesPayment(RegisterationFees model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.RegisterationFeesPayment(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Registeration Fees Payment : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UnlockForm(string oTP, int studentId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UnlockForm(oTP, studentId);

                serviceResult.ResultData = _result;
                if (serviceResult.ResultData == 1)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else if (serviceResult.ResultData == 0)
                {
                    serviceResult.Message = MessageConfig.InvalidOTP;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Unlock Form : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateStudentDetails(UpdateStudentDetails model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.UpdateStudentDetails(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Student Details : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }
        public async Task<ServiceResult> ForgotPassword(string Email)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                ForgotLoginMaster result = await _studentRepository.ForgotPassword(Email);
                if (result.response == "1")
                {
                    EmailUtility _email = new EmailUtility(_configuration, _logError);
                    StringBuilder str = new StringBuilder();
                    str.AppendLine("<p>Dear " + result.Name + ",</p>");
                    str.AppendLine("</br>");
                    str.AppendLine("</br>");
                    str.AppendLine("<p><b>Please check your login detail:</b></p>");
                    str.AppendLine("<p><b>User Name:</b> " + result.UserName + "</p>");
                    str.AppendLine("<p><b>Password:</b> " + Decryption.DecodeFrom64(result.Password) + "</p>");
                    str.AppendLine("</br>");
                    str.AppendLine("</br>");
                    str.AppendLine("<p>Thanks</p>");
                    str.AppendLine("<p>DHE Punjab</p>");
                    var response = await _email.SendEmailAsync(Email, str.ToString(), "Digital Punjab Login Detail");

                    serviceResult.Message = response.Message;
                    if (response.Status == true)
                    {
                        serviceResult.Status = true;
                        serviceResult.StatusCode = 200;
                    }
                    else
                    {
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK); ;
                    }
                }
                else
                {
                    serviceResult.Message = "Email not found in our records";
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Forgot Password : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetStudentDetailsByRegId(string RegId, string RollNo)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetStudentDetailsByRegId(RegId, RollNo);

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
        public async Task<ServiceResult> GetStudentAppDetailsByRegId(FilterStudent _filter)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetStudentAppDetailsByRegId(_filter);

                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        if (serviceResult.ResultData.Student_ID > 0)
                        {
                            serviceResult.Message = MessageConfig.Success;
                            serviceResult.Status = true;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        }
                        else
                        {
                            serviceResult.ResultData = null;
                            serviceResult.Message = MessageConfig.RegistrationNotFound;
                            serviceResult.Status = true;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
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
                _logError.WriteTextToFile("Get Student By Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
        public async Task<ServiceResult> GetCourseChoiceByRegId(string RegId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetCourseChoiceByRegId(RegId);

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
        public async Task<ServiceResult> GetSubjectByRegId(string RegId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetSubjectByRegId(RegId);

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

        public async Task<ServiceResult> GetDocumentsByRegId(string RegId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetDocumentsByRegId(RegId);
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        if (serviceResult.ResultData.Photo != null)
                        {
                            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.Photo);
                            serviceResult.ResultData.base64Photo = photo.Result.ResultData.ToString();
                        }
                        else
                        {
                            serviceResult.ResultData.base64Photo = null;
                        }

                        if (serviceResult.ResultData.Signature != null)
                        {
                            var signature = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.Signature);
                            serviceResult.ResultData.base64Signature = signature.Result.ResultData.ToString();
                        }
                        else
                        {
                            serviceResult.ResultData.base64Signature = null;
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
                _logError.WriteTextToFile("Get Uploaded Documents : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateCourseChoiceWithSubject(SubjectCombinationsWithCollege model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                int SubjectCount = 0;
                int BACombinationCount = 3;
                Boolean messagecheck = false;
                foreach (var clg in model.SubjectCombinations)
                {
                    if (clg.CollegeCourseId == 41)//Course 41: BA
                    {
                        SubjectCount = await _studentRepository.checkCollegeSubject(clg.CollegeId, clg.CollegeCourseId);
                        if (SubjectCount < 5 || SubjectCount == 5)
                        {
                            BACombinationCount = 3;
                            messagecheck = true;
                        }
                        int BASbjectCount = model.SubjectCombinations.Where(x => x.CollegeCourseId == 41 && x.CollegeId == clg.CollegeId).Count();
                        if (BASbjectCount < BACombinationCount)
                        {
                            if (messagecheck)
                            {
                                serviceResult.Message = MessageConfig.MustCreateThreeCombinations;

                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.MustCreateThreeCombinations;
                            }
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                            return serviceResult;
                        }
                        else if (BASbjectCount > 15)
                        {
                            serviceResult.Message = MessageConfig.CantCreateMoreThanFifteenCombinations;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                            return serviceResult;
                        }
                    }
                }


                var _result = await _studentRepository.UpdateCourseChoiceWithSubject(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Update Course Choice With Subject: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetCourseChoiceFee(CourseChoiceFee model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetCourseChoiceFee(model);

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
                _logError.WriteTextToFile("Get Selected Course Fee : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetEligibleCourseByStudentID(Int32 CollegeId, Int32 StudentId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.getEligibleCourseByStudentIDs(CollegeId, StudentId);

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

        public async Task<ServiceResult> GetObjectionsByStudentID(Int32 Studentid)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetObjectionsByStudentID(Studentid);

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

        public async Task<ServiceResult> GetAdmissionSeatDetails(Int32 Studentid)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetAdmissionSeatDetails(Studentid);

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
        public async Task<ServiceResult> GetAdmissionFeeReceipt(Int32 Studentid, string transactionId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetAdmissionFeeReceipt(Studentid, transactionId);

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
                _logError.WriteTextToFile("Get fee By Student Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
        public async Task<ServiceResult> GetAdmissionFeeReceiptList(Int32 Studentid)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepository.GetAdmissionFeeReceiptList(Studentid);

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
                _logError.WriteTextToFile("Get fee By Student Id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
    }
}
