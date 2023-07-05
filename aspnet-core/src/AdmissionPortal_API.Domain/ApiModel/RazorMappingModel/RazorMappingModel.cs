using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.RazorMappingModel
{
    
    public class RazorMappingModel
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

}
