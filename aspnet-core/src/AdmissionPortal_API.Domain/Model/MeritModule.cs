using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class MeritModule
    {
        public Boolean? IsSeatOffered { get; set; }
        public Boolean? IsSeatBooked { get; set; }
        public int? SeatBookedCollegeID { get; set; }
        public string PaymentMode { get; set; }
        public List<MeritCourse> Course { get; set; }
    }

    public class StudentSeatBookedOffered
    {
        public Boolean IsSeatOffered { get; set; }
        public Boolean IsSeatBooked { get; set; }
        public Boolean IsPaymentDone { get; set; }
        public string PaymentMode { get; set; }
        public List<OfferedCourses> Course { get; set; }
    }
    public class OfferedCourses
    {
        public Int32 CourseId { get; set; }
        public string CourseName { get; set; }
        public string PaymentAmount { get; set; }
        public string CombinationId { get; set; }
        public string CombinationName { get; set; }
        public string CollegeName { get; set; }
        public string CollegeId { get; set; }
        public string CourseType { get; set; }
        public string OfferedPaymentMode { get; set; }
    }

    public class AdmissionSeatPaymentReciept
    {
        public string CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string StudentID { get; set; }
        public string RegistrationNumber { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string AdmissionSeatCategory { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public string CombinationId { get; set; }
        public string CombinationName { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string TotalAmount { get; set; }
        public string PaymentGateway { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentAmount { get; set; }
        public string BalanceAmount { get; set; }
        public string PaymentDate { get; set; }
        public string ConcessionFee { get; set; }
        public Boolean? IsfeeConcession { get; set; }
        public string status { get; set; }
    }

    public class AdmissionSeatPaymentRecieptList
    {
        public string CollegeName { get; set; }
        public string StudentID { get; set; }
        public string RegistrationNumber { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string AdmissionSeatCategory { get; set; }
        public string CombinationName { get; set; }
        public string CourseName { get; set; }
        public string TransactionId { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentDate { get; set; }
        public string Status { get; set; }
    }

    public class ProvisionalList
    {
        public string sno { get; set; }
        public string TotalCount { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MobileNo { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string TwelvethPercentage { get; set; }
        public string YearOfPassing { get; set; }
        public string Weightage { get; set; }
        public string FinalWeightage { get; set; }
        public string RegistrationNumber { get; set; }
        public string CollegeName { get; set; }
        public string CourseName { get; set; }
        public string AdmissionSeatCategory { get; set; }
        public string Rank { get; set; }
        public string AdmissionSeatReservationCategory { get; set; }
        public string CreatedOn { get; set; }
        public string Meritstatus { get; set; }
        public string Category { get; set; }
        public string MeritInCourses { get; set; }
    }

    public class WaitingList
    {
        public string sno { get; set; }
        public string TotalCount { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MobileNo { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string TwelvethPercentage { get; set; }
        public string YearOfPassing { get; set; }
        public string Weightage { get; set; }
        public string FinalWeightage { get; set; }
        public string RegistrationNumber { get; set; }
        public string CollegeName { get; set; }
        public string CourseName { get; set; }
        public string AdmissionSeatCategory { get; set; }
        public string Rank { get; set; }
        public string AdmissionSeatReservationCategory { get; set; }
        public string CreatedOn { get; set; }
        public string Meritstatus { get; set; }
        public string Category { get; set; }
    }

    public class AdmissionSeat
    {
        public string RegistrationNumber { get; set; }
        [RegularExpression("Online|Offline|Both", ErrorMessage = "Please enter Valid Payment Type")]

        public string PaymentMode { get; set; }
        public int? CourseId { get; set; }
        public string CombinationId { get; set; }
        public string TotalAmount { get; set; }
        public string PaymentAmount { get; set; }
        public string BalanceAmount { get; set; }
        public string PaymentType { get; set; }
        public string CollegeId { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        public bool? IsFeeConcession { get; set; } = false;
        public int? ConcessionFee { get; set; }

        [RegularExpression("Merit|Waiting", ErrorMessage = "Please enter Valid Admission Type")]
        public string AdmissionType { get; set; } = "Merit";
        public int? ReservationCategoryId { get; set; }
        public string Partialpaymentremark { get; set; }

    }


    public class Combination
    {
        public string CombinationId { get; set; }
        public string CombinationName { get; set; }
    }

    public class MeritCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string AdmissionSeatCategory { get; set; }
        public string Rank { get; set; }
        public string Weightage { get; set; }
    }


    public class SaveAdmissionSeatModel
    {
        public int Response { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string StudentName { get; set; }
        public string RegistrationNumber { get; set; }
        public string CollegeName { get; set; }
    }
    public class studentFeeHeadModel
    {
        public int Id { get; set; }
        public int HeadTypeId { get; set; }
        public string HeadName { get; set; }
        public decimal Fee { get; set; }
        public Boolean iswaiver { get; set; }
        public int? Parameter { get; set; }
        public int? fundtype { get; set; }
        public Boolean applicable { get; set; }
    }
    public class CombinationStudentFee
    {
        public int collegeId { get; set; }
        public int courseId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int PracticalFee { get; set; }
        public bool IsselffinaceAcc { get; set; }
        public bool IsRegularAcc { get; set; }


    }
    public class StudentCourseFee
    {
        public decimal TotalCourseFee { get; set; }
        public decimal? TotalCombinationFee { get; set; }
        public decimal TotalFee { get; set; }
    }
    public class StudentFeeDetails
    {
        public List<studentFeeHeadModel> studentFeeHeadModels { get; set; }
        public List<CombinationStudentFee> studentCombinationFees { get; set; }
        public StudentCourseFee studentCourse { get; set; }

    }
    public class PGStudentfeeHeadModel
    {
        public List<studentFeeHeadModel> studentFeeHeadModels { get; set; }
        public StudentCourseFee studentCourse { get; set; }

    }
    public class CategoryCombination
    {
        public string id { get; set; }
        public string category { get; set; }
        public string categoryId { get; set; }
        public string SubCategoryId { get; set; }
    }

    public class OTPModel
    {
        public int Response { get; set; }

        public string StudentId { get; set; }
        public string StudentType { get; set; }
        public string OTPType { get; set; }
        public string OTPToken { get; set; }
        public string Mobile { get; set; }
        public bool Is_verified { get; set; }
        public string CourseName { get; set; }
        public string CollageName { get; set; }
        public string StudentName { get; set; }
        public string PaymentMode { get; set; }
        public DateTime Created_Date { get; set; }
        public string EmailId { get; set; }


    }

    public class OTPRequestModel
    {

        [Required]
        public string StudentReferenceNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CourseId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CollegeId { get; set; }

    }

    public class VerifyOTP
    {
        [Required]
        public string StudentReferenceNumber { get; set; }
        [Required]
        public string OTP { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CourseId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CollegeId { get; set; }

        [RegularExpression("Merit|Waiting", ErrorMessage = "Please enter Valid Admission Type")]
        public string AdmissionType { get; set; } = "Merit";

    }

    public class CourseSubjectCount
    {
        public string CourseName { get; set; }
        public string SubjectName { get; set; }
        public int? SubjectSeat { get; set; }
        public int? BookedSubjectSeat { get; set; }
        public int? OfferedSubjectSeat { get; set; }
        public int? TotalSeat { get; set; }
        public int? BookedSeats { get; set; }
    }

    public class RevokeSeat
    {
        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public string CollegeId { get; set; }

        [Required]
        public string revokeRemark { get; set; }

        [JsonIgnore]
        public int? revokeBy { get; set; }
        [RegularExpression("Merit|Waiting", ErrorMessage = "Please enter Valid Admission Type")]
        public string AdmissionType { get; set; } = "Merit";
    }

    public class RevokeAdmissionSeatModel
    {
        public int Response { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CourseName { get; set; }
        public string RegistrationNumber { get; set; }
        public string CollegeName { get; set; }
    }
    public class CancelAdmissionSeatModel
    {
        public int Response { get; set; }
        public string Mobile { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public string CollegeName { get; set; }
        public string Email { get; set; }
    }

    public class VacantSeatForWaitingList
    {
        public int Response { get; set; }
        public int TotalVacantSeats { get; set; }
    }

    public class UpdateBookedSeatMatrix
    {
        [JsonIgnore]
        public Int32 UserId { get; set; }
        public int BalanceSeats { get; set; }
        public int CombinationId { get; set; }
    }
    public class GetvacantSeatList
    {
        public string id { get; set; }
        public string CourseName { get; set; }
        public string Type { get; set; }
        public string TotalSeat { get; set; }
        public string General { get; set; }
        public string SC { get; set; }
        public string BC { get; set; }
        public string PWD { get; set; }
        public string FreedomFighters { get; set; }
        public string SportsGen { get; set; }
        public string SportsSC { get; set; }
        public string ExSerGen { get; set; }
        public string ExSerSC { get; set; }
        public string ExSerBc { get; set; }
        public string TotalCancerSeat { get; set; }
        public string TotalAIDSSeat { get; set; }
        public string TotalThalassemicSeat { get; set; }
        public string TotalOneGirlSeat { get; set; }
        public string TotalNRISeat { get; set; }
        public string TotalKashmiriRiotVictims { get; set; }
        public string TotalBorderAreaSeat { get; set; }
        public string TotalRuralAreaSeat { get; set; }
    }
}
