using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Navigation;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class NavigationRepository : INavigationRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public NavigationRepository(IMapper mapper, IConfiguration configuration, ILogError logError)
        {
            _logError = logError;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<int> AddAsync(NavigationMaster entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<AddNavigation>(entity);
                var procedure = Procedure.SaveNavigation;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Save Navigation : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="navigationId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int navigationId)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.DeleteNavigation;
                var values = new { NavigationId = navigationId };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Navigation : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public async Task<List<NavigationMaster>> GetAllNavigations(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {

            List<NavigationMaster> lstNavigation = new List<NavigationMaster>();
            try
            {
                var procedure = Procedure.GetAllNavigations;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<NavigationMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstNavigation = _mapper.Map<List<NavigationMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Navigation : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstNavigation;
        }

        public async Task<NavigationMaster> GetByIdAsync(int navigationId)
        {
            NavigationMaster navigationMaster = new NavigationMaster();
            try
            {
                var procedure = Procedure.GetNavigationById;
                var values = new { NavigationId = navigationId };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QuerySingleAsync<NavigationMaster>(procedure, values, commandType: CommandType.StoredProcedure);
                    navigationMaster = _mapper.Map<NavigationMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Navigation By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return navigationMaster;
        }

        public async Task<int> UpdateAsync(NavigationMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UpdateNavigation;
                var values = _mapper.Map<UpdateNavigation>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Navigation  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// GetNavigation
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<List<FeatureMapping>> GetNavigation(int Id, string UserType, string LoginType)
        {
            List<FeatureMapping> employeeFeatureNavigationMaster = new List<FeatureMapping>();
            try
            {
                var value = new { Id = Id, UserType = UserType, LoginType = LoginType};
                var procedure = Procedure.GetNavigation;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<FeatureNavigationMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    var data = obj.GroupBy(x => new { x.Feature_Id, x.Feature_Title }).Select(
                        x => new FeatureMapping
                        {
                            Feature_Id = x.Key.Feature_Id,
                            Feature_Title = x.Key.Feature_Title,
                            NavList = x.Select(x => new NavigationList
                            {
                                Navigation_Id = x.Navigation_Id,
                                Navigation_Title = x.Navigation_Title,
                                NavigationPosition = x.NavigationPosition,
                                NavigationUrl = x.NavigationUrl,
                                ParentNavigationId = x.ParentNavigationId,
                                CssClass = x.CssClass
                            }).OrderBy(x => x.NavigationPosition).ToList()
                        }).ToList();
                    employeeFeatureNavigationMaster = _mapper.Map<List<FeatureMapping>>(data);


                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Navigation ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return employeeFeatureNavigationMaster;
        }



    }
}


