using AdmissionPortal_API.Domain.ApiModel.PGCollegeCourseMapping;
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
    [Route("api/pgcourse")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class PgCollegeCourseMappingController : Controller
    {
        private readonly IPgCollegeCourseMappingService _pgCollegeCourseMappingService;
        private readonly ILogError _logError;
        public PgCollegeCourseMappingController(IPgCollegeCourseMappingService pgCollegeCourseMappingService, ILogError logError)
        {
            _pgCollegeCourseMappingService = pgCollegeCourseMappingService;
            _logError = logError;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMappingAsync([FromBody] PgCollegeCourse model)
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
                    ServiceResult data = await _pgCollegeCourseMappingService.CreateMappingAsync(model);
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
                    ServiceResult data = await _pgCollegeCourseMappingService.getCoursesByCollegeId(collegeId, universityId,Type);
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
                    ServiceResult data = await _pgCollegeCourseMappingService.getMappedCoursesByCollegeId(collegeId);
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
                data = await _pgCollegeCourseMappingService.LockUnlockCoursesByCollegeID(collegeID, status, userid);
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
                _logError.WriteTextToFile("College PG Courses lock unlock : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        //[HttpGet]
        //[Route("getCombinationCoursesByCollegeId/{collegeId}")]
        //public async Task<IActionResult> getCombinationCoursesByCollegeId(Int32 collegeId)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var identity = HttpContext.User.Identity as ClaimsIdentity;
        //            ServiceResult data = await _pgCollegeCourseMappingService.getCombinationCoursesByCollegeId(collegeId);
        //            if (data.StatusCode == 200)
        //            {
        //                return await Task.Run(() => Ok(data));
        //            }
        //            else
        //            {
        //                return await Task.Run(() => BadRequest(data));
        //            }
        //        }
        //        else
        //        {
        //            return await Task.Run(() => BadRequest(ModelState));

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logError.WriteTextToFile("get collage course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
        //        return await Task.Run(() => BadRequest(ex.Message));

        //    }
        //}


        [HttpGet]
        [Route("GetPgMappedCourseByCollege/{collegeId}")]
        public async Task<IActionResult> GetPgMappedCourseByCollege(Int32 collegeId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    ServiceResult data = await _pgCollegeCourseMappingService.GetPgMappedCourseByCollege(collegeId);
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
    }
}
