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
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class PgCollegeCourseSeatRepository : IPgCollegeCourseSeatRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public PgCollegeCourseSeatRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }
        public async Task<int> AddAsyncSeat(PgCollegeCourseSeat entity)
        {
            int result = 0;
            try
            {
                string procedure = Procedure.PGSaveCollegeCourseSeat;
                var values = _mapper.Map<PgCollegeCourseSeat>(entity);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    //addcourse = _mapper.Map<AddCourse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Add PG College Course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> DeleteSeat(int Id, int userid)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.PGDeleteCollegeCourseSeatbyId;
                var values = new { Id = Id, DeletedBy = userid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete PG College Course Seat  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<PgCollegeCourseSeatDetails>> GetAllSeat(int collegeId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<PgCollegeCourseSeatDetails> lstCourse = new List<PgCollegeCourseSeatDetails>();
            try
            {
                var procedure = Procedure.PGGetAllCollegeCourseSeat;
                var value = new { collegeId= collegeId, PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync<PgCollegeCourseSeatDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCourse = _mapper.Map<List<PgCollegeCourseSeatDetails>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All PG Colleges Course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCourse;
        }

        public async Task<List<PgGetReservationQuotaModel>> GetReservationQuota()
        {
            List<PgGetReservationQuotaModel> Quota = new List<PgGetReservationQuotaModel>();
            try
            {
                var procedure = Procedure.PGReservationQuota;
               
                
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PgGetReservationQuotaModel>(procedure, null, commandType: CommandType.StoredProcedure);
                    Quota = _mapper.Map <List<PgGetReservationQuotaModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get PG Reservation  Quota : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return Quota;
        }

        public async Task<PgCollegeCourseSeatDetails> GetSeatById(int Id)
        {
            PgCollegeCourseSeatDetails coursedetail = new PgCollegeCourseSeatDetails();
            try
            {
                var procedure = Procedure.PGGetCollegeCourseSeatByID;
                var value = new
                {
                    Id = Id
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PgCollegeCourseSeatDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    coursedetail = _mapper.Map<PgCollegeCourseSeatDetails>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get PG College course Seat By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return coursedetail;
        }

        public async Task<List<PgSeatMatrixM>> PGseatMatrixMs(int id)
        {

            List<PgSeatMatrixM> lstSeat = new List<PgSeatMatrixM>();
            try
            {
                var procedure = Procedure.PGCollegeCourseSeatMatrix;
                var value = new { College_Id = id };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync<PgSeatMatrixM>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSeat = _mapper.Map<List<PgSeatMatrixM>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All PG Colleges Course Seat Matrix: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSeat;
        }

        public async Task<int> UpdateSeat(UpdatePgCollegeCourseSeat entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<UpdatePgCollegeCourseSeat>(entity);
                var procedure = Procedure.PGUpdateCollegeCourseSeat;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update PG College Course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> LockUnlockCourseSeatsByCollegeID(int CollegeId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifyBy };

                var procedure = Procedure.PGLockUnlockCourseSeatsByCollegeID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock PG College Course Seats : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
