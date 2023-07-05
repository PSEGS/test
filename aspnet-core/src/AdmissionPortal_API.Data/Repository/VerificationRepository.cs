using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ApiModel.Objection;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.CommonEnam;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.VerificationSectionEnum;

namespace AdmissionPortal_API.Data.Repository
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        
        public VerificationRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }

        public async Task<StudentDetails> GetVerifiedStudentByRegIdCollege(string RegId, int CollegeId)
        {
            StudentDetails studentMaster = new StudentDetails();
            try
            {
                var procedure = Procedure.GetVerifiedStudentByRegIdCollege;
                var value = new
                {
                    RegId = RegId,
                    CollegeId = CollegeId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    using (var multi = con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure).Result)
                    {
                        studentMaster.studentDetail = multi.Read<BasicDetails>().ToList();
                        studentMaster.meritStatus = multi.Read<MeritStatus>().ToList();
                        studentMaster.admissionStatus = multi.Read<AdmissionStatusPostMerit>().ToList();
                        studentMaster.actionHistory = multi.Read<ActionHistory>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Verified Student By RegId:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentMaster;
        }

        public async Task<List<BasicDetailsNew>> GetStudentsByCollege(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<BasicDetailsNew> basicDetails = new List<BasicDetailsNew>();
            try
            {
                var procedure = Procedure.GetStudentsByCollege;
                var value = new
                {
                    CollegeId = collegeId,
                    VerificationStatus = verificationStatus,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    searchKeyword = searchKeyword,
                    sortBy = sortBy,
                    sortOrder = sortOrder
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<BasicDetailsNew>(procedure, value, commandType: CommandType.StoredProcedure);
                    basicDetails = _mapper.Map<List<BasicDetailsNew>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return basicDetails;
        }

        public async Task<ServiceResult> VerifyStudentWithSection(VerifyStudentWithSection verifyStudentWithSection)
        {
            ServiceResult _result = new ServiceResult();
            try
            {
                var values = _mapper.Map<VerifyStudentWithSection>(verifyStudentWithSection);
                var procedure = Procedure.VerifyStudentWithSection;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync<ServiceResult>(procedure, values, commandType: CommandType.StoredProcedure);
                    _result = _mapper.Map<ServiceResult>(obj.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Verify Student With Section : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return _result;
        }

        public async Task<int> RevokeStudentVerification(CancelStudentRegistration model)
        {
            int result = 0;
            try
            {
                string procedure = Procedure.RevokeStudentVerification;
                var values = new { RegistrationNo = model.RegId, Remarks = model.Remarks, Type = model.RegType, UserId = model.UserId };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    //addcourse = _mapper.Map<AddCourse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Revoke UG Student Verification : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<dynamic> GetStudentByRegId(string RegId,Int32 type)
        {
            StudentDetailsVerification studentMaster = new StudentDetailsVerification();
            BasicDetailsVerification basicDetailsVerification = new BasicDetailsVerification();
            AcademicResult AcademicDetails = new AcademicResult();
            List<Weightageforverification> WeightageforVerification = new List<Weightageforverification>();
            List<ReservationDetails> ReservationDetails = new List<ReservationDetails>();
            List<ActionHistoryVerification> ActionHistory = new List<ActionHistoryVerification>();
            dynamic result=null;
            try
            {
                var procedure = Procedure.GetStudentByRegId;
                var value = new
                {
                    RegId = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    using (var multi = con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure).Result)
                    {
                     
                            switch (type)
                            {
                                case (int)VerificationSectionType.PersonalDetails:
                                result = multi.Read<BasicDetailsVerification>().FirstOrDefault();
                                break;
                                case (int)(VerificationSectionType.AcademicDetails):
                                basicDetailsVerification = multi.Read<BasicDetailsVerification>().FirstOrDefault();

                                AcademicDetails.AcademicDetails = multi.Read<Academicdetails>().FirstOrDefault();
                                AcademicDetails.AcademicSubjects = multi.Read<AcademicSubject>().ToList();
                                AcademicDetails.ChoiceOfCourseforVerifications = multi.Read<ChoiceOfCourseforverification>().ToList();
                                result = AcademicDetails;
                                break;
                                case (int)(VerificationSectionType.Weightages):
                                basicDetailsVerification = multi.Read<BasicDetailsVerification>().FirstOrDefault();

                                AcademicDetails.AcademicDetails = multi.Read<Academicdetails>().FirstOrDefault();
                                AcademicDetails.AcademicSubjects = multi.Read<AcademicSubject>().ToList();
                                AcademicDetails.ChoiceOfCourseforVerifications = multi.Read<ChoiceOfCourseforverification>().ToList();
                                WeightageforVerification = multi.Read<Weightageforverification>().ToList();
                                result = WeightageforVerification;
                                break;
                                case (int)(VerificationSectionType.ReservationDetails):
                                basicDetailsVerification = multi.Read<BasicDetailsVerification>().FirstOrDefault();

                                AcademicDetails.AcademicDetails = multi.Read<Academicdetails>().FirstOrDefault();
                                AcademicDetails.AcademicSubjects = multi.Read<AcademicSubject>().ToList();
                                AcademicDetails.ChoiceOfCourseforVerifications = multi.Read<ChoiceOfCourseforverification>().ToList();
                                WeightageforVerification = multi.Read<Weightageforverification>().ToList();
                                ReservationDetails = multi.Read<ReservationDetails>().ToList();
                                result = ReservationDetails;
                                break;
                                case (int)(VerificationSectionType.ActionHistory):
                                basicDetailsVerification = multi.Read<BasicDetailsVerification>().FirstOrDefault();

                                AcademicDetails.AcademicDetails = multi.Read<Academicdetails>().FirstOrDefault();
                                AcademicDetails.AcademicSubjects = multi.Read<AcademicSubject>().ToList();
                                AcademicDetails.ChoiceOfCourseforVerifications = multi.Read<ChoiceOfCourseforverification>().ToList();
                                WeightageforVerification = multi.Read<Weightageforverification>().ToList();
                                ReservationDetails = multi.Read<ReservationDetails>().ToList();
                                ActionHistory = multi.Read<ActionHistoryVerification>().ToList();
                                result = ActionHistory;
                                break;
                            default:
                                break;

                        }


                      
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Student By RegId:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> StudentCourseEligible(StudentChoiceofCourseEligible model)
        {
            int result = 0; 
            try
            {
                string procedure = Procedure.VerificationStudentCourseChoice;
                var values = new { 
                    RegistrationNo = model.RegistrationNumber,
                    CourseAction = model.CourseAction,
                    Remarks = model.Remarks,
                    ActionTaken = model.ActionTaken,
                    CourseId=model.CourseId
                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    //addcourse = _mapper.Map<AddCourse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Revoke UG Student Verification : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<ReturnModelVerification> FinalVerification(FinalVerificationModel model)
        {
            ReturnModelVerification returnmodel = new ReturnModelVerification();

            try
            {
                string procedure = Procedure.VerificationFInalSubmit;
                var values = new
                {
                    RegistrationNumber = model.RegistrationNumber,
                    VerifiedBy = model.verifedBy,
                    CollegeId = model.CollegeID
                    
                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    ServiceResult serviceResult = new ServiceResult();
                    var obj = await connection.QueryMultipleAsync(procedure, values, commandType: CommandType.StoredProcedure);
                    returnmodel.verificationReturnModel = obj.Read<VerificationReturnModel>().FirstOrDefault();
                    returnmodel.objectremarks = obj.Read<objectremarks>().ToList();
                    
                     
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Final Verification Verification : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return returnmodel;
        }

        public async Task<List<BasicDetailsNew>> ExportExcelStudentsByCollege(int collegeId, int? verificationStatus)
        {
            List<BasicDetailsNew> basicDetails = new List<BasicDetailsNew>();
            try
            {
                var procedure = Procedure.GetStudentsByCollege;
                var value = new
                {
                    CollegeId = collegeId,
                    VerificationStatus = verificationStatus,
                    pageNumber = 1,
                    pageSize = 500000,
                    searchKeyword = "",
                    sortBy = "registrationId",
                    sortOrder = "asc"
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryAsync<BasicDetailsNew>(procedure, value, commandType: CommandType.StoredProcedure);
                    basicDetails = _mapper.Map<List<BasicDetailsNew>>(obj);
                    

                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return basicDetails;
        }

        public async Task<List<BasicDetailsNew>> GetStudentsByCollegeBCOM(Int32 collegeId, Int32? verificationStatus, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<BasicDetailsNew> basicDetails = new List<BasicDetailsNew>();
            try
            {
                var procedure = Procedure.GetStudentsByCollegeBCOM;
                var value = new
                {
                    CollegeId = collegeId,
                    VerificationStatus = verificationStatus,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    searchKeyword = searchKeyword,
                    sortBy = sortBy,
                    sortOrder = sortOrder
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<BasicDetailsNew>(procedure, value, commandType: CommandType.StoredProcedure);
                    basicDetails = _mapper.Map<List<BasicDetailsNew>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return basicDetails;
        }
        public async Task<dynamic> GetStudentByRegIdBCOM(string RegId, Int32 type)
        {
            StudentDetailsVerification studentMaster = new StudentDetailsVerification();
            BasicDetailsVerification basicDetailsVerification = new BasicDetailsVerification();
            AcademicResult AcademicDetails = new AcademicResult();
            List<Weightageforverification> WeightageforVerification = new List<Weightageforverification>();
            List<ReservationDetails> ReservationDetails = new List<ReservationDetails>();
            List<ActionHistoryVerification> ActionHistory = new List<ActionHistoryVerification>();
            dynamic result = null;
            try
            {
                var procedure = Procedure.GetStudentByRegIdBCOM;
                var value = new
                {
                    RegId = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    using (var multi = con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure).Result)
                    {

                        switch (type)
                        {
                            case (int)VerificationSectionType.PersonalDetails:
                                result = multi.Read<BasicDetailsVerification>().FirstOrDefault();
                                break;
                            case (int)(VerificationSectionType.AcademicDetails):
                                basicDetailsVerification = multi.Read<BasicDetailsVerification>().FirstOrDefault();

                                AcademicDetails.AcademicDetails = multi.Read<Academicdetails>().FirstOrDefault();
                                AcademicDetails.AcademicSubjects = multi.Read<AcademicSubject>().ToList();
                                AcademicDetails.ChoiceOfCourseforVerifications = multi.Read<ChoiceOfCourseforverification>().ToList();
                                result = AcademicDetails;
                                break;
                            case (int)(VerificationSectionType.Weightages):
                                basicDetailsVerification = multi.Read<BasicDetailsVerification>().FirstOrDefault();

                                AcademicDetails.AcademicDetails = multi.Read<Academicdetails>().FirstOrDefault();
                                AcademicDetails.AcademicSubjects = multi.Read<AcademicSubject>().ToList();
                                AcademicDetails.ChoiceOfCourseforVerifications = multi.Read<ChoiceOfCourseforverification>().ToList();
                                WeightageforVerification = multi.Read<Weightageforverification>().ToList();
                                result = WeightageforVerification;
                                break;
                            case (int)(VerificationSectionType.ReservationDetails):
                                basicDetailsVerification = multi.Read<BasicDetailsVerification>().FirstOrDefault();

                                AcademicDetails.AcademicDetails = multi.Read<Academicdetails>().FirstOrDefault();
                                AcademicDetails.AcademicSubjects = multi.Read<AcademicSubject>().ToList();
                                AcademicDetails.ChoiceOfCourseforVerifications = multi.Read<ChoiceOfCourseforverification>().ToList();
                                WeightageforVerification = multi.Read<Weightageforverification>().ToList();
                                ReservationDetails = multi.Read<ReservationDetails>().ToList();
                                result = ReservationDetails;
                                break;
                            case (int)(VerificationSectionType.ActionHistory):
                                basicDetailsVerification = multi.Read<BasicDetailsVerification>().FirstOrDefault();

                                AcademicDetails.AcademicDetails = multi.Read<Academicdetails>().FirstOrDefault();
                                AcademicDetails.AcademicSubjects = multi.Read<AcademicSubject>().ToList();
                                AcademicDetails.ChoiceOfCourseforVerifications = multi.Read<ChoiceOfCourseforverification>().ToList();
                                WeightageforVerification = multi.Read<Weightageforverification>().ToList();
                                ReservationDetails = multi.Read<ReservationDetails>().ToList();
                                ActionHistory = multi.Read<ActionHistoryVerification>().ToList();
                                result = ActionHistory;
                                break;
                            default:
                                break;

                        }



                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Student By RegId:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> SAVEBCOMStudentWeightage(BCOMStudentWeightage model)
        {
            int result = 0;
            try
            {
                string procedure = Procedure.BCOMStudentWeightage;
                var values = new
                {
                    RegistrationNo = model.RegistrationNumber,
                    CollegeID = model.CollegeID,
                    weightage = model.weightage 
                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    //addcourse = _mapper.Map<AddCourse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("weightage save for BCOM student : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<BasicDetailsNew>> ExportExcelStudentsByCollegeBCOM(int collegeId, int? verificationStatus)
        {
            List<BasicDetailsNew> basicDetails = new List<BasicDetailsNew>();
            try
            {
                var procedure = Procedure.GetStudentsByCollegeBCOM;
                var value = new
                {
                    CollegeId = collegeId,
                    VerificationStatus = verificationStatus,
                    pageNumber = 1,
                    pageSize = 500000,
                    searchKeyword = "",
                    sortBy = "registrationId",
                    sortOrder = "asc"
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<BasicDetailsNew>(procedure, value, commandType: CommandType.StoredProcedure);
                    basicDetails = _mapper.Map<List<BasicDetailsNew>>(obj);


                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Students By College: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return basicDetails;
        }
        public async Task<List<StudentCombinationDetails>> StudentCombinationDetails(Int32 CollegeID, Int32 CourseId, string RegId)
        {
            List<StudentCombinationDetails> studentCombinationSubject = new List<StudentCombinationDetails>();
            try
            {
                var procedure = Procedure.studentCourseCombination;
                var value = new
                {
                    CourseId = CourseId,
                    CollegeID = CollegeID,
                    RegId = RegId
                    
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<StudentCombinationDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    studentCombinationSubject = _mapper.Map<List<StudentCombinationDetails>>(obj);


                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Students By CollegeCombination Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentCombinationSubject;
        }
    }
}
