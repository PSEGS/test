using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgFeeHead;  
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper; 
using Microsoft.Extensions.Configuration;
using AdmissionPortal_API.Utility.DataConfig;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace AdmissionPortal_API.Data.Repository
{
    public class PgFeeHeadRepository : IPgFeeHeadRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public PgFeeHeadRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }
        public async Task<int> AddAsyncFeeHead(AddPgFeeHead entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<AddPgFeeHead>(entity);
                var procedure = Procedure.SavePGFeeHead;                
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Add PG Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> DeleteFeeHead(int Id, int userid)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeletePGFeeHeadById;
                var values = new { Id = Id, UserID = userid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete PG Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<PgFeeHeadDetails>> GetAllFeeHead(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<PgFeeHeadDetails> lstCollege = new List<PgFeeHeadDetails>();
            try
            {
                var procedure = Procedure.GetAllPGFeeHead;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync<PgFeeHeadDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollege = _mapper.Map<List<PgFeeHeadDetails>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All PG Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollege;
        }

        public async Task<PgFeeHeadDetails> GetFeeHeadById(int Id)
        {
            PgFeeHeadDetails collegedetail = new PgFeeHeadDetails();
            try
            {
                var procedure = Procedure.GetPGFeeHeadById;
                var value = new
                {
                    Id = Id
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PgFeeHeadDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    collegedetail = _mapper.Map<PgFeeHeadDetails>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return collegedetail;
        }

        public async Task<int> UpdateFeeHead(UpdatePgFeeHead entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<UpdatePgFeeHead>(entity);
                var procedure = Procedure.UpdatePGFeeHead;                 
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update PG Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
