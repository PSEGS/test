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
    public class GeoService : IGeoService
    {
        private readonly IGeoRepository _geoRepository;
        private readonly ILogError _logError;
        public GeoService(IGeoRepository geoRepository, ILogError logError)
        {
            _logError = logError;
            _geoRepository = geoRepository;
        }
        public async Task<ServiceResult> GetBlockByDistrictId(string districtId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = _geoRepository.GetAllBlockByDistrict(districtId);
                if (result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result.Result;
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
                _logError.WriteTextToFile("Get Block By district Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetTehsilByDistrictId(string districtId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = _geoRepository.GetAllTehsilByDistrict(districtId);
                if (result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result.Result;
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
                _logError.WriteTextToFile("Get Tehsil By district Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetVillageByBlockId(string blockId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = _geoRepository.GetAllVillageByBlock(blockId);
                if (result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result.Result;
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
                _logError.WriteTextToFile("Get Village By block Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
    }
}
