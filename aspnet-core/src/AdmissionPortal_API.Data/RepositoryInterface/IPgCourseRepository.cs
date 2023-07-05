using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgCourseModel;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IPgCourseRepository
    {
        Task<int> AddAsyncCourse(AddPgCourse entity);
        Task<int> UpdateCourse(UpdatePgCourse entity);
        Task<PgCourseDetail> GetCourseById(int courseId);
        Task<List<PgCourseDetail>> GetAllCourse(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<int> DeleteCourse( int courseid,int userid);

    }
}
