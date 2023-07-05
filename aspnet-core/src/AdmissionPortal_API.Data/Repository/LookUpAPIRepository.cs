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
using static AdmissionPortal_API.Domain.Model.LookupAPIModel;
using Newtonsoft.Json.Linq;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Domain.ExternalAPI;

namespace AdmissionPortal_API.Data.Repository
{
    public class LookUpAPIRepository : ILookUpAPIRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public LookUpAPIRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }

        public Task<int> AddAsync(LookupAPIModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CollegeMode>> GetAllCollegeMode()
        {

            List<CollegeMode> lstCollegeMode = new List<CollegeMode>();
            try
            {
                var procedure = Procedure.GetAllCollegeMode;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CollegeMode>(procedure, commandType: CommandType.StoredProcedure);
                    lstCollegeMode = _mapper.Map<List<CollegeMode>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All College Mode : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegeMode;
        }

        public async Task<List<CollegeType>> GetAllCollegeType()
        {
            List<CollegeType> lstCollegeType = new List<CollegeType>();
            try
            {
                var procedure = Procedure.GetAllCollegeType;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CollegeType>(procedure, commandType: CommandType.StoredProcedure);
                    lstCollegeType = _mapper.Map<List<CollegeType>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All College Type : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegeType;
        }

        public async Task<List<CourseTypeModel>> GetAllCourseType()
        {
            List<CourseTypeModel> lstCourseType = new List<CourseTypeModel>();
            try
            {
                var procedure = Procedure.GetallCourseType;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CourseTypeModel>(procedure, commandType: CommandType.StoredProcedure);
                    lstCourseType = _mapper.Map<List<CourseTypeModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All College Type : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCourseType;
        }
        public async Task<List<LookUpTypeModel>> GetAllLookupType(string type, string level)
        {
            List<LookUpTypeModel> lstCourseType = new List<LookUpTypeModel>();
            try
            {
                var procedure = Procedure.GetallLookupTypeByType;
                var val = new { LookupType = type, level = level };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<LookUpTypeModel>(procedure, val, commandType: CommandType.StoredProcedure);
                    lstCourseType = _mapper.Map<List<LookUpTypeModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All College Type : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCourseType;
        }
        public async Task<List<ReservationTypeModel>> GetReservationCategorys(string RegId)
        {
            List<ReservationTypeModel> lstQuotaType = new List<ReservationTypeModel>();
            try
            {
                var values = new
                {
                    RegId = RegId
                };
                var procedure = Procedure.GetReservationCategorys;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<ReservationTypeModel>(procedure, values, commandType: CommandType.StoredProcedure);
                    lstQuotaType = _mapper.Map<List<ReservationTypeModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Reservation Categorys : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstQuotaType;
        }

        public async Task<List<ReservationTypeModel>> GetReservationCategorysPG(string RegId)
        {
            List<ReservationTypeModel> lstQuotaType = new List<ReservationTypeModel>();
            try
            {
                var values = new
                {
                    RegId = RegId
                };
                var procedure = Procedure.GetReservationCategorysPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<ReservationTypeModel>(procedure, values, commandType: CommandType.StoredProcedure);
                    lstQuotaType = _mapper.Map<List<ReservationTypeModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Reservation Categorys PG : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstQuotaType;
        }
        ServiceResult datar = new ServiceResult();

        public async Task<ServiceResult> testconnection()
        {
            try
            {
                
            datar.ResultData = "con open";


                string re;
              
                SqlConnection con = new SqlConnection("server=dev.sql.psegs.in,6001; Initial Catalog=AdmissionPortal;User ID=schooladmin;Password=schooladmin@123;Integrated Security=false;Max Pool Size=50000;");
                con.Open();

                SqlCommand cmd = new SqlCommand("App_Get_All_CourseType", con);
                    DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                datar.ResultData = ds.Tables[0];
                    
              using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            //{
            //        datar.ResultData = "con open1"+connection.ConnectionString;

            //        re = connection.ConnectionString;
            //}
               // datar.ResultData = "con openx3";

                //datar.ResultData = re;

            return await Task.Run(() => datar);
            }
            catch (Exception e)
            {
                datar.ResultData = e;
                return await Task.Run(() => datar);

            }
        }

        public Task<int> UpdateAsync(LookupAPIModel entity)
        {
            throw new NotImplementedException();
        }


        public async Task<List<ReservationTypeModel>> GetOfflineAdmissionReservationCategorys()
        {
            List<ReservationTypeModel> lstQuotaType = new List<ReservationTypeModel>();
            try
            {
               
                var procedure = Procedure.GetReservationCategorysOfflineAdmission;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<ReservationTypeModel>(procedure, commandType: CommandType.StoredProcedure);
                    lstQuotaType = _mapper.Map<List<ReservationTypeModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Reservation Categorys : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstQuotaType;
        }
    }
}
