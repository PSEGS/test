using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using AdmissionPortal_API.Utility.PaymentFunction;
using System.Security.Claims;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Service.ServiceInterface;
using System.Net;
using AdmissionPortal_API.Utility.EncryptDecrypt;

namespace AdmissionPortal_API.Web.Controllers
{

    [Route("api/HDFC")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class HdfcPaymentController : Controller
    {
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        private readonly IHdfcService _hdfcService;
        public HdfcPaymentController(ILogError logError, IConfiguration configuration, IHdfcService hdfcService)
        {
            _logError = logError;
            _configuration = configuration;
            _hdfcService = hdfcService;
        }

        /// <summary>
        /// Payment API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ApplicationPayment")]
        public async Task<IActionResult> Payment()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            ServiceResult data = new ServiceResult();
            string StudentType = string.Empty;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                PaymentStudentModel model = new PaymentStudentModel();
                model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                model.StudentType = identity.FindFirst("loginType").Value;
                model.Name = identity.FindFirst("Name").Value;
                model.Phone = identity.FindFirst("Phone").Value;
                model.Email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                model.Note = "Registration Fee";

                var AppFormAmount = _configuration.GetValue<string>("HDFC:AppFormAmount");
                data = await _hdfcService.MakeHdfcPayment(AppFormAmount, model);
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
                data.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                return await Task.Run(() => Unauthorized(data));
            }
        }


        /// <summary>
        /// Payment Response Callback API for UG
        /// </summary>
        /// <param name = "response" ></ param >
        /// < returns ></ returns >
        [AllowAnonymous]
        [HttpPost]
        [Route("PaymentResponse")]
        public async Task<IActionResult> PaymentResponse(string response)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                var _collection = HttpContext.Request.Form;

