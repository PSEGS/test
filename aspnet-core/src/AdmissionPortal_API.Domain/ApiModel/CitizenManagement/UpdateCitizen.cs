using System;
using System.ComponentModel.DataAnnotations;

namespace AdmissionPortal_API.Domain.ApiModel.CitizenManagement
{
    public class UpdateCitizen
    {
        public long CitizenID { get; set; }
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
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile is required")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public char Gender { get; set; }
        public string PermanentAddress { get; set; }
        public long? PermanentAddressDistrictId { get; set; }
        // public long? PermanentAddressTehsilId { get; set; }
        //public long? PermanentAddressBlockId { get; set; }
        //public long? PermanentAddressVillageId { get; set; }
        //public string CommunicationAddress { get; set; }
        //public long? CommunicationAddressDistrictId { get; set; }
        //public long? CommunicationAddressTehsilId { get; set; }
        //public long? CommunicationAddressBlockId { get; set; }
        //public long? CommunicationAddressVillageId { get; set; }
        public long? ModifiedBy { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
}

