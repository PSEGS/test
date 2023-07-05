using AdmissionPortal_API.Domain.ApiModel.CollegeGroup;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.Service;
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
    [Route("api/collegegroup")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class CollegeGroupController : Controller
    {
        private readonly ICollegeGroupService _CollegeGroupService;
        private readonly ILogError _logError;
        public CollegeGroupController(ICollegeGroupService CollegeGroupService, ILogError logError)
        { 
            _CollegeGroupService = CollegeGroupService;
            _logError = logError;
        }
        [HttpPost]
        [Route("CreateGroup")]
        public async Task<IActionResult> CreateGroup([FromBody] collegegroup model)
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
                    ServiceResult data = await _CollegeGroupService.CreateCollegeGroup(model);
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
                _logError.WriteTextToFile("Create Group Subject Mapping : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("GetGroupSubjectById/{CourseId}")]
        public async Task<IActionResult> GetGroupSubjectById(int CourseId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int CollegeId = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    CollegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                }
                else
                {
                    CollegeId = 1;

                }
                data = await _CollegeGroupService.GetGroupDetails(CollegeId, CourseId);
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
                _logError.WriteTextToFile("Get Subject  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetAllGroup/{courseId}")]

        public async Task<IActionResult> GetAllGroup(int courseId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                Int32 collegeid = 0;
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    collegeid = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                }
                else
                {
                    collegeid = 1;

                }
                data = await _CollegeGroupService.GetAllGroup(courseId,collegeid, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get ALL College  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetGroupDetailsByGroupId/{GroupId}/{CourseId}")]

        public async Task<IActionResult> GetGroupDetailsByGroupId(int GroupId, int CourseId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                Int32 collegeid = 0;
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    collegeid = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                }
                else
                {
                    collegeid = 1;

                }
                data = await _CollegeGroupService.GetGroupDetailsByGroupId(collegeid, GroupId, CourseId);
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
                _logError.WriteTextToFile("Get ALL College  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetGroupSubjectByGroupId/{GroupId}/{CourseId}")]

        public async Task<IActionResult> GetGroupSubjectByGroupId(int GroupId, int CourseId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                Int32 collegeid = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    collegeid = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                }
                else
                {
                    collegeid = 1;

                }
                data = await _CollegeGroupService.GetGroupDetailsByGroupIdEdit(collegeid, GroupId,CourseId);
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
                _logError.WriteTextToFile("Get ALL group subject list  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllGroupCombinedWithSubject/{collegeId}/{courseId}")]
        public async Task<IActionResult> GetAllGroupCombined(int collegeId, int courseId)
        {
            try
            {
                Int32 studentID = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentID = Convert.ToInt32(identity.FindFirst("UserID").Value);

                }
                else
                {
                    studentID = 0;

                }
                ServiceResult data = new ServiceResult();
                data = await _CollegeGroupService.GetAllGroupCombined(courseId, collegeId,studentID);
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
                _logError.WriteTextToFile("Get ALL College group combined subjects : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("lockUnlockGroupSubjectByCollegeID/{collegeID}/{status}")]
        public async Task<IActionResult> lockUnlockGroupSubjectByCollegeID(Int32 collegeID, int status)
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
                data = await _CollegeGroupService.lockUnlockGroupSubjectByCollegeID(collegeID, status, userid);
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
        [HttpDelete]
        [Route("DeleteSubjectGroup/{CourseId}/{GroupId}")]
        public async Task<IActionResult> DeleteSubjectGroup(Int32 CourseId, int GroupId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int collegeId = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    collegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);
                }
                else
                {
                    collegeId = 1;
                }
                data = await _CollegeGroupService.DeleteSubjectGroup(collegeId, CourseId, GroupId);
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
