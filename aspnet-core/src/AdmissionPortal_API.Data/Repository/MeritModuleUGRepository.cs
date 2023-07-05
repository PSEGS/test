using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace AdmissionPortal_API.Data.Repository
{
    public class MeritModuleUGRepository : IMeritModuleUGRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public MeritModuleUGRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<List<ProvisionalList>> GetProvisionalList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            List<ProvisionalList> lst = new List<ProvisionalList>();
            try
            {
                string procedure = Procedure.ProvisionalMeritListUG;
                var value = new { CollegeId = CollegeId, CourseId = CourseId, ReservationId = ReservationId, CategoryId = CategoryId, PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await (connection.QueryAsync<ProvisionalList>(procedure, value, commandType: CommandType.StoredProcedure));
                    lst = _mapper.Map<List<ProvisionalList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Provisional List UG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lst;
        }
        public async Task<List<WaitingList>> GetWaitingList(Int32 CollegeId, Int32 CourseId, Int32 ReservationId, Int32 CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            List<WaitingList> lst = new List<WaitingList>();
            try
            {
                string procedure = Procedure.WaitingListUG;
                var value = new
                {
                    CollegeId = CollegeId,
                    CourseId = CourseId,
                    ReservationId = ReservationId,
                    CategoryId = CategoryId,
                    IsBorderArea = IsBorderArea,
                    OnlyGirlChild = SingleGirlChild,
                    CancerAidsThalassemia = CancerAidsThalassemia,
                    NRI = NRI,
                    IsKashmiriMigrant = IsKashmiriMigrant,
                    RuralArea = RuralArea,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchKeyword = searchKeyword,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<WaitingList>(procedure, value, commandType: CommandType.StoredProcedure);
                    lst = _mapper.Map<List<WaitingList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Waiting List UG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lst;
        }
        public async Task<dynamic> GetCourseFeeByStudentId(int CourseId, int CollegeId, int StudentId, string Combination)
        {
            StudentFeeDetails studentFeeDetails = new StudentFeeDetails();
            try
            {

                var procedure = Procedure.GetUGCourseFeeByStudentID;

                var value = new
                {
                    CourseId = CourseId,
                    CollegeId = CollegeId,
                    StudentId = StudentId,
                    CombinationSet = Combination


                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var multi = await connection.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);

                    studentFeeDetails.studentFeeHeadModels = multi.Read<studentFeeHeadModel>().ToList();
                    studentFeeDetails.studentCombinationFees = multi.Read<CombinationStudentFee>().ToList();
                    studentFeeDetails.studentCourse = multi.Read<StudentCourseFee>().FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Course Fee ByStudentId  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentFeeDetails;
        }

        public async Task<SaveAdmissionSeatModel> SaveAdmissionSeat(AdmissionSeat model)
        {
            SaveAdmissionSeatModel result = new SaveAdmissionSeatModel();
            try
            {
                string procedure = string.Empty;
                if (model.AdmissionType == "Merit")
                {
                    procedure = Procedure.SaveAdmissionSeatUG;
                }
                else
                {
                    procedure = Procedure.SaveAdmissionSeatUGForWaiting;

                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = await connection.QuerySingleAsync<SaveAdmissionSeatModel>(procedure, model, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("save admission seat: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<MeritModule> GetAdmissionSeatStatus(string RegId, string CollegeId,string AdmissionType)
        {
            MeritModule model = new MeritModule();
            try
            {
                string procedure = string.Empty;
                if(AdmissionType == "Merit")
                {
                    procedure = Procedure.GetAdmissionSeatStatusUG;

                }
                else
                {
                    procedure = Procedure.GetAdmissionSeatStatusUGforwaitinglist;

                }
                var value = new
                {
                    RegId = RegId.Trim(),
                    CollegeId = CollegeId
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    model = obj.Read<MeritModule>().FirstOrDefault();
                    model.Course = obj.Read<MeritCourse>().ToList();
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get admission seat: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return model;
        }

        public async Task<List<Combination>> GetCourseChoiceByRegId(string RegId, Int32 CourseId, Int32 CollegeId)
        {
            List<Combination> model = new List<Combination>();
            try
            {
                var procedure = Procedure.GetStudentCourseChoiceByRegIdUG;
                var value = new
                {
                    RegId = RegId,
                    CollegeId = CollegeId,
                    CourseId = CourseId
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<Combination>(procedure, value, commandType: CommandType.StoredProcedure);
                    model = _mapper.Map<List<Combination>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Course Choice By RegId: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return model;
        }

        public async Task<AdmissionSeatPaymentReciept> GetFeeReceiptByRegId(string RegId, Int32 CollegeId)
        {
            AdmissionSeatPaymentReciept model = new AdmissionSeatPaymentReciept();
            try
            {
                var procedure = Procedure.GetStudentReceiptByRegIdUG;
                var value = new
                {
                    RegId = RegId,
                    CollegeId = CollegeId
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QuerySingleAsync<AdmissionSeatPaymentReciept>(procedure, value, commandType: CommandType.StoredProcedure);
                    model = _mapper.Map<AdmissionSeatPaymentReciept>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get recceipt By RegId: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return model;
        }

        public async Task<List<CategoryCombination>> GetCategoryCombination()
        {
            List<CategoryCombination> model = new List<CategoryCombination>();
            try
            {
                var procedure = Procedure.GetCategoryCombinationMaster;

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CategoryCombination>(procedure, commandType: CommandType.StoredProcedure);
                    model = _mapper.Map<List<CategoryCombination>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Category Combination: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return model;
        }

        public async Task<List<ProvisionalList>> ExportMeritExcel(int collegeId, int? courseId, int? ReservationId, int? CategoryId, string searchKeyword)
        {
            List<ProvisionalList> basicDetails = new List<ProvisionalList>();
            try
            {
                var procedure = Procedure.ProvisionalMeritListUG;
                var value = new
                {
                    CollegeId = collegeId,
                    courseId = courseId,
                    ReservationId = ReservationId,
                    CategoryId = CategoryId,
                    pageNumber = 1,
                    pageSize = 10000,
                    searchKeyword = searchKeyword,
                    sortBy = "",
                    sortOrder = "asc"
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<ProvisionalList>(procedure, value, commandType: CommandType.StoredProcedure);
                    basicDetails = _mapper.Map<List<ProvisionalList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Merit Excel ug: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return basicDetails;
        }

        public async Task<OTPModel> SendOTP(OTPRequestModel model, string studentType, string OTP)
        {
            OTPModel result = new OTPModel();
            try
            {
                var procedure = Procedure.SaveToken;
                var value = new
                {
                    StudentType = studentType,
                    StudentReferenceNumber = model.StudentReferenceNumber,
                    CourseId = model.CourseId,
                    CollageId = model.CollegeId,
                    OTPToken = OTP
                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = await connection.QuerySingleAsync<OTPModel>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Send OTP", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;


        }



        public async Task<int> VerifyOTP(VerifyOTP model, string studentType)
        {
            int result = 0;
            try
            {
                string procedure = string.Empty;
                if (model.AdmissionType == "Merit")
                {
                    procedure = Procedure.VerifyToken;

                }
                else
                {
                    procedure = Procedure.VerifyWaitingToken;
                }


                var value = new
                {
                    StudentType = studentType,
                    StudentReferenceNumber = model.StudentReferenceNumber,
                    OTPToken = model.OTP,
                    CollageId = model.CollegeId,
                    CourseId = model.CourseId,
                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = await connection.QuerySingleAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Verify OTP", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;


        }

        public async Task<List<CourseSubjectCount>> CollegeCourseSubjectCount(string CollegeId, string CourseId)
        {
            List<CourseSubjectCount> model = new List<CourseSubjectCount>();
            try
            {
                var procedure = Procedure.CollegeCourseSubjectCount;
                var value = new
                {
                    collegeId = CollegeId,
                    CourseId = CourseId
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<CourseSubjectCount>(procedure, value, commandType: CommandType.StoredProcedure);
                    model = _mapper.Map<List<CourseSubjectCount>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("College Course Subject Count: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return model;
        }

        public async Task<List<AdmissionSeatPaymentRecieptList>> GetStudentFeeReceiptList(string RegId, int CollegeId)
        {
            List<AdmissionSeatPaymentRecieptList> list = new List<AdmissionSeatPaymentRecieptList>();
            try
            {
                var procedure = Procedure.StudentFeeReceiptList;
                var value = new
                {
                    RegId = RegId,
                    CollegeId = CollegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await con.QueryAsync<AdmissionSeatPaymentRecieptList>(procedure, value, commandType: CommandType.StoredProcedure);
                    list = _mapper.Map<List<AdmissionSeatPaymentRecieptList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Admission receipt List By Reg Id:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return list;

        }

        public async Task<RevokeAdmissionSeatModel> RevokeAdmissionSeat(RevokeSeat model)
        {
            RevokeAdmissionSeatModel result = new RevokeAdmissionSeatModel();
            try
            {
                var procedure = "";
                if (model.AdmissionType == "Merit")
                {
                    procedure = Procedure.RevokeSeatUG;
                }
                else
                {
                    procedure = Procedure.RevokeSeatUGForWaiting;

                }
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = await connection.QuerySingleAsync<RevokeAdmissionSeatModel>(procedure, model, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Revoke admission seat: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<WaitingList>> ExportWaitingExcel(int collegeId, int CourseId, int ReservationId, int CategoryId, bool IsBorderArea, bool SingleGirlChild, bool CancerAidsThalassemia, bool NRI, bool IsKashmiriMigrant, bool RuralArea, string searchKeyword)
        {
            List<WaitingList> basicDetails = new List<WaitingList>();
            try
            {
                var procedure = Procedure.WaitingListUG;
                var value = new
                {
                    CollegeId = collegeId,
                    courseId = CourseId,
                    ReservationId = ReservationId,
                    CategoryId = CategoryId,
                    IsBorderArea = IsBorderArea,
                    OnlyGirlChild = SingleGirlChild,
                    CancerAidsThalassemia = CancerAidsThalassemia,
                    IsKashmiriMigrant = IsKashmiriMigrant,
                    NRI = NRI,
                    RuralArea = RuralArea,
                    pageNumber = 1,
                    pageSize = 10000,
                    searchKeyword = searchKeyword,
                    sortBy = "",
                    sortOrder = "asc"
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<WaitingList>(procedure, value, commandType: CommandType.StoredProcedure);
                    basicDetails = _mapper.Map<List<WaitingList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Merit Excel ug: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return basicDetails;
        }

        public async Task<VacantSeatForWaitingList> GetVacantSeatMatrixByCategory(int? collegeId, int courseId, int combinationId)
        {
            VacantSeatForWaitingList list = new VacantSeatForWaitingList();
            try
            {
                var procedure = Procedure.GetVacantSeatsByCategory;
                var value = new
                {
                    CollegeId = collegeId,
                    CourseId = courseId,
                    combinationId = combinationId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await con.QueryAsync<VacantSeatForWaitingList>(procedure, value, commandType: CommandType.StoredProcedure);
                    list = _mapper.Map<VacantSeatForWaitingList>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Vacant Seat Matrix By Category:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return list;

        }

        public async Task<int> UpdateBookedSeatMatrix(UpdateBookedSeatMatrix model)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UpdateBookedSeatMatrix;

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Booked Seat Matrix: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<GetvacantSeatList>> GetVacantSeatByCollege(int CollegeId)
        {
            List<GetvacantSeatList> list = new List<GetvacantSeatList>();
            try
            {
                var procedure = Procedure.GetVacantSeat;
                var value = new
                {

                    CollegeID = CollegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await con.QueryAsync<GetvacantSeatList>(procedure, value, commandType: CommandType.StoredProcedure);
                    list = _mapper.Map<List<GetvacantSeatList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Vacant Seat By College:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return list;

        }

    }
}
