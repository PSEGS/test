using AdmissionPortal_API.Data.RepositoryInterface;
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
    public class LogService :ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        public LogService(ILogRepository logRepository, IMapper mapper, ILogError logError)
        {
            _logError = logError;
            _logRepository = logRepository;
            _mapper = mapper;
        }
        public ServiceResult GetAllLog(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder, DateTime? startDate, DateTime? endDate, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _logRepository.GetAllLog(pageNumber, pageSize, searchKeyword, sortBy, sortOrder,startDate,endDate,type);

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
                _logError.WriteTextToFile("Get All Log : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
    }
}

