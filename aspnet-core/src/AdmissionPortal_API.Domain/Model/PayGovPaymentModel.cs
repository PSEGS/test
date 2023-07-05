using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class PayGovPaymentModel
    {
        public string SuccessFlag { get; set; }
        public string MessageType { get; set; }
        public string SurePayMerchantId { get; set; }
        public string ServiceId { get; set; }
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string TransactionAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentMode { get; set; }
        public string ResponseDateTime { get; set; }
        public string SurePayYxnId { get; set; }
        public string BankTransactionNo { get; set; }
        public string TransactionStatus { get; set; }
        public string AdditionalInfo1 { get; set; }
        public string AdditionalInfo2 { get; set; }
        public string AdditionalInfo3 { get; set; }
        public string AdditionalInfo4 { get; set; }
        public string AdditionalInfo5 { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string CheckSum { get; set; }
        public bool PaymentSuccess { get; set; }
    }
    public class verifyPaymentModel
    {
        public string BankTransactionNo { get; set; } = null;
        public string OrderId { get; set; } = null;
        public string SearchDate { get; set; } = null;
    }
}
