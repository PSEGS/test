using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Domain.ApiModel.DownloadDocument;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface ICollegeRepository : IGenericRepository<CollegeMaster>
    {
        Task<ServiceResult> AddColleges(AddCollege addCollege);
        Task<int> updateCollegeNew(UpdateCollegeModel updateCollege);
        Task<int> DeleteCollegeById(int id, int userid);
        Task<GetCollege> GetCollegesById(int id);
        Task<string> GenerateOTP(int Collegeid, string otp);

        Task<int> Updatecollege(UpdateCollege entity);
        Task<List<CollegeCoursesListing>> GetCollegeCourses(int id, string CGtype);
        Task<List<GetAllCollege>> GetAllColleges();
        Task<List<GetCollegeByDistrict>> GetCollegeByDistrictId(int districtId, int collegeTypeId, int admissionId, string type, string ugpg);
        Task<List<GetCollegeByDistrict>> GetDistrictCollege(int districtId, int collegeTypeId, int admissionId, string type);


        Task<List<GetCollege>> GetAllCollege(int? universityid, int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> ResetCollegePassword(int CollegeId, int modifiyBY);
        Task<CollegeActiveInActiveResponse> ActiveInActive(int CollegeId, int Status, int modifiyBY);
        Task<int> lockunlockCollegeInfo(int CollegeId, int Status, int modifyBy, string otp);
        Task<int> UploadProspectus(UploadCollegeProspectus model);
        Task<int> UploadCancelledCheque(UploadCancelledChequeModel model);
        Task<List<GetCollegeByDistrict>> GetDistrictCollegesByGender(int districtId, int collegeTypeId, int admissionId, int studentId, string type, string ugpg);
        Task<List<GetAllCollege>> GetCollegeByCGtype(string type);
        Task<string> ReportsLogin(int UserId, string token);
        Task<int> UnlockStudent(UnlockStudentModel model);
        Task<StudentDetailsForCancellation> GetStudentDetailsByRegId(string RegId, string type, int collegeId);
        Task<CancelAdmissionSeatModel> CancelStudentAdmissionSeat(CancelAdmissionSeat model);

        Task<List<DownloadDocument>> GetStudentDocumentDownload(string CollegeID, string type);
        Task<string> GetCollegeProspectus(string CollegeID);
        Task<List<AllCollegesISLOCK>> GetCollegesIslock(int Admissiontype);
    

    }
}

