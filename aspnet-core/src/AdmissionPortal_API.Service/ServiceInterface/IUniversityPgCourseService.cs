using AdmissionPortal_API.Domain.ApiModel.UniversityPgCourse;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IUniversityPgCourseService
    {
        Task<ServiceResult> AddAsync(UniversityPgCourse model);
        Task<ServiceResult> GetCoursesByUniversityId(Int32? universityId,Int32? Type );
        Task<ServiceResult> GetMappedCoursesByUniversityId(Int32 universityId,Int32? Type);
    }
}
