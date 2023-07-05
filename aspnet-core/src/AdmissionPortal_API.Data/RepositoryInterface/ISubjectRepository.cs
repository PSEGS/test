using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
   public interface ISubjectRepository: IGenericRepository<SubjectMaster>
    {
        Task<int> UpdateAsync(UpdateSubject updateSubject);
        Task<int> Delete(int subjectId,int UserId);
        Task<SubjectDetails> GetById(int subjectId);
        Task<List<SubjectDetails>> GetAllSubject(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<List<coursesubject>> GetAllSubjectByCourseId(int courseid,int universityId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<List<coursesubject>> GetSubjectByCourseId(int courseid, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<List<coursesubject>> GetSubjectByCourseAndUniversityid(int courseid, int UniversityId);
        Task<List<CourseSubjectCombinationCheck>> GetCourseSubjectCombinationCheckByUniversity(int universityId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
                
    }
    public interface ICourseSubjectCombinationCheck
    {
        Task<int> CreateUniversityCourseSubjectCombinationCheck(UnvCourseSubjectCombinationCheck model);
    }
} 
