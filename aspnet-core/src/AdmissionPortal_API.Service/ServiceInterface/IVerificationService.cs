using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.Objection;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IVerificationService
    {
        Task<ServiceResult> GetVerifiedStudentByRegIdCollege(string RegId, int CollegeId);
        Task<ServiceResult> GetStudentsByCollege(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> VerifyStudentWithSection(VerifyStudentWithSection model);
        Task<ServiceResult> RevokeStudentVerification(CancelStudentRegistration model);
        Task<ServiceResult> GetStudentByRegId(string RegId, Int32 type);
        Task<ServiceResult> StudentCourseEligible(StudentChoiceofCourseEligible model);
        Task<ServiceResult> FinalSubmit(FinalVerificationModel model);
        Task<ServiceResult> ExportExcelStudentsByCollege(Int32 collegeId, Int32? verificationStatus);
        Task<ServiceResult> GetStudentsByCollegeBCOM(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> GetStudentByRegIdBCOM(string RegId, Int32 type);
        Task<ServiceResult> SAVEBCOMStudentWeightage(BCOMStudentWeightage model);
        Task<ServiceResult> ExportExcelStudentsByCollegeBCOM(Int32 collegeId, Int32? verificationStatus);
        Task<ServiceResult> GetStudentCombinationByCCId(Int32 CollegeID, Int32 CourseId, string RegId);






    }
}
