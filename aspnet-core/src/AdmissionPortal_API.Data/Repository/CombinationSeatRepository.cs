using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
 
using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
 
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
    public class CombinationSeatRepository: ICombinationWiseSeat
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public CombinationSeatRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<int> AddAsyncCourse(CombinationSeat entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.AddCombinationSeat;
                DataTable table = new DataTable();
                table = lst.ToDataTable<CombinationListforSeat>(entity.combinationListforSeats);
                var value = new
                {
                    CollegeId=entity.CollegeId,
                    CourseId=entity.CourseID,
                    CombinationSeat = table,
                    CreatedBy=entity.createdBy
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Add  Combination Seat : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<CombinationSeatDetails>> GetCourseById(int collegeId, int courseId)
        {
            List<CombinationSeatDetails> lstCombinationSeat = new List<CombinationSeatDetails>();
            try
            {
                var procedure = Procedure.GetCombinationSeat;
                var value = new
                {
                    
                    CollegeID = collegeId,
                    CourseID = courseId 
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<CombinationSeatDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCombinationSeat = _mapper.Map<List<CombinationSeatDetails>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Combination with Seat: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCombinationSeat;
        }
    }
}
