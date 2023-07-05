
using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/college")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class CollegeController : Controller
    {
        private readonly ICollegeService _collegeService;
        private readonly ILogError _logError;
        public CollegeController(ICollegeService collegeService, ILogError logError)
        {
            _collegeService = collegeService;
            _logError = logError;
        }

        /// <summary>
        /// Create College
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCollege")]
        public async Task<IActionResult> CreateCollegeAsync([FromBody] AddCollege model)
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
                    ServiceResult data = await _collegeService.CreateCollegeAsync(model);
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
        /// <summary>
        /// Create Citizen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCollege")]

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateCollege([FromBody] UpdateCollege model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.CreatedBy = Convert.ToString(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.CreatedBy = "1";

                    }

                    ServiceResult data = await _collegeService.UpdateCollege(model);
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

        [HttpPut]
        [Route("UpdateCollege")]

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateCollegeNew([FromBody] UpdateCollegeModel model)
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
                    ServiceResult data = await _collegeService.UpdateCollegeNew(model);
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




        /// <summary>
        /// Delete college
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{CollegeId}")]
        public async Task<IActionResult> DeleteCollegeAsync(int CollegeId)
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

                ServiceResult data = await _collegeService.DeleteCollege(CollegeId, userid);

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
                _logError.WriteTextToFile("Delete Citizen : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get All College
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetAllCollge/{university}")]

        public async Task<IActionResult> GetAllCollge(int university, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _collegeService.GetAllCollege(university, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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

        /// <summary>
        /// Get   Colleges by Id
        /// </summary>
        /// <param name="Collegeid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{collegeId}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetcollegeByidAsync(int collegeId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetCollegeById(collegeId);
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
                _logError.WriteTextToFile("Get College By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCollegeByDistrictId")]
        public async Task<IActionResult> GetCollegeByDistrictId(int districtId, int collegeTypeId, int admissionId, string type, string ugpg = "ug")
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetCollegeByDistrictId(districtId, collegeTypeId, admissionId, type, ugpg);
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
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetDistrictCollegesByGender")]
        public async Task<IActionResult> GetDistrictCollegesByGender(int districtId, int collegeTypeId, int admissionId, int studentId, string type, string ugpg = "ug")
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetDistrictCollegesByGender(districtId, collegeTypeId, admissionId, studentId, type, ugpg);
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
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCollegeCourses/{collegeId}")]
        [Route("GetCollegeCourses/{collegeId}/{CGtype}")]
        public async Task<IActionResult> GetCollegeCourses(int collegeId, string CGtype = "ug")
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetCollegeCourses(collegeId, CGtype);
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
                _logError.WriteTextToFile("Get College courses : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllColleges")]
        public async Task<IActionResult> GetAllColleges()
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetAllColleges();
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
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("ResetCollegePassword/{collegeId}")]
        public async Task<IActionResult> ResetCollegePassword(Int32 CollegeID)
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
                data = await _collegeService.ResetCollegePassword(CollegeID, userid);
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
                _logError.WriteTextToFile("College Password Reset : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPut]
        [Route("CollegeActiveInactive/{CollegeID}/{status}")]
        public async Task<IActionResult> CollegeActiveInactive(Int32 CollegeID, int status)
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
                data = await _collegeService.CollegeInactiveActive(CollegeID, status, userid);
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
                _logError.WriteTextToFile("College Active Incative : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("lockUnlockCollegeInfo/{CollegeID}/{status}")]
        [Route("lockUnlockCollegeInfo/{CollegeID}/{status}/{OTP}")]
        public async Task<IActionResult> lockUnlockCollegeInfo(Int32 CollegeID, int status, string OTP = null)
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
                data = await _collegeService.lockunlockCollegeInfo(CollegeID, status, userid, OTP);
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
                _logError.WriteTextToFile("College lock unlock : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GenreateOTP/{CollegeID}")]
        public async Task<IActionResult> GenreateOTP(Int32 CollegeID)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int userid = 0;

                data = await _collegeService.generateOTP(CollegeID);
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
                _logError.WriteTextToFile("College Generate OTP: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("UploadProspectus")]
        public async Task<IActionResult> UploadProspectus([FromForm] UploadCollegeProspectus model)
        {
            try
            {

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    model.CollegeID = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                }
                else
                {
                    model.CollegeID = 1;

                }
                ServiceResult data = new ServiceResult();


                string Base64File = string.Empty;
                if (model.file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        model.prospectus = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                else
                {
                    model.prospectus = "";
                }

                data = await _collegeService.UploadProspectus(model);
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
                _logError.WriteTextToFile("College Upload Prospectus: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetDistrictCollege")]
        public async Task<IActionResult> GetDistrictCollege(int districtId, int collegeTypeId, int admissionId, string type)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetDistrictCollege(districtId, collegeTypeId, admissionId, type);
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
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCollegeByCGtype/{type}")]
        public async Task<IActionResult> GetCollegeByCGtype(string type)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetCollegeByCGtype(type);
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
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("ReportsLogin")]
        public async Task<IActionResult> ReportsLogin()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = 0;
                string username = null;
                string userType = null;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    userId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    username = identity.FindFirst("user_Name").Value;
                    userType = identity.FindFirst("UserType").Value;
                }
                ServiceResult data = await _collegeService.ReportsLogin(userId, username, userType);
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
                _logError.WriteTextToFile("Reports Login : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("UploadCancelledCheque")]
        public async Task<IActionResult> UploadCancelledCheque([FromForm] UploadCancelledChequeModel model)
        {
            try
            {

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    model.CollegeID = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                }
                else
                {
                    model.CollegeID = 1;

                }
                ServiceResult data = new ServiceResult();


                string Base64File = string.Empty;
                if (model.file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        model.CancelledCheque = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                else
                {
                    model.CancelledCheque = "";
                }

                data = await _collegeService.UploadCancelledCheque(model);
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
                _logError.WriteTextToFile("College Upload Cancelled Cheque: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("UnlockStudentForCollege")]
        public async Task<IActionResult> UnlockStudentForCollege([FromBody] UnlockStudentModel model)
        {
            try
            {
                int collegeId = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    collegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);                   
                }
                else
                {
                    collegeId = 0;
                }
                ServiceResult data = new ServiceResult();
                model.CollegeID = collegeId;
                data = await _collegeService.UnlockStudent(model);
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
                _logError.WriteTextToFile("Student Unlock by college: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetStudentDetailsForCancellation/{RegId}/{type}")]
        public async Task<IActionResult> GetStudentDetailsForCancellation(string RegId, string type)
        {
            try
            {
                int collegeId = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    collegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);
                }
                else
                {
                    collegeId = 0;
                }
                ServiceResult data = await _collegeService.GetStudentDetailsByRegId(RegId, type, collegeId);
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
                _logError.WriteTextToFile("Get Student Details For Cancellation : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("CancelStudentAdmissionSeat")]
        public async Task<IActionResult> CancelStudentAdmissionSeat([FromBody]CancelAdmissionSeat model)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    model.collegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);
                }
                else
                {
                    model.collegeId = 0;
                }
                ServiceResult data = await _collegeService.CancelStudentAdmissionSeat(model);
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
                _logError.WriteTextToFile("Cancel Student admission seat : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }


        [HttpGet]       
        [Route("GetStudentDocumentDownload/{CollegeID}/{type}")]
        public async Task<IActionResult> GetStudentDocumentDownload(string CollegeID,string type)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;                
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetStudentDocumentDownload(CollegeID, type);
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
                _logError.WriteTextToFile("Get Student Document Download : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCollegeProspectus/{CollegeID}")]
        public async Task<IActionResult> GetCollegeProspectus(string CollegeID)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetCollegeProspectus(CollegeID);
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
                _logError.WriteTextToFile("Get College Prospectus : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllCollegeIslock/{Admissiontype}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCollegeIslock(int Admissiontype)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _collegeService.GetCollegesIslock(Admissiontype);
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
                _logError.WriteTextToFile("Get All Colleges when Islock is 1 : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

    }
}

