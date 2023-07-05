using AdmissionPortal_API.Domain.ApiModel.CollegeGroup;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
   public  interface ICollegeGroup
    {
        Task<int> AddSubjectGroup(collegegroup entity);
        Task<GroupDetails> GetGroupDetails(int collegeId,int courseID);
        Task<List<GroupsList>> GetAllGroup(int courseId, int collegeId, int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<List<groupsubjectDetails>> GetGroupDetailsByGroupId(int collegeId,int groupid, int CourseId);
        Task<groupsubjectDetailsForEdit> GetGroupDetailsByGroupIdEdit(int collegeId, int groupid, int CourseId);
        Task<List<GroupsListCombinedSubjects>> GetAllGroupCombined(int courseId, int collegeId, Int32 studentID);
        Task<int> lockUnlockGroupSubjectByCollegeID(Int32 collegeID, Int32 status, Int32 userid);
        Task<ServiceResult> DeleteSubjectGroup(Int32 collegeID, Int32 courseid, Int32 groupid);

    }
}
