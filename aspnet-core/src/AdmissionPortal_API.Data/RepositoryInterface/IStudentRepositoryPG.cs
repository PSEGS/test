using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ApiModel.StudentPG;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IStudentRepositoryPG
    {
        Task<StudentRegisterResponseModel> AddAsync(RegisterStudent entity);
        Task<StudentLoginMaster> GetAsync(StudentPGLogin entity);
        Task<int> UpdateStudentPersonalDetails(UpdatePersonalDetails model);
        Task<int> UpdateBankDetails(UpdateBankDetails model);
        Task<int> UpdateAddressDetails(UpdateAddressDetails model);
        Task<int> UpdateAcademicDetails(UpdateAcademicDetailsPG model);
        Task<int> UpdateWeightage(WeightagePG model);
        Task<int> UploadDocuments(UploadDocumentsPG model);
        Task<StudentMasterPG> GetStudentById(int studentID);
        Task<List<GetStudentAcademicPG>> GetSubjectByStudentId(int studentID);
        Task<ProgressBar> GetProgressBar(int studentID);
        Task<int> UpdateDeclarations(Declaration model);
        Task<string> GenerateOTP(string userName, string otp);
        Task<int> UpdateCourseChoice(CourseChoice model);
        Task<UploadedDocumentDetailPG> GetUploadedDocuments(int studentID);
        Task<List<SubjectCombinationDetail>> GetCourseChoiceByStudentId(int studentID);
        Task<int> RegisterationFeesPayment(RegisterationFees model);
        Task<int> UnlockForm(string oTP, int studentId);
        Task<int> UpdateStudentDetails(UpdateStudentDetails model);
        Task<ForgotLoginMaster> ForgotPassword(string email);
        Task<StudentDetails> GetStudentDetailsByRegId(string RegId);
        Task<StudentMaster> GetStudentAppDetailsByRegId(FilterStudent _filter);
        Task<List<SubjectCombinationDetail>> GetCourseChoiceByRegId(string RegId);
        Task<List<GetStudentAcademicPG>> GetSubjectByRegId(string RegId);
        Task<UploadedDocumentDetailPG> GetDocumentsByRegId(string RegId);
        Task<List<ObjectionListByStudentModel>> GetObjectionsByPGStudentID(Int32 studentid);
        Task<StudentSeatBookedOffered> GetAdmissionSeatDetails(int StudentId);
        Task<AdmissionSeatPaymentReciept> GetAdmissionFeeReceipt(int StudentId, string transactionId);
        Task<List<AdmissionSeatPaymentRecieptList>> GetAdmissionFeeReceiptList(int StudentId);
    }
}
