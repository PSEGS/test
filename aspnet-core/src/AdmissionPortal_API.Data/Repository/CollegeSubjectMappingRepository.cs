using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.CollegeSubjectMapping;
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
    public class CollegeSubjectMappingRepository : ICollegeSubjectMappingRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public CollegeSubjectMappingRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddMapping(CollegeCourseSectionSubject entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.SaveCollegeCourseSubjectMapping;
                DataTable table = new DataTable();
                table = lst.ToDataTable<CreateSubject>(entity.subjects);
                var value = new
                {
                    CollegeId = entity.CollegeId,
                    SectionId = entity.SectionId,
                    CourseId = entity.CourseId,
                    Subjects = table,
                    createdby = entity.CreatedBy,
                    fees = entity.Fees
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
        public async Task<List<Subject>> getSubjectByCollegeId(Int32 collegeId)
        {
            List<Subject> cours = new List<Subject>();
            try
            {
                var procedure = Procedure.GetSubjectsByCollegeId;
                var value = new
                {
                    CollegeId = collegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<Subject>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<Subject>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get College subject mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }

        public async Task<List<SubjectListing>> getCombinationByCollegeId(Int32 collegeId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            List<SubjectListing> cours = new List<SubjectListing>();
            try
            {
                var procedure = Procedure.GetCombinationByCollegeId;
                var value = new
                {
                    CollegeId = collegeId,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchKeyword = searchKeyword,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<SubjectListing>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<SubjectListing>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get College subject mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }
        public async Task<List<Common>> getCombinationByCollegeCourse(Int32 collegeId, Int32 courseId)
        {
            List<Common> cours = new List<Common>();
            try
            {
                var procedure = Procedure.GetCombinationByCollegeCourse;
                var value = new
                {
                    CollegeId = collegeId,
                    CourseId = courseId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<Common>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<Common>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get College Course: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }

        private object await(Task<CollegeCourseSectionSubject> task)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(int id)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeleteCombinationById;
                var values = new { combinationId = id, IsDelete = true };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = await connection.ExecuteScalarAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete combination : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public Task<int> AddAsync(CollegeCourseSectionSubject entity)
        {
            throw new NotImplementedException();
        }

        public Task<CollegeCourseSectionSubject> GetAsync(CollegeCourseSectionSubject entity)
        {
            throw new NotImplementedException();
        }

        public Task<CollegeCourseSectionSubject> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(CollegeCourseSectionSubject entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.UpdateCombination;
                DataTable table = new DataTable();
                table = lst.ToDataTable<CreateSubject>(entity.subjects);
                var value = new
                {
                    CollegeId = entity.CollegeId,
                    SectionId = entity.SectionId,
                    CourseId = entity.CourseId,
                    Subjects = table,
                    createdby = entity.CreatedBy,
                    fees = entity.Fees
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("update combination: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> LockUnlockCollegeSubjectMappingByCollegeID(int CollegeId, int Status, int modifyBy)
        {
            int result = 0;
            try
            {
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifyBy };

                var procedure = Procedure.LockUnlockCollegeCourseSubjectMappingByCollegeId;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Lock Unlock College Course Subject Mapping : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
