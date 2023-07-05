using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.PGObjection;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class PGObjectionRepository : IPGObjectionRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public PGObjectionRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<ServiceResult> CreateObjectionAsync(AddPgObjection addObjection)
        {
            ServiceResult _result = new ServiceResult();
            try
            {
                var values = _mapper.Map<AddPgObjection>(addObjection);
                var procedure = Procedure.PGSaveObjection;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync<ServiceResult>(procedure, values, commandType: CommandType.StoredProcedure);
                    _result = _mapper.Map<ServiceResult>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("add PG Objection : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _result;
        }

        public async Task<PgObjectionDetails> GetAllObjectionsByRegId(string RegId)
        {
            PgObjectionDetails objectionDetails = new PgObjectionDetails();

            try
            {
                var procedure = Procedure.PGGetAllObjectionsByRegId;
                var values = new { RegistrationNumber = RegId };

                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    using (var multi = con.QueryMultipleAsync(procedure, values, commandType: CommandType.StoredProcedure).Result)
                    {
                        objectionDetails.objections = multi.Read<GetPgObjection>().ToList();
                        objectionDetails.studentQualifications = multi.Read<PgStudentQualification>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All PG Objectioins By Reg Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return objectionDetails;
        }

        public async Task<PgObjectionDetails> GetAllObjectionsByCollegId(string CollegeId)
        {
            PgObjectionDetails objectionDetails = new PgObjectionDetails();
            try
            {
                var procedure = Procedure.PGGetAllObjectionsByCollegeId;
                var values = new { CollegeId = CollegeId };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    using (var multi = con.QueryMultipleAsync(procedure, values, commandType: CommandType.StoredProcedure).Result)
                    {
                        objectionDetails.objections = multi.Read<GetPgObjection>().ToList();
                        objectionDetails.studentQualifications = multi.Read<PgStudentQualification>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All PG Objectioins By College Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return objectionDetails;
        }

        public async Task<GetSinglePgObjection> GetObjectionById(int id)
        {
            GetSinglePgObjection objectionDetail = new GetSinglePgObjection();
            try
            {
                var procedure = Procedure.PGGetObjectionById;
                var value = new
                {
                    ObjectionId = id
                };

                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    using (var multi = con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure).Result)
                    {
                        objectionDetail = multi.Read<GetSinglePgObjection>().FirstOrDefault();
                        objectionDetail.StudentQualifications = multi.Read<PgStudentQualification>().ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get PG Objection By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return objectionDetail;
        }
    }
}
