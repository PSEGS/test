using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using static AdmissionPortal_API.Domain.CommonEnam.UserTypeEnum;

namespace AdmissionPortal_API.Service.Service
{
    public class CitizenService : ICitizenService
    {
        private readonly ICitizenRepository _citizenRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public CitizenService(ICitizenRepository citizenRepository, IMapper mapper, IConfiguration configuration,ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _citizenRepository = citizenRepository;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }

        public ServiceResult CreateCitizen(AddCitizen model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<CitizenMaster>(model);
                var _result = _citizenRepository.AddAsync(obj);

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
                _logError.WriteTextToFile("Create Citizen : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public ServiceResult DeleteCitizen(int CitizenID)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _citizenRepository.DeleteAsync(CitizenID);

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
                _logError.WriteTextToFile("Delete Citizen : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public ServiceResult GetAllCitizen(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _citizenRepository.GetAllCitizen(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);

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
                _logError.WriteTextToFile("Get All Citizen : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public ServiceResult GetCitizenById(int CitizenID)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            try
            {
                var _result = _citizenRepository.GetByIdAsync(CitizenID);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        var result = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, serviceResult.ResultData.ImageReference);
                        if (result.Result.ResultData != null)
                        {
                            serviceResult.ResultData.ImageCode = result.Result.ResultData;
                        }
                        else
                        {
                            serviceResult.ResultData.ImageCode = null;
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
                _logError.WriteTextToFile("Get Citizen By Id : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public ServiceResult UpdateCitizen(UpdateCitizen model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<CitizenMaster>(model);
                var _result = _citizenRepository.UpdateAsync(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordUpdated;
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
                _logError.WriteTextToFile("Update Citizen : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public ServiceResult ForgotPassword(string emailId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = _citizenRepository.ForgotPassword(emailId);

                if (result != null)
                {
                    serviceResult.ResultData = result.Result;
                    if (serviceResult.ResultData !=null)
                    {
                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.ResultData = result.Result;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.InvalidRecord;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.InvalidRecord;
                    serviceResult.ResultData = null ;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Forgot Password : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;

        }

        public ServiceResult ResetPassword(string OTP, string password)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _citizenRepository.ResetPassword(OTP, password);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.PasswordUpdated;
                        serviceResult.ResultData = _result.Result;
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
                    serviceResult.ResultData = _result;
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
                _logError.WriteTextToFile("Reset Password : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;

        }
        public ServiceResult CitizenLogin(CitizenLogin model)
        {

            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                var obj = _mapper.Map<CitizenLoginMaster>(model);
                var result = _citizenRepository.GetCitizenLogin(obj);
                if (result.Result.CitizenID == 0)
                {
                    serviceResult.Message = MessageConfig.InvalidEmailPassword;
                    serviceResult.ResultData = result.Result;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);

                }
                else
                {
                    var data = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey, containerName, result.Result.ImageReference);
                    if (data.Result.ResultData != null)
                    {
                        result.Result.ImageCode = data.Result.ResultData;
                    }
                    else
                    {
                        result.Result.ImageCode = null;
                    }
                    result.Result.Token = JWTBearerAuthentication.GenerateJSONWebToken(model.Email, Convert.ToString(result.Result.CitizenID), nameof(UserType.Citizen),string.Empty, string.Empty, string.Empty, string.Empty, _configuration);
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Citizen Login : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }
    }

}


