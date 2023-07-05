using AdmissionPortal_API.Domain.ApiModel.UniversityPgCourse;
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
    [Route("api/UniversityPgCourses")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UniversityPgCoursesController : ControllerBase
    {
        private readonly IUniversityPgCourseService _universityPgCourseService;
        private readonly ILogError _logError;
        public UniversityPgCoursesController(IUniversityPgCourseService universityPgCourseService, ILogError logError)
        {
            _universityPgCourseService = universityPgCourseService;
            _logError = logError;
        }
        [HttpPost]
        public async Task<IActionResult> AddUniversityCourse([FromBody] UniversityPgCourse model)
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
                    ServiceResult data = await _universityPgCourseService.AddAsync(model);
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
                _logError.WriteTextToFile("Create university PG course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("getCoursesByUniversityId/{universityId}")]
        public async Task<IActionResult> GetCoursesByUniversityId(Int32? universityId,Int32? Type)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                ServiceResult data = await _universityPgCourseService.GetCoursesByUniversityId(universityId,Type);
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
                _logError.WriteTextToFile("get university PG course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        //[HttpGet]
        //[Route("getMappedCoursesByUniversityId/{universityId}")]
        //public async Task<IActionResult> GetMappedCoursesByUniversityId(Int32 universityId,Int32 ? Type)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var identity = HttpContext.User.Identity as ClaimsIdentity;
        //            ServiceResult data = await _universityPgCourseService.GetMappedCoursesByUniversityId(universityId,Type);
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
        //        _logError.WriteTextToFile("get university PG course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
        //        return await Task.Run(() => BadRequest(ex.Message));

        //    }
        //}
    }
}
