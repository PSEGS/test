using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface INavigationRepository:IGenericRepository<NavigationMaster>
    {
        Task<List<NavigationMaster>> GetAllNavigations(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<List<FeatureMapping>> GetNavigation(int Id, string UserType, string LoginType);
    }
}

