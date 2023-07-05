using AdmissionPortal_API.Domain.ApiModel.RoleNavigation;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IRoleNavigationService
    {
        ServiceResult CreateRoleNavigation(AddRoleNavigation model);

        ServiceResult UpdateRoleNavigation(UpdateRoleNavigation model);

        ServiceResult GetAllRoleNavigation(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        ServiceResult GetRoleNavigationById(int mappingId);
    }
}

