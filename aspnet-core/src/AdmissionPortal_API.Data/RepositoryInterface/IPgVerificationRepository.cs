using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.PGObjection;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IPgVerificationRepository
    {
        Task<StudentDetails> GetVerifiedStudentByRegIdCollege(string RegId, int CollegeId);
        Task<List<BasicDetailsNew>> GetStudentsByCollege(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> VerifyStudentWithSection(PgVerifyStudentWithSection verifyStudentWithSection);

        Task<int> RevokeStudentVerification(CancelStudentRegistration model);
        Task<int> UnlockStudentVerification(CancelStudentRegistration model);

        Task<dynamic> GetPGStudentByRegId(string RegId, Int32 type);

        Task<int> StudentCourseEligible(PGStudentChoiceofCourseEligible model);
        Task<ReturnModelVerification> FinalVerification(PGFinalVerificationModel model);
        Task<List<BasicDetailsNew>> ExportExcelStudentsByCollege(Int32 collegeId, Int32? verificationStatus);
    }
}
