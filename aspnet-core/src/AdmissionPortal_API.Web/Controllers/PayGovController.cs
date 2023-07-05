using AdmissionPortal_API.Domain.ExternalAPI;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/Paygov")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class PayGovController : Controller
    {
        #region GLOBAL DECLARATIONS        

        private readonly IConfiguration _Configuration;
        private readonly ILogError _logError;
        private readonly IPaygovService _paygovservice;
        #endregion

        #region 0.) CONSTRUCTOR
        public PayGovController(ILogError logError, IConfiguration Configuration, IPaygovService paygovservice)
        {
            _logError = logError;
            _Configuration = Configuration;
            _paygovservice = paygovservice;
        }
        #endregion
        #region Citizen Payment Panel
        [HttpGet]
        [Route("RegistrationPayment")]
        public async Task<IActionResult> PayGovRegistrationPayment()
        {
            ServiceResult _OBJResponse = new ServiceResult();
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //BELOW IS PAYGOV PAYNMENT GATEWAY INTEGRATION
                if (identity != null)
                {
                    #region PAYGOV INTEGRATION
                    PaymentStudentModel _model = new PaymentStudentModel();

                    _model.StudentType = identity.FindFirst("loginType").Value;
                    _model.Name = identity.FindFirst("Name").Value;
                    _model.Phone = identity.FindFirst("Phone").Value;
                    _model.Email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                    _model.Note = "Registration Fee";
                    _model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    string transactionAmount = this._Configuration["PayGovSettings:RegistrationAmount"];
                    _OBJResponse = await _paygovservice.MakePaygovRegistrationPayment(transactionAmount, _model);

                    #endregion
                    if (_OBJResponse.Status == true)
                    {
                        return await Task.Run(() => Ok(_OBJResponse));
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(_OBJResponse));
                    }
                }
                else
                {
                    _OBJResponse.Status = false;
                    _OBJResponse.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                    return await Task.Run(() => Unauthorized(_OBJResponse));
                }
            }
            catch (Exception ex)
            {
                _OBJResponse.Status = false;
                _OBJResponse.Message = ex.Message.ToString();
                _OBJResponse.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                return await Task.Run(() => BadRequest(_OBJResponse));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("PaymentRegistrationStatus")]
        public async Task<IActionResult> PaymentRegistrationStatus(string msg)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            PaymentStudentModel _model = new PaymentStudentModel();
            _model.StudentType = "UG";
            ServiceResult _OBJResponse = new ServiceResult();
            string _txnid = string.Empty;
            //F|UATPSGSSG0000001455|PAYGOV8UG3152997119|admission fee|||400|Transaction Cancelled|Transaction Cancelled by User|12-07-2022 10:24:55|2898129546
            //F|UATPSGSSG0000001455|PAYGOV8UG1636504565|||400|PAYMENT_DECLINED_S|Duplicate Order Id|12-07-2022 10:53:10|1615246383
            string[] response = msg.Split('|');
            if (response[0] == "F" || response[0] == "P")
            {
                if (response.Length > 3)
                {
                    _txnid = response[2];
                }
            }
            else
            {
                _txnid = response[4];
            }
            _OBJResponse = await _paygovservice.PaymentRegistrationResponse(msg, _model);
            var PAYU_ReturnUrl = this._Configuration["PayGovSettings:UIReturnURL"];

            if (_OBJResponse.Status)
            {
                _txnid = Convert.ToString(Encryption.EncodeStringToHexa(_txnid));
                return RedirectPermanent(PAYU_ReturnUrl + _txnid);
            }
            else
            {
                _txnid = "0";
                return RedirectPermanent(PAYU_ReturnUrl + _txnid);
                // return await Task.Run(() => Ok(_OBJResponse));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("PaymentRegistrationStatusPG")]
        public async Task<IActionResult> PaymentRegistrationStatusPG(string msg)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            PaymentStudentModel _model = new PaymentStudentModel();
            _model.StudentType = "PG";
            ServiceResult _OBJResponse = new ServiceResult();
            string _txnid = string.Empty;
            string[] response = msg.Split('|');
            if (response[0] == "F" || response[0] == "P")
            {
                if (response.Length > 3)
                {
                    _txnid = response[2];
                }
            }
            else
            {
                _txnid = response[4];
            }
            _OBJResponse = await _paygovservice.PaymentRegistrationResponse(msg, _model);

            string PAYU_ReturnUrl = this._Configuration["PayGovSettings:UIReturnURLPG"];

            if (_OBJResponse.Status)
            {
                _txnid = Convert.ToString(Encryption.EncodeStringToHexa(_txnid));
                return RedirectPermanent(PAYU_ReturnUrl + _txnid);
            }
            else
            {
                _txnid = "0";
                return RedirectPermanent(PAYU_ReturnUrl + _txnid);
                // return await Task.Run(() => Ok(_OBJResponse));
            }
        }
                [HttpGet]
        [Route("PaygovPaymentDetail/{txnid}")]
        public async Task<IActionResult> PaygovPaymentDetail(string txnid)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                PaymentStudentModel _model = new PaymentStudentModel();
                if (identity != null)
                {
                    _model.StudentType = identity.FindFirst("loginType").Value;
                    _model.Name = identity.FindFirst("Name").Value;
                    _model.Phone = identity.FindFirst("Phone").Value;
                    _model.Email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                    _model.Note = "Registration Fee";
                    _model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    //string transactionAmount = this._Configuration["PayGovSettings:RegistrationAmount"];
                }

                ServiceResult data = await _paygovservice.PaygovPaymentDetail(txnid, _model);
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
                _logError.WriteTextToFile("Paygov Payment Detail : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("ReConcilePaymentUG")]
        public async Task<IActionResult> ReConcilePayGovPayment([FromBody] verifyPaymentModel _model)
        {
            ServiceResult data = await _paygovservice.ReConcilePayGovPaymentUG(_model);
            return await Task.Run(() => Ok(data));
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("ReConcilePaymentPG")]
        public async Task<IActionResult> ReConcilePayGovPaymentPG([FromBody] verifyPaymentModel _model)
        {
            ServiceResult data = await _paygovservice.ReConcilePayGovPaymentPG(_model);
            return await Task.Run(() => Ok(data));
        }
    }

    #endregion
}



