using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
   public  interface ICollegeCourseSeatService
    {
        Task<ServiceResult> CreateSeatAsync(CollegeCourseSeat model);

        Task<ServiceResult> UpdateSeat(UpdateCollegeCourseSeat model);

        Task<ServiceResult> DeleteSeat(int ID, int userid);

        Task<ServiceResult> GetAllSeat(int CollegeId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        Task<ServiceResult> GetSeatById(int courseId);
        Task<ServiceResult> GetSeatMatrixById(int Collegeid);
        Task<ServiceResult> GetPGSeatMatrixById(int Collegeid);
        Task<ServiceResult> GetReservationQuota();
        Task<ServiceResult> LockUnlockCourseSeatsByCollegeID(int collegeId, int Status, int modifiedBy);
        Task<ServiceResult> lockUnlockSeatMatrixByCollegeID(int collegeId, int Status, int modifiedBy);
        Task<ServiceResult> PGLockUnlockSeatMatrixByCollegeID(int collegeId, int Status, int modifiedBy);
        
    }
}
