using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IUniversityRepository : IGenericRepository<UniversityMaster>
    {
        Task<List<GetUniversity>> GetAllUniversities(int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder, Boolean onBoard);
        Task<int> DeleteUniversityById(int universityId, int userid);

        Task<UniversityMaster> GetUniversityById(int id);

        Task<int> UpdateUniversityAsync(updateuniversityMaster entity);

        Task<List<GetAllUniversity>> GetAllUniversity();

        Task<List<GetAllUniversity>> GetAllUniversityPG();

    }
}

