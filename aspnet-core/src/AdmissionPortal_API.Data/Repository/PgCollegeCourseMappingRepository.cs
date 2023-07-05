using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.PGCollegeCourseMapping;
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

namespace AdmissionPortal_API.Data.Repository
{
    public class PgCollegeCourseMappingRepository : IPgCollegeCourseMappingRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public PgCollegeCourseMappingRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddMapping(PgCollegeCourse entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.PGSaveCollegeCourseMapping;
                DataTable table = new DataTable();
                table = lst.ToDataTable<CreatePgCourse>(entity.courses);
                var value = new
                {
                    CollegeId = entity.CollegeId,
                    courses = table,
                    createdby = entity.CreatedBy
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("add College course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<List<PgCourse>> getCoursesByCollegeId(Int32 collegeId, Int32 universityId,Int32? Type)
        {
            List<PgCourse> cours = new List<PgCourse>();
            try
            {
                var procedure = Procedure.PGGetCoursesByCollegeId;
                var value = new
                {
                    CollegeId = collegeId,
                    universityId = universityId,
                    Type = Type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PgCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<PgCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get College course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }

        public async Task<List<PgCourse>> getMappedCoursesByCollegeId(Int32 collegeId)
        {
            List<PgCourse> cours = new List<PgCourse>();
            try
            {
                var procedure = Procedure.PGGetMappedCoursesByCollegeId;
                var value = new
                {
                    CollegeId = collegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PgCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<PgCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get College course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }
        public async Task<List<PgCourse>> getCombinationCoursesByCollegeId(Int32 collegeId)
        {
            List<PgCourse> cours = new List<PgCourse>();
            try
            {
                var procedure = Procedure.PGGetCombinationsCoursesByCollegeId;
                var value = new
                {
                    CollegeId = collegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PgCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<PgCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get College course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }

        public async Task<int> LockUnlockCoursesByCollegeID(int CollegeId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifyBy };

                var procedure = Procedure.PGLockUnlockCoursesByCollegeID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock College PG Courses : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        private object await(Task<PgCollegeCourse> task)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> AddAsync(PgCollegeCourse entity)
        {
            throw new NotImplementedException();
        }

        public Task<PgCollegeCourse> GetAsync(PgCollegeCourse entity)
        {
            throw new NotImplementedException();
        }

        public Task<PgCollegeCourse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(PgCollegeCourse entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<PgMappedCourse>> GetPgMappedCourseByCollege(Int32 collegeId)
        {
            List<PgMappedCourse> cours = new List<PgMappedCourse>();
            try
            {
                var procedure = Procedure.GetPgMappedCourseByCollege;
                var value = new
                {
                    CollegeId = collegeId
                    
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PgMappedCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<PgMappedCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get PG Mapped Course by College ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }   
    }
}
