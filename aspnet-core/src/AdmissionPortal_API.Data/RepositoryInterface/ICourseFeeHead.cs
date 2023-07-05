using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.CourseFeeHead;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
   public  interface ICourseFeeHead
    {
        Task<int>AddMapping(List<AddCourseFeeHead> addCourseFeeHead);
        Task<int>AddCourseFeeHeadFee(List<AddCourseFeeHeadFEE> addCourseFeeHead);
        Task<int>AddCourseWaveOff(CourseFeeWaveOff addCourseFeeHead);
        Task<List<CourseFeeHeadDetails>>getAllFeedetailsByHeadyId(Int32 HeadId);
        Task<List<CourseFeeHeadDetails>>getMappedCoursesById(Int32 Id);
        Task<List<FeedHeadDetailsByCourse>>getALLFeeDetailsByCourseID(Int32 universityId, Int32 collegeID, Int32 CourseID, string mode);
        Task<List<FeedHeadDetailsByCourse>>GetallFeeHeadByLoginTYpe(Int32 TypeId,Int32 CollegeType,Int32 CreatedBy,int CourseFundType);

        Task<List<CourseFeeByCollegId>> getCourseFeebyColegeId(Int32 collegeId);
        Task<GetCourseFeeWaveOff> getWaveOffDetailsById(Int32 HeadId,Int32 CollegeType,Int32 UniversityCollegeId);
        Task<int> LockUnlockCourseFeeHeadByCollegeID(int CollegeId, int Status, int modifyBy);
        Task<int> LockUnlockCourseFeeHeadByUniversityID(int UniversityId, int Status, int modifyBy);
    }
}
