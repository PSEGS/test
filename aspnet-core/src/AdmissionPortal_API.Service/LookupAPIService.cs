using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service
{
    public class LookupAPIService : ILookUpApi
    {
        public Task<ServiceResult> GetAllCollegeMode()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetAllCollegeType()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetAllCourseType()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetLookupType(string type, string level)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetReservationCategorys(string RegId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetReservationCategorysPG(string RegId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetOfflineAdmissionReservationCategorys()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> testconnection()
        {
            throw new NotImplementedException();
        }
    }
}
