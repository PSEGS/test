using AdmissionPortal_API.Domain.ApiModel.Section;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ISectionService
    {
        Task<ServiceResult> CreateSection(AddSection model);
        Task<ServiceResult> UpdateSection(UpdateSection model);
        Task<ServiceResult> DeleteSection(int sectionId);
        Task<ServiceResult> GetAllSection(int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder);
        Task<ServiceResult> GetSectionById(int sectionId);
    }
}
