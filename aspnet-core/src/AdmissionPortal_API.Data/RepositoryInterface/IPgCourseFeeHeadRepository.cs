using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgCourseFeeHead;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
   public  interface IPgCourseFeeHeadRepository
    {
        Task<int> AddMapping(List<AddPgCourseFeeHead> addCourseFeeHead);
        Task<int> AddCourseFeeHeadFee(List<AddPgCourseFeeHeadFee> addCourseFeeHead);
        Task<int> AddCourseWaveOff(PgCourseFeeWaveOff addCourseFeeHead);
        Task<List<PgCourseFeeHeadDetails>> GetAllFeeDetailsByHeadId(Int32 HeadId);
        Task<List<PgCourseFeeHeadDetails>> GetMappedCoursesById(Int32 Id);
        Task<List<FeedHeadDetailsByPgCourse>> GetALLFeeDetailsByCourseID(Int32 universityId, Int32 collegeID, Int32 CourseID, string mode);
        Task<List<FeedHeadDetailsByPgCourse>> GetAllFeeHeadByLoginType(Int32 TypeId,Int32 CollegeType,Int32 CreatedBy,Int32 CourseFundType);

        Task<List<PgCourseFeeByCollegId>> GetCourseFeebyColegeId(Int32 collegeId);
        Task<GetPgCourseFeeWaveOff> GetWaveOffDetailsById(Int32 HeadId,Int32 CollegeType,Int32 UniversityCollegeId);
        Task<int> LockUnlockCourseFeeHeadByCollegeID(int CollegeId, int Status, int modifyBy);
        Task<int> LockUnlockCourseFeeHeadByUniversityID(int UniversityId, int Status, int modifyBy);

    }
}
