using AdmissionPortal_API.Domain.ApiModel.CollegeGroup;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
   public  interface ICollegeGroupService
    {
        Task<ServiceResult> CreateCollegeGroup(collegegroup model);

        Task<ServiceResult> GetGroupDetails(int collegeId, int courseID);
        Task<ServiceResult> GetAllGroup(int courseId, int collegeId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);
   
        Task<ServiceResult> GetGroupDetailsByGroupId(int collegeId, int groupID,int CourseId);
        Task<ServiceResult> GetGroupDetailsByGroupIdEdit(int collegeId, int groupID, int CourseId);
        Task<ServiceResult> GetAllGroupCombined(int courseId, int collegeId,Int32 studentID);
        Task<ServiceResult> lockUnlockGroupSubjectByCollegeID(Int32 collegeID,Int32 status,Int32 userid);
        Task<ServiceResult> DeleteSubjectGroup(Int32 collegeID, Int32 courseid, Int32 groupid);


    }
}
