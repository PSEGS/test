using AdmissionPortal_API.Domain.ApiModel.PGCollegeCourseMapping;
using AdmissionPortal_API.Domain.ApiModel.UniversityPgCourse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IUniversityPgCourseRepository : IGenericRepository<UniversityPgCourse>
    {
        Task<List<PGUniversityCourse>> GetCoursesByUniversityId(Int32? UniversityId, Int32? Type);
        Task<List<PGUniversityCourse>> GetMappedCoursesByUniversityId(Int32 UniversityId, Int32?  Type);
    }
}