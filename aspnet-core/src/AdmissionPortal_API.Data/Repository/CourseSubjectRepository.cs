using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AdmissionPortal_API.Data.Repository
{
    public class CourseSubjectRepository : ISubjectRepository, ICourseSubjectCombinationCheck
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;

        public CourseSubjectRepository(IMapper mapper, IConfiguration configuration, ILogError logError)
        {
            _logError = logError;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<int> AddAsync(SubjectMaster entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<SubjectMaster>(entity);
                // var values = new { University_Name  = entity.UniversityName }

                var procedure = Procedure.SaveSubject;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else if (response.Response == "2")
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create Add Subject: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> Delete(int subjectId, int UserId)
        {
            int result = 0;
            try
            {

                var procedure = Procedure.DeleteSubjectbyId;

                var values = new { SubjectId = subjectId, DeletedBy = UserId };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete university : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();

        }

        public async Task<List<SubjectDetails>> GetAllSubject(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<SubjectDetails> lstSubjectMaster = new List<SubjectDetails>();
            try
            {
                var procedure = Procedure.GetallSubject;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<SubjectDetails>(procedure, commandType: CommandType.StoredProcedure);
                    lstSubjectMaster = _mapper.Map<List<SubjectDetails>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Subjects : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSubjectMaster;
        }

        public async Task<List<coursesubject>> GetAllSubjectByCourseId(int courseid, int universityId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<coursesubject> lstSubjectMaster = new List<coursesubject>();
            try
            {
                var procedure = Procedure.GetallSubjectbyCourseId;
                var value = new
                {
                    courseid = courseid,
                    UniversityId = universityId,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchKeyword = searchKeyword,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<coursesubject>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSubjectMaster = _mapper.Map<List<coursesubject>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Subjects by Course ID : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSubjectMaster;

        }

        public async Task<List<coursesubject>> GetSubjectByCourseId(int courseid, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<coursesubject> lstSubjectMaster = new List<coursesubject>();
            try
            {
                var procedure = Procedure.GetSubjectbyCourseId;
                var value = new
                {
                    courseid = courseid,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchKeyword = searchKeyword,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<coursesubject>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSubjectMaster = _mapper.Map<List<coursesubject>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Subjects by Course ID : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSubjectMaster;

        }
        public async Task<List<coursesubject>> GetSubjectByCourseAndUniversityid(int courseid, int UniversityId)
        {
            List<coursesubject> lstSubjectMaster = new List<coursesubject>();
            try
            {
                var procedure = Procedure.GetSubjectbyCourseAndUniversityId;
                var value = new
                {
                    courseid = courseid,
                    UniversityId = UniversityId
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<coursesubject>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSubjectMaster = _mapper.Map<List<coursesubject>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Subjects by Course And University ID : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSubjectMaster;

        }

        public async Task<SubjectDetails> GetById(int id)
        {
            SubjectDetails lstSubjectMaster = new SubjectDetails();
            try
            {
                var values = new { SubjectId = id };
                var procedure = Procedure.SubjectGetByID;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<SubjectDetails>(procedure, values, commandType: CommandType.StoredProcedure);
                    lstSubjectMaster = _mapper.Map<SubjectDetails>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get  Subjects BY Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSubjectMaster;
        }


        public async Task<int> UpdateAsync(UpdateSubject updateSubject)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<UpdateSubject>(updateSubject);
                var procedure = Procedure.UpdateSubject;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Subject : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public Task<int> UpdateAsync(SubjectMaster entity)
        {
            throw new NotImplementedException();
        }

        Task<SubjectMaster> IGenericRepository<SubjectMaster>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<CourseSubjectCombinationCheck>> GetCourseSubjectCombinationCheckByUniversity(int universityId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<CourseSubjectCombinationCheck> lstSubjectMaster = new List<CourseSubjectCombinationCheck>();
            try
            {
                var procedure = Procedure.GetallSubjectbyCourseId;
                var value = new
                {
                    UniversityId = universityId,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchKeyword = searchKeyword,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CourseSubjectCombinationCheck>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSubjectMaster = _mapper.Map<List<CourseSubjectCombinationCheck>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Subjects by Course ID : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSubjectMaster;

        }

        public async Task<int> CreateUniversityCourseSubjectCombinationCheck(UnvCourseSubjectCombinationCheck entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<UnvCourseSubjectCombinationCheck>(entity);
                var procedure = Procedure.SaveCourseSubjectCombinationCheckByUniversity;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var response = await connection.QuerySingleAsync<DBResponse>(procedure, values, commandType: System.Data.CommandType.StoredProcedure);
                    if (response.Response == "1")
                    {
                        result = 1;
                    }
                    else if (response.Response == "2")
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create UniversityCourseSubjectCombinationCheck: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
