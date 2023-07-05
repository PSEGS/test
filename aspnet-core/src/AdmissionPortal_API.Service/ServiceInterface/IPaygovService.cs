using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
   public interface IPaygovService
    {
        Task<ServiceResult> MakePaygovRegistrationPayment(string Amount, PaymentStudentModel model);
        Task<ServiceResult> PaymentRegistrationResponse(string msg, PaymentStudentModel _student);
        Task<ServiceResult> PaygovPaymentDetail(string txnid, PaymentStudentModel _student);
        Task<ServiceResult> ReConcilePayGovPaymentUG(verifyPaymentModel _model);
        Task<ServiceResult> ReConcilePayGovPaymentPG(verifyPaymentModel _model);
        Task<ServiceResult> PaymentLog(UgPaymentLog UgPaymentLog);
    }
}