                data = await _hdfcService.PaymentResponse(_collection);
                var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AppReturnUrl");
                var _txnid = Convert.ToString(Encryption.EncodeStringToHexa(_collection["txnid"]));
                return RedirectPermanent(PAYU_ReturnUrl + _txnid);
                // return await Task.Run(() => Ok(data));

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get UG Payment Detail API 
        /// </summary>
        /// <param name = "txnid" ></ param >
        /// < returns ></ returns >
        [HttpGet]
        [Route("HdfcPaymentDetail/{txnid}")]
        public async Task<IActionResult> HdfcPaymentDetail(string txnid)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);

                ServiceResult data = await _hdfcService.HdfcPaymentDetail(txnid, StudentId);
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
                _logError.WriteTextToFile("Hdfc Payment Detail : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Payment Verification API 
        /// </summary>
        /// <param name = "obj" ></ param >
        /// < returns ></ returns >
        [HttpPost]
        [Route("verifyPayment")]
        public async Task<IActionResult> verifyPayment(Hdfc_verifyPayment obj)
        {

            if (!ModelState.IsValid)
            {
                return await Task.Run(() => BadRequest(ModelState));
            }


            var identity = HttpContext.User.Identity as ClaimsIdentity;

            IEnumerable<Claim> claims = identity.Claims;
            PaymentStudentModel model = new PaymentStudentModel();
            model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
            model.StudentType = identity.FindFirst("loginType").Value;
            model.Name = identity.FindFirst("Name").Value;
            model.Phone = identity.FindFirst("Phone").Value;
            model.Email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            model.Note = "Registration Fee";

            ServiceResult data = new ServiceResult();
            string StudentType = string.Empty;
            if (identity != null)
            {
                data = await _hdfcService.VerifyPayment(obj.trxId, model);

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
                data.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                return await Task.Run(() => Unauthorized(data));
            }
        }


        [HttpGet]
        [Route("ApplicationPaymentPG")]
        public async Task<IActionResult> PaymentPG()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            ServiceResult data = new ServiceResult();
            string StudentType = string.Empty;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                PaymentStudentModel model = new PaymentStudentModel();
                model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                model.StudentType = identity.FindFirst("loginType").Value;
                model.Name = identity.FindFirst("Name").Value;
                model.Phone = identity.FindFirst("Phone").Value;
                model.Email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                model.Note = "Registration Fee";

                var AppFormAmount = _configuration.GetValue<string>("HDFC:AppFormAmount");
                data = await _hdfcService.MakeHdfcPaymentPG(AppFormAmount, model);
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
                data.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                return await Task.Run(() => Unauthorized(data));
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("PaymentResponsePG")]
        public async Task<IActionResult> PaymentResponsePG()
        {
            var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AppReturnUrlPG");
            try
            {
                ServiceResult data = new ServiceResult();
                var _collection = HttpContext.Request.Form;
                data = await _hdfcService.PaymentResponsePG(_collection);
                var _txnid = Convert.ToString(Encryption.EncodeStringToHexa(_collection["txnid"]));
                return RedirectPermanent(PAYU_ReturnUrl + _txnid);
                // return await Task.Run(() => Ok(data));

            }
            catch (Exception ex)
            {

                return RedirectPermanent(PAYU_ReturnUrl + 0);
            }
        }

        [HttpGet]
        [Route("HdfcPaymentDetailPG/{txnid}")]
        public async Task<IActionResult> HdfcPaymentDetailPG(string txnid)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                ServiceResult data = await _hdfcService.HdfcPaymentDetailPG(txnid, StudentId);
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
                _logError.WriteTextToFile("Hdfc Payment Detail PG: ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Payment Verification API without Authentication
        /// </summary>
        /// <param name = "obj" ></ param >
        /// < returns ></ returns >
        [AllowAnonymous]
        [HttpPost]
        [Route("Publicly_verifyPayment")]
        public async Task<IActionResult> PubliclyVerifyPayment(Hdfc_Public_verifyPayment obj)
        {

            if (!ModelState.IsValid)
            {
                return await Task.Run(() => BadRequest(ModelState));
            }
            ServiceResult data = new ServiceResult();
            string StudentType = string.Empty;
            data = await _hdfcService.PubliclyVerifyPayment(obj.trxId);
            if (data.Status == true)
            {
                return await Task.Run(() => Ok(data));
            }
            else
            {
                return await Task.Run(() => BadRequest(data));
            }
        }
        ///// <summary>
        /// Payment Verification API without Authentication
        /// </summary>
        /// <param name = "obj" ></ param >
        /// < returns ></ returns >
        [AllowAnonymous]
        [HttpPost]
        [Route("Publicly_verifyPayment_PG")]
        public async Task<IActionResult> PubliclyVerifyPaymentPG(Hdfc_Public_verifyPayment obj)
        {

            if (!ModelState.IsValid)
            {
                return await Task.Run(() => BadRequest(ModelState));
            }
            ServiceResult data = new ServiceResult();
            string StudentType = string.Empty;
            data = await _hdfcService.PubliclyVerifyPaymentPG(obj.trxId);
            if (data.Status == true)
            {
                return await Task.Run(() => Ok(data));
            }
            else
            {
                return await Task.Run(() => BadRequest(data));
            }
        }

        //------------------------------------Admission Methods-------------------------------------------

        /// <summary>
        /// Payment API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AdmissionPaymentUG")]
        public async Task<IActionResult> AdmissionPayment([FromBody]AdmissionPaymentModel _model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            ServiceResult data = new ServiceResult();
            string StudentType = string.Empty;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                HdfcStudentAdmissionModel model = new HdfcStudentAdmissionModel();
                model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                model.CollegeId = _model.CollegeId;
                model.CourseId = _model.CourseId;
                model.CourseType = _model.CourseType;
                model.StudentType = identity.FindFirst("loginType").Value;
                model.Name = identity.FindFirst("Name").Value;
                model.Phone = identity.FindFirst("Phone").Value;
                model.Email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                model.Note = "Course Admission Fee";

                var AppFormAmount = _model.Amount;//_configuration.GetValue<string>("HDFC:AppFormAmount");
                data = await _hdfcService.MakeAdmissionPayment(AppFormAmount, model);
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
                data.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                return await Task.Run(() => Unauthorized(data));
            }
        }

        /// <summary>
        /// Payment Response Callback API for UG
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("AdmissionPaymentResponseUG")]
        public async Task<IActionResult> AdmissionResponse(string response)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                var _collection = HttpContext.Request.Form;

                data = await _hdfcService.AdmissionPaymentResponse(_collection);
                var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AdmissionUGReturnUrl");
                var _txnid = Convert.ToString(Encryption.EncodeStringToHexa(_collection["txnid"]));
                return RedirectPermanent(PAYU_ReturnUrl + _txnid);
                // return await Task.Run(() => Ok(data));

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Payment API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AdmissionPaymentPG")]
        public async Task<IActionResult> AdmissionPaymentPG([FromBody]AdmissionPaymentModel _model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            ServiceResult data = new ServiceResult();
            string StudentType = string.Empty;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                HdfcStudentAdmissionModel model = new HdfcStudentAdmissionModel();
                model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                model.CollegeId = _model.CollegeId;
                model.CourseId = _model.CourseId;
                model.CourseType = _model.CourseType;
                model.StudentType = identity.FindFirst("loginType").Value;
                model.Name = identity.FindFirst("Name").Value;
                model.Phone = identity.FindFirst("Phone").Value;
                model.Email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                model.Note = "Course Admission Fee";

                var AppFormAmount = _model.Amount;//_configuration.GetValue<string>("HDFC:AppFormAmount");
                data = await _hdfcService.MakeAdmissionPaymentPG(AppFormAmount, model);
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
                data.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                return await Task.Run(() => Unauthorized(data));
            }
        }

        /// <summary>
        /// Payment Response Callback API for UG
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("AdmissionPaymentResponsePG")]
        public async Task<IActionResult> AdmissionResponsePG(string response)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                var _collection = HttpContext.Request.Form;

                data = await _hdfcService.AdmissionPaymentResponsePG(_collection);
                var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AdmissionPGReturnUrl");
                var _txnid = Convert.ToString(Encryption.EncodeStringToHexa(_collection["txnid"]));
                return RedirectPermanent(PAYU_ReturnUrl + _txnid);
                // return await Task.Run(() => Ok(data));

            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ReconsileUG")]
        public async Task<IActionResult> ReconsileUG()
        {

            ServiceResult data = new ServiceResult();
            var result = await _hdfcService.ReconsileList("UG");
            List<ReconcileTransaction> _lst = new List<ReconcileTransaction>();
            _lst = result.ResultData;
            foreach (var item in _lst)
            {
                data = await _hdfcService.PubliclyVerifyPayment(item.TransactionId);
            }
            if (data.Status == true)
            {
                return await Task.Run(() => Ok(data));
            }
            else
            {
                return await Task.Run(() => BadRequest(data));
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ReconsilePG")]
        public async Task<IActionResult> ReconsilePG()
        {

            ServiceResult data = new ServiceResult();
            var result=await _hdfcService.ReconsileList("PG");
            List<ReconcileTransaction> _lst = new List<ReconcileTransaction>();
            _lst = result.ResultData;
            foreach (var item in _lst)
            {
                data = await _hdfcService.PubliclyVerifyPaymentPG(item.TransactionId);
            }
            
            if (data.Status == true)
            {
                return await Task.Run(() => Ok(data));
            }
            else
            {
                return await Task.Run(() => BadRequest(data));
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ReconsileUGwithTXNId")]
        public async Task<IActionResult> ReconsileUGwithTXNId([FromBody] ReconcileTransaction reconcile)
        {

            ServiceResult data = new ServiceResult();
            //var result = await _hdfcService.ReconsileList("UG");
            //List<ReconcileTransaction> _lst = new List<ReconcileTransaction>();
            //_lst = result.ResultData;
            //foreach (var item in _lst)
            //{
                data = await _hdfcService.PubliclyVerifyPayment(reconcile.TransactionId);
            //}
            if (data.Status == true)
            {
                return await Task.Run(() => Ok(data));
            }
            else
            {
                return await Task.Run(() => BadRequest(data));
            }
        }

    }
}
