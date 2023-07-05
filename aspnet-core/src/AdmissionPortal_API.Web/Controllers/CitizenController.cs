using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdmissionPortal_API.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/citizen")]
    public class CitizenController : Controller
    {
        private readonly ICitizenService _citizenService;
        private readonly ILogError _logError;
        public CitizenController(ICitizenService citizenService, ILogError logError)
        {
            _citizenService = citizenService;
            _logError = logError;
        }

        /// <summary>
        /// Create Citizen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CreateCitizen([FromBody] AddCitizen model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = _citizenService.CreateCitizen(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create Citizen : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update Citizen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateCitizen([FromBody] UpdateCitizen model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _citizenService.UpdateCitizen(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Citizen : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete Citizen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{CitizenID}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteCitizen(int CitizenID)
        {
            try
            {
                var data = _citizenService.DeleteCitizen(CitizenID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Citizen : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get All Citizens
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllCitizen(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                var data = _citizenService.GetAllCitizen(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Citizen : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get Citizen By Id
        /// </summary>
        /// <param name="CitizenID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{CitizenID}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCitizenById(int CitizenID)
        {
            try
            {
                var data = _citizenService.GetCitizenById(CitizenID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Citizen By Id : ", ex.Message, ex.HResult, ex.StackTrace);
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
                    ServiceResult serviceResult = _citizenService.ForgotPassword(model.Email);
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
                ServiceResult serviceResult = _citizenService.ResetPassword(OTP, Password);
                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Reset Password : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Citizen Login 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CitizenLogin")]
        public IActionResult CitizenLogin([FromBody] CitizenLogin model)
        {
            try
            {
                ServiceResult data = _citizenService.CitizenLogin(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Citizen Login : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

