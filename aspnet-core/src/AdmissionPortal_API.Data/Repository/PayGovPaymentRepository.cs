using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.HdfcPaymentMapping;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class PayGovPaymentRepository : IPayGovPaymentRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public PayGovPaymentRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task<List<verifyPaymentModel>> FindPendingTransactionUG(string date)
        {
            List<verifyPaymentModel> _result = new List<verifyPaymentModel>();
            try
            {
                var values = new { filterDate = date };
                var procedure = Procedure.FindPendingTransactionUG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var _dbResult = await connection.QueryAsync<verifyPaymentModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    // paymentData = _mapper.Map<HdfcPaymentReceipt>(response);
                    _result = (List<verifyPaymentModel>)_dbResult;
                    //_result = _dbResult;
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("FindPendingTransactionPG Detail : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _result;
        }
        public async Task<List<verifyPaymentModel>> FindPendingTransactionPG(string date)
        {
            List<verifyPaymentModel> _result = new List<verifyPaymentModel>();
            try
            {
                var values = new { filterDate = date };
                var procedure = Procedure.FindPendingTransactionPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var _dbResult = await connection.QueryAsync<List<verifyPaymentModel>>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    // paymentData = _mapper.Map<HdfcPaymentReceipt>(response);
                    _result = (List<verifyPaymentModel>)_dbResult;
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("FindPendingTransactionPG Detail : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _result;
        }
        public async Task<HdfcPaymentReceipt> PayGovPaymentDetail(string _decrTxnid, PaymentStudentModel _student)
        {
            HdfcPaymentReceipt paymentData = new HdfcPaymentReceipt();
            try
            {

                var values = new { transactionId = _decrTxnid };

                var procedure = Procedure.GetHDFCPaymentDetail;
                if (_student.StudentType.ToUpper() == "PG")
                {
                    procedure = Procedure.GetHDFCPaymentDetailPG;
                }
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<HdfcPaymentReceipt>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<HdfcPaymentReceipt>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("HdfcPayment Detail : ", "_decrTxnid:" + _decrTxnid + ", " + ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;

        }
        public async Task<int> AddAsync(UgPaymentLog entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<HdfcPaymentMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                string procedure = Procedure.Payment_Initiated_PAYGOV;
                if (entity.StudentType.ToUpper() == "PG")
                {
                    procedure = Procedure.HDFCPaymentInitiatedPG;
                }
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
                _logError.WriteTextToFile("Create payment request : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<int> PayGovMatchHashPaymentResponse(PayGovPaymentModel entity, PaymentStudentModel _student)
        {
            int result = 0;
            try
            {
                var values = new
                {
                    TxnId = entity.SurePayYxnId,
                    Amount = entity.TransactionAmount
                };
                string procedure = Procedure.HDFCMatchHashPaymentResponse;
                if (_student.StudentType.ToUpper() == "PG")
                {
                    procedure = Procedure.HDFCMatchHashPaymentResponsePG;
                }
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("HdfcPayment match hash PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<PaymentDataModel> PayGovPaymentResponse(PaymentResponseModel entity, PaymentStudentModel _student)
        {
            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var values = _mapper.Map<HdfcPaymentResponseMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.HDFCPaymentResponse;
                if (_student.StudentType.ToUpper() == "PG")
                {
                    procedure = Procedure.HDFCPaymentResponsePG;
                }
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<PaymentDataModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<PaymentDataModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("HdfcPayment Response : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;
        }
        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UgPaymentLog> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(UgPaymentLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
