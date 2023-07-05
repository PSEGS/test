using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
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
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        private readonly IBlobService _blobService;
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IConfiguration configuration,ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _configuration = configuration;
            _blobService = blobService;
        }
        public ServiceResult CreateEmployee(AddEmployee model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<EmployeeMaster>(model);
                var _result = _employeeRepository.AddAsync(obj);

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
                _logError.WriteTextToFile("Create Employee : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public ServiceResult DeleteEmployee(int EmployeeID)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _employeeRepository.DeleteAsync(EmployeeID);
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
                _logError.WriteTextToFile("Delete Employee : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public ServiceResult GetAllEmployee(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _employeeRepository.GetAllEmployee(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);

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
                _logError.WriteTextToFile("Get All Employee : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public ServiceResult GetEmployeeById(int EmployeeID)
        {
            ServiceResult serviceResult = new ServiceResult();
            string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
            string containerName = _configuration["BlobContainers:ProfileImage"];
            try
            {
                var _result = _employeeRepository.GetEmployeeById(EmployeeID);

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
                _logError.WriteTextToFile("Get Employee By Id : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public ServiceResult UpdateEmployee(UpdateEmployee model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<EmployeeMaster>(model);
                var _result = _employeeRepository.UpdateAsync(obj);

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
                _logError.WriteTextToFile("Update Employee : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public ServiceResult ForgotPassword(string emailId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                var result = _employeeRepository.ForgotPassword(emailId);

                if (result != null)
                {
                    serviceResult.ResultData = result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = MessageConfig.Success;
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
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
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
                
                var _result = _employeeRepository.ResetPassword(OTP, password);

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
                _logError.WriteTextToFile("Reset Password : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;

        }


        public ServiceResult EmployeeLogin(EmployeeLogin model)
        {

            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string blobAccessKey = _configuration.GetConnectionString("ProfileBlobStorageAccessKey");
                string containerName = _configuration["BlobContainers:ProfileImage"];
                var obj = _mapper.Map<EmployeeLoginMaster>(model);
                var result = _employeeRepository.GetEmployeeLogin(obj);
                if (result.Result.EmployeeID == 0)
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
                    result.Result.Token=JWTBearerAuthentication.GenerateJSONWebToken(model.Email, Convert.ToString(result.Result.EmployeeID), nameof(UserType.Employee), string.Empty, string.Empty, string.Empty, string.Empty, _configuration);
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Employee Login : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public ServiceResult GetEmployementType()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _employeeRepository.GetEmployementType();

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
                _logError.WriteTextToFile("Get Employement Type : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

    }
}

