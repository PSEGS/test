using AdmissionPortal_API.Data.RepositoryInterface; 
using AdmissionPortal_API.Domain.ApiModel.College;
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
    public class CourseRepository : ICourse
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public CourseRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }
        public async Task<int> AddAsyncCourse(AddCourse entity)
        {
            int result =0;
            try
            {
                string procedure = Procedure.SaveCourse;
                var values = _mapper.Map<AddCourse>(entity);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result =  Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    //addcourse = _mapper.Map<AddCourse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Add Course : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<int> DeleteCourse( int courseid, int userid)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.DeleteCoursebyId;
                var values = new { CourseId = courseid, UserID = userid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Course : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<List<CourseDetail>> GetAllCourse(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<CourseDetail> lstCourse = new List<CourseDetail>();
            try
            {
                var procedure = Procedure.GetallCourse;
                var value = new { PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CourseDetail>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCourse = _mapper.Map<List<CourseDetail>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Colleges : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCourse;
        }     
        public async Task<CourseDetail> GetCourseById(int courseId)
        {
            CourseDetail coursedetail = new CourseDetail();
            try
            {
                var procedure = Procedure.CourseGetByID;
                var value = new
                {
                    CourseId = courseId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<CourseDetail>(procedure, value, commandType: CommandType.StoredProcedure);
                    coursedetail = _mapper.Map<CourseDetail>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return coursedetail;
        }
        public async Task<int> UpdateCourse(updateCourse entity)
        {
              int result = 0;
            try
            {
                var values = _mapper.Map<updateCourse>(entity);
                var procedure = Procedure.UpdateCourse;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<CourseList>> GetAllUgCourse()
        {
            List<CourseList> lstCourse = new List<CourseList>();
            try
            {
                var procedure = Procedure.GetallUgCourse;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CourseList>(procedure, commandType: CommandType.StoredProcedure);
                    lstCourse = _mapper.Map<List<CourseList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Ug courses : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCourse;
        }

    }
}
