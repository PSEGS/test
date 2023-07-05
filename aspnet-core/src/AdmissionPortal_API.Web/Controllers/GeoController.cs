using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/geo")]
    [ApiController]
    public class GeoController : ControllerBase
    {
        public readonly IStateService _stateService;
        public readonly IDistrictService _districtService;
        public readonly IGeoService _geoService;
        private readonly ILogError _logError;
        public GeoController(ILogError logError, IDistrictService districtService, IStateService stateService, IGeoService geoService)
        {
            _stateService = stateService;
            _districtService = districtService;
            _geoService = geoService;
            _logError = logError;
        }
        /// <summary>
        /// Get State
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllState")]
        public async Task<IActionResult> GetState()
        {
            try
            {
                var data = _stateService.GetAllState();
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get State : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }
        /// <summary>
        /// Get Disitrict By State Id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDistrictByState/{stateId}")]
        public async Task<IActionResult> GetDistrictByStateId(int stateId)
        {
            try
            {
                if (stateId == 0)
                {
                    return await Task.Run(() => BadRequest("Invalid State"));
                }
                else
                {
                    var data = _districtService.GetDistrictByStateId(stateId);
                    if (data.StatusCode == 200)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else if (data.StatusCode == 404)
                    {
                        return await Task.Run(() => NotFound(data));
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(data));
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get District By State Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }
        /// <summary>
        /// Get Tehsil By Disitrict
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTehsilByDistrict/{districtId}")]
        public async Task<IActionResult> GetTehsilByDistrictId(string districtId)
        {
            try
            {
                if (districtId == "0" || districtId == null)
                {
                    return await Task.Run(() => BadRequest("Invalid District"));
                }
                else
                {
                    var data = await _geoService.GetTehsilByDistrictId(districtId);
                    if (data.StatusCode == 200)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else if (data.StatusCode == 404)
                    {
                        return await Task.Run(() => NotFound(data));
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(data));
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Tehsil By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }
        /// <summary>
        /// Get Block By district Id
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetBlockByDistrict/{districtId}")]
        public async Task<IActionResult> GetBlockByDistrictId(string districtId)
        {
            try
            {
                if (districtId == "0" || districtId == null)
                {
                    return await Task.Run(() => BadRequest("Invalid District"));
                }
                else
                {
                    var data =await _geoService.GetBlockByDistrictId(districtId);
                    if (data.StatusCode == 200)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else if (data.StatusCode == 404)
                    {
                        return await Task.Run(() => NotFound(data));
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(data));
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Tehsil By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }
        /// <summary>
        /// Get Village By block Id
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVillageByBlock/{blockId}")]
        public async Task<IActionResult> GetVillageByBlockId(string blockId)
        {
            try
            {
                if (blockId == "0" || blockId == null)
                {
                    return await Task.Run(() => BadRequest("Invalid block"));
                }
                else
                {
                    var data =await _geoService.GetVillageByBlockId(blockId);
                    if (data.StatusCode == 200)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else if (data.StatusCode == 404)
                    {
                        return await Task.Run(() => NotFound(data));
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(data));
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Village By block Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }
    }
}
