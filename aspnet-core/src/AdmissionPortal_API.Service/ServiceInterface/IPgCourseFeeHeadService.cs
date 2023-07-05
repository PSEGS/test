using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgCourseFeeHead;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IPgCourseFeeHeadService
    {
        Task<ServiceResult> AddCourseFeeHead(List<AddPgCourseFeeHead> model);
        Task<ServiceResult> AddCourseWaveoffFee(PgCourseFeeWaveOff model);
        Task<ServiceResult> AddCourseFeeHeadWithCollegeType(List<AddPgCourseFeeHeadFee> model);
        Task<ServiceResult> GetCoursesFeeHeadByHeadTypeId(Int32 Id);
        Task<ServiceResult> GetAllFeeHead();
        Task<ServiceResult> GetAllFeeDetailsCourseID(Int32 universityId, Int32 collegeID, Int32 CourseID, string mode);
        Task<ServiceResult> GetcoursefeeBYCollegeId(Int32 Id);
        Task<ServiceResult> GetHeadByLoginType(Int32 Id, Int32 CollegeType, Int32 CreatedBy,int CourseFundType);
        Task<ServiceResult> GetFeeWaveOffDetails(Int32 HeadId, Int32 CollegeType, Int32 UniversityCollegeId);
        Task<ServiceResult> LockUnlockCourseFeeHeadByCollegeID(int collegeId, int Status, int modifiedBy);
        Task<ServiceResult> LockUnlockCourseFeeHeadByUniversityID(int UniversityId, int Status, int modifyBy);
    }
}
