using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ICollegeCourseMappingService
    {
        Task<ServiceResult> CreateMappingAsync(CollegeCourse model);
        Task<ServiceResult> getCoursesByCollegeId(Int32 CollegeId,Int32 universityId,Int32? Type);
        Task<ServiceResult> getMappedCoursesByCollegeId(Int32 CollegeId);
        Task<ServiceResult> getCombinationCoursesByCollegeId(Int32 CollegeId);
        Task<ServiceResult> LockUnlockCoursesByCollegeID(int collegeId, int Status, int modifiedBy);
    }
}
