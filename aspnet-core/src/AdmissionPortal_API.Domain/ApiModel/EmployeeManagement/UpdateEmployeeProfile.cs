using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.EmployeeManagement
{
    public class UpdateEmployeeProfile
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "FatherName is required")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "MotherName is required")]
        public string MotherName { get; set; }
        public string Mobile { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public char Gender { get; set; }
        public string PermanentAddress { get; set; }
        public long? PermanentAddressDistrictId { get; set; }
        [Required]
        public string Email { get; set; }
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string UserType { get; set; }
        [Required]
        public int EmploymentType { get; set; }
        public string DigiSignSerialNumber { get; set; }

    }
}

