using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.PGObjection;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IPgVerificationService
    {
        Task<ServiceResult> GetVerifiedStudentByRegIdCollege(string RegId, int CollegeId);
        Task<ServiceResult> GetStudentsByCollege(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> VerifyStudentWithSection(PgVerifyStudentWithSection model);

        Task<ServiceResult> RevokeStudentVerification(CancelStudentRegistration model);
        Task<ServiceResult> UnlockStudentVerification(CancelStudentRegistration model);

        Task<ServiceResult> GetPGStudentByRegId(string RegId, Int32 type);

        Task<ServiceResult> StudentCourseEligible(PGStudentChoiceofCourseEligible model);
        Task<ServiceResult> FinalSubmit(PGFinalVerificationModel model);
        Task<ServiceResult> ExportExcelStudentsByCollege(Int32 collegeId, Int32? verificationStatus);
    }
}
