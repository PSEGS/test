using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
  public   interface ILookUpApi
    {
        Task<ServiceResult> GetAllCollegeType();
        Task<ServiceResult> GetAllCollegeMode();
        Task<ServiceResult> GetAllCourseType();
        Task<ServiceResult> testconnection();
        
        Task<ServiceResult> GetLookupType(string type,string level);

        Task<ServiceResult> GetReservationCategorys(string RegId);
        Task<ServiceResult> GetOfflineAdmissionReservationCategorys();
        Task<ServiceResult> GetReservationCategorysPG(string RegId);
    }
}
