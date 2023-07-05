using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class StudentDetails
    {
        public List<BasicDetails> studentDetail { get; set; }
        public List<MeritStatus> meritStatus { get; set; }
        public List<AdmissionStatusPostMerit> admissionStatus { get; set; }
        public List<ActionHistory> actionHistory { get; set; }
        public StudentVerificationDetails verificationDetails { get; set; }
    }
    public class StudentDetailsForCancellation
    {
        public List<BasicDetailsForCancellation> studentDetail { get; set; }
        public List<MeritStatus> meritStatus { get; set; }
        public List<AdmissionStatusPostMerit> admissionStatus { get; set; }
    }
    public class BasicDetailsForCancellation
    {
        public string RegistrationId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public Boolean IsAdmissionCancelled { get; set; }
    }
    public class StudentVerificationDetails
    {
        public string CollegeName { get; set; }
        public string VerifiedOn { get; set; }
        public string Status { get; set; }
    }
    public class CancelAdmissionSeat
    {
        public string RegId { get; set; }
        public string Doc { get; set; }
        public string DocReference { get; set; }
        public string type { get; set; }
        public int collegeId { get; set; }
        public string Remarks { get; set; }
    }

    public class BasicDetails
    {
        public string SrNo { get; set; }

        public string RegistrationId { get; set; }

        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string Board { get; set; }
        public string PassingYear { get; set; }

        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string SubmissionDate { get; set; }
        public string UnlockedDate { get; set; }
        public string ObjectionRaisedbyVerifier { get; set; }
        public string TotalCount { get; set; }
        public string RollNo { get; set; }
        public bool isLocked { get; set; } = true;
    }
    public class BasicDetailsNew
    {
        [Display(Name = "SrNO")]
        public string SrNo { get; set; }

        [Display(Name = "RegistrationId")]

        public string RegistrationId { get; set; }

        [Display(Name = "StudentName")]

        public string StudentName { get; set; }
        [Display(Name = "FatherName")]

        public string FatherName { get; set; }
        [Display(Name = "Board")]

        public string Board { get; set; }
        [Display(Name = "PassingYear")]

        public string PassingYear { get; set; }
        [Display(Name = "MobileNo")]


        public string MobileNo { get; set; }
        [Display(Name = "EmailId")]

        public string EmailId { get; set; }
         
        [Display(Name = "TotalCount")]

        public string TotalCount { get; set; }
        public string CourseName { get; set; }
        public string Gender { get; set; }


    }

    public class MeritStatus
    {
        public string CollegeName { get; set; }
        public string CourseName { get; set; }
        public string SubjectCombination { get; set; }
        public string Counseling { get; set; }
    }
    public class AdmissionStatusPostMerit
    {
        public string CollegeName { get; set; }
        public string CourseName { get; set; }
        public string SubjectCombination { get; set; }
        public string DateOfAdmission { get; set; }
        public string AdmissionStatus { get; set; }
        public string PaymentTransactionId { get; set; }
    }
    public class ActionHistory
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public string Remarks { get; set; }
        public string ActionBy { get; set; }
    }
    public class StudentCombinationDetails
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string SubjectCombination { get; set; }
        public int PreferenceChoice { get; set; }
    }

}
