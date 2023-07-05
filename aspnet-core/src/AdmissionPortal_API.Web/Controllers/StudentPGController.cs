using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ApiModel.StudentPG;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/studentpg")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class StudentPGController : Controller
    {
        private readonly IStudentServicePG _studentServicePG;
        private readonly ILogError _logError;
        public StudentPGController(IStudentServicePG studentServicePG, ILogError logError)
        {
            _studentServicePG = studentServicePG;
            _logError = logError;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterStudentPG")]
        public async Task<IActionResult> RegisterStudentPG([FromBody] RegisterStudent model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = await _studentServicePG.RegisterStudentPG(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Register Student PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("StudentLoginPG")]
        public async Task<IActionResult> StudentLoginPG([FromBody] StudentPGLogin model)
        {
            try
            {
                var userAgent = HttpContext.Request.Headers["User-Agent"];
                string VisitorsIPAddress = string.Empty;
                if (string.IsNullOrEmpty(VisitorsIPAddress))
                {
                    try
                    {
                        VisitorsIPAddress = CommonMethod.GetPublicIp();
                    }
                    catch (Exception ex)
                    {
                        VisitorsIPAddress = string.Empty;
                    }
                }
                model.IPAddress = VisitorsIPAddress;
                model.Browser = userAgent;
                if(!string.IsNullOrEmpty(model.UserPassword))
                {
                    model.UserPassword = Decryption.DecodeLoginPassword(model.UserPassword);

                }
                ServiceResult data = await _studentServicePG.StudentLoginPG(model);
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
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Student Login PG : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }
        [HttpPut]
        [Route("UpdateStudentPersonalDetailsPG")]
        public async Task<IActionResult> UpdateStudentPersonalDetails([FromBody] UpdatePersonalDetails model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UpdateStudentPersonalDetails(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Update Student Personal Details PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateBankDetailsPG")]
        public async Task<IActionResult> UpdateBankDetails([FromBody] UpdateBankDetails model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UpdateBankDetails(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Update Bank Details PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateAddressDetailsPG")]
        public async Task<IActionResult> UpdateAddressDetails([FromBody] UpdateAddressDetails model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UpdateAddressDetails(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Update Address Details PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateAcademicDetailsPG")]
        public async Task<IActionResult> UpdateAcademicDetails([FromBody] UpdateAcademicDetailsPG model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UpdateAcademicDetails(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Update Academic Details PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateWeightagePG")]
        public async Task<IActionResult> UpdateWeightagePG([FromBody] WeightagePG model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UpdateWeightage(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Update Weightage: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UploadDocumentsPG")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadDocumentsPG([FromBody] UploadDocumentsPG model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UploadDocuments(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Upload Documents : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetStudentByIdPG")]
        public async Task<IActionResult> GetStudentByIdPG()
        {
            try
            {
                int studentID = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentID = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetStudentById(studentID);
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
                _logError.WriteTextToFile("Get Student By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSubjectByStudentIdPG")]
        public async Task<IActionResult> GetSubjectByStudentIdPG()
        {
            try
            {
                int studentID = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentID = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetSubjectByStudentId(studentID);
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
                _logError.WriteTextToFile("Get Subject By Student Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetProgressBarPG")]
        public async Task<IActionResult> GetProgressBarPG()
        {
            try
            {
                int studentID = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentID = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetProgressBar(studentID);
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
                _logError.WriteTextToFile("Get Progress Bar : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("UpdateDeclarationsPG")]
        public async Task<IActionResult> UpdateDeclarationsPG([FromBody] Declaration model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UpdateDeclarations(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Update Declarations: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GenerateOTPpg")]
        public async Task<IActionResult> GenerateOTPpg([Required] string userName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = await _studentServicePG.GenerateOTP(userName);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Generate OTP : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("UpdateCourseChoicePG")]
        public async Task<IActionResult> UpdateCourseChoicePG([FromBody] CourseChoice model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UpdateCourseChoice(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Update Course Choice: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }
        [HttpGet]
        [Route("GetCourseChoiceByStudentIdPG")]
        public async Task<IActionResult> GetCourseChoiceByStudentIdPG()
        {
            try
            {
                int studentID = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentID = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetCourseChoiceByStudentId(studentID);
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
                _logError.WriteTextToFile("Get Subject By Student Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetUploadedDocumentsPG")]
        public async Task<IActionResult> GetUploadedDocumentsPG()
        {
            try
            {
                int studentID = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentID = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetUploadedDocuments(studentID);
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
                _logError.WriteTextToFile("Get Uploaded Documents : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetBlobDocumentPG/{blobReference}")]
        public async Task<IActionResult> GetBlobDocumentPG(string blobReference)
        {
            try
            {
                blobReference = Uri.UnescapeDataString(blobReference);
                ServiceResult data = _studentServicePG.GetBlobDocument(blobReference);
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
                _logError.WriteTextToFile("Get Uploaded Documents : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("RegisterationFeesPaymentPG")]
        public async Task<IActionResult> RegisterationFeesPaymentPG([FromBody] RegisterationFees model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.RegisterationFeesPayment(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Registeration Fees Payment : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpGet]
        [Route("UnlockFormPG")]
        public async Task<IActionResult> UnlockFormPG([Required] string oTP)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int studentId = 0;
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        studentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UnlockForm(oTP, studentId);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Unlock Form : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("UpdateStudentDetailsPG")]
        public async Task<IActionResult> UpdateStudentDetailsPG([FromBody] UpdateStudentDetails model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _studentServicePG.UpdateStudentDetails(model);
                    if (data.Status == true)
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
                _logError.WriteTextToFile("Update Student Details: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("StudentForgotPasswordPG")]
        public async Task<IActionResult> StudentForgotPasswordPG([FromBody] ForgotPassword model)
        {
            try
            {
                var data = await _studentServicePG.ForgotPassword(model.Email);
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
            catch (Exception ex)
            {
                _logError.WriteTextToFile("StudentForgotPassword : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpGet]
        [Route("GetStudentDetailsByRegIdPG/{RegId}/{RollNo}")]
        public async Task<IActionResult> GetStudentDetailsByRegIdPG(string RegId, string RollNo)
        {
            try
            {
                int studentID = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentID = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetStudentDetailsByRegId(RegId);
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
                _logError.WriteTextToFile("Get Subject By Student Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("GetStudentAppDetailsByRegIdPG")]
        public async Task<IActionResult> GetStudentAppDetailsByRegIdPG([FromBody] FilterStudent _filter)
        {
            try
            {
                ServiceResult data = await _studentServicePG.GetStudentAppDetailsByRegId(_filter);
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
                _logError.WriteTextToFile("Get Student details By Reg Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetCourseChoiceByRegIdPG/{RegId}")]
        public async Task<IActionResult> GetCourseChoiceByRegIdPG(string RegId)
        {
            try
            {
                ServiceResult data = await _studentServicePG.GetCourseChoiceByRegId(RegId);
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
                _logError.WriteTextToFile("Get Subject By Student Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetSubjectByRegIdPG/{RegId}")]
        public async Task<IActionResult> GetSubjectByRegIdPG(string RegId)
        {
            try
            {

                ServiceResult data = await _studentServicePG.GetSubjectByRegId(RegId);
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
                _logError.WriteTextToFile("Get Subject By Reg Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetDocumentsByRegIdPG/{RegId}")]
        public async Task<IActionResult> GetDocumentsByRegIdPG(string RegId)
        {
            try
            {
                ServiceResult data = await _studentServicePG.GetDocumentsByRegId(RegId);
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
                _logError.WriteTextToFile("Get docs By Reg Id PG : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetObjectionsByPGStudentID")]
        public async Task<IActionResult> GetObjectionsByPGStudentID()
        {
            try
            {
                int studentid = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentid = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetObjectionsByPGStudentID(studentid);
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
                _logError.WriteTextToFile("Get Objection ByStudentID : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetAdmissionSeatDetails")]
        public async Task<IActionResult> GetAdmissionSeatDetails()
        {
            try
            {
                int studentid = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentid = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetAdmissionSeatDetails(studentid);
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
        [Route("GetAdmissionFeeReceipt/{transactionId}")]
        public async Task<IActionResult> GetAdmissionFeeReceipt(string transactionId)
        {
            try
            {
                int studentid = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentid = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetAdmissionFeeReceipt(studentid, transactionId);
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
                _logError.WriteTextToFile("Get Fee Receipt By student Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAdmissionFeeReceiptList")]
        public async Task<IActionResult> GetAdmissionFeeReceiptList()
        {
            try
            {
                int studentid = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    studentid = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                ServiceResult data = await _studentServicePG.GetAdmissionFeeReceiptList(studentid);
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
                _logError.WriteTextToFile("Get Fee Receipt By student Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
