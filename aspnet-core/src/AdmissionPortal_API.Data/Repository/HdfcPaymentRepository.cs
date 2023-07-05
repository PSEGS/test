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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class HdfcPaymentRepository : IHdfcPaymentRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public HdfcPaymentRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddAsync(UgPaymentLog entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<HdfcPaymentMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.HDFCPaymentInitiated;
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
                _logError.WriteTextToFile("Create University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }


        public async Task<PaymentDataModel> HdfcPaymentResponse(PaymentResponseModel entity)
        {
            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var values = _mapper.Map<HdfcPaymentResponseMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.HDFCPaymentResponse;
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

        public async Task<PaymentDataModel> VerifyPayment(PaymentResponseModel entity)
        {
            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var values = _mapper.Map<HdfcPaymentResponseMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.HDFCVerifyPayment;
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
        public async Task<PaymentDataModel> VerifyPaymentPG(PaymentResponseModel entity)
        {
            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var values = _mapper.Map<HdfcPaymentResponseMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.HDFCVerifyPaymentPG;
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


        public async Task<int> ReconcilePaymentLog(UgPaymentLog entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<HdfcPaymentMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.UG_ReconcilePayment;
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
        public async Task<int> PublicyReconcilePaymentLog(UgPaymentLog entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<HdfcPaymentMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.UG_Publicly_ReconcilePayment;
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
                _logError.WriteTextToFile("Publicy Reconcile PaymentLog : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<int> PublicyReconcilePaymentLogPG(PGPaymentLog entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<HdfcPaymentMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.UG_Publicly_ReconcilePayment;
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
                _logError.WriteTextToFile("Publicy Reconcile PaymentLog : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<HdfcPaymentReceipt> HdfcPaymentDetail(string _decrTxnid)
        {
            HdfcPaymentReceipt paymentData = new HdfcPaymentReceipt();
            try
            {

                var values = new { transactionId = _decrTxnid };

                var procedure = Procedure.GetHDFCPaymentDetail;
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

        public async Task<int> AddAsyncPG(UgPaymentLog entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<HdfcPaymentMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.HDFCPaymentInitiatedPG;
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
                _logError.WriteTextToFile("Add Payment PG : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }


        public async Task<PaymentDataModel> HdfcPaymentResponsePG(PaymentResponseModel entity)
        {
            PaymentDataModel paymentData = new PaymentDataModel();
            try
            {
                var values = _mapper.Map<HdfcPaymentResponseMapping>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.HDFCPaymentResponsePG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<PaymentDataModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<PaymentDataModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("HdfcPayment Response PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;
        }


        public async Task<HdfcPaymentReceipt> HdfcPaymentDetailPG(string _decrTxnid)
        {
            HdfcPaymentReceipt paymentData = new HdfcPaymentReceipt();
            try
            {

                var values = new { transactionId = _decrTxnid };

                var procedure = Procedure.GetHDFCPaymentDetailPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<HdfcPaymentReceipt>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<HdfcPaymentReceipt>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("HdfcPayment Detail PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;

        }

        public async Task<int> HdfcMatchHashPaymentResponsePG(HdfcPaymentObject entity)
        {
            HdfcPaymentObject paymentData = new HdfcPaymentObject();
            int result = 0;
            try
            {
                var values = new
                {
                    TxnId = entity.txnid,
                    Amount = entity.amount
                };
                var procedure = Procedure.HDFCMatchHashPaymentResponsePG;
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

        public async Task<int> HdfcMatchHashPaymentResponse(HdfcPaymentObject entity)
        {
            HdfcPaymentObject paymentData = new HdfcPaymentObject();
            int result = 0;
            try
            {
                var values = new
                {
                    TxnId = entity.txnid,
                    Amount = entity.amount
                };
                var procedure = Procedure.HDFCMatchHashPaymentResponse;
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
        public async Task<CollegeBankId> HdfcCollegeDetail(int CollegeId, int CollegeType)
        {
            CollegeBankId paymentData = new CollegeBankId();
            try
            {

                var values = new { CollegId = CollegeId, CollegeType = CollegeType };

                var procedure = Procedure.HDFCCollegeBank;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<CollegeBankId>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    paymentData = _mapper.Map<CollegeBankId>(response);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("HdfcPayment college Detail: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return paymentData;

        }
        public async Task<List<ReconcileTransaction>> ReconsileList(string Type)
        {
            List<ReconcileTransaction> _result = new List<ReconcileTransaction>();
            try
            {
                var values = new { Type = Type };
                var procedure = Procedure.Reconciliation_Transaction_List;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QueryAsync<List<ReconcileTransaction>>(procedure, null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("HdfcPayment reconcile list: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _result;
        }
    }
}
