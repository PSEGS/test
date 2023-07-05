using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
   public interface IHdfcService
    {
         Task<ServiceResult> MakeHdfcPayment(string Amount, PaymentStudentModel model);
        Task<ServiceResult> PaymentLog(UgPaymentLog UgPaymentLog);        
        Task<ServiceResult> PaymentResponse(IFormCollection form);
        Task<ServiceResult> HdfcPaymentDetail(string txnid, int studentId);
        Task<ServiceResult> MakeHdfcPaymentPG(string Amount, PaymentStudentModel model);
        Task<ServiceResult> PaymentLogPG(UgPaymentLog UgPaymentLog);
        Task<ServiceResult> PaymentResponsePG(IFormCollection _collection);
        Task<ServiceResult> HdfcPaymentDetailPG(string txnid, int studentId);
        Task<ServiceResult> HdfcPaymentDetail(string txnid);
        Task<ServiceResult> VerifyPayment(string txnid, PaymentStudentModel model);

        Task<ServiceResult> PubliclyVerifyPayment(string txnid);
        Task<ServiceResult> PubliclyVerifyPaymentPG(string txnid);
        Task<ServiceResult> MakeAdmissionPayment(string Amount, HdfcStudentAdmissionModel model);
        Task<ServiceResult> AdmissionPaymentResponse(IFormCollection _collection);
        Task<ServiceResult> MakeAdmissionPaymentPG(string Amount, HdfcStudentAdmissionModel model);
        Task<ServiceResult> AdmissionPaymentResponsePG(IFormCollection _collection);
        Task<ServiceResult> ReconsileList(string Type);

    }
}
