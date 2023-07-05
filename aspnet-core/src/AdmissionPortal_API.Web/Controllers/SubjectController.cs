using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/subject")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly ILogError _logError;
        public SubjectController(ISubjectService subjectService, ILogError logError)
        {
            _subjectService = subjectService;
            _logError = logError;
        }

        [HttpPost]
        [Route("AddSubject")]
        public async Task<IActionResult> CreateSubjectAsync([FromBody] SubjectMaster model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.Createdby = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.Createdby = 1;

                    }
                    ServiceResult data = await _subjectService.CreateSubject(model);
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
                _logError.WriteTextToFile("Create Subject : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpPost]
        [Route("UpdateSubject")]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubject model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.ModifyBy = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.ModifyBy = 1;

                    }
                    ServiceResult data = await _subjectService.UpdateSubject(model);
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
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update college : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{SubjectId}")]
        public async Task<IActionResult> DeleteSubjectAsync(int SubjectId)
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

                ServiceResult data = await _subjectService.DeleteSubject(SubjectId, userid);

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
                _logError.WriteTextToFile("Delete Subject : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllSubject")]

        public async Task<IActionResult> GetAllSubject(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _subjectService.GetAllSubject(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get ALL Subject  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{subjectId}")]
        public async Task<IActionResult> GetSubjectbyidAsync(int subjectId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _subjectService.GetSubjectById(subjectId);
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
                _logError.WriteTextToFile("Get subject By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetSubjectbyCourseAndUniversity/{CourseId}/{UniversityId}")]
        public async Task<IActionResult> GetSubjectbyCourseAndUniversityidAsync(int CourseId, int UniversityId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                List<coursesubject> coursesubject = new List<coursesubject>();
                data = await _subjectService.GetSubjectbyCourseAndUniversityid(CourseId, UniversityId);
                DataTable ds = new DataTable();
                coursesubject = data.ResultData;
                //var resultSubjectList = coursesubject.AsEnumerable().GroupBy(x => x.SubjectType).ToList();
                var resultSubjectList = coursesubject.ToList();
                var dictResult = new Dictionary<string, object>();
                //foreach (var item in resultSubjectList)
                //{
                //    dictResult.Add(item.Key, item);
                //}
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(dictResult));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get subject By course and universityId : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSubjectbyCourse/{CourseId}")]
        public async Task<IActionResult> GetSubjectbyCourseidAsync(int CourseId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                List<coursesubject> coursesubject = new List<coursesubject>();
                data = await _subjectService.GetSubjectByCourseId(CourseId, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
                DataTable ds = new DataTable();
                coursesubject = data.ResultData;
                //var resultSubjectList = coursesubject.AsEnumerable().GroupBy(x => x.SubjectType).ToList();
                var resultSubjectList = coursesubject.ToList();
                var dictResult = new Dictionary<string, object>();
                //foreach (var item in resultSubjectList)
                //{
                //    dictResult.Add(item.Key, item);
                //}
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(dictResult));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get subject By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllSubjectbyCourseId/{CourseId}/{UniversityId}")]
        public async Task<IActionResult> GetAllSubjectbyCourseId(int CourseId, int UniversityId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                List<coursesubject> coursesubject = new List<coursesubject>();
                data = await _subjectService.GetAllSubjectByCourseId(CourseId, UniversityId, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get subject By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetCourseSubjectCombinationCheckByUniversity/{UniversityId}")]
        public async Task<IActionResult> GetCourseSubjectCombinationCheckByUniversity(int UniversityId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();
            List<CourseSubjectCombinationCheck> coursesubject = new List<CourseSubjectCombinationCheck>();
            data = await _subjectService.GetCourseSubjectCombinationCheckByUniversity(UniversityId, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get subject By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("UniversityCourseSubjectCombinationCheck")]
        public async Task<IActionResult> UniversityCourseSubjectCombinationCheck([FromBody] UnvCourseSubjectCombinationCheck model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = await _subjectService.CreateUniversityCourseSubjectCombinationCheck(model);
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
                _logError.WriteTextToFile("Create Subject : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

    }
}
