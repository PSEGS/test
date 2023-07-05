using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
   public interface IPayGovPaymentRepository: IGenericRepository<UgPaymentLog>
    {
        Task<int> PayGovMatchHashPaymentResponse(PayGovPaymentModel entity, PaymentStudentModel _model);
        Task<PaymentDataModel> PayGovPaymentResponse(PaymentResponseModel entity, PaymentStudentModel _student);
        Task<HdfcPaymentReceipt> PayGovPaymentDetail(string _decrTxnid, PaymentStudentModel _student);
        Task<List<verifyPaymentModel>> FindPendingTransactionUG(string date);
        Task<List<verifyPaymentModel>> FindPendingTransactionPG(string date);
    }
}
