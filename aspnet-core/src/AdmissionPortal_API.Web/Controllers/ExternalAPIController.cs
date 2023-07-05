using AdmissionPortal_API.Domain.ExternalAPI;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/externalAPI")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class ExternalAPIController : Controller
    {
        private readonly IExternalAPIService _éxternalService;
        private readonly ILogError _logError;
        public ExternalAPIController(IExternalAPIService éxternalapiService, ILogError logError)
        {
            _éxternalService = éxternalapiService;
            _logError = logError;

        }
        [HttpPost]
        [Route("{Type}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStudentInfoFromBoard(string Type,[FromBody] CBSESchoolDetails model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = await _éxternalService.GetStudentInfoFromBoard(Type,model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("PSEB API : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ValidateDocument")]
        public async Task<IActionResult> ValidateDocument([FromBody] DocumentValidate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Int32 StudentId = 0;
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _éxternalService.ValidateDocument(model, StudentId, "UG");
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Validate Document : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ValidateDocumentPG")]
        public async Task<IActionResult> ValidateDocumentPG([FromBody] DocumentValidate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Int32 StudentId = 0;
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _éxternalService.ValidateDocument(model, StudentId, "PG");
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Validate Document : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("StudentMarksVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> StudentMarksVerification()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = await _éxternalService.StudentMarksVerification();
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("PSEB API : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
