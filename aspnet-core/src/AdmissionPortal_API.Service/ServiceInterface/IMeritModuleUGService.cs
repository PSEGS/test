using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IMeritModuleUGService
    {
        Task<ServiceResult> GetProvisionalMeritList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);
        Task<ServiceResult> GetWaitingList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);
        Task<ServiceResult> GetCourseFeeByStudentId(int CourseId, int CollegeId, int StudentId, string Combination);
        Task<ServiceResult> SaveAdmissionSeat(AdmissionSeat model);
        Task<ServiceResult> GetAdmissionSeatStatus(string RegId, string CollegeId,string AdmissionType);
        Task<ServiceResult> GetCourseChoiceByRegId(string RegId, Int32 CourseId, Int32 CollegeId);
        Task<ServiceResult> GetFeeReceiptByRegId(string RegId, Int32 CollegeId);
        Task<ServiceResult> GetCategoryCombination();
        Task<ServiceResult> SendOTP(OTPRequestModel model);
        ServiceResult VerifyOTP(VerifyOTP model);
        Task<ServiceResult> ExportMeritExcel(int collegeId, int? courseId, int? ReservationId, int? CategoryId, string searchKeyword);
        Task<ServiceResult> CollegeCourseSubjectCount(string CollegeId, string CourseId);
        Task<ServiceResult> GetStudentFeeReceiptList(string RegId, int CollegeId);

        Task<ServiceResult> RevokeAdmissionSeat(RevokeSeat model);

        Task<ServiceResult> ExportWaitingExcel(int collegeId, int CourseId, int ReservationId, int CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, string searchKeyword);

        Task<ServiceResult> GetVacantSeatMatrixByCategory(int? CollegeId, int CourseId, int CombinationId);
        Task<ServiceResult> UpdateBookedSeatMatrix(UpdateBookedSeatMatrix model);
        Task<ServiceResult> GetVacantSeatByCollege(int CollegeId);

    }
}
