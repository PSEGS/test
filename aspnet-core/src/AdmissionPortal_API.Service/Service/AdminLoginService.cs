using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Admin;
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
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.UserTypeEnum;

namespace AdmissionPortal_API.Service.Service
{
    public class AdminLoginService : IAdminLoginService
    {
        private readonly IMapper _mapper;
        private readonly IAdminLoginRepository _adminLoginRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        private readonly IBlobService _blobService;
        public readonly IStudentRepository _studentRepository;
        private readonly IStudentRepositoryPG _studentRepositoryPG;

        public AdminLoginService(IMapper mapper, IAdminLoginRepository adminLoginRepository, IConfiguration configuration, ILogError logError, IBlobService blobService, IStudentRepository studentRepository, IStudentRepositoryPG studentRepositoryPG)
        {
            _logError = logError;
            _mapper = mapper;
            _adminLoginRepository = adminLoginRepository;
            _configuration = configuration;
            _blobService = blobService;
            _studentRepository = studentRepository;
            _studentRepositoryPG = studentRepositoryPG;
        }
        public async Task<ServiceResult> AdminLogin(AdminLogin model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                var obj = _mapper.Map<AdminLoginMaster>(model);
                var result = await _adminLoginRepository.GetAsync(obj);
                if (result.EmployeeID == 0)
                {
                    serviceResult.Message = MessageConfig.InvalidEmailPassword;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else if (result.EmployeeID == -1)
                {
                    serviceResult.Message = MessageConfig.UserInActive;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {
                    result.Token = JWTBearerAuthentication.GenerateJSONWebToken(model.Email, Convert.ToString(result.EmployeeID), nameof(UserType.Admin), Convert.ToString(result.LoginType), Convert.ToString(result.UniversityCollegeId), Convert.ToString(result.User_Name), Convert.ToString(result.HeadId), _configuration);
                    var data = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, result.ImageReference);
                    if (data.Result.ResultData != null)
                    {
                        result.ImageCode = data.Result.ResultData;
                    }
                    else
                    {
                        result.ImageCode = null;
                    }
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Admin Login : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> UpdatePassword(ChangePassword model, string userType, string userId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.ResultData = await _adminLoginRepository.UpdateAsync(model, userType, userId);

                if (serviceResult.ResultData.STATUS == 1)
                {
                    EmailUtility _email = new EmailUtility(_configuration, _logError);
                    StringBuilder str = new StringBuilder();
                    str.AppendLine("<p>Hello "+serviceResult.ResultData.collegeName + "");
                    str.AppendLine("</br>");
                    str.AppendLine("</br>");
                    str.AppendLine("<p><b>You have successfully changed your college login password on admission portal.</b></p>");
                    str.AppendLine("</br>");
                    str.AppendLine("</br>");
                    str.AppendLine("<p>Thanks</p>");
                    str.AppendLine("<p>DHE Punjab</p>");
                    var response = await _email.SendEmailAsync(serviceResult.ResultData.email, str.ToString(), "Digital Punjab - Password Change");

                    SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                    await smsUtility.SendSmsAsync(serviceResult.ResultData.Mobile, "You have successfully changed your college login password on admission portal. -DHE, Punjab", "1407165526725478722");
                    serviceResult.Message = MessageConfig.PasswordUpdated;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else if (serviceResult.ResultData.STATUS == 2)
                {
                    serviceResult.Message = MessageConfig.WrongPassword;
                    serviceResult.ResultData = null;
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
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Change Password : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> ResetEmployeePassword(int Id)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = await _adminLoginRepository.ResetEmployeePassword(Id);
                if (result == 1)
                {
                    serviceResult.Message = MessageConfig.PasswordUpdated;
                    serviceResult.ResultData = result; ;
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
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Reset Employee Password : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> ForgotPassword(string Email)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                ForgotLoginMaster result = await _adminLoginRepository.ForgotPassword(Email);
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
                    var response = await _email.SendEmailAsync(result.UserName, str.ToString(), "Digital Punjab Login Detail");

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
                _logError.WriteTextToFile("Forgot Employee Password : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> CancelStudentRegistrationByRegId(CancelStudentRegistration model)
        {
            //TODO: As discussed crrently merit list feature is not developed, 
            //Pending - Do not cancel registration if student is in merit list
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                StudentMaster student = new StudentMaster();
                FilterStudent filter = new FilterStudent();
                if (!string.IsNullOrEmpty(model.RegType) && model.RegType == "UG")
                {
                    filter.regId = model.RegId;
                    student = await _studentRepository.GetStudentAppDetailsByRegId(filter);
                }
                else if (!string.IsNullOrEmpty(model.RegType) && model.RegType == "PG")
                {
                    filter.regId = model.RegId;
                    student = await _studentRepositoryPG.GetStudentAppDetailsByRegId(filter);
                }

                var _result = await _adminLoginRepository.CancelStudentRegistrationByRegId(model);
                if (_result != 0)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData == 1)
                    {
                        if (student != null)
                        {
                            EmailUtility _email = new EmailUtility(_configuration, _logError);
                            StringBuilder str = new StringBuilder();
                            str.AppendLine("<p>Dear Student");
                            str.AppendLine("</br>");
                            str.AppendLine("</br>");
                            str.AppendLine("<p><b>Your Higher Education Portal registration is cancelled. Kindly register again to apply for the admissions.</b></p>");
                            str.AppendLine("</br>");
                            str.AppendLine("</br>");
                            str.AppendLine("<p>Thanks</p>");
                            str.AppendLine("<p>DHE Punjab</p>");
                            var response = await _email.SendEmailAsync(student.Email, str.ToString(), "Digital Punjab - Registration Cancelled");

                            SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                            await smsUtility.SendSmsAsync(student.Mobile, "Your Higher Education Portal registration is cancelled. Kindly register again to apply for the admissions.", "1407162148555429189");
                        }

                        serviceResult.Message = MessageConfig.StatusUpdated;
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
                _logError.WriteTextToFile("Cancel Student Registration By RegId : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }


        public async Task<ServiceResult> viewStudentObjections(string RegId, string admission, Int32 college)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _adminLoginRepository.viewStudentObjections(RegId, admission, college);

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
                _logError.WriteTextToFile("Get Student objections : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }


        public async Task<ServiceResult> UploadNotification(UploadNotification model)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            bool sizeOk = true;


            if (model.Notification != null)
            {
                var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.Notification, "Notification");
                if (_blobResult.Result.StatusCode == 1)
                {
                    model.NotificationReference = _blobResult.Result.ResultData;
                }
                else
                {
                    sizeOk = false;
                    serviceResult.Message = _blobResult.Result.Message;
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
                try
                {
                    var _result = await _adminLoginRepository.UploadNotification(model);
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
                    _logError.WriteTextToFile("Upload Notification : ", ex.Message, ex.HResult, ex.StackTrace);
                }
            }
            return serviceResult;
        }


        public ServiceResult GetUploadedNotification(int status)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            try
            {
                var _result = _adminLoginRepository.GetUploadedNotification(status);

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
                _logError.WriteTextToFile("Get Uploaded Notification : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public ServiceResult GetNotificationbyPath(string model)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            try
            {
                var _result = _adminLoginRepository.GetNotificationbyPath(model);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        if (serviceResult.ResultData.NotificationReference != null)
                        {
                            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.NotificationReference);
                            serviceResult.ResultData.Notification = photo.Result.ResultData.ToString();
                            serviceResult.ResultData.NotificationType = StringExtensions.GetFileExtension(serviceResult.ResultData.Notification);
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
                _logError.WriteTextToFile("Get Uploaded Notification : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

    }
}

