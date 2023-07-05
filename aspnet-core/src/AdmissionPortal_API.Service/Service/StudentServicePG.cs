using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ApiModel.StudentPG;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.UserTypeEnum;

namespace AdmissionPortal_API.Service.Service
{
    public class StudentServicePG : IStudentServicePG
    {
        private readonly IStudentRepositoryPG _studentRepositoryPG;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public StudentServicePG(IStudentRepositoryPG studentRepositoryPG, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _studentRepositoryPG = studentRepositoryPG;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }

        public async Task<ServiceResult> RegisterStudentPG(RegisterStudent model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                string password = RandomStringGenerator.GenerateRandomString(8, "Capital");
                model.Password = Encryption.EncodeToBase64(password);
                model.RegistrationNumber = "PG" + model.PassingYear.Substring(model.PassingYear.Length - 2) + RandomStringGenerator.GenerateRandomString(10, "Numeric");
                var _result = _studentRepositoryPG.AddAsync(model);
                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData.ResponseCode == 1)
                    {
                        StringBuilder str = new StringBuilder();
                        str.AppendLine("<p>Dear <b>" + model.Name + "</b>,</p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("Registration Successful.");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p><b>Please check your login detail:</b></p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("URL for Online Admission Portal 2022-23 is");
                        str.AppendLine("<a href='https://admission.punjab.gov.in/'>https://admission.punjab.gov.in/</a>");
                        str.AppendLine("<p>Your registration ID is <b>" + model.RegistrationNumber + "</b></p>");
                        str.AppendLine("<p>Password: <b>" + password + "</b></p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p>All future communications will take place on Email ID <b>" + model.Email + "</b> and Mobile No. <b>" + model.Mobile + "</b> registered on the admission portal.</p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p>Thank You</p>");
                        str.AppendLine("<p>Dept. of Higher Education,</p>");
                        str.AppendLine("<p>State of Punjab</p>");
                        await emailUtility.SendEmailAsync(model.Email, str.ToString(), "Digital Punjab Student Login Detail");
                        await smsUtility.SendSmsAsync(model.Mobile, "Dear " + model.Name + " Registration is Successful. Your registration ID : " + model.RegistrationNumber + ", Password: " + password + ". -DHE, Punjab", "1407162633865778084");
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
                _logError.WriteTextToFile("Register Student PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> StudentLoginPG(StudentPGLogin model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = await _studentRepositoryPG.GetAsync(model);
                if (result.Student_ID == 0)
                {
                    serviceResult.Message = MessageConfig.InvalidEmailPassword;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {
                    result.Token = JWTBearerAuthentication.GenerateJSONWebToken(result.Email, Convert.ToString(result.Student_ID), nameof(UserType.Student), "PG", string.Empty, model.UserName, string.Empty, _configuration, result.Name, result.Mobile);
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Student Login PG: ", ex.Message, ex.HResult, ex.StackTrace);
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
                var _result = await _studentRepositoryPG.UpdateStudentPersonalDetails(model);
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
                var _result = await _studentRepositoryPG.UpdateBankDetails(model);
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
                var _result = await _studentRepositoryPG.UpdateAddressDetails(model);
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

        public async Task<ServiceResult> UpdateAcademicDetails(UpdateAcademicDetailsPG model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepositoryPG.UpdateAcademicDetails(model);
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

        public async Task<ServiceResult> UpdateWeightage(WeightagePG model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepositoryPG.UpdateWeightage(model);
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

        public async Task<ServiceResult> UploadDocuments(UploadDocumentsPG model)
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
            bool graduation = false;
            //bool physicaldisability = false;
            bool pwd = false;
            bool esm = false;
            bool sports = false;
            bool freedomFighter = false;
            bool residence = false;
            bool migration = false;
            bool character = false;
            bool isRural = false;
            bool borderArea = false;
            bool Kashmiri = false;
            bool nriPassport = false;
            bool patient = false;
            bool IsAdvanceYouthLeadershiptrainingcamp = false; bool IsYouthLeadershiptrainingcamp = false;
            bool IsAdvancedMountaineering = false; bool IsHikingTraining = false; bool IsMountaineering = false;
            bool IsZonalYouthFestival = false; bool IsInterUniversityNationalYouthFestival = false;
            bool IsUniversityLevelYouthFestival = false;
            bool sizeOk = true;


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
            if (model.Character != null)
            {
                twelveth = true;
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
            if (model.graduation != null)
            {
                graduation = true;
                if (StringExtensions.IsBase64String(model.graduation))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.graduation);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.graduationReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.graduationReference = model.graduation;
                }
            }
            //if (model.physicalDisability != null)
            //{
            //    physicaldisability = true;
            //    if (StringExtensions.IsBase64String(model.physicalDisability))
            //    {
            //        var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.physicalDisability);
            //        if (_blobResult.Result.StatusCode == 1)
            //        {
            //            model.physicalDisabilityReference = _blobResult.Result.ResultData;
            //        }
            //        else
            //        {
            //            serviceResult.Message = _blobResult.Result.Message;
            //        }
            //    }
            //    else
            //    {
            //        model.physicalDisabilityReference = model.physicalDisability;
            //    }
            //}
            if (model.Sports != null)
            {
                sports = true;
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
            if (model.freedomFighter != null)
            {
                freedomFighter = true;
                if (StringExtensions.IsBase64String(model.freedomFighter))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.freedomFighter);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.freedomFighterReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.freedomFighterReference = model.freedomFighter;
                }
            }
            if (model.PWD != null)
            {
                pwd = true;
                if (StringExtensions.IsBase64String(model.PWD))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.PWD);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.PWDReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.PWDReference = model.PWD;
                }
            }
            if (model.ESM != null)
            {
                esm = true;
                if (StringExtensions.IsBase64String(model.ESM))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.ESM);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.ESMReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.ESMReference = model.ESM;
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
            if (model.kashmiriMigrant != null)
            {
                Kashmiri = true;
                if (StringExtensions.IsBase64String(model.kashmiriMigrant))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.kashmiriMigrant);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.kashmiriMigrantReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.kashmiriMigrantReference = model.kashmiriMigrant;
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
            if (model.NriPassport != null)
            {
                nriPassport = true;
                if (StringExtensions.IsBase64String(model.NriPassport))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.NriPassport);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.NriPassportReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.NriPassportReference = model.NriPassport;
                }
            }
            if (model.PatientProof != null)
            {
                patient = true;
                if (StringExtensions.IsBase64String(model.PatientProof))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.PatientProof);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.PatientProofReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.PatientProofReference = model.PatientProof;
                }
            }
            if (model.AdvanceYouthLeadershipTrainingCamp != null)
            {
                IsAdvanceYouthLeadershiptrainingcamp = true;
                if (StringExtensions.IsBase64String(model.AdvanceYouthLeadershipTrainingCamp))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.AdvanceYouthLeadershipTrainingCamp);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.AdvanceYouthLeadershiptrainingcampReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.AdvanceYouthLeadershiptrainingcampReference = model.AdvanceYouthLeadershipTrainingCamp;
                }
            }
            if (model.YouthLeadershipTrainingCamp != null)
            {
                IsYouthLeadershiptrainingcamp = true;
                if (StringExtensions.IsBase64String(model.YouthLeadershipTrainingCamp))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.YouthLeadershipTrainingCamp);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.YouthLeadershipTrainingCampReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.YouthLeadershipTrainingCampReference = model.YouthLeadershipTrainingCamp;
                }
            }
            if (model.AdvancedMountaineering != null)
            {
                IsAdvancedMountaineering = true;
                if (StringExtensions.IsBase64String(model.AdvancedMountaineering))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.AdvancedMountaineering);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.AdvancedMountaineeringReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.AdvancedMountaineeringReference = model.AdvancedMountaineering;
                }
            }
            if (model.HikingTraining != null)
            {
                IsHikingTraining = true;
                if (StringExtensions.IsBase64String(model.HikingTraining))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.HikingTraining);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.HikingTrainingReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.HikingTrainingReference = model.HikingTraining;
                }
            }
            if (model.Mountaineering != null)
            {
                IsMountaineering = true;
                if (StringExtensions.IsBase64String(model.Mountaineering))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Mountaineering);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.MountaineeringReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.MountaineeringReference = model.Mountaineering;
                }
            }
            if (model.ZonalYouthFestival != null)
            {
                IsZonalYouthFestival = true;
                if (StringExtensions.IsBase64String(model.ZonalYouthFestival))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.ZonalYouthFestival);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.ZonalYouthFestivalReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.ZonalYouthFestivalReference = model.ZonalYouthFestival;
                }
            }
            if (model.UniversityLevelYouthFestival != null)
            {
                IsUniversityLevelYouthFestival = true;
                if (StringExtensions.IsBase64String(model.UniversityLevelYouthFestival))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.UniversityLevelYouthFestival);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.UniversityLevelYouthFestivalReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.UniversityLevelYouthFestivalReference = model.UniversityLevelYouthFestival;
                }
            }
            if (model.InterUniversityNationalYouthFestival != null)
            {
                IsInterUniversityNationalYouthFestival = true;
                if (StringExtensions.IsBase64String(model.InterUniversityNationalYouthFestival))
                {
                    var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.InterUniversityNationalYouthFestival);
                    if (_blobResult.Result.StatusCode == 1)
                    {
                        model.InterUniversityNationalYouthFestivalReference = _blobResult.Result.ResultData;
                    }
                    else
                    {
                        sizeOk = false;
                        serviceResult.Message = _blobResult.Result.Message;
                    }
                }
                else
                {
                    model.InterUniversityNationalYouthFestivalReference = model.ZonalYouthFestival;
                }
            }
            if (model.Rural != null)
            {
                isRural = true;
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
            if (!sizeOk)
            {
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            else
            {

                if (tenth == true && photo == true && signature == true || (twelveth == true || ncc == true || nss == true || youthWelfare == true || bc == true
                    || sc == true || residence == true || migration == true || character == true || borderArea || Kashmiri || graduation || sports
                    || freedomFighter || pwd || esm || nriPassport || patient || IsAdvanceYouthLeadershiptrainingcamp || IsYouthLeadershiptrainingcamp
                    || IsAdvancedMountaineering || IsHikingTraining || IsMountaineering || IsZonalYouthFestival || IsUniversityLevelYouthFestival || IsInterUniversityNationalYouthFestival))
                {
                    try
                    {
                        var _result = await _studentRepositoryPG.UploadDocuments(model);
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
                var _result = await _studentRepositoryPG.GetStudentById(studentID);

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
                var _result = await _studentRepositoryPG.GetSubjectByStudentId(studentID);

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
                var _result = await _studentRepositoryPG.GetProgressBar(studentID);

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
                var _result = await _studentRepositoryPG.UpdateDeclarations(model);
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
                var _result = await _studentRepositoryPG.GenerateOTP(userName, otp);

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
                var _result = await _studentRepositoryPG.UpdateCourseChoice(model);
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
                var _result = await _studentRepositoryPG.GetUploadedDocuments(studentID);
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
                var _result = await _studentRepositoryPG.GetCourseChoiceByStudentId(studentID);

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
                var _result = await _studentRepositoryPG.RegisterationFeesPayment(model);
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
                var _result = await _studentRepositoryPG.UnlockForm(oTP, studentId);

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
                serviceResult.Message = MessageConfig.InvalidOTP;
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
                var _result = await _studentRepositoryPG.UpdateStudentDetails(model);
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
                ForgotLoginMaster result = await _studentRepositoryPG.ForgotPassword(Email);
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

        public async Task<ServiceResult> GetStudentDetailsByRegId(string RegId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepositoryPG.GetStudentDetailsByRegId(RegId);

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
                var _result = await _studentRepositoryPG.GetStudentAppDetailsByRegId(_filter);

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
                var _result = await _studentRepositoryPG.GetCourseChoiceByRegId(RegId);

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
                var _result = await _studentRepositoryPG.GetSubjectByRegId(RegId);

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
                var _result = await _studentRepositoryPG.GetDocumentsByRegId(RegId);
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
                _logError.WriteTextToFile("Get Uploaded Documents PG : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetObjectionsByPGStudentID(Int32 studentid)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _studentRepositoryPG.GetObjectionsByPGStudentID(studentid);

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
                var _result = await _studentRepositoryPG.GetAdmissionSeatDetails(Studentid);

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
                var _result = await _studentRepositoryPG.GetAdmissionFeeReceipt(Studentid, transactionId);

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
                var _result = await _studentRepositoryPG.GetAdmissionFeeReceiptList(Studentid);

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
