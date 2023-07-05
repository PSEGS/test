using AdmissionPortal_API.Data.RepositoryInterface;
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
using static AdmissionPortal_API.Domain.Model.PgCourseFeeHead;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/PgCourseFeeHead")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

   
    public class PgCourseFeeHeadController : Controller
    {
        private readonly IPgCourseFeeHeadService _pgCourseFeeHeadService;
        private readonly ILogError _logError;
        public PgCourseFeeHeadController(IPgCourseFeeHeadService pgCourseFeeHeadService, ILogError logError)
        {
            _pgCourseFeeHeadService = pgCourseFeeHeadService;
            _logError = logError;
        }
        [HttpPost]
        [Route("AddCourseHeadFee")]
        public async Task<IActionResult> CreateCourseHeadFeeAsync([FromBody] List<AddPgCourseFeeHead> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                     
                        IEnumerable<Claim> claims = identity.Claims;

                        for (int i = 0; i < model.Count; i++)
                        {
                            model[i].CreatedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);
                            model[i].UniversityCollegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                        }

 
                    ServiceResult data = await _pgCourseFeeHeadService.AddCourseFeeHead(model);
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
                _logError.WriteTextToFile("Create PG Course Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }
        [HttpPost]
        [Route("AddCourseHeadFeewithCollegeType")]
        public async Task<IActionResult> AddCourseHeadFeewithCollegeType([FromBody] List<AddPgCourseFeeHeadFee> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;

                    IEnumerable<Claim> claims = identity.Claims;

                    for (int i = 0; i < model.Count; i++)
                    {
                        model[i].CreatedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);
                        model[i].UniversityCollegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                    }


                    ServiceResult data = await _pgCourseFeeHeadService.AddCourseFeeHeadWithCollegeType(model);
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
                _logError.WriteTextToFile("Create PG Course Fee Head Fee : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("CourseWiseFeeDetail")]
        public async Task<IActionResult>GetAllFeeDetailsByCourse(int universityId=0, int collegeId=0,int courseId = 0)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int college=0, University=0, Course = 0;
                string Logintype = string.Empty;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    Logintype = Convert.ToString(identity.FindFirst("loginType").Value);
                    if(Logintype== "Admin".ToUpper() || Logintype == "Education")
                    {
                        college = Convert.ToInt32(collegeId);
                        University = Convert.ToInt32(universityId);
                        Course = Convert.ToInt32(courseId);

                    }
                    else if(Logintype == "University".ToUpper())
                    {
                        University = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);
                        college = Convert.ToInt32(collegeId);
                        Course = Convert.ToInt32(courseId);

                    }
                    else if(Logintype== "COLLEGE".ToUpper())
                    {
                        University = 0; ;
                        college = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value); ;
                        Course = Convert.ToInt32(courseId);
                    }

                }
                else
                {
                    college = 1;
                    University = 1;
                    Course = 1;

                }
                data = await _pgCourseFeeHeadService.GetAllFeeDetailsCourseID(University,college,Course,Logintype);
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
                _logError.WriteTextToFile("Get ALL Fee Details By PG Course  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("CourseFeebyCollegeId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCourseFeeByCollegeId(string collegeID)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int college = 0;
                string Logintype = string.Empty;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if(string.IsNullOrEmpty(collegeID))
                {
                    if (identity != null)
                    {


                        college = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value); ;



                    }
                    else
                    {
                        college = 1;


                    }
                }
                else
                {
                    college = Convert.ToInt32(collegeID);
                }
                
                data = await _pgCourseFeeHeadService.GetcoursefeeBYCollegeId(college);
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
                _logError.WriteTextToFile("Get ALL PG Course Fee Details By CollegeId  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("FeeheadbyLoginType/{CollegeType}/{CourseFundType}")]       
        public async Task<IActionResult> FeeheadbyLoginType(int CollegeType ,int CourseFundType)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int typeID = 0,CreatedBy=0;
                string Logintype = string.Empty;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
             
                    if (identity != null)
                    {

                    Logintype = Convert.ToString(identity.FindFirst("loginType").Value);
                    CreatedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);


                }
                    else
                    {
                    Logintype = null;
                    }

                if (Logintype == "Admin".ToUpper() || Logintype == "Education")
                {
                    typeID = 1;
                }
                else if (Logintype == "University".ToUpper())
                {
                    typeID = 2;

                }
                else if (Logintype == "COLLEGE".ToUpper())
                {
                    typeID = 3;

                }
                else 
                {
                    typeID = 0;

                }

                data = await _pgCourseFeeHeadService.GetHeadByLoginType(typeID,CollegeType,CreatedBy,CourseFundType);
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
                _logError.WriteTextToFile("Get ALL PG Course Fee Details By CollegeId  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("AddFeeWaveOff")]
        public async Task<IActionResult> AddFeeWaveOff([FromBody] PgCourseFeeWaveOff model)
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
                        model.UniversityCollege_Id = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                    }
                    else
                    {
                        model.CreatedBy = 1;
                    }
                    ServiceResult data = await _pgCourseFeeHeadService.AddCourseWaveoffFee(model);
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
                _logError.WriteTextToFile("Create PG Fee Wave Off: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }
        [HttpGet]
        [Route("WaveOffDetails/{HeadId}/{CollegeType}")]
        public async Task<IActionResult> WaveOffDetails(int HeadId,int CollegeType)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int UniversityCollegeId = 0;
                string Logintype = string.Empty;
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null)
                {

                    UniversityCollegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);


                }
                else
                {
                    UniversityCollegeId = 0;
                }

               

                data = await _pgCourseFeeHeadService.GetFeeWaveOffDetails(HeadId, CollegeType,UniversityCollegeId);
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
                _logError.WriteTextToFile("Get WaveOff Details By Id: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("lockUnlockByCollegeID/{collegeID}/{status}")]
        public async Task<IActionResult> LockUnlockCourseSeatsByCollegeID(Int32 collegeID, int status)
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
                data = await _pgCourseFeeHeadService.LockUnlockCourseFeeHeadByCollegeID(collegeID, status, userid);
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
                _logError.WriteTextToFile("College PG Course head fee lock unlock : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("lockUnlockByUniversityID/{universityID}/{status}")]
        public async Task<IActionResult> LockUnlockCourseFeeHeadByUniversityID(Int32 universityID, int status)
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
                data = await _pgCourseFeeHeadService.LockUnlockCourseFeeHeadByUniversityID(universityID, status, userid);
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
                _logError.WriteTextToFile("University PG Course head fee lock unlock : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
