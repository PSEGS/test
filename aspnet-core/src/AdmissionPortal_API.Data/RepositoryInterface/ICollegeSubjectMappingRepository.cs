using AdmissionPortal_API.Domain.ApiModel.CollegeSubjectMapping;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface ICollegeSubjectMappingRepository
    {
        Task<int> AddMapping(CollegeCourseSectionSubject addSubject);
        Task<List<Subject>> getSubjectByCollegeId(Int32 CollegeId);
        Task<List<SubjectListing>> getCombinationByCollegeId(Int32 CollegeId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);
        Task<List<Common>> getCombinationByCollegeCourse(Int32 CollegeId, Int32 courseId);
        Task<int> DeleteAsync(Int32 combinationId);
        Task<int> UpdateAsync(CollegeCourseSectionSubject addSubject);
        Task<int> LockUnlockCollegeSubjectMappingByCollegeID(int CollegeId, int Status, int modifyBy);
    }
}
