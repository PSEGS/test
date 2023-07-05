using AdmissionPortal_API.Domain.ApiModel.OfflineAdmisson;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
  public   interface IOfflineAdmission
    {
        Task<ServiceResult> AddOfflineAdmisson(OfflineAdmissionModel model);
    }
}
