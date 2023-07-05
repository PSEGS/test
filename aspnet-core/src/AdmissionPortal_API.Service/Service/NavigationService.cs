using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Navigation;
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
    public class NavigationService: INavigationService
    {
        private readonly INavigationRepository _navigationRepository;
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        public NavigationService(INavigationRepository navigationRepositor, IMapper mapper,ILogError logError)
        {
            _logError = logError;
            _navigationRepository = navigationRepositor;
            _mapper = mapper;
        }

        /// <summary>
        /// Create Role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult CreateNavigation(AddNavigation model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<NavigationMaster>(model);
                var _result = _navigationRepository.AddAsync(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData !=0)
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
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Create Navigation : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }

        /// <summary>
        /// Delete Navigation
        /// </summary>
        /// <param name="navigationId"></param>
        /// <returns></returns>
        public ServiceResult DeleteNavigation(int navigationId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _navigationRepository.DeleteAsync(navigationId);

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
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Delete Navigation : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }

        /// <summary>
        /// Get All Navigations
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public ServiceResult GetAllNavigations(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _navigationRepository.GetAllNavigations(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get All Navigation : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        /// <summary>
        /// Get Navigation By Id
        /// </summary>
        /// <param name="navigationId"></param>
        /// <returns></returns>

        public ServiceResult GetNavigationById(int navigationId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _navigationRepository.GetByIdAsync(navigationId);
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
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Navigation By Id : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return serviceResult;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult UpdateNavigation(UpdateNavigation model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<NavigationMaster>(model);
                var _result = _navigationRepository.UpdateAsync(obj);

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
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Update Workflow Process : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }
        
        
        public ServiceResult GetNavigation(int Id,string UserType,string LoginType)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _navigationRepository.GetNavigation(Id,UserType,LoginType);

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
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Navigation : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }

    }
}

