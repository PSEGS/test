using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgCourseFeeHead;

namespace AdmissionPortal_API.Data.Repository
{
    public class PgCourseFeeHeadRepository : IPgCourseFeeHeadRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public PgCourseFeeHeadRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddCourseFeeHeadFee(List<AddPgCourseFeeHeadFee> addCourseFeeHead)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.PgCourseFeeHeadWithCollegeType;
                DataTable table = new DataTable();
                table = lst.ToDataTable<AddPgCourseFeeHeadFee>(addCourseFeeHead);
                var value = new
                {
                    CourseFeeHead = table,
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Add  course FeeHead Fee: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> AddCourseWaveOff(PgCourseFeeWaveOff addCourseFeeHead)
        {
            int result = 0;
            try
            {
                //ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.PgSaveCourseFeeWaveOff;
                //DataTable table = new DataTable();
                //table = lst.ToDataTable<AddCourseFeeHeadFEE>(addCourseFeeHead);
                var values = _mapper.Map<PgCourseFeeWaveOff>(addCourseFeeHead);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Add Wave Off: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> AddMapping(List<AddPgCourseFeeHead> addCourseFeeHead)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.PgCourseFeeHead;
                DataTable table = new DataTable();
                table = lst.ToDataTable<AddPgCourseFeeHead>(addCourseFeeHead);
                var value = new
                {
                    CourseFeeHead = table,                    
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Add  course FeeHead Fee: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<FeedHeadDetailsByPgCourse>> GetALLFeeDetailsByCourseID(Int32 universityId, Int32 collegeID, Int32 CourseID, string mode)
        {
            List<FeedHeadDetailsByPgCourse> lstCollegedetail = new List<FeedHeadDetailsByPgCourse>();
            try
            {
                var procedure = Procedure.PgCourseFeeDetails;
                var value = new
                {
                    UniversityID=universityId,
                    CollegeID = collegeID,
                    CourseID = CourseID,
                    mode=mode
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<FeedHeadDetailsByPgCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<FeedHeadDetailsByPgCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College courses Fee Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public Task<List<PgCourseFeeHeadDetails>> GetAllFeeDetailsByHeadId(int HeadId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FeedHeadDetailsByPgCourse>> GetAllFeeHeadByLoginType(int TypeId,int CollegeType,Int32 CreatedBy,Int32 CourseFundType)
        {
            List<FeedHeadDetailsByPgCourse> lstCollegedetail = new List<FeedHeadDetailsByPgCourse>();
            try
            {
                var procedure = Procedure.PgFeeHeadByLoginType;
                var value = new
                {
                    Type = TypeId ,
                    CollegeType=CollegeType,
                    CreatedBy=CreatedBy,
                    CourseFundType=CourseFundType
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<FeedHeadDetailsByPgCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<FeedHeadDetailsByPgCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Fee head By Logint Type : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public async Task<List<PgCourseFeeByCollegId>> GetCourseFeebyColegeId(int collegeId)
        {
            List<PgCourseFeeByCollegId> lstCollegedetail = new List<PgCourseFeeByCollegId>();
            try
            {
                var procedure = Procedure.GetPGCourseFee;
                var value = new
                {

                    CollegeId = collegeId
                    
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PgCourseFeeByCollegId>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<PgCourseFeeByCollegId>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College courses Fee Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public Task<List<PgCourseFeeHeadDetails>> GetMappedCoursesById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetPgCourseFeeWaveOff> GetWaveOffDetailsById(int HeadId, int CollegeType, int UniversityCollegeId)
        {
            GetPgCourseFeeWaveOff WaveOffdetail = new GetPgCourseFeeWaveOff();
            try
            {
                var procedure = Procedure.PgGetCourseFeeWaveOff;
                var value = new
                {
                    HeadId = HeadId,
                    UniversityCollegeId = UniversityCollegeId,

                    CollegeType = CollegeType

                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<GetPgCourseFeeWaveOff>(procedure, value, commandType: CommandType.StoredProcedure);
                    WaveOffdetail = _mapper.Map<GetPgCourseFeeWaveOff>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get PG Fee Header WaveOff Details By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return WaveOffdetail;
        }

        public async Task<int> LockUnlockCourseFeeHeadByCollegeID(int CollegeId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifyBy };

                var procedure = Procedure.PgCoursesFeeHeadLockUnlockByCollegeID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock PG Course Head Fee By College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> LockUnlockCourseFeeHeadByUniversityID(int UniversityId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { UniversityId = UniversityId, Status = Status, ModifyBy = modifyBy };

                var procedure = Procedure.PgCoursesFeeHeadLockUnlockByUniversityID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock PG Course Head Fee By University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
