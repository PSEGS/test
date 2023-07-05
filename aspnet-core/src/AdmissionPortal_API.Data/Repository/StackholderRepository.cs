using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration; 
using System.Data;
using System.Data.SqlClient;
 

namespace AdmissionPortal_API.Data.Repository
{
  public   class StackholderRepository: IStackholderRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public StackholderRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }

        public Task<int> AddAsync(StackHolderMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StackHolderMaster>> GetAllAsync()
        {
            List<StackHolderMaster> lstStackholder = new List<StackHolderMaster>();
            try
            {
                var procedure = Procedure.GetStackholder;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync<StackHolderMaster>(procedure, commandType: CommandType.StoredProcedure);
                    lstStackholder = _mapper.Map<List<StackHolderMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All StackHolder : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstStackholder;
        }

        public Task<StackHolderMaster> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(StackHolderMaster entity)
        {
            throw new NotImplementedException();
        }
    }
}
