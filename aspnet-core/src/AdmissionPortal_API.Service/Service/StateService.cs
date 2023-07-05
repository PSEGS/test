using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.Service
{

    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly ILogError _logError;
        public StateService(IStateRepository stateRepository,ILogError logError)
        {
            _logError = logError;
            _stateRepository = stateRepository;
        }
        /// <summary>
        /// Get All State
        /// </summary>
        /// <returns></returns>
        public ServiceResult GetAllState()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _stateRepository.GetAllAsync();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
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
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get All State : " , ex.Message, ex.HResult, ex.StackTrace); 
            }
            return serviceResult;
        }

        
    }
}

