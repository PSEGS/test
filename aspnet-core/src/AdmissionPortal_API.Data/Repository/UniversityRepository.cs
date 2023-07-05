using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.University;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public UniversityRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task<int> AddAsync(UniversityMaster entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<AddUniversity>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.SaveUniversity;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else if (response.Response == "2")
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> DeleteUniversityById(int universityId, int userId)
        {
            int result = 0;
            try
            {

                var procedure = Procedure.DeleteUniversityById;

                var values = new { University_Id = universityId, UserID = userId };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete university : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;

        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();

        }


        public async Task<UniversityMaster> GetUniversityById(int id)
        {
            UniversityMaster master = new UniversityMaster();
            try
            {
                var procedure = Procedure.GetUniversityById;
                var value = new
                {
                    University_Id = id
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<UniversityMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    //var obj = await con.QuerySingleAsync<UniversityMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    master = _mapper.Map<UniversityMaster>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get University By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return master;

        }
        public Task<UniversityMaster> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }



        public Task<int> UpdateAsync(UniversityMaster entity)
        {
            throw new NotImplementedException();
        }
        public async Task<int> UpdateUniversityAsync(updateuniversityMaster entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<updateuniversityMaster>(entity);
                var procedure = Procedure.UniversityUpdateByID;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<List<GetUniversity>> GetAllUniversities(int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder, Boolean onBoard)
        {
            List<GetUniversity> lstUniversity = new List<GetUniversity>();
            try
            {
                var procedure = Procedure.GetUniversityById;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder, OnBoard = onBoard };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GetUniversity>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstUniversity = _mapper.Map<List<GetUniversity>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstUniversity;
        }
        //Task<List<GetAllUniversity>> GetAllUniversity()
        public async Task<List<GetAllUniversity>> GetAllUniversity()
        {
            List<GetAllUniversity> lstUniversity = new List<GetAllUniversity>();
            try
            {
                var procedure = Procedure.GetAllUniversity;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GetAllUniversity>(procedure, commandType: CommandType.StoredProcedure);
                    lstUniversity = _mapper.Map<List<GetAllUniversity>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstUniversity;
        }
        public async Task<int> DelteteUniversity(int universityId, int userid)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.DeleteUniversityById;
                var values = new { University_Id = universityId, UserID = userid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<GetAllUniversity>> GetAllUniversityPG()
        {
            List<GetAllUniversity> lstUniversity = new List<GetAllUniversity>();
            try
            {
                var procedure = Procedure.GetAllUniversityPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GetAllUniversity>(procedure, commandType: CommandType.StoredProcedure);
                    lstUniversity = _mapper.Map<List<GetAllUniversity>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstUniversity;
        }
    }
}
