using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.HdfcPaymentMapping;
using AdmissionPortal_API.Domain.ApiModel.RazorMappingModel;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class RazorPayReporsitory : IRazorPayRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public RazorPayReporsitory(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task<int> GenerateOrder(Int32 studentId, string OrderId, decimal Amount, string type)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.GenerateOrderUG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        studentId = studentId,
                        OrderId = OrderId,
                        Amount = Amount
                    };
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("generate order razorpay : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> PaymentFailure(Int32 studentId, PaymentFailure model, string type)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.PaymentFailureUG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        studentId = studentId,
                        code = model.code,
                        description = model.description,
                        order_id = model.metadata.order_id,
                        payment_id = model.metadata.payment_id,
                        reason = model.reason,
                        source = model.source,
                        step = model.step,
                    };
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("generate order razorpay : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<PaymentDataModel> PaymentSuccess(Int32 studentId, RazorRoot model, string type)
        {

            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var procedure = Procedure.PaymentSuccessUG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = _mapper.Map<RazorMappingModel>(model);
                    //result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    var response = await connection.QuerySingleAsync<PaymentDataModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<PaymentDataModel>(response);

                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("payment success razorpay : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;
        }

        public async Task<PaymentDetail> GetStudentPaymentDetail(Int32 studentId, string type)
        {
            PaymentDetail result = new PaymentDetail();
            try
            {
                var procedure = Procedure.PaymentDetailUG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        studentId = studentId
                    };
                    result = await connection.QuerySingleAsync<PaymentDetail>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("payment detail razorpay : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> GenerateOrderPG(Int32 studentId, string OrderId, decimal Amount, string type)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.GenerateOrderPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        studentId = studentId,
                        OrderId = OrderId,
                        Amount = Amount
                    };
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("generate order razorpay : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> PaymentFailurePG(Int32 studentId, PaymentFailure model, string type)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.PaymentFailurePG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        studentId = studentId,
                        code = model.code,
                        description = model.description,
                        order_id = model.metadata.order_id,
                        payment_id = model.metadata.payment_id,
                        reason = model.reason,
                        source = model.source,
                        step = model.step,
                    };
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("generate order razorpay : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<PaymentDataModel> PaymentSuccessPG(Int32 studentId, RazorRoot model, string type)
        {
            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var procedure = Procedure.PaymentSuccessPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = _mapper.Map<RazorMappingModel>(model);
                    var response = await connection.QuerySingleAsync<PaymentDataModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<PaymentDataModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("payment success razorpay : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;
        }

        public async Task<PaymentDetail> GetStudentPaymentDetailPG(Int32 studentId, string type)
        {
            PaymentDetail result = new PaymentDetail();
            try
            {
                var procedure = Procedure.PaymentDetailPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        studentId = studentId
                    };
                    result = await connection.QuerySingleAsync<PaymentDetail>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("payment detail razorpay : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<string> GetICICICollegeAccount(IciciCollegeAccount _model)
        {
            string result = string.Empty;
            try
            {
                var procedure = Procedure.AccountDetailICICI;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        CollegeId = _model.CollegeId,
                        AccountType = _model.AccountType
                    };
                    result = await connection.QuerySingleAsync<string>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                result = null;
                _logError.WriteTextToFile("GetICICICollegeAccount : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<PaymentDataModel> FetchPaymentUG(RazorRoot entity)
        {
            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var values = _mapper.Map<RazorMappingModel>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.FetchPaymentUG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<PaymentDataModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<PaymentDataModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Verify Payment : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;
        }

        public async Task<int> FetchPaymentLogUG(UgPaymentLog entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<HdfcPaymentMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.FetchPaymentLog_UG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else if (response.Response == "2")
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Reconcile Payment Log : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }


        public async Task<PaymentDataModel> FetchPaymentPG(RazorRoot entity)
        {
            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var values = _mapper.Map<RazorMappingModel>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.FetchPaymentPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<PaymentDataModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<PaymentDataModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Verify Payment : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;
        }

        public async Task<int> FetchPaymentLogPG(UgPaymentLog entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<HdfcPaymentMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.FetchPaymentLog_PG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else if (response.Response == "2")
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Reconcile Payment Log : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> CheckSeatAvailability(AdmissionPaymentModel _model)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.CheckSeatAvailabilityBeforePayment;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        CollegeId = _model.CollegeId,
                        CourseId = _model.CourseId,
                        StudentId = _model.StudentId
                    };
                    result = await connection.QuerySingleAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                _logError.WriteTextToFile("CheckSeatAvailability : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> CheckSeatAvailabilityPG(AdmissionPaymentModel _model)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.CheckSeatAvailabilityBeforePaymentPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var values = new
                    {
                        CollegeId = _model.CollegeId,
                        CourseId = _model.CourseId,
                        StudentId = _model.StudentId
                    };
                    result = await connection.QuerySingleAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                result = 0;
                _logError.WriteTextToFile("CheckSeatAvailabilityPG : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<dynamic> FetchStudentTransactions(StudentPayment model)
        {
            dynamic _response = null;
            try
            {
                var values = new { RegistrationNumber = model.RegistrationNumber, StudentType = model.StudentType };

                var procedure = Procedure.FetchAllPaymentsForStudent;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    _response = await connection.QueryAsync<dynamic>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                   // paymentData = _mapper.Map<PaymentDataModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Verify Payment : ", ex.Message, ex.HResult, ex.StackTrace);
                _response = null;
            }
            return _response;
        }
        public async Task<dynamic> TempAllPendingTransaction(string admissiontype, string paymentType)
        {
            dynamic _response = null;
            try
            {
                // var values = new { RegistrationNumber = model.RegistrationNumber, StudentType = model.StudentType };
                var procedure = string.Empty;
                if (admissiontype.ToLower() == "ug" && paymentType.ToLower() == "admission")
                {
                    procedure = Procedure.FetchAllPendingPaymentsTemp;
                }
                else if (admissiontype.ToLower() == "pg" && paymentType.ToLower() == "admission")
                {
                    procedure = Procedure.FetchAllPendingRegistration_PG_PaymentsTemp;
                }
                
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    _response = await connection.QueryAsync<dynamic>(procedure, null, commandType: System.Data.CommandType.StoredProcedure);
                    // paymentData = _mapper.Map<PaymentDataModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Verify Payment : ", ex.Message, ex.HResult, ex.StackTrace);
                _response = null;
            }
            return _response;
        }
    }
}
