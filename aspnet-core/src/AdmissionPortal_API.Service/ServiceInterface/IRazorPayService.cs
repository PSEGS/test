using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IRazorPayService
    {
        Task<ServiceResult> GenerateOrder(Int32 studentId, string OrderId, decimal Amount, string type);
        Task<ServiceResult> PaymentFailure(Int32 studentId, PaymentFailure model, string type);

        Task<ServiceResult> PaymentSuccess(Int32 studentId, RazorRoot model, string type);
        Task<ServiceResult> GetStudentPaymentDetail(Int32 studentId, string type);
        Task<ServiceResult> GenerateOrderPG(Int32 studentId, string OrderId, decimal Amount, string type);
        Task<ServiceResult> PaymentFailurePG(Int32 studentId, PaymentFailure model, string type);

        Task<ServiceResult> PaymentSuccessPG(Int32 studentId, RazorRoot model, string type);       
        Task<ServiceResult> GetStudentPaymentDetailPG(Int32 studentId, string type);
        Task<ServiceResult> GetICICICollegeAccount(IciciCollegeAccount model);

        Task<ServiceResult> FetchPaymentUG(RazorRoot model);

        Task<ServiceResult> FetchPaymentLogUG(UgPaymentLog entity);
        Task<ServiceResult> FetchPaymentPG(RazorRoot model);

        Task<ServiceResult> FetchPaymentLogPG(UgPaymentLog entity);
        Task<ServiceResult> CheckSeatAvailability(AdmissionPaymentModel model);

        Task<ServiceResult> CheckSeatAvailabilityPG(AdmissionPaymentModel model);
        Task<ServiceResult> FetchStudentTransactions(StudentPayment entity);
        Task<ServiceResult> TempAllPendingTransaction(string admissiontype, string paymentType);
    }
}
