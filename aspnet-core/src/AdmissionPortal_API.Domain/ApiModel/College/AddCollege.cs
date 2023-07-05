using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AdmissionPortal_API.Domain.ApiModel.College
{
    public class UpdateCollege
    {
        public int CollegeId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string CollegeName { get; set; }
        [Required(ErrorMessage = "College Type is required")]
        public string CollegeTypeId { get; set; }

        [Required(ErrorMessage = "University is required")]
        public int UniversityId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string CollegeEmail { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string CollegeContact { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string CollegeAddress { get; set; }
        [Required(ErrorMessage = "District is required")]
        public int DistrictId { get; set; }
        [Required(ErrorMessage = "Education mode is required")]
        public string EducationMode { get; set; }
        [Required(ErrorMessage = "Allowed Courses is required")]
        public int CGType { get; set; }

        [Required(ErrorMessage = "Name of Principle is required")]
        public string NameofPrincipal { get; set; }
        public string PrincipalPhone { get; set; }
        [Required(ErrorMessage = "Nodel Officer is required")]
        public string NodalOfficer { get; set; }
        [Required(ErrorMessage = "Nodel Officer Phone is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string NodelOfficerPhone { get; set; }
        [Required(ErrorMessage = "Nodel Officer Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
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
        [JsonIgnore]
        public string Password { get; set; }
        public string CollegeWebsite { get; set; }
        public string ShortName { get; set; }
         
         public BankOptions BankDetails { get; set; }
    
    }
    public class BankOptions
    {
        public BankDetails Regular_Course { get; set; }
        public BankDetails Self_Financed_Course { get; set; }
    }

    public class BankDetails
    {
        public string BankName { get; set; }
        public string AccountholderName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankIFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string BankMICRCode { get; set; }
        [JsonIgnore]
        public int CourseFundTypeId { get; set; }



    }
    public class AddCollege
    {
        [Required(ErrorMessage = "Please Provide University")]
        public int UniversityId { get; set; }
        [Required(ErrorMessage = "Please Provide College Name")]

        public string CollegeName { get; set; }
        [Required(ErrorMessage = "Please Provide College Type")]

        public int CollegeTypeId { get; set; }
        [Required(ErrorMessage = "Please Provied College Email")]

        public string CollegeEmail { get; set; }
        [Required(ErrorMessage = "Please Provied College Contact")]

        public string CollegeContact { get; set; }

        public string CollegeAddress { get; set; }
        [Required(ErrorMessage = "Please Provide District")]

        public int DistrictId { get; set; }
        //[Required(ErrorMessage = "Please Provied Nodal Officer")]

        public string NodalOfficer { get; set; }
        [Required(ErrorMessage = "Please Provide College Mode")]

        public int EducationMode { get; set; }
        [Required(ErrorMessage = "Please Provied Nodel Officer Number")]

        public string NodelOfficerPhone { get; set; }
        public string NodalOfficerEmail { get; set; }
        public string ShortName { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]

        public string password { get; set; }

        public int? CGType { get; set; }

    };

    public class UpdateCollegeModel
    {
        [Required(ErrorMessage = "Please Provide University")]
        public int UniversityId { get; set; }
        [Required(ErrorMessage = "Please Provide College Id")]

        public int CollegeId { get; set; }
        [Required(ErrorMessage = "Please Provide College Name")]

        public string CollegeName { get; set; }
        [Required(ErrorMessage = "Please Provide College Type")]

        public int CollegeTypeId { get; set; }
        [Required(ErrorMessage = "Please Provied College Email")]

        public string CollegeEmail { get; set; }
        //[Required(ErrorMessage = "Please Provied College Contact")]

        public string CollegeContact { get; set; }

        public string CollegeAddress { get; set; }
        [Required(ErrorMessage = "Please Provide District")]

        public int DistrictId { get; set; }
        //[Required(ErrorMessage = "Please Provied Nodal Officer")]

        public string NodalOfficer { get; set; }
        [Required(ErrorMessage = "Please Provide College Mode")]

        public int EducationMode { get; set; }
        [Required(ErrorMessage = "Please Provied Nodel Officer Number")]

        public string NodelOfficerPhone { get; set; }
        public string NodalOfficerEmail { get; set; }
        public string ShortName { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]

        public string password { get; set; }

        public int? CGType { get; set; }

    };


    public class GetCollege
    {         
        public int CollegeId { get; set; }
        public int sno { get; set; }
        public string CollegeName { get; set; }
        public string CollegeType { get; set; }
        public int CollegeTypeId { get; set; }

        public int? CGType { get; set; }
        public string UniversityName { get; set; }
        public int UniversityId { get; set; }

        public string CollegeEmail { get; set; }
        public string CollegeContact { get; set; }
        public string CollegeAddress { get; set; }

        public string DistrictName { get; set; }
        public int DistrictId { get; set; }
        public string EducationMode { get; set; }
        public string EducationModeId { get; set; }

        public string NameofPrincipal { get; set; }
        public string PrincipalPhone { get; set; }
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
        public string CollegeWebsite { get; set; }
        public string ShortName { get; set; }
        public string NodalOfficer { get; set; }
        public string NodalOfficerEmail { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmailID { get; set; }
        public string ContactPersonMobile { get; set; }
        public string username { get; set; }
        public Int32 TotalCount { get; set; }
        public string  IsActive { get; set; }
        public Boolean isLock { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankIFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string BankMICRCode { get; set; }
        public string AccountHolderName { get; set;}
        public string BankNameSF { get; set; }
        public string BankAccountNumberSF { get; set; }
        public string BankIFSCCodeSF { get; set; }
        public string BankBranchSF { get; set; }
        public string BankMICRCodeSF { get; set; }
        public string SFCancelledCheque { get; set; }
        public string CancelledCheque { get; set; }
        public string prospectusRef { get; set; }
        public string AccountHolderNameS { get; set; }



    }

    public class GetCollegeByDistrict
    {
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeType { get; set; }
    }

    public class GetAllCollege
    {
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
    }
    public class UploadCollegeProspectus
    {
        [JsonIgnore]

        public int CollegeID { get; set; }
        public IFormFile? file { get; set; }
        [JsonIgnore]
        public string prospectus { get; set; }
        [JsonIgnore]
        public string prospectusRef { get; set; }
    }
    public class UploadCancelledChequeModel
    {
        [JsonIgnore]

        public int CollegeID { get; set; }
        [Required]
        public int CourseFundType { get; set; }
        public IFormFile? file { get; set; }
        [JsonIgnore]
        public string CancelledCheque { get; set; }
    }
    public class UnlockStudentModel
    {
        [JsonIgnore]

        public int CollegeID { get; set; }
        [Required]
        public string RegistrationId { get; set; }
        [Required]

        public string CourseType { get; set; }
        [Required]

        public string Remarks { get; set; }
    }
    public class CollegeActiveInActiveResponse
    {
        public int Response { get; set; }
        public string SysMessage { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CollegeName { get; set; }
    }
}

