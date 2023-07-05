using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
using AdmissionPortal_API.Domain.ApiModel.Profile;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AdmissionPortal_API.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/profile")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ILogError _logError;
        public ProfileController(IProfileService profileService, ILogError logError)
        {
            _profileService = profileService;
            _logError = logError;
        }

        [HttpGet]
        [Route("GetEmployeeProfile")]
        public IActionResult GetEmployeeProfile()
        {
            try
            {
                int profileId = 0;
                string userType = string.Empty;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    profileId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    userType = identity.FindFirst("UserType").Value;
                }
                var data = _profileService.GetEmployeeProfile(profileId, userType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Employee Profile : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCitizenProfile")]
        public IActionResult GetCitizenProfile()
        {
            try
            {
                int profileId = 0;
                string userType = string.Empty;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    profileId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    userType = identity.FindFirst("UserType").Value;
                }
                var data = _profileService.GetCitizenProfile(profileId, userType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Citizen Profile : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("UpdateEmployeeProfile")]
        public IActionResult UpdateEmployeeProfile([FromBody] UpdateEmployeeProfile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.Id = Convert.ToInt32(identity.FindFirst("UserID").Value);
                        model.UserType = identity.FindFirst("UserType").Value;
                    }
                    var data = _profileService.UpdateEmployeeProfile(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Employee Profile : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("UpdateCitizenProfile")]
        public IActionResult UpdateCitizenProfile([FromBody] UpdateCitizenProfile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.Id = Convert.ToInt32(identity.FindFirst("UserID").Value);
                        model.UserType = identity.FindFirst("UserType").Value;
                    }
                    var data = _profileService.UpdateCitizenProfile(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Citizen Profile : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("UploadProfileImage")]
        public IActionResult UploadProfileImage([FromBody] ProfileImage model)
        { 
            try
            {
                if (ModelState.IsValid)
                {
                    int profileId = 0;
                    string userType = string.Empty;
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        profileId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                        userType = identity.FindFirst("UserType").Value;
                    }
                    var data = _profileService.UploadProfileImage(profileId, userType, model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Profile Image : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("UpdateProfileImageById")]
        public IActionResult UpdateProfileImageById([FromBody] UpdateProfileImage model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _profileService.UpdateProfileImageById(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Profile Image By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

