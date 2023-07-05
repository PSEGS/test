using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IMeritModulePGService
    {
        Task<ServiceResult> GetProvisionalMeritList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);
        Task<ServiceResult> GetWaitingList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<ServiceResult> GetCourseFeeByStudentId(int CourseId, int CollegeId, int StudentId);
        Task<ServiceResult> SaveAdmissionSeat(AdmissionSeat model);
        Task<ServiceResult> GetAdmissionSeatStatus(string RegId, string CollegeId,string AdmissionType);
        Task<ServiceResult> GetFeeReceiptByRegId(string RegId, Int32 CollegeId);
        Task<ServiceResult> ExportMeritExcel(int collegeId, int? courseId, int? ReservationId, int? CategoryId,string searchKeyword);

        Task<ServiceResult> SendOTP(OTPRequestModel model);
        ServiceResult VerifyOTP(VerifyOTP model);
        Task<ServiceResult> GetStudentFeeReceiptList(string RegId, int CollegeId);

        Task<ServiceResult> RevokeAdmissionSeat(RevokeSeat model);
        Task<ServiceResult> GetVacantSeatByCollege(int CollegeId);
        Task<ServiceResult> ExportWaitingExcel(int collegeId, int CourseId, int ReservationId, int CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, string searchKeyword);


    }
}
