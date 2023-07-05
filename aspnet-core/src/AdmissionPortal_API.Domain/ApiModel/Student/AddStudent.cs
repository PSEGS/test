using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class AddStudent
    {
        [Required]
        public string number { get; set; }
        [Required]

        public string name { get; set; }
        public string motherName { get; set; }
        [Required]

        public string fatherName { get; set; }
        [Required]

        public string Dob { get; set; }
        public string text { get; set; }
        public string schoolName { get; set; }
        public string result { get; set; }
        public string marksTotal { get; set; }
        public string Totmax { get; set; }
        public List<subject> subject { get; set; }
        [JsonIgnore]
        public string ProfileImageReference { get; set; }
        [Required]
        public int Board { get; set; }
        [Required]
        public string BoardName { get; set; }
        [Required]
        public string PassingYear { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public int ReservationCategory { get; set; }
        [Required]
        public int CasteCategory { get; set; }
        [Required]
        public int Caste { get; set; }
        [Required]
        public int Nationality { get; set; }
        [Required]
        public string AadhaarNumber { get; set; }
        
        [Required]
        public bool PunjabResident { get; set; }
        [Required]
        public bool PunjabiInMetric { get; set; }
        [Required]
        public bool TwelvethFromPunjab { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string RegistrationNumber { get; set; }
        public bool IsManualData { get; set; }
        public bool IsDiploma { get; set; }
        public int stream { get; set; }
        

    }

    public class subject
    {
        public string name { get; set; }
        public string marksTotal { get; set; }
        public string marksMax { get; set; }
    }
}
