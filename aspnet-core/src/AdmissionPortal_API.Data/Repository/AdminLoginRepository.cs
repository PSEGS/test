using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class AdminLoginRepository : IAdminLoginRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public AdminLoginRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }
        public Task<int> AddAsync(AdminLoginMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(AdminLoginMaster entity)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<AdminMaster> GetAsync(AdminLoginMaster entity)
        {
            AdminMaster adminMaster = new AdminMaster();
            try
            {
                string procedure = Procedure.AdminLogin;
                entity.UserPassword = Encryption.EncodeToBase64(entity.UserPassword);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await (connection.QuerySingleAsync<AdminMaster>(procedure, entity, commandType: CommandType.StoredProcedure));
                    adminMaster = _mapper.Map<AdminMaster>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("GetAsync Admin Login : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return adminMaster;
        }

        public Task<AdminLoginMaster> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ChangePasswordResponse> UpdateAsync(ChangePassword entity, string userType, string userId)
        {
            ChangePasswordResponse result =new ChangePasswordResponse();
            string procedure = string.Empty;
            object values;
            try
            {
                entity.OldPassword = Encryption.EncodeToBase64(entity.OldPassword);
                entity.NewPassword = Encryption.EncodeToBase64(entity.NewPassword);
                values = new
                {
                    Id = userId,
                    OldPassword = entity.OldPassword,
                    NewPassword = entity.NewPassword,
                    UserType = userType,
                    ModifiedBy = userId
                };
                procedure = Procedure.ChangePassword;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await (connection.QuerySingleAsync<ChangePasswordResponse>(procedure, values, commandType: CommandType.StoredProcedure));
                    result = _mapper.Map<ChangePasswordResponse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Change Password : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> ResetEmployeePassword(int Id)
        {
            int result = 0;
            try
            {
                var values = new { Id = Id };
                var procedure = Procedure.ResetEmployeePassword;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Reset Password : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<ForgotLoginMaster> ForgotPassword(string email)
        {
            ForgotLoginMaster detail = new ForgotLoginMaster();
            try
            {
                var procedure = Procedure.ForgotEmployeePassword;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    ForgotPassword forgot = new ForgotPassword();
                    forgot.Email = email;
                    var obj = await (connection.QuerySingleAsync<ForgotLoginMaster>(procedure, forgot, commandType: CommandType.StoredProcedure));
                    detail = _mapper.Map<ForgotLoginMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("ForgotPassword : ", "emp mail:" + email + ", error: " + ex.Message, ex.HResult, ex.StackTrace);
                detail.response = "0";
            }
            return detail;
        }

        public async Task<int> CancelStudentRegistrationByRegId(CancelStudentRegistration model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.CancelStudentRegistrationByRegId;
                var value = new
                {
                    RegId = model.RegId,
                    RegType = model.RegType,
                    Remarks = model.Remarks,
                    UserId = model.UserId
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Cancel Student Registration By Id: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<StudentObjectionList>> viewStudentObjections(string RegId, string admission, Int32 college)
        {
            List<StudentObjectionList> result = new List<StudentObjectionList>();

            try
            {
                var procedure = Procedure.ViewStudentObjections;
                var value = new
                {
                    RegId = RegId,
                    RegType = admission,
                    college = college
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<StudentObjectionList>(procedure, value, commandType: CommandType.StoredProcedure);
                    result = _mapper.Map<List<StudentObjectionList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("view Student objections: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UploadNotification(UploadNotification model)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UploadNotification;
                var value = new
                {
                    UserId = model.UserId,
                    Description = model.Description,
                    NotificationReference = model.NotificationReference
                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = await connection.ExecuteScalarAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Upload Notification", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<NotificationsMaster>> GetUploadedNotification(int status)
        {
            List<NotificationsMaster> master = new List<NotificationsMaster>();
            try
            {
                var value = new
                {
                    UserId = 0,
                    Status = status
                };

                var procedure = Procedure.GetDepartmentNotifications;
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await con.QueryAsync<NotificationsMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    master = _mapper.Map<List<NotificationsMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Uploaded Notification :", ex.Message, ex.HResult, ex.StackTrace);

            }
            return master;
        }
        
        public async Task<NotificationMaster> GetNotificationbyPath(string model)
        {
            NotificationMaster master = new NotificationMaster();
            try
            {
                var value = new
                {
                    NotificationReference = model
                };

                var procedure = Procedure.GetNotificationDetail;
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    master = await con.QuerySingleAsync<NotificationMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    //master = _mapper.Map<NotificationMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Notification by Path :", ex.Message, ex.HResult, ex.StackTrace);

            }
            return master;
        }
    }
}

