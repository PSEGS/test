using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.FeeHead;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IFeeHeadService
    {
        Task<ServiceResult> CreateFeeHeadAsync(AddFeeHead model);

        Task<ServiceResult> UpdateFeeHead(UpdateFeeHead model);

        Task<ServiceResult> DeleteFeeHead(int Id, int userid);

        Task<ServiceResult> GetAllFeeHead(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        Task<ServiceResult> GetFeeHeadById(int Id);
    }
}
