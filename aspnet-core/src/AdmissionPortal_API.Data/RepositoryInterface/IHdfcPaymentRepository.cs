using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IHdfcPaymentRepository : IGenericRepository<UgPaymentLog>
    {
        Task<PaymentDataModel> HdfcPaymentResponse(PaymentResponseModel entity);
        
        Task<PaymentDataModel> VerifyPayment(PaymentResponseModel entity);
        Task<PaymentDataModel> VerifyPaymentPG(PaymentResponseModel entity);

        Task<HdfcPaymentReceipt> HdfcPaymentDetail(string _decrTxnid);
        Task<int> AddAsyncPG(UgPaymentLog entity);
        Task<PaymentDataModel> HdfcPaymentResponsePG(PaymentResponseModel entity);
        Task<HdfcPaymentReceipt> HdfcPaymentDetailPG(string _decrTxnid);

        Task<int> ReconcilePaymentLog(UgPaymentLog entity);
        Task<int> PublicyReconcilePaymentLog(UgPaymentLog entity);
        Task<int> PublicyReconcilePaymentLogPG(PGPaymentLog entity);
        Task<int> HdfcMatchHashPaymentResponsePG(HdfcPaymentObject entity);
        Task<int> HdfcMatchHashPaymentResponse(HdfcPaymentObject entity);
        Task<CollegeBankId> HdfcCollegeDetail(int CollegeId, int CollegeType);
        Task<List<ReconcileTransaction>> ReconsileList(string Type);
    }
}
