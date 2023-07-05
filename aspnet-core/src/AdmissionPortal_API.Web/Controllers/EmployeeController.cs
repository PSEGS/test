using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace AdmissionPortal_API.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogError _logError;
        public EmployeeController(IEmployeeService employeeService, ILogError logError)
        {
            _employeeService = employeeService;
            _logError = logError;
        }

        /// <summary>
        /// Create Employee 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        public IActionResult CreateEmployee([FromBody] AddEmployee model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = _employeeService.CreateEmployee(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create Employee : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update Employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateEmployee([FromBody] UpdateEmployee model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _employeeService.UpdateEmployee(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Employee : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete Employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{EmployeeID}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        public IActionResult DeleteEmployee(int EmployeeID)
        {
            try
            {
                var data = _employeeService.DeleteEmployee(EmployeeID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Employee : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get All Employees
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        public IActionResult GetAllEmployees(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                var data = _employeeService.GetAllEmployee(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Employee : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get Employee By Id
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("{EmployeeID}")]
        public IActionResult GetEmployeeById(int EmployeeID)
        {
            try
            {
                var data = _employeeService.GetEmployeeById(EmployeeID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Employee By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }


        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPassword model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ServiceResult serviceResult = _employeeService.ForgotPassword(model.Email);
                    return Ok(serviceResult);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Forgot Password : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword([Required] string OTP, [Required] string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult serviceResult = _employeeService.ResetPassword(OTP, Password);
                    return Ok(serviceResult);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Reset Password : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Employee Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EmployeeLogin")]
        public IActionResult EmployeeLogin([FromBody] EmployeeLogin model)
        {
            try
            {
                ServiceResult data = _employeeService.EmployeeLogin(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Employee Login : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetEmployementType")]
        public IActionResult GetEmployementType()
        {
            try
            {
                var data = _employeeService.GetEmployementType();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Employement Type : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

