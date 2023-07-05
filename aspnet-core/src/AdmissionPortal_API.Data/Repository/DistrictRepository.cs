using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public DistrictRepository(IMapper mapper, IConfiguration configuration, ILogError logError)
        {
            _logError = logError;
            _mapper = mapper;
            _configuration = configuration;
        }
        public Task<int> AddAsync(DistrictMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<DistrictMaster> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(DistrictMaster entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<DistrictMaster>> GetDistrictByStateId(int stateId)
        {
            List<DistrictMaster> districtsResult = new List<DistrictMaster>();
            try
            {
                var procedure = Procedure.GetDistrictsByStateId;
                var values = new { StateId = stateId };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var result = await con.QueryAsync<DistrictMaster>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    districtsResult = _mapper.Map<List<DistrictMaster>>(result);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get District By State Id : " , ex.Message,ex.HResult,ex.StackTrace);
            }
            return districtsResult;
        }

    }
}

