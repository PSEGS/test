using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
   public  interface ICollegeCourseSeatRepository
    {
        Task<int> AddAsyncSeat(CollegeCourseSeat entity);
        Task<int> UpdateSeat(UpdateCollegeCourseSeat entity);
        Task<CollegeCourseSeatDetails> GetSeatById(int Id);
        Task<List<CollegeCourseSeatDetails>> GetAllSeat(int collegeId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<int> DeleteSeat(int Id, int userid);
        Task<List<SeatMatrixM>> seatMatrixMs(int id);
        Task<List<SeatMatrixM>> PGseatMatrixMs(int id);
        Task<List<GetReservationQuotaModel>> GetReservationQuota();
        Task<int> LockUnlockCourseSeatsByCollegeID(int CollegeId, int Status, int modifyBy);
        Task<int> lockUnlockSeatMatrixByCollegeID(int CollegeId, int Status, int modifyBy);
        Task<int> PGlockUnlockSeatMatrixByCollegeID(int CollegeId, int Status, int modifyBy);
    }
}
