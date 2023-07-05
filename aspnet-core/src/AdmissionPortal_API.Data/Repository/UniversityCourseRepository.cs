using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Domain.ApiModel.UniversityCourse;
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
    public class UniversityCourseRepository: IUniversityCourseRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public UniversityCourseRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddAsync(UniversityCourse entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.SaveUniversityCourseMapping;
                DataTable table = new DataTable();
                table = lst.ToDataTable<universityCourse>(entity.courses);
                //for (int i = 0; i < table.Rows.Count; i++)
                //{
                //    if(Convert.ToBoolean(table.Rows[i]["IsSelfFinace"]) == true)
                //    {

                //    }
                //    else
                //    {

                //    }
                //}
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
                _logError.WriteTextToFile("add uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<List<universityCourse>> getCoursesByUniversityId(int? universityId, Int32? Type)
        {
            List<universityCourse> cours = new List<universityCourse>();
            try
            {
                var procedure = Procedure.GetCoursesByUniversityId;
                var value = new
                {
                    UniversityId = universityId,
                    Type=Type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<universityCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<universityCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }

        public async Task<List<universityCourse>> getMappedCoursesByUniversityId(int universityId,Int32? Type )
        {
            List<universityCourse> cours = new List<universityCourse>();
            try
            {
                var procedure = Procedure.GetMappedCoursesByUniversityId;
                var value = new
                {
                    UniversityId = universityId,
                    Type=Type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<universityCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<universityCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }
        public async Task<List<universityCourse>> getCombinationsCoursesByUniversityId(int universityId, Int32? Type)
        {
            List<universityCourse> cours = new List<universityCourse>();
            try
            {
                var procedure = Procedure.GetCombinationsCoursesByUniversityId;
                var value = new
                {
                    UniversityId = universityId,
                    Type = Type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<universityCourse>(procedure, value, commandType: CommandType.StoredProcedure);
                    cours = _mapper.Map<List<universityCourse>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get uni course mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return cours;
        }
        private object await(Task<UniversityCourse> task)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
      
        public Task<UniversityCourse> GetAsync(UniversityCourse entity)
        {
            throw new NotImplementedException();
        }

        public Task<UniversityCourse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(UniversityCourse entity)
        {
            throw new NotImplementedException();
        }
    }
}
