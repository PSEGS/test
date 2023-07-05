using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.CourseModel;


namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/course")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class CourseController : Controller
    {
        
        private readonly ICourseService _courseService;
        private readonly ILogError _logError;
        public CourseController(ICourseService courseService, ILogError logError)
        {
            _courseService = courseService;
            _logError = logError;
        }

         
        [HttpPost]
        [Route("AddCourse")]
        public async Task<IActionResult> CreateCourseAsync([FromBody] AddCourse model)
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
                    ServiceResult data = await _courseService.CreateCourseAsync(model);
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
                _logError.WriteTextToFile("Create collage : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpPost]
        [Route("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] updateCourse model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.ModifiedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.ModifiedBy = 1;

                    }
                    ServiceResult data = await _courseService.UpdateCourse(model);
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
                _logError.WriteTextToFile("update course : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpDelete]
        [Route("{CourseId}")]
        public async Task<IActionResult>DeleteCourse(int CourseId)
        {
            try
            {
                Int32 userid = 0;
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

                ServiceResult data = await _courseService.DeleteCourse(CourseId, userid);

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
                _logError.WriteTextToFile("Delete Course : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetAllCourse")]
        public async Task<IActionResult> GetAllCollge(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _courseService.GetAllCourse( pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get ALL Course  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetCourseById/{courseId}")]
        public async Task<IActionResult>GetcourseByidAsync(int courseId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _courseService.GetCourseById(courseId);
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
                _logError.WriteTextToFile("Get Course By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllUgCourse")]
        public async Task<IActionResult> GetAllUgCourse()
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _courseService.GetAllUgCourse();
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
                _logError.WriteTextToFile("Get ALL Course  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

    }
}
