using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using AdmissionPortal_API.Utility.MessageConfig;
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
    [Route("api/student")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogError _logError;
        public StudentController(IStudentService studentService, ILogError logError)
        {
            _studentService = studentService;
            _logError = logError;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterStudent")]
        public async Task<IActionResult> RegisterStudent([FromBody] AddStudent model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = await _studentService.RegisterStudent(model);

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
                _logError.WriteTextToFile("Register Student: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("StudentLogin")]
        public async Task<IActionResult> StudentLogin([FromBody] StudentLogin model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.OTP))
                {
                    model.OTP = null;
                }
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
                if (!string.IsNullOrEmpty(model.UserPassword))
                {
                    model.UserPassword = Decryption.DecodeLoginPassword(model.UserPassword);
                }
                ServiceResult data = await _studentService.StudentLogin(model);
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
                _logError.WriteTextToFile("Student Login : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateStudentPersonalDetails")]
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
                    ServiceResult data = await _studentService.UpdateStudentPersonalDetails(model);
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
                _logError.WriteTextToFile("Update Student Personal Details: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateBankDetails")]
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
                    ServiceResult data = await _studentService.UpdateBankDetails(model);
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
                _logError.WriteTextToFile("Update Bank Details: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateAddressDetails")]
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
                    ServiceResult data = await _studentService.UpdateAddressDetails(model);
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
                _logError.WriteTextToFile("Update Address Details: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateAcademicDetails")]
        public async Task<IActionResult> UpdateAcademicDetails([FromBody] UpdateAcademicDetails model)
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
                    ServiceResult data = new ServiceResult();
                    if(model.TotalMarks==null)
                    {
                        model.ObtainedMarks = 0;

                    }
                    if (model.ObtainedMarks == null)
                    {
                        model.ObtainedMarks = 0;
                    }
                    if (model.TwelvethTotalMarks == null)
                    {
                        model.TwelvethTotalMarks = 0;

                    }
                    if (model.TwelvethObtainedMarks == null)
                    {
                        model.TwelvethObtainedMarks = 0;
                    }
                    //model.TotalMarks=if( model.TotalMarks == null ? 0 : model.TotalMarks) ;
                        if ((model.TwelvethTotalMarks >= model.TwelvethObtainedMarks) || (model.TotalMarks>=model.ObtainedMarks))
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(model.IsDiploma))) { model.IsDiploma = false; }
                         data = await _studentService.UpdateAcademicDetails(model);
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
                        data.Status = false;
                        data.Message = MessageConfig.MarksAlert;
                        data.StatusCode = 200;
                        return await Task.Run(() => Ok(data));
                    }

                }
                else
                {
                    return await Task.Run(() => BadRequest(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Academic Details: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPut]
        [Route("UpdateWeightage")]
        public async Task<IActionResult> UpdateWeightage([FromBody] Weightage model)
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
                    ServiceResult data = await _studentService.UpdateWeightage(model);
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
        [Route("UploadDocuments")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadDocuments([FromBody] UploadDocuments model)
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
                    ServiceResult data = await _studentService.UploadDocuments(model);
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
        [Route("GetStudentById")]
        public async Task<IActionResult> GetStudentById()
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
                ServiceResult data = await _studentService.GetStudentById(studentID);
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
        [Route("GetSubjectByStudentId")]
        public async Task<IActionResult> GetSubjectByStudentId()
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
                ServiceResult data = await _studentService.GetSubjectByStudentId(studentID);
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
        [Route("GetProgressBar")]
        public async Task<IActionResult> GetProgressBar()
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
                ServiceResult data = await _studentService.GetProgressBar(studentID);
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
        [Route("UpdateDeclarations")]
        public async Task<IActionResult> UpdateDeclarations([FromBody] Declaration model)
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
                    ServiceResult data = await _studentService.UpdateDeclarations(model);
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
        [Route("GenerateOTP")]
        public async Task<IActionResult> GenerateOTP([Required] string userName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = await _studentService.GenerateOTP(userName);
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
        [Route("UpdateCourseChoice")]
        public async Task<IActionResult> UpdateCourseChoice([FromBody] CourseChoice model)
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
                    ServiceResult data = await _studentService.UpdateCourseChoice(model);
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
        [Route("GetCourseChoiceByStudentId")]
        public async Task<IActionResult> GetCourseChoiceByStudentId()
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
                ServiceResult data = await _studentService.GetCourseChoiceByStudentId(studentID);
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
        [Route("GetUploadedDocuments")]
        public async Task<IActionResult> GetUploadedDocuments()
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
                ServiceResult data = await _studentService.GetUploadedDocuments(studentID);
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
        [Route("GetBlobDocument/{blobReference}")]
        public async Task<IActionResult> GetBlobDocument(string blobReference)
        {
            try
            {
                blobReference = Uri.UnescapeDataString(blobReference);
                ServiceResult data = _studentService.GetBlobDocument(blobReference);
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
        [Route("RegisterationFeesPayment")]
        public async Task<IActionResult> RegisterationFeesPayment([FromBody] RegisterationFees model)
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
                    ServiceResult data = await _studentService.RegisterationFeesPayment(model);
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
        [Route("UnlockForm")]
        public async Task<IActionResult> UnlockForm([Required] string oTP)
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
                    ServiceResult data = await _studentService.UnlockForm(oTP, studentId);
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
        [Route("UpdateStudentDetails")]
        public async Task<IActionResult> UpdateStudentDetails([FromBody] UpdateStudentDetails model)
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
                    ServiceResult data = await _studentService.UpdateStudentDetails(model);
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
        [Route("StudentForgotPassword")]
        public async Task<IActionResult> StudentForgotPassword([FromBody] ForgotPassword model)
        {
            try
            {
                var data = await _studentService.ForgotPassword(model.Email);
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
        [Route("GetStudentDetailsByRegId/{RegId}/{RollNo}")]
        public async Task<IActionResult> GetStudentDetailsByRegId(string RegId, string RollNo)
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
                ServiceResult data = await _studentService.GetStudentDetailsByRegId(RegId, RollNo);
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
        [Route("GetStudentAppDetailsByRegId")]
        public async Task<IActionResult> GetStudentAppDetailsByRegId([FromBody] FilterStudent _filter)
        {
            try
            {
                ServiceResult data = await _studentService.GetStudentAppDetailsByRegId(_filter);
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
        [Route("GetCourseChoiceByRegId/{RegId}")]
        public async Task<IActionResult> GetCourseChoiceByRegId(string RegId)
        {
            try
            {
                ServiceResult data = await _studentService.GetCourseChoiceByRegId(RegId);
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
        [Route("GetSubjectByRegId/{RegId}")]
        public async Task<IActionResult> GetSubjectByRegId(string RegId)
        {
            try
            {

                ServiceResult data = await _studentService.GetSubjectByRegId(RegId);
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
        [Route("GetDocumentsByRegId/{RegId}")]
        public async Task<IActionResult> GetDocumentsByRegId(string RegId)
        {
            try
            {
                ServiceResult data = await _studentService.GetDocumentsByRegId(RegId);
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
                _logError.WriteTextToFile("Get docs By Reg Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("UpdateCourseChoiceWithSubject")]
        public async Task<IActionResult> UpdateCourseChoiceWithSubject([FromBody] SubjectCombinationsWithCollege model)
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
                    ServiceResult data = await _studentService.UpdateCourseChoiceWithSubject(model);
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
                _logError.WriteTextToFile("Update Course Choice With Subject: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPost]
        [Route("GetSelectedCourseFee")]
        public async Task<IActionResult> GetSelectedCourseFee([FromBody] CourseChoiceFee model)
        {
            try
            {
                ServiceResult data = await _studentService.GetCourseChoiceFee(model);
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
                _logError.WriteTextToFile("Get select course fee : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetEligibleCourseByStudentID/{CollegeId}/{StudentId}")]
        public async Task<IActionResult> GetEligibleCourseByStudentID(Int32 CollegeId, Int32 StudentId)
        {
            try
            {

                ServiceResult data = await _studentService.GetEligibleCourseByStudentID(CollegeId, StudentId);
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
                _logError.WriteTextToFile("Get Eligible Course ByStudentID : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetObjectionsByStudentID")]
        public async Task<IActionResult> GetObjectionsByStudentID()
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
                ServiceResult data = await _studentService.GetObjectionsByStudentID(studentid);
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
                ServiceResult data = await _studentService.GetAdmissionSeatDetails(studentid);
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
                ServiceResult data = await _studentService.GetAdmissionFeeReceipt(studentid, transactionId);
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
                ServiceResult data = await _studentService.GetAdmissionFeeReceiptList(studentid);
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
