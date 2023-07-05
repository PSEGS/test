using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IRazorPayRepository
    {
        Task<int> GenerateOrder(Int32 StudentID, string OrderId, decimal Amount,string type);
        Task<int> PaymentFailure(Int32 studentId, PaymentFailure model, string type);
        Task<PaymentDataModel> PaymentSuccess(Int32 studentId, RazorRoot model, string type);
        Task<PaymentDetail> GetStudentPaymentDetail(Int32 studentId, string type); 
        Task<int> GenerateOrderPG(Int32 StudentID, string OrderId, decimal Amount,string type);
        Task<int> PaymentFailurePG(Int32 studentId, PaymentFailure model, string type);
        Task<PaymentDataModel> PaymentSuccessPG(Int32 studentId, RazorRoot model, string type);
        Task<PaymentDetail> GetStudentPaymentDetailPG(Int32 studentId, string type);
        Task<string> GetICICICollegeAccount(IciciCollegeAccount model);

        Task<PaymentDataModel> FetchPaymentUG(RazorRoot model);

        Task<int> FetchPaymentLogUG(UgPaymentLog entity);
        Task<PaymentDataModel> FetchPaymentPG(RazorRoot model);
        Task<int> CheckSeatAvailability(AdmissionPaymentModel _model);

        Task<int> FetchPaymentLogPG(UgPaymentLog entity);
        Task<int> CheckSeatAvailabilityPG(AdmissionPaymentModel _model);
        Task<dynamic> FetchStudentTransactions(StudentPayment model);
        Task<dynamic> TempAllPendingTransaction(string admissiontype, string paymentType);
    }
}
