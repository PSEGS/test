using AdmissionPortal_API.Domain.ApiModel.UniversityCourse;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/universitycourses")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UniversityCoursesController : ControllerBase
    {
        private readonly IUniversityCourseService _uniService;
        private readonly ILogError _logError;
        public UniversityCoursesController(IUniversityCourseService uniService, ILogError logError)
        {
            _uniService = uniService;
            _logError = logError;
        }
        [HttpPost]
        public async Task<IActionResult> AddUniversityCourse([FromBody] UniversityCourse model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.CreatedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.CreatedBy = 1;

                    }
                    ServiceResult data = await _uniService.AddAsync(model);
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
                _logError.WriteTextToFile("Create uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("getCoursesByUniversityId/{universityId?}")]
        public async Task<IActionResult> getCoursesByUniversityId(Int32? universityId = null,Int32? Type=null)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                ServiceResult data = await _uniService.getCoursesByUniversityId(universityId,Type);
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
                _logError.WriteTextToFile("get uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("getMappedCoursesByUniversityId/{universityId}")]
        public async Task<IActionResult> getMappedCoursesByUniversityId(Int32 universityId,Int32 ? Type)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    ServiceResult data = await _uniService.getMappedCoursesByUniversityId(universityId,Type);
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
                _logError.WriteTextToFile("get uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("getCombinationsCoursesByUniversityId/{universityId}")]
        public async Task<IActionResult> getCombinationsCoursesByUniversityId(Int32 universityId, Int32? Type)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    ServiceResult data = await _uniService.getCombinationsCoursesByUniversityId(universityId, Type);
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
                _logError.WriteTextToFile("get uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }
    }
}
