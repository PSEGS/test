using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IStateRepository:IGenericRepository<StateMaster>
    {
        Task<List<StateMaster>> GetAllAsync();
    }
}

