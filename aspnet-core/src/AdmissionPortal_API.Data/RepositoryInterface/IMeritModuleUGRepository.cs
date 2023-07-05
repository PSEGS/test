using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IMeritModuleUGRepository
    {
        Task<List<ProvisionalList>> GetProvisionalList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);
        Task<List<WaitingList>> GetWaitingList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);


        Task<SaveAdmissionSeatModel> SaveAdmissionSeat(AdmissionSeat model);

        Task<dynamic> GetCourseFeeByStudentId(int CourseId, int CollegeId, int StudentId, string Combination);
        //Task<int> SaveAdmissionSeat(AdmissionSeat model);

        Task<MeritModule> GetAdmissionSeatStatus(string RegId, string CollegeId,string AdmissionType);
        Task<List<Combination>> GetCourseChoiceByRegId(string RegId, Int32 CourseId, Int32 CollegeId);
        Task<AdmissionSeatPaymentReciept> GetFeeReceiptByRegId(string RegId, Int32 CollegeId);
        Task<List<CategoryCombination>> GetCategoryCombination();
        Task<List<ProvisionalList>> ExportMeritExcel(int collegeId, int? courseId, int? ReservationId, int? CategoryId, string searchKeyword);

        Task<OTPModel> SendOTP(OTPRequestModel model, string studentType, string OTP);

        Task<int> VerifyOTP(VerifyOTP model, string studentType);
        Task<List<CourseSubjectCount>> CollegeCourseSubjectCount(string CollegeId, string CourseId);
        Task<List<AdmissionSeatPaymentRecieptList>> GetStudentFeeReceiptList(string RegId, int CollegeId);

        Task<RevokeAdmissionSeatModel> RevokeAdmissionSeat(RevokeSeat model);

        Task<List<WaitingList>> ExportWaitingExcel(int collegeId, int CourseId, int ReservationId, int CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, string searchKeyword);
        Task<VacantSeatForWaitingList> GetVacantSeatMatrixByCategory(int? collegeId, int courseId, int combinationId);
        Task<int> UpdateBookedSeatMatrix(UpdateBookedSeatMatrix model);
        Task<List<GetvacantSeatList>> GetVacantSeatByCollege(int CollegeId);


    }
}
