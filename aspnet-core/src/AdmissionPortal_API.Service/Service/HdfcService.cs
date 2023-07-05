using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using AdmissionPortal_API.Utility.PaymentFunction;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace AdmissionPortal_API.Service.Service
{
    public class HdfcService : IHdfcService
    {
        private readonly IHdfcPaymentRepository _hdfcPaymentRepository;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public HdfcService(IHdfcPaymentRepository hdfcPaymentRepository, IMapper mapper, ILogError logError, IConfiguration configuration)
        {
            _logError = logError;
            _configuration = configuration;
            _hdfcPaymentRepository = hdfcPaymentRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> MakeHdfcPayment(string Amount, PaymentStudentModel model)
        {
            ServiceResult serviceResult = new ServiceResult();

            string action1 = string.Empty;
            string hash1 = string.Empty;
            string txnid1 = string.Empty;
            var MERCHANT_KEY = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
            var SALT = _configuration.GetValue<string>("HDFC:SALT");
            var PAYU_BASE_URL = _configuration.GetValue<string>("HDFC:PAYU_BASE_URL");
            var PortalLink = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
            var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
            var PAYU_SuccessUrl = _configuration.GetValue<string>("HDFC:SuccessUrl");
            try
            {
                string hash_string = string.Empty;
                Random rnd = new Random();
                string strHash = HdfcPaymentFunction.Generatehash512(model.StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                txnid1 = model.StudentId + strHash.ToString().Substring(0, 25);
                //hash_string += SALT;// appending SALT
                //"key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10"
                hash_string += MERCHANT_KEY + "|" + txnid1 + "|" + Convert.ToDecimal(Amount).ToString("g29") + "|" + model.Note + "|" + model.Name + "|" + model.Email + "|||||||||||" + SALT;

                hash1 = HdfcPaymentFunction.Generatehash512(hash_string).ToLower();         //generating hash
                action1 = PAYU_BASE_URL + "/_payment";// setting URL


                UgPaymentLog _logModel = new UgPaymentLog();
                _logModel.PaymentMethod = "HDFC";
                _logModel.PaymentRequest = hash_string;
                _logModel.PaymentRequestDate = DateTime.Now;
                _logModel.PaymentResponse = null;
                _logModel.PaymentResponseDate = null;
                _logModel.PaymentStatus = false;
                _logModel.StudentId = model.StudentId;
                _logModel.TransactionId = txnid1;
                _logModel.HashForm = hash1;
                _logModel.PaymentMessage = "Pending";
                _logModel.Amount = Amount;

                await PaymentLog(_logModel);
                // PaymentLog

                if (!string.IsNullOrEmpty(hash1))
                {
                    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                    data.Add("hash", hash1);
                    data.Add("txnid", txnid1);
                    data.Add("key", MERCHANT_KEY);
                    string AmountForm = Convert.ToDecimal(Amount).ToString("g29");// eliminating trailing zeros
                                                                                  // amount.Text = AmountForm;
                    data.Add("amount", AmountForm);
                    data.Add("firstname", model.Name);
                    data.Add("email", model.Email);
                    data.Add("phone", model.Phone);
                    data.Add("productinfo", model.Note);
                    //success url
                    data.Add("surl", PAYU_SuccessUrl);
                    //Failed Url
                    data.Add("furl", PAYU_SuccessUrl);
                    data.Add("lastname", "");
                    data.Add("curl", PAYU_SuccessUrl);
                    data.Add("address1", "");
                    data.Add("address2", "");
                    data.Add("city", "");
                    data.Add("state", "");
                    data.Add("country", "");
                    data.Add("zipcode", "");
                    data.Add("udf1", "");
                    data.Add("udf2", "");
                    data.Add("udf3", "");
                    data.Add("udf4", "");
                    data.Add("udf5", "");
                    data.Add("pg", "CC");


                    string strForm = HdfcPaymentFunction.PreparePOSTForm(action1, data);
                    serviceResult.ResultData = strForm;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    //Page.Controls.Add(new LiteralControl(strForm));
                    // return await Task.Run(() => Ok(new { action= action1,data=data }));
                }

                else
                {
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                }
            }
            catch (Exception ex)
            {
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> PaymentLog(UgPaymentLog UgPaymentLog)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<UgPaymentLog>(UgPaymentLog);
                var _result = _hdfcPaymentRepository.AddAsync(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {

                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Payment Log : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> PaymentResponse(IFormCollection _collection)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
                var MERCHANT_KEY = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
                var SALT = _configuration.GetValue<string>("HDFC:SALT");
                var hash_seq = _configuration.GetValue<string>("HDFC:hashSequence");
                string command = "verify_payment";


                #region respone Bind with model

                PaymentResponseModel _respone = new();
                _respone.mihpayid = _collection["mihpayid"];
                _respone.mode = _collection["mode"];
                _respone.status = _collection["status"];
                _respone.unmappedstatus = _collection["unmappedstatus"];
                _respone.key = _collection["key"];
                _respone.txnid = _collection["txnid"];
                _respone.amount = _collection["amount"];
                _respone.cardCategory = _collection["cardCategory"];
                _respone.discount = _collection["discount"];
                _respone.net_amount_debit = _collection["net_amount_debit"];
                _respone.addedon = _collection["addedon"];
                _respone.productinfo = _collection["productinfo"];
                _respone.firstname = _collection["firstname"];
                _respone.lastnam = _collection["lastnam"];
                _respone.address1 = _collection["address1"];
                _respone.address2 = _collection["address2"];
                _respone.city = _collection["city"];
                _respone.state = _collection["state"];
                _respone.country = _collection["country"];
                _respone.zipcode = _collection["zipcode"];
                _respone.email = _collection["email"];
                _respone.phone = _collection["phone"];
                _respone.udf1 = _collection["udf1"];
                _respone.udf2 = _collection["udf2"];
                _respone.udf3 = _collection["udf3"];
                _respone.udf4 = _collection["udf4"];
                _respone.udf5 = _collection["udf5"];
                _respone.udf6 = _collection["udf6"];
                _respone.udf7 = _collection["udf7"];
                _respone.udf8 = _collection["udf8"];
                _respone.udf9 = _collection["udf9"];
                _respone.udf10 = _collection["udf10"];
                _respone.hash = _collection["hash"];
                _respone.field1 = _collection["field1"];
                _respone.field2 = _collection["field2"];
                _respone.field3 = _collection["field3"];
                _respone.field4 = _collection["field4"];
                _respone.field5 = _collection["field5"];
                _respone.field6 = _collection["field6"];
                _respone.field7 = _collection["field7"];
                _respone.field8 = _collection["field8"];
                _respone.field9 = _collection["field9"];
                _respone.payment_source = _collection["payment_source"];
                _respone.PG_TYPE = _collection["PG_TYPE"];
                _respone.bank_ref_num = _collection["bank_ref_num"];
                _respone.bankcode = _collection["bankcode"];
                _respone.error = _collection["error"];
                _respone.error_Message = _collection["error_Message"];
                _respone.name_on_card = _collection["name_on_card"];
                _respone.cardnum = _collection["cardnum"];
                _respone.cardhash = _collection["cardhash"];

                _respone.paymentResponse = JsonConvert.SerializeObject(_respone);

                #endregion

                #region hash calculation
                HdfcPaymentObject objClsPayment = new HdfcPaymentObject();
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                merc_hash_vars_seq = hash_seq.Split('|');
                Array.Reverse(merc_hash_vars_seq);
                merc_hash_string = SALT + "|" + _respone.status;

                foreach (string merc_hash_var in merc_hash_vars_seq)
                {
                    merc_hash_string += "|";
                    merc_hash_string = merc_hash_string + (Convert.ToString(_collection[merc_hash_var]) != null ? _collection[merc_hash_var] : "");
                }
                objClsPayment.amount = _respone.amount;
                objClsPayment.txnid = _respone.txnid;
                objClsPayment.strWithoutHash = merc_hash_string;
                merc_hash = HdfcPaymentFunction.Generatehash512(merc_hash_string).ToLower();
                objClsPayment.strHash512 = merc_hash;
                #endregion
                var _resultDB = _hdfcPaymentRepository.HdfcMatchHashPaymentResponse(objClsPayment);
                objClsPayment.PaymentSuccess = false;
                if (_resultDB.Status > 0)
                {
                    objClsPayment.PaymentSuccess = true;
                }

                if (objClsPayment.PaymentSuccess)
                {
                    if (merc_hash != _respone.hash)
                    {
                        objClsPayment.PaymentSuccess = false;
                        _respone.status = "failure";
                        var _result = _hdfcPaymentRepository.HdfcPaymentResponse(_respone);
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        #region Verify Payment before response
                        string hashstr = MERCHANT_KEY + "|" + command + "|" + _respone.txnid + "|" + SALT;
                        string hash = HdfcPaymentFunction.Generatehash512(hashstr).ToLower();
                        ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                        var request = (HttpWebRequest)WebRequest.Create(PAYU_VERIFY_URL);
                        var postData = "key=" + Uri.EscapeDataString(MERCHANT_KEY);
                        postData += "&hash=" + Uri.EscapeDataString(hash);
                        postData += "&var1=" + Uri.EscapeDataString(_respone.txnid);
                        postData += "&command=" + Uri.EscapeDataString(command);
                        var data = Encoding.ASCII.GetBytes(postData);
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;
                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                        var response = (HttpWebResponse)request.GetResponse();
                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        var _VerifyPayment_RequestData = JsonConvert.DeserializeObject<Root>(responseString);
                        var transaction_details = (Newtonsoft.Json.Linq.JObject)((Newtonsoft.Json.Linq.JContainer)_VerifyPayment_RequestData.transaction_details).First().First();
                        PaymentResponseModel obj = ((JObject)transaction_details).ToObject<PaymentResponseModel>();

                        #endregion

                        _respone.VerifyPaymentResponse = JsonConvert.SerializeObject(transaction_details);
                        _respone.VerifyPaymentStatus = obj.status;
                        if (obj.status.ToLower() != "success")
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        if (obj.txnid != _respone.txnid)
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        if (obj.mihpayid != _respone.mihpayid)
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        var _result = _hdfcPaymentRepository.HdfcPaymentResponse(_respone);

                        if (_result != null)
                        {
                            serviceResult.ResultData = _result.Result;
                            if (serviceResult.ResultData.Response == "1")
                            {

                                serviceResult.Message = MessageConfig.PaymentSuccess;
                                serviceResult.Status = true;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.ErrorOccurred;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                            }
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ErrorOccurred;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                        }
                    }
                }
                else
                {
                    _respone.status = "failure";
                    var _result = _hdfcPaymentRepository.HdfcPaymentResponse(_respone);
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Payment Log : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);

        }


        public async Task<ServiceResult> HdfcPaymentDetail(string txnid, int studentId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _decrTxnid = Decryption.DecodeHexaValue(txnid);
                var _result = await _hdfcPaymentRepository.HdfcPaymentDetail(_decrTxnid);

                if (_result != null)
                {
                    if (_result.StudentId == studentId)
                    {
                        serviceResult.ResultData = _result;
                        if (serviceResult.ResultData != null)
                        {
                            serviceResult.Message = MessageConfig.Success;
                            serviceResult.Status = true;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ErrorOccurred;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.NotAuthorize;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get Student objections : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> MakeHdfcPaymentPG(string Amount, PaymentStudentModel model)
        {
            ServiceResult serviceResult = new ServiceResult();

            string action1 = string.Empty;
            string hash1 = string.Empty;
            string txnid1 = string.Empty;
            var MERCHANT_KEY = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
            var SALT = _configuration.GetValue<string>("HDFC:SALT");
            var PAYU_BASE_URL = _configuration.GetValue<string>("HDFC:PAYU_BASE_URL");
            var PortalLink = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
            var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
            var PAYU_SuccessUrl = _configuration.GetValue<string>("HDFC:SuccessUrlPG");
            try
            {
                string hash_string = string.Empty;
                Random rnd = new Random();
                string strHash = HdfcPaymentFunction.Generatehash512(model.StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                txnid1 = model.StudentId + strHash.ToString().Substring(0, 25);
                //hash_string += SALT;// appending SALT
                //"key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10"
                hash_string += MERCHANT_KEY + "|" + txnid1 + "|" + Convert.ToDecimal(Amount).ToString("g29") + "|" + model.Note + "|" + model.Name + "|" + model.Email + "|||||||||||" + SALT;

                hash1 = HdfcPaymentFunction.Generatehash512(hash_string).ToLower();         //generating hash
                action1 = PAYU_BASE_URL + "/_payment";// setting URL


                UgPaymentLog _logModel = new UgPaymentLog();
                _logModel.PaymentMethod = "HDFC";
                _logModel.PaymentRequest = hash_string;
                _logModel.PaymentRequestDate = DateTime.Now;
                _logModel.PaymentResponse = null;
                _logModel.PaymentResponseDate = null;
                _logModel.PaymentStatus = false;
                _logModel.StudentId = model.StudentId;
                _logModel.TransactionId = txnid1;
                _logModel.HashForm = hash1;
                _logModel.PaymentMessage = "Pending";
                _logModel.Amount = Amount;


                await PaymentLogPG(_logModel);
                // PaymentLog

                if (!string.IsNullOrEmpty(hash1))
                {
                    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                    data.Add("hash", hash1);
                    data.Add("txnid", txnid1);
                    data.Add("key", MERCHANT_KEY);
                    string AmountForm = Convert.ToDecimal(Amount).ToString("g29");// eliminating trailing zeros
                                                                                  // amount.Text = AmountForm;
                    data.Add("amount", AmountForm);
                    data.Add("firstname", model.Name);
                    data.Add("email", model.Email);
                    data.Add("phone", model.Phone);
                    data.Add("productinfo", model.Note);
                    //success url
                    data.Add("surl", PAYU_SuccessUrl);
                    //Failed Url
                    data.Add("furl", PAYU_SuccessUrl);
                    data.Add("lastname", "");
                    data.Add("curl", PAYU_SuccessUrl);
                    data.Add("address1", "");
                    data.Add("address2", "");
                    data.Add("city", "");
                    data.Add("state", "");
                    data.Add("country", "");
                    data.Add("zipcode", "");
                    data.Add("udf1", "");
                    data.Add("udf2", "");
                    data.Add("udf3", "");
                    data.Add("udf4", "");
                    data.Add("udf5", "");
                    data.Add("pg", "CC");


                    string strForm = HdfcPaymentFunction.PreparePOSTForm(action1, data);
                    serviceResult.ResultData = strForm;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    //Page.Controls.Add(new LiteralControl(strForm));
                    // return await Task.Run(() => Ok(new { action= action1,data=data }));
                }

                else
                {
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                }
            }
            catch (Exception ex)
            {
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> PaymentLogPG(UgPaymentLog UgPaymentLog)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<UgPaymentLog>(UgPaymentLog);
                var _result = _hdfcPaymentRepository.AddAsyncPG(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {

                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Payment Log PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> PaymentResponsePG(IFormCollection _collection)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
                var MERCHANT_KEY = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
                var SALT = _configuration.GetValue<string>("HDFC:SALT");
                var hash_seq = _configuration.GetValue<string>("HDFC:hashSequence");
                string command = "verify_payment";
                #region respone Bind with model

                PaymentResponseModel _respone = new();
                _respone.mihpayid = _collection["mihpayid"];
                _respone.mode = _collection["mode"];
                _respone.status = _collection["status"];
                _respone.unmappedstatus = _collection["unmappedstatus"];
                _respone.key = _collection["key"];
                _respone.txnid = _collection["txnid"];
                _respone.amount = _collection["amount"];
                _respone.cardCategory = _collection["cardCategory"];
                _respone.discount = _collection["discount"];
                _respone.net_amount_debit = _collection["net_amount_debit"];
                _respone.addedon = _collection["addedon"];
                _respone.productinfo = _collection["productinfo"];
                _respone.firstname = _collection["firstname"];
                _respone.lastnam = _collection["lastnam"];
                _respone.address1 = _collection["address1"];
                _respone.address2 = _collection["address2"];
                _respone.city = _collection["city"];
                _respone.state = _collection["state"];
                _respone.country = _collection["country"];
                _respone.zipcode = _collection["zipcode"];
                _respone.email = _collection["email"];
                _respone.phone = _collection["phone"];
                _respone.udf1 = _collection["udf1"];
                _respone.udf2 = _collection["udf2"];
                _respone.udf3 = _collection["udf3"];
                _respone.udf4 = _collection["udf4"];
                _respone.udf5 = _collection["udf5"];
                _respone.udf6 = _collection["udf6"];
                _respone.udf7 = _collection["udf7"];
                _respone.udf8 = _collection["udf8"];
                _respone.udf9 = _collection["udf9"];
                _respone.udf10 = _collection["udf10"];
                _respone.hash = _collection["hash"];
                _respone.field1 = _collection["field1"];
                _respone.field2 = _collection["field2"];
                _respone.field3 = _collection["field3"];
                _respone.field4 = _collection["field4"];
                _respone.field5 = _collection["field5"];
                _respone.field6 = _collection["field6"];
                _respone.field7 = _collection["field7"];
                _respone.field8 = _collection["field8"];
                _respone.field9 = _collection["field9"];
                _respone.payment_source = _collection["payment_source"];
                _respone.PG_TYPE = _collection["PG_TYPE"];
                _respone.bank_ref_num = _collection["bank_ref_num"];
                _respone.bankcode = _collection["bankcode"];
                _respone.error = _collection["error"];
                _respone.error_Message = _collection["error_Message"];
                _respone.name_on_card = _collection["name_on_card"];
                _respone.cardnum = _collection["cardnum"];
                _respone.cardhash = _collection["cardhash"];

                _respone.paymentResponse = JsonConvert.SerializeObject(_respone);

                #endregion


                #region hash calculation
                HdfcPaymentObject objClsPayment = new HdfcPaymentObject();
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                merc_hash_vars_seq = hash_seq.Split('|');
                Array.Reverse(merc_hash_vars_seq);
                merc_hash_string = SALT + "|" + _respone.status;

                foreach (string merc_hash_var in merc_hash_vars_seq)
                {
                    merc_hash_string += "|";
                    merc_hash_string = merc_hash_string + (Convert.ToString(_collection[merc_hash_var]) != null ? _collection[merc_hash_var] : "");
                }
                objClsPayment.amount = _respone.amount;
                objClsPayment.txnid = _respone.txnid;
                objClsPayment.strWithoutHash = merc_hash_string;
                merc_hash = HdfcPaymentFunction.Generatehash512(merc_hash_string).ToLower();
                objClsPayment.strHash512 = merc_hash;
                #endregion
                var _resultDB = _hdfcPaymentRepository.HdfcMatchHashPaymentResponsePG(objClsPayment).Result;
                objClsPayment.PaymentSuccess = false;
                if (_resultDB > 0)
                {
                    objClsPayment.PaymentSuccess = true;
                }
               
                if (objClsPayment.PaymentSuccess)
                {
                    if (merc_hash != _respone.hash)
                    {
                        objClsPayment.PaymentSuccess = false;
                        _respone.status = "failure";
                        var _result = _hdfcPaymentRepository.HdfcPaymentResponsePG(_respone);
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        #region Verify Payment before response
                        string hashstr = MERCHANT_KEY + "|" + command + "|" + _respone.txnid + "|" + SALT;
                        string hash = HdfcPaymentFunction.Generatehash512(hashstr).ToLower();
                        ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                        var request = (HttpWebRequest)WebRequest.Create(PAYU_VERIFY_URL);
                        var postData = "key=" + Uri.EscapeDataString(MERCHANT_KEY);
                        postData += "&hash=" + Uri.EscapeDataString(hash);
                        postData += "&var1=" + Uri.EscapeDataString(_respone.txnid);
                        postData += "&command=" + Uri.EscapeDataString(command);
                        var data = Encoding.ASCII.GetBytes(postData);
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;
                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                        var response = (HttpWebResponse)request.GetResponse();
                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        var _VerifyPayment_RequestData = JsonConvert.DeserializeObject<Root>(responseString);
                        var transaction_details = (Newtonsoft.Json.Linq.JObject)((Newtonsoft.Json.Linq.JContainer)_VerifyPayment_RequestData.transaction_details).First().First();
                        PaymentResponseModel obj = ((JObject)transaction_details).ToObject<PaymentResponseModel>();

                        #endregion

                        _respone.VerifyPaymentResponse = JsonConvert.SerializeObject(transaction_details);
                        _respone.VerifyPaymentStatus = obj.status;
                        if (obj.status.ToLower() != "success")
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        if (obj.txnid != _respone.txnid)
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        if (obj.mihpayid != _respone.mihpayid)
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        var _result = _hdfcPaymentRepository.HdfcPaymentResponsePG(_respone);

                        if (_result != null)
                        {
                            serviceResult.ResultData = _result.Result;
                            if (serviceResult.ResultData.Response == "1")
                            {

                                serviceResult.Message = MessageConfig.PaymentSuccess;
                                serviceResult.Status = true;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.ErrorOccurred;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                            }
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ErrorOccurred;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                        }
                    }
                }
                else
                {
                    _respone.status = "failure";
                    var _result = _hdfcPaymentRepository.HdfcPaymentResponsePG(_respone);
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }

            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Payment Log PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);

        }


        public async Task<ServiceResult> HdfcPaymentDetailPG(string txnid, int studentId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _decrTxnid = Decryption.DecodeHexaValue(txnid);
                var _result = await _hdfcPaymentRepository.HdfcPaymentDetailPG(_decrTxnid);

                if (_result != null)
                {
                    if (_result.StudentId == studentId)
                    {
                        serviceResult.ResultData = _result;
                        if (serviceResult.ResultData != null)
                        {
                            serviceResult.Message = MessageConfig.Success;
                            serviceResult.Status = true;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ErrorOccurred;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.NotAuthorize;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get Student objections PG: ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }


        public async Task<ServiceResult> VerifyPayment(string txnid, PaymentStudentModel model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                var MERCHANT_KEY = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
                var SALT = _configuration.GetValue<string>("HDFC:SALT");
                var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
                string command = "verify_payment";
                string hashstr = MERCHANT_KEY + "|" + command + "|" + txnid + "|" + SALT;
                string hash = HdfcPaymentFunction.Generatehash512(hashstr).ToLower();


                #region Reconcil log
                UgPaymentLog _logModel = new UgPaymentLog();
                _logModel.PaymentMethod = "HDFC";
                _logModel.PaymentRequest = hash;
                _logModel.PaymentRequestDate = DateTime.Now;
                _logModel.PaymentResponse = null;
                _logModel.PaymentResponseDate = null;
                _logModel.PaymentStatus = false;
                _logModel.StudentId = model.StudentId;
                _logModel.TransactionId = txnid;
                _logModel.HashForm = hash;
                _logModel.PaymentMessage = "Pending";
                var _logresponse = _hdfcPaymentRepository.ReconcilePaymentLog(_logModel);
                #endregion

                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                var request = (HttpWebRequest)WebRequest.Create(PAYU_VERIFY_URL);
                var postData = "key=" + Uri.EscapeDataString(MERCHANT_KEY);
                postData += "&hash=" + Uri.EscapeDataString(hash);
                postData += "&var1=" + Uri.EscapeDataString(txnid);
                postData += "&command=" + Uri.EscapeDataString(command);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }


                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                var _VerifyPayment_RequestData = JsonConvert.DeserializeObject<Root>(responseString);
                var transaction_details = (Newtonsoft.Json.Linq.JObject)((Newtonsoft.Json.Linq.JContainer)_VerifyPayment_RequestData.transaction_details).First().First();
                PaymentResponseModel obj = ((JObject)transaction_details).ToObject<PaymentResponseModel>();
                obj.paymentResponse = JsonConvert.SerializeObject(transaction_details);
                obj.hash = hash;
                var _result = _hdfcPaymentRepository.VerifyPayment(obj);

                if (_result != null)
                {
                    if (_result.Result.Response == "1")
                    {
                        serviceResult.ResultData = _result.Result;
                        if (serviceResult.ResultData != null)
                        {
                            serviceResult.Message = MessageConfig.Success;
                            serviceResult.Status = true;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ErrorOccurred;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.InvalidTransaction;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {

                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Verify Payment : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }

            return serviceResult;
        }

        public async Task<ServiceResult> PubliclyVerifyPayment(string txnid)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                var MERCHANT_KEY = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
                var SALT = _configuration.GetValue<string>("HDFC:SALT");
                var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
                string command = "verify_payment";
                string hashstr = MERCHANT_KEY + "|" + command + "|" + txnid + "|" + SALT;
                string hash = HdfcPaymentFunction.Generatehash512(hashstr).ToLower();


                #region Reconcil log
                UgPaymentLog _logModel = new UgPaymentLog();
                _logModel.PaymentMethod = "HDFC";
                _logModel.PaymentRequest = hash;
                _logModel.PaymentRequestDate = DateTime.Now;
                _logModel.PaymentResponse = null;
                _logModel.PaymentResponseDate = null;
                _logModel.PaymentStatus = false;
                _logModel.TransactionId = txnid;
                _logModel.HashForm = hash;
                _logModel.PaymentMessage = "Pending";
                var _logresponse = await _hdfcPaymentRepository.PublicyReconcilePaymentLog(_logModel);
                #endregion

                if (_logresponse == 1)
                {

                    ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                    var request = (HttpWebRequest)WebRequest.Create(PAYU_VERIFY_URL);
                    var postData = "key=" + Uri.EscapeDataString(MERCHANT_KEY);
                    postData += "&hash=" + Uri.EscapeDataString(hash);
                    postData += "&var1=" + Uri.EscapeDataString(txnid);
                    postData += "&command=" + Uri.EscapeDataString(command);
                    var data = Encoding.ASCII.GetBytes(postData);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }


                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                    var _VerifyPayment_RequestData = JsonConvert.DeserializeObject<Root>(responseString);
                    var transaction_details = (Newtonsoft.Json.Linq.JObject)((Newtonsoft.Json.Linq.JContainer)_VerifyPayment_RequestData.transaction_details).First().First();
                    PaymentResponseModel obj = ((JObject)transaction_details).ToObject<PaymentResponseModel>();
                    obj.paymentResponse = JsonConvert.SerializeObject(transaction_details);
                    obj.hash = hash;
                    var _result = _hdfcPaymentRepository.VerifyPayment(obj);

                    if (_result != null)
                    {
                        if (_result.Result.Response == "1")
                        {
                            serviceResult.ResultData = _result.Result;
                            if (serviceResult.ResultData != null)
                            {
                                serviceResult.Message = MessageConfig.Success;
                                serviceResult.Status = true;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.ErrorOccurred;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                            }
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.InvalidTransaction;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                        }
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {

                    serviceResult.Message = MessageConfig.InvalidTransaction;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                }

            }
            catch (Exception ex)
            {

                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Verify Payment : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }

            return serviceResult;
        }

        public async Task<ServiceResult> PubliclyVerifyPaymentPG(string txnid)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                var MERCHANT_KEY = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
                var SALT = _configuration.GetValue<string>("HDFC:SALT");
                var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
                string command = "verify_payment";
                string hashstr = MERCHANT_KEY + "|" + command + "|" + txnid + "|" + SALT;
                string hash = HdfcPaymentFunction.Generatehash512(hashstr).ToLower();


                #region Reconcil log
                PGPaymentLog _logModel = new PGPaymentLog();
                _logModel.PaymentMethod = "HDFC";
                _logModel.PaymentRequest = hash;
                _logModel.PaymentRequestDate = DateTime.Now;
                _logModel.PaymentResponse = null;
                _logModel.PaymentResponseDate = null;
                _logModel.PaymentStatus = false;
                _logModel.TransactionId = txnid;
                _logModel.HashForm = hash;
                _logModel.PaymentMessage = "Pending";
                _logModel.Type = "PG";
                var _logresponse = await _hdfcPaymentRepository.PublicyReconcilePaymentLogPG(_logModel);
                #endregion

                if (_logresponse == 1)
                {

                    ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                    var request = (HttpWebRequest)WebRequest.Create(PAYU_VERIFY_URL);
                    var postData = "key=" + Uri.EscapeDataString(MERCHANT_KEY);
                    postData += "&hash=" + Uri.EscapeDataString(hash);
                    postData += "&var1=" + Uri.EscapeDataString(txnid);
                    postData += "&command=" + Uri.EscapeDataString(command);
                    var data = Encoding.ASCII.GetBytes(postData);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }


                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                    var _VerifyPayment_RequestData = JsonConvert.DeserializeObject<Root>(responseString);
                    var transaction_details = (Newtonsoft.Json.Linq.JObject)((Newtonsoft.Json.Linq.JContainer)_VerifyPayment_RequestData.transaction_details).First().First();
                    PaymentResponseModel obj = ((JObject)transaction_details).ToObject<PaymentResponseModel>();
                    obj.paymentResponse = JsonConvert.SerializeObject(transaction_details);
                    obj.hash = hash;
                    var _result = _hdfcPaymentRepository.VerifyPaymentPG(obj);

                    if (_result != null)
                    {
                        if (_result.Result.Response == "1")
                        {
                            serviceResult.ResultData = _result.Result;
                            if (serviceResult.ResultData != null)
                            {
                                serviceResult.Message = MessageConfig.Success;
                                serviceResult.Status = true;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.ErrorOccurred;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                            }
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.InvalidTransaction;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                        }
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {

                    serviceResult.Message = MessageConfig.InvalidTransaction;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                }

            }
            catch (Exception ex)
            {

                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Verify Payment : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }

            return serviceResult;
        }
        public Task<ServiceResult> HdfcPaymentDetail(string txnid)
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceResult> ReconsileList(string Type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                #region Reconcil log
                
                var _logresponse = await _hdfcPaymentRepository.ReconsileList(Type);
                #endregion

                serviceResult.ResultData = _logresponse;
                if (serviceResult.ResultData != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {

                    serviceResult.Message = MessageConfig.InvalidTransaction;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                }

            }
            catch (Exception ex)
            {

                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Reconsile UG : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }

            return serviceResult;
        }
        //--------------------------  Admission Fee HDFC  ------------------------------------------

        public async Task<ServiceResult> MakeAdmissionPayment(string Amount, HdfcStudentAdmissionModel model)
        {
            ServiceResult serviceResult = new ServiceResult();

            string action1 = string.Empty;
            string hash1 = string.Empty;
            string txnid1 = string.Empty;

            var collegeDet = _hdfcPaymentRepository.HdfcCollegeDetail(model.CollegeId,model.CourseType).Result;

            var MERCHANT_KEY = collegeDet.MERCHANT_KEY;
            var SALT = collegeDet.SALT;
            var PAYU_BASE_URL = _configuration.GetValue<string>("HDFC:PAYU_BASE_URL");
            //var PortalLink = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
            var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
            var PAYU_SuccessUrl = _configuration.GetValue<string>("HDFC:AdmissionUGSuccessUrl");
            try
            {
                string hash_string = string.Empty;
                Random rnd = new Random();
                //string strHash = HdfcPaymentFunction.Generatehash512(rnd.ToString() + DateTime.Now);
                string strHash = HdfcPaymentFunction.Generatehash512(model.StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                txnid1 = model.StudentId + model.CollegeId + model.CourseId + strHash.ToString().Substring(0, 25);
                //hash_string += SALT;// appending SALT
                //"key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10"
                hash_string += MERCHANT_KEY + "|" + txnid1 + "|" + Convert.ToDecimal(Amount).ToString("g29") + "|" + model.Note + "|" + model.Name + "|" + model.Email + "|" + model.CollegeId + "|" + model.CourseId + "|" + model.StudentId + "|"+ model.CourseType + "|||||||" + SALT;

                hash1 = HdfcPaymentFunction.Generatehash512(hash_string).ToLower();         //generating hash
                action1 = PAYU_BASE_URL + "/_payment";// setting URL


                UgPaymentLog _logModel = new UgPaymentLog();
                _logModel.PaymentMethod = "HDFC Admission";
                _logModel.PaymentRequest = hash_string;
                _logModel.PaymentRequestDate = DateTime.Now;
                _logModel.PaymentResponse = null;
                _logModel.PaymentResponseDate = null;
                _logModel.PaymentStatus = false;
                _logModel.StudentId = model.StudentId;
                _logModel.TransactionId = txnid1;
                _logModel.HashForm = hash1;
                _logModel.PaymentMessage = "Pending";
                _logModel.Amount = Amount;

                await PaymentLog(_logModel);
                // PaymentLog

                if (!string.IsNullOrEmpty(hash1))
                {
                    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                    data.Add("hash", hash1);
                    data.Add("txnid", txnid1);
                    data.Add("key", MERCHANT_KEY);
                    string AmountForm = Convert.ToDecimal(Amount).ToString("g29");// eliminating trailing zeros
                                                                                  // amount.Text = AmountForm;
                    data.Add("amount", AmountForm);
                    data.Add("firstname", model.Name);
                    data.Add("email", model.Email);
                    data.Add("phone", model.Phone);
                    data.Add("productinfo", model.Note);
                    //success url
                    data.Add("surl", PAYU_SuccessUrl);
                    //Failed Url
                    data.Add("furl", PAYU_SuccessUrl);
                    data.Add("lastname", "");
                    data.Add("curl", PAYU_SuccessUrl);
                    data.Add("address1", "");
                    data.Add("address2", "");
                    data.Add("city", "");
                    data.Add("state", "");
                    data.Add("country", "");
                    data.Add("zipcode", "");
                    data.Add("udf1", model.CollegeId);
                    data.Add("udf2", model.CourseId);
                    data.Add("udf3", model.StudentId);
                    data.Add("udf4", model.CourseType);
                    data.Add("udf5", "");
                    data.Add("pg", "CC");


                    string strForm = HdfcPaymentFunction.PreparePOSTForm(action1, data);
                    serviceResult.ResultData = strForm;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    //Page.Controls.Add(new LiteralControl(strForm));
                    // return await Task.Run(() => Ok(new { action= action1,data=data }));
                }

                else
                {
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                }
            }
            catch (Exception ex)
            {
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> AdmissionPaymentResponse(IFormCollection _collection)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                #region respone Bind with model

                PaymentResponseModel _respone = new();
                _respone.mihpayid = _collection["mihpayid"];
                _respone.mode = _collection["mode"];
                _respone.status = _collection["status"];
                _respone.unmappedstatus = _collection["unmappedstatus"];
                _respone.key = _collection["key"];
                _respone.txnid = _collection["txnid"];
                _respone.amount = _collection["amount"];
                _respone.cardCategory = _collection["cardCategory"];
                _respone.discount = _collection["discount"];
                _respone.net_amount_debit = _collection["net_amount_debit"];
                _respone.addedon = _collection["addedon"];
                _respone.productinfo = _collection["productinfo"];
                _respone.firstname = _collection["firstname"];
                _respone.lastnam = _collection["lastnam"];
                _respone.address1 = _collection["address1"];
                _respone.address2 = _collection["address2"];
                _respone.city = _collection["city"];
                _respone.state = _collection["state"];
                _respone.country = _collection["country"];
                _respone.zipcode = _collection["zipcode"];
                _respone.email = _collection["email"];
                _respone.phone = _collection["phone"];
                _respone.udf1 = _collection["udf1"];
                _respone.udf2 = _collection["udf2"];
                _respone.udf3 = _collection["udf3"];
                _respone.udf4 = _collection["udf4"];
                _respone.udf5 = _collection["udf5"];
                _respone.udf6 = _collection["udf6"];
                _respone.udf7 = _collection["udf7"];
                _respone.udf8 = _collection["udf8"];
                _respone.udf9 = _collection["udf9"];
                _respone.udf10 = _collection["udf10"];
                _respone.hash = _collection["hash"];
                _respone.field1 = _collection["field1"];
                _respone.field2 = _collection["field2"];
                _respone.field3 = _collection["field3"];
                _respone.field4 = _collection["field4"];
                _respone.field5 = _collection["field5"];
                _respone.field6 = _collection["field6"];
                _respone.field7 = _collection["field7"];
                _respone.field8 = _collection["field8"];
                _respone.field9 = _collection["field9"];
                _respone.payment_source = _collection["payment_source"];
                _respone.PG_TYPE = _collection["PG_TYPE"];
                _respone.bank_ref_num = _collection["bank_ref_num"];
                _respone.bankcode = _collection["bankcode"];
                _respone.error = _collection["error"];
                _respone.error_Message = _collection["error_Message"];
                _respone.name_on_card = _collection["name_on_card"];
                _respone.cardnum = _collection["cardnum"];
                _respone.cardhash = _collection["cardhash"];

                _respone.paymentResponse = JsonConvert.SerializeObject(_respone);

                #endregion

                var collgeModel = _hdfcPaymentRepository.HdfcCollegeDetail(Convert.ToInt32(_respone.udf1), Convert.ToInt32(_respone.udf1)).Result;


                var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
                var MERCHANT_KEY = collgeModel.MERCHANT_KEY;
                var SALT = collgeModel.SALT;
                var hash_seq = _configuration.GetValue<string>("HDFC:hashSequence");
                string command = "verify_payment";

                #region hash calculation
                HdfcPaymentObject objClsPayment = new HdfcPaymentObject();
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                merc_hash_vars_seq = hash_seq.Split('|');
                Array.Reverse(merc_hash_vars_seq);
                merc_hash_string = SALT + "|" + _respone.status;

                foreach (string merc_hash_var in merc_hash_vars_seq)
                {
                    merc_hash_string += "|";
                    merc_hash_string = merc_hash_string + (Convert.ToString(_collection[merc_hash_var]) != null ? _collection[merc_hash_var] : "");
                }
                objClsPayment.amount = _respone.amount;
                objClsPayment.txnid = _respone.txnid;
                objClsPayment.strWithoutHash = merc_hash_string;
                merc_hash = HdfcPaymentFunction.Generatehash512(merc_hash_string).ToLower();
                objClsPayment.strHash512 = merc_hash;
                #endregion
                var _resultDB = _hdfcPaymentRepository.HdfcMatchHashPaymentResponse(objClsPayment);
                objClsPayment.PaymentSuccess = false;
                if (_resultDB.Status > 0)
                {
                    objClsPayment.PaymentSuccess = true;
                }

                if (objClsPayment.PaymentSuccess)
                {
                    if (merc_hash != _respone.hash)
                    {
                        objClsPayment.PaymentSuccess = false;
                        _respone.status = "failure";
                        var _result = _hdfcPaymentRepository.HdfcPaymentResponse(_respone);
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        #region Verify Payment before response
                        string hashstr = MERCHANT_KEY + "|" + command + "|" + _respone.txnid + "|" + SALT;
                        string hash = HdfcPaymentFunction.Generatehash512(hashstr).ToLower();
                        ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                        var request = (HttpWebRequest)WebRequest.Create(PAYU_VERIFY_URL);
                        var postData = "key=" + Uri.EscapeDataString(MERCHANT_KEY);
                        postData += "&hash=" + Uri.EscapeDataString(hash);
                        postData += "&var1=" + Uri.EscapeDataString(_respone.txnid);
                        postData += "&command=" + Uri.EscapeDataString(command);
                        var data = Encoding.ASCII.GetBytes(postData);
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;
                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                        var response = (HttpWebResponse)request.GetResponse();
                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        var _VerifyPayment_RequestData = JsonConvert.DeserializeObject<Root>(responseString);
                        var transaction_details = (Newtonsoft.Json.Linq.JObject)((Newtonsoft.Json.Linq.JContainer)_VerifyPayment_RequestData.transaction_details).First().First();
                        PaymentResponseModel obj = ((JObject)transaction_details).ToObject<PaymentResponseModel>();

                        #endregion

                        _respone.VerifyPaymentResponse = JsonConvert.SerializeObject(transaction_details);
                        _respone.VerifyPaymentStatus = obj.status;
                        if (obj.status.ToLower() != "success")
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        if (obj.txnid != _respone.txnid)
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        if (obj.mihpayid != _respone.mihpayid)
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        var _result = _hdfcPaymentRepository.HdfcPaymentResponse(_respone);

                        if (_result != null)
                        {
                            serviceResult.ResultData = _result.Result;
                            if (serviceResult.ResultData.Response == "1")
                            {

                                serviceResult.Message = MessageConfig.PaymentSuccess;
                                serviceResult.Status = true;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.ErrorOccurred;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                            }
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ErrorOccurred;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                        }
                    }
                }
                else
                {
                    _respone.status = "failure";
                    var _result = _hdfcPaymentRepository.HdfcPaymentResponse(_respone);
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Payment Log : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);

        }

        public async Task<ServiceResult> MakeAdmissionPaymentPG(string Amount, HdfcStudentAdmissionModel model)
        {
            ServiceResult serviceResult = new ServiceResult();

            string action1 = string.Empty;
            string hash1 = string.Empty;
            string txnid1 = string.Empty;

            var collegeDet = _hdfcPaymentRepository.HdfcCollegeDetail(model.CollegeId, model.CourseType).Result;

            var MERCHANT_KEY = collegeDet.MERCHANT_KEY;
            var SALT = collegeDet.SALT;
            var PAYU_BASE_URL = _configuration.GetValue<string>("HDFC:PAYU_BASE_URL");
            //var PortalLink = _configuration.GetValue<string>("HDFC:MERCHANT_KEY");
            var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
            var PAYU_SuccessUrl = _configuration.GetValue<string>("HDFC:AdmissionPGSuccessUrl");
            try
            {
                string hash_string = string.Empty;
                Random rnd = new Random();
                //string strHash = HdfcPaymentFunction.Generatehash512(rnd.ToString() + DateTime.Now);
                string strHash = HdfcPaymentFunction.Generatehash512(model.StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond);
                txnid1 = model.StudentId + model.CollegeId + model.CourseId + strHash.ToString().Substring(0, 25);
                //hash_string += SALT;// appending SALT
                //"key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10"
                //sha512(key | txnid | amount | productinfo | firstname | email | udf1 | udf2 | udf3 | udf4 | udf5 |||||| SALT)
                hash_string += MERCHANT_KEY + "|" + txnid1 + "|" + Convert.ToDecimal(Amount).ToString("g29") + "|" + model.Note + "|" + model.Name + "|" +
                    model.Email + "|" + model.CollegeId + "|" + model.CourseId + "|" + model.StudentId + "|"+ model.CourseType + "|||||||" + SALT;

                hash1 = HdfcPaymentFunction.Generatehash512(hash_string).ToLower();         //generating hash
                action1 = PAYU_BASE_URL + "/_payment";// setting URL


                UgPaymentLog _logModel = new UgPaymentLog();
                _logModel.PaymentMethod = "HDFC Admission";
                _logModel.PaymentRequest = hash_string;
                _logModel.PaymentRequestDate = DateTime.Now;
                _logModel.PaymentResponse = null;
                _logModel.PaymentResponseDate = null;
                _logModel.PaymentStatus = false;
                _logModel.StudentId = model.StudentId;
                _logModel.TransactionId = txnid1;
                _logModel.HashForm = hash1;
                _logModel.PaymentMessage = "Pending";
                _logModel.Amount = Amount;

                await PaymentLogPG(_logModel);
                // PaymentLog

                if (!string.IsNullOrEmpty(hash1))
                {
                    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                    data.Add("hash", hash1);
                    data.Add("txnid", txnid1);
                    data.Add("key", MERCHANT_KEY);
                    string AmountForm = Convert.ToDecimal(Amount).ToString("g29");// eliminating trailing zeros
                                                                                  // amount.Text = AmountForm;
                    data.Add("amount", AmountForm);
                    data.Add("firstname", model.Name);
                    data.Add("email", model.Email);
                    data.Add("phone", model.Phone);
                    data.Add("productinfo", model.Note);
                    //success url
                    data.Add("surl", PAYU_SuccessUrl);
                    //Failed Url
                    data.Add("furl", PAYU_SuccessUrl);
                    data.Add("lastname", "");
                    data.Add("curl", PAYU_SuccessUrl);
                    data.Add("address1", "");
                    data.Add("address2", "");
                    data.Add("city", "");
                    data.Add("state", "");
                    data.Add("country", "");
                    data.Add("zipcode", "");
                    data.Add("udf1", model.CollegeId);
                    data.Add("udf2", model.CourseId);
                    data.Add("udf3", model.StudentId);
                    data.Add("udf4", model.CourseType);
                    data.Add("udf5", "");
                    data.Add("pg", "CC");


                    string strForm = HdfcPaymentFunction.PreparePOSTForm(action1, data);
                    serviceResult.ResultData = strForm;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    //Page.Controls.Add(new LiteralControl(strForm));
                    // return await Task.Run(() => Ok(new { action= action1,data=data }));
                }

                else
                {
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                }
            }
            catch (Exception ex)
            {
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> AdmissionPaymentResponsePG(IFormCollection _collection)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                #region respone Bind with model

                PaymentResponseModel _respone = new();
                _respone.mihpayid = _collection["mihpayid"];
                _respone.mode = _collection["mode"];
                _respone.status = _collection["status"];
                _respone.unmappedstatus = _collection["unmappedstatus"];
                _respone.key = _collection["key"];
                _respone.txnid = _collection["txnid"];
                _respone.amount = _collection["amount"];
                _respone.cardCategory = _collection["cardCategory"];
                _respone.discount = _collection["discount"];
                _respone.net_amount_debit = _collection["net_amount_debit"];
                _respone.addedon = _collection["addedon"];
                _respone.productinfo = _collection["productinfo"];
                _respone.firstname = _collection["firstname"];
                _respone.lastnam = _collection["lastnam"];
                _respone.address1 = _collection["address1"];
                _respone.address2 = _collection["address2"];
                _respone.city = _collection["city"];
                _respone.state = _collection["state"];
                _respone.country = _collection["country"];
                _respone.zipcode = _collection["zipcode"];
                _respone.email = _collection["email"];
                _respone.phone = _collection["phone"];
                _respone.udf1 = _collection["udf1"];
                _respone.udf2 = _collection["udf2"];
                _respone.udf3 = _collection["udf3"];
                _respone.udf4 = _collection["udf4"];
                _respone.udf5 = _collection["udf5"];
                _respone.udf6 = _collection["udf6"];
                _respone.udf7 = _collection["udf7"];
                _respone.udf8 = _collection["udf8"];
                _respone.udf9 = _collection["udf9"];
                _respone.udf10 = _collection["udf10"];
                _respone.hash = _collection["hash"];
                _respone.field1 = _collection["field1"];
                _respone.field2 = _collection["field2"];
                _respone.field3 = _collection["field3"];
                _respone.field4 = _collection["field4"];
                _respone.field5 = _collection["field5"];
                _respone.field6 = _collection["field6"];
                _respone.field7 = _collection["field7"];
                _respone.field8 = _collection["field8"];
                _respone.field9 = _collection["field9"];
                _respone.payment_source = _collection["payment_source"];
                _respone.PG_TYPE = _collection["PG_TYPE"];
                _respone.bank_ref_num = _collection["bank_ref_num"];
                _respone.bankcode = _collection["bankcode"];
                _respone.error = _collection["error"];
                _respone.error_Message = _collection["error_Message"];
                _respone.name_on_card = _collection["name_on_card"];
                _respone.cardnum = _collection["cardnum"];
                _respone.cardhash = _collection["cardhash"];

                _respone.paymentResponse = JsonConvert.SerializeObject(_respone);

                #endregion

                var collgeModel = _hdfcPaymentRepository.HdfcCollegeDetail(Convert.ToInt32(_respone.udf1), Convert.ToInt32(_respone.udf1)).Result;


                var PAYU_VERIFY_URL = _configuration.GetValue<string>("HDFC:PAYU_VERIFY_URL");
                var MERCHANT_KEY = collgeModel.MERCHANT_KEY;
                var SALT = collgeModel.SALT;
                var hash_seq = _configuration.GetValue<string>("HDFC:hashSequence");
                string command = "verify_payment";

                #region hash calculation
                HdfcPaymentObject objClsPayment = new HdfcPaymentObject();
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                merc_hash_vars_seq = hash_seq.Split('|');
                Array.Reverse(merc_hash_vars_seq);
                merc_hash_string = SALT + "|" + _respone.status;

                foreach (string merc_hash_var in merc_hash_vars_seq)
                {
                    merc_hash_string += "|";
                    merc_hash_string = merc_hash_string + (Convert.ToString(_collection[merc_hash_var]) != null ? _collection[merc_hash_var] : "");
                }
                objClsPayment.amount = _respone.amount;
                objClsPayment.txnid = _respone.txnid;
                objClsPayment.strWithoutHash = merc_hash_string;
                merc_hash = HdfcPaymentFunction.Generatehash512(merc_hash_string).ToLower();
                objClsPayment.strHash512 = merc_hash;
                #endregion
                var _resultDB = _hdfcPaymentRepository.HdfcMatchHashPaymentResponsePG(objClsPayment);
                objClsPayment.PaymentSuccess = false;
                if (_resultDB.Status > 0)
                {
                    objClsPayment.PaymentSuccess = true;
                }

                if (objClsPayment.PaymentSuccess)
                {
                    if (merc_hash != _respone.hash)
                    {
                        objClsPayment.PaymentSuccess = false;
                        _respone.status = "failure";
                        var _result = _hdfcPaymentRepository.HdfcPaymentResponsePG(_respone);
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                    else
                    {
                        #region Verify Payment before response
                        string hashstr = MERCHANT_KEY + "|" + command + "|" + _respone.txnid + "|" + SALT;
                        string hash = HdfcPaymentFunction.Generatehash512(hashstr).ToLower();
                        ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                        var request = (HttpWebRequest)WebRequest.Create(PAYU_VERIFY_URL);
                        var postData = "key=" + Uri.EscapeDataString(MERCHANT_KEY);
                        postData += "&hash=" + Uri.EscapeDataString(hash);
                        postData += "&var1=" + Uri.EscapeDataString(_respone.txnid);
                        postData += "&command=" + Uri.EscapeDataString(command);
                        var data = Encoding.ASCII.GetBytes(postData);
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;
                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                        var response = (HttpWebResponse)request.GetResponse();
                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        var _VerifyPayment_RequestData = JsonConvert.DeserializeObject<Root>(responseString);
                        var transaction_details = (Newtonsoft.Json.Linq.JObject)((Newtonsoft.Json.Linq.JContainer)_VerifyPayment_RequestData.transaction_details).First().First();
                        PaymentResponseModel obj = ((JObject)transaction_details).ToObject<PaymentResponseModel>();

                        #endregion

                        _respone.VerifyPaymentResponse = JsonConvert.SerializeObject(transaction_details);
                        _respone.VerifyPaymentStatus = obj.status;
                        if (obj.status.ToLower() != "success")
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        if (obj.txnid != _respone.txnid)
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        if (obj.mihpayid != _respone.mihpayid)
                        {
                            _respone.error_Message = obj.error_Message;
                            _respone.error = obj.error;
                        }
                        var _result = _hdfcPaymentRepository.HdfcPaymentResponsePG(_respone);

                        if (_result != null)
                        {
                            serviceResult.ResultData = _result.Result;
                            if (serviceResult.ResultData.Response == "1")
                            {

                                serviceResult.Message = MessageConfig.PaymentSuccess;
                                serviceResult.Status = true;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.ErrorOccurred;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                            }
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ErrorOccurred;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                        }
                    }
                }
                else
                {
                    _respone.status = "failure";
                    var _result = _hdfcPaymentRepository.HdfcPaymentResponse(_respone);
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Payment Log : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);

        }


    }
}
