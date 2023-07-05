using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class StudentDetailsVerification
    {
        public List<BasicDetailsVerification> PersonalDetail { get; set; }
        public Academicdetails AcademicDetails { get; set; }
        public List<AcademicSubject> AcademicSubjects { get; set; }
        public List<ChoiceOfCourseforverification> ChoiceOfCourseforVerifications { get; set; }
        public List<Weightageforverification> Weightages { get; set; }
        public List<ReservationDetails> ReservationDetails { get; set; }
        public List<ActionHistoryVerification> ActionHistory { get; set; }

    }

    public class BasicDetailsVerification
    {
        public string Photograph { get; set; }
        public string PhotographRef { get; set; }
        public string SignatureRef { get; set; }
        public string Signature { get; set; }
        public string ApplicantName { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string RegistrationId { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string MetricCertificate { get; set; }
        public string MetricCertificateRef{ get; set; }
        public int Category { get; set; }
        public string ReservationCategory { get; set; }
        public string MotherName { get; set; }
        public int?  SectionStatus { get; set; }
        public int? IsLocked { get; set; }
        public int IsFinalSubmit { get; set; }


    }

    public class Academicdetails
    {
        public string Examination { get; set; }
        public string SchoolCollegeName { get; set; }
        public string Stream { get; set; }
        public string SchoolBoard { get; set; }
        public string rollNo { get; set; }
        public string YearOfPassing { get; set; }
        public string TotalMarks { get; set; }
        public decimal? ObtainedMarks { get; set; }
        public decimal? TotalPercentage { get; set; }
        public string Result { get; set; }
        public string Certificate { get; set; }
        public Boolean IsManualData { get; set; }
        public int? SectionStatus { get; set; }
        public decimal? TopFiveTotalMarks { get; set; }
        public decimal? TopFiveObtainedNumber { get; set; }
        public decimal? TopFiveSubjectPercentage { get; set; }
        public decimal? GapYear { get; set; }

        public bool? IsAlreadyScholarship { get; set; } = false;
        public bool? IsCMScholarship { get; set; } = false;
        public string Amount { get; set; }
        public string SchemeName { get; set; }
        public string Sponseredby { get; set; }





    }

    public class PGAcademicdetails
    {
        public string Examination { get; set; }
        public string SchoolCollegeName { get; set; }
        public string CourseName { get; set; }
        public string UniversityName { get; set; }
        public string rollNo { get; set; }
        public string YearOfPassing { get; set; }
        public string TotalMarks { get; set; }
        public decimal? ObtainedMarks { get; set; }
        public decimal? TotalPercentage { get; set; }
        public string Result { get; set; }
        public string Certificate { get; set; }
        public Boolean IsManualData { get; set; }
        public int? SectionStatus { get; set; }
        public decimal? GapYear { get; set; }



    }
    public class AcademicSubject
    {
        public string SrNo { get; set; }
        public string SubjectName { get; set; }
        public string MaxNumber { get; set; }
        public string ObtainedNumber { get; set; }
    }
    public class ChoiceOfCourseforverification
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string CollegeId { get; set; }
        public string CourseName { get; set; }
        public Boolean? VerificationStatus { get; set; }
        public string University { get; set; }
    }

    public class AcademicResult
    {
        public Academicdetails AcademicDetails { get; set; }
        public List<AcademicSubject> AcademicSubjects { get; set; }
        public List<ChoiceOfCourseforverification> ChoiceOfCourseforVerifications { get; set; }
    }
    public class PGAcademicResult
    {
        public PGAcademicdetails AcademicDetails { get; set; }
         
        public List<ChoiceOfCourseforverification> ChoiceOfCourseforVerifications { get; set; }
    }
    public class Weightageforverification
    {
        public string DocID { get; set; }
        public string DocmentType { get; set; }
        public string DocmentBlobReference { get; set; }
        public string Image { get; set; }
        public string NCCOption { get; set; }
        public string NSSOption { get; set; }
        public int? SectionStatus { get; set; }

    }
    public class ReservationDetails
    {
        public string ReservationId { get; set; }
        public string Reservation { get; set; }
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public string CasteName { get; set; }
        public string CasteID { get; set; }
        public Boolean IsPunjabDomicile { get; set; }
        public string DocID { get; set; }
        public string DocmentType { get; set; }
        public string ImageRef { get; set; }
        public string Image { get; set; }
        public Boolean isCasteSerialNumberVerified { get; set; }
        public int? SectionStatus { get; set; }
    }
    public class MeritStatusVerification
    {
        public string CollegeName { get; set; }
        public string CourseName { get; set; }
        public string SubjectCombination { get; set; }
        public string Counseling { get; set; }
    }
    public class AdmissionStatusPostMeritVerification
    {
        public string CollegeName { get; set; }
        public string CourseName { get; set; }
        public string SubjectCombination { get; set; }
        public string DateOfAdmission { get; set; }
        public string AdmissionStatus { get; set; }
        public string PaymentTransactionId { get; set; }
    }
    public class ActionHistoryVerification
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public string Remarks { get; set; }
        public string ActionBy { get; set; }
    }
}
