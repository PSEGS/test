using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.Service
{

    public class RazorPayService : IRazorPayService
    {
        private readonly IRazorPayRepository _razorPayRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public RazorPayService(IRazorPayRepository razorPayRepository, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _razorPayRepository = razorPayRepository;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }
        public async Task<ServiceResult> GenerateOrder(Int32 studentId, string OrderId, decimal Amount, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.GenerateOrder(studentId, OrderId, Amount, type);

                if (_result != 0)
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
                _logError.WriteTextToFile("generate order razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> PaymentFailure(Int32 studentId, PaymentFailure model, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.PaymentFailure(studentId, model, type);

                if (_result != 0)
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
                        serviceResult.Message = MessageConfig.InvalidOTP;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.InvalidOTP;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.InvalidOTP;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("payment failure razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> PaymentSuccess(Int32 studentId, RazorRoot model, string type)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _razorPayRepository.PaymentSuccess(studentId, model, type);

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
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("payment success razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetStudentPaymentDetail(Int32 studentId, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.GetStudentPaymentDetail(studentId, type);

                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = null;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("payment details razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GenerateOrderPG(Int32 studentId, string OrderId, decimal Amount, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.GenerateOrderPG(studentId, OrderId, Amount, type);

                if (_result != 0)
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
                _logError.WriteTextToFile("generate order razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> PaymentFailurePG(Int32 studentId, PaymentFailure model, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.PaymentFailurePG(studentId, model, type);

                if (_result != 0)
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
                        serviceResult.Message = MessageConfig.InvalidOTP;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.InvalidOTP;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.InvalidOTP;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("payment failure razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> PaymentSuccessPG(Int32 studentId, RazorRoot model, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _razorPayRepository.PaymentSuccessPG(studentId, model, type);

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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("payment success razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GetStudentPaymentDetailPG(Int32 studentId, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.GetStudentPaymentDetailPG(studentId, type);

                if (_result != null)
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = null;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("payment details razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
        public async Task<ServiceResult> GetICICICollegeAccount(IciciCollegeAccount model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.GetICICICollegeAccount(model);

                if (!string.IsNullOrEmpty(_result))
                {
                    serviceResult.ResultData = _result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = null;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = null;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("GetICICICollegeAccount: ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> FetchPaymentUG(RazorRoot model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _razorPayRepository.FetchPaymentUG(model);

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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("payment success razorpay : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> FetchPaymentLogUG(UgPaymentLog entity)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _razorPayRepository.FetchPaymentLogUG(entity);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (Convert.ToString(serviceResult.ResultData) == "1")
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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Fetch Payment Log UG : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;

        }
        public async Task<ServiceResult> TempAllPendingTransaction(string admissiontype, string paymentType)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _razorPayRepository.TempAllPendingTransaction(admissiontype,paymentType);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (Convert.ToInt32(serviceResult.ResultData.Count) >0)
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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Fetch Payment Log UG : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;

        }

        public async Task<ServiceResult> FetchPaymentPG(RazorRoot model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _razorPayRepository.FetchPaymentPG(model);

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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Fetch Payment PG : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> FetchPaymentLogPG(UgPaymentLog entity)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _razorPayRepository.FetchPaymentLogPG(entity);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (Convert.ToString(serviceResult.ResultData) == "1")
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
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
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
                _logError.WriteTextToFile("Fetch Payment Log PG : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;

        }

        public async Task<ServiceResult> CheckSeatAvailability(AdmissionPaymentModel model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.CheckSeatAvailability(model);

                if (_result != 0)
                {
                    serviceResult.Message = null;
                    serviceResult.Status = true;
                    serviceResult.ResultData = 1;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.CourseSeatFull;
                    serviceResult.ResultData = 0;
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
                _logError.WriteTextToFile("CheckSeatAvailability: ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
        public async Task<ServiceResult> CheckSeatAvailabilityPG(AdmissionPaymentModel model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = await _razorPayRepository.CheckSeatAvailabilityPG(model);

                if (_result != 0)
                {
                    serviceResult.Message = null;
                    serviceResult.Status = true;
                    serviceResult.ResultData = 1;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.CourseSeatFull;
                    serviceResult.ResultData = 0;
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
                _logError.WriteTextToFile("CheckSeatAvailability: ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
        public async Task<ServiceResult> FetchStudentTransactions(StudentPayment entity)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _razorPayRepository.FetchStudentTransactions(entity);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
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
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("FetchStudentTransactions : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;

        }

    }
}
