using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IAdminLoginService
    {
        Task<ServiceResult> AdminLogin(AdminLogin model);
        Task<ServiceResult> UpdatePassword(ChangePassword model, string userType, string userId);
        Task<ServiceResult> ResetEmployeePassword(int Id);
        Task<ServiceResult> ForgotPassword(string email);
        Task<ServiceResult> CancelStudentRegistrationByRegId(CancelStudentRegistration model);
        Task<ServiceResult> viewStudentObjections(string RegId, string admission, Int32 college);

        Task<ServiceResult> UploadNotification(UploadNotification model);

        ServiceResult GetUploadedNotification(int status);

        ServiceResult GetNotificationbyPath(string model);
    }
}

