using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
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
    [Route("api/course")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class CollegeCourseMappingController : Controller
    {
        private readonly ICollegeCourseMappingService _collegeService;
        private readonly ILogError _logError;
        public CollegeCourseMappingController(ICollegeCourseMappingService collegeService, ILogError logError)
        {
            _collegeService = collegeService;
            _logError = logError;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMappingAsync([FromBody] CollegeCourse model)
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
                    ServiceResult data = await _collegeService.CreateMappingAsync(model);
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
                _logError.WriteTextToFile("Create collage mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("getCoursesByCollegeId/{collegeId}/{universityId}")]
        public async Task<IActionResult> getCoursesByCollegeId(Int32 collegeId,Int32 universityId,Int32? Type)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    ServiceResult data = await _collegeService.getCoursesByCollegeId(collegeId, universityId,Type);
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
                _logError.WriteTextToFile("get collage course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getMappedCoursesByCollegeId/{collegeId}")]
        public async Task<IActionResult> getMappedCoursesByCollegeId(Int32 collegeId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    ServiceResult data = await _collegeService.getMappedCoursesByCollegeId(collegeId);
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
                _logError.WriteTextToFile("get collage course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("getCombinationCoursesByCollegeId/{collegeId}")]
        public async Task<IActionResult> getCombinationCoursesByCollegeId(Int32 collegeId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    ServiceResult data = await _collegeService.getCombinationCoursesByCollegeId(collegeId);
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
                _logError.WriteTextToFile("get collage course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpPut]
        [Route("lockUnlockCoursesByCollegeID/{collegeID}/{status}")]
        public async Task<IActionResult> LockUnlockCoursesByCollegeID(Int32 collegeID, int status)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int userid = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    userid = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                else
                {
                    userid = 1;
                }
                data = await _collegeService.LockUnlockCoursesByCollegeID(collegeID, status, userid);
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
                _logError.WriteTextToFile("College Courses lock unlock : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
