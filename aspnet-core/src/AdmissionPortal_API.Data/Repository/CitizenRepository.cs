using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.UserTypeEnum;

namespace AdmissionPortal_API.Data.Repository
{
    public class CitizenRepository : ICitizenRepository
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public CitizenRepository(IMapper mapper, IConfiguration configuration , ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task<int> AddAsync(CitizenMaster entity)
        {
            int result = 0;

            try
            {
                var values = _mapper.Map<AddCitizen>(entity);
                var procedure = Procedure.SaveCitizen;
                values.UserPassword = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Create Citizen : ", ex.Message,ex.HResult,ex.StackTrace);
            }
            return result;
        }

        public async Task<int> DeleteAsync(int CitizenID)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeleteCitizen;
                var values = new { CitizenID = CitizenID, IsDelete = true };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Citizen : " ,ex.Message,ex.HResult,ex.StackTrace);
            }
            return result;
        }

        public async Task<List<CitizenMaster>> GetAllCitizen(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {

            List<CitizenMaster> lstCitizen = new List<CitizenMaster>();
            try
            {
                var procedure = Procedure.GetAllCitizen;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CitizenMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCitizen = _mapper.Map<List<CitizenMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Citizen : " ,ex.Message,ex.HResult,ex.StackTrace);
            }
            return lstCitizen;

        }

        public async Task<CitizenMaster> GetByIdAsync(int CitizenID)
        {
            CitizenMaster citizenMaster = new CitizenMaster();
            try
            {
                var procedure = Procedure.GetCitizenById;
                var value = new
                {
                    CitizenID = CitizenID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QuerySingleAsync<CitizenMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    citizenMaster = _mapper.Map<CitizenMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Citizen By Id : " ,ex.Message,ex.HResult,ex.StackTrace);
            }
            return citizenMaster;
        }

        public async Task<int> UpdateAsync(CitizenMaster entity)
        {
            int result = 0;
            object values;
            string procedure = string.Empty;
            try
            {
                values = _mapper.Map<UpdateCitizen>(entity);
                procedure = Procedure.UpdateCitizen;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Citizen : " ,ex.Message,ex.HResult,ex.StackTrace);
            }
            return result;
        }

        public async Task<string> ForgotPassword(string email)
        {
            string data = null;
            try
            {
                var procedure = Procedure.ForgotPassword;
                Random ran = new Random();
                string OTP = Convert.ToString(ran.Next(100001, 1000000));
                var value = new
                {
                    Email = email,
                    OTP = OTP,
                    UserType = nameof(UserType.Citizen)
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.ExecuteScalarAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                    if (obj == 1)
                    {
                        data = OTP;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Forgot Password : " ,ex.Message,ex.HResult,ex.StackTrace);

            }
            return data;
        }

        public async Task<int> ResetPassword(string OTP, string password)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.ResetPassword;
                password = Encryption.EncodeToBase64(password);
                var value = new
                {
                    OTP = OTP,
                    Password = password,
                    UserType = nameof(UserType.Citizen)
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = await con.ExecuteScalarAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Reset Password : " ,ex.Message,ex.HResult,ex.StackTrace);

            }
            return result;
        }
        public async Task<CitizenLoginMaster> GetCitizenLogin(CitizenLoginMaster entity)
        {
            CitizenLoginMaster citizenLogin = new CitizenLoginMaster(); 
            try
            {
                string procedure = Procedure.CitizenLogin;
                entity.UserPassword = Encryption.EncodeToBase64(entity.UserPassword);
                var values = new
                {
                    Email= entity.Email,
                    UserPassword = entity.UserPassword
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var result = await (connection.QuerySingleAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    citizenLogin = _mapper.Map<CitizenLoginMaster>(result);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Citizen Login : " ,ex.Message,ex.HResult,ex.StackTrace);
            }
            return citizenLogin;
        }
    }
}

