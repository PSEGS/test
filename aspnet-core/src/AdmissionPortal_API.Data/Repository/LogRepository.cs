using AdmissionPortal_API.Data.RepositoryInterface;
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
    public class LogRepository : ILogRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public LogRepository(IMapper mapper, IConfiguration configuration, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }
        public async Task<List<LogMaster>> GetAllLog(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder, DateTime? startDate, DateTime? endDate, string type)
        {
            List<LogMaster> lstLog = new List<LogMaster>();
            try
            {
                var procedure = Procedure.GetAllLog;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder ,
                    StartDate=startDate ,EndDate=endDate,Type =type};
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<LogMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstLog = _mapper.Map<List<LogMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Logs : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstLog; ;
        }
    }
}

