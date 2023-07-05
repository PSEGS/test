using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Domain.ApiModel.UniversityCourse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IUniversityCourseRepository : IGenericRepository<UniversityCourse>
    {
        Task<List<universityCourse>> getCoursesByUniversityId(Int32? UniversityId, Int32? Type);
        Task<List<universityCourse>> getMappedCoursesByUniversityId(Int32 UniversityId, Int32?  Type);
        Task<List<universityCourse>> getCombinationsCoursesByUniversityId(Int32 UniversityId, Int32? Type);
    }
}