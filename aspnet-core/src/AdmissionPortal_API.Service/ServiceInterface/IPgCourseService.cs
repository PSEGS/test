using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgCourseModel;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IPgCourseService
    {
        Task<ServiceResult> CreateCourseAsync(AddPgCourse model);

        Task<ServiceResult> UpdateCourse(UpdatePgCourse model);

        Task<ServiceResult> DeleteCourse(int courseid, int userid);

        Task<ServiceResult> GetAllCourse(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        Task<ServiceResult> GetCourseById(int courseId);
    }
}
