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
    public class StackholderService : IStackholder
    {
        private readonly IStackholderRepository _stackholderRepository;
        private readonly ILogError _logError;
        public StackholderService(IStackholderRepository stackholderRepository,ILogError logError)
        {
            _stackholderRepository = stackholderRepository;
            _logError = logError;
        }

        

        async Task<ServiceResult> IStackholder.GetAllStackholder()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _stackholderRepository.GetAllAsync();

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
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get All Stackholder : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
    }
}
