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
    [Route("api/state")]
    public class StateController : Controller
    {
        public readonly IStateService _stateService;
        private readonly ILogError _logError;

        public StateController(ILogError logError, IStateService stateService)
        {
            _stateService = stateService;
            _logError = logError;
        }

        /// <summary>
        /// Get State
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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


    }
}

