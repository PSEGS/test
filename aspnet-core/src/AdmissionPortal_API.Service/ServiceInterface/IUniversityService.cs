using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.University;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IUniversityService
    {
        Task<ServiceResult> CreateUniversity(AddUniversity model);

        Task<ServiceResult> UpdateUniversity(UpdateUniversity model);

        Task<ServiceResult> DeleteUniversity(int UniversityId, int UserId);

        Task<ServiceResult> GetAllUniversity(int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder, Boolean onBoard);

        Task<ServiceResult> GetUniversityById(int UniversityId);
        Task<ServiceResult> GetAllUniversities();
        Task<ServiceResult> GetAllUniversitiesPG();

        //ServiceResult ForgotPassword(string emailId);

        //ServiceResult ResetPassword(string OTP, string password);
        //ServiceResult CitizenLogin(CitizenLogin model);
    }
}

