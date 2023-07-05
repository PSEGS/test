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
    [Route("api/department")]
    public class DepartmentController : Controller
    {
        public readonly IDepartmentService _departmentService;
        private readonly ILogError _logError;

        public DepartmentController(ILogError logError, IDepartmentService departmentService)
        {
            _departmentService = departmentService;
            _logError = logError;
        }

        /// <summary>
        /// Get Department
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDepartment()
        {
            try
            {
                var data = _departmentService.GetAllDepartment();
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
                _logError.WriteTextToFile("Get Department : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }


    }
}

