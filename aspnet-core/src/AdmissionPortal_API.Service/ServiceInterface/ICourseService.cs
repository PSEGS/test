using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.CourseModel;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ICourseService
    {
        Task<ServiceResult> CreateCourseAsync(AddCourse model);

        Task<ServiceResult> UpdateCourse(updateCourse model);

        Task<ServiceResult> DeleteCourse(int courseid, int userid);

        Task<ServiceResult> GetAllCourse(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        Task<ServiceResult> GetCourseById(int courseId);
        Task<ServiceResult> GetAllUgCourse();
    }
}
