using AdmissionPortal_API.Domain.ApiModel.Navigation;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface INavigationService
    {
        ServiceResult CreateNavigation(AddNavigation model);

        ServiceResult GetAllNavigations(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        ServiceResult UpdateNavigation(UpdateNavigation model);
        ServiceResult DeleteNavigation(int navigationId);
        ServiceResult GetNavigationById(int navigationId);
        ServiceResult GetNavigation(int Id,string UserType,string LoginType);
    }
}

