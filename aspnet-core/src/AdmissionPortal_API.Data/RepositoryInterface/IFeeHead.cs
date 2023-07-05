using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.FeeHead;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
  public   interface IFeeHead
    {
        Task<int> AddAsyncFeeHead(AddFeeHead entity);
        Task<int> UpdateFeeHead(UpdateFeeHead entity);
        Task<FeeHeadDetails> GetFeeHeadById(int Id);
        Task<List<FeeHeadDetails>> GetAllFeeHead(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);
        Task<int> DeleteFeeHead(int Id, int userid);
    }
}
