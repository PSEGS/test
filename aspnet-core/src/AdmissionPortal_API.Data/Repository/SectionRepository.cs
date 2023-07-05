using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Section;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
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
    public class SectionRepository : ISectionRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public SectionRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task<int> AddSection(AddSection entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.SaveSection;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, entity, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Add Section: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UpdateSection(UpdateSection entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UpdateSection;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, entity, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Section: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> DeleteSectionById(int id)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeleteSectionbyId;
                var values = new { SectionId = id};
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Section : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<GetAllSection> GetSectionById(int id)
        {
            GetAllSection sectionDetail = new GetAllSection();
            try
            {
                var procedure = Procedure.GetSectionByID;
                var value = new
                {
                    SectionId = id
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QuerySingleAsync<GetAllSection>(procedure, value, commandType: CommandType.StoredProcedure);
                    sectionDetail = _mapper.Map<GetAllSection>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Section By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return sectionDetail;
        }

        public async Task<List<GetAllSection>> GetAllSection(int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<GetAllSection> lstSection = new List<GetAllSection>();
            try
            {
                var procedure = Procedure.GetAllSection;
                var value = new {PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync<GetAllSection>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSection = _mapper.Map<List<GetAllSection>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Section : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSection;
        }
    }
}
