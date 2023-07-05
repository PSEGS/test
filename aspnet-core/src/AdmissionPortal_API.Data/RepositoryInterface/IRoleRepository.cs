using AdmissionPortal_API.Domain.ApiModel.Role;
using AdmissionPortal_API.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IRoleRepository : IGenericRepository<RoleMaster>
    {
        Task<List<RoleMaster>> GetAllRole(int pageNumber,int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<List<RoleNavigationMapping>> GetRoleById(int roleId);
    }

}

