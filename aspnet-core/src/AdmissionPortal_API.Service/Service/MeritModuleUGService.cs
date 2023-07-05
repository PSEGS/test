using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
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

namespace AdmissionPortal_API.Service.Service
{
    public class MeritModuleUGService : IMeritModuleUGService
    {
        private readonly IMeritModuleUGRepository _meritModuleUGRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public MeritModuleUGService(IMeritModuleUGRepository meritModuleUGRepository, IMapper mapper, IConfiguration configuration, ILogError logError)
        {
            _logError = logError;
            _meritModuleUGRepository = meritModuleUGRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResult> GetProvisionalMeritList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetProvisionalList(CollegeId, CourseId, ReservationId, CategoryId, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);

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
                _logError.WriteTextToFile("Get provisional merit list ug : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetWaitingList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetWaitingList(CollegeId, CourseId, ReservationId, CategoryId, IsBorderArea, SingleGirlChild, CancerAidsThalassemia, NRI, IsKashmiriMigrant, RuralArea, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);

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
                _logError.WriteTextToFile("Get provisional merit list ug : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetCourseFeeByStudentId(int CourseId, int CollegeId, int StudentId, string Combination)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetCourseFeeByStudentId(CourseId, CollegeId, StudentId, Combination);

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
                _logError.WriteTextToFile("Get provisional merit list ug : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> SaveAdmissionSeat(AdmissionSeat model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                OTPRequestModel objOTPRequestModel = new OTPRequestModel();
                var _result = await _meritModuleUGRepository.SaveAdmissionSeat(model);
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData.Response == 1)
                    {
                        objOTPRequestModel.CollegeId = Convert.ToInt32(model.CollegeId);
                        objOTPRequestModel.CourseId = Convert.ToInt32(model.CourseId);
                        objOTPRequestModel.StudentReferenceNumber = model.RegistrationNumber;
                        await SendOTP(objOTPRequestModel);

                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                    }
                    else if (serviceResult.ResultData.Response == 2)
                    {
                        serviceResult.Message = MessageConfig.SeatFull;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData.Response == 3)
                    {
                        serviceResult.Message = MessageConfig.SeatFull;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData.Response == 4)
                    {
                        serviceResult.Message = MessageConfig.CourseSeatFull;
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
                _logError.WriteTextToFile("save admission seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetAdmissionSeatStatus(string RegId, string CollegeId, string AdmissionType)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetAdmissionSeatStatus(RegId, CollegeId, AdmissionType);
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        if (_result.Course.Count() > 0)
                        {
                            serviceResult.Message = MessageConfig.Success;
                            serviceResult.Status = true;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        }
                        else
                        {
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
                _logError.WriteTextToFile("get admission seat status: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetCourseChoiceByRegId(string RegId, Int32 CourseId, Int32 CollegeId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetCourseChoiceByRegId(RegId, CourseId, CollegeId);
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
                _logError.WriteTextToFile("get combination by reg id: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetFeeReceiptByRegId(string RegId, Int32 CollegeId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetFeeReceiptByRegId(RegId, CollegeId);
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
                _logError.WriteTextToFile("get fee receipt by reg id: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetCategoryCombination()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetCategoryCombination();
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
                _logError.WriteTextToFile("Get Category Combination ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> ExportMeritExcel(int collegeId, int? courseId, int? ReservationId, int? CategoryId, string searchKeyword)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _meritModuleUGRepository.ExportMeritExcel(collegeId, courseId, ReservationId, CategoryId, searchKeyword);

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
                _logError.WriteTextToFile("Merit excel ug: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> SendOTP(OTPRequestModel model)
        {
            StringBuilder str = new StringBuilder();

            ServiceResult serviceResult = new ServiceResult();
            SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
            EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
            try
            {
                string sms = string.Empty;
                string smsId = string.Empty;
                var OTP = await smsUtility.GenerateRandomOTP();
                var _result = await _meritModuleUGRepository.SendOTP(model, "UG", OTP);
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData.Response == 1)
                    {
                        if (serviceResult.ResultData.Mobile != null)
                        {
                            if (serviceResult.ResultData.PaymentMode == "Offline")
                            {
                                str.AppendLine("<p>Dear " + serviceResult.ResultData.StudentName + ",</p>");
                                str.AppendLine("</br>");
                                str.AppendLine("</br>");
                                str.AppendLine("<p><b>Congratulations.</b></p>");
                                str.AppendLine("<p><b>You have been offered a Seat in </b> " + serviceResult.ResultData.CollageName + " for " + serviceResult.ResultData.CourseName + "</p>");
                                str.AppendLine("<p><b>Kindly reach to college with this otp:</b>" + OTP + " for offline admission.</p>");
                                str.AppendLine("</br>");
                                //str.AppendLine("<p><b>Book seat on the same day of offer by college, else the seat offer will get expire by midnight.</b></p>");
                                str.AppendLine("<p><b>Book your seat offered by the college at the earliest, else the seat offer will get expire by tomorrow midnight.</b></p>");
                                str.AppendLine("</br>");
                                str.AppendLine("</br>");
                                str.AppendLine("<p>Thanks</p>");
                                str.AppendLine("<p>DHE Punjab</p>");
                                await emailUtility.SendEmailAsync(serviceResult.ResultData.EmailId, str.ToString(), "DHE, PUNJAB");

                                sms = $"Dear {serviceResult.ResultData.StudentName},Congratulations. You have been offered a Seat in { serviceResult.ResultData.CollageName} for { serviceResult.ResultData.CourseName}. Kindly reach to college with this otp: {OTP} for offline admission. -DHE, PUNJAB";
                                smsId = "1407163066212615321";
                                await smsUtility.SendSmsAsync(serviceResult.ResultData.Mobile, sms, smsId);

                            }
                            else if (serviceResult.ResultData.PaymentMode == "Online")
                            {
                                str.AppendLine("<p>Dear " + serviceResult.ResultData.StudentName + ",</p>");
                                str.AppendLine("</br>");
                                str.AppendLine("</br>");
                                str.AppendLine("<p><b>Congratulations.</b></p>");
                                str.AppendLine("<p><b>You have been offered a Seat in </b> " + serviceResult.ResultData.CollageName + " for " + serviceResult.ResultData.CourseName + "</p>");
                                str.AppendLine("<p><b>Kindly login on </b> <a href='https://admission.punjab.gov.in/'>https://admission.punjab.gov.in/</a> to pay & confirm your seat</p>");
                                str.AppendLine("</br>");
                                //str.AppendLine("<p><b>Book seat on the same day of offer by college, else the seat offer will get expire by midnight.</b></p>");
                                str.AppendLine("<p><b>Book your seat offered by the college at the earliest, else the seat offer will get expire by tomorrow midnight.</b></p>");
                                str.AppendLine("</br>");
                                str.AppendLine("</br>");
                                str.AppendLine("<p>Thanks</p>");
                                str.AppendLine("<p>DHE Punjab</p>");
                                await emailUtility.SendEmailAsync(serviceResult.ResultData.EmailId, str.ToString(), "DHE, PUNJAB");

                                sms = $"Dear {serviceResult.ResultData.StudentName},Congratulations. You have been offered a Seat in { serviceResult.ResultData.CollageName} for { serviceResult.ResultData.CourseName}. Kindly login on { "https://admission.punjab.gov.in/"} to pay & confirm your seat. -DHE, PUNJAB";
                                smsId = "1407163066199500103";
                                await smsUtility.SendSmsAsync(serviceResult.ResultData.Mobile, sms, smsId);
                            }
                            else if (serviceResult.ResultData.PaymentMode == "Both")
                            {
                                str.AppendLine("<p>Dear " + serviceResult.ResultData.StudentName + ",</p>");
                                str.AppendLine("</br>");
                                str.AppendLine("</br>");
                                str.AppendLine("<p><b>Congratulations.</b></p>");
                                str.AppendLine("<p><b>You have been offered a Seat in </b> " + serviceResult.ResultData.CollageName + " for " + serviceResult.ResultData.CourseName + "</p>");
                                str.AppendLine("<p><b>Kindly login on </b> <a href='https://admission.punjab.gov.in/'>https://admission.punjab.gov.in/</a> to pay & confirm your seat OR reach to college with this otp: " + OTP + " for offline admission.</p>");
                                str.AppendLine("</br>");
                                //str.AppendLine("<p><b>Book seat on the same day of offer by college, else the seat offer will get expire by midnight.</b></p>");
                                str.AppendLine("<p><b>Book your seat offered by the college at the earliest, else the seat offer will get expire by tomorrow midnight.</b></p>");

                                str.AppendLine("</br>");
                                str.AppendLine("</br>");
                                str.AppendLine("<p>Thanks</p>");
                                str.AppendLine("<p>DHE Punjab</p>");
                                await emailUtility.SendEmailAsync(serviceResult.ResultData.EmailId, str.ToString(), "DHE, PUNJAB");

                                sms = $"Dear {serviceResult.ResultData.StudentName},Congratulations. You have been offered a Seat in { serviceResult.ResultData.CollageName} for { serviceResult.ResultData.CourseName}. Kindly login on { "https://admission.punjab.gov.in/"} to pay & confirm your seat OR reach to college with this otp: {OTP} for offline admission. -DHE, PUNJAB";
                                smsId = "1407163066185245448";
                                await smsUtility.SendSmsAsync(serviceResult.ResultData.Mobile, sms, smsId);
                            }

                            serviceResult.ResultData = null;
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
                    else if (serviceResult.ResultData.Response == 3)
                    {
                        serviceResult.Message = MessageConfig.CourseSeatFull;
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
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Send OTP ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }




        public ServiceResult VerifyOTP(VerifyOTP model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _meritModuleUGRepository.VerifyOTP(model, "UG");
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData.Result == 1)
                    {
                        serviceResult.Message = MessageConfig.OTPVerified;
                        serviceResult.Status = true;
                        serviceResult.ResultData = null;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData.Result == 2)
                    {
                        serviceResult.Message = MessageConfig.ExpiredOTP;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData.Result == 3)
                    {
                        serviceResult.Message = MessageConfig.CourseSeatFull;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.InvalidOTP;
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
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Verify OTP ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;

        }

        public async Task<ServiceResult> CollegeCourseSubjectCount(string CollegeId, string CourseId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.CollegeCourseSubjectCount(CollegeId, CourseId);
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
                _logError.WriteTextToFile("Get Category Combination ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetStudentFeeReceiptList(string RegId, int CollegeId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetStudentFeeReceiptList(RegId, CollegeId);

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

        public async Task<ServiceResult> RevokeAdmissionSeat(RevokeSeat model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.RevokeAdmissionSeat(model);
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData.Response == 1)
                    {
                        EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                        SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                        StringBuilder str = new StringBuilder();
                        string sms = "";
                        string smsId = "";
                        str.AppendLine("<p>Dear Candidate, </p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p><b>As per your/college request seat offered in </b> " + serviceResult.ResultData.CollegeName + " for " + serviceResult.ResultData.CourseName + "</p>");
                        str.AppendLine("<p><b>has been cancelled. Please contact college.</p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p>Thanks</p>");
                        str.AppendLine("<p>DHE Punjab</p>");
                        await emailUtility.SendEmailAsync(serviceResult.ResultData.Email, str.ToString(), "DHE, PUNJAB");

                        //sms = $"Dear Candidate, As per your/college request seat offered in { serviceResult.ResultData.CollegeName} for { serviceResult.ResultData.CourseName} has been cancelled. Please contact college -DHE, PUNJAB";
                        sms = $"Dear Candidate, As per your or college request seat offered in { serviceResult.ResultData.CollegeName} for { serviceResult.ResultData.CourseName} has been cancelled. Please contact college. -DHE Punjab";
                        smsId = "1407163101281638912";
                        await smsUtility.SendSmsAsync(serviceResult.ResultData.Mobile, sms, smsId);

                        serviceResult.Message = MessageConfig.SeatRevoked;
                        serviceResult.ResultData = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                    }
                    else if (serviceResult.ResultData.Response == 2)
                    {
                        serviceResult.Message = MessageConfig.NoRecord;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData.Response == 0)
                    {
                        serviceResult.Message = MessageConfig.SeatNotRevoke;
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
                _logError.WriteTextToFile("Revoke admission seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> ExportWaitingExcel(int collegeId, int CourseId, int ReservationId, int CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, string searchKeyword)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _meritModuleUGRepository.ExportWaitingExcel(collegeId, CourseId, ReservationId, CategoryId, IsBorderArea, SingleGirlChild, CancerAidsThalassemia, NRI, IsKashmiriMigrant, RuralArea, searchKeyword);

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
                _logError.WriteTextToFile("Merit excel ug: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetVacantSeatMatrixByCategory(int? CollegeId, int CourseId, int CombinationId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetVacantSeatMatrixByCategory(CollegeId, CourseId, CombinationId);
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
                _logError.WriteTextToFile("Get Vacant Seat Matrix By Category ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateBookedSeatMatrix(UpdateBookedSeatMatrix model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.UpdateBookedSeatMatrix(model);
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData.Response == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
                        serviceResult.ResultData = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                    }
                    else if (serviceResult.ResultData.Response == 2)
                    {
                        serviceResult.Message = MessageConfig.NoRecord;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData.Response == 0)
                    {
                        serviceResult.Message = MessageConfig.SeatNotRevoke;
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
                _logError.WriteTextToFile("Revoke admission seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }
        public async Task<ServiceResult> GetVacantSeatByCollege(int CollegeId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _meritModuleUGRepository.GetVacantSeatByCollege(CollegeId);

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
                _logError.WriteTextToFile("Get Vacant Seat By College : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
    }
}
