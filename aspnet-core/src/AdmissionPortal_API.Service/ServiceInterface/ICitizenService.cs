using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ViewModel;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ICitizenService
    {
        ServiceResult CreateCitizen(AddCitizen model);

        ServiceResult UpdateCitizen(UpdateCitizen model);

        ServiceResult DeleteCitizen(int CitizenID);

        ServiceResult GetAllCitizen(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        ServiceResult GetCitizenById(int CitizenID);

        ServiceResult ForgotPassword(string emailId);

        ServiceResult ResetPassword(string OTP, string password);
        ServiceResult CitizenLogin(CitizenLogin model);
    }
}

