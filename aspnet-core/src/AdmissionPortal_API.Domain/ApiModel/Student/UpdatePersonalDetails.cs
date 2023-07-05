using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class UpdatePersonalDetails
    {
        [JsonIgnore]
        public int StudentId { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public string MotherName { get; set; }
        public string FatherOccupation { get; set; }
        public string MotherOccupation { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public string DateofBirth { get; set; }
        public int MaritalStatus { get; set; }
        [Required]
        public string AadhaarNumber { get; set; }
        public string GaurdianName { get; set; }
        public string GaurdianContactNumber { get; set; }
        public string GaurdianEmailId { get; set; }
        public int? BloodGroup { get; set; }
        public int Religion { get; set; }
        public int HouseholdIncome { get; set; }
        public string GapYear { get; set; }
        public bool NeedHostel { get; set; }
        public string ModeOfTransport { get; set; }
        public bool NeedParking { get; set; }
        public int Age { get; set; }
        public string LandlineNo { get; set; }
        public bool PunjabiInMetric { get; set; }
        public bool AreuNRI { get; set; }
        public string PassportNumber { get; set; }
        public string PassportExpiryDate { get; set; }
        public bool OnlyGirlChild { get; set; }
        public bool AreYouCancerAIDSThalassemiaPatient { get; set; }
        public bool IsPhysicalDisable { get; set; }
        [Required]
        public Boolean? IsAlreadyRegisteredwithUniversity { get; set; }
        public int? AlreadyRegisteredUniversityId { get; set; }
        public string AlreadyRegisteredNo { get; set; }


    }
}





