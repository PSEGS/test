using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.Objection;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using AdmissionPortal_API.Utility.Notification;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.VerificationSectionEnum;

namespace AdmissionPortal_API.Service.Service
{
    public class VerificationService : IVerificationService
    {
        private readonly IVerificationRepository _verificationRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public VerificationService(IVerificationRepository verificationRepository, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _verificationRepository = verificationRepository;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }

        public async Task<ServiceResult> GetVerifiedStudentByRegIdCollege(string RegId, int CollegeId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _verificationRepository.GetVerifiedStudentByRegIdCollege(RegId, CollegeId);

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
                _logError.WriteTextToFile("Get Verified Student by RegId and College : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetStudentsByCollege(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _verificationRepository.GetStudentsByCollege(collegeId, verificationStatus, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);

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
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> VerifyStudentWithSection(VerifyStudentWithSection model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                var obj = _mapper.Map<VerifyStudentWithSection>(model);
                var _result = _verificationRepository.VerifyStudentWithSection(obj);

                if (_result != null)
                {
                    serviceResult = _result.Result;
                    if (serviceResult.StatusCode == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
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
                _logError.WriteTextToFile("Verify student with section : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> RevokeStudentVerification(CancelStudentRegistration model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //var obj = _mapper.Map<VerifyStudentWithSection>(model);
                var _result = _verificationRepository.RevokeStudentVerification(model);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.StatusUpdated;
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
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetStudentByRegId(string RegId, Int32 type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _verificationRepository.GetStudentByRegId(RegId, type);
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        //bunty comment this for test producation data
                        //switch (type)
                        //{
                        //    case (int)VerificationSectionType.PersonalDetails:
                        //        if (serviceResult.ResultData.PhotographRef != null)
                        //        {
                        //            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.PhotographRef);
                        //            serviceResult.ResultData.Photograph = photo.Result.ResultData.ToString();
                        //        }
                        //        if (serviceResult.ResultData.SignatureRef != null)
                        //        {
                        //            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.SignatureRef);
                        //            serviceResult.ResultData.Signature = photo.Result.ResultData.ToString();
                        //        }
                        //        if (serviceResult.ResultData.MetricCertificateRef != null)
                        //        {
                        //            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.MetricCertificateRef);
                        //            serviceResult.ResultData.MetricCertificate = photo.Result.ResultData.ToString();
                        //        }
                        //        break;
                        //    case (int)(VerificationSectionType.AcademicDetails):
                        //        if (serviceResult.ResultData.AcademicDetails.Certificate != null)
                        //        {
                        //            var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.AcademicDetails.Certificate);
                        //            serviceResult.ResultData.AcademicDetails.Certificate = photo.Result.ResultData.ToString();
                        //        }
                        //        break;
                        //    case (int)(VerificationSectionType.Weightages):
                        //        for (int i = 0; i < serviceResult.ResultData.Count; i++)
                        //        {
                        //            if (serviceResult.ResultData[i].DocmentBlobReference != null)
                        //            {
                        //                var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData[i].DocmentBlobReference);
                        //                serviceResult.ResultData[i].Image = photo.Result.ResultData.ToString();
                        //            }
                        //        }
                        //        break;
                        //    case (int)(VerificationSectionType.ReservationDetails):
                        //        for (int i = 0; i < serviceResult.ResultData.Count; i++)
                        //        {
                        //            if (serviceResult.ResultData[i].ImageRef != null)
                        //            {
                        //                var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData[i].ImageRef);
                        //                serviceResult.ResultData[i].Image = photo.Result.ResultData.ToString();
                        //            }

                        //        }
                        //        break;

                        //    default:
                        //        break;

                        //}
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
                _logError.WriteTextToFile("Get Student by RegId : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> StudentCourseEligible(StudentChoiceofCourseEligible model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //var obj = _mapper.Map<VerifyStudentWithSection>(model);
                var _result = _verificationRepository.StudentCourseEligible(model);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.StatusUpdated;
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
            }
            return serviceResult;
        }

        public async Task<ServiceResult> FinalSubmit(FinalVerificationModel model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //var obj = _mapper.Map<VerifyStudentWithSection>(model);
                var _result = _verificationRepository.FinalVerification(model);
                SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData.verificationReturnModel.StatusCode == 1)
                    {
                        serviceResult.Message = MessageConfig.StatusUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData.verificationReturnModel.StatusCode == 2)
                    {
                        string Remarks = string.Empty;
                        if (serviceResult.ResultData.objectremarks.Count > 0)
                        {
                            for (int i = 0; i < serviceResult.ResultData.objectremarks.Count; i++)
                            {
                                Remarks = Remarks + "<p><b>" + Convert.ToInt32(i + 1) + ". " + serviceResult.ResultData.objectremarks[i].VerifierRemarks + ".</b></p></br>";
                            }
                        }
                        StringBuilder str = new StringBuilder();
                        str.AppendLine("<p>Dear " + serviceResult.ResultData.verificationReturnModel.name + ",</p>");
                        str.AppendLine("</br>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p><b>Following Objection(s) are raised against your Registration Id </b>" + model.RegistrationNumber + "</p>");
                        str.AppendLine("<p><b>Remarks:</b></p>");

                        str.AppendLine(Remarks);
                        str.AppendLine("<p><b>Kindly login on https://admission.punjab.gov.in/  immidiately to view/remove objection(s)</b></p>");
                        str.AppendLine("</br>");
                        str.AppendLine("<p>DHE, Punjab</p>");
                        await emailUtility.SendEmailAsync(serviceResult.ResultData.verificationReturnModel.email, str.ToString(), "Digital Punjab Student Login Detail");

                        await smsUtility.SendSmsAsync(serviceResult.ResultData.verificationReturnModel.Mobile, "Dear " + serviceResult.ResultData.verificationReturnModel.name + " Objection(s) raised against your Registration Id " + model.RegistrationNumber + ",. Kindly login on https://admission.punjab.gov.in/ to remove objection(s). -DHE, Punjab", "1407162633877000764");
                        serviceResult.Message = MessageConfig.StatusUpdated;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData.verificationReturnModel.StatusCode == 0)
                    {

                        serviceResult.Message = MessageConfig.NoRecord;
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
            }
            return serviceResult;
        }

        public async Task<ServiceResult> ExportExcelStudentsByCollege(int collegeId, int? verificationStatus)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _verificationRepository.ExportExcelStudentsByCollege(collegeId, verificationStatus);

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
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> GetStudentsByCollegeBCOM(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _verificationRepository.GetStudentsByCollegeBCOM(collegeId, verificationStatus, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);

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
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> GetStudentByRegIdBCOM(string RegId, Int32 type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _verificationRepository.GetStudentByRegIdBCOM(RegId, type);
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        switch (type)
                        {
                            case (int)VerificationSectionType.PersonalDetails:
                                if (serviceResult.ResultData.PhotographRef != null)
                                {
                                    var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.PhotographRef);
                                    serviceResult.ResultData.Photograph = photo.Result.ResultData.ToString();
                                }
                                if (serviceResult.ResultData.SignatureRef != null)
                                {
                                    var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.SignatureRef);
                                    serviceResult.ResultData.Signature = photo.Result.ResultData.ToString();
                                }
                                if (serviceResult.ResultData.MetricCertificateRef != null)
                                {
                                    var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.MetricCertificateRef);
                                    serviceResult.ResultData.MetricCertificate = photo.Result.ResultData.ToString();
                                }
                                break;
                            case (int)(VerificationSectionType.AcademicDetails):
                                if (serviceResult.ResultData.AcademicDetails.Certificate != null)
                                {
                                    var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.AcademicDetails.Certificate);
                                    serviceResult.ResultData.AcademicDetails.Certificate = photo.Result.ResultData.ToString();
                                }
                                break;
                            case (int)(VerificationSectionType.Weightages):
                                for (int i = 0; i < serviceResult.ResultData.Count; i++)
                                {
                                    if (serviceResult.ResultData[i].DocmentBlobReference != null)
                                    {
                                        var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData[i].DocmentBlobReference);
                                        serviceResult.ResultData[i].Image = photo.Result.ResultData.ToString();
                                    }
                                }
                                break;
                            case (int)(VerificationSectionType.ReservationDetails):
                                for (int i = 0; i < serviceResult.ResultData.Count; i++)
                                {
                                    if (serviceResult.ResultData[i].ImageRef != null)
                                    {
                                        var photo = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData[i].ImageRef);
                                        serviceResult.ResultData[i].Image = photo.Result.ResultData.ToString();
                                    }

                                }
                                break;

                            default:
                                break;

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
                _logError.WriteTextToFile("Get Student by RegId : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> SAVEBCOMStudentWeightage(BCOMStudentWeightage model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //var obj = _mapper.Map<VerifyStudentWithSection>(model);
                var _result = _verificationRepository.SAVEBCOMStudentWeightage(model);
                //SMSUtility smsUtility = new SMSUtility(_configuration, _logError);
                //EmailUtility emailUtility = new EmailUtility(_configuration, _logError);
                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.StatusUpdated;
                    serviceResult.Status = true;
                    serviceResult.ResultData = null;

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
            }
            return serviceResult;
        }

        public async Task<ServiceResult> ExportExcelStudentsByCollegeBCOM(int collegeId, int? verificationStatus)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _verificationRepository.ExportExcelStudentsByCollegeBCOM(collegeId, verificationStatus);

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
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> GetStudentCombinationByCCId(Int32 CollegeID, Int32 CourseId, string RegId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _verificationRepository.StudentCombinationDetails(CollegeID, CourseId, RegId);

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
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
    }
}
