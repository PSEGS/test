using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IAdminLoginRepository : IGenericRepository<AdminLoginMaster>
    {
        Task<AdminMaster> GetAsync(AdminLoginMaster entity);

        public Task<ChangePasswordResponse> UpdateAsync(ChangePassword entity, string userType, string userId);
        Task<int> ResetEmployeePassword(int Id);
        Task<ForgotLoginMaster> ForgotPassword(string email);
        Task<int> CancelStudentRegistrationByRegId(CancelStudentRegistration model);
        Task<List<StudentObjectionList>> viewStudentObjections(string RegId, string admission, Int32 college);
        Task<int> UploadNotification(UploadNotification model);
        Task<List<NotificationsMaster>> GetUploadedNotification(int status);
        Task<NotificationMaster> GetNotificationbyPath(string model);

    }
}

