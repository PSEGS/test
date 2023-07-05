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
using static AdmissionPortal_API.Domain.Model.CourseFeeHead;

namespace AdmissionPortal_API.Data.Repository
{
    public class CourseFeeHead : ICourseFeeHead
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public CourseFeeHead(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddCourseFeeHeadFee(List<AddCourseFeeHeadFEE> addCourseFeeHead)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.courseFeeHeadWithCollegeType;
                DataTable table = new DataTable();
                table = lst.ToDataTable<AddCourseFeeHeadFEE>(addCourseFeeHead);
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

        public async Task<int> AddCourseWaveOff(CourseFeeWaveOff addCourseFeeHead)
        {
            int result = 0;
            try
            {
                //ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.coursefeewaveoff;
                //DataTable table = new DataTable();
                //table = lst.ToDataTable<AddCourseFeeHeadFEE>(addCourseFeeHead);
                var values = _mapper.Map<CourseFeeWaveOff>(addCourseFeeHead);
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

        public async Task<int> AddMapping(List<AddCourseFeeHead> addCourseFeeHead)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.courseFeeHead;
                DataTable table = new DataTable();
                table = lst.ToDataTable<AddCourseFeeHead>(addCourseFeeHead);
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

        public async Task<List<FeedHeadDetailsByCourse>> getALLFeeDetailsByCourseID(Int32 universityId, Int32 collegeID, Int32 CourseID, string mode)
        {
            List<FeedHeadDetailsByCourse> lstCollegedetail = new List<FeedHeadDetailsByCourse>();
            try
            {
                var procedure = Procedure.courseFeeDetails;
                var value = new
                {
                    UniversityID=universityId,
                    CollegeID = collegeID,
                    CourseID = CourseID,
                    mode=mode
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<FeedHeadDetailsByCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<FeedHeadDetailsByCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College courses Fee Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public Task<List<CourseFeeHeadDetails>> getAllFeedetailsByHeadyId(int HeadId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FeedHeadDetailsByCourse>> GetallFeeHeadByLoginTYpe(int TypeId,int CollegeType,Int32 CreatedBy, Int32 CourseFundType)
        {
            List<FeedHeadDetailsByCourse> lstCollegedetail = new List<FeedHeadDetailsByCourse>();
            try
            {
                var procedure = Procedure.FeeHeadByLoginType;
                var value = new
                {
                    Type = TypeId ,
                    CollegeType=CollegeType,
                    CreatedBy=CreatedBy,
                    CourseFundType = CourseFundType,
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<FeedHeadDetailsByCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<FeedHeadDetailsByCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Fee head By Logint Type : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public async Task<List<CourseFeeByCollegId>> getCourseFeebyColegeId(int collegeId)
        {
            List<CourseFeeByCollegId> lstCollegedetail = new List<CourseFeeByCollegId>();
            try
            {
                var procedure = Procedure.GetCourseFee;
                var value = new
                {

                    CollegeId = collegeId
                    
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<CourseFeeByCollegId>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<CourseFeeByCollegId>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College courses Fee Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public Task<List<CourseFeeHeadDetails>> getMappedCoursesById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetCourseFeeWaveOff> getWaveOffDetailsById(int HeadId, int CollegeType, int UniversityCollegeId)
        {
            GetCourseFeeWaveOff WaveOffdetail = new GetCourseFeeWaveOff();
            try
            {
                var procedure = Procedure.Getcoursefeewaveoff;
                var value = new
                {
                    HeadId = HeadId,
                    UniversityCollegeId = UniversityCollegeId,

                    CollegeType = CollegeType

                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<GetCourseFeeWaveOff>(procedure, value, commandType: CommandType.StoredProcedure);
                    WaveOffdetail = _mapper.Map<GetCourseFeeWaveOff>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get WaveOff Details By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return WaveOffdetail;
        }

        public async Task<int> LockUnlockCourseFeeHeadByCollegeID(int CollegeId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifyBy };

                var procedure = Procedure.CoursesFeeHeadLockUnlockByCollegeID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock Course Head Fee By College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> LockUnlockCourseFeeHeadByUniversityID(int UniversityId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { UniversityId = UniversityId, Status = Status, ModifyBy = modifyBy };

                var procedure = Procedure.CoursesFeeHeadLockUnlockByUniversityID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock Course Head Fee By University : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
