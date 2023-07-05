﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace AdmissionPortal_API.Domain.ApiModel.Objection
{
    public class ObjectionDetails
    {
        public List<GetObjection> objections { get; set; }
        public List<StudentQualification> studentQualifications { get; set; }
    }

    public class AddObjection
    {
         
        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }

       
        public int CollegeId { get; set; }

        [Required(ErrorMessage = "Please enter Verification Date")]
        public DateTime VerificationDate { get; set; }

        [Required(ErrorMessage = "Please enter Verifier Remarks")]
        public string VerifierRemarks { get; set; }

        [Required(ErrorMessage = "Please enter Section Name")]
        public int SectionId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class GetObjection
    {
        public string ObjectionNo { get; set; }
        public string CandidateFullName { get; set; }
        public string MobileNo { get; set; }
        public string RegistrationNumber { get; set; }
        public string CollegeName { get; set; }
        public DateTime VerificationDate { get; set; }
        public string VerifierRemarks { get; set; }
        public int CreatedBy { get; set; }
        public string SectionName { get; set; }
        public bool Resolved { get; set; }
    }

    public class GetSingleObjection
    {
        public string ObjectionNo { get; set; }
        public string CandidateFullName { get; set; }
        public string MobileNo { get; set; }
        public string RegistrationNumber { get; set; }
        public string CollegeName { get; set; }
        public DateTime VerificationDate { get; set; }
        public string VerifierRemarks { get; set; }
        public int CreatedBy { get; set; }
        public string SectionName { get; set; }
        public bool Resolved { get; set; }
        public List<StudentQualification> StudentQualifications { get; set; }
    }

    public class StudentQualification
    {
        public int Qualification_Id { get; set; }
        public int Student_Id { get; set; }
        public string Examination { get; set; }
        public string BoardUniversity { get; set; }
        public string RollNo { get; set; }
        public string RegistrationNumber { get; set; }
    }

    public class VerifyStudentWithSection
    {
        public VerifyStudentWithSection()
        {
            IsapplicableCM = false;
            IsapplicablePM = false;

        }
        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Section is required")]
        public int SectionId { get; set; }
        [JsonIgnore]

        public int VerifiedBy { get; set; }
        public string Remarks { get; set; }
 
        [Required(ErrorMessage = "verificationAction is required")]
        public int verificationAction { get; set; }
        [JsonIgnore]
        public int CollegeId { get; set; }
        [DefaultValue(false)]
        public bool IsapplicableCM { get; set; }
        [DefaultValue(false)]

        public bool IsapplicablePM { get; set; }  
    }
    public class StudentChoiceofCourseEligible
    {
        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }
        [Required(ErrorMessage = "Action is required")]
        public int CourseAction { get; set; }
        [Required(ErrorMessage = "Course is required")]

        public int CourseId { get; set; }
        public string Remarks { get; set; } 
        [JsonIgnore]
        public int ActionTaken { get; set; }
    }
    public class FinalVerificationModel
    {
        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }
        [JsonIgnore]
        public int verifedBy { get; set; }
        [JsonIgnore]
        public int CollegeID { get; set; }
    }
    public class BCOMStudentWeightage
    {
        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; }
        [JsonIgnore]
        public int CollegeID { get; set; }
        
        public int weightage { get; set; }
    }
    public class VerificationReturnModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Mobile { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
    public class objectremarks
    {
        public string VerifierRemarks { get; set; }
    }
    public class ReturnModelVerification
    {
        public VerificationReturnModel verificationReturnModel { get; set; }
        public List<objectremarks> objectremarks { get; set; }
    }

}
