using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace AdmissionPortal_API.Domain.ApiModel.EmployeeManagement
{
    public class UpdateEmployee
    {
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "FatherName is required")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "MotherName is required")]
        public string MotherName { get; set; }

        //[Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Mobile is required")]
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
        //public long DepartmentId { get; set; }
        //[Required]
        //public long DesignationId { get; set; }
        //[Required]
        //public long OfficeId { get; set; }
        //[Required]
        //public DateTime DateOfJoining { get; set; }
        //[Required]
        //public DateTime DateOfRetirement { get; set; }
        //public long? HRMSEmpCode { get; set; }
        public long? ModifiedBy { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string RoleIds { get; set; }
        [Required]
        public int EmploymentType { get; set; }
        public string DigiSignSerialNumber { get; set; }
        public bool IsDeptAdmin { get; set; }
    }

}


