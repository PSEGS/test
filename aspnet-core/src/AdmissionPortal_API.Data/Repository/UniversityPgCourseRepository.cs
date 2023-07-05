using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.PGCollegeCourseMapping;
using AdmissionPortal_API.Domain.ApiModel.UniversityPgCourse;
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
    public class UniversityPgCourseRepository : IUniversityPgCourseRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public UniversityPgCourseRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddAsync(UniversityPgCourse entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.PGSaveUniversityCourseMapping;
                DataTable table = new DataTable();
                table = lst.ToDataTable<PGUniversityCourse>(entity.courses);
                var value = new
                {
                    UniversityId = entity.UniversityId,
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
                _logError.WriteTextToFile("add university PG course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<List<PGUniversityCourse>> GetCoursesByUniversityId(int? universityId, Int32? Type)
        {
            List<PGUniversityCourse> cours = new List<PGUniversityCourse>();
            try
            {
                var procedure = Procedure.PGGetCoursesByUniversityId;
                var value = new
                {
                    UniversityId = universityId,
                    Type=Type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PGUniversityCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<PGUniversityCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get university PG course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }

        public async Task<List<PGUniversityCourse>> GetMappedCoursesByUniversityId(int universityId,Int32? Type )
        {
            List<PGUniversityCourse> cours = new List<PGUniversityCourse>();
            try
            {
                var procedure = Procedure.PGGetMappedCoursesByUniversityId;
                var value = new
                {
                    UniversityId = universityId,
                    Type=Type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<PGUniversityCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<PGUniversityCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }
        private object await(Task<UniversityPgCourse> task)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
      
        public Task<UniversityPgCourse> GetAsync(UniversityPgCourse entity)
        {
            throw new NotImplementedException();
        }

        public Task<UniversityPgCourse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(UniversityPgCourse entity)
        {
            throw new NotImplementedException();
        }
    }
}
