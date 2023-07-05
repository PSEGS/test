using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.HdfcPaymentMapping
{
    
    public class HdfcPaymentMapping
    {
        public int StudentId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentRequest { get; set; }        
        public bool PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public string HashForm { get; set; }
        public string PaymentMessage { get; set; }
        public string Amount { get; set; }
        public string Type { get; set; } = null;
    }
}
