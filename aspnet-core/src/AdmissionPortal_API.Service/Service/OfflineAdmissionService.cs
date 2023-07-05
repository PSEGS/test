using AdmissionPortal_API.Domain.ApiModel.OfflineAdmisson;
using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AdmissionPortal_API.Utility.MessageConfig;

namespace AdmissionPortal_API.Service.Service
{
    public class OfflineAdmissionService : ServiceInterface.IOfflineAdmission
    {
        private readonly Data.RepositoryInterface.IOfflineAdmission _OfflineAdmissonRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public OfflineAdmissionService(Data.RepositoryInterface.IOfflineAdmission OfflineAdmissonRepository, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _OfflineAdmissonRepository = OfflineAdmissonRepository;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }
        public async Task<ServiceResult> AddOfflineAdmisson(OfflineAdmissionModel model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<OfflineAdmissionModel>(model);
                 
                var _result = _OfflineAdmissonRepository.AddAdmission(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK); 
                    }
                    else if (serviceResult.ResultData == 2 || serviceResult.ResultData == 5)
                    {
                        serviceResult.Message = MessageConfig.AlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 3)
                    {
                        serviceResult.Message = MessageConfig.MobileAlreadyExists;
                        serviceResult.ResultData = 3;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 4)
                    {
                        serviceResult.Message = MessageConfig.EmailAlreadyExists;
                        serviceResult.ResultData = 4;
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
                _logError.WriteTextToFile("Offline Admission Service : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
    }
}
