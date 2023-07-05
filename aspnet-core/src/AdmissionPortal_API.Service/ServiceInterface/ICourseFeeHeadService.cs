using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.CourseFeeHead;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ICourseFeeHeadService
    {
        Task<ServiceResult> AddCourseFeeHead(List<AddCourseFeeHead> model);
        Task<ServiceResult> AddCourseWaveoffFee(CourseFeeWaveOff model);
        Task<ServiceResult> AddCourseFeeHeadWithCollegeType(List<AddCourseFeeHeadFEE> model);
        Task<ServiceResult> getCoursesFeeHeadByHeadTypeId(Int32 Id);
        Task<ServiceResult> getAllFeeHead();
        Task<ServiceResult> getAllFeeDetailsCourseID(Int32 universityId,Int32 collegeID,Int32 CourseID,string mode);
        Task<ServiceResult> getcoursefeeBYCollegeId(Int32 Id);
        Task<ServiceResult> FeeheadbyLoginType(Int32 Id,Int32 CollegeType,Int32 CreatedBy,Int32 CourseFundType);
        Task<ServiceResult> getFeeWaveOffDetails(Int32 HeadId,Int32 CollegeType,Int32 UniversityCollegeId);
        Task<ServiceResult> LockUnlockCourseFeeHeadByCollegeID(int collegeId, int Status, int modifiedBy);
        Task<ServiceResult> LockUnlockCourseFeeHeadByUniversityID(int UniversityId, int Status, int modifyBy);
    }
}
