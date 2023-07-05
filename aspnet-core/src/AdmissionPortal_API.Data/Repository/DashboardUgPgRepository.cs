using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using AdmissionPortal_API.Data.RepositoryInterface;

namespace AdmissionPortal_API.Data.Repository
{
    public class DashboardUgPgRepository : IDashboardUgPgRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public DashboardUgPgRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task<DashboardUg> GetDashboardUg(string type, string UserID)
        {
            DashboardUg collegedetail = new DashboardUg();
            try
            {
                var procedure = Procedure.DashboardUg;
                var value = new
                {
                    UserID = UserID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    collegedetail = _mapper.Map<DashboardUg>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get dashboard ug : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return collegedetail;
        }
        public async Task<List<DashboardChartUg>> GetDashboardChartUg(string type)
        {
            List<DashboardChartUg> dash = new List<DashboardChartUg>();
            try
            {
                var procedure = Procedure.DashboardChartUg;
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync(procedure, commandType: CommandType.StoredProcedure);
                    dash = _mapper.Map<List<DashboardChartUg>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get dashboard ug chart : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return dash;
        }
        public async Task<DashboardPg> GetDashboardPg(string type)
        {
            DashboardPg collegedetail = new DashboardPg();
            try
            {
                var procedure = Procedure.DashboardPg;
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync(procedure, commandType: CommandType.StoredProcedure);
                    collegedetail = _mapper.Map<DashboardPg>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get dashboard pg : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return collegedetail;
        }
        public async Task<List<DashboardChartUg>> GetDashboardChartPg(string type)
        {
            List<DashboardChartUg> dash = new List<DashboardChartUg>();
            try
            {
                var procedure = Procedure.DashboardChartPg;
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync(procedure, commandType: CommandType.StoredProcedure);
                    dash = _mapper.Map<List<DashboardChartUg>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get dashboard pg chart : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return dash;
        }
    }
}
