using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.PGObjection;
using AdmissionPortal_API.Domain.ViewModel;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IPGObjectionService
    {
        Task<ServiceResult> CreateObjectionAsync(AddPgObjection model);
        Task<ServiceResult> GetAllObjectionsByRegId(string RegId);
        Task<ServiceResult> GetAllObjectionsByCollegeId(string CollegeId);
        Task<ServiceResult> GetObjectionById(int objectonId);
    }
}

