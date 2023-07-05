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
    public class MasterApiRepository : IMasterApiRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public MasterApiRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }
        public async Task<List<Board>> GetBoard()
        {

            List<Board> lstBoard = new List<Board>();
            try
            {
                var procedure = Procedure.GetAllBoards;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<Board>(procedure, commandType: CommandType.StoredProcedure);
                    lstBoard = _mapper.Map<List<Board>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Board : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstBoard;
        }

        public async Task<List<GetReligion>> GetReligion()
        {

            List<GetReligion> lstReligion = new List<GetReligion>();
            try
            {
                var procedure = Procedure.GetAllReligion;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GetReligion>(procedure, commandType: CommandType.StoredProcedure);
                    lstReligion = _mapper.Map<List<GetReligion>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Religion  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstReligion;
        }

        public async Task<List<GetReservationCategory>> GetReservationCategory()
        {

            List<GetReservationCategory> lstReservation = new List<GetReservationCategory>();
            try
            {
                var procedure = Procedure.GetReservationCategory;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GetReservationCategory>(procedure, commandType: CommandType.StoredProcedure);
                    lstReservation = _mapper.Map<List<GetReservationCategory>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Reservation Category  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstReservation;
        }

        public async Task<List<GetReservationSubCategory>> GetReservationSubCategory(int id, int CatId = 0)
        {

            List<GetReservationSubCategory> lstReservation = new List<GetReservationSubCategory>();
            try
            {
                var procedure = Procedure.GetReservationSubCategory;
                var values = new { Id = id, CatId = CatId };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GetReservationSubCategory>(procedure, values, commandType: CommandType.StoredProcedure);
                    lstReservation = _mapper.Map<List<GetReservationSubCategory>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Reservation Sub Category  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstReservation;
        }
        public async Task<List<Common>> GetOccupation()
        {

            List<Common> lstBoard = new List<Common>();
            try
            {
                var procedure = Procedure.GetAllOccupation;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<Common>(procedure, commandType: CommandType.StoredProcedure);
                    lstBoard = _mapper.Map<List<Common>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Occupation : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstBoard;
        }
        public async Task<List<Common>> GetFatherOccupation()
        {

            List<Common> lstBoard = new List<Common>();
            try
            {
                var procedure = Procedure.GetFatherOccupation;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<Common>(procedure, commandType: CommandType.StoredProcedure);
                    lstBoard = _mapper.Map<List<Common>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Occupation : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstBoard;
        }
        public async Task<List<Common>> GetBoardSubject()
        {

            List<Common> lstBoard = new List<Common>();
            try
            {
                var procedure = Procedure.GetBoardSubject;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<Common>(procedure, commandType: CommandType.StoredProcedure);
                    lstBoard = _mapper.Map<List<Common>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All board subject : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstBoard;
        }
        public async Task<List<Common>> GetHouseholdIncome()
        {

            List<Common> lstBoard = new List<Common>();
            try
            {
                var procedure = Procedure.GetAllHouseholdIncome;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<Common>(procedure, commandType: CommandType.StoredProcedure);
                    lstBoard = _mapper.Map<List<Common>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All HouseholdIncome : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstBoard;
        }
        public async Task<List<BankMaster>> BankMaster()
        {

            List<BankMaster> lstBoard = new List<BankMaster>();
            try
            {
                var procedure = Procedure.GetAllBank;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<BankMaster>(procedure, commandType: CommandType.StoredProcedure);
                    lstBoard = _mapper.Map<List<BankMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Bank Master : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstBoard;
        }

        public async Task<islockDetails> GetIsLockDetails(int collegeid)
        {

            islockDetails islockDetails = new islockDetails();
            UGIsLock uGIsLock = new UGIsLock();
            PgIsLock pGIsLock = new PgIsLock();
            try
            {
                var procedure = Procedure.IsLockDetails;
                var values = new { collegeid = collegeid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryMultipleAsync(procedure, values, commandType: CommandType.StoredProcedure);

                    islockDetails.UGIsLock = obj.Read<UGIsLock>().ToList();
                    islockDetails.PGIsLock = obj.Read<PgIsLock>().ToList();
                    //lstBoard = _mapper.Map<List<BankMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Bank Master : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return islockDetails;
        }

        public async Task<List<Country>> GetCountries()
        {

            List<Country> lstCountry = new List<Country>();
            try
            {
                var procedure = Procedure.GetAllCountries;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<Country>(procedure, commandType: CommandType.StoredProcedure);
                    lstCountry = _mapper.Map<List<Country>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Countries : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCountry;
        }
    }
}
