using AdmissionPortal_API.Domain.ApiModel.CollegeSubjectMapping;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ICollegeCourseSubjectMappingService
    {
        Task<ServiceResult> CreateMappingAsync(CollegeCourseSectionSubject model);
        Task<ServiceResult> getSubjectByCollegeId(Int32 CollegeId);
        Task<ServiceResult> getCombinationByCollegeId(Int32 CollegeId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);
        Task<ServiceResult> getCombinationByCollegeCourse(Int32 CollegeId, Int32 courseId);
        Task<ServiceResult> DeleteCombination(Int32 combinationId);
        Task<ServiceResult> UpdateCombination(CollegeCourseSectionSubject model);
        Task<ServiceResult> LockUnlockCollegeSubjectMappingByCollegeID(int collegeId, int Status, int modifiedBy);
    }
}
