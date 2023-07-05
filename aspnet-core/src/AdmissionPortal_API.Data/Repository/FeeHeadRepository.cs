using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.FeeHead;  
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper; 
using Microsoft.Extensions.Configuration;
using AdmissionPortal_API.Utility.DataConfig;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace AdmissionPortal_API.Data.Repository
{
    public class FeeHeadRepository : IFeeHead
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public FeeHeadRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }
        public async Task<int> AddAsyncFeeHead(AddFeeHead entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<AddFeeHead>(entity);
                var procedure = Procedure.savefeehead;                
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Add Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> DeleteFeeHead(int Id, int userid)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeletefeeheadbyId;
                var values = new { Id = Id, UserID = userid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<FeeHeadDetails>> GetAllFeeHead(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<FeeHeadDetails> lstCollege = new List<FeeHeadDetails>();
            try
            {
                var procedure = Procedure.GetAllfeehead;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<FeeHeadDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollege = _mapper.Map<List<FeeHeadDetails>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollege;
        }

        public async Task<FeeHeadDetails> GetFeeHeadById(int Id)
        {
            FeeHeadDetails collegedetail = new FeeHeadDetails();
            try
            {
                var procedure = Procedure.GetfeeheadbyId;
                var value = new
                {
                    Id = Id
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<FeeHeadDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    collegedetail = _mapper.Map<FeeHeadDetails>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return collegedetail;
        }

        public async Task<int> UpdateFeeHead(UpdateFeeHead entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<UpdateFeeHead>(entity);
                var procedure = Procedure.updatefeehead;                 
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Fee Head : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
