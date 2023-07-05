using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
using AdmissionPortal_API.Domain.ApiModel.Profile;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IProfileService
    {
        ServiceResult GetEmployeeProfile(int profileId, string userType);
        ServiceResult GetCitizenProfile(int profileId, string userType);
        ServiceResult UpdateEmployeeProfile(UpdateEmployeeProfile model);
        ServiceResult UpdateCitizenProfile(UpdateCitizenProfile model);
        ServiceResult UploadProfileImage(int profileId, string userType, ProfileImage model);
        ServiceResult UpdateProfileImageById(UpdateProfileImage model);

    }
}

