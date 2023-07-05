using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.OfflineAdmisson;
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
    public class OfflineAdmissionRepository : IOfflineAdmission
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public OfflineAdmissionRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task<int> AddAdmission(OfflineAdmissionModel entity)
        {
            int result = 0;

            try
            {
                var values = _mapper.Map<OfflineAdmissionModel>(entity);
                var procedure = Procedure.OfflineAdmission;
                
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Offline Admission : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
