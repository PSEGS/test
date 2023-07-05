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
    public class StateRepository : IStateRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public StateRepository(IMapper mapper,IConfiguration configuration,ILogError logError)
        {
            _logError = logError;
            _mapper = mapper;
            _configuration = configuration;
        }
        public Task<int> AddAsync(StateMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        public  async Task<List<StateMaster>> GetAllAsync()
        {
            List<StateMaster> stateResult = new List<StateMaster>();
            try
            {
                var procedure = Procedure.GetAllStates;
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var result = await con.QueryAsync<StateMaster>(procedure,commandType:System.Data.CommandType.StoredProcedure);
                    stateResult = _mapper.Map<List<StateMaster>>(result);
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All State : ", ex.Message, ex.HResult, ex.StackTrace) ;
            }
            return stateResult;
        }

        public Task<StateMaster> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> UpdateAsync(StateMaster entity)
        {
            throw new NotImplementedException();
        }
    }
}

