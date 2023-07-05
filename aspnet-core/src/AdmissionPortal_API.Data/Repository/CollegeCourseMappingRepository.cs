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

namespace AdmissionPortal_API.Data.Repository
{
    public class CollegeCourseMappingRepository : ICollegeCourseMappingRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public CollegeCourseMappingRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddMapping(CollegeCourse entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.SaveCollegeCourseMapping;
                DataTable table = new DataTable();
                table = lst.ToDataTable<CreateUgCourse>(entity.courses);
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
        public async Task<List<Course>> getCoursesByCollegeId(Int32 collegeId, Int32 universityId,Int32? Type)
        {
            List<Course> cours = new List<Course>();
            try
            {
                var procedure = Procedure.GetCoursesByCollegeId;
                var value = new
                {
                    CollegeId = collegeId,
                    universityId = universityId,
                    Type = Type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<Course>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<Course>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get College course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }

        public async Task<List<Course>> getMappedCoursesByCollegeId(Int32 collegeId)
        {
            List<Course> cours = new List<Course>();
            try
            {
                var procedure = Procedure.GetMappedCoursesByCollegeId;
                var value = new
                {
                    CollegeId = collegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<Course>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<Course>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get College course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }
        public async Task<List<CombinationCourse>> getCombinationCoursesByCollegeId(Int32 collegeId)
        {
            List<CombinationCourse> cours = new List<CombinationCourse>();
            try
            {
                var procedure = Procedure.GetCombinationsCoursesByCollegeId;
                var value = new
                {
                    CollegeId = collegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<CombinationCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<CombinationCourse>>(obj);
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
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifyBy};

                var procedure = Procedure.LockUnlockCoursesByCollegeID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock College Courses : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        private object await(Task<CollegeCourse> task)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> AddAsync(CollegeCourse entity)
        {
            throw new NotImplementedException();
        }

        public Task<CollegeCourse> GetAsync(CollegeCourse entity)
        {
            throw new NotImplementedException();
        }

        public Task<CollegeCourse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(CollegeCourse entity)
        {
            throw new NotImplementedException();
        }

    }
}
