using AdmissionPortal_API.Domain.ApiModel.RoleNavigation;
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
    [Route("api/roleNavigation")]
    public class RoleNavigationController : Controller
    {
        private readonly IRoleNavigationService _RoleNavigationService;
        private readonly ILogError _logError;

        public RoleNavigationController(IRoleNavigationService RoleNavigationService, ILogError logError)
        {
            _RoleNavigationService = RoleNavigationService;
            _logError = logError;
        }

        /// <summary>
        /// Create RoleNavigation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRoleNavigation([FromBody] AddRoleNavigation model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _RoleNavigationService.CreateRoleNavigation(model);
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
                else
                {
                    return await Task.Run(() => BadRequest(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create Role Navigation : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }

        /// <summary>
        /// Update RoleNavigation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateRoleNavigation([FromBody] UpdateRoleNavigation model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _RoleNavigationService.UpdateRoleNavigation(model);
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
                else
                {
                    return await Task.Run(() => BadRequest(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Role Navigation : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }



        /// <summary>
        /// Get All RoleNavigations
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRoleNavigations(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                var data = _RoleNavigationService.GetAllRoleNavigation(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get All Role Navigations : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }

        /// <summary>
        /// Get RoleNavigation By Id
        /// </summary>
        /// <param name="mappingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{mappingId}")]
        public async Task<IActionResult> GetRoleNavigationById(int mappingId)
        {
            try
            {
                var data = _RoleNavigationService.GetRoleNavigationById(mappingId);
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
                _logError.WriteTextToFile("Get Role Navigation By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }
    }
}



