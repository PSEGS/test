using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ApiModel.University;
using AdmissionPortal_API.Domain.ViewModel;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ICollegeService
    {
       Task<ServiceResult> CreateCollegeAsync(AddCollege model);

        Task<ServiceResult> UpdateCollege(UpdateCollege model);
        Task<ServiceResult> UpdateCollegeNew(UpdateCollegeModel model);

        Task<ServiceResult>   DeleteCollege(int CollegeId,int userid);

        Task<ServiceResult> GetAllCollege(int? university,int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);

        Task<ServiceResult> GetCollegeById(int collegeId);
        Task<ServiceResult> GetCollegeCourses(int collegeId,string CGtype);
        Task<ServiceResult> GetAllColleges();
        Task<ServiceResult> GetCollegeByDistrictId(int districtId, int collegeTypeId, int admissionId, string type,string ugpg);
        Task<ServiceResult> GetDistrictCollege(int districtId, int collegeTypeId, int admissionId, string type);
        Task<ServiceResult> ResetCollegePassword(int collegeId,int Stataus);
        Task<ServiceResult> CollegeInactiveActive(int collegeId,int Stataus,int modifiedBy);

        Task<ServiceResult> lockunlockCollegeInfo(int collegeId, int Status, int modifiedBy,string otp);
        Task<ServiceResult> generateOTP(int collegeId);
        Task<ServiceResult> UploadProspectus(UploadCollegeProspectus model);
        Task<ServiceResult> UploadCancelledCheque(UploadCancelledChequeModel model);

        //ServiceResult ForgotPassword(string emailId);

        //ServiceResult ResetPassword(string OTP, string password);
        //ServiceResult CitizenLogin(CitizenLogin model);
        Task<ServiceResult> GetDistrictCollegesByGender(int districtId, int collegeTypeId, int admissionId, int studentId, string type, string ugpg);

        Task<ServiceResult> GetCollegeByCGtype(string type);
        Task<ServiceResult> ReportsLogin(int userId,string username,string userType);
        Task<ServiceResult> UnlockStudent(UnlockStudentModel model);
        Task<ServiceResult> GetStudentDetailsByRegId(string RegId, string type, int collegeId);
        Task<ServiceResult> CancelStudentAdmissionSeat(CancelAdmissionSeat model);

        Task<ServiceResult> GetStudentDocumentDownload(string collegeId, string type);
        Task<ServiceResult> GetCollegeProspectus(string collegeId);
        Task<ServiceResult> GetCollegesIslock(int Admissiontype);

    }
}

