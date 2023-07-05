using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/meritmodulepg")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class MeritModulePGController : ControllerBase
    {
        private readonly IMeritModulePGService _meritService;
        private readonly ILogError _logError;
        public MeritModulePGController(IMeritModulePGService meritService, ILogError logError)
        {
            _meritService = meritService;
            _logError = logError;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetProvisionalMeritList/{CollegeId}")]
        public async Task<IActionResult> GetProvisionalMeritList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            try
            {

                ServiceResult data = await _meritService.GetProvisionalMeritList(CollegeId, CourseId, ReservationId, CategoryId, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get Provisional Merit List UG: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetWaitingList/{CollegeId}")]
        public async Task<IActionResult> GetWaitingList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {

                ServiceResult data = await _meritService.GetWaitingList(CollegeId, CourseId, ReservationId, CategoryId,IsBorderArea,SingleGirlChild,CancerAidsThalassemia,NRI,IsKashmiriMigrant,RuralArea, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get Waiting List UG: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetCourseFeeByStudentId/{CourseId}/{CollegeId}/{StudentId}")]
        public async Task<IActionResult> GetCourseFeeByStudentId(int CourseId, int CollegeId, int StudentId)
        {
            try
            {

                ServiceResult data = await _meritService.GetCourseFeeByStudentId(CourseId, CollegeId, StudentId);
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
                _logError.WriteTextToFile("Get Waiting List PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("SaveAdmissionSeat")]
        public async Task<IActionResult> SaveAdmissionSeat([FromBody] AdmissionSeat seat)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    seat.UserId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _meritService.SaveAdmissionSeat(seat);
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
                _logError.WriteTextToFile("save admission seat PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAdmissionSeatStatusByRegID/{RegId}/{CollegeId}/{AdmissionType}")]
        public async Task<IActionResult> GetAdmissionSeatStatus(string RegId, string CollegeId, string AdmissionType)
        {
            try
            {
                ServiceResult data = await _meritService.GetAdmissionSeatStatus(RegId, CollegeId,AdmissionType);
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
                _logError.WriteTextToFile("save admission seat UG: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetFeeReceiptByRegId/{RegId}/{CollegeId}")]
        public async Task<IActionResult> GetFeeReceiptByRegId(string RegId, Int32 CollegeId)
        {
            try
            {
                ServiceResult data = await _meritService.GetFeeReceiptByRegId(RegId, CollegeId);
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
                _logError.WriteTextToFile("Get Fee Receipt By Reg Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("ExportMeritExcel/{CollegeId}/{CourseId}/{ReservationId}/{CategoryId}/{searchKeyword}")]
        [Route("ExportMeritExcel/{CollegeId}/{CourseId}/{ReservationId}/{CategoryId}")]
        public async Task<IActionResult> ExportMeritExcel(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, string searchKeyword)
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
                    ServiceResult data = await _meritService.ExportMeritExcel(collegeId, CourseId, ReservationId, CategoryId, searchKeyword);

                    string fileName = string.Empty;
                    fileName = "MeritPG.xlsx";

                    List<ExportExcelModel> exportColumns = new()
                    {
                        new ExportExcelModel { Column = "sno", ColumnLabel = "S.No." },
                        new ExportExcelModel { Column = "Rank", ColumnLabel = "Rank" },
                        new ExportExcelModel { Column = "RegistrationNumber", ColumnLabel = "Registration Id" },
                        new ExportExcelModel { Column = "StudentName", ColumnLabel = "Name" },
                        new ExportExcelModel { Column = "FatherName", ColumnLabel = "Father Name" },
                        new ExportExcelModel { Column = "MobileNo", ColumnLabel = "Mobile" },
                        new ExportExcelModel { Column = "Gender", ColumnLabel = "Gender" },
                        new ExportExcelModel { Column = "Category", ColumnLabel = "Category" },
                        new ExportExcelModel { Column = "AdmissionSeatReservationCategory", ColumnLabel = "Seat Allocation Category" },
                        new ExportExcelModel { Column = "YearOfPassing", ColumnLabel = "Year Of Passing" },
                        new ExportExcelModel { Column = "TwelvethPercentage", ColumnLabel = "UG" },
                        new ExportExcelModel { Column = "Weightage", ColumnLabel = "weightage" },
                        new ExportExcelModel { Column = "FinalWeightage", ColumnLabel = "Final Weightage" },
                        new ExportExcelModel { Column = "MeritInCourses", ColumnLabel = "Merit In Courses" }
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
                _logError.WriteTextToFile("Export to Merit Excel", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }


        [HttpPost]
        [Route("SendBookSeatOTP")]
        public async Task<IActionResult> SendBookSeatOTP([FromBody] OTPRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = await _meritService.SendOTP(model);
                    if (data.StatusCode == 200)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else if (data.StatusCode == 404)
                    {
                        return await Task.Run(() => NotFound(data));
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
                _logError.WriteTextToFile("Send OTP : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPost]
        [Route("VerifyBookSeatOTP")]
        public async Task<IActionResult> VerifyBookSeatOTP([FromBody] VerifyOTP model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = _meritService.VerifyOTP(model);
                    if (data.StatusCode == 200)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else if (data.StatusCode == 404)
                    {
                        return await Task.Run(() => NotFound(data));
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
                _logError.WriteTextToFile("Verify Book Seat OTP : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpGet]
        [Route("GetStudentFeeReceiptList/{RegId}/{CollegeId}")]
        public async Task<IActionResult> GetStudentFeeReceiptList(string RegId, int CollegeId)
        {
            try
            {
                ServiceResult data = await _meritService.GetStudentFeeReceiptList(RegId, CollegeId);
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
                _logError.WriteTextToFile("Get Fee Receipt List : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }


        [HttpPost]
        [Route("RevokeAdmissionSeat")]
        public async Task<IActionResult> RevokeAdmissionSeat([FromBody] RevokeSeat revoke)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    revoke.revokeBy = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _meritService.RevokeAdmissionSeat(revoke);
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
                _logError.WriteTextToFile("Revoke admission seat PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetVacantSeatByCollege/{collegeId}")]
        public async Task<IActionResult> GetVacantSeatByCollege(int collegeId)
        {
            try
            {

                //var identity = HttpContext.User.Identity as ClaimsIdentity;
                //if (identity != null)
                //{
                //    IEnumerable<Claim> claims = identity.Claims;
                //    collegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                //}
                ServiceResult data = await _meritService.GetVacantSeatByCollege(collegeId);
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
                _logError.WriteTextToFile("Get Vacant Seat List : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("ExportWaitingExcel/{CollegeId}/{CourseId}/{ReservationId}/{CategoryId}/{IsBorderArea}/{SingleGirlChild}/{CancerAidsThalassemia}/{NRI}/{IsKashmiriMigrant}/{RuralArea}/{searchKeyword}")]
        [Route("ExportWaitingExcel/{CollegeId}/{CourseId}/{ReservationId}/{CategoryId}/{IsBorderArea}/{SingleGirlChild}/{CancerAidsThalassemia}/{NRI}/{IsKashmiriMigrant}/{RuralArea}")]
        public async Task<IActionResult> ExportWaitingExcel(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, string? searchKeyword)
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
                        collegeId = 0;
                    }
                    ServiceResult data = await _meritService.ExportWaitingExcel(collegeId, CourseId, ReservationId, CategoryId, IsBorderArea, SingleGirlChild, CancerAidsThalassemia, NRI, IsKashmiriMigrant, RuralArea, searchKeyword);
                    string fileName = string.Empty;
                    string dt = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    fileName = "WaitingList_" + dt.Replace(" ", "-") + ".xlsx";

                    List<ExportExcelModel> exportColumns = new()
                    {
                        new ExportExcelModel { Column = "sno", ColumnLabel = "S.No." },
                        new ExportExcelModel { Column = "Rank", ColumnLabel = "Rank" },
                        new ExportExcelModel { Column = "RegistrationNumber", ColumnLabel = "Registration Id" },
                        new ExportExcelModel { Column = "StudentName", ColumnLabel = "Name" },
                        new ExportExcelModel { Column = "FatherName", ColumnLabel = "Father Name" },
                        new ExportExcelModel { Column = "MobileNo", ColumnLabel = "Mobile" },
                        new ExportExcelModel { Column = "Gender", ColumnLabel = "Gender" },
                        new ExportExcelModel { Column = "Category", ColumnLabel = "Category" },
                        new ExportExcelModel { Column = "YearOfPassing", ColumnLabel = "Year Of Passing" },
                        new ExportExcelModel { Column = "TwelvethPercentage", ColumnLabel = "UG" },
                        new ExportExcelModel { Column = "Weightage", ColumnLabel = "weightage" },
                        new ExportExcelModel { Column = "FinalWeightage", ColumnLabel = "Final Weightage" }
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
                _logError.WriteTextToFile("Export to Merit Excel ug", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));
            }
        }

    }
}
