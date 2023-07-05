using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
using AdmissionPortal_API.Domain.ApiModel.Profile;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IBlobService _blobService;
        private readonly IConfiguration _configuration;
        public ProfileService(IProfileRepository profileRepository, IMapper mapper, ILogError logError ,IConfiguration configuration,IBlobService blobService)
        {
            _logError = logError;
            _profileRepository = profileRepository;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }

        public ServiceResult GetEmployeeProfile(int profileId, string userType)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            try
            {
                var _result = _profileRepository.GetEmployeeProfile(profileId, userType);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        var result = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey,containerName,serviceResult.ResultData.ImageReference);

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
                _logError.WriteTextToFile("Get Employee Profile : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }

        public ServiceResult GetCitizenProfile(int profileId, string userType)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            try
            {
                var _result = _profileRepository.GetCitizenProfile(profileId, userType);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        var result = _blobService.FetchBlobUsingFileReferenceType(blobAccessKey,containerName,serviceResult.ResultData.ImageReference);
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
                _logError.WriteTextToFile("Get Citizen Profile : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }

        public ServiceResult UpdateEmployeeProfile(UpdateEmployeeProfile model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<EmployeeMaster>(model);
                var _result = _profileRepository.UpdateEmployeeProfile(obj);

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
                _logError.WriteTextToFile("Update Employee Profile : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }

        public ServiceResult UpdateCitizenProfile(UpdateCitizenProfile model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<CitizenMaster>(model);
                var _result = _profileRepository.UpdateCitizenProfile(obj);

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
                _logError.WriteTextToFile("Update Citizen Profile : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }

        public ServiceResult UploadProfileImage(int profileId, string userType, ProfileImage model)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.ImageCode);
            if (_blobResult.Result.StatusCode == 1)
            {
                try
                {
                    model.ImageReference = _blobResult.Result.ResultData;
                    var _result = _profileRepository.UploadProfileImage(profileId, userType, model);
                    if (_result != null)
                    {
                        serviceResult.ResultData = _result.Result;
                        if (serviceResult.ResultData == 1)
                        {
                            serviceResult.Message = MessageConfig.UploadImage;
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
                    _logError.WriteTextToFile("Upload Profile Image : " , ex.Message, ex.HResult, ex.StackTrace);
                }
            }
            else if (_blobResult.Result.StatusCode == 2)
            {
                serviceResult.Message = _blobResult.Result.Message;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            else
            {
                serviceResult.Message = _blobResult.Result.Message;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            return serviceResult;
        }

        public ServiceResult UpdateProfileImageById(UpdateProfileImage model)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            var _blobResult = _blobService.UploadBlobUsingFileReferenceType(blobAccessKey, containerName, model.ImageCode);
            if (_blobResult.Result.StatusCode == 1)
            {
                try
                {
                    model.ImageReference = _blobResult.Result.ResultData;
                    var _result = _profileRepository.UpdateProfileImageById(model);
                    if (_result != null)
                    {
                        serviceResult.ResultData = _result.Result;
                        if (serviceResult.ResultData == 1)
                        {
                            serviceResult.Message = MessageConfig.ChangeImage;
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
                    _logError.WriteTextToFile("Update Profile Image : " , ex.Message, ex.HResult, ex.StackTrace);
                }
            }
            else if (_blobResult.Result.StatusCode == 2)
            {
                serviceResult.Message = _blobResult.Result.Message;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            else
            {
                serviceResult.Message = _blobResult.Result.Message;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            return serviceResult;
        }
    }
}

