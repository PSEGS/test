using AdmissionPortal_API.Domain.ApiModel.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface ISectionRepository
    {
        Task<int> AddSection(AddSection model);
        Task<int> UpdateSection(UpdateSection model);
        Task<int> DeleteSectionById(int id);
        Task<GetAllSection> GetSectionById(int id);
        Task<List<GetAllSection>> GetAllSection(int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder);
    }
}
