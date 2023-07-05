using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/district")]
    public class DistrictController : Controller
    {
        public readonly IDistrictService _districtService;
        private readonly ILogError _logError;
        public DistrictController(ILogError logError, IDistrictService districtService)
        {
            _districtService = districtService;
            _logError = logError;
        }
        /// <summary>
        /// Get Disitrict By State Id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{stateId}")]
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
    }
}

