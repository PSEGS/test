using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class RazorPay
    {
    }
    public class IciciCollegeAccount
    {
        public int CollegeId { get; set; }
        public int AccountType { get; set; }
    }
    public class PaymentOrderResponse
    {
        public string id { get; set; }
        public string entity { get; set; }
        public string amount { get; set; }
        public string amount_paid { get; set; }
        public string amount_due { get; set; }
        public string currency { get; set; }
        public string receipt { get; set; }
        public string offer_id { get; set; }
        public string status { get; set; }
        public string attempts { get; set; }
        public string notes { get; set; }
        public string created_at { get; set; }

    }

    public class PaymentFailure
    {
        public string code { get; set; }
        public string description { get; set; }
        public metadata metadata { get; set; }
        public string reason { get; set; }
        public string source { get; set; }
        public string step { get; set; }

    }
    public class metadata
    {
        public string payment_id { get; set; }
        public string order_id { get; set; }
    }
    public class PaymentSuccess
    {
        public string paymentId { get; set; }
        public string orderId { get; set; }

        public string Signature { get; set; }
    }

    public class PaymentDetail
    {
        public string paymentId { get; set; }
        public string orderId { get; set; }
        public string Amount { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentGateway { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 


    public class RazorRoot
    {
        public string id { get; set; }
        public string entity { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string order_id { get; set; }
        public string invoice_id { get; set; }
        public bool international { get; set; }
        public string method { get; set; }
        public int amount_refunded { get; set; }
        public string refund_status { get; set; }
        public bool captured { get; set; }
        public string description { get; set; }
        public string card_id { get; set; }
        public string bank { get; set; }
        public string wallet { get; set; }
        public string vpa { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public int fee { get; set; }
        public int tax { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }
        public string error_source { get; set; }
        public string error_step { get; set; }
        public string error_reason { get; set; }
        public int created_at { get; set; }

        public string paymentId { get; set; }

        public string Signature { get; set; }

        public string udf_name { get; set; }
        public int udf_studentId { get; set; }
        public string udf_address { get; set; }
        public string udf_contact { get; set; }
        public string udf_feeType { get; set; }
        public string rrn { get; set; }
        public string paymentResponse { get; set; }

        public int studentId { get; set; }

        public string collegeId { get; set; }

        public string courseId { get; set; }

        public string bank_transaction_id { get; set; }
    }

    public class FetchPayment
    {
        [Required]
        public string paymentId { get; set; }
      
    }
    public class StudentPayment
    {
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string StudentType { get; set; }

    }

}
