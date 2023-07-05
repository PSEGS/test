using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/dashboardugpg")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardUgPgController : ControllerBase
    {
        private readonly IDashboardUgPgService _dasboardService;
        private readonly ILogError _logError;
        public DashboardUgPgController(IDashboardUgPgService dasboardService, ILogError logError)
        {
            _dasboardService = dasboardService;
            _logError = logError;
        }
        [HttpGet]
        [Route("GetDashboardUg")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDashboardUg()
        {
            try
            {
                ServiceResult data = new ServiceResult();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                string loginType = "";
                string UserID = "";
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    if (claims.Count() > 0)
                    {
                        loginType = Convert.ToString(identity.FindFirst("loginType").Value);
                        UserID = Convert.ToString(identity.FindFirst("UserID").Value);
                    }
                }
                data = await _dasboardService.GetDashboardUg(loginType,UserID);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get dashboard ug : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetDashboardChartUg")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDashboardChartUg()
        {
            try
            {
                ServiceResult data = new ServiceResult();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                string loginType = "";
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    if (claims.Count() > 0)
                    {
                        loginType = Convert.ToString(identity.FindFirst("loginType").Value);
                    }
                }

                data = await _dasboardService.GetDashboardChartUg(loginType);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get dashboard chart ug : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetDashboardPg")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDashboardPg()
        {
            try
            {
                ServiceResult data = new ServiceResult();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                string loginType = "";
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    if (claims.Count() > 0)
                    {
                        loginType = Convert.ToString(identity.FindFirst("loginType").Value);
                    }
                }
                data = await _dasboardService.GetDashboardPg(loginType);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get dashboard pg : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetDashboardChartPg")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDashboardChartPg()
        {
            try
            {
                ServiceResult data = new ServiceResult();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                string loginType = "";
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    if (claims.Count() > 0)
                    {
                        loginType = Convert.ToString(identity.FindFirst("loginType").Value);
                    }
                }

                data = await _dasboardService.GetDashboardChartPg(loginType);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get dashboard chart pg : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
