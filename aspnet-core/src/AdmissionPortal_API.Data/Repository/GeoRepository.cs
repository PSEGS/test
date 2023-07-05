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
    public class GeoRepository : IGeoRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public GeoRepository(IMapper mapper, IConfiguration configuration, ILogError logError)
        {
            _logError = logError;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<List<DropDownModel>> GetAllBlockByDistrict(string districtId)
        {
            List<DropDownModel> districtsResult = new List<DropDownModel>();
            try
            {
                var procedure = Procedure.GetAllBlockByDistrict;
                var values = new { districtId = districtId };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var result = await con.QueryAsync<DropDownModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    districtsResult = _mapper.Map<List<DropDownModel>>(result);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get BlockByDistrict Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return districtsResult;
        }

        public async Task<List<DropDownModel>> GetAllTehsilByDistrict(string districtId)
        {
            List<DropDownModel> districtsResult = new List<DropDownModel>();
            try
            {
                var procedure = Procedure.GetAllTehsilByDistrict;
                var values = new { districtId = districtId };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var result = await con.QueryAsync<DropDownModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    districtsResult = _mapper.Map<List<DropDownModel>>(result);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("GetTehsilByDistrict Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return districtsResult;
        }

        public async Task<List<DropDownModel>> GetAllVillageByBlock(string blockId)
        {
            List<DropDownModel> districtsResult = new List<DropDownModel>();
            try
            {
                var procedure = Procedure.GetAllVillageByBlock;
                var values = new { blockId = blockId };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var result = await con.QueryAsync<DropDownModel>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    districtsResult = _mapper.Map<List<DropDownModel>>(result);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get VillageByBlock Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return districtsResult;
        }
    }
}
