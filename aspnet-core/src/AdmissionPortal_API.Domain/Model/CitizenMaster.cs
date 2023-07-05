using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AdmissionPortal_API.Domain.Model
{
    public class CitizenMaster
    {
        public long CitizenID { get; set; }
        public string FirstName { get; set; }
        //public string FirstNameLocal { get; set; }
        public string MiddleName { get; set; }
        //public string MiddleNameLocal { get; set; }
        public string LastName { get; set; }
        //public string LastNameLocal { get; set; }
        public string FatherName { get; set; }
        //public string FatherNameLocal { get; set; }
        public string MotherName { get; set; }
        //public string MotherNameLocal { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime DateOfBirth { get; set; }
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
        //public bool IsActive { get; set; }
        //public long CreatedBy { get; set; }
        //public DateTime? ModifiedOn { get; set; }
        //public long? ModifiedBy { get; set; }
        //public string UserPassword { get; set; }
        //public DateTime PasswordExp { get; set; }
        //public bool IsFirstLogin { get; set; }
        //public int LoginAttempts { get; set; }
        [IgnoreDataMember]
        public string NewPassword { get; set; }
        [IgnoreDataMember]
        public string OldPassword { get; set; }
        [IgnoreDataMember]
        public bool IsChangePassword { get; set; }
        [IgnoreDataMember]
        public long PageNumber { get; set; }
        [IgnoreDataMember]
        public long PageSize { get; set; }
        [IgnoreDataMember]
        public string SearchKeyword { get; set; }
        [IgnoreDataMember]
        public string SortBy { get; set; }
        [IgnoreDataMember]
        public string SortOrder { get; set; }
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }
        [IgnoreDataMember]
        public int Id { get; set; }
        [IgnoreDataMember]
        public string UserType { get; set; }
        [IgnoreDataMember]
        public string ImageReference { get; set; }
        public string ImageCode { get; set; }

        [JsonIgnore]
        public string UserPassword { get; set; }
    }
}

