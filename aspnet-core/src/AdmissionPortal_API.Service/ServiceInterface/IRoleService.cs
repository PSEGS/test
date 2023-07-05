using AdmissionPortal_API.Domain.ApiModel.Role;
using AdmissionPortal_API.Domain.ViewModel;

namespace AdmissionPortal_API.Service.ServiceInterface
{ 
    public interface IRoleService
    {
        ServiceResult CreateRole(AddRole model);

        ServiceResult UpdateRole(UpdateRole model);

        ServiceResult DeleteRole(int roleId);

        ServiceResult GetAllRole(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        ServiceResult GetRoleById(int roleId);
    }
}

