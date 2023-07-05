using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using AdmissionPortal_API.Domain.ViewModel;
using static AdmissionPortal_API.Domain.Model.CourseModel;

namespace AdmissionPortal_API.Data.Repository
{
    public class CollegeCourseSeatRepository : ICollegeCourseSeatRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public CollegeCourseSeatRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }
        public async Task<int> AddAsyncSeat(CollegeCourseSeat entity)
        {
            int result = 0;
            try
            {
                string procedure = Procedure.SaveCollegeCourseSeat;
                var values = _mapper.Map<CollegeCourseSeat>(entity);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    //addcourse = _mapper.Map<AddCourse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Add College Course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> DeleteSeat(int Id, int userid)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeleteCollegeCourseSeatbyId;
                var values = new { Id = Id, DeletedBy = userid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete College Course Seat  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<CollegeCourseSeatDetails>> GetAllSeat(int collegeId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<CollegeCourseSeatDetails> lstCourse = new List<CollegeCourseSeatDetails>();
            try
            {
                var procedure = Procedure.GetAllCollegeCourseSeat;
                var value = new { collegeId= collegeId, PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CollegeCourseSeatDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCourse = _mapper.Map<List<CollegeCourseSeatDetails>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Colleges Course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCourse;
        }

        public async Task<List<GetReservationQuotaModel>> GetReservationQuota()
        {
            List<GetReservationQuotaModel> Quota = new List<GetReservationQuotaModel>();
            try
            {
                var procedure = Procedure.ReservationQuota;
               
                
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<GetReservationQuotaModel>(procedure, null, commandType: CommandType.StoredProcedure);
                    Quota = _mapper.Map <List<GetReservationQuotaModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Get Reservation  Quota : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return Quota;
        }

        public async Task<CollegeCourseSeatDetails> GetSeatById(int Id)
        {
            CollegeCourseSeatDetails coursedetail = new CollegeCourseSeatDetails();
            try
            {
                var procedure = Procedure.GetCollegeCourseSeatByID;
                var value = new
                {
                    Id = Id
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<CollegeCourseSeatDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    coursedetail = _mapper.Map<CollegeCourseSeatDetails>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College course Seat By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return coursedetail;
        }

        public async Task<List<SeatMatrixM>> PGseatMatrixMs(int id)
        {

            List<SeatMatrixM> lstSeat = new List<SeatMatrixM>();
            try
            {
                var procedure = Procedure.GetPGCollegeCourseSeatMatrix;
                var value = new { College_Id = id };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<SeatMatrixM>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSeat = _mapper.Map<List<SeatMatrixM>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Colleges Course Seat Matrix: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSeat;
        }

        public async Task<List<SeatMatrixM>> seatMatrixMs(int id)
        {
            List<SeatMatrixM> lstSeat = new List<SeatMatrixM>();
            try
            {
                var procedure = Procedure.GetAllCollegeCourseSeatMatrix;
                var value = new { College_Id = id};
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<SeatMatrixM>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSeat = _mapper.Map<List<SeatMatrixM>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Colleges Course Seat Matrix: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSeat;
        }

        public async Task<int> UpdateSeat(UpdateCollegeCourseSeat entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<UpdateCollegeCourseSeat>(entity);
                var procedure = Procedure.UpdateCollegeCourseSeat;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update College Course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> LockUnlockCourseSeatsByCollegeID(int CollegeId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifyBy };

                var procedure = Procedure.LockUnlockCourseSeatsByCollegeID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock College Course Seats : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> lockUnlockSeatMatrixByCollegeID(int CollegeId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { collegeID = CollegeId, Status = Status, userid = modifyBy };

                var procedure = Procedure.LockUnlockCourseSeatMatrixByCollegeID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock College Course Seats : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> PGlockUnlockSeatMatrixByCollegeID(int CollegeId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { collegeID = CollegeId, Status = Status, userid = modifyBy };

                var procedure = Procedure.PGLockUnlockCourseSeatMatrixByCollegeID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Pg Lock Unlock College Course Seats : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
