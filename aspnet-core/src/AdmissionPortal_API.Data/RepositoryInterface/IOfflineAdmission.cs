using AdmissionPortal_API.Domain.ApiModel.OfflineAdmisson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IOfflineAdmission
    {
        Task<int> AddAdmission(OfflineAdmissionModel entity);        
    }
}
