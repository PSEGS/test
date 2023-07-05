using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.ApiModel.Objection;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgCourseModel;


namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/verification")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class VerificationController : Controller
    {

        private readonly IVerificationService _verificationService;
        private readonly ILogError _logError;
        public VerificationController(IVerificationService verificationService, ILogError logError)
        {
            _verificationService = verificationService;
            _logError = logError;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetVerifiedStudentByRegId")]
        public async Task<IActionResult> GetVerifiedStudentByRegIdCollege(string RegId, int CollegeId)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _verificationService.GetVerifiedStudentByRegIdCollege(RegId, CollegeId);
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
                _logError.WriteTextToFile("Get Verified Student By RegId : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetStudentsByCollege/{verificationStatus}")]
        public async Task<IActionResult> GetStudentsByCollege(Int32 verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                Int32 collegeId = 0;
                if (ModelState.IsValid)
                {
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
                    ServiceResult data = await _verificationService.GetStudentsByCollege(collegeId, verificationStatus, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpPost]
        [Route("VerifyStudentWithSection")]
        public async Task<IActionResult> VerifyStudentWithSection([FromBody] VerifyStudentWithSection model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.VerifiedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);
                        model.CollegeId= Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);
                    }
                    ServiceResult data = await _verificationService.VerifyStudentWithSection(model);
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
                _logError.WriteTextToFile("Verify Student with section : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpPost]
        [Route("RevokeStudentVerification")]
        public async Task<IActionResult> RevokeStudentVerification([FromBody] CancelStudentRegistration model)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    model.UserId = Convert.ToInt32(identity.FindFirst("UserID").Value);

                }
                ServiceResult data = await _verificationService.RevokeStudentVerification(model);

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
                _logError.WriteTextToFile("Verify Student with section : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }

        [HttpGet]         
        [Route("GetStudentByRegId")]
        public async Task<IActionResult> GetStudentByRegId(string RegId,Int32 type)
        {
            try
            {

                ServiceResult data = new ServiceResult();
                data = await _verificationService.GetStudentByRegId(RegId,type);
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
                _logError.WriteTextToFile("Get Verified Student By RegId : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("Student-Course-Eligible")]
        public async Task<IActionResult> StudentCourseEligible([FromBody]StudentChoiceofCourseEligible model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.ActionTaken = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    ServiceResult data = new ServiceResult();
                    data = await _verificationService.StudentCourseEligible(model);
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
                _logError.WriteTextToFile("Get Verified Student By RegId : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("FinalVerification")]
        public async Task<IActionResult> FinalVerification([FromBody] FinalVerificationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.verifedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);
                        model.CollegeID = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                    }
                    ServiceResult data = new ServiceResult();
                    data = await _verificationService.FinalSubmit(model);
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
                _logError.WriteTextToFile("Final Verification : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        
        [HttpGet]
        [Route("ExportExcelStudentsByCollege/{verificationStatus}")]       
        public async Task<IActionResult> ExportExcelStudentsByCollege(Int32 verificationStatus)
        {
            try
            {
                Int32 collegeId = 0;
                if (ModelState.IsValid)
                {
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
                    ServiceResult data = await _verificationService.ExportExcelStudentsByCollege(collegeId, verificationStatus);

                    string fileName = string.Empty;
                    if(verificationStatus == 0) {
                        fileName = "Pending-Applications.xlsx";
                    }
                    else if(verificationStatus == 1) {
                        fileName = "Verified-Applications.xlsx";
                    }
                    else if(verificationStatus == 0) {
                        fileName = "Objection-Raised-Applications.xlsx";
                    }
                    else if(verificationStatus == 0) {
                        fileName = "Objection-Resolved-By-Student-Applications.xlsx";
                    }
                    else {
                        fileName = "Applications.xlsx";
                    }

                    List<ExportExcelModel> exportColumns = new()
                    {
                        new ExportExcelModel { Column = "SrNo", ColumnLabel = "S.No." },
                        new ExportExcelModel { Column = "RegistrationId", ColumnLabel = "Registration Id" },
                        new ExportExcelModel { Column = "StudentName", ColumnLabel = "Student Name" },
                        new ExportExcelModel { Column = "Gender", ColumnLabel = "Gender" },
                        new ExportExcelModel { Column = "CourseName", ColumnLabel = "Course Name" },
                        new ExportExcelModel { Column = "MobileNo", ColumnLabel = "Mobile No" },
                        new ExportExcelModel { Column = "Gender", ColumnLabel = "Gender" }
                    };

                    ExportController export = new();
                    return export.ExcelExport(data.ResultData, exportColumns, fileName);
                }
                else
                {
                    return await Task.Run(() => BadRequest(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Export to Excel To be verified Student List ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpGet]
        [Route("GetStudentsByCollegeBCOM/{verificationStatus}")]
        public async Task<IActionResult> GetStudentsByCollegeBCOM(Int32 verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                Int32 collegeId = 0;
                if (ModelState.IsValid)
                {
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
                    ServiceResult data = await _verificationService.GetStudentsByCollegeBCOM(collegeId, verificationStatus, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }
        [HttpGet]
        [Route("GetStudentByRegIdBCOM")]
        public async Task<IActionResult> GetStudentByRegIdBCOM(string RegId, Int32 type)
        {
            try
            {

                ServiceResult data = new ServiceResult();
                data = await _verificationService.GetStudentByRegIdBCOM(RegId, type);
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
                _logError.WriteTextToFile("Get Verified Student By RegId : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("SAVEBCOMStudentWeightage")]
        public async Task<IActionResult> SAVEBCOMStudentWeightage([FromBody] BCOMStudentWeightage model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;                        
                        model.CollegeID = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                    }
                    ServiceResult data = new ServiceResult();
                    data = await _verificationService.SAVEBCOMStudentWeightage(model);
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
                _logError.WriteTextToFile("Final Verification : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("ExportExcelStudentsByCollegeBCOM/{verificationStatus}")]
        public async Task<IActionResult> ExportExcelStudentsByCollegeBCOM(Int32 verificationStatus)
        {
            try
            {
                Int32 collegeId = 0;
                if (ModelState.IsValid)
                {
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
                    ServiceResult data = await _verificationService.ExportExcelStudentsByCollegeBCOM(collegeId, verificationStatus);

                    string fileName = string.Empty;
                    if (verificationStatus == 0)
                    {
                        fileName = "Pending-Applications.xlsx";
                    }                     
                    else
                    {
                        fileName = "Applications.xlsx";
                    }

                    List<ExportExcelModel> exportColumns = new()
                    {
                        new ExportExcelModel { Column = "SrNo", ColumnLabel = "S.No." },
                        new ExportExcelModel { Column = "RegistrationId", ColumnLabel = "Registration Id" },
                        new ExportExcelModel { Column = "StudentName", ColumnLabel = "Student Name" },
                        new ExportExcelModel { Column = "CourseName", ColumnLabel = "Course Name" },
                        new ExportExcelModel { Column = "MobileNo", ColumnLabel = "Mobile No" }
                    };

                    ExportController export = new();
                    return export.ExcelExport(data.ResultData, exportColumns, fileName);
                }
                else
                {
                    return await Task.Run(() => BadRequest(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Export to Excel To be verified Student List ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }
        [HttpGet]
        [Route("GetStudentCombinationByCCId/{CollegeID}/{CourseId}/{RegId}")]
        public async Task<IActionResult> GetStudentCombinationByCCId(Int32 CollegeID, Int32 CourseId,string RegId)
        {
            try
            {

                ServiceResult data = new ServiceResult();
                data = await _verificationService.GetStudentCombinationByCCId(CollegeID, CourseId, RegId);
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
                _logError.WriteTextToFile("Get Student Courese Combination : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

    }
}
