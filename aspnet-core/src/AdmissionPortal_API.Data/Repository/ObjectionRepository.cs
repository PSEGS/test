using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Objection;
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
    public class ObjectionRepository : IObjectionRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;

        public ObjectionRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<ServiceResult> CreateObjectionAsync(AddObjection addObjection)
        {
            ServiceResult _result = new ServiceResult();
            try
            {
                var values = _mapper.Map<AddObjection>(addObjection);
                var procedure = Procedure.SaveObjection;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<ServiceResult>(procedure, values, commandType: CommandType.StoredProcedure);
                    _result = _mapper.Map<ServiceResult>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("add Objection : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _result;
        }

        public async Task<ObjectionDetails> GetAllObjectionsByRegId(string RegId)
        {
            ObjectionDetails objectionDetails = new ObjectionDetails();

            try
            {
                var procedure = Procedure.GetAllObjectionsByRegId;
                var values = new { RegistrationNumber = RegId };

                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    using (var multi = con.QueryMultipleAsync(procedure, values, commandType: CommandType.StoredProcedure).Result)
                    {
                        objectionDetails.objections = multi.Read<GetObjection>().ToList();
                        objectionDetails.studentQualifications = multi.Read<StudentQualification>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Objectioins By Reg Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return objectionDetails;
        }

        public async Task<ObjectionDetails> GetAllObjectionsByCollegId(string CollegeId)
        {
            ObjectionDetails objectionDetails = new ObjectionDetails();
            try
            {
                var procedure = Procedure.GetAllObjectionsByCollegeId;
                var values = new { CollegeId = CollegeId };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    using (var multi = con.QueryMultipleAsync(procedure, values, commandType: CommandType.StoredProcedure).Result)
                    {
                        objectionDetails.objections = multi.Read<GetObjection>().ToList();
                        objectionDetails.studentQualifications = multi.Read<StudentQualification>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Objectioins By College Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return objectionDetails;
        }

        public async Task<GetSingleObjection> GetObjectionById(int id)
        {
            GetSingleObjection objectionDetail = new GetSingleObjection();
            try
            {
                var procedure = Procedure.GetObjectionById;
                var value = new
                {
                    ObjectionId = id
                };

                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    using (var multi = con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure).Result)
                    {
                        objectionDetail = multi.Read<GetSingleObjection>().FirstOrDefault();
                        objectionDetail.StudentQualifications = multi.Read<StudentQualification>().ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Objection By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return objectionDetail;
        }
    }
}
