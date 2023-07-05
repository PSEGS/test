using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.Objection;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IVerificationRepository
    {
        Task<StudentDetails> GetVerifiedStudentByRegIdCollege(string RegId, int CollegeId);
        Task<List<BasicDetailsNew>> GetStudentsByCollege(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> VerifyStudentWithSection(VerifyStudentWithSection verifyStudentWithSection);
        Task<dynamic> GetStudentByRegId(string RegId, Int32 type);
        Task<int> RevokeStudentVerification(CancelStudentRegistration model);
        Task<int> StudentCourseEligible(StudentChoiceofCourseEligible model);
        Task<ReturnModelVerification> FinalVerification(FinalVerificationModel model);
        Task<List<BasicDetailsNew>> ExportExcelStudentsByCollege(Int32 collegeId, Int32? verificationStatus);
        Task<List<BasicDetailsNew>> GetStudentsByCollegeBCOM(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        Task<dynamic> GetStudentByRegIdBCOM(string RegId, Int32 type);
        Task<int> SAVEBCOMStudentWeightage(BCOMStudentWeightage model);
        Task<List<BasicDetailsNew>> ExportExcelStudentsByCollegeBCOM(Int32 collegeId, Int32? verificationStatus);
        Task<List<StudentCombinationDetails>> StudentCombinationDetails(Int32 CollegeID, Int32 CourseId, string RegId);

    }
}
