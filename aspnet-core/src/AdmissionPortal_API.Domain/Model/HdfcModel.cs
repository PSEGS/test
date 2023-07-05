using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class HdfcModel
    {
    }
    public class AdmissionPaymentModel
    {
        [JsonIgnore]
        public int StudentId { get; set; }
        public int CollegeId { get; set; }
        public int CourseId { get; set; }
        public int CourseType { get; set; }
        public string Amount { get; set; }
        public bool IsUGStudent { get; set; } = true;
    }
    public class HdfcStudentAdmissionModel
    {
        public int StudentId { get; set; }
        public int CollegeId { get; set; }
        public int CourseType { get; set; }
        public int CourseId { get; set; }
        public string StudentType { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
    }
    public class PaymentStudentModel
    {
        public int StudentId { get; set; }
        public string StudentType { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
    }
    public class UgPaymentLog
    {
        public int StudentId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentRequest { get; set; }
        public DateTime? PaymentRequestDate { get; set; }
        public string PaymentResponse { get; set; }
        public DateTime? PaymentResponseDate { get; set; }
        public bool PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public string HashForm { get; set; }
        public string PaymentMessage { get; set; }
        public string Amount { get; set; }
        public string StudentType { get; set; } = null;
    }
    public class PGPaymentLog
    {
        public int StudentId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentRequest { get; set; }
        public DateTime? PaymentRequestDate { get; set; }
        public string PaymentResponse { get; set; }
        public DateTime? PaymentResponseDate { get; set; }
        public bool PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public string HashForm { get; set; }
        public string PaymentMessage { get; set; }
        public string Amount { get; set; }
        public string Type { get; set; }
    }

    public class PaymentResponseModel
    {
        public string mihpayid { get; set; }
        public string mode { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public string key { get; set; }
        public string txnid { get; set; }
        public string amount { get; set; }
        public string cardCategory { get; set; }
        public string discount { get; set; }
        public string net_amount_debit { get; set; }
        public string addedon { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string lastnam { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string udf6 { get; set; }
        public string udf7 { get; set; }
        public string udf8 { get; set; }
        public string udf9 { get; set; }
        public string udf10 { get; set; }
        public string hash { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string payment_source { get; set; }
        public string PG_TYPE { get; set; }
        public string bank_ref_num { get; set; }
        public string bankcode { get; set; }
        public string error { get; set; }
        public string error_Message { get; set; }
        public string name_on_card { get; set; }
        public string cardnum { get; set; }
        public string cardhash { get; set; }

        public string paymentResponse { get; set; }

        public string VerifyPaymentResponse { get; set; }

        public string VerifyPaymentStatus { get; set; }

    }




    public class PaymentDataModel
    {
        public string Response { get; set; }
        public int StudentId { get; set; }
        public string StudentType { get; set; }
        public string mihpayid { get; set; }
        public string mode { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public string key { get; set; }
        public string txnid { get; set; }
        public string amount { get; set; }
        public string cardCategory { get; set; }
        public string discount { get; set; }
        public string net_amount_debit { get; set; }
        public string addedon { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string lastnam { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string udf6 { get; set; }
        public string udf7 { get; set; }
        public string udf8 { get; set; }
        public string udf9 { get; set; }
        public string udf10 { get; set; }
        public string hash { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string payment_source { get; set; }
        public string PG_TYPE { get; set; }
        public string bank_ref_num { get; set; }
        public string bankcode { get; set; }
        public string error { get; set; }
        public string error_Message { get; set; }
        public string name_on_card { get; set; }
        public string cardnum { get; set; }
        public string cardhash { get; set; }
        public string paymentResponse { get; set; }

    }
    public class CollegeBankId
    {
        public string MERCHANT_KEY { get; set; }
        public string SALT { get; set; }
        public string TID { get; set; }
    }
    public class HdfcPaymentReceipt
    {
        public int StudentId { get; set; }
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        public string Bank_ref_num { get; set; }
        public string PaymentResponseDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentRequestDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMessage { get; set; }
        public string RegistrationNumber { get; set; }
        public string Response { get; set; }
        public string CandidateName { get; set; }

    }

    public class Hdfc_verifyPayment
    {
        [Required(ErrorMessage = "Transaction Id is required ")]
        public string trxId { get; set; }
        public string Mihpayid { get; set; }
    }
    public class Hdfc_Public_verifyPayment
    {
        [Required(ErrorMessage = "Transaction Id is required ")]
        public string trxId { get; set; }

    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class verifyPaymentData
    {
        public string mihpayid { get; set; }
        public string request_id { get; set; }
        public string bank_ref_num { get; set; }
        public string amt { get; set; }
        public string transaction_amount { get; set; }
        public string txnid { get; set; }
        public string additional_charges { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string bankcode { get; set; }
        public string udf1 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string field2 { get; set; }
        public object field9 { get; set; }
        public string error_code { get; set; }
        public string addedon { get; set; }
        public string payment_source { get; set; }
        public string card_type { get; set; }
        public string error_Message { get; set; }
        public int net_amount_debit { get; set; }
        public string disc { get; set; }
        public string mode { get; set; }
        public string PG_TYPE { get; set; }
        public string card_no { get; set; }
        public string name_on_card { get; set; }
        public string udf2 { get; set; }
        public string field5 { get; set; }
        public string field7 { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public object Merchant_UTR { get; set; }
        public string Settled_At { get; set; }

        public string paymentResponse { get; set; }
    }

    public class Root
    {
        public int status { get; set; }
        public string msg { get; set; }
        public dynamic transaction_details { get; set; }
    }

    public class HdfcPaymentObject
    {
        public string mihpayid { get; set; }
        public string mode { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public string key { get; set; }
        public string txnid { get; set; }
        public string amount { get; set; }
        public string cardCategory { get; set; }
        public string discount { get; set; }
        public string net_amount_debit { get; set; }
        public string addedon { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string lastnam { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string udf6 { get; set; }
        public string udf7 { get; set; }
        public string udf8 { get; set; }
        public string udf9 { get; set; }
        public string udf10 { get; set; }
        public string hash { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string payment_source { get; set; }
        public string PG_TYPE { get; set; }
        public string bank_ref_num { get; set; }
        public string bankcode { get; set; }
        public string error { get; set; }
        public string error_Message { get; set; }
        public string name_on_card { get; set; }
        public string cardnum { get; set; }
        public string cardhash { get; set; }
        public string paymentResponse { get; set; }
        public string VerifyPaymentResponse { get; set; }
        public string VerifyPaymentStatus { get; set; }

        public string strHash512 { get; set; }
        public string strWithoutHash { get; set; }

        public Boolean PaymentSuccess { get; set; }

    }
    public class ReconcileTransaction
    {
        public string TransactionId { get; set; }
    }



}
