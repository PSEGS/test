using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.Service
{
    public class PaygovService : IPaygovService
    {

        private readonly IConfiguration _configuration;
        private readonly IPayGovPaymentRepository _paygovPaymentRepository;
        private readonly ILogError _logError;
        private readonly string MerchantId = string.Empty;
        private readonly string admissionServiceId = string.Empty;
        private readonly string BillDeskUrl = string.Empty;
        private readonly string CheckSumKey = string.Empty;
        private readonly string successUrl = string.Empty;
        private readonly string checkSum = string.Empty;
        private readonly string ReturnUrl = string.Empty;
        private readonly string PAYU_VERIFY_URL = string.Empty;
        private readonly string PAYU_PasswordL = string.Empty;
        private readonly string successUrlPG = string.Empty;
        public PaygovService(IConfiguration configuration, ILogError logError, IPayGovPaymentRepository payGovPaymentRepository)
        {
            _logError = logError;
            _configuration = configuration;
            _paygovPaymentRepository = payGovPaymentRepository;
            MerchantId = _configuration.GetValue<string>("PayGovSettings:MerchantID");
            BillDeskUrl = _configuration.GetValue<string>("PayGovSettings:PaymentURL");
            admissionServiceId = _configuration.GetValue<string>("PayGovSettings:ServiceId");
            CheckSumKey = _configuration.GetValue<string>("PayGovSettings:SecurityKey");
            successUrl = _configuration.GetValue<string>("PayGovSettings:SuccessURL");
            successUrlPG = _configuration.GetValue<string>("PayGovSettings:SuccessURLPG");
            checkSum = _configuration.GetValue<string>("PayGovSettings:SecurityKey");
            PAYU_VERIFY_URL = _configuration.GetValue<string>("PayGovSettings:PaymentVerificationUrl");
            PAYU_PasswordL = _configuration.GetValue<string>("PayGovSettings:Password");
        }
        public async Task<ServiceResult> MakePaygovRegistrationPayment(string Amount, PaymentStudentModel model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string CurrencyType = "INR";

                Random rnd = new Random();
                //string strHash = HdfcPaymentFunction.Generatehash512(rnd.ToString() + DateTime.Now);
                string strHash = generateCRC32Checksum(model.StudentId + rnd.ToString() + DateTime.Now + DateTime.Now.Second + DateTime.Now.Millisecond, CheckSumKey);

                string orderId = "PAYGOV" + model.StudentId + model.StudentType.ToUpper() + strHash.ToString();
                string StudentID = Convert.ToString(model.StudentId);
                string additionalField1 = model.StudentType;
                string additionalField2 = model.Name;
                string additionalField3 = model.Phone;
                string additionalField4 = model.Email;
                string additionalField5 = model.Note;
                string currentDateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:sss");
                string url = model.StudentType.ToUpper() == "UG" ? successUrl : successUrlPG;

                string paymentstring = "0100" + "|" + MerchantId + "|" + admissionServiceId + "|" + orderId + "|" + StudentID + "|" + Amount + "|INR|" + currentDateTime
                    + "|" + url + "|" + url + "|" + additionalField1 + "|" + additionalField2 + "|" + additionalField3 + "|" + additionalField4 + "|" + additionalField5;

                string urlMsg = paymentstring + "|" + generateCRC32Checksum(paymentstring, CheckSumKey).ToUpper();
                string checkSumId = generateCRC32Checksum(paymentstring, CheckSumKey).ToUpper();
                StringBuilder sb = new StringBuilder();

                sb.Append("<html><header>");
                sb.AppendFormat(@"<header>");
                string formID = "PostForm";
                sb.Append("<body><form id=" + formID + " name=" + formID + " action=" + BillDeskUrl + " method=POST>");

                sb.AppendFormat("<input type='text' id='checksum' name='checksum' value='{0}'/>", checkSumId);
                sb.AppendFormat("<input type='text' id='messageType' name='messageType' value='{0}'/>", "0100");
                sb.AppendFormat("<input type='text' id='merchantId' name='merchantId' value='{0}'/>", MerchantId);
                sb.AppendFormat("<input type='text' id='serviceId' name='serviceId' value='{0}'/>", admissionServiceId);
                sb.AppendFormat("<input type='text' id='orderId' name='orderId' value='{0}'/>", orderId);
                sb.AppendFormat("<input type='text' id='customerId' name='customerId' value='{0}'/>", StudentID);
                sb.AppendFormat("<input type='text' id='transactionAmount' name='transactionAmount' value='{0}'/>", Amount);
                sb.AppendFormat("<input type='text' id='currencyCode' name='currencyCode' value='{0}'/>", CurrencyType);
                sb.AppendFormat("<input type='text' id='requestDateTime' name='requestDateTime' value='{0}'/>", currentDateTime);
                sb.AppendFormat("<input type='text' id='successUrl' name='successUrl' value='{0}'/>", url);
                sb.AppendFormat("<input type='text' id='failUrl' name='failUrl' value='{0}'/>", url);
                sb.AppendFormat("<input type='text' id='additionalField1' name='additionalField1' value='{0}'/>", additionalField1);
                sb.AppendFormat("<input type='text' id='additionalField2' name='additionalField2' value='{0}'/>", additionalField2);
                sb.AppendFormat("<input type='text' id='additionalField3' name='additionalField3' value='{0}'/>", additionalField3);
                sb.AppendFormat("<input type='text' id='additionalField4' name='additionalField4' value='{0}'/>", additionalField4);
                sb.AppendFormat("<input type='text' id='additionalField5' name='additionalField5' value='{0}'/>", additionalField5);
                sb.AppendFormat("</form></body></html>");

                UgPaymentLog _logModel = new UgPaymentLog();
                _logModel.PaymentMethod = "PAYGOV";
                _logModel.PaymentRequest = paymentstring;
                _logModel.PaymentRequestDate = DateTime.Now;
                _logModel.PaymentResponse = null;
                _logModel.PaymentResponseDate = null;
                _logModel.PaymentStatus = false;
                _logModel.StudentId = model.StudentId;
                _logModel.TransactionId = orderId;
                _logModel.HashForm = checkSumId;
                _logModel.PaymentMessage = "Payment Initiated";
                _logModel.Amount = Amount;
                _logModel.StudentType = model.StudentType;

                var resultLog = await PaymentLog(_logModel);
                if (resultLog.Status)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append("<script language='javascript'>");
                    strScript.Append("var v" + formID + " = document." + formID + ";");
                    strScript.Append("v" + formID + ".submit();");
                    strScript.Append("</script>");
                    serviceResult.ResultData = sb.ToString() + strScript.ToString();
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.Status = true;
                }
                else
                {
                    serviceResult.ResultData = string.Empty;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.Status = false;
                }

            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                serviceResult.ResultData = ex.Message.ToString();
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.Status = false;
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> ReConcilePayGovPaymentUG(verifyPaymentModel _verifymodel)
        {
            ServiceResult serviceResult = new ServiceResult();
            PayGovPaymentModel _response = new PayGovPaymentModel();
            PaymentStudentModel _student = null;
            try
            {
                if (string.IsNullOrEmpty(_verifymodel.SearchDate) && !string.IsNullOrEmpty(_verifymodel.OrderId))
                {
                    var _verifyResponse = await VerifySinglePayment(_verifymodel.BankTransactionNo, _verifymodel.OrderId);
                    if (_verifyResponse.StatusCode == (int)HttpStatusCode.OK)
                    {
                        _student = new PaymentStudentModel();
                        _response = ConvertPaymentToResponseModel(_verifyResponse.ResultData);
                        _student.StudentId = string.IsNullOrEmpty(_response.CustomerId) ? 0 : Convert.ToInt32(_response.CustomerId);
                        _student.StudentType = "UG";
                        var _resultDB = _paygovPaymentRepository.PayGovMatchHashPaymentResponse(_response, _student);
                        _response.PaymentSuccess = false;
                        if (_resultDB.Status > 0)
                        {
                            _response.PaymentSuccess = true;
                        }
                        PaymentResponseModel _model = new PaymentResponseModel();
                        _model.amount = _response.TransactionAmount;
                        _model.net_amount_debit = _response.TransactionAmount;
                        _model.email = _response.AdditionalInfo4;
                        _model.phone = _response.AdditionalInfo3;
                        _model.firstname = _response.AdditionalInfo2;
                        _model.txnid = _response.OrderId;

                        _model.mihpayid = _response.BankTransactionNo;
                        _model.key = _response.BankTransactionNo;
                        _model.mode = _response.PaymentMode;
                        _model.productinfo = "Registration Fee";
                        _model.payment_source = _response.PaymentMode;
                        _model.status = _response.SuccessFlag == "S" ? "Success" : _response.SuccessFlag == "F" ? "Failed" : _response.SuccessFlag == "D" ? "Failed" : "Pending";
                        _model.error_Message = _response.ErrorDescription;
                        _model.error = _response.ErrorCode;
                        _model.hash = _response.CheckSum;
                        _model.udf4 = _response.CustomerId;
                        _model.field9 = _response.ResponseDateTime;
                        _model.VerifyPaymentStatus = _model.status;
                        var _result = _paygovPaymentRepository.PayGovPaymentResponse(_model, _student);
                        if (_result != null)
                        {
                            serviceResult.ResultData = _result.Result;
                            if (serviceResult.ResultData.Response == "1")
                            {
                                if (_model.status == "Success")
                                {
                                    serviceResult.Message = MessageConfig.PaymentSuccess;
                                }
                                else
                                {
                                    serviceResult.Message = _model.error_Message;
                                }
                                serviceResult.Status = true;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                            }
                            else if (serviceResult.ResultData.Response == "2")
                            {
                                serviceResult.Message = MessageConfig.ReconcilePaymentFailed;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
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
                    else if (_verifyResponse.StatusCode == (int)HttpStatusCode.NotFound)
                    {
                        serviceResult.Message = MessageConfig.NoRecord;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ServiceNotAvailable;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_verifymodel.SearchDate))
                    {
                        serviceResult.Message = MessageConfig.NoRecord;
                        serviceResult.ResultData = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        var _filterPendindTransaction = await _paygovPaymentRepository.FindPendingTransactionUG(_verifymodel.SearchDate);
                        foreach (var item in _filterPendindTransaction)
                        {
                            _student = new PaymentStudentModel();
                            var _verifyResponse = await VerifySinglePayment(item.BankTransactionNo, item.OrderId);
                            if (_verifyResponse.StatusCode == (int)HttpStatusCode.OK)
                            {
                                _response = ConvertPaymentToResponseModel(_verifyResponse.ResultData);
                                _student.StudentId = string.IsNullOrEmpty(_response.CustomerId) ? 0 : Convert.ToInt32(_response.CustomerId);
                                _student.StudentType = "UG";
                                var _resultDB = _paygovPaymentRepository.PayGovMatchHashPaymentResponse(_response, _student);
                                _response.PaymentSuccess = false;
                                if (_resultDB.Status > 0)
                                {
                                    _response.PaymentSuccess = true;
                                }
                                PaymentResponseModel _model = new PaymentResponseModel();
                                _model.amount = _response.TransactionAmount;
                                _model.net_amount_debit = _response.TransactionAmount;
                                _model.email = _response.AdditionalInfo4;
                                _model.phone = _response.AdditionalInfo3;
                                _model.firstname = _response.AdditionalInfo2;
                                _model.txnid = _response.OrderId;

                                _model.mihpayid = _response.BankTransactionNo;
                                _model.key = _response.BankTransactionNo;
                                _model.mode = _response.PaymentMode;
                                _model.productinfo = "Registration Fee";
                                _model.payment_source = _response.PaymentMode;
                                _model.status = _response.SuccessFlag == "S" ? "Success" : _response.SuccessFlag == "F" ? "Failed" : _response.SuccessFlag == "D" ? "Failed" : "Pending";
                                _model.error_Message = _response.ErrorDescription;
                                _model.error = _response.ErrorCode;
                                _model.hash = _response.CheckSum;
                                _model.udf4 = _response.CustomerId;
                                _model.field9 = _response.ResponseDateTime;
                                _model.VerifyPaymentStatus = _model.status;
                                var _result = _paygovPaymentRepository.PayGovPaymentResponse(_model, _student);
                                if (_result != null)
                                {
                                    serviceResult.ResultData = _result.Result;
                                    if (serviceResult.ResultData.Response == "1")
                                    {

                                        if (_model.status == "Success")
                                        {
                                            serviceResult.Message = MessageConfig.PaymentSuccess;
                                        }
                                        else
                                        {
                                            serviceResult.Message = _model.error_Message;
                                        }
                                        serviceResult.Status = true;
                                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                                    }
                                    else if (serviceResult.ResultData.Response == "2")
                                    {
                                        serviceResult.Message = MessageConfig.ReconcilePaymentFailed;
                                        serviceResult.ResultData = null;
                                        serviceResult.Status = false;
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
                            else if (_verifyResponse.StatusCode == (int)HttpStatusCode.NotFound)
                            {
                                serviceResult.Message = MessageConfig.NoRecord;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                            }
                            else
                            {
                                serviceResult.Message = MessageConfig.ServiceNotAvailable;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                            }
                        }
                        serviceResult.Message = MessageConfig.Success;
                        serviceResult.ResultData = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                serviceResult.ResultData = ex.Message.ToString();
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.Status = false;
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> ReConcilePayGovPaymentPG(verifyPaymentModel _verifymodel)
        {
            ServiceResult serviceResult = new ServiceResult();
            PayGovPaymentModel _response = new PayGovPaymentModel();
            PaymentStudentModel _student = null;
            try
            {
                if (string.IsNullOrEmpty(_verifymodel.SearchDate) && !string.IsNullOrEmpty(_verifymodel.OrderId))
                {
                    var _verifyResponse = await VerifySinglePayment(_verifymodel.BankTransactionNo, _verifymodel.OrderId);
                    if (_verifyResponse.StatusCode == (int)HttpStatusCode.OK)
                    {
                        _student = new PaymentStudentModel();
                        _response = ConvertPaymentToResponseModel(_verifyResponse.ResultData);
                        _student.StudentId = string.IsNullOrEmpty(_response.CustomerId) ? 0 : Convert.ToInt32(_response.CustomerId);
                        _student.StudentType = "PG";
                        var _resultDB = _paygovPaymentRepository.PayGovMatchHashPaymentResponse(_response, _student);
                        _response.PaymentSuccess = false;
                        if (_resultDB.Status > 0)
                        {
                            _response.PaymentSuccess = true;
                        }
                        PaymentResponseModel _model = new PaymentResponseModel();
                        _model.amount = _response.TransactionAmount;
                        _model.net_amount_debit = _response.TransactionAmount;
                        _model.email = _response.AdditionalInfo4;
                        _model.phone = _response.AdditionalInfo3;
                        _model.firstname = _response.AdditionalInfo2;
                        _model.txnid = _response.OrderId;

                        _model.mihpayid = _response.BankTransactionNo;
                        _model.key = _response.BankTransactionNo;
                        _model.mode = _response.PaymentMode;
                        _model.productinfo = "Registration Fee";
                        _model.payment_source = _response.PaymentMode;
                        _model.status = _response.SuccessFlag == "S" ? "Success" : _response.SuccessFlag == "F" ? "Failed" : _response.SuccessFlag == "D" ? "Failed" : "Pending";
                        _model.error_Message = _response.ErrorDescription;
                        _model.error = _response.ErrorCode;
                        _model.hash = _response.CheckSum;
                        _model.udf4 = _response.CustomerId;
                        _model.field9 = _response.ResponseDateTime;
                        _model.VerifyPaymentStatus = _model.status;
                        var _result = _paygovPaymentRepository.PayGovPaymentResponse(_model, _student);
                        if (_result != null)
                        {
                            serviceResult.ResultData = _result.Result;
                            if (serviceResult.ResultData.Response == "1")
                            {
                                if (_model.status == "Success")
                                {
                                    serviceResult.Message = MessageConfig.PaymentSuccess;
                                }
                                else
                                {
                                    serviceResult.Message = _model.error_Message;
                                }
                                serviceResult.Status = true;
                                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                            }
                            else if (serviceResult.ResultData.Response == "2")
                            {
                                serviceResult.Message = MessageConfig.ReconcilePaymentFailed;
                                serviceResult.ResultData = null;
                                serviceResult.Status = false;
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
                    else if (_verifyResponse.StatusCode == (int)HttpStatusCode.NotFound)
                    {
                        serviceResult.Message = MessageConfig.NoRecord;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ServiceNotAvailable;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                    }
                }
                else
                {
                    var _filterPendindTransaction = await _paygovPaymentRepository.FindPendingTransactionPG(_verifymodel.SearchDate);
                    foreach (var item in _filterPendindTransaction)
                    {
                        var _verifyResponse = await VerifySinglePayment(item.BankTransactionNo, item.OrderId);
                        if (_verifyResponse.StatusCode == (int)HttpStatusCode.OK)
                        {
                            _student = new PaymentStudentModel();
                            _response = ConvertPaymentToResponseModel(_verifyResponse.ResultData);
                            _student.StudentId = string.IsNullOrEmpty(_response.CustomerId) ? 0 : Convert.ToInt32(_response.CustomerId);
                            _student.StudentType = "PG";
                            var _resultDB = _paygovPaymentRepository.PayGovMatchHashPaymentResponse(_response, _student);
                            _response.PaymentSuccess = false;
                            if (_resultDB.Status > 0)
                            {
                                _response.PaymentSuccess = true;
                            }
                            PaymentResponseModel _model = new PaymentResponseModel();
                            _model.amount = _response.TransactionAmount;
                            _model.net_amount_debit = _response.TransactionAmount;
                            _model.email = _response.AdditionalInfo4;
                            _model.phone = _response.AdditionalInfo3;
                            _model.firstname = _response.AdditionalInfo2;
                            _model.txnid = _response.OrderId;

                            _model.mihpayid = _response.BankTransactionNo;
                            _model.key = _response.BankTransactionNo;
                            _model.mode = _response.PaymentMode;
                            _model.productinfo = "Registration Fee";
                            _model.payment_source = _response.PaymentMode;
                            _model.status = _response.SuccessFlag == "S" ? "Success" : _response.SuccessFlag == "F" ? "Failed" : "Pending";
                            _model.error_Message = _response.ErrorDescription;
                            _model.error = _response.ErrorCode;
                            _model.hash = _response.CheckSum;
                            _model.udf4 = _response.CustomerId;
                            _model.field9 = _response.ResponseDateTime;
                            _model.VerifyPaymentStatus = _model.status;
                            var _result = _paygovPaymentRepository.PayGovPaymentResponse(_model, _student);
                            if (_result != null)
                            {
                                serviceResult.ResultData = _result.Result;
                                if (serviceResult.ResultData.Response == "1")
                                {
                                    if (_model.status == "Success")
                                    {
                                        serviceResult.Message = MessageConfig.PaymentSuccess;
                                    }
                                    else
                                    {
                                        serviceResult.Message = _model.error_Message;
                                    }
                                    serviceResult.Status = true;
                                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                                }
                                else if (serviceResult.ResultData.Response == "2")
                                {
                                    serviceResult.Message = MessageConfig.ReconcilePaymentFailed;
                                    serviceResult.ResultData = null;
                                    serviceResult.Status = false;
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
                        else if (_verifyResponse.StatusCode == (int)HttpStatusCode.NotFound)
                        {
                            serviceResult.Message = MessageConfig.NoRecord;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ServiceNotAvailable;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                        }
                    }
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = null;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                serviceResult.ResultData = ex.Message.ToString();
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.Status = false;
            }
            return await Task.Run(() => serviceResult);
        }
        private async Task<ServiceResult> VerifySinglePayment(string BankTransactionNo, string OrderId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string reverifyRequestParm = string.Empty;
                if (!string.IsNullOrEmpty(BankTransactionNo))
                {
                    reverifyRequestParm = BankTransactionNo + "|" + MerchantId + "|" + OrderId;
                }
                else
                {
                    reverifyRequestParm = "|" + MerchantId + "|" + OrderId;
                }
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                var request = (HttpWebRequest)WebRequest.Create(PAYU_VERIFY_URL);
                var postData = "requestMsg=" + Uri.EscapeDataString(reverifyRequestParm);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(MerchantId + ":" + PAYU_PasswordL));
                request.Headers.Add("Authorization", "Basic " + encoded);
                request.PreAuthenticate = true;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var responseMsg = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(responseMsg.GetResponseStream()).ReadToEnd();
                serviceResult.Message = MessageConfig.Success;
                serviceResult.ResultData = responseString;
                serviceResult.Status = true;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                //S|0100|UATPSGSSG0000001455|admission fee|PAYGOV_6_886065575|6|200.00|INR|Debit-Card|27-06-2022 09:18:50|68873|pay_JmNOVSvCQxGNGZ|A|UG|chetan verma|9463170381|verma.chetan88+9@email.com|Registration Fee|||4094841067

            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToLower() == "The remote server returned an error: (404) 404.".ToLower())
                {
                    serviceResult.Message = MessageConfig.NoRecord;
                    serviceResult.ResultData = null;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            return await Task.Run(() => serviceResult);
        }
        private PayGovPaymentModel ConvertPaymentToResponseModel(string msg)
        {
            PayGovPaymentModel _response = new PayGovPaymentModel();
            try
            {
                string[] response = msg.Split('|');
                string checksum = msg.Remove(0, msg.LastIndexOf("|") + 1);
                string _CalculatedHash = generateCRC32Checksum(msg.Substring(0, msg.LastIndexOf('|')), checkSum);

                if (checksum == _CalculatedHash)
                {
                    _response.SuccessFlag = Convert.ToString(response[0]);
                    if (response[0] == "F" || response[0] == "P" || response[0] == "D" || response[0] == "I")
                    {
                        _response.SurePayMerchantId = Convert.ToString(response[1]);
                        _response.OrderId = Convert.ToString(response[2]);
                        _response.ServiceId = Convert.ToString(response[3]);
                        _response.PaymentMode = Convert.ToString(response[4]);
                        _response.PaymentSuccess = false;
                        if (response[0] == "F")
                        {
                            ////F|UATPSGSSG0000001455|PAYGOV8UG3152997119|admission fee|||400|Transaction Cancelled|Transaction Cancelled by User|12-07-2022 10:24:55|2898129546
                            _response.BankTransactionNo = Convert.ToString(response[5]);
                            _response.ErrorCode = Convert.ToString(response[6]);
                            _response.ErrorDescription = Convert.ToString(response[8]);
                            _response.ResponseDateTime = Convert.ToString(response[9]);
                            _response.CheckSum = Convert.ToString(response[10]);
                        }
                        else
                        {
                            _response.ErrorDescription = Convert.ToString(response[5]);
                            _response.ResponseDateTime = Convert.ToString(response[6]);
                            _response.CheckSum = Convert.ToString(response[7]);
                        }
                    }
                    else
                    {
                        _response.PaymentSuccess = true;
                        _response.MessageType = Convert.ToString(response[1]);
                        _response.SurePayMerchantId = Convert.ToString(response[2]);
                        _response.ServiceId = Convert.ToString(response[3]);
                        _response.OrderId = Convert.ToString(response[4]);
                        _response.CustomerId = Convert.ToString(response[5]);
                        _response.TransactionAmount = Convert.ToString(response[6]);
                        _response.CurrencyCode = Convert.ToString(response[7]);
                        _response.PaymentMode = Convert.ToString(response[8]);
                        _response.ResponseDateTime = Convert.ToString(response[9]);
                        _response.SurePayYxnId = Convert.ToString(response[10]);
                        _response.BankTransactionNo = Convert.ToString(response[11]);
                        _response.TransactionStatus = Convert.ToString(response[12]);
                        _response.AdditionalInfo1 = Convert.ToString(response[13]);
                        _response.AdditionalInfo2 = Convert.ToString(response[14]);
                        _response.AdditionalInfo3 = Convert.ToString(response[15]);
                        _response.AdditionalInfo4 = Convert.ToString(response[16]);
                        _response.AdditionalInfo5 = Convert.ToString(response[17]);
                        _response.ErrorCode = Convert.ToString(response[18]);
                        _response.ErrorDescription = Convert.ToString(response[19]);
                        _response.CheckSum = Convert.ToString(response[20]);
                    }

                }
                else
                {
                    _response.PaymentSuccess = false;
                    _response.SuccessFlag = "F";
                }
            }
            catch (Exception ex)
            {
                _response.PaymentSuccess = false;
                _response.SuccessFlag = "F";
            }
            return _response;
        }

        public async Task<ServiceResult> PaymentRegistrationResponse(string msg, PaymentStudentModel _student)
        {

            ServiceResult serviceResult = new ServiceResult();
            PayGovPaymentModel _response = new PayGovPaymentModel();
            try
            {
                _response = ConvertPaymentToResponseModel(msg);
                if (_response.PaymentSuccess == false)
                {
                    if (_response.SuccessFlag != "F")
                    {
                        var _verifyResponse = await VerifySinglePayment(_response.BankTransactionNo, _response.OrderId);
                        if (_verifyResponse.ResultData != null)
                        {
                            _response = ConvertPaymentToResponseModel(_verifyResponse.ResultData);
                        }
                        else
                        {
                            serviceResult.Message = MessageConfig.ServiceNotAvailable;
                            serviceResult.ResultData = null;
                            serviceResult.Status = false;
                            serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                            return await Task.Run(() => serviceResult);
                        }
                    }
                }
                _student.StudentId = string.IsNullOrEmpty(_response.CustomerId) ? 0 : Convert.ToInt32(_response.CustomerId);
                var _resultDB = _paygovPaymentRepository.PayGovMatchHashPaymentResponse(_response, _student);
                _response.PaymentSuccess = false;
                if (_resultDB.Result > 0)
                {
                    _response.PaymentSuccess = true;
                }
                PaymentResponseModel _model = new PaymentResponseModel();
                _model.amount = _response.TransactionAmount;
                _model.net_amount_debit = _response.TransactionAmount;
                _model.email = _response.AdditionalInfo4;
                _model.phone = _response.AdditionalInfo3;
                _model.firstname = _response.AdditionalInfo2;
                _model.txnid = _response.OrderId;

                _model.mihpayid = _response.BankTransactionNo;
                _model.key = _response.BankTransactionNo;
                _model.mode = _response.PaymentMode;
                _model.productinfo = "Registration Fee";
                _model.payment_source = _response.PaymentMode;
                _model.status = _response.SuccessFlag == "S" ? "Success" : _response.SuccessFlag == "F" ? "Failed" : "Pending";
                _model.error_Message = _response.ErrorDescription;
                _model.error = _response.ErrorCode;
                _model.hash = _response.CheckSum;
                _model.udf4 = _response.CustomerId;
                _model.field9 = _response.ResponseDateTime;
                _model.VerifyPaymentStatus = _model.status;
                var _result = _paygovPaymentRepository.PayGovPaymentResponse(_model, _student);
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
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("PaygovPaymentDetail : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> PaygovPaymentDetail(string txnid, PaymentStudentModel _student)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _decrTxnid = Decryption.DecodeHexaValue(txnid);
                dynamic _result = await _paygovPaymentRepository.PayGovPaymentDetail(_decrTxnid, _student);

                if (_result != null)
                {
                    if (_result.StudentId == _student.StudentId)
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
                _logError.WriteTextToFile("PaygovPaymentDetail : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> PaymentLog(UgPaymentLog UgPaymentLog)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {

                var _result = _paygovPaymentRepository.AddAsync(UgPaymentLog);
                // dynamic _result = null;

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

        private static String generateCRC32Checksum(String message, String secretKey)
        {
            String msg = message + "|" + secretKey;
            byte[] bytes = Encoding.ASCII.GetBytes(msg);

            var crc32 = new Crc32();
            return crc32.Get(bytes).ToString();
        }
    }
    public class Crc32
    {
        #region Constants
        /// <summary>
        /// Generator polynomial (modulo 2) for the reversed CRC32 algorithm.
        /// </summary>
        private const UInt32 s_generator = 0xEDB88320;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new instance of the Crc32 class.
        /// </summary>
        public Crc32()
        {
            // Constructs the checksum lookup table. Used to optimize the checksum.
            m_checksumTable = Enumerable.Range(0, 256).Select(i =>
            {
                var tableEntry = (uint)i;
                for (var j = 0; j < 8; ++j)
                {
                    tableEntry = ((tableEntry & 1) != 0)
                    ? (s_generator ^ (tableEntry >> 1))
                    : (tableEntry >> 1);
                }
                return tableEntry;
            }).ToArray();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Calculates the checksum of the byte stream.
        /// </summary>
        /// <param name="byteStream">The byte stream to calculate the checksum for.</param>
        /// <returns>A 32-bit reversed checksum.</returns>
        public UInt32 Get<T>(IEnumerable<T> byteStream)
        {
            try
            {
                // Initialize checksumRegister to 0xFFFFFFFF and calculate the checksum.
                return ~byteStream.Aggregate(0xFFFFFFFF, (checksumRegister, currentByte) =>
(m_checksumTable[(checksumRegister & 0xFF) ^ Convert.ToByte(
currentByte)] ^ (checksumRegister >> 8)));
            }
            catch (FormatException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
            catch (InvalidCastException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
            catch (OverflowException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
        }
        #endregion
        #region Fields
        /// <summary>
        /// Contains a cache of calculated checksum chunks.
        /// </summary>
        private readonly UInt32[] m_checksumTable;
        #endregion
    }
}
