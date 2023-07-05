using AdmissionPortal_API.Domain.ApiModel.EmployeeRoleMapping;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IEmployeeRoleMappingService
    {
        ServiceResult CreateEmployeeRoleMapping(AddEmployeeRoleMapping model);

        ServiceResult UpdateEmployeeRoleMapping(UpdateEmployeeRoleMapping model);

        ServiceResult GetAllEmployeeRoleMapping(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        ServiceResult GetEmployeeRoleMappingById(int mappingId);
    }
}

