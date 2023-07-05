using AdmissionPortal_API.Domain.ApiModel.EmployeeRoleMapping;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/employeeRoleMapping")]
    public class EmployeeRoleMappingController : Controller
    {
        private readonly IEmployeeRoleMappingService _employeeRoleMappingService;
        private readonly ILogError _logError;

        public EmployeeRoleMappingController(IEmployeeRoleMappingService employeeRoleMappingService, ILogError logError)
        {
            _employeeRoleMappingService = employeeRoleMappingService;
            _logError = logError;
        }

        /// <summary>
        /// Create EmployeeRoleMapping
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateEmployeeRoleMapping([FromBody] AddEmployeeRoleMapping model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _employeeRoleMappingService.CreateEmployeeRoleMapping(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create Employee Role Mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update EmployeeRoleMapping
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateEmployeeRoleMapping([FromBody] UpdateEmployeeRoleMapping model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _employeeRoleMappingService.UpdateEmployeeRoleMapping(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Employee Role Mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }



        /// <summary>
        /// Get All EmployeeRoleMappings
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllEmployeeRoleMappings(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                var data = _employeeRoleMappingService.GetAllEmployeeRoleMapping(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Employee Role Mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get EmployeeRoleMapping By Id
        /// </summary>
        /// <param name="mappingId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{mappingId}")]
        public IActionResult GetEmployeeRoleMappingById(int mappingId)
        {
            try
            {
                var data = _employeeRoleMappingService.GetEmployeeRoleMappingById(mappingId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Employee Role Mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

