using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.StudentPG
{
    public class RegisterStudent
    {
        [Required]
        public string RollNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int UniversityId { get; set; }
        [Required]
        public string PassingYear { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Mname { get; set; }
        [Required]
        public string Dob { get; set; }
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
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool KashmiriMigrant { get; set; }
        [Required]
        public bool PunjabResident { get; set; }
        [Required]
        public bool PunjabiInMetric { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string RegistrationNumber { get; set; }
        public bool IsFromBorderArea { get; set; }
    }
}


  
  


