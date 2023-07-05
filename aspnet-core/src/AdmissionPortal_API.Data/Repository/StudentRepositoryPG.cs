using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Student;
using AdmissionPortal_API.Domain.ApiModel.StudentPG;
using AdmissionPortal_API.Domain.Model;
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
    public class StudentRepositoryPG : IStudentRepositoryPG
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public StudentRepositoryPG(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<StudentRegisterResponseModel> AddAsync(RegisterStudent entity)
        {
            int result = 0;
            StudentRegisterResponseModel studentRegisterResponseModel = new StudentRegisterResponseModel();
            try
            {
                var procedure = Procedure.SaveStudentPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await (connection.QuerySingleAsync<StudentRegisterResponseModel>(procedure, entity, commandType: CommandType.StoredProcedure));
                    studentRegisterResponseModel = _mapper.Map<StudentRegisterResponseModel>(obj);
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Register Student PG : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentRegisterResponseModel;
        }

        public async Task<StudentLoginMaster> GetAsync(StudentPGLogin entity)
        {
            StudentLoginMaster studentLogin = new StudentLoginMaster();
            try
            {
                string procedure = Procedure.StudentLoginPG;
                if (!String.IsNullOrEmpty(entity.UserPassword))
                {
                    entity.UserPassword = Encryption.EncodeToBase64(entity.UserPassword);
                }
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await (connection.QuerySingleAsync<StudentLoginMaster>(procedure, entity, commandType: CommandType.StoredProcedure));
                    studentLogin = _mapper.Map<StudentLoginMaster>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Student Login PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentLogin;
        }
        public async Task<int> UpdateStudentPersonalDetails(UpdatePersonalDetails model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateStudentPersonalDetailsPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Student Personal Details PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UpdateBankDetails(UpdateBankDetails model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateBankDetailsPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Bank Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UpdateAddressDetails(UpdateAddressDetails model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateAddressDetailsPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Address Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UpdateAcademicDetails(UpdateAcademicDetailsPG model)
        {
            int result = 0;

            try
            {
                ListToDataTableExtension lstSubject = new ListToDataTableExtension();
                var procedure = Procedure.UpdateAcademicDetailsPG;

                var value = new
                {
                    StudentId = model.StudentId,
                    TwelvethRollno = model.TwelvethRollno,
                    TwelvethYearOfPassing = model.TwelvethYearOfPassing,
                    TwelvethExamination = model.TwelvethExamination,
                    TwelvethBoardUniversity = model.TwelvethBoardUniversity,
                    TwelvethBoardUniversityID = model.TwelvethBoardUniversityID,
                    TwelvethSchoolCollegeName = model.TwelvethSchoolCollegeName,
                    TwelvethStreamID = model.TwelvethStreamID,
                    TwelvethResultStatus = model.TwelvethResultStatus,
                    TwelvethTotalMarks = model.TwelvethTotalMarks,
                    TwelvethObtainedMarks = model.TwelvethObtainedMarks,
                    TwelvethPercentage = model.TwelvethPercentage,
                    TwelvethCGPA = model.TwelvethCGPA,

                    graduationRollno = model.graduationRollno,
                    graduationYearOfPassing = model.graduationYearOfPassing,
                    graduationExamination = model.graduationExamination,
                    graduationBoardUniversity = model.graduationUniversity,
                    UniversityID = model.UniversityID,
                    graduationCollegeName = model.graduationCollegeName,
                    graduationCourseID = model.graduationCourseID,
                    graduationResultStatus = model.graduationResultStatus,
                    graduationTotalMarks = model.graduationTotalMarks,
                    graduationObtainedMarks = model.graduationObtainedMarks,
                    graduationPercentage = model.graduationPercentage,
                    graduationCGPA = model.graduationCGPA,

                    Rollno = model.Rollno,
                    YearOfPassing = model.YearOfPassing,
                    Examination = model.Examination,
                    BoardUniversity = model.BoardUniversity,
                    BoardUniversityID = model.BoardUniversityID,
                    SchoolCollegeName = model.SchoolCollegeName,
                    ResultStatus = model.ResultStatus,
                    TotalMarks = model.TotalMarks,
                    ObtainedMarks = model.ObtainedMarks,
                    Percentage = model.Percentage,
                    CGPA = model.CGPA
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Academic Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UpdateWeightage(WeightagePG model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateWeightagePG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Weightage PG: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UploadDocuments(UploadDocumentsPG model)
        {
            int result = 0;
            List<DocumentDetailPG> lstDedtail = new List<DocumentDetailPG>();
            try
            {
                DocumentDetailPG obj;
                obj = new DocumentDetailPG();
                obj.DocumentType = Convert.ToString(EnumDocumentType.Tenth);
                obj.DocumentReference = model.TenthCertificateReference;
                lstDedtail.Add(obj);
                obj = new DocumentDetailPG();
                obj.DocumentType = Convert.ToString(EnumDocumentType.Twelth);
                obj.DocumentReference = model.TwelvethCertificateReference;
                lstDedtail.Add(obj);
                obj = new DocumentDetailPG();
                obj.DocumentType = Convert.ToString(EnumDocumentType.Photo);
                obj.DocumentReference = model.PhotoReference;
                lstDedtail.Add(obj);
                obj = new DocumentDetailPG();
                obj.DocumentType = Convert.ToString(EnumDocumentType.Signature);
                obj.DocumentReference = model.SignatureReference;
                lstDedtail.Add(obj);
                if (!String.IsNullOrEmpty(model.CharacterReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Character);
                    obj.DocumentReference = model.CharacterReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.RuralReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Rural);
                    obj.DocumentReference = model.RuralReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.NCCReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.NCC);
                    obj.DocumentReference = model.NCCReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.NSSReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.NSS);
                    obj.DocumentReference = model.NSSReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.YouthWelfareReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.YouthWelfare);
                    obj.DocumentReference = model.YouthWelfareReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.SCReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.SC);
                    obj.DocumentReference = model.SCReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.BCReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.BC);
                    obj.DocumentReference = model.BCReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.IncomeReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Income);
                    obj.DocumentReference = model.IncomeReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.graduationReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Graduation);
                    obj.DocumentReference = model.graduationReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.ESMReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.ESM);
                    obj.DocumentReference = model.ESMReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.PWDReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.PWD);
                    obj.DocumentReference = model.PWDReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.freedomFighterReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.FreedomFighter);
                    obj.DocumentReference = model.freedomFighterReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.SportsReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Sports);
                    obj.DocumentReference = model.SportsReference;
                    lstDedtail.Add(obj);
                }
                //if (!String.IsNullOrEmpty(model.physicalDisabilityReference))
                //{
                //    obj = new DocumentDetailPG();
                //    obj.DocumentType = Convert.ToString(EnumDocumentType.PhysicalDisability);
                //    obj.DocumentReference = model.physicalDisabilityReference;
                //    lstDedtail.Add(obj);
                //}
                if (!String.IsNullOrEmpty(model.kashmiriMigrantReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.KashmiriMigrant);
                    obj.DocumentReference = model.kashmiriMigrantReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.ResidenceReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Residence);
                    obj.DocumentReference = model.ResidenceReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.MigrationReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Migration);
                    obj.DocumentReference = model.MigrationReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.SchoolLeavingReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.SchoolLeaving);
                    obj.DocumentReference = model.SchoolLeavingReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.BorderAreaCertificateReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.BorderArea);
                    obj.DocumentReference = model.BorderAreaCertificateReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.NriPassportReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.NriPassport);
                    obj.DocumentReference = model.NriPassportReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.PatientProofReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.PatientProof);
                    obj.DocumentReference = model.PatientProofReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.AdvanceYouthLeadershiptrainingcampReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.AdvanceYouthLeadershipTrainingCamp);
                    obj.DocumentReference = model.AdvanceYouthLeadershiptrainingcampReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.YouthLeadershipTrainingCampReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.YouthLeadershipTrainingCamp);
                    obj.DocumentReference = model.YouthLeadershipTrainingCampReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.AdvancedMountaineeringReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.AdvancedMountaineering);
                    obj.DocumentReference = model.AdvancedMountaineeringReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.HikingTrainingReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.HikingTraining);
                    obj.DocumentReference = model.HikingTrainingReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.MountaineeringReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Mountaineering);
                    obj.DocumentReference = model.MountaineeringReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.ZonalYouthFestivalReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.ZonalYouthFestival);
                    obj.DocumentReference = model.ZonalYouthFestivalReference;
                    lstDedtail.Add(obj);
                }

                if (!String.IsNullOrEmpty(model.UniversityLevelYouthFestivalReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.UniversityLevelYouthFestival);
                    obj.DocumentReference = model.UniversityLevelYouthFestivalReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.InterUniversityNationalYouthFestivalReference))
                {
                    obj = new DocumentDetailPG();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.InterUniversityNationalYouthFestival);
                    obj.DocumentReference = model.InterUniversityNationalYouthFestivalReference;
                    lstDedtail.Add(obj);
                }

                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.UploadDocumentsPG;
                DataTable table = new DataTable();
                table = lst.ToDataTable<DocumentDetailPG>(lstDedtail);
                var value = new
                {
                    StudentId = model.StudentId,
                    TenthSerialNumber = model.TenthSerialNumber,
                    TwelvethSerialNumber = model.TwelvethSerialNumber,
                    ResidenceSerialNumber = model.ResidenceSerialNumber,
                    IncomeSerialNumber = model.IncomeSerialNumber,
                    borderAreaCertificateSerialNumber = model.BorderAreaCertificateSerialNumber,
                    DocumentDetail = table
                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = await connection.ExecuteScalarAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Upload Documents PG", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<StudentMasterPG> GetStudentById(int studentID)
        {
            StudentMasterPG studentMaster = new StudentMasterPG();
            try
            {
                var procedure = Procedure.GetStudentByIdPG;
                var value = new
                {
                    StudentID = studentID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QuerySingleAsync<StudentMasterPG>(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster = _mapper.Map<StudentMasterPG>(obj);
                    studentMaster.DateOfBirth = studentMaster.Date_Of_Birth.ToString("dd/MM/yyyy");
                    studentMaster.Passport_Expiry_Date = studentMaster.PassportExpiryDate?.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Student By Id:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return studentMaster;
        }

        public async Task<List<GetStudentAcademicPG>> GetSubjectByStudentId(int studentID)
        {
            List<GetStudentAcademicPG> studentMaster = new List<GetStudentAcademicPG>();
            try
            {
                var procedure = Procedure.GetSubjectByStudentIdPG;
                var value = new
                {
                    StudentID = studentID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetStudentAcademicPG>(procedure, value, commandType: CommandType.StoredProcedure);
                    var tenthDetails = obj.Where(x => x.Examination == "Diploma" && obj.Any(y => y.Examination == "Twelveth"));
                    var twelvethDetails = obj.Where(x => x.Examination == "Twelveth" || x.Examination == "Diploma").OrderByDescending(x => x.Examination);
                    var graduationDetails = obj.Where(x => x.Examination == "Graduation");
                    var data = graduationDetails?.GroupBy(x => new
                    {
                        x.QualificationId,
                        x.StudentId,
                        x.Examination,
                        x.BoardUniversity,
                        x.BoardUniversityID,
                        x.SchoolCollegeName,
                        x.StreamID,
                        x.StreamName,
                        x.ResultStatus,
                        x.TotalMarks,
                        x.ObtainedMarks,
                        x.Percentage,
                        x.CGPA,
                        x.YearOfPassing,
                        x.Rollno,
                        x.BoardUniversityName,
                        x.Result
                    }).Select(x => new GetStudentAcademicPG
                    {
                        StudentId = x.Key.StudentId,
                        TwelvethQualificationId = x.Key.QualificationId,
                        TwelvethBoardUniversity = x.Key.BoardUniversity,
                        TwelvethBoardUniversityID = x.Key.BoardUniversityID,
                        TwelvethBoardUniversityName = x.Key.BoardUniversityName,
                        TwelvethExamination = x.Key.Examination,
                        TwelvethPercentage = x.Key.Percentage,
                        TwelvethObtainedMarks = x.Key.ObtainedMarks,
                        TwelvethCGPA = x.Key.CGPA,
                        TwelvethResultStatus = x.Key.ResultStatus,
                        TwelvethResult = x.Key.Result,
                        TwelvethSchoolCollegeName = x.Key.SchoolCollegeName,
                        TwelvethStreamID = x.Key.StreamID,
                        TwelvethStreamName = x.Key.StreamName,
                        TwelvethTotalMarks = x.Key.TotalMarks,
                        TwelvethYearOfPassing = x.Key.YearOfPassing,
                        TwelvethRollNo = x.Key.Rollno,
                        QualificationId = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().QualificationId : 0,
                        Examination = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Examination : null,
                        BoardUniversity = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversity : null,
                        BoardUniversityID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityID : null,
                        CGPA = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().CGPA : null,
                        ObtainedMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().ObtainedMarks : null,
                        Percentage = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Percentage : 0,
                        ResultStatus = x.Key.ResultStatus,
                        Result = x.Key.Result,
                        SchoolCollegeName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().SchoolCollegeName : null,
                        StreamID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamID : 0,
                        StreamName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamName : null,
                        TotalMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().TotalMarks : null,
                        YearOfPassing = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().YearOfPassing : null,
                        Rollno = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Rollno : null,
                        BoardUniversityName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityName : null,

                        graduationQualificationId = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().QualificationId : 0,
                        UniversityID = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().BoardUniversityID : null,
                        graduationUniversityName = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().BoardUniversityName : null,
                        graduationExamination = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().Examination : null,
                        graduationPercentage = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().Percentage : 0,
                        graduationObtainedMarks = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().ObtainedMarks : null,
                        graduationCGPA = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().CGPA : null,
                        graduationResultStatus = x.Key.ResultStatus,
                        graduationResult = x.Key.Result,
                        graduationCollegeName = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().SchoolCollegeName : null,
                        graduationCourseName = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().graduationCourseName : null,
                        graduationTotalMarks = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().TotalMarks : null,
                        graduationYearOfPassing = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().YearOfPassing : null,
                        graduationRollNo = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().Rollno : null,
                        graduationCourseID = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().graduationCourseID : null,
                        graduationCollege = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().graduationCollege : null,

                    }).ToList();
                    studentMaster = _mapper.Map<List<GetStudentAcademicPG>>(data);

                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Subject By Student Id:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return studentMaster;
        }
        public async Task<List<GetStudentAcademicPG>> GetSubjectByRegId(string RegId)
        {
            List<GetStudentAcademicPG> studentMaster = new List<GetStudentAcademicPG>();
            try
            {
                var procedure = Procedure.GetSubjectByRegIdPG;
                var value = new
                {
                    RegID = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetStudentAcademicPG>(procedure, value, commandType: CommandType.StoredProcedure);
                    var tenthDetails = obj.Where(x => x.Examination == "Diploma" && obj.Any(y => y.Examination == "Twelveth"));
                    var twelvethDetails = obj.Where(x => x.Examination == "Twelveth" || x.Examination == "Diploma").OrderByDescending(x => x.Examination);
                    var graduationDetails = obj.Where(x => x.Examination == "Graduation");
                    var data = graduationDetails?.GroupBy(x => new
                    {
                        x.QualificationId,
                        x.StudentId,
                        x.Examination,
                        x.BoardUniversity,
                        x.BoardUniversityID,
                        x.SchoolCollegeName,
                        x.StreamID,
                        x.StreamName,
                        x.ResultStatus,
                        x.TotalMarks,
                        x.ObtainedMarks,
                        x.Percentage,
                        x.CGPA,
                        x.YearOfPassing,
                        x.Rollno,
                        x.BoardUniversityName,
                        x.Result
                    }).Select(x => new GetStudentAcademicPG
                    {
                        graduationQualificationId = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().QualificationId : 0,
                        UniversityID = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().BoardUniversityID : null,
                        graduationUniversityName = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().BoardUniversityName : null,
                        graduationExamination = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().Examination : null,
                        graduationPercentage = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().Percentage : 0,
                        graduationObtainedMarks = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().ObtainedMarks : null,
                        graduationCGPA = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().CGPA : null,
                        graduationResultStatus = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().ResultStatus : null,
                        graduationResult = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().Result : null,
                        graduationCollegeName = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().SchoolCollegeName : null,
                        graduationCourseName = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().graduationCourseName : null,
                        graduationTotalMarks = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().TotalMarks : null,
                        graduationYearOfPassing = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().YearOfPassing : null,
                        graduationRollNo = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().Rollno : null,
                        graduationCourseID = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().graduationCourseID : null,
                        graduationCollege = graduationDetails.FirstOrDefault() != null ? graduationDetails.FirstOrDefault().graduationCollege : null,

                    }).ToList();
                    studentMaster = _mapper.Map<List<GetStudentAcademicPG>>(data);

                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Subject By Student Id:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return studentMaster;
        }
        public async Task<StudentDetails> GetStudentDetailsByRegId(string RegId)
        {
            StudentDetails studentMaster = new StudentDetails();
            try
            {
                var procedure = Procedure.StudentDetailsPG;
                var value = new
                {
                    RegId = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var multi = await con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster.studentDetail = multi.Read<BasicDetails>().ToList();
                    studentMaster.meritStatus = multi.Read<MeritStatus>().ToList();
                    studentMaster.admissionStatus = multi.Read<AdmissionStatusPostMerit>().ToList();
                    studentMaster.actionHistory = multi.Read<ActionHistory>().ToList();
                    studentMaster.verificationDetails = multi.Read<StudentVerificationDetails>().FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get pg StudentDetailsByRegId:", "regid:"+RegId+", "+ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentMaster;
        }
        public async Task<List<SubjectCombinationDetail>> GetCourseChoiceByStudentId(int studentID)
        {
            List<SubjectCombinationDetail> studentMaster = new List<SubjectCombinationDetail>();
            try
            {
                var procedure = Procedure.GetCourseChoiceByStudentIdPG;
                var value = new
                {
                    StudentID = studentID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<SubjectCombinationDetail>(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster = _mapper.Map<List<SubjectCombinationDetail>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get pg course choice By Student Id:", "studentId:" + studentID + ", " + ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentMaster;
        }
        public async Task<ProgressBar> GetProgressBar(int studentID)
        {
            ProgressBar obj = new ProgressBar();
            try
            {
                var procedure = Procedure.GetProgressBarPG;
                var value = new
                {
                    StudentID = studentID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    obj = await con.QuerySingleAsync<ProgressBar>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Progress Bar:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return obj;
        }

        public async Task<int> UpdateDeclarations(Declaration model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateDeclarationsPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Declarations: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<string> GenerateOTP(string userName, string otp)
        {
            string result = string.Empty;
            try
            {
                var procedure = Procedure.GenerateOTPPG;
                var value = new
                {
                    UserName = userName,
                    OTP = otp
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = await con.ExecuteScalarAsync<string>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Generate OTP:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return result;
        }

        public async Task<int> UpdateCourseChoice(CourseChoice model)
        {
            int result = 0;

            try
            {

                ListToDataTableExtension lst = new ListToDataTableExtension();
                DataTable table = new DataTable();
                table = lst.ToDataTable<SubjectCombination>(model.SubjectCombinations);
                var values = new
                {
                    StudentId = model.StudentId,
                    SubjectCombinations = table
                };
                var procedure = Procedure.UpdateCourseChoicePG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Course Choice: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<UploadedDocumentDetailPG> GetUploadedDocuments(int studentID)
        {
            List<GetUploadedDocumentPG> uploadDocuments = new List<GetUploadedDocumentPG>();
            UploadedDocumentDetailPG documentDetail = new UploadedDocumentDetailPG();
            try
            {
                var procedure = Procedure.GetDocumentsByStudentIdPG;
                var value = new
                {
                    StudentID = studentID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetUploadedDocumentPG>(procedure, value, commandType: CommandType.StoredProcedure);
                    uploadDocuments = _mapper.Map<List<GetUploadedDocumentPG>>(obj);
                    if (uploadDocuments.Count > 0)
                    {
                        documentDetail.TenthSerialNumber = uploadDocuments.FirstOrDefault().TenthSerialNumber;
                        documentDetail.TwelvethSerialNumber = uploadDocuments.FirstOrDefault().TwelvethSerialNumber;
                        documentDetail.ResidenceSerialNumber = uploadDocuments.FirstOrDefault().ResidenceSerialNumber;
                        documentDetail.IncomeSerialNumber = uploadDocuments.FirstOrDefault().IncomeSerialNumber;
                        documentDetail.BorderAreaCertificateSerialNumber = uploadDocuments.FirstOrDefault().BorderAreaCertificateSerialNumber;
                        documentDetail.isFromBorderArea = uploadDocuments.FirstOrDefault().isFromBorderArea;
                        documentDetail.casteSerialNumber = uploadDocuments.FirstOrDefault().casteSerialNumber;
                        documentDetail.isResidenceSerialNumberVerified = uploadDocuments.FirstOrDefault().isResidenceSerialNumberVerified;
                        documentDetail.isIncomeSerialNumberVerified = uploadDocuments.FirstOrDefault().isIncomeSerialNumberVerified;
                        documentDetail.isCasteSerialNumberVerified = uploadDocuments.FirstOrDefault().isCasteSerialNumberVerified;
                        documentDetail.isPhysicalDisable = uploadDocuments.FirstOrDefault().isPhysicalDisable;
                        documentDetail.ReservationCategoryName = uploadDocuments.FirstOrDefault().ReservationCategoryName;
                        documentDetail.isNCC = uploadDocuments.FirstOrDefault().isNCC;
                        documentDetail.isNSS = uploadDocuments.FirstOrDefault().isNSS;
                        documentDetail.isBcSerialNumberVerified = uploadDocuments.FirstOrDefault().isBcSerialNumberVerified;
                        documentDetail.bcSerialNumber = uploadDocuments.FirstOrDefault().bcSerialNumber;
                        documentDetail.isRural = uploadDocuments.FirstOrDefault().IsRural;

                        documentDetail.TenthCertificate = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Tenth)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.TwelvethCertificate = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Twelth)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Photo = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Photo)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Signature = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Signature)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.NCC = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.NCC)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.NSS = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.NSS)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.YouthWelfare = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.YouthWelfare)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.BC = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.BC)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.SC = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.SC)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Income = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Income)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Residence = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Residence)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Migration = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Migration)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.SchoolLeaving = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.SchoolLeaving)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Graduation = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Graduation)).Select(x => x.DocumentReference).FirstOrDefault();
                        //documentDetail.physicalDisability = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.PhysicalDisability)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.FreedomFighter = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.FreedomFighter)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.PWD = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.PWD)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.ESM = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.ESM)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Sports = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Sports)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.kashmiriMigrant = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.KashmiriMigrant)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.BorderAreaCertificate = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.BorderArea)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.NriPassport = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.NriPassport)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.PatientProof = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.PatientProof)).Select(x => x.DocumentReference).FirstOrDefault();
                        //AdvanceYouthLeadershiptrainingcamp
                        documentDetail.AdvanceYouthLeadershipTrainingCamp = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.AdvanceYouthLeadershipTrainingCamp)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.YouthLeadershipTrainingCamp = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.YouthLeadershipTrainingCamp)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.AdvancedMountaineering = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.AdvancedMountaineering)).Select(x => x.DocumentReference).FirstOrDefault();

                        documentDetail.HikingTraining = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.HikingTraining)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Mountaineering = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Mountaineering)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.ZonalYouthFestival = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.ZonalYouthFestival)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.UniversityLevelYouthFestival = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.UniversityLevelYouthFestival)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.InterUniversityNationalYouthFestival = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.InterUniversityNationalYouthFestival)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Character = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Character)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Rural = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Rural)).Select(x => x.DocumentReference).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Uploaded Documents:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return documentDetail;
        }

        public async Task<int> RegisterationFeesPayment(RegisterationFees model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.SaveRegisterationFeesPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Registeration Fees Payment : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UnlockForm(string oTP, int studentId)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UnlockFormPG;
                var value = new
                {
                    OTP = oTP,
                    StudentId = studentId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = await con.ExecuteScalarAsync<int>(procedure, value, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Unlock Form:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return result;
        }

        public async Task<int> UpdateStudentDetails(UpdateStudentDetails model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateStudentDetailsPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Student Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<ForgotLoginMaster> ForgotPassword(string email)
        {
            ForgotLoginMaster detail = new ForgotLoginMaster();
            try
            {
                var procedure = Procedure.StudentForgotPasswordPG;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    ForgotPassword forgot = new ForgotPassword();
                    forgot.Email = email;
                    var obj = await (connection.QuerySingleAsync<ForgotLoginMaster>(procedure, forgot, commandType: CommandType.StoredProcedure));
                    detail = _mapper.Map<ForgotLoginMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("ForgotPassword : ", "pg mail:" + email + ", error: " + ex.Message, ex.HResult, ex.StackTrace);
                detail.response = "0";
            }
            return detail;
        }
        public async Task<StudentMaster> GetStudentAppDetailsByRegId(FilterStudent _filter)
        {
            StudentMaster studentMaster = new StudentMaster();
            try
            {
                var procedure = Procedure.StudentAppDetailsPG;
                var value = new
                {
                    RegId = _filter.regId,
                    MobileNumber = _filter.mobileNumber,
                    RollNumber = _filter.rollNumber,
                    Year = _filter.year,
                    BoardId = _filter.boardId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QuerySingleAsync<StudentMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster = _mapper.Map<StudentMaster>(obj);
                    studentMaster.DateOfBirth = studentMaster.Date_Of_Birth.ToString("dd/MM/yyyy");
                    studentMaster.Passport_Expiry_Date = studentMaster.PassportExpiryDate;
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Student By Reg Id:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return studentMaster;
        }
        public async Task<List<SubjectCombinationDetail>> GetCourseChoiceByRegId(string RegId)
        {
            List<SubjectCombinationDetail> studentMaster = new List<SubjectCombinationDetail>();
            try
            {
                var procedure = Procedure.GetCourseChoiceByRegIdPG;
                var value = new
                {
                    RegId = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<SubjectCombinationDetail>(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster = _mapper.Map<List<SubjectCombinationDetail>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get course choice By Reg Id:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentMaster;
        }

        public async Task<UploadedDocumentDetailPG> GetDocumentsByRegId(string RegId)
        {
            List<GetUploadedDocumentPG> uploadDocuments = new List<GetUploadedDocumentPG>();
            UploadedDocumentDetailPG documentDetail = new UploadedDocumentDetailPG();
            try
            {
                var procedure = Procedure.GetDocumentsByRegIdPG;
                var value = new
                {
                    RegId = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetUploadedDocumentPG>(procedure, value, commandType: CommandType.StoredProcedure);
                    uploadDocuments = _mapper.Map<List<GetUploadedDocumentPG>>(obj);
                    if (uploadDocuments.Count > 0)
                    {
                        documentDetail.TenthSerialNumber = uploadDocuments.FirstOrDefault().TenthSerialNumber;
                        documentDetail.TwelvethSerialNumber = uploadDocuments.FirstOrDefault().TwelvethSerialNumber;
                        documentDetail.ResidenceSerialNumber = uploadDocuments.FirstOrDefault().ResidenceSerialNumber;
                        documentDetail.IncomeSerialNumber = uploadDocuments.FirstOrDefault().IncomeSerialNumber;
                        documentDetail.BorderAreaCertificateSerialNumber = uploadDocuments.FirstOrDefault().BorderAreaCertificateSerialNumber;
                        documentDetail.isFromBorderArea = uploadDocuments.FirstOrDefault().isFromBorderArea;
                        documentDetail.casteSerialNumber = uploadDocuments.FirstOrDefault().casteSerialNumber;
                        documentDetail.isResidenceSerialNumberVerified = uploadDocuments.FirstOrDefault().isResidenceSerialNumberVerified;
                        documentDetail.isIncomeSerialNumberVerified = uploadDocuments.FirstOrDefault().isIncomeSerialNumberVerified;
                        documentDetail.isCasteSerialNumberVerified = uploadDocuments.FirstOrDefault().isCasteSerialNumberVerified;
                        documentDetail.isPhysicalDisable = uploadDocuments.FirstOrDefault().isPhysicalDisable;
                        documentDetail.ReservationCategoryName = uploadDocuments.FirstOrDefault().ReservationCategoryName;
                        documentDetail.isNCC = uploadDocuments.FirstOrDefault().isNCC;
                        documentDetail.isNSS = uploadDocuments.FirstOrDefault().isNSS;
                        documentDetail.isBcSerialNumberVerified = uploadDocuments.FirstOrDefault().isBcSerialNumberVerified;
                        documentDetail.bcSerialNumber = uploadDocuments.FirstOrDefault().bcSerialNumber;

                        documentDetail.TenthCertificate = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Tenth)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.TwelvethCertificate = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Twelth)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Photo = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Photo)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Signature = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Signature)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.NCC = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.NCC)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.NSS = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.NSS)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.YouthWelfare = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.YouthWelfare)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.BC = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.BC)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.SC = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.SC)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Income = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Income)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Residence = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Residence)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Migration = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Migration)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.SchoolLeaving = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.SchoolLeaving)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Graduation = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Graduation)).Select(x => x.DocumentReference).FirstOrDefault();
                        //documentDetail.physicalDisability = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.PhysicalDisability)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.FreedomFighter = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.FreedomFighter)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.PWD = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.PWD)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.ESM = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.ESM)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Sports = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Sports)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.kashmiriMigrant = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.KashmiriMigrant)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.BorderAreaCertificate = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.BorderArea)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.NriPassport = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.NriPassport)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.PatientProof = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.PatientProof)).Select(x => x.DocumentReference).FirstOrDefault();
                        //AdvanceYouthLeadershiptrainingcamp
                        documentDetail.AdvanceYouthLeadershipTrainingCamp = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.AdvanceYouthLeadershipTrainingCamp)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.YouthLeadershipTrainingCamp = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.YouthLeadershipTrainingCamp)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.AdvancedMountaineering = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.AdvancedMountaineering)).Select(x => x.DocumentReference).FirstOrDefault();

                        documentDetail.HikingTraining = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.HikingTraining)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Mountaineering = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Mountaineering)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.ZonalYouthFestival = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.ZonalYouthFestival)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.UniversityLevelYouthFestival = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.UniversityLevelYouthFestival)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.InterUniversityNationalYouthFestival = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.InterUniversityNationalYouthFestival)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Character = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Character)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Rural = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Rural)).Select(x => x.DocumentReference).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Uploaded Documents:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return documentDetail;
        }

        public async Task<List<ObjectionListByStudentModel>> GetObjectionsByPGStudentID(int studentid)
        {
            List<ObjectionListByStudentModel> objectionslist = new List<ObjectionListByStudentModel>();
            try
            {
                var procedure = Procedure.GetObjectionsBystudentId;
                var value = new
                {
                    StudentId = studentid
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<ObjectionListByStudentModel>(procedure, value, commandType: CommandType.StoredProcedure);
                    objectionslist = _mapper.Map<List<ObjectionListByStudentModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Objections by Student ID:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return objectionslist;
        }

        public async Task<StudentSeatBookedOffered> GetAdmissionSeatDetails(int StudentId)
        {
            StudentSeatBookedOffered list = new StudentSeatBookedOffered();
            try
            {
                var procedure = Procedure.GetAdmissionSeatDetailsByStudentIdPG;
                var value = new
                {
                    StudentId = StudentId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    list = obj.Read<StudentSeatBookedOffered>().FirstOrDefault();
                    list.Course = obj.Read<OfferedCourses>().ToList();
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Admission Seat Details By Student Id:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return list;

        }

        public async Task<AdmissionSeatPaymentReciept> GetAdmissionFeeReceipt(int StudentId, string transactionId)
        {
            AdmissionSeatPaymentReciept list = new AdmissionSeatPaymentReciept();
            try
            {
                var procedure = Procedure.GetStudentReceiptByStudentIdPG;
                var value = new
                {
                    StudentId = StudentId,
                    transactionId = transactionId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await con.QuerySingleAsync<AdmissionSeatPaymentReciept>(procedure, value, commandType: CommandType.StoredProcedure);
                    list = _mapper.Map<AdmissionSeatPaymentReciept>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Admission receipt By Student Id:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return list;

        }
        public async Task<List<AdmissionSeatPaymentRecieptList>> GetAdmissionFeeReceiptList(int StudentId)
        {
            List<AdmissionSeatPaymentRecieptList> list = new List<AdmissionSeatPaymentRecieptList>();
            try
            {
                var procedure = Procedure.GetStudentReceiptListByStudentIdPG;
                var value = new
                {
                    StudentId = StudentId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await con.QueryAsync<AdmissionSeatPaymentRecieptList>(procedure, value, commandType: CommandType.StoredProcedure);
                    list = _mapper.Map<List<AdmissionSeatPaymentRecieptList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Admission receipt By Student Id:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return list;

        }
    }
}
