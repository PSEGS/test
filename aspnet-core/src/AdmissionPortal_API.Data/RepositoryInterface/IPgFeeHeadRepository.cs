using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgFeeHead;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IPgFeeHeadRepository
    {
        Task<int> AddAsyncFeeHead(AddPgFeeHead entity);
        Task<int> UpdateFeeHead(UpdatePgFeeHead entity);
        Task<PgFeeHeadDetails> GetFeeHeadById(int Id);
        Task<List<PgFeeHeadDetails>> GetAllFeeHead(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<int> DeleteFeeHead(int Id, int userid);
    }
}
