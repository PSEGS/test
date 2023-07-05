using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.UserTypeEnum;

namespace AdmissionPortal_API.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public EmployeeRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment HostEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _configuration = configuration;
            _HostEnvironment = HostEnvironment;
        }
        public async Task<int> AddAsync(EmployeeMaster entity)
        {
            int result = 0;

            try
            {
                var values = _mapper.Map<AddEmployee>(entity);
                values.UserPassword =Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                var procedure = Procedure.SaveEmployee;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = await connection.ExecuteScalarAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Create Employee : " , ex.Message,ex.HResult,ex.StackTrace);
            }
            return result;
        }

        public async Task<int> DeleteAsync(int EmployeeID)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeleteEmployee;
                var values = new { EmployeeID = EmployeeID, IsDelete = true };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = await connection.ExecuteScalarAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Employee : " , ex.Message,ex.HResult,ex.StackTrace);
            }
            return result;
        }

        public async Task<List<EmployeeMaster>> GetAllEmployee(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<EmployeeMaster> lstEmployee = new List<EmployeeMaster>();
            try
            {
                var procedure = Procedure.GetAllEmployee;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<EmployeeMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstEmployee = _mapper.Map<List<EmployeeMaster>>(obj);
                }        
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Employee : " , ex.Message,ex.HResult,ex.StackTrace);
            }
            return lstEmployee;
        }

        public async Task<EmployeeMaster> GetEmployeeById(int EmployeeID)
        {
            EmployeeMaster employeeMaster = new EmployeeMaster();
            try
            {
                var procedure = Procedure.GetEmployeeById;
                var value = new
                {
                    EmployeeID = EmployeeID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QuerySingleAsync<EmployeeMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    employeeMaster = _mapper.Map<EmployeeMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Employee By Id:" , ex.Message,ex.HResult,ex.StackTrace);

            }
            return employeeMaster;
        }

        public async Task<EmployeeLoginMaster> GetEmployeeLogin(EmployeeLoginMaster entity)
        {
            EmployeeLoginMaster employeeLoginMaster = new EmployeeLoginMaster();
            try
            {
                string procedure = Procedure.EmployeeLogin;
                entity.UserPassword = Encryption.EncodeToBase64(entity.UserPassword);
                var values = new
                {
                    Email = entity.Email,
                    UserPassword = entity.UserPassword
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var result = await (connection.QuerySingleAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    employeeLoginMaster = _mapper.Map<EmployeeLoginMaster>(result);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Employee Login: " , ex.Message,ex.HResult,ex.StackTrace);
            }
            return employeeLoginMaster;
        }

        public async Task<int> UpdateAsync(EmployeeMaster entity)
        {
            int result = 0;
            string procedure = string.Empty;
            object values;
            try
            {
                values = _mapper.Map<UpdateEmployee>(entity);
                procedure = Procedure.UpdateEmployee;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = await connection.ExecuteScalarAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Employee : " , ex.Message,ex.HResult,ex.StackTrace);
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
                    UserType = nameof(UserType.Employee)
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
                _logError.WriteTextToFile("Forgot Password:" , ex.Message,ex.HResult,ex.StackTrace);

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
                    UserType = nameof(UserType.Employee)
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = await con.ExecuteScalarAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Reset Password:" , ex.Message,ex.HResult,ex.StackTrace);

            }
            return result;
        }

        public async Task<List<EmployementTypes>> GetEmployementType()
        {
            List<EmployementTypes> lstEmployement = new List<EmployementTypes>();
            try
            {
                var procedure = Procedure.EmployementType;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<EmployementTypes>(procedure, commandType: CommandType.StoredProcedure);
                    lstEmployement = _mapper.Map<List<EmployementTypes>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Employement Type : " , ex.Message,ex.HResult,ex.StackTrace);
            }
            return lstEmployement;
        }



        Task<EmployeeMaster> IGenericRepository<EmployeeMaster>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

