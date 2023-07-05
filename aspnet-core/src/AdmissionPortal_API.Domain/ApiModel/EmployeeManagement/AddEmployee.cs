using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace AdmissionPortal_API.Domain.ApiModel.EmployeeManagement
{
    public class AddEmployee
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

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]

        public string Mobile { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public char Gender { get; set; }
        public string PermanentAddress { get; set; }
        public long? PermanentAddressDistrictId { get; set; }
        //public long? PermanentAddressTehsilId { get; set; }
        //public long? PermanentAddressBlockId { get; set; }
        //public long? PermanentAddressVillageId { get; set; }
        //public string CommunicationAddress { get; set; }
        //public long? CommunicationAddressDistrictId { get; set; }
        //public long? CommunicationAddressTehsilId { get; set; }
        //public long? CommunicationAddressBlockId { get; set; }
        //public long? CommunicationAddressVillageId { get; set; }
        //[Required]
        public long DepartmentId { get; set; }
        //[Required]
        //public long DesignationId { get; set; }
        //[Required]
        //public long OfficeId { get; set; }
        //[Required]
        //public DateTime DateOfJoining { get; set; }
        //[Required]
        //public DateTime DateOfRetirement { get; set; }
        //public long? HRMSEmpCode { get; set; }
        [Required]
        public string RoleIds { get; set; }
        [Required]
        public int EmploymentType { get; set; }
        public string DigiSignSerialNumber { get; set; }
        [JsonIgnore]
        public string UserPassword { get; set; }
        public bool IsDeptAdmin { get; set; }
    }


}

