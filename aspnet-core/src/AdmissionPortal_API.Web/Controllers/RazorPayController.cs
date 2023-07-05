using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Razorpay.Api;
using System.Security.Claims;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Newtonsoft.Json.Linq;
using AdmissionPortal_API.Domain.Model;
using Microsoft.Extensions.Configuration;
using AdmissionPortal_API.Utility.PaymentFunction;
using Razorpay.Api.Errors;
using Newtonsoft.Json;
using Nancy.Json;
using System.Net;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.MessageConfig;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/razorpay")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class RazorPayController : ControllerBase
    {
        private readonly IRazorPayService _razorPayService;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public RazorPayController(IRazorPayService razorPayService, IConfiguration configuration, ILogError logError)
        {
            _razorPayService = razorPayService;
            _logError = logError;
            _configuration = configuration;
        }

        #region Razor Payment Admission fee UG

        [HttpPost]
        [Route("GenerateOrder")]
        public async Task<IActionResult> GenerateOrder()
        {
            try
            {
                Int32 StudentId = 0;
                string type = "UG";
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    var key = _configuration.GetValue<string>("RazorPay:KEY");
                    var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                    RazorpayClient client = new RazorpayClient(key, SecretKey);
                    var AppFormAmount = _configuration.GetValue<string>("RazorPay:AppFormAmount");

                    //Convert paise into rupies
                    Int32 Amount = Convert.ToInt16(AppFormAmount) * 100;


                    #region Generate randon receipt No
                    string receipt = string.Empty;
                    Random rnd = new Random();
                    string strHash = HdfcPaymentFunction.Generatehash512(StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                    receipt = StudentId + strHash.ToString().Substring(0, 25);
                    #endregion

                    Dictionary<string, object> options = new Dictionary<string, object>();
                    options.Add("amount", Amount); // 1 rs
                    options.Add("receipt", receipt);
                    options.Add("currency", "INR");
                    options.Add("payment_capture", true);
                    Dictionary<string, object> Notesoptions = new Dictionary<string, object>();
                    //options.Add("notes", "{'feeType':'admission fee','name':'Sharma','StudentId':'1'}");
                    Notesoptions.Add("name", identity.FindFirst("Name").Value);
                    Notesoptions.Add("studentId", StudentId);
                    Notesoptions.Add("address", "#123");
                    Notesoptions.Add("contact", identity.FindFirst("Phone").Value);
                    Notesoptions.Add("feeType", "Registration Fee");
                    options.Add("notes", Notesoptions);
                    Order order = client.Order.Create(options);
                    string OrderId = order.Attributes.id;
                    var orderJson = order.Attributes.ToString();
                    ServiceResult data = await _razorPayService.GenerateOrder(StudentId, OrderId, Convert.ToInt16(AppFormAmount), type);
                    data.ResultData = new { OrderId = OrderId, Skey = key };
                    //ServiceResult data = new ServiceResult();
                    //data.Status = true;


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

        [HttpPost]
        [Route("PaymentSuccess")]
        public async Task<IActionResult> PaymentSuccess([FromBody] PaymentSuccess model)
        {
            bool IsVerifyiedPayment = false;
            try
            {
                Int32 StudentId = 0;
                string type = "UG";

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }

                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                Dictionary<string, string> attributes = new Dictionary<string, string>();
                var payload = model.orderId + '|' + model.paymentId;

                Utils.verifyWebhookSignature(payload, model.Signature, SecretKey);

                IsVerifyiedPayment = true;
                var pay = client.Payment.Fetch(model.paymentId);


                int actualamount = pay.Attributes["amount"] / 100;

                RazorRoot root = new RazorRoot();
                root.amount = actualamount;
                // root.acquirer_data = acq;
                root.amount_refunded = pay.Attributes["amount_refunded"];
                root.bank = pay.Attributes["bank"];
                root.captured = pay.Attributes["captured"];
                root.card_id = pay.Attributes["card_id"];
                root.contact = pay.Attributes["contact"];
                root.created_at = pay.Attributes["created_at"];
                root.currency = pay.Attributes["currency"];
                root.description = pay.Attributes["description"];
                root.email = pay.Attributes["email"];
                root.entity = pay.Attributes["entity"];
                root.error_code = pay.Attributes["error_code"];
                root.error_description = pay.Attributes["error_description"];
                root.error_reason = pay.Attributes["error_reason"];
                root.error_source = pay.Attributes["error_source"];
                root.error_step = pay.Attributes["error_step"];
                root.fee = pay.Attributes["fee"];
                root.id = pay.Attributes["id"];
                root.international = pay.Attributes["international"];
                root.invoice_id = pay.Attributes["invoice_id"];
                root.method = pay.Attributes["method"];

                root.order_id = pay.Attributes["order_id"];
                root.refund_status = pay.Attributes["refund_status"];
                root.status = pay.Attributes["status"];
                root.tax = pay.Attributes["tax"];
                root.vpa = pay.Attributes["vpa"];
                root.wallet = pay.Attributes["wallet"];
                root.paymentId = model.paymentId;
                root.Signature = model.Signature;

                root.udf_address = pay.Attributes["notes"]["address"];
                root.udf_contact = pay.Attributes["notes"]["contact"];
                root.udf_feeType = pay.Attributes["notes"]["feeType"];
                root.udf_name = pay.Attributes["notes"]["name"];
                root.udf_studentId = pay.Attributes["notes"]["studentId"];
                root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                root.studentId = StudentId;
                root.paymentResponse = JsonConvert.SerializeObject(root);

                if (ModelState.IsValid)
                {
                    ServiceResult data = await _razorPayService.PaymentSuccess(StudentId, root, type);
                    if (data.Status == true)
                    {
                        // return await Task.Run(() => Ok(data));
                        var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AppReturnUrl");
                        var _txnid = Convert.ToString(Encryption.EncodeStringToHexa(model.orderId));
                        data.ResultData = PAYU_ReturnUrl + _txnid;
                        //return RedirectPermanent(PAYU_ReturnUrl + _txnid);
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
            catch (SignatureVerificationError ex)
            {
                _logError.WriteTextToFile("Registeration Fees Payment SignatureVerificationError: ", ex.Message, ex.HResult, ex.StackTrace);
                IsVerifyiedPayment = false;
                return await Task.Run(() => BadRequest(ex));
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Registeration Fees Payment : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }

        }

        [HttpPost]
        [Route("PaymentFailure")]
        public async Task<IActionResult> PaymentFailure([FromBody] PaymentFailure model)
        {
            try
            {
                Int32 StudentId = 0;
                string type = "UG";
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _razorPayService.PaymentFailure(StudentId, model, type);
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
        [Route("GetStudentPaymentDetail")]
        public async Task<IActionResult> GetStudentPaymentDetail()
        {
            try
            {
                Int32 StudentId = 0;
                string type = "UG";
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _razorPayService.GetStudentPaymentDetail(StudentId, type);
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

        #endregion

        #region Razor Payment Admission fee PG

        [HttpPost]
        [Route("GenerateOrderPG")]
        public async Task<IActionResult> GenerateOrderPG()
        {
            try
            {
                Int32 StudentId = 0;
                string type = "PG";
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    var key = _configuration.GetValue<string>("RazorPay:KEY");
                    var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                    RazorpayClient client = new RazorpayClient(key, SecretKey);
                    var AppFormAmount = _configuration.GetValue<string>("RazorPay:AppFormAmount");

                    // Convert paise into rupies
                    Int32 Amount = Convert.ToInt16(AppFormAmount) * 100;


                    #region Generate randon receipt No
                    string receipt = string.Empty;
                    Random rnd = new Random();
                    string strHash = HdfcPaymentFunction.Generatehash512(StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                    receipt = StudentId + strHash.ToString().Substring(0, 25);
                    #endregion

                    Dictionary<string, object> options = new Dictionary<string, object>();
                    options.Add("amount", Amount); // 1 rs
                    options.Add("receipt", receipt);
                    options.Add("currency", "INR");
                    options.Add("payment_capture", true);
                    Dictionary<string, object> Notesoptions = new Dictionary<string, object>();
                    //options.Add("notes", "{'feeType':'admission fee','name':'Sharma','StudentId':'1'}");
                    Notesoptions.Add("name", identity.FindFirst("Name").Value);
                    Notesoptions.Add("studentId", StudentId);
                    Notesoptions.Add("address", "#123");
                    Notesoptions.Add("contact", identity.FindFirst("Phone").Value);
                    Notesoptions.Add("feeType", "Registration Fee");
                    options.Add("notes", Notesoptions);
                    Order order = client.Order.Create(options);
                    string OrderId = order.Attributes.id;
                    var orderJson = order.Attributes.ToString();
                    ServiceResult data = await _razorPayService.GenerateOrderPG(StudentId, OrderId, Convert.ToInt16(AppFormAmount), type);
                    data.ResultData = new { OrderId = OrderId, Skey = key };
                    //ServiceResult data = new ServiceResult();
                    data.Status = true;


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

        [HttpPost]
        [Route("PaymentSuccessPG")]
        public async Task<IActionResult> PaymentSuccessPG([FromBody] PaymentSuccess model)
        {
            bool IsVerifyiedPayment = false;
            try
            {
                Int32 StudentId = 0;
                string type = "PG";
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                Dictionary<string, string> attributes = new Dictionary<string, string>();
                var payload = model.orderId + '|' + model.paymentId;

                Utils.verifyWebhookSignature(payload, model.Signature, SecretKey);

                IsVerifyiedPayment = true;
                var pay = client.Payment.Fetch(model.paymentId);
                int actualamount = pay.Attributes["amount"] / 100;

                RazorRoot root = new RazorRoot();
                root.amount = actualamount;
                // root.acquirer_data = acq;
                root.amount_refunded = pay.Attributes["amount_refunded"];
                root.bank = pay.Attributes["bank"];
                root.captured = pay.Attributes["captured"];
                root.card_id = pay.Attributes["card_id"];
                root.contact = pay.Attributes["contact"];
                root.created_at = pay.Attributes["created_at"];
                root.currency = pay.Attributes["currency"];
                root.description = pay.Attributes["description"];
                root.email = pay.Attributes["email"];
                root.entity = pay.Attributes["entity"];
                root.error_code = pay.Attributes["error_code"];
                root.error_description = pay.Attributes["error_description"];
                root.error_reason = pay.Attributes["error_reason"];
                root.error_source = pay.Attributes["error_source"];
                root.error_step = pay.Attributes["error_step"];
                root.fee = pay.Attributes["fee"];
                root.id = pay.Attributes["id"];
                root.international = pay.Attributes["international"];
                root.invoice_id = pay.Attributes["invoice_id"];
                root.method = pay.Attributes["method"];

                root.order_id = pay.Attributes["order_id"];
                root.refund_status = pay.Attributes["refund_status"];
                root.status = pay.Attributes["status"];
                root.tax = pay.Attributes["tax"];
                root.vpa = pay.Attributes["vpa"];
                root.wallet = pay.Attributes["wallet"];
                root.paymentId = model.paymentId;
                root.Signature = model.Signature;

                root.udf_address = pay.Attributes["notes"]["address"];
                root.udf_contact = pay.Attributes["notes"]["contact"];
                root.udf_feeType = pay.Attributes["notes"]["feeType"];
                root.udf_name = pay.Attributes["notes"]["name"];
                root.udf_studentId = pay.Attributes["notes"]["studentId"];
                root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                root.studentId = StudentId;
                root.paymentResponse = JsonConvert.SerializeObject(root);

                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _razorPayService.PaymentSuccessPG(StudentId, root, type);
                    if (data.Status == true)
                    {

                        var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AppReturnUrlPG");
                        var _txnid = Convert.ToString(Encryption.EncodeStringToHexa(model.orderId));
                        data.ResultData = PAYU_ReturnUrl + _txnid;
                        //return RedirectPermanent(PAYU_ReturnUrl + _txnid);
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
            catch (SignatureVerificationError ex)
            {
                _logError.WriteTextToFile("Registeration Fees Payment PG SignatureVerificationError: ", ex.Message, ex.HResult, ex.StackTrace);
                IsVerifyiedPayment = false;
                return await Task.Run(() => BadRequest(ex));
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Registeration Fees Payment PG : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }

        }

        [HttpPost]
        [Route("PaymentFailurePG")]
        public async Task<IActionResult> PaymentFailurePG([FromBody] PaymentFailure model)
        {
            try
            {
                Int32 StudentId = 0;
                string type = "PG";
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _razorPayService.PaymentFailurePG(StudentId, model, type);
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
        [Route("GetStudentPaymentDetailPG")]
        public async Task<IActionResult> GetStudentPaymentDetailPG()
        {
            try
            {
                Int32 StudentId = 0;
                string type = "PG";
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    }
                    ServiceResult data = await _razorPayService.GetStudentPaymentDetailPG(StudentId, type);
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

        #endregion


        #region Admission Fee Methods
        //------------------------------------Admission Methods-------------------------------------------

        /// <summary>
        /// Payment API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AdmissionPaymentUG")]
        public async Task<IActionResult> AdmissionPayment([FromBody] AdmissionPaymentModel _model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            ServiceResult data = new ServiceResult();
            ServiceResult checkAvailability = new ServiceResult();
            string StudentType = string.Empty;
            string type = "UG";
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                _model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                IciciCollegeAccount account = new IciciCollegeAccount();
                account.CollegeId = _model.CollegeId;
                account.AccountType = _model.CourseType;
                checkAvailability = await _razorPayService.CheckSeatAvailability(_model);
                if (checkAvailability != null)
                {
                    if (checkAvailability.ResultData == 1)
                    {
                        string transferAccountNumber = _razorPayService.GetICICICollegeAccount(account).Result.ResultData;
                        if (!string.IsNullOrEmpty(transferAccountNumber))
                        {
                            decimal AppFormAmount = Convert.ToDecimal(_model.Amount);//_configuration.GetValue<string>("HDFC:AppFormAmount");
                                                                                     //Convert paise into rupies
                            decimal Amount = Convert.ToDecimal(AppFormAmount) * 100;

                            #region Generate randon receipt No
                            string receipt = string.Empty;
                            Random rnd = new Random();

                            string strHash = HdfcPaymentFunction.Generatehash512(_model.StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                            receipt = _model.StudentId + _model.CourseId + _model.CourseId + strHash.ToString().Substring(0, 25);
                            #endregion

                            Dictionary<string, object> options = new Dictionary<string, object>();
                            options.Add("amount", Amount); // 1 rs
                            options.Add("receipt", receipt);
                            options.Add("currency", "INR");
                            options.Add("payment_capture", true);

                            Dictionary<string, object> Notesoptions = new Dictionary<string, object>();
                            //options.Add("notes", "{'feeType':'admission fee','name':'Sharma','StudentId':'1'}");
                            Notesoptions.Add("name", identity.FindFirst("Name").Value);
                            Notesoptions.Add("studentId", _model.StudentId);
                            Notesoptions.Add("collegeId", _model.CollegeId);
                            Notesoptions.Add("courseId", _model.CourseId);
                            Notesoptions.Add("contact", identity.FindFirst("Phone").Value);
                            Notesoptions.Add("feeType", "Course Admission Fee");
                            options.Add("notes", Notesoptions);

                            List<Dictionary<string, object>> ourList = new List<Dictionary<string, object>>();
                            Dictionary<string, object> input1 = new Dictionary<string, object>();
                            input1.Add("account", transferAccountNumber); // this amount should be same as transaction amount
                            input1.Add("currency", "INR");//
                            input1.Add("amount", Amount);
                            input1.Add("on_hold", false);

                            Dictionary<string, object> input2 = new Dictionary<string, object>();
                            input2.Add("branch", "fundType-" + _model.CourseType);
                            input2.Add("name", identity.FindFirst("Name").Value);
                            input2.Add("studentId", _model.StudentId);
                            input2.Add("collegeId", _model.CollegeId);
                            input2.Add("courseId", _model.CourseId);

                            input1.Add("notes", input2);
                            ourList.Add(input1);
                            options.Add("transfers", ourList);


                            Order order = client.Order.Create(options);
                            string OrderId = order.Attributes.id;
                            var orderJson = order.Attributes.ToString();

                            data = await _razorPayService.GenerateOrder(_model.StudentId, OrderId, AppFormAmount, type);
                            data.ResultData = new { OrderId = OrderId, Skey = key };
                            // data.Status = true;
                            if (data.Status == true)
                            {
                                return await Task.Run(() => Ok(data));
                            }
                            else
                            {
                                data.Status = false;
                                return await Task.Run(() => BadRequest(data));
                            }
                        }
                        else
                        {
                            data.Status = false;
                            data.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                            data.Message = "College Account not found";
                            return await Task.Run(() => BadRequest(data));
                        }
                    }
                    else
                    {
                        return await Task.Run(() => Ok(checkAvailability));
                    }
                }
                else
                {
                    checkAvailability.Status = false;
                    checkAvailability.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    checkAvailability.Message = checkAvailability.Message;
                    return await Task.Run(() => BadRequest(checkAvailability));
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
        [Route("AdmissionPaymentSuccessUG")]
        public async Task<IActionResult> AdmissionResponse([FromBody] PaymentSuccess model)
        {
            bool IsVerifyiedPayment = false;
            try
            {
                Int32 StudentId = 0;
                string type = "UG";

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }

                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                Dictionary<string, string> attributes = new Dictionary<string, string>();
                var payload = model.orderId + '|' + model.paymentId;

                Utils.verifyWebhookSignature(payload, model.Signature, SecretKey);

                IsVerifyiedPayment = true;
                Payment pay = null;
                try
                {
                    pay = client.Payment.Fetch(model.paymentId);
                }
                catch (Exception ex)
                {
                    pay = client.Payment.Fetch(model.paymentId);
                }
                if (pay == null)
                {
                    pay = client.Payment.Fetch(model.paymentId);
                }
                else
                {

                    pay = client.Payment.Fetch(model.paymentId);
                }

                decimal actualamount = pay.Attributes["amount"] / 100;

                RazorRoot root = new RazorRoot();
                root.amount = actualamount;
                // root.acquirer_data = acq;
                root.amount_refunded = pay.Attributes["amount_refunded"];
                root.bank = pay.Attributes["bank"];
                root.captured = pay.Attributes["captured"];
                root.card_id = pay.Attributes["card_id"];
                root.contact = pay.Attributes["contact"];
                root.created_at = pay.Attributes["created_at"];
                root.currency = pay.Attributes["currency"];
                root.description = pay.Attributes["description"];
                root.email = pay.Attributes["email"];
                root.entity = pay.Attributes["entity"];
                root.error_code = pay.Attributes["error_code"];
                root.error_description = pay.Attributes["error_description"];
                root.error_reason = pay.Attributes["error_reason"];
                root.error_source = pay.Attributes["error_source"];
                root.error_step = pay.Attributes["error_step"];
                root.fee = pay.Attributes["fee"];
                root.id = pay.Attributes["id"];
                root.international = pay.Attributes["international"];
                root.invoice_id = pay.Attributes["invoice_id"];
                root.method = pay.Attributes["method"];

                root.order_id = pay.Attributes["order_id"];
                root.refund_status = pay.Attributes["refund_status"];
                root.status = pay.Attributes["status"];
                root.tax = pay.Attributes["tax"];
                root.vpa = pay.Attributes["vpa"];
                root.wallet = pay.Attributes["wallet"];
                root.paymentId = model.paymentId;
                root.Signature = model.Signature;

                root.udf_address = pay.Attributes["notes"]["address"];
                root.udf_contact = pay.Attributes["notes"]["contact"];
                root.udf_feeType = pay.Attributes["notes"]["feeType"];
                root.udf_name = pay.Attributes["notes"]["name"];
                root.udf_studentId = pay.Attributes["notes"]["studentId"];
                root.collegeId = pay.Attributes["notes"]["collegeId"];
                root.courseId = pay.Attributes["notes"]["courseId"];
                root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                root.studentId = StudentId;
                root.paymentResponse = JsonConvert.SerializeObject(root);

                if (ModelState.IsValid)
                {

                    ServiceResult data = await _razorPayService.PaymentSuccess(StudentId, root, type);
                    if (data.Status == true)
                    {

                        //var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AppReturnUrl");
                        var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AdmissionUGReturnUrl");
                        var _txnid = Convert.ToString(Encryption.EncodeStringToHexa(model.orderId));
                        data.ResultData = PAYU_ReturnUrl + _txnid;
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
                // return await Task.Run(() => Ok(data));

            }
            catch (SignatureVerificationError ex)
            {
                _logError.WriteTextToFile("Admission Payment SuccessUG SignatureVerificationError: ", ex.Message, ex.HResult, ex.StackTrace);
                IsVerifyiedPayment = false;
                return await Task.Run(() => BadRequest(ex));
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
        public async Task<IActionResult> AdmissionPaymentPG([FromBody] AdmissionPaymentModel _model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string type = "PG";
            ServiceResult data = new ServiceResult();
            ServiceResult checkAvailability = new ServiceResult();
            string StudentType = string.Empty;
            try
            {


                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var key = _configuration.GetValue<string>("RazorPay:KEY");
                    var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                    RazorpayClient client = new RazorpayClient(key, SecretKey);
                    _model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    IciciCollegeAccount account = new IciciCollegeAccount();
                    account.CollegeId = _model.CollegeId;
                    account.AccountType = _model.CourseType;

                    checkAvailability = await _razorPayService.CheckSeatAvailabilityPG(_model);
                    if (checkAvailability != null)
                    {
                        if (checkAvailability.ResultData == 1)
                        {
                            string transferAccountNumber = _razorPayService.GetICICICollegeAccount(account).Result.ResultData;
                            if (!string.IsNullOrEmpty(transferAccountNumber))
                            {
                                decimal AppFormAmount = Convert.ToDecimal(_model.Amount);
                                decimal Amount = Convert.ToDecimal(AppFormAmount) * 100;


                                #region Generate randon receipt No
                                string receipt = string.Empty;
                                Random rnd = new Random();
                                _model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                                string strHash = HdfcPaymentFunction.Generatehash512(_model.StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                                receipt = _model.StudentId + _model.CourseId + _model.CourseId + strHash.ToString().Substring(0, 25);
                                #endregion

                                Dictionary<string, object> options = new Dictionary<string, object>();
                                options.Add("amount", Amount); // 1 rs
                                options.Add("receipt", receipt);
                                options.Add("currency", "INR");
                                options.Add("payment_capture", true);


                                Dictionary<string, object> Notesoptions = new Dictionary<string, object>();
                                //options.Add("notes", "{'feeType':'admission fee','name':'Sharma','StudentId':'1'}");
                                Notesoptions.Add("name", identity.FindFirst("Name").Value);
                                Notesoptions.Add("studentId", _model.StudentId);
                                Notesoptions.Add("collegeId", _model.CollegeId);
                                Notesoptions.Add("courseId", _model.CourseId);
                                Notesoptions.Add("contact", identity.FindFirst("Phone").Value);
                                Notesoptions.Add("feeType", "Course Admission Fee");
                                options.Add("notes", Notesoptions);

                                List<Dictionary<string, object>> ourList = new List<Dictionary<string, object>>();
                                Dictionary<string, object> input1 = new Dictionary<string, object>();
                                input1.Add("account", transferAccountNumber); // this amount should be same as transaction amount
                                input1.Add("currency", "INR");//
                                input1.Add("amount", Amount);
                                input1.Add("on_hold", false);

                                Dictionary<string, object> input2 = new Dictionary<string, object>();
                                input2.Add("branch", "fundType-" + _model.CourseType);
                                input2.Add("name", identity.FindFirst("Name").Value);
                                input2.Add("studentId", _model.StudentId);
                                input2.Add("collegeId", _model.CollegeId);
                                input2.Add("courseId", _model.CourseId);

                                input1.Add("notes", input2);
                                ourList.Add(input1);
                                options.Add("transfers", ourList);

                                Order order = client.Order.Create(options);
                                string OrderId = order.Attributes.id;
                                var orderJson = order.Attributes.ToString();

                                data = await _razorPayService.GenerateOrderPG(_model.StudentId, OrderId, AppFormAmount, type);
                                data.ResultData = new { OrderId = OrderId, Skey = key };
                                if (data.Status == true)
                                {
                                    return await Task.Run(() => Ok(data));
                                }
                                else
                                {
                                    data.Status = false;
                                    return await Task.Run(() => BadRequest(data));
                                }
                            }
                            else
                            {
                                data.Status = false;
                                data.Message = "College Account not found";
                                data.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                                return await Task.Run(() => BadRequest(data));
                            }
                        }
                        else
                        {
                            return await Task.Run(() => Ok(checkAvailability));
                        }
                    }
                    else
                    {
                        checkAvailability.Status = false;
                        checkAvailability.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                        checkAvailability.Message = checkAvailability.Message;
                        return await Task.Run(() => BadRequest(checkAvailability));
                    }
                }
                else
                {
                    data.Status = false;
                    data.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                    return await Task.Run(() => Unauthorized(data));
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Payment Response Callback API for UG
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("AdmissionPaymentSuccessPG")]
        public async Task<IActionResult> AdmissionResponsePG([FromBody] PaymentSuccess model)
        {
            bool IsVerifyiedPayment = false;
            string type = "PG";
            try
            {

                int StudentId = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                }
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                Dictionary<string, string> attributes = new Dictionary<string, string>();
                var payload = model.orderId + '|' + model.paymentId;

                Utils.verifyWebhookSignature(payload, model.Signature, SecretKey);


                IsVerifyiedPayment = true;
                Payment pay = null;
                try
                {
                    pay = client.Payment.Fetch(model.paymentId);
                }
                catch (Exception ex)
                {
                    pay = client.Payment.Fetch(model.paymentId);
                }
                if (pay == null)
                {
                    pay = client.Payment.Fetch(model.paymentId);
                }
                else
                {

                    pay = client.Payment.Fetch(model.paymentId);
                }

                decimal actualamount = pay.Attributes["amount"] / 100;

                RazorRoot root = new RazorRoot();
                root.amount = actualamount;
                // root.acquirer_data = acq;
                root.amount_refunded = pay.Attributes["amount_refunded"];
                root.bank = pay.Attributes["bank"];
                root.captured = pay.Attributes["captured"];
                root.card_id = pay.Attributes["card_id"];
                root.contact = pay.Attributes["contact"];
                root.created_at = pay.Attributes["created_at"];
                root.currency = pay.Attributes["currency"];
                root.description = pay.Attributes["description"];
                root.email = pay.Attributes["email"];
                root.entity = pay.Attributes["entity"];
                root.error_code = pay.Attributes["error_code"];
                root.error_description = pay.Attributes["error_description"];
                root.error_reason = pay.Attributes["error_reason"];
                root.error_source = pay.Attributes["error_source"];
                root.error_step = pay.Attributes["error_step"];
                root.fee = pay.Attributes["fee"];
                root.id = pay.Attributes["id"];
                root.international = pay.Attributes["international"];
                root.invoice_id = pay.Attributes["invoice_id"];
                root.method = pay.Attributes["method"];

                root.order_id = pay.Attributes["order_id"];
                root.refund_status = pay.Attributes["refund_status"];
                root.status = pay.Attributes["status"];
                root.tax = pay.Attributes["tax"];
                root.vpa = pay.Attributes["vpa"];
                root.wallet = pay.Attributes["wallet"];
                root.paymentId = model.paymentId;
                root.Signature = model.Signature;

                root.udf_address = pay.Attributes["notes"]["address"];
                root.udf_contact = pay.Attributes["notes"]["contact"];
                root.udf_feeType = pay.Attributes["notes"]["feeType"];
                root.udf_name = pay.Attributes["notes"]["name"];
                root.udf_studentId = pay.Attributes["notes"]["studentId"];
                root.collegeId = pay.Attributes["notes"]["collegeId"];
                root.courseId = pay.Attributes["notes"]["courseId"];
                root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                root.studentId = StudentId;
                root.paymentResponse = JsonConvert.SerializeObject(root);
                // return await Task.Run(() => Ok(data));

                if (ModelState.IsValid)
                {

                    ServiceResult data = await _razorPayService.PaymentSuccessPG(StudentId, root, type);
                    if (data.Status == true)
                    {

                        //var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AppReturnUrlPG");
                        var PAYU_ReturnUrl = _configuration.GetValue<string>("HDFC:AdmissionPGReturnUrl");
                        var _txnid = Convert.ToString(Encryption.EncodeStringToHexa(model.orderId));
                        data.ResultData = PAYU_ReturnUrl + _txnid;
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
            catch (SignatureVerificationError ex)
            {
                _logError.WriteTextToFile("Admission Payment Success PG SignatureVerificationError: ", ex.Message, ex.HResult, ex.StackTrace);
                IsVerifyiedPayment = false;
                return await Task.Run(() => BadRequest(ex));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Reconcile Payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("FetchPaymentUG")]
        public async Task<IActionResult> FetchPaymentUG([FromBody] FetchPayment model)
        {
            string errorMessage = string.Empty;
            try
            {
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                // Payment pay = client.Payment.Fetch(model.paymentId);
                Payment pay = null;
                var _resultPayment = client.Order.Fetch(model.paymentId).Payments().ToList();//.Where(x=>x.Attributes.status== "captured").FirstOrDefault();
                if (_resultPayment.Count > 1)
                {
                    pay = _resultPayment.Where(x => x.Attributes.status == "captured" && x.Attributes.description== "Registration Fee").FirstOrDefault();
                }
                else
                {
                    pay = client.Order.Fetch(model.paymentId).Payments().Where(x=>x.Attributes.status== "captured" && x.Attributes.description == "Registration Fee").FirstOrDefault();
                }
                //Payment pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                if (pay != null)
                {
                    RazorRoot root = new RazorRoot();
                    int actualamount = pay.Attributes["amount"] / 100;
                    root.amount = actualamount;
                    root.amount_refunded = pay.Attributes["amount_refunded"];
                    root.bank = pay.Attributes["bank"];
                    root.captured = pay.Attributes["captured"];
                    root.card_id = pay.Attributes["card_id"];
                    root.contact = pay.Attributes["contact"];
                    root.created_at = pay.Attributes["created_at"];
                    root.currency = pay.Attributes["currency"];
                    root.description = pay.Attributes["description"];
                    root.email = pay.Attributes["email"];
                    root.entity = pay.Attributes["entity"];
                    root.error_code = pay.Attributes["error_code"];
                    root.error_description = pay.Attributes["error_description"];
                    root.error_reason = pay.Attributes["error_reason"];
                    root.error_source = pay.Attributes["error_source"];
                    root.error_step = pay.Attributes["error_step"];
                    root.fee = string.IsNullOrEmpty(Convert.ToString(pay.Attributes["fee"])) ? 0 : pay.Attributes["fee"];
                    root.id = pay.Attributes["id"];
                    root.international = pay.Attributes["international"];
                    root.invoice_id = pay.Attributes["invoice_id"];
                    root.method = pay.Attributes["method"];

                    root.order_id = pay.Attributes["order_id"];
                    root.refund_status = pay.Attributes["refund_status"];
                    root.status = pay.Attributes["status"];
                    root.tax = string.IsNullOrEmpty(Convert.ToString(pay.Attributes["tax"])) ? 0 : pay.Attributes["tax"];
                    root.vpa = pay.Attributes["vpa"];
                    root.wallet = pay.Attributes["wallet"];
                    root.paymentId = model.paymentId;

                    root.udf_address = pay.Attributes["notes"]["address"];
                    root.udf_contact = pay.Attributes["notes"]["contact"];
                    root.udf_feeType = pay.Attributes["notes"]["feeType"];
                    root.udf_name = pay.Attributes["notes"]["name"];
                    root.udf_studentId = pay.Attributes["notes"]["studentId"];
                    root.collegeId = pay.Attributes["notes"]["collegeId"];
                    root.courseId = pay.Attributes["notes"]["courseId"];
                    root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                    root.bank_transaction_id = pay.Attributes["acquirer_data"]["bank_transaction_id"];
                    root.studentId = pay.Attributes["notes"]["studentId"];
                    root.paymentResponse = JsonConvert.SerializeObject(root);


                    #region Reconcil log
                    UgPaymentLog _logModel = new UgPaymentLog();
                    _logModel.PaymentMethod = "RazorPay";
                    _logModel.PaymentRequest = model.paymentId;
                    _logModel.PaymentRequestDate = DateTime.Now;
                    _logModel.PaymentResponse = null;
                    _logModel.PaymentResponseDate = null;
                    _logModel.PaymentStatus = false;
                    _logModel.StudentId = root.studentId;
                    _logModel.TransactionId = root.order_id;
                    _logModel.HashForm = "";
                    _logModel.PaymentMessage = "Pending";
                    var _logresponse = _razorPayService.FetchPaymentLogUG(_logModel);
                    #endregion



                    ServiceResult data = await _razorPayService.FetchPaymentUG(root);
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    ServiceResult data = new ServiceResult();
                    data.Status = false;
                    data.Message = Convert.ToString(MessageConfig.ReconcilePaymentFailed);
                    data.ResultData = null;
                    data.StatusCode = 200;
                    return await Task.Run(() => Ok(data));
                }
            }
            catch (Exception ex)
            {
                ServiceResult data = new ServiceResult();
                data.Status = false;
                data.Message = Convert.ToString(ex.Message);
                data.ResultData = null;
                data.StatusCode = 200;
                return await Task.Run(() => Ok(data));
                //if (Convert.ToString(ex) == "The id provided does not exist")
                //{
                //    ServiceResult data = new ServiceResult();
                //    data.Status = false;
                //    data.Message = "The id provided does not exist";
                //    data.ResultData = null;
                //    data.StatusCode = 200;
                //    return await Task.Run(() => Ok(data));

                //}
                //else
                //{
                //    return await Task.Run(() => BadRequest(ex));

                //}
            }

        }
        /// <summary>
        /// Reconcile Payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("FetchAdmissionPaymentUG")]
        public async Task<IActionResult> FetchAdmissionPaymentUG([FromBody] FetchPayment model)
        {
            string errorMessage = string.Empty;
            try
            {
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                // Payment pay = client.Payment.Fetch(model.paymentId);
                Payment pay = null;
                var _resultPayment = client.Order.Fetch(model.paymentId).Payments().ToList();//.Where(x=>x.Attributes.status== "captured").FirstOrDefault();
                if (_resultPayment.Count > 1)
                {
                    pay = _resultPayment.Where(x => x.Attributes.status == "captured" && x.Attributes.description == "Course Admission Fee").FirstOrDefault();
                }
                else
                {
                    pay = client.Order.Fetch(model.paymentId).Payments().Where(x => x.Attributes.status == "captured" && x.Attributes.description == "Course Admission Fee").FirstOrDefault();
                }
                //Payment pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                if (pay != null)
                {
                    RazorRoot root = new RazorRoot();
                    int actualamount = pay.Attributes["amount"] / 100;
                    root.amount = actualamount;
                    root.amount_refunded = pay.Attributes["amount_refunded"];
                    root.bank = pay.Attributes["bank"];
                    root.captured = pay.Attributes["captured"];
                    root.card_id = pay.Attributes["card_id"];
                    root.contact = pay.Attributes["contact"];
                    root.created_at = pay.Attributes["created_at"];
                    root.currency = pay.Attributes["currency"];
                    root.description = pay.Attributes["description"];
                    root.email = pay.Attributes["email"];
                    root.entity = pay.Attributes["entity"];
                    root.error_code = pay.Attributes["error_code"];
                    root.error_description = pay.Attributes["error_description"];
                    root.error_reason = pay.Attributes["error_reason"];
                    root.error_source = pay.Attributes["error_source"];
                    root.error_step = pay.Attributes["error_step"];
                    root.fee = string.IsNullOrEmpty(Convert.ToString(pay.Attributes["fee"])) ? 0 : pay.Attributes["fee"];
                    root.id = pay.Attributes["id"];
                    root.international = pay.Attributes["international"];
                    root.invoice_id = pay.Attributes["invoice_id"];
                    root.method = pay.Attributes["method"];

                    root.order_id = pay.Attributes["order_id"];
                    root.refund_status = pay.Attributes["refund_status"];
                    root.status = pay.Attributes["status"];
                    root.tax = string.IsNullOrEmpty(Convert.ToString(pay.Attributes["tax"])) ? 0 : pay.Attributes["tax"];
                    root.vpa = pay.Attributes["vpa"];
                    root.wallet = pay.Attributes["wallet"];
                    root.paymentId = model.paymentId;

                    root.udf_address = pay.Attributes["notes"]["address"];
                    root.udf_contact = pay.Attributes["notes"]["contact"];
                    root.udf_feeType = pay.Attributes["notes"]["feeType"];
                    root.udf_name = pay.Attributes["notes"]["name"];
                    root.udf_studentId = pay.Attributes["notes"]["studentId"];
                    root.collegeId = pay.Attributes["notes"]["collegeId"];
                    root.courseId = pay.Attributes["notes"]["courseId"];
                    root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                    root.bank_transaction_id = pay.Attributes["acquirer_data"]["bank_transaction_id"];
                    root.studentId = pay.Attributes["notes"]["studentId"];
                    root.paymentResponse = JsonConvert.SerializeObject(root);


                    #region Reconcil log
                    UgPaymentLog _logModel = new UgPaymentLog();
                    _logModel.PaymentMethod = "RazorPay";
                    _logModel.PaymentRequest = model.paymentId;
                    _logModel.PaymentRequestDate = DateTime.Now;
                    _logModel.PaymentResponse = null;
                    _logModel.PaymentResponseDate = null;
                    _logModel.PaymentStatus = false;
                    _logModel.StudentId = root.studentId;
                    _logModel.TransactionId = root.order_id;
                    _logModel.HashForm = "";
                    _logModel.PaymentMessage = "Pending";
                    var _logresponse = _razorPayService.FetchPaymentLogUG(_logModel);
                    #endregion



                    ServiceResult data = await _razorPayService.FetchPaymentUG(root);
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    ServiceResult data = new ServiceResult();
                    data.Status = false;
                    data.Message = Convert.ToString(MessageConfig.ReconcilePaymentFailed);
                    data.ResultData = null;
                    data.StatusCode = 200;
                    return await Task.Run(() => Ok(data));
                }
            }
            catch (Exception ex)
            {
                ServiceResult data = new ServiceResult();
                data.Status = false;
                data.Message = Convert.ToString(ex.Message);
                data.ResultData = null;
                data.StatusCode = 200;
                return await Task.Run(() => Ok(data));
                //if (Convert.ToString(ex) == "The id provided does not exist")
                //{
                //    ServiceResult data = new ServiceResult();
                //    data.Status = false;
                //    data.Message = "The id provided does not exist";
                //    data.ResultData = null;
                //    data.StatusCode = 200;
                //    return await Task.Run(() => Ok(data));

                //}
                //else
                //{
                //    return await Task.Run(() => BadRequest(ex));

                //}
            }

        }


        [AllowAnonymous]
        [HttpPost]
        [Route("FetchPaymentPG")]
        public async Task<IActionResult> FetchPaymentPG([FromBody] FetchPayment model)
        {
            try
            {
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
               // Payment pay = client.Payment.Fetch(model.paymentId);
              //  Payment pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                Payment pay = null;
                var _resultPayment = client.Order.Fetch(model.paymentId).Payments().ToList();//.Where(x=>x.Attributes.status== "captured").FirstOrDefault();
                if (_resultPayment.Count > 1)
                {
                    pay = _resultPayment.Where(x => x.Attributes.status == "captured" && x.Attributes.description == "Registration Fee").FirstOrDefault();
                }
                else
                {
                    pay = client.Order.Fetch(model.paymentId).Payments().Where(x => x.Attributes.status == "captured" && x.Attributes.description == "Registration Fee").FirstOrDefault();
                    //pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                }
                if (pay != null)
                {
                    RazorRoot root = new RazorRoot();
                    int actualamount = pay.Attributes["amount"] / 100;
                    root.amount = actualamount;
                    root.amount_refunded = pay.Attributes["amount_refunded"];
                    root.bank = pay.Attributes["bank"];
                    root.captured = pay.Attributes["captured"];
                    root.card_id = pay.Attributes["card_id"];
                    root.contact = pay.Attributes["contact"];
                    root.created_at = pay.Attributes["created_at"];
                    root.currency = pay.Attributes["currency"];
                    root.description = pay.Attributes["description"];
                    root.email = pay.Attributes["email"];
                    root.entity = pay.Attributes["entity"];
                    root.error_code = pay.Attributes["error_code"];
                    root.error_description = pay.Attributes["error_description"];
                    root.error_reason = pay.Attributes["error_reason"];
                    root.error_source = pay.Attributes["error_source"];
                    root.error_step = pay.Attributes["error_step"];
                    root.fee = pay.Attributes["fee"];
                    root.id = pay.Attributes["id"];
                    root.international = pay.Attributes["international"];
                    root.invoice_id = pay.Attributes["invoice_id"];
                    root.method = pay.Attributes["method"];

                    root.order_id = pay.Attributes["order_id"];
                    root.refund_status = pay.Attributes["refund_status"];
                    root.status = pay.Attributes["status"];
                    root.tax = pay.Attributes["tax"];
                    root.vpa = pay.Attributes["vpa"];
                    root.wallet = pay.Attributes["wallet"];
                    root.paymentId = model.paymentId;

                    root.udf_address = pay.Attributes["notes"]["address"];
                    root.udf_contact = pay.Attributes["notes"]["contact"];
                    root.udf_feeType = pay.Attributes["notes"]["feeType"];
                    root.udf_name = pay.Attributes["notes"]["name"];
                    root.udf_studentId = pay.Attributes["notes"]["studentId"];
                    root.collegeId = pay.Attributes["notes"]["collegeId"];
                    root.courseId = pay.Attributes["notes"]["courseId"];
                    root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                    root.bank_transaction_id = pay.Attributes["acquirer_data"]["bank_transaction_id"];
                    root.studentId = pay.Attributes["notes"]["studentId"];
                    root.paymentResponse = JsonConvert.SerializeObject(root);


                    #region Reconcil log
                    UgPaymentLog _logModel = new UgPaymentLog();
                    _logModel.PaymentMethod = "RazorPay";
                    _logModel.PaymentRequest = model.paymentId;
                    _logModel.PaymentRequestDate = DateTime.Now;
                    _logModel.PaymentResponse = null;
                    _logModel.PaymentResponseDate = null;
                    _logModel.PaymentStatus = false;
                    _logModel.StudentId = root.studentId;
                    _logModel.TransactionId = root.order_id;
                    _logModel.HashForm = "";
                    _logModel.PaymentMessage = "Pending";
                    var _logresponse = _razorPayService.FetchPaymentLogPG(_logModel);
                    #endregion



                    ServiceResult data = await _razorPayService.FetchPaymentPG(root);
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    ServiceResult data = new ServiceResult();
                    data.Status = false;
                    data.Message = Convert.ToString(MessageConfig.ReconcilePaymentFailed);
                    data.ResultData = null;
                    data.StatusCode = 200;
                    return await Task.Run(() => Ok(data));
                }
            }
            catch (Exception ex)
            {
                ServiceResult data = new ServiceResult();
                data.Status = false;
                data.Message =Convert.ToString(ex.Message);
                data.ResultData = null;
                data.StatusCode = 200;
                return await Task.Run(() => Ok(data));
                //if (Convert.ToString(ex) == "The id provided does not exist")
                //{
                //    ServiceResult data = new ServiceResult();
                //    data.Status = false;
                //    data.Message = "The id provided does not exist";
                //    data.ResultData = null;
                //    data.StatusCode = 200;
                //    return await Task.Run(() => Ok(data));

                //}
                //else
                //{
                //    return await Task.Run(() => BadRequest(ex));
                //}
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("FetchFailedOrPendingPaymentPG")]
        public async Task<IActionResult> FetchFailedOrPendingPaymentPG()
        {
            try
            {
                ServiceResult data = new ServiceResult();
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                ServiceResult _data = new ServiceResult();
                _data = await _razorPayService.TempAllPendingTransaction("pg", "registration");
                foreach (var item in _data.ResultData)
                {
                    FetchPayment model = new FetchPayment();
                    model.paymentId= item.TransactionId;
                    // Payment pay = client.Payment.Fetch(model.paymentId);
                    //  Payment pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                    Payment pay = null;
                    var _resultPayment = client.Order.Fetch(model.paymentId).Payments().ToList();//.Where(x=>x.Attributes.status== "captured").FirstOrDefault();
                    if (_resultPayment.Count > 1)
                    {
                        pay = _resultPayment.Where(x => x.Attributes.status == "captured" && x.Attributes.description == "Registration Fee").FirstOrDefault();
                    }
                    else
                    {
                        pay = client.Order.Fetch(model.paymentId).Payments().Where(x => x.Attributes.status == "captured" && x.Attributes.description == "Registration Fee").FirstOrDefault();
                        //pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                    }
                    if (pay != null)
                    {
                        RazorRoot root = new RazorRoot();
                        int actualamount = pay.Attributes["amount"] / 100;
                        root.amount = actualamount;
                        root.amount_refunded = pay.Attributes["amount_refunded"];
                        root.bank = pay.Attributes["bank"];
                        root.captured = pay.Attributes["captured"];
                        root.card_id = pay.Attributes["card_id"];
                        root.contact = pay.Attributes["contact"];
                        root.created_at = pay.Attributes["created_at"];
                        root.currency = pay.Attributes["currency"];
                        root.description = pay.Attributes["description"];
                        root.email = pay.Attributes["email"];
                        root.entity = pay.Attributes["entity"];
                        root.error_code = pay.Attributes["error_code"];
                        root.error_description = pay.Attributes["error_description"];
                        root.error_reason = pay.Attributes["error_reason"];
                        root.error_source = pay.Attributes["error_source"];
                        root.error_step = pay.Attributes["error_step"];
                        root.fee = pay.Attributes["fee"];
                        root.id = pay.Attributes["id"];
                        root.international = pay.Attributes["international"];
                        root.invoice_id = pay.Attributes["invoice_id"];
                        root.method = pay.Attributes["method"];

                        root.order_id = pay.Attributes["order_id"];
                        root.refund_status = pay.Attributes["refund_status"];
                        root.status = pay.Attributes["status"];
                        root.tax = pay.Attributes["tax"];
                        root.vpa = pay.Attributes["vpa"];
                        root.wallet = pay.Attributes["wallet"];
                        root.paymentId = model.paymentId;

                        root.udf_address = pay.Attributes["notes"]["address"];
                        root.udf_contact = pay.Attributes["notes"]["contact"];
                        root.udf_feeType = pay.Attributes["notes"]["feeType"];
                        root.udf_name = pay.Attributes["notes"]["name"];
                        root.udf_studentId = pay.Attributes["notes"]["studentId"];
                        root.collegeId = pay.Attributes["notes"]["collegeId"];
                        root.courseId = pay.Attributes["notes"]["courseId"];
                        root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                        root.bank_transaction_id = pay.Attributes["acquirer_data"]["bank_transaction_id"];
                        root.studentId = pay.Attributes["notes"]["studentId"];
                        root.paymentResponse = JsonConvert.SerializeObject(root);


                        #region Reconcil log
                        UgPaymentLog _logModel = new UgPaymentLog();
                        _logModel.PaymentMethod = "RazorPay";
                        _logModel.PaymentRequest = model.paymentId;
                        _logModel.PaymentRequestDate = DateTime.Now;
                        _logModel.PaymentResponse = null;
                        _logModel.PaymentResponseDate = null;
                        _logModel.PaymentStatus = false;
                        _logModel.StudentId = root.studentId;
                        _logModel.TransactionId = root.order_id;
                        _logModel.HashForm = "";
                        _logModel.PaymentMessage = "Success";
                        var _logresponse = _razorPayService.FetchPaymentLogPG(_logModel);
                        #endregion



                        data = await _razorPayService.FetchPaymentPG(root);
                        
                    }
                    else
                    {
                        
                        data.Status = false;
                        data.Message = Convert.ToString(MessageConfig.ReconcilePaymentFailed);
                        data.ResultData = null;
                        data.StatusCode = 200;
                        //return await Task.Run(() => Ok(data));
                    }
                }
                return await Task.Run(() => Ok(data));
            }
            catch (Exception ex)
            {
                ServiceResult data = new ServiceResult();
                data.Status = false;
                data.Message = Convert.ToString(ex.Message);
                data.ResultData = null;
                data.StatusCode = 200;
                return await Task.Run(() => Ok(data));                
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("FetchAdmissionPaymentPG")]
        public async Task<IActionResult> FetchAdmissionPaymentPG([FromBody] FetchPayment model)
        {
            try
            {
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);
                // Payment pay = client.Payment.Fetch(model.paymentId);
                //  Payment pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                Payment pay = null;
                var _resultPayment = client.Order.Fetch(model.paymentId).Payments().ToList();//.Where(x=>x.Attributes.status== "captured").FirstOrDefault();
                if (_resultPayment.Count > 1)
                {
                    pay = _resultPayment.Where(x => x.Attributes.status == "captured" && x.Attributes.description == "course admission fee").FirstOrDefault();
                }
                else
                {
                    pay = client.Order.Fetch(model.paymentId).Payments().Where(x => x.Attributes.status == "captured" && x.Attributes.description == "course admission fee").FirstOrDefault();
                    //pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                }
                if (pay != null)
                {
                    RazorRoot root = new RazorRoot();
                    int actualamount = pay.Attributes["amount"] / 100;
                    root.amount = actualamount;
                    root.amount_refunded = pay.Attributes["amount_refunded"];
                    root.bank = pay.Attributes["bank"];
                    root.captured = pay.Attributes["captured"];
                    root.card_id = pay.Attributes["card_id"];
                    root.contact = pay.Attributes["contact"];
                    root.created_at = pay.Attributes["created_at"];
                    root.currency = pay.Attributes["currency"];
                    root.description = pay.Attributes["description"];
                    root.email = pay.Attributes["email"];
                    root.entity = pay.Attributes["entity"];
                    root.error_code = pay.Attributes["error_code"];
                    root.error_description = pay.Attributes["error_description"];
                    root.error_reason = pay.Attributes["error_reason"];
                    root.error_source = pay.Attributes["error_source"];
                    root.error_step = pay.Attributes["error_step"];
                    root.fee = pay.Attributes["fee"];
                    root.id = pay.Attributes["id"];
                    root.international = pay.Attributes["international"];
                    root.invoice_id = pay.Attributes["invoice_id"];
                    root.method = pay.Attributes["method"];

                    root.order_id = pay.Attributes["order_id"];
                    root.refund_status = pay.Attributes["refund_status"];
                    root.status = pay.Attributes["status"];
                    root.tax = pay.Attributes["tax"];
                    root.vpa = pay.Attributes["vpa"];
                    root.wallet = pay.Attributes["wallet"];
                    root.paymentId = model.paymentId;

                    root.udf_address = pay.Attributes["notes"]["address"];
                    root.udf_contact = pay.Attributes["notes"]["contact"];
                    root.udf_feeType = pay.Attributes["notes"]["feeType"];
                    root.udf_name = pay.Attributes["notes"]["name"];
                    root.udf_studentId = pay.Attributes["notes"]["studentId"];
                    root.collegeId = pay.Attributes["notes"]["collegeId"];
                    root.courseId = pay.Attributes["notes"]["courseId"];
                    root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                    root.bank_transaction_id = pay.Attributes["acquirer_data"]["bank_transaction_id"];
                    root.studentId = pay.Attributes["notes"]["studentId"];
                    root.paymentResponse = JsonConvert.SerializeObject(root);


                    #region Reconcil log
                    UgPaymentLog _logModel = new UgPaymentLog();
                    _logModel.PaymentMethod = "RazorPay";
                    _logModel.PaymentRequest = model.paymentId;
                    _logModel.PaymentRequestDate = DateTime.Now;
                    _logModel.PaymentResponse = null;
                    _logModel.PaymentResponseDate = null;
                    _logModel.PaymentStatus = false;
                    _logModel.StudentId = root.studentId;
                    _logModel.TransactionId = root.order_id;
                    _logModel.HashForm = "";
                    _logModel.PaymentMessage = "Pending";
                    var _logresponse = _razorPayService.FetchPaymentLogPG(_logModel);
                    #endregion



                    ServiceResult data = await _razorPayService.FetchPaymentPG(root);
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    ServiceResult data = new ServiceResult();
                    data.Status = false;
                    data.Message = Convert.ToString(MessageConfig.ReconcilePaymentFailed);
                    data.ResultData = null;
                    data.StatusCode = 200;
                    return await Task.Run(() => Ok(data));
                }
            }
            catch (Exception ex)
            {
                ServiceResult data = new ServiceResult();
                data.Status = false;
                data.Message = Convert.ToString(ex.Message);
                data.ResultData = null;
                data.StatusCode = 200;
                return await Task.Run(() => Ok(data));
                //if (Convert.ToString(ex) == "The id provided does not exist")
                //{
                //    ServiceResult data = new ServiceResult();
                //    data.Status = false;
                //    data.Message = "The id provided does not exist";
                //    data.ResultData = null;
                //    data.StatusCode = 200;
                //    return await Task.Run(() => Ok(data));

                //}
                //else
                //{
                //    return await Task.Run(() => BadRequest(ex));
                //}
            }

        }

        /// <summary>
        /// Reconcile Payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("TempTableAdmissionPaymentUG")]
        public async Task<IActionResult> TempTableAdmissionPaymentUG()
        {
            ServiceResult data = new ServiceResult();
            string errorMessage = string.Empty;
            try
            {
                ServiceResult _data = new ServiceResult();
                _data = await _razorPayService.TempAllPendingTransaction("ug","admission");
                foreach (var item in _data.ResultData)
                {
                    FetchPayment model = new FetchPayment();
                    model.paymentId = item.TransactionId;

                    var key = _configuration.GetValue<string>("RazorPay:KEY");
                    var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                    RazorpayClient client = new RazorpayClient(key, SecretKey);
                    // Payment pay = client.Payment.Fetch(model.paymentId);
                    Payment pay = null;
                    var _resultPayment = client.Order.Fetch(model.paymentId).Payments().ToList();//.Where(x=>x.Attributes.status== "captured").FirstOrDefault();
                    if (_resultPayment.Count > 1)
                    {
                        pay = _resultPayment.Where(x => x.Attributes.status == "captured" && x.Attributes.description == "Course Admission Fee").FirstOrDefault();
                    }
                    else
                    {
                        pay = client.Order.Fetch(model.paymentId).Payments().Where(x => x.Attributes.status == "captured" && x.Attributes.description == "Course Admission Fee").FirstOrDefault();
                    }
                    //Payment pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                    if (pay != null)
                    {
                        RazorRoot root = new RazorRoot();
                        int actualamount = pay.Attributes["amount"] / 100;
                        root.amount = actualamount;
                        root.amount_refunded = pay.Attributes["amount_refunded"];
                        root.bank = pay.Attributes["bank"];
                        root.captured = pay.Attributes["captured"];
                        root.card_id = pay.Attributes["card_id"];
                        root.contact = pay.Attributes["contact"];
                        root.created_at = pay.Attributes["created_at"];
                        root.currency = pay.Attributes["currency"];
                        root.description = pay.Attributes["description"];
                        root.email = pay.Attributes["email"];
                        root.entity = pay.Attributes["entity"];
                        root.error_code = pay.Attributes["error_code"];
                        root.error_description = pay.Attributes["error_description"];
                        root.error_reason = pay.Attributes["error_reason"];
                        root.error_source = pay.Attributes["error_source"];
                        root.error_step = pay.Attributes["error_step"];
                        root.fee = string.IsNullOrEmpty(Convert.ToString(pay.Attributes["fee"])) ? 0 : pay.Attributes["fee"];
                        root.id = pay.Attributes["id"];
                        root.international = pay.Attributes["international"];
                        root.invoice_id = pay.Attributes["invoice_id"];
                        root.method = pay.Attributes["method"];

                        root.order_id = pay.Attributes["order_id"];
                        root.refund_status = pay.Attributes["refund_status"];
                        root.status = pay.Attributes["status"];
                        root.tax = string.IsNullOrEmpty(Convert.ToString(pay.Attributes["tax"])) ? 0 : pay.Attributes["tax"];
                        root.vpa = pay.Attributes["vpa"];
                        root.wallet = pay.Attributes["wallet"];
                        root.paymentId = model.paymentId;

                        root.udf_address = pay.Attributes["notes"]["address"];
                        root.udf_contact = pay.Attributes["notes"]["contact"];
                        root.udf_feeType = pay.Attributes["notes"]["feeType"];
                        root.udf_name = pay.Attributes["notes"]["name"];
                        root.udf_studentId = pay.Attributes["notes"]["studentId"];
                        root.collegeId = pay.Attributes["notes"]["collegeId"];
                        root.courseId = pay.Attributes["notes"]["courseId"];
                        root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                        root.bank_transaction_id = pay.Attributes["acquirer_data"]["bank_transaction_id"];
                        root.studentId = pay.Attributes["notes"]["studentId"];
                        root.paymentResponse = JsonConvert.SerializeObject(root);


                        #region Reconcil log
                        UgPaymentLog _logModel = new UgPaymentLog();
                        _logModel.PaymentMethod = "RazorPay";
                        _logModel.PaymentRequest = model.paymentId;
                        _logModel.PaymentRequestDate = DateTime.Now;
                        _logModel.PaymentResponse = null;
                        _logModel.PaymentResponseDate = null;
                        _logModel.PaymentStatus = true;
                        _logModel.StudentId = root.studentId;
                        _logModel.TransactionId = root.order_id;
                        _logModel.HashForm = "";
                        _logModel.PaymentMessage = "Reconcil";
                        var _logresponse = _razorPayService.FetchPaymentLogUG(_logModel);
                        #endregion



                        data = await _razorPayService.FetchPaymentUG(root);
                        data.Status = true;
                        //  return await Task.Run(() => Ok(data));
                    }
                    else
                    {
                        data = new ServiceResult();
                        data.Status = false;
                        data.Message = Convert.ToString(MessageConfig.ReconcilePaymentFailed);
                        data.ResultData = null;
                        data.StatusCode = 200;
                        
                       // return await Task.Run(() => Ok(data));
                    }
                    #region Reconcil log
                    UgPaymentLog _logModel1 = new UgPaymentLog();
                    _logModel1.PaymentMethod = "RazorPay";
                    //var json = new JavaScriptSerializer().Serialize(obj);
                    _logModel1.PaymentRequest = data.Status==false?"Not Found": Convert.ToString(new JavaScriptSerializer().Serialize(data.ResultData));
                    _logModel1.PaymentRequestDate = DateTime.Now;
                    _logModel1.PaymentResponse = null;
                    _logModel1.PaymentResponseDate = null;
                    _logModel1.PaymentStatus = data.Status == false ? false : data.ResultData.status == "Success" ? true : false;
                    _logModel1.StudentId = data.Status == false ? 0 : data.ResultData.StudentId;
                    _logModel1.TransactionId = model.paymentId;
                    _logModel1.HashForm = "";
                    _logModel1.PaymentMessage = "LOOP";
                    var _logresponse1 = _razorPayService.FetchPaymentLogUG(_logModel1);
                    #endregion
                }

                return await Task.Run(() => Ok());
            }
            catch (Exception ex)
            {
                data = new ServiceResult();
                data.Status = false;
                data.Message = Convert.ToString(ex.Message);
                data.ResultData = null;
                data.StatusCode = 200;
                return await Task.Run(() => Ok(data));
                //if (Convert.ToString(ex) == "The id provided does not exist")
                //{
                //    ServiceResult data = new ServiceResult();
                //    data.Status = false;
                //    data.Message = "The id provided does not exist";
                //    data.ResultData = null;
                //    data.StatusCode = 200;
                //    return await Task.Run(() => Ok(data));

                //}
                //else
                //{
                //    return await Task.Run(() => BadRequest(ex));

                //}
            }

        }

        /// <summary>
        /// Reconcile Payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("TempTableAdmissionPaymentPG")]
        public async Task<IActionResult> TempTableAdmissionPaymentPG()
        {
            ServiceResult data = new ServiceResult();
            string errorMessage = string.Empty;
            try
            {
                ServiceResult _data = new ServiceResult();
                _data = await _razorPayService.TempAllPendingTransaction("pg", "admission");
                foreach (var item in _data.ResultData)
                {
                    FetchPayment model = new FetchPayment();
                    model.paymentId = item.TransactionId;

                    var key = _configuration.GetValue<string>("RazorPay:KEY");
                    var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                    RazorpayClient client = new RazorpayClient(key, SecretKey);
                    // Payment pay = client.Payment.Fetch(model.paymentId);
                    Payment pay = null;
                    var _resultPayment = client.Order.Fetch(model.paymentId).Payments().ToList();//.Where(x=>x.Attributes.status== "captured").FirstOrDefault();
                    if (_resultPayment.Count > 1)
                    {
                        pay = _resultPayment.Where(x => x.Attributes.status == "captured" && x.Attributes.description == "course admission fee").FirstOrDefault();
                    }
                    else
                    {
                        pay = client.Order.Fetch(model.paymentId).Payments().Where(x => x.Attributes.status == "captured" && x.Attributes.description == "course admission fee").FirstOrDefault();
                    }
                    //Payment pay = client.Order.Fetch(model.paymentId).Payments().FirstOrDefault();
                    if (pay != null)
                    {
                        RazorRoot root = new RazorRoot();
                        int actualamount = pay.Attributes["amount"] / 100;
                        root.amount = actualamount;
                        root.amount_refunded = pay.Attributes["amount_refunded"];
                        root.bank = pay.Attributes["bank"];
                        root.captured = pay.Attributes["captured"];
                        root.card_id = pay.Attributes["card_id"];
                        root.contact = pay.Attributes["contact"];
                        root.created_at = pay.Attributes["created_at"];
                        root.currency = pay.Attributes["currency"];
                        root.description = pay.Attributes["description"];
                        root.email = pay.Attributes["email"];
                        root.entity = pay.Attributes["entity"];
                        root.error_code = pay.Attributes["error_code"];
                        root.error_description = pay.Attributes["error_description"];
                        root.error_reason = pay.Attributes["error_reason"];
                        root.error_source = pay.Attributes["error_source"];
                        root.error_step = pay.Attributes["error_step"];
                        root.fee = string.IsNullOrEmpty(Convert.ToString(pay.Attributes["fee"])) ? 0 : pay.Attributes["fee"];
                        root.id = pay.Attributes["id"];
                        root.international = pay.Attributes["international"];
                        root.invoice_id = pay.Attributes["invoice_id"];
                        root.method = pay.Attributes["method"];

                        root.order_id = pay.Attributes["order_id"];
                        root.refund_status = pay.Attributes["refund_status"];
                        root.status = pay.Attributes["status"];
                        root.tax = string.IsNullOrEmpty(Convert.ToString(pay.Attributes["tax"])) ? 0 : pay.Attributes["tax"];
                        root.vpa = pay.Attributes["vpa"];
                        root.wallet = pay.Attributes["wallet"];
                        root.paymentId = model.paymentId;

                        root.udf_address = pay.Attributes["notes"]["address"];
                        root.udf_contact = pay.Attributes["notes"]["contact"];
                        root.udf_feeType = pay.Attributes["notes"]["feeType"];
                        root.udf_name = pay.Attributes["notes"]["name"];
                        root.udf_studentId = pay.Attributes["notes"]["studentId"];
                        root.collegeId = pay.Attributes["notes"]["collegeId"];
                        root.courseId = pay.Attributes["notes"]["courseId"];
                        root.rrn = pay.Attributes["acquirer_data"]["rrn"];
                        root.bank_transaction_id = pay.Attributes["acquirer_data"]["bank_transaction_id"];
                        root.studentId = pay.Attributes["notes"]["studentId"];
                        root.paymentResponse = JsonConvert.SerializeObject(root);


                        #region Reconcil log
                        UgPaymentLog _logModel = new UgPaymentLog();
                        _logModel.PaymentMethod = "RazorPay";
                        _logModel.PaymentRequest = model.paymentId;
                        _logModel.PaymentRequestDate = DateTime.Now;
                        _logModel.PaymentResponse = null;
                        _logModel.PaymentResponseDate = null;
                        _logModel.PaymentStatus = true;
                        _logModel.StudentId = root.studentId;
                        _logModel.TransactionId = root.order_id;
                        _logModel.HashForm = "";
                        _logModel.PaymentMessage = "Reconcil";
                        var _logresponse = _razorPayService.FetchPaymentLogPG(_logModel);
                        #endregion



                        data = await _razorPayService.FetchPaymentPG(root);
                        data.Status = true;
                        //  return await Task.Run(() => Ok(data));
                    }
                    else
                    {
                        data = new ServiceResult();
                        data.Status = false;
                        data.Message = Convert.ToString(MessageConfig.ReconcilePaymentFailed);
                        data.ResultData = null;
                        data.StatusCode = 200;

                        // return await Task.Run(() => Ok(data));
                    }
                    #region Reconcil log
                    UgPaymentLog _logModel1 = new UgPaymentLog();
                    _logModel1.PaymentMethod = "RazorPay";
                    //var json = new JavaScriptSerializer().Serialize(obj);
                    _logModel1.PaymentRequest = data.Status == false ? "Not Found" : Convert.ToString(new JavaScriptSerializer().Serialize(data.ResultData));
                    _logModel1.PaymentRequestDate = DateTime.Now;
                    _logModel1.PaymentResponse = null;
                    _logModel1.PaymentResponseDate = null;
                    _logModel1.PaymentStatus = data.Status == false ? false : data.ResultData.status == "Success" ? true : false;
                    _logModel1.StudentId = data.Status == false ? 0 : data.ResultData.StudentId;
                    _logModel1.TransactionId = model.paymentId;
                    _logModel1.HashForm = "";
                    _logModel1.PaymentMessage = "LOOP";
                    var _logresponse1 = _razorPayService.FetchPaymentLogPG(_logModel1);
                    #endregion
                }

                return await Task.Run(() => Ok());
            }
            catch (Exception ex)
            {
                data = new ServiceResult();
                data.Status = false;
                data.Message = Convert.ToString(ex.Message);
                data.ResultData = null;
                data.StatusCode = 200;
                return await Task.Run(() => Ok(data));
                //if (Convert.ToString(ex) == "The id provided does not exist")
                //{
                //    ServiceResult data = new ServiceResult();
                //    data.Status = false;
                //    data.Message = "The id provided does not exist";
                //    data.ResultData = null;
                //    data.StatusCode = 200;
                //    return await Task.Run(() => Ok(data));

                //}
                //else
                //{
                //    return await Task.Run(() => BadRequest(ex));

                //}
            }

        }
        #endregion
        [AllowAnonymous]
        [HttpPost]
        [Route("TestOrder")]
        public async Task<IActionResult> TestOrder()
        {
            try
            {
                Int32 StudentId = 1221;
                string type = "PG";
                if (ModelState.IsValid)
                {
                    //var identity = HttpContext.User.Identity as ClaimsIdentity;
                    //if (identity != null)
                    //{
                    //    IEnumerable<Claim> claims = identity.Claims;
                    //    StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                    //}
                    var key = _configuration.GetValue<string>("RazorPay:KEY");
                    var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                    RazorpayClient client = new RazorpayClient(key, SecretKey);
                    Int32 Amount = 100;
                    Dictionary<string, object> options = new Dictionary<string, object>();
                    options.Add("amount", Amount); // 1 rs
                    options.Add("receipt", "order_rcptid_11");
                    options.Add("currency", "INR");
                    options.Add("payment_capture", true);
                    Dictionary<string, object> Notesoptions = new Dictionary<string, object>();
                    //options.Add("notes", "{'feeType':'admission fee','name':'Sharma','StudentId':'1'}");
                    Notesoptions.Add("name", "Sharma");
                    Notesoptions.Add("studentId", "1");
                    Notesoptions.Add("address", "#123");
                    Notesoptions.Add("contact", "9999999999");
                    options.Add("notes", Notesoptions);
                    Order order = client.Order.Create(options);
                    string OrderId = order.Attributes.id;
                    var orderJson = order.Attributes.ToString();
                    //options.Add("order_id", OrderId);
                    //options.Add("name", "Sharma");
                    //options.Add("studentId", "1");
                    //options.Add("address", "#123");
                    //options.Add("contact", "9999999999");

                    //var ss = client.Payment.Capture(options);
                    //Int32 Amount = 1;
                    ServiceResult data = await _razorPayService.GenerateOrder(StudentId, OrderId, Amount, type);
                    data.ResultData = OrderId;
                    //ServiceResult data = new ServiceResult();
                    data.Status = true;

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

        /// Payment API
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("transfer")]
        public async Task<IActionResult> Transfer()
        {
            AdmissionPaymentModel _model = new AdmissionPaymentModel();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            ServiceResult data = new ServiceResult();
            string StudentType = string.Empty;
            string type = "UG";
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var key = _configuration.GetValue<string>("RazorPay:KEY");
                var SecretKey = _configuration.GetValue<string>("RazorPay:SECRET_KEY");
                RazorpayClient client = new RazorpayClient(key, SecretKey);

                //HdfcStudentAdmissionModel model = new HdfcStudentAdmissionModel();
                //model.StudentId = Convert.ToInt32(identity.FindFirst("UserID").Value);
                //model.CollegeId = _model.CollegeId;
                //model.CourseId = _model.CourseId;
                //model.CourseType = _model.CourseType;
                //model.StudentType = identity.FindFirst("loginType").Value;
                //model.Name = identity.FindFirst("Name").Value;
                //model.Phone = identity.FindFirst("Phone").Value;
                //model.Email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                //model.Note = "Course Admission Fee";

                decimal AppFormAmount = Convert.ToDecimal(15);//_configuration.GetValue<string>("HDFC:AppFormAmount");
                                                              //Convert paise into rupies
                decimal Amount = Convert.ToDecimal(AppFormAmount) * 100;



                #region Generate randon receipt No
                string receipt = string.Empty;
                Random rnd = new Random();
                _model.StudentId = Convert.ToInt32(1);
                string strHash = HdfcPaymentFunction.Generatehash512(_model.StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                receipt = _model.StudentId + strHash.ToString().Substring(0, 25);
                #endregion


                _model.CollegeId = 1;
                _model.CourseId = 1;
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", Amount); // 1 rs
                options.Add("receipt", receipt);
                options.Add("currency", "INR");
                options.Add("payment_capture", true);
                Dictionary<string, object> Notesoptions = new Dictionary<string, object>();
                //options.Add("notes", "{'feeType':'admission fee','name':'Sharma','StudentId':'1'}");
                Notesoptions.Add("name", "Vicky");
                Notesoptions.Add("studentId", _model.StudentId);
                Notesoptions.Add("collegeId", _model.CollegeId);
                Notesoptions.Add("courseId", _model.CourseId);
                Notesoptions.Add("contact", "9872310778");
                Notesoptions.Add("feeType", "Course Admission Fee");
                options.Add("notes", Notesoptions);

                List<Dictionary<string, object>> ourList = new List<Dictionary<string, object>>();
                Dictionary<string, object> input1 = new Dictionary<string, object>();
                input1.Add("account", "acc_Hq7FEwAMW1R3gP"); // this amount should be same as transaction amount
                input1.Add("currency", "INR");//
                input1.Add("amount", Amount);
                input1.Add("on_hold", false);

                Dictionary<string, object> input2 = new Dictionary<string, object>();
                input2.Add("branch", "SBI");
                input2.Add("name", "Vikas Sharma");

                input1.Add("notes", input2);
                ourList.Add(input1);
                options.Add("transfers", ourList);


                Order order = client.Order.Create(options);
                string OrderId = order.Attributes.id;
                var orderJson = order.Attributes.ToString();

                data = await _razorPayService.GenerateOrder(_model.StudentId, OrderId, AppFormAmount, type);
                data.ResultData = new { OrderId = OrderId, Skey = key };
                data.Status = true;
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
        [Route("StudentTransactions")]
        public async Task<IActionResult> FetchStudentTransactions([FromBody] StudentPayment model)
        {
            ServiceResult data = new ServiceResult();
            data = await _razorPayService.FetchStudentTransactions(model);
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
