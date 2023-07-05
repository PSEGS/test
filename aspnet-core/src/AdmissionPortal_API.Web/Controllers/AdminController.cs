using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.Service;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/AdminLogin")]
    public class AdminController : Controller
    {
        private readonly IAdminLoginService _adminservice;
        private readonly ILogError _logError;

        public AdminController(IAdminLoginService adminservice, ILogError logError)
        {
            _adminservice = adminservice;
            _logError = logError;
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLogin model)
        {
            try
            {
                var userAgent = HttpContext.Request.Headers["User-Agent"];
                string VisitorsIPAddress = string.Empty;
                if (string.IsNullOrEmpty(VisitorsIPAddress))
                {
                    try
                    {
                        VisitorsIPAddress = CommonMethod.GetPublicIp();
                    }
                    catch (Exception ex)
                    {
                        VisitorsIPAddress = string.Empty;
                    }
                }
                model.IPAddress = VisitorsIPAddress;
                model.Browser = userAgent;

                //string ip1 = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
                //string ip2 = HttpContext.Connection.RemoteIpAddress.ToString();

                model.UserPassword = Decryption.DecodeLoginPassword(model.UserPassword);
                ServiceResult data = await _adminservice.AdminLogin(model);
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

        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userType = HttpContext.User.Claims.First(x => x.Type == "UserType").Value;
                    string userId = HttpContext.User.Claims.First(x => x.Type == "UserID").Value;
                    var data = await _adminservice.UpdatePassword(model, userType, userId);
                    if (data.StatusCode == 200)
                    {
                        return await Task.Run(() => Ok(data));
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
                _logError.WriteTextToFile("Change Password : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }


        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("ResetEmployeePassword/{Id}")]
        public async Task<IActionResult> ResetEmployeePassword( int Id)
        {
            try
            {
                var data = await _adminservice.ResetEmployeePassword(Id);
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
                _logError.WriteTextToFile("Reset Employee Password : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotEmployeePassword")]
        public async Task<IActionResult> ForgotEmployeePassword(string UserName)
        {
            try
            {
                var data = await _adminservice.ForgotPassword(UserName);
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
                _logError.WriteTextToFile("ForgotEmployeePassword : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("CancelStudentRegistration")]
        public async Task<IActionResult> CancelStudentRegistration([FromBody] CancelStudentRegistration model)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    model.UserId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _adminservice.CancelStudentRegistrationByRegId(model);
                if (data.Status == true)
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
                _logError.WriteTextToFile("Cancel Student Registration By RegId : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("viewObjections/{RegId}/{admission}/{college}")]
        [Route("viewObjections/{admission}/{college}")]
        public async Task<IActionResult> viewStudentObjections(string admission, Int32 college, string RegId = null)
        {
            try
            {
                ServiceResult data = await _adminservice.viewStudentObjections(RegId, admission, college);
                if (data.Status == true)
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
                _logError.WriteTextToFile("view Student objections : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }



        [HttpPut]       
        [Route("UploadNotification")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadNotification([FromBody] UploadNotification model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    //if (identity != null)
                    //{
                    //    IEnumerable<Claim> claims = identity.Claims;
                    //    model.UserId = identity.FindFirst("UserID").Value;
                    //}
                    ServiceResult data = await _adminservice.UploadNotification(model);
                    if (data.Status == true)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(data));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Upload Documents : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
       
        [Route("GetUploadedNotification/{status}")]
        public IActionResult GetUploadedNotification(int status)
        {
            try
            {
               
                string userType = string.Empty;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //if (identity != null)
                //{
                //    IEnumerable<Claim> claims = identity.Claims;
                //    profileId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                //    userType = identity.FindFirst("UserType").Value;
                //}
                var data = _adminservice.GetUploadedNotification(status);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Uploaded Notification : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        
        [HttpGet]
       
        [Route("GetNotificationbyPath/{blobReference}")]
        public IActionResult GetNotificationbyPath(string blobReference)
        {
            try
            {

                blobReference = Uri.UnescapeDataString(blobReference);
                var data = _adminservice.GetNotificationbyPath(blobReference);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Notification by Path : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

