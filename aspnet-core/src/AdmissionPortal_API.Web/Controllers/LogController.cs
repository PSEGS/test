using AdmissionPortal_API.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{

    [Route("api/log")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class LogController : Controller
    {
        private readonly ILogService _logService;
        private readonly ILogger _logger;

        public LogController(ILogService logService, ILoggerFactory logFactory)
        {
            _logService = logService;
            _logger = logFactory.CreateLogger<LogController>();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLogs(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder, DateTime? startDate =null, DateTime? endDate=null,string type=null)
        {
            try
            {
                var data = _logService.GetAllLog(pageNumber, pageSize, searchKeyword, sortBy, sortOrder,startDate,endDate,type);
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
                _logger.Log(0, ex.Message);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }
    }
}

