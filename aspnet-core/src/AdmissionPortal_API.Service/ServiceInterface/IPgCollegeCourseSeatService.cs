using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public  interface IPgCollegeCourseSeatService
    {
        Task<ServiceResult> CreateSeatAsync(PgCollegeCourseSeat model);

        Task<ServiceResult> UpdateSeat(UpdatePgCollegeCourseSeat model);

        Task<ServiceResult> DeleteSeat(int ID, int userid);

        Task<ServiceResult> GetAllSeat(int CollegeId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        Task<ServiceResult> GetSeatById(int courseId);
        Task<ServiceResult> GetPGSeatMatrixById(int Collegeid);
        Task<ServiceResult> GetReservationQuota();
        Task<ServiceResult> LockUnlockCourseSeatsByCollegeID(int collegeId, int Status, int modifiedBy);
    }
}
