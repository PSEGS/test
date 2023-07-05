using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ISubjectService
    {
        Task<ServiceResult> CreateSubject(SubjectMaster model);

        Task<ServiceResult> UpdateSubject(UpdateSubject model);

        Task<ServiceResult> DeleteSubject(int SubjectId, int UserId);

        Task<ServiceResult> GetAllSubject(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        Task<ServiceResult> GetSubjectById(int SubjectId);
        Task<ServiceResult> GetSubjectByCourseId(int CourseId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> GetSubjectbyCourseAndUniversityid(int CourseId, int UniversityId);
        Task<ServiceResult> GetAllSubjectByCourseId(int CourseId, int universityId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> GetCourseSubjectCombinationCheckByUniversity(int universityId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> CreateUniversityCourseSubjectCombinationCheck(UnvCourseSubjectCombinationCheck model);
    }
}
