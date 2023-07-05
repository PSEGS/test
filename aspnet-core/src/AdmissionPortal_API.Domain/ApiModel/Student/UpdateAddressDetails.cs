using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class UpdateAddressDetails
    {
        [JsonIgnore]
        public int StudentId { get; set; }
        public int Permanent_Region { get; set; }
        public string Permanent_Address { get; set; }
        public int Permanent_Address_State_Id { get; set; }
        public int Permanent_Address_District_Id { get; set; }
        //public int Permanent_Address_Tehsil_Id { get; set; }
        //public int Permanent_Address_Block_Id { get; set; }
        //public int Permanent_Address_Village_Id { get; set; }
        public string Permanent_PinCode { get; set; }
        public int Communication_Region { get; set; }
        public string Communication_Address { get; set; }
        public int Communication_Address_State_Id { get; set; }
        public int Communication_Address_District_Id { get; set; }
        //public int Communication_Address_Tehsil_Id { get; set; }
        //public int Communication_Address_Block_Id { get; set; }
        //public int Communication_Address_Village_Id { get; set; }
        public string Communication_Pincode { get; set; }
    }
}
