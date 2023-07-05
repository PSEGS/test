using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.College;
using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Domain.ApiModel.DownloadDocument;
using AdmissionPortal_API.Domain.ApiModel.Student;
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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        string _refImage;

        public CollegeRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public async Task<int> AddAsync(CollegeMaster entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<CollegeMaster>(entity);
                var procedure = Procedure.SaveCollege;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("add College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<ServiceResult> AddColleges(AddCollege addCollege)
        {
            ServiceResult _result = new ServiceResult();
            try
            {
                var values = _mapper.Map<AddCollege>(addCollege);
                var procedure = Procedure.SaveCollege;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<ServiceResult>(procedure, values, commandType: CommandType.StoredProcedure);
                    _result = _mapper.Map<ServiceResult>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("add College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _result;
        }

        public async Task<int> DeleteAsync(int id, int userid)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeletecollegebyId;
                var values = new { College_Id = id, UserID = userid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete college : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<string> GenerateOTP(int CollegeId, string otp)
        {
            string result = "0";
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var procedure = Procedure.CollegeGenreateOTP;
                var values = new { CollegeId = CollegeId, otp = otp };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<ServiceResult>(procedure, values, commandType: CommandType.StoredProcedure);
                    serviceResult = _mapper.Map<ServiceResult>(obj.FirstOrDefault());
                    if (serviceResult.StatusCode == 200)
                    {
                        result = serviceResult.ResultData;
                    }
                    else
                    {
                        result = "0";

                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Generate OTP : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public Task<GetCollege> DeleteCollegeById(int id, int userid)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetCollege>> GetAllCollege(int? universityid, int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<GetCollege> lstCollege = new List<GetCollege>();
            try
            {
                var procedure = Procedure.GetallCollege;
                var value = new { universityid = universityid, PageNumber = pageNumber, PageSize = pageSize, SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GetCollege>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollege = _mapper.Map<List<GetCollege>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Colleges : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollege;
        }

        public Task<CollegeMaster> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetCollege> GetCollegesById(int id)
        {
            GetCollege collegedetail = new GetCollege();
            try
            {
                var procedure = Procedure.CollegeGetByID;
                var value = new
                {
                    College_Id = id
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetCollege>(procedure, value, commandType: CommandType.StoredProcedure);
                    collegedetail = _mapper.Map<GetCollege>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return collegedetail;
        }



        public async Task<int> Updatecollege(UpdateCollege entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();

                //var values = _mapper.Map<CollegeMaster>(entity);
                var procedure = Procedure.Updatecollege;
                DataTable table = new DataTable();
                List<Domain.ApiModel.College.BankDetails> banklist = new List<Domain.ApiModel.College.BankDetails>();
                
                entity.BankDetails.Regular_Course.CourseFundTypeId = 77;
                entity.BankDetails.Self_Financed_Course.CourseFundTypeId = 78;
                banklist.Add(entity.BankDetails.Regular_Course);
                banklist.Add(entity.BankDetails.Self_Financed_Course);

                table = lst.ToDataTable<Domain.ApiModel.College.BankDetails>(banklist);
                var value = new
                {
                    CollegeId = entity.CollegeId,
                    UniversityId = entity.UniversityId,
                    CollegeName = entity.CollegeName,
                    CollegeTypeId = entity.CollegeTypeId,
                    CollegeWebsite = entity.CollegeWebsite,
                    CollegeEmail = entity.CollegeEmail,
                    CGType = entity.CGType,
                    CollegeContact = entity.CollegeContact,
                    CollegeAddress = entity.CollegeAddress,
                    DistrictId = entity.DistrictId,
                    EducationMode = entity.EducationMode,
                    NameofPrincipal = entity.NameofPrincipal,
                    PrincipalPhone = entity.PrincipalPhone,
                    NodalOfficer = entity.NodalOfficer,
                    NodelOfficerPhone = entity.NodelOfficerPhone,
                    NodalOfficerEmail = entity.NodalOfficerEmail,
                    CoOrdinatorArtsStreamName = entity.CoOrdinatorArtsStreamName,
                    CoOrdinatorArtsStreamPhone = entity.CoOrdinatorArtsStreamPhone,
                    CoOrdinatorCommerceStreamName = entity.CoOrdinatorCommerceStreamName,
                    CoOrdinatorCommerceStreamPhone = entity.CoOrdinatorCommerceStreamPhone,
                    CoOrdinatorScienceStreamName = entity.CoOrdinatorScienceStreamName,
                    CoOrdinatorScienceStreamPhone = entity.CoOrdinatorScienceStreamPhone,
                    CoOrdinatorJobOrientedCourseName = entity.CoOrdinatorJobOrientedCourseName,
                    CoOrdinatorJobOrientedCoursePhone = entity.CoOrdinatorJobOrientedCoursePhone,
                    CoOrdinatorFeeStructureName = entity.CoOrdinatorFeeStructureName,
                    CoOrdinatorFeeStructurePhone = entity.CoOrdinatorFeeStructurePhone,
                    ShortName = entity.ShortName,
                    CreatedBy = entity.CreatedBy,
                    Password = entity.Password,
                    BankDetails = table

                };
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        async Task<int> ICollegeRepository.DeleteCollegeById(int id, int userid)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeletecollegebyId;
                var values = new { College_Id = id, UserID = userid };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete college : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<GetCollegeByDistrict>> GetCollegeByDistrictId(int districtId, int collegeTypeId, int admissionId, string type, string ugpg)
        {
            List<GetCollegeByDistrict> lstCollegedetail = new List<GetCollegeByDistrict>();
            try
            {
                var procedure = Procedure.GetCollegeByDistrictId;
                var value = new
                {
                    DistrictId = districtId,
                    CollegeTypeId = collegeTypeId,
                    AdmissionId = admissionId,
                    Type = type,
                    UgPg = ugpg
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetCollegeByDistrict>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<GetCollegeByDistrict>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public async Task<List<CollegeCoursesListing>> GetCollegeCourses(int collegeId, string CGtype)
        {
            List<CollegeCoursesListing> lstCollegedetail = new List<CollegeCoursesListing>();
            try
            {
                var procedure = Procedure.GetCollegeCourses;
                var value = new
                {
                    CollegeId = collegeId,
                    CGtype = CGtype
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<CollegeCoursesListing>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<CollegeCoursesListing>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College courses : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }
        public async Task<List<GetAllCollege>> GetAllColleges()
        {
            List<GetAllCollege> lstCollegedetail = new List<GetAllCollege>();
            try
            {
                var procedure = Procedure.GetAllColleges;
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetAllCollege>(procedure, null, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<GetAllCollege>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }
        public async Task<CollegeActiveInActiveResponse> ActiveInActive(int CollegeId, int Status, int modifiyBY)
        {
            CollegeActiveInActiveResponse result =new CollegeActiveInActiveResponse();
            try
            {
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifiyBY };

                var procedure = Procedure.CollegeActiveInactive;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                   var obj = await connection.QueryAsync<CollegeActiveInActiveResponse>(procedure, value, commandType: CommandType.StoredProcedure);
                    result = _mapper.Map<CollegeActiveInActiveResponse>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Active DeActive College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> lockunlockCollegeInfo(int CollegeId, int Status, int modifyBy, string otp)
        {
            int result = 0;
            try
            {
                var value = new { College_Id = CollegeId, Status = Status, ModifyBy = modifyBy, otp = otp };

                var procedure = Procedure.CollegeLockUnlock;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Active DeActive College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<ServiceResult> ResetCollegePassword(int CollegeId, int modifiyBY)
        {

            ServiceResult _result = new ServiceResult();
            try
            {
                string password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                var value = new { College_Id = CollegeId, Password = password, ModifyBy = modifiyBY };
                var procedure = Procedure.ResetPasswordByAdmin;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<ServiceResult>(procedure, value, commandType: CommandType.StoredProcedure);
                    _result = _mapper.Map<ServiceResult>(obj.FirstOrDefault());
                    _result.ResultData = _configuration["Passwords:DefaultPassword"];
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Reset College Password : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _result;
        }

        public async Task<int> updateCollegeNew(UpdateCollegeModel updateCollege)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<UpdateCollegeModel>(updateCollege);
                var procedure = Procedure.CollegeupdateBycollegeId;
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

        public Task<int> UpdateAsync(CollegeMaster entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UploadProspectus(UploadCollegeProspectus model)
        {
            int result = 0;
            try
            {


                //var values = _mapper.Map<CollegeMaster>(entity);
                var procedure = Procedure.UploadCollegeProspectus;

                var value = new
                {
                    CollegeId = model.CollegeID,
                    prospectusRef = model.prospectusRef

                };
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Upload Prospectus  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<int> UploadCancelledCheque(UploadCancelledChequeModel model)
        {
            int result = 0;
            try
            {

                var procedure = Procedure.UploadCancelledCheque;

                var value = new
                {
                    CollegeId = model.CollegeID,
                    CourseFundType = model.CourseFundType,
                    CancelledCheque = model.CancelledCheque


                };
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Upload Cancelled Cheque  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<GetCollegeByDistrict>> GetDistrictCollege(int districtId, int collegeTypeId, int admissionId, string type)
        {
            List<GetCollegeByDistrict> lstCollegedetail = new List<GetCollegeByDistrict>();
            try
            {
                var procedure = Procedure.GetDistrictCollege;
                var value = new
                {
                    DistrictId = districtId,
                    CollegeTypeId = collegeTypeId,
                    AdmissionId = admissionId,
                    Type = type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetCollegeByDistrict>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<GetCollegeByDistrict>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By District Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public async Task<List<GetCollegeByDistrict>> GetDistrictCollegesByGender(int districtId, int collegeTypeId, int admissionId, int studentId, string type, string ugpg)
        {
            List<GetCollegeByDistrict> lstCollegedetail = new List<GetCollegeByDistrict>();
            try
            {
                var procedure = Procedure.GetDistrictCollegesByGender;
                var value = new
                {
                    DistrictId = districtId,
                    CollegeTypeId = collegeTypeId,
                    AdmissionId = admissionId,
                    StudentId = studentId,
                    Type = type,
                    UgPg = ugpg
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetCollegeByDistrict>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<GetCollegeByDistrict>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("GetDistrictColleges By Gender : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public async Task<List<GetAllCollege>> GetCollegeByCGtype(string type)
        {
            List<GetAllCollege> lstCollegedetail = new List<GetAllCollege>();
            try
            {
                var procedure = Procedure.GetCollegeByCGtype;
                var value = new
                {
                    Type = type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetAllCollege>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<GetAllCollege>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get college By cg type : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }
        public async Task<string> ReportsLogin(int UserId, string token)
        {
            try
            {
                var procedure = Procedure.AppLoginReportProc;
                var value = new
                {
                    UserId = UserId,
                    Token = token,
                    ExpiryDateTime = DateTime.Now.AddMinutes(10)
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("get Reports Login : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return "OK";
        }
        public async Task<int> UnlockStudent(UnlockStudentModel model)
        {
            int result = 0;
            try
            {

                var procedure = Procedure.UnLockStudent;

                var value = new
                {
                    RegistrationNo = model.RegistrationId,
                    Remarks = model.Remarks,
                    Type = model.CourseType,
                    CollegeId = model.CollegeID


                };
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("UnLock Student by College  : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<StudentDetailsForCancellation> GetStudentDetailsByRegId(string RegId, string type, int collegeId)
        {
            StudentDetailsForCancellation studentMaster = new StudentDetailsForCancellation();
            try
            {
                var procedure = "";
                if (type.ToLower() == "ug")
                {
                    procedure = Procedure.StudentDetailsForCancellation;
                }
                else
                {
                    procedure = Procedure.StudentDetailsForCancellationPG;
                }
                var value = new
                {
                    RegId = RegId,
                    collegeId = collegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var multi = await con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster.studentDetail = multi.Read<BasicDetailsForCancellation>().ToList();
                    studentMaster.meritStatus = multi.Read<MeritStatus>().ToList();
                    studentMaster.admissionStatus = multi.Read<AdmissionStatusPostMerit>().ToList();
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get ug Student BY RegId for cancellation:", "REGID:" + RegId + ", " + ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentMaster;
        }

        public async Task<CancelAdmissionSeatModel> CancelStudentAdmissionSeat(CancelAdmissionSeat model)
        {
            CancelAdmissionSeatModel result = new CancelAdmissionSeatModel();
            try
            {
                var procedure = "";
                if (model.type.ToLower() == "ug")
                {
                    procedure = Procedure.StudentCancelAdmissionSeat;
                }
                else
                {
                    procedure = Procedure.StudentCancelAdmissionSeatPG;
                }
                var value = new
                {
                    RegId = model.RegId,
                    collegeId = model.collegeId,
                    DocReference = model.DocReference,
                    Remarks = model.Remarks
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = await con.QuerySingleAsync<CancelAdmissionSeatModel>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Student seat cancellation:", "REGID:" + model.RegId + ", " + ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<DownloadDocument>> GetStudentDocumentDownload(string CollegeID, string type)
        {
            List<DownloadDocument> lstCollegedetail = new List<DownloadDocument>();
            try
            {
                var procedure = Procedure.DownloadStudentDocument;
                var value = new
                {
                    collegeID= CollegeID,
                    type = type
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<DownloadDocument>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstCollegedetail = _mapper.Map<List<DownloadDocument>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("GetStudentDocumentDownload : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstCollegedetail;
        }

        public async Task<string> GetCollegeProspectus(string CollegeID)
        {
            ServiceResult _serviceResult = new ServiceResult();

            try
            {
                var procedure = Procedure.DownloadcollegeProspectus;
                var value = new
                {
                    College_Id = CollegeID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    _refImage =Convert.ToString(await con.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                    
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By Id : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _refImage;
        }

        public async Task<List<AllCollegesISLOCK>> GetCollegesIslock(int Admissiontype)
        {

            List<AllCollegesISLOCK> lstSeat = new List<AllCollegesISLOCK>();
            try
            {
                var procedure = Procedure.GetAllCollegeIslock;
                var value = new { AdmissionType = Admissiontype };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await connection.QueryAsync<AllCollegesISLOCK>(procedure, value, commandType: CommandType.StoredProcedure);
                    lstSeat = _mapper.Map<List<AllCollegesISLOCK>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All PG Colleges Course Seat Matrix: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstSeat;
        }

    }

    
}
