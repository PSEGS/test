using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.EmployeeRoleMapping;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
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
    public class EmployeeRoleMappingRepository : IEmployeeRoleMappingRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        public EmployeeRoleMappingRepository(IConfiguration configuration, IMapper mapper,ILogError logError)
        {
            _logError = logError;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(EmployeeRoleMappingMaster entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map <AddEmployeeRoleMapping>(entity);
                var procedure = Procedure.SaveEmployeeRoleMapping;
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await con.ExecuteScalarAsync(procedure,values,commandType : CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create Employee Role Mapping : ", ex.Message,ex.HResult,ex.StackTrace);
            }
            return result;   
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get ALL
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public async Task<List<EmployeeRoleMappingMaster>> GetAllEmployeeRoleMapping(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<EmployeeRoleMappingMaster> lstEmployeeRoleMappings= new List<EmployeeRoleMappingMaster>();
            try
            {
                var procedure = Procedure.GetAllEmployeeRoleMapping;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<EmployeeRoleMappingMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstEmployeeRoleMappings = _mapper.Map<List<EmployeeRoleMappingMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Employee Role Mapping : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstEmployeeRoleMappings;
        }

        /// <summary>
        /// Get BY Id
        /// </summary>
        /// <param name="mappingId"></param>
        /// <returns></returns>
        public async Task<EmployeeRoleMappingMaster> GetByIdAsync(int mappingId)
        {
            EmployeeRoleMappingMaster employeeRoleMapping = new EmployeeRoleMappingMaster();
            try
            {
                var procedure = Procedure.GetEmployeeRoleMappingById;
                var values = new { MappingId = mappingId };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QuerySingleAsync<EmployeeRoleMappingMaster>(procedure, values, commandType: CommandType.StoredProcedure);
                    employeeRoleMapping = _mapper.Map<EmployeeRoleMappingMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Employee Role Mapping By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return employeeRoleMapping;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EmployeeRoleMappingMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UpdateEmployeeRoleMapping;
                var values = _mapper.Map<UpdateEmployeeRoleMapping>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Employee Role Mapping : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}

