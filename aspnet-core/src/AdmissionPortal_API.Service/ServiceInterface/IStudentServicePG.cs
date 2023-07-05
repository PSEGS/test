using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ApiModel.StudentPG;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IStudentServicePG
    {
        Task<ServiceResult> RegisterStudentPG(RegisterStudent model);
        Task<ServiceResult> StudentLoginPG(StudentPGLogin model);
        Task<ServiceResult> UpdateStudentPersonalDetails(UpdatePersonalDetails model);
        Task<ServiceResult> UpdateBankDetails(UpdateBankDetails model);
        Task<ServiceResult> UpdateAddressDetails(UpdateAddressDetails model);
        Task<ServiceResult> UpdateAcademicDetails(UpdateAcademicDetailsPG model);
        Task<ServiceResult> UpdateWeightage(WeightagePG model);
        Task<ServiceResult> UploadDocuments(UploadDocumentsPG model);
        Task<ServiceResult> GetStudentById(int studentID);
        Task<ServiceResult> GetSubjectByStudentId(int studentID);
        Task<ServiceResult> GetProgressBar(int studentID);
        Task<ServiceResult> UpdateDeclarations(Declaration model);
        Task<ServiceResult> GenerateOTP(string userName);
        Task<ServiceResult> UpdateCourseChoice(CourseChoice model);
        Task<ServiceResult> GetUploadedDocuments(int studentID);
        ServiceResult GetBlobDocument(string blobReference);
        Task<ServiceResult> GetCourseChoiceByStudentId(int studentID);
        Task<ServiceResult> RegisterationFeesPayment(RegisterationFees model);
        Task<ServiceResult> UnlockForm(string oTP, int studentId);
        Task<ServiceResult> UpdateStudentDetails(UpdateStudentDetails model);
        Task<ServiceResult> ForgotPassword(string Email);
        Task<ServiceResult> GetStudentDetailsByRegId(string RegId);
        Task<ServiceResult> GetStudentAppDetailsByRegId(FilterStudent _filter);
        Task<ServiceResult> GetCourseChoiceByRegId(string RegId);
        Task<ServiceResult> GetSubjectByRegId(string RegId);
        Task<ServiceResult> GetDocumentsByRegId(string RegId);
        Task<ServiceResult> GetObjectionsByPGStudentID(Int32 studentid);
        Task<ServiceResult> GetAdmissionSeatDetails(Int32 Studentid);
        Task<ServiceResult> GetAdmissionFeeReceipt(Int32 Studentid, string transactionId);
        Task<ServiceResult> GetAdmissionFeeReceiptList(Int32 Studentid);
    }
}
