using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
   public interface IGeoService
    {
        Task<ServiceResult> GetTehsilByDistrictId(string districtId);
        Task<ServiceResult> GetBlockByDistrictId(string districtId);
        Task<ServiceResult> GetVillageByBlockId(string blockId);
    }
}
