using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
   public  interface IPgCollegeCourseSeatRepository
    {
        Task<int> AddAsyncSeat(PgCollegeCourseSeat entity);
        Task<int> UpdateSeat(UpdatePgCollegeCourseSeat entity);
        Task<PgCollegeCourseSeatDetails> GetSeatById(int Id);
        Task<List<PgCollegeCourseSeatDetails>> GetAllSeat(int collegeId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<int> DeleteSeat(int Id, int userid);
        Task<List<PgSeatMatrixM>> PGseatMatrixMs(int id);
        Task<List<PgGetReservationQuotaModel>> GetReservationQuota();
        Task<int> LockUnlockCourseSeatsByCollegeID(int CollegeId, int Status, int modifyBy);
    }
}
