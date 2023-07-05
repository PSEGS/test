using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/Stakeholder")]
    public class StackholderController : Controller
    {
        public readonly IStackholder _stackholderService;
        private readonly ILogError _logError;
        public StackholderController(IStackholder stackholder, ILogError logError)
        {
            _stackholderService = stackholder;
            _logError = logError;
        }
        [HttpGet]
        public async Task<IActionResult> GetStackholder()
        {
            try
            {
                ServiceResult data = await _stackholderService.GetAllStackholder();
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
                _logError.WriteTextToFile("Admin Login : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

    }
}
