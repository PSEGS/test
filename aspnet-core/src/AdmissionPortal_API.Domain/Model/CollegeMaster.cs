using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AdmissionPortal_API.Domain.Model
{
    public class CollegeMaster
    {
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeTypeId { get; set; }
        public int UniversityId { get; set; }
        public string CollegeEmail { get; set; }
        public string CollegeContact { get; set; }
        public string CollegeAddress { get; set; }
        public int DistrictId { get; set; }
        public string EducationMode { get; set; }
        public string NameofPrincipal { get; set; }
        public string PrincipalPhone { get; set; }
        public string NodalOfficer { get; set; }
        public string NodelOfficerPhone { get; set; }
        public string NodalOfficerEmail { get; set; }
        public string CoOrdinatorArtsStreamName { get; set; }
        public string CoOrdinatorArtsStreamPhone { get; set; }
        public string CoOrdinatorCommerceStreamName { get; set; }
        public string CoOrdinatorCommerceStreamPhone { get; set; }
        public string CoOrdinatorScienceStreamName { get; set; }
        public string CoOrdinatorScienceStreamPhone { get; set; }
        public string CoOrdinatorJobOrientedCourseName { get; set; }
        public string CoOrdinatorJobOrientedCoursePhone { get; set; }
        public string CoOrdinatorFeeStructureName { get; set; }
        public string CoOrdinatorFeeStructurePhone { get; set; }
        public string CreatedBy { get; set; }
        public string Password { get; set; }
        public string CollegeWebsite { get; set; }
        public string ShortName { get; set; }
        public BankOptions BankDetails { get; set; }
        public string prospectusRef { get; set; }


    }
    public class BankOptions
    {
        public BankDetails Regular_Course { get; set; }
        public BankDetails Self_Financed_Course { get; set; }
    }

    public class BankDetails
    {
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankIFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string BankMICRCode { get; set; }
        [JsonIgnore]
        public int FundTypeId { get; set; }

    }
    public class AllCollegesISLOCK
    {
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
    }


}

