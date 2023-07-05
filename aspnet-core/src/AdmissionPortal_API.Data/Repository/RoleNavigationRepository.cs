using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.RoleNavigation;
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
    public  class RoleNavigationRepository: IRoleNavigationRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public RoleNavigationRepository(IMapper mapper, IConfiguration configuration,ILogError logError)
        {
            _logError = logError;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Create All Role Navigation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(RoleNavigationMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.SaveRoleNavigation;
                var values = _mapper.Map<AddRoleNavigation>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create Role Navigation : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Delete Role Navigation
        /// </summary>
        /// <param name="mappingId"></param>
        /// <returns></returns>

        /// <summary>
        /// Get Role Navigation
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public async Task<List<RoleNavigationMaster>> GetAllRoleNavigation(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<RoleNavigationMaster> lstRoleNavigation = new List<RoleNavigationMaster>();
            try
            {
                var procedure = Procedure.GetAllRoleNavigation;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync<RoleNavigationMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstRoleNavigation = _mapper.Map<List<RoleNavigationMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All RoleNavigation : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstRoleNavigation;
        }

        /// <summary>
        /// Get Role Navigation By Id
        /// </summary>
        /// <param name="mappingId"></param>
        /// <returns></returns>
        public async Task<RoleNavigationMaster> GetByIdAsync(int mappingId)
        {
            RoleNavigationMaster RoleNavigationMaster = new RoleNavigationMaster();
            try
            {
                var procedure = Procedure.GetRoleNavigationById;
                var values = new { MappingId = mappingId };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QuerySingleAsync<RoleNavigationMaster>(procedure, values, commandType: CommandType.StoredProcedure);
                    RoleNavigationMaster = _mapper.Map<RoleNavigationMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get RoleNavigation By Id : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return RoleNavigationMaster;
        }

        /// <summary>
        /// Update Role Navigation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(RoleNavigationMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UpdateRoleNavigation;
                var values = _mapper.Map<UpdateRoleNavigation>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Role Navigation : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }


        public Task<int> DeleteAsync(int mappingId)
        {
            throw new NotImplementedException();
        }

       
    }
}

