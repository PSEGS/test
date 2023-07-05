using AdmissionPortal_API.Domain.ApiModel.UniversityCourse;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IUniversityCourseService
    {
        Task<ServiceResult> getCoursesByUniversityId(Int32? universityId,Int32? Type );
        Task<ServiceResult> getMappedCoursesByUniversityId(Int32 universityId,Int32? Type);
        Task<ServiceResult> getCombinationsCoursesByUniversityId(Int32 universityId, Int32? Type);
        Task<ServiceResult> AddAsync(UniversityCourse model);
    }
}
