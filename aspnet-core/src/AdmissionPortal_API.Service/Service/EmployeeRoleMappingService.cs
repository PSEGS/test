using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.EmployeeRoleMapping;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.Service
{
    public class EmployeeRoleMappingService : IEmployeeRoleMappingService
    {
        private readonly IEmployeeRoleMappingRepository _employeeRoleMappingRepository;
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        public EmployeeRoleMappingService(IEmployeeRoleMappingRepository employeeRoleMappingRepository, IMapper mapper,ILogError logError)
        {
            _logError = logError;
            _employeeRoleMappingRepository = employeeRoleMappingRepository;
            _mapper = mapper;
        }
        public ServiceResult CreateEmployeeRoleMapping(AddEmployeeRoleMapping model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<EmployeeRoleMappingMaster>(model);
                var _result = _employeeRoleMappingRepository.AddAsync(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != 0)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
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
                _logError.WriteTextToFile("Create EmployeeRoleMapping : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }


        public ServiceResult GetAllEmployeeRoleMapping(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _employeeRoleMappingRepository.GetAllEmployeeRoleMapping(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get All EmployeeRoleMapping : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        public ServiceResult GetEmployeeRoleMappingById(int mappingId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _employeeRoleMappingRepository.GetByIdAsync(mappingId);
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
                _logError.WriteTextToFile("Get EmployeeRoleMapping By Id : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public ServiceResult UpdateEmployeeRoleMapping(UpdateEmployeeRoleMapping model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<EmployeeRoleMappingMaster>(model);
                var _result = _employeeRoleMappingRepository.UpdateAsync(obj);
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
                _logError.WriteTextToFile("Update EmployeeRoleMapping : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
    }
}

