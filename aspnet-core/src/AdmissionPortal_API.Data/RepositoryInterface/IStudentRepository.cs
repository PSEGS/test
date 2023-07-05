using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IStudentRepository
    {
        Task<StudentRegisterResponseModel> AddAsync(AddStudent entity);
        Task<StudentLoginMaster> GetAsync(StudentLogin entity);
        Task<int> UpdateStudentPersonalDetails(UpdatePersonalDetails model);
        Task<int> UpdateBankDetails(UpdateBankDetails model);
        Task<int> UpdateAddressDetails(UpdateAddressDetails model);
        Task<int> UpdateAcademicDetails(UpdateAcademicDetails model);
        Task<int> UpdateWeightage(Weightage model);
        Task<int> UploadDocuments(UploadDocuments model);
        Task<StudentMaster> GetStudentById(int studentID);
        Task<List<StudentAcademicMaster>> GetSubjectByStudentId(int studentID);
        Task<ProgressBar> GetProgressBar(int studentID);
        Task<int> UpdateDeclarations(Declaration model);
        Task<string> GenerateOTP(string userName, string otp);
        Task<int> UpdateCourseChoice(CourseChoice model);
        Task<UploadedDocumentDetail> GetUploadedDocuments(int studentID);
        Task<SubjectCombinationForStudent> GetCourseChoiceByStudentId(int studentID);
        Task<int> RegisterationFeesPayment(RegisterationFees model);
        Task<int> UnlockForm(string oTP, int studentId);
        Task<int> UpdateStudentDetails(UpdateStudentDetails model);
        Task<ForgotLoginMaster> ForgotPassword(string email);
        Task<StudentDetails> GetStudentDetailsByRegId(string RegId, string RollNo);
        Task<StudentMaster> GetStudentAppDetailsByRegId(FilterStudent _filter);
        Task<SubjectCombinationForStudent> GetCourseChoiceByRegId(string RegId);
        Task<List<StudentAcademicMaster>> GetSubjectByRegId(string RegId);
        Task<UploadedDocumentDetail> GetDocumentsByRegId(string RegId);
        Task<int> UpdateCourseChoiceWithSubject(SubjectCombinationsWithCollege model);
        Task<SelectedCourseTotalFee> GetCourseChoiceFee(CourseChoiceFee model);
        Task<List<GetEligibleCourseByStudentID>> getEligibleCourseByStudentIDs(Int32 CollegeId, Int32 StudentId);
        Task<int> checkCollegeSubject(int collegeID, int courseId);
        Task<List<ObjectionListByStudentModel>> GetObjectionsByStudentID(Int32 StudentId);
        Task<StudentSeatBookedOffered> GetAdmissionSeatDetails(int StudentId);
        Task<AdmissionSeatPaymentReciept> GetAdmissionFeeReceipt(int StudentId, string transactionId);
        Task<List<AdmissionSeatPaymentRecieptList>> GetAdmissionFeeReceiptList(int StudentId);
    }
}
