using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
using AdmissionPortal_API.Domain.ApiModel.Profile;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public ProfileRepository(IMapper mapper, IConfiguration configuration, IHostingEnvironment HostEnvironment,ILogError logError)
        {
            _mapper = mapper;
            _configuration = configuration;
            _HostEnvironment = HostEnvironment;
            _logError = logError;
        }

        public async Task<EmployeeMaster> GetEmployeeProfile(int profileId, string userType)
        {
            EmployeeMaster employeeMaster = new EmployeeMaster();
            try
            {
                var procedure = Procedure.GetProfile;
                var value = new
                {
                    ID = profileId,
                    UserType = userType
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QuerySingleAsync<EmployeeMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    employeeMaster = _mapper.Map<EmployeeMaster>(obj);
                    
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Employee Profile :", ex.Message, ex.HResult, ex.StackTrace);

            }
            return employeeMaster;

        }

        public async Task<CitizenMaster> GetCitizenProfile(int profileId, string userType)
        {
            CitizenMaster citizenMaster = new CitizenMaster();
            try
            {
                var procedure = Procedure.GetProfile;
                var value = new
                {
                    ID = profileId,
                    UserType = userType
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QuerySingleAsync<CitizenMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    citizenMaster = _mapper.Map<CitizenMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Citizen Profile :", ex.Message, ex.HResult, ex.StackTrace);

            }
            return citizenMaster;
        }

        public async Task<int> UpdateEmployeeProfile(EmployeeMaster entity)
        {
            int result = 0;
            string procedure = string.Empty;
            object values;
            try
            {
                values = _mapper.Map<UpdateEmployeeProfile>(entity);
                procedure = Procedure.UpdateProfile;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = await connection.ExecuteScalarAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Employee Profile : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UpdateCitizenProfile(CitizenMaster entity)
        {
            int result = 0;
            string procedure = string.Empty;
            object values;
            try
            {
                values = _mapper.Map<UpdateCitizenProfile>(entity);
                procedure = Procedure.UpdateProfile;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = await connection.ExecuteScalarAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Citizen Profile : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

       

        public async Task<int> UploadProfileImage(int profileId, string userType, ProfileImage model)
        {
            int result = 0;
            try
            {
                var value = new { Id = profileId, UserType= userType, ImageReference = model.ImageReference };
                var procedure = Procedure.UploadProfileImage;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = await connection.ExecuteScalarAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Upload Profile Image", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }


        public async Task<int> UpdateProfileImageById(UpdateProfileImage model)
        {
            int result = 0;
            try
            {
                var value = new { Id = model.Id, ImageReference = model.ImageReference };
                var procedure = Procedure.UploadProfileImageById;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = await connection.ExecuteScalarAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Upload Profile Image By Id", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}

