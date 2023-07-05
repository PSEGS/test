using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.CourseModel;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface ICourse
    {
        Task<int> AddAsyncCourse(AddCourse entity);
        Task<int> UpdateCourse(updateCourse entity);
        Task<CourseDetail> GetCourseById(int courseId);
        Task<List<CourseDetail>> GetAllCourse(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<int> DeleteCourse( int courseid,int userid);

        Task<List<CourseList>> GetAllUgCourse();

    }
}
