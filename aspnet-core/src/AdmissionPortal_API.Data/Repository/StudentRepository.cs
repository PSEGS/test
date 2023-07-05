using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Student;
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
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly ILogError _logError;
        public StudentRepository(IMapper mapper, IConfiguration configuration, ILogError logError, IHostingEnvironment hostingEnvironment)
        {
            _logError = logError;
            _mapper = mapper;
            _HostEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<StudentRegisterResponseModel> AddAsync(AddStudent entity)
        {
            int result = 0;
            StudentRegisterResponseModel studentRegisterResponseModel = new StudentRegisterResponseModel();
            try
            {
                List<Subjects> lstSubjects = new List<Subjects>();
                Subjects obj = new Subjects();
                if (entity.subject != null)
                {
                    foreach (var l in entity.subject)
                    {
                        if (!string.IsNullOrEmpty(l.name))
                        {
                            obj = new Subjects();
                            obj.SubjectName = l.name;
                            obj.MaxNumber = String.IsNullOrEmpty(l.marksMax) ? 0 : Convert.ToInt32(l.marksMax);
                            obj.ObtainedNumber = String.IsNullOrEmpty(l.marksTotal) ? 0 : Convert.ToInt32(l.marksTotal);

                            lstSubjects.Add(obj);
                        }
                    }
                }
                var procedure = Procedure.SaveStudent;
                ListToDataTableExtension lstSubjectTable = new ListToDataTableExtension();
                DataTable tableSubject = new DataTable();
                tableSubject = lstSubjectTable.ToDataTable<Subjects>(lstSubjects);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var values = new
                    {
                        Roll = entity.number,
                        Name = entity.name,
                        Fname = entity.fatherName,
                        Mname = entity.motherName,
                        Dob = entity.Dob,
                        SchoolName = entity.schoolName,
                        TotalObtainedMarks = entity.marksTotal,
                        TotalMax = entity.Totmax,
                        ResultStatus = entity.result,
                        Board = entity.Board,
                        BoardName = entity.BoardName,
                        PassingYear = entity.PassingYear,
                        Mobile = entity.Mobile,
                        Email = entity.Email,
                        Gender = entity.Gender,
                        ReservationCategory = entity.ReservationCategory,
                        CasteCategory = entity.CasteCategory,
                        Caste = entity.Caste,
                        Nationality = entity.Nationality,
                        AadhaarNumber = entity.AadhaarNumber,
                        //KashmiriMigrant = entity.KashmiriMigrant,
                        PunjabResident = entity.PunjabResident,
                        PunjabiInMetric = entity.PunjabiInMetric,
                        TwelvethFromPunjab = entity.TwelvethFromPunjab,
                        Password = entity.Password,
                        RegistrationNumber = entity.RegistrationNumber,
                        ProfileImageReference = entity.ProfileImageReference,
                        SubjectDetails = tableSubject,
                        IsManualData = entity.IsManualData,
                        IsDiploma = entity.IsDiploma,
                        stream = entity.stream
                    };

                    studentRegisterResponseModel = await (connection.QuerySingleAsync<StudentRegisterResponseModel>(procedure, values, commandType: CommandType.StoredProcedure));
                   

                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Register Student : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentRegisterResponseModel;
        }

        public async Task<StudentLoginMaster> GetAsync(StudentLogin entity)
        {
            StudentLoginMaster studentLogin = new StudentLoginMaster();
            try
            {
                string procedure = Procedure.StudentLogin;
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

                _logError.WriteTextToFile("GetAsync Student Login : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentLogin;
        }

        public async Task<int> UpdateStudentPersonalDetails(UpdatePersonalDetails model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateStudentPersonalDetails;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Student Personal Details: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UpdateBankDetails(UpdateBankDetails model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateBankDetails;
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
                var procedure = Procedure.UpdateAddressDetails;
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

        public async Task<int> UpdateAcademicDetails(UpdateAcademicDetails model)
        {
            int result = 0;

            try
            {
                ListToDataTableExtension lstSubject = new ListToDataTableExtension();
                var procedure = Procedure.UpdateAcademicDetails;
                DataTable tableSubject = new DataTable();
                tableSubject = lstSubject.ToDataTable<Subjects>(model.lstSubjectDetails);
                var value = new
                {
                    StudentId = model.StudentId,
                    Rollno = model.Rollno,
                    YearOfPassing = model.YearOfPassing,
                    Examination = model.Examination,
                    BoardUniversity = model.BoardUniversity,
                    BoardUniversityID = model.BoardUniversityID,
                    SchoolCollegeName = model.SchoolCollegeName,
                    StreamID = model.StreamID,
                    ResultStatus = model.ResultStatus,
                    TotalMarks = model.TotalMarks,
                    ObtainedMarks = model.ObtainedMarks,
                    Percentage = model.Percentage,
                    CGPA = model.CGPA,
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
                    IsDiploma = model.IsDiploma,
                    IsManual = model.IsManual,
                    IsCMScholarship=model.IsCMScholarship,
                    IsAlreadyScholarship=model.IsAlreadyScholarship,
                    CMAmount = model. Amount ,
                    SchemeName=model.SchemeName,
                    sponseredby=model.sponseredby,
                    SubjectDetails = tableSubject ?? new DataTable()
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

        public async Task<int> UpdateWeightage(Weightage model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.UpdateWeightage;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, model, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Weightage: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<int> UploadDocuments(UploadDocuments model)
        {
            int result = 0;
            List<DocumentDetail> lstDedtail = new List<DocumentDetail>();
            try
            {
                DocumentDetail obj;
                obj = new DocumentDetail();
                obj.DocumentType = Convert.ToString(EnumDocumentType.Tenth);
                obj.DocumentReference = model.TenthCertificateReference;
                lstDedtail.Add(obj);
                obj = new DocumentDetail();
                obj.DocumentType = Convert.ToString(EnumDocumentType.Twelth);
                obj.DocumentReference = model.TwelvethCertificateReference;
                lstDedtail.Add(obj);
                obj = new DocumentDetail();
                obj.DocumentType = Convert.ToString(EnumDocumentType.Photo);
                obj.DocumentReference = model.PhotoReference;
                lstDedtail.Add(obj);
                obj = new DocumentDetail();
                obj.DocumentType = Convert.ToString(EnumDocumentType.Signature);
                obj.DocumentReference = model.SignatureReference;
                lstDedtail.Add(obj);
                if (!String.IsNullOrEmpty(model.CharacterReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Character);
                    obj.DocumentReference = model.CharacterReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.RuralReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Rural);
                    obj.DocumentReference = model.RuralReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.NCCReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.NCC);
                    obj.DocumentReference = model.NCCReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.NSSReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.NSS);
                    obj.DocumentReference = model.NSSReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.YouthWelfareReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.YouthWelfare);
                    obj.DocumentReference = model.YouthWelfareReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.SCReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.SC);
                    obj.DocumentReference = model.SCReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.BCReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.BC);
                    obj.DocumentReference = model.BCReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.IncomeReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Income);
                    obj.DocumentReference = model.IncomeReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.ResidenceReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Residence);
                    obj.DocumentReference = model.ResidenceReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.MigrationReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Migration);
                    obj.DocumentReference = model.MigrationReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.SchoolLeavingReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.SchoolLeaving);
                    obj.DocumentReference = model.SchoolLeavingReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.physicalDisabilityReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.PhysicalDisability);
                    obj.DocumentReference = model.physicalDisabilityReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.BorderAreaCertificateReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.BorderArea);
                    obj.DocumentReference = model.BorderAreaCertificateReference;
                    lstDedtail.Add(obj);
                }

                if (!String.IsNullOrEmpty(model.SportsReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.Sports);
                    obj.DocumentReference = model.SportsReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.ExServiceManReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.ExServiceMan);
                    obj.DocumentReference = model.ExServiceManReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.FreedomFighterReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.FreedomFighter);
                    obj.DocumentReference = model.FreedomFighterReference;
                    lstDedtail.Add(obj);
                }
                if (!String.IsNullOrEmpty(model.KashmiriMigrantReference))
                {
                    obj = new DocumentDetail();
                    obj.DocumentType = Convert.ToString(EnumDocumentType.KashmiriMigrant);
                    obj.DocumentReference = model.KashmiriMigrantReference;
                    lstDedtail.Add(obj);
                }
                ListToDataTableExtension lst = new ListToDataTableExtension();
                var procedure = Procedure.UploadDocuments;
                DataTable table = new DataTable();
                table = lst.ToDataTable<DocumentDetail>(lstDedtail);
                var value = new
                {
                    StudentId = model.StudentId,
                    TenthSerialNumber = model.TenthSerialNumber,
                    TwelvethSerialNumber = model.TwelvethSerialNumber,
                    ResidenceSerialNumber = model.ResidenceSerialNumber,
                    IncomeSerialNumber = model.IncomeSerialNumber,
                    casteSerialNumber = model.casteSerialNumber,
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
                _logError.WriteTextToFile("Upload Documents", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<StudentMaster> GetStudentById(int studentID)
        {
            StudentMaster studentMaster = new StudentMaster();
            try
            {
                var procedure = Procedure.GetStudentById;
                var value = new
                {
                    StudentID = studentID
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
                _logError.WriteTextToFile("Get Student By Id:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return studentMaster;
        }

        public async Task<List<StudentAcademicMaster>> GetSubjectByStudentId(int studentID)
        {
            List<StudentAcademicMaster> studentMaster = new List<StudentAcademicMaster>();
            try
            {
                var procedure = Procedure.GetSubjectByStudentId;
                var value = new
                {
                    StudentID = studentID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<StudentAcademicMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    var tenthDetails = obj.Where(x => x.Examination == "Diploma");
                    var twelvethDetails = obj.Where(x => x.Examination == "Twelveth");
                    if (twelvethDetails.ToList().Count() > 0)
                    {

                        var data = twelvethDetails?.GroupBy(x => new
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
                            x.Result,
                            x.IsDiploma,
                            x.IsManualData,
                            x.IsTwelveth
                        }).Select(x => new StudentAcademicMaster
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
                            IsDiploma = x.Key.IsDiploma,
                            IsManualData = x.Key.IsManualData,
                            IsTwelveth = x.Key.IsTwelveth,
                            QualificationId = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().QualificationId : 0,
                            Examination = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Examination : null,
                            BoardUniversity = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversity : null,
                            BoardUniversityID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityID : null,
                            CGPA = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().CGPA : null,
                            ObtainedMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().ObtainedMarks : null,
                            Percentage = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Percentage : 0,
                            ResultStatus = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().ResultStatus : null,
                            Result = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Result : null,
                            SchoolCollegeName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().SchoolCollegeName : null,
                            //StreamID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamID : null,
                            StreamID = x.Key.StreamID,
                            //StreamName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamName : null,
                            StreamName = x.Key.StreamName,
                            TotalMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().TotalMarks : null,
                            YearOfPassing = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().YearOfPassing : null,
                            Rollno = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Rollno : null,
                            BoardUniversityName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityName : null,

                            lstSubjectDetails = x.Select(x => new Id
                            {
                                SubjectName = x.SubjectName,
                                MaxNumber = x.MaxNumber,
                                SubjectID = x.SubjectID,
                                ObtainedNumber = x.ObtainedNumber
                            }).ToList().Count > 1 ? x.Select(x => new Id
                            {
                                SubjectName = x.SubjectName,
                                MaxNumber = x.MaxNumber,
                                SubjectID = x.SubjectID,
                                ObtainedNumber = x.ObtainedNumber
                            }).ToList() : null
                        }).ToList();
                        studentMaster = _mapper.Map<List<StudentAcademicMaster>>(data);

                    }
                    else
                    {
                        var data1 = tenthDetails?.GroupBy(x => new
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
                            x.Result,
                            x.IsDiploma,
                            x.IsManualData,
                            x.IsTwelveth
                        }).Select(x => new StudentAcademicMaster
                        {
                            StudentId = x.Key.StudentId,

                            IsDiploma = x.Key.IsDiploma,
                            IsManualData = x.Key.IsManualData,
                            IsTwelveth = x.Key.IsTwelveth,
                            QualificationId = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().QualificationId : 0,
                            Examination = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Examination : null,
                            BoardUniversity = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversity : null,
                            BoardUniversityID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityID : null,
                            CGPA = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().CGPA : null,
                            ObtainedMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().ObtainedMarks : null,
                            Percentage = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Percentage : 0,
                            ResultStatus = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().ResultStatus : null,
                            Result = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Result : null,
                            SchoolCollegeName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().SchoolCollegeName : null,
                            StreamID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamID : null,
                            StreamName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamName : null,
                            TotalMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().TotalMarks : null,
                            YearOfPassing = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().YearOfPassing : null,
                            Rollno = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Rollno : null,
                            BoardUniversityName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityName : null,
                            lstSubjectDetails = x.Select(x => new Id
                            {
                                SubjectName = x.SubjectName,
                                MaxNumber = x.MaxNumber,

                                SubjectID = x.SubjectID,
                                ObtainedNumber = x.ObtainedNumber
                            }).ToList().Count > 1 ? x.Select(x => new Id
                            {
                                SubjectName = x.SubjectName,
                                MaxNumber = x.MaxNumber,
                                SubjectID = x.SubjectID,
                                ObtainedNumber = x.ObtainedNumber
                            }).ToList() : null
                        }).ToList();
                        studentMaster = _mapper.Map<List<StudentAcademicMaster>>(data1);

                    }


                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Subject By Student Id:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return studentMaster;
        }
        public async Task<List<StudentAcademicMaster>> GetSubjectByRegId(string RegId)
        {
            List<StudentAcademicMaster> studentMaster = new List<StudentAcademicMaster>();
            try
            {
                var procedure = Procedure.GetSubjectByRegId;
                var value = new
                {
                    RegId = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<StudentAcademicMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    var tenthDetails = obj.Where(x => x.Examination == "Diploma");
                    var twelvethDetails = obj.Where(x => x.Examination == "Twelveth");
                    if (twelvethDetails.ToList().Count() > 0)
                    {

                        var data = twelvethDetails?.GroupBy(x => new
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
                            x.Result,
                            x.IsDiploma,
                            x.IsManualData,
                            x.IsTwelveth
                        }).Select(x => new StudentAcademicMaster
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
                            IsDiploma = x.Key.IsDiploma,
                            IsManualData = x.Key.IsManualData,
                            IsTwelveth = x.Key.IsTwelveth,
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
                            //StreamID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamID : null,
                            StreamID = x.Key.StreamID,
                            //StreamName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamName : null,
                            StreamName = x.Key.StreamName,
                            TotalMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().TotalMarks : null,
                            YearOfPassing = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().YearOfPassing : null,
                            Rollno = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Rollno : null,
                            BoardUniversityName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityName : null,

                            lstSubjectDetails = x.Select(x => new Id
                            {
                                SubjectName = x.SubjectName,
                                MaxNumber = x.MaxNumber,
                                SubjectID = x.SubjectID,
                                ObtainedNumber = x.ObtainedNumber
                            }).ToList().Count > 1 ? x.Select(x => new Id
                            {
                                SubjectName = x.SubjectName,
                                MaxNumber = x.MaxNumber,
                                SubjectID = x.SubjectID,
                                ObtainedNumber = x.ObtainedNumber
                            }).ToList() : null
                        }).ToList();
                        studentMaster = _mapper.Map<List<StudentAcademicMaster>>(data);

                    }
                    else
                    {
                        var data1 = tenthDetails?.GroupBy(x => new
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
                            x.Result,
                            x.IsDiploma,
                            x.IsManualData,
                            x.IsTwelveth
                        }).Select(x => new StudentAcademicMaster
                        {
                            StudentId = x.Key.StudentId,

                            IsDiploma = x.Key.IsDiploma,
                            IsManualData = x.Key.IsManualData,
                            IsTwelveth = x.Key.IsTwelveth,
                            QualificationId = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().QualificationId : 0,
                            Examination = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Examination : null,
                            BoardUniversity = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversity : null,
                            BoardUniversityID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityID : null,
                            CGPA = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().CGPA : null,
                            ObtainedMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().ObtainedMarks : null,
                            Percentage = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Percentage : 0,
                            ResultStatus = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().ResultStatus : null,
                            Result = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Result : null,
                            SchoolCollegeName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().SchoolCollegeName : null,
                            StreamID = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamID : null,
                            StreamName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().StreamName : null,
                            TotalMarks = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().TotalMarks : null,
                            YearOfPassing = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().YearOfPassing : null,
                            Rollno = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().Rollno : null,
                            BoardUniversityName = tenthDetails.FirstOrDefault() != null ? tenthDetails.FirstOrDefault().BoardUniversityName : null,
                            lstSubjectDetails = x.Select(x => new Id
                            {
                                SubjectName = x.SubjectName,
                                MaxNumber = x.MaxNumber,
                                SubjectID = x.SubjectID,
                                ObtainedNumber = x.ObtainedNumber
                            }).ToList().Count > 1 ? x.Select(x => new Id
                            {
                                SubjectName = x.SubjectName,
                                MaxNumber = x.MaxNumber,
                                SubjectID = x.SubjectID,
                                ObtainedNumber = x.ObtainedNumber
                            }).ToList() : null
                        }).ToList();
                        studentMaster = _mapper.Map<List<StudentAcademicMaster>>(data1);

                    }


                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Subject By Reg Id:", ex.Message, ex.HResult, ex.StackTrace);

            }
            return studentMaster;
        }
        public async Task<UploadedDocumentDetail> GetDocumentsByRegId(string RegId)
        {
            List<GetUploadedDocument> uploadDocuments = new List<GetUploadedDocument>();
            UploadedDocumentDetail documentDetail = new UploadedDocumentDetail();
            try
            {
                var procedure = Procedure.GetDocumentsByRegId;
                var value = new
                {
                    RegId = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetUploadedDocument>(procedure, value, commandType: CommandType.StoredProcedure);
                    uploadDocuments = _mapper.Map<List<GetUploadedDocument>>(obj);
                    if (uploadDocuments.Count > 0)
                    {
                        documentDetail.TenthSerialNumber = uploadDocuments.FirstOrDefault().TenthSerialNumber;
                        documentDetail.TwelvethSerialNumber = uploadDocuments.FirstOrDefault().TwelvethSerialNumber;
                        documentDetail.ResidenceSerialNumber = uploadDocuments.FirstOrDefault().ResidenceSerialNumber;
                        documentDetail.IncomeSerialNumber = uploadDocuments.FirstOrDefault().IncomeSerialNumber;
                        documentDetail.casteSerialNumber = uploadDocuments.FirstOrDefault().casteSerialNumber;
                        documentDetail.BorderAreaCertificateSerialNumber = uploadDocuments.FirstOrDefault().BorderAreaCertificateSerialNumber;
                        documentDetail.isFromBorderArea = uploadDocuments.FirstOrDefault().isFromBorderArea;
                        documentDetail.isResidenceSerialNumberVerified = uploadDocuments.FirstOrDefault().isResidenceSerialNumberVerified;
                        documentDetail.isIncomeSerialNumberVerified = uploadDocuments.FirstOrDefault().isIncomeSerialNumberVerified;
                        documentDetail.isCasteSerialNumberVerified = uploadDocuments.FirstOrDefault().isCasteSerialNumberVerified;
                        documentDetail.IsManualData = uploadDocuments.FirstOrDefault().IsManualData;
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
                        documentDetail.physicalDisability = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.PhysicalDisability)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.BorderAreaCertificate = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.BorderArea)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Sports = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Sports)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.ExServiceMan = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.ExServiceMan)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.FreedomFighter = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.FreedomFighter)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.KashmiriMigrant = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.KashmiriMigrant)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Character = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Character)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.isBcSerialNumberVerified = uploadDocuments.FirstOrDefault().isBcSerialNumberVerified;
                        documentDetail.bcSerialNumber = uploadDocuments.FirstOrDefault().bcSerialNumber;
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
        public async Task<StudentDetails> GetStudentDetailsByRegId(string RegId, string RollNo)
        {
            StudentDetails studentMaster = new StudentDetails();
            try
            {
                var procedure = Procedure.StudentDetails;
                var value = new
                {
                    RegId = RegId,
                    RollNo = RollNo
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var multi = await con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster.studentDetail = multi.Read<BasicDetails>().ToList();
                    studentMaster.meritStatus = multi.Read<MeritStatus>().ToList();
                    studentMaster.admissionStatus = multi.Read<AdmissionStatusPostMerit>().ToList();
                    studentMaster.actionHistory = multi.Read<ActionHistory>().ToList();
                    studentMaster.verificationDetails= multi.Read<StudentVerificationDetails>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Student Details By RegId:", "RegId:"+RegId+","+ ex.Message, ex.HResult, ex.StackTrace);

            }
            return studentMaster;
        }
        public async Task<SubjectCombinationForStudent> GetCourseChoiceByStudentId(int studentID)
        {
            SubjectCombinationForStudent studentMaster = new SubjectCombinationForStudent();
            try
            {
                var procedure = Procedure.GetCourseChoiceByStudentId;
                var value = new
                {
                    StudentID = studentID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster.choiceCourseSelecteds = obj.Read<ChoiceCourseSelected>().ToList();
                    List<studentCombinationSubject> combList = new List<studentCombinationSubject>();
                    combList = obj.Read<studentCombinationSubject>().ToList();
                    for (int i = 0; i < studentMaster.choiceCourseSelecteds.Count; i++)
                    {
                        string prefChoice = studentMaster.choiceCourseSelecteds[i].PreferenceChoice;
                        string set = studentMaster.choiceCourseSelecteds[i].CombinationSet.ToString();
                        int collegeId = studentMaster.choiceCourseSelecteds[i].CollegeId;
                        int courseId = studentMaster.choiceCourseSelecteds[i].CollegeCourseId;
                        if (combList.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(combList[i].SubjectId))
                            {
                                studentMaster.choiceCourseSelecteds[i].subjects = combList.ToList().Where(x => x.PreferenceChoice.Equals(prefChoice) && x.CombinationSet.Equals(set) && x.CollegeId.Equals(collegeId) && x.CourseId.Equals(courseId)).ToList();

                            }
                            else
                            {
                                studentMaster.choiceCourseSelecteds[i].subjects = null;
                            }
                        }
                        else
                        {
                            studentMaster.choiceCourseSelecteds[i].subjects = null;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get ug course choice By Student Id:", "studentId:" + studentID + ", " + ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentMaster;
        }
        public async Task<ProgressBar> GetProgressBar(int studentID)
        {
            ProgressBar obj = new ProgressBar();
            try
            {
                var procedure = Procedure.GetProgressBar;
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
                var procedure = Procedure.UpdateDeclarations;
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
                var procedure = Procedure.GenerateOTP;
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
                var procedure = Procedure.UpdateCourseChoice;
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

        public async Task<UploadedDocumentDetail> GetUploadedDocuments(int studentID)
        {
            List<GetUploadedDocument> uploadDocuments = new List<GetUploadedDocument>();
            UploadedDocumentDetail documentDetail = new UploadedDocumentDetail();
            try
            {
                var procedure = Procedure.GetDocumentsByStudentId;
                var value = new
                {
                    StudentID = studentID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetUploadedDocument>(procedure, value, commandType: CommandType.StoredProcedure);
                    uploadDocuments = _mapper.Map<List<GetUploadedDocument>>(obj);
                    if (uploadDocuments.Count > 0)
                    {
                        documentDetail.TenthSerialNumber = uploadDocuments.FirstOrDefault().TenthSerialNumber;
                        documentDetail.TwelvethSerialNumber = uploadDocuments.FirstOrDefault().TwelvethSerialNumber;
                        documentDetail.ResidenceSerialNumber = uploadDocuments.FirstOrDefault().ResidenceSerialNumber;
                        documentDetail.IncomeSerialNumber = uploadDocuments.FirstOrDefault().IncomeSerialNumber;
                        documentDetail.casteSerialNumber = uploadDocuments.FirstOrDefault().casteSerialNumber;
                        documentDetail.BorderAreaCertificateSerialNumber = uploadDocuments.FirstOrDefault().BorderAreaCertificateSerialNumber;
                        documentDetail.isFromBorderArea = uploadDocuments.FirstOrDefault().isFromBorderArea;
                        documentDetail.isResidenceSerialNumberVerified = uploadDocuments.FirstOrDefault().isResidenceSerialNumberVerified;
                        documentDetail.isIncomeSerialNumberVerified = uploadDocuments.FirstOrDefault().isIncomeSerialNumberVerified;
                        documentDetail.isCasteSerialNumberVerified = uploadDocuments.FirstOrDefault().isCasteSerialNumberVerified;
                        documentDetail.IsManualData = uploadDocuments.FirstOrDefault().IsManualData;
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
                        documentDetail.physicalDisability = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.PhysicalDisability)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.BorderAreaCertificate = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.BorderArea)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Sports = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Sports)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.ExServiceMan = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.ExServiceMan)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.FreedomFighter = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.FreedomFighter)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.KashmiriMigrant = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.KashmiriMigrant)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.Character = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Character)).Select(x => x.DocumentReference).FirstOrDefault();
                        documentDetail.isBcSerialNumberVerified = uploadDocuments.FirstOrDefault().isBcSerialNumberVerified;
                        documentDetail.bcSerialNumber = uploadDocuments.FirstOrDefault().bcSerialNumber;
                        documentDetail.Rural = uploadDocuments.Where(x => x.DocumentType == Convert.ToString(EnumDocumentType.Rural)).Select(x => x.DocumentReference).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Uploaded Documents:", "studentID:" + studentID + ", error:" + ex.Message, ex.HResult, ex.StackTrace);

            }
            return documentDetail;
        }

        public async Task<int> RegisterationFeesPayment(RegisterationFees model)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.SaveRegisterationFees;
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
            int result=0;
            try
            {
                var procedure = Procedure.UnlockForm;
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
                var procedure = Procedure.UpdateStudentDetails;
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
                var procedure = Procedure.StudentForgotPassword;
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
                _logError.WriteTextToFile("ForgotPassword : ", "ug mail:"+email+", error: "+ex.Message, ex.HResult, ex.StackTrace);
                detail.response = "0";
            }
            return detail;
        }
        public async Task<StudentMaster> GetStudentAppDetailsByRegId(FilterStudent _filter)
        {
            StudentMaster studentMaster = new StudentMaster();
            try
            {
                var procedure = Procedure.StudentAppDetails;
                var value = new
                {
                    RegId = _filter.regId,
                    MobileNumber= _filter.mobileNumber,
                    RollNumber= _filter.rollNumber,
                    Year=_filter.year,
                    BoardId=_filter.boardId
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
        public async Task<SubjectCombinationForStudent> GetCourseChoiceByRegId(string RegId)
        {
            SubjectCombinationForStudent studentMaster = new SubjectCombinationForStudent();
            try
            {
                var procedure = Procedure.GetCourseChoiceByRegId;
                var value = new
                {
                    RegId = RegId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    studentMaster.choiceCourseSelecteds = obj.Read<ChoiceCourseSelected>().ToList();
                    List<studentCombinationSubject> combList = new List<studentCombinationSubject>();
                    combList = obj.Read<studentCombinationSubject>().ToList();
                    for (int i = 0; i < studentMaster.choiceCourseSelecteds.Count; i++)
                    {
                        string prefChoice = studentMaster.choiceCourseSelecteds[i].PreferenceChoice;
                        string set = studentMaster.choiceCourseSelecteds[i].CombinationSet.ToString();
                        int collegeId = studentMaster.choiceCourseSelecteds[i].CollegeId;
                        int courseId = studentMaster.choiceCourseSelecteds[i].CollegeCourseId;
                        if (combList.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(combList[i].SubjectId))
                            {
                                studentMaster.choiceCourseSelecteds[i].subjects = combList.ToList().Where(x => x.PreferenceChoice.Equals(prefChoice) && x.CombinationSet.Equals(set) && x.CollegeId.Equals(collegeId) && x.CourseId.Equals(courseId)).ToList();

                            }
                            else
                            {
                                studentMaster.choiceCourseSelecteds[i].subjects = null;
                            }
                        }
                        else
                        {
                            studentMaster.choiceCourseSelecteds[i].subjects = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get course choice By Reg Id:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return studentMaster;
        }

        public async Task<int> UpdateCourseChoiceWithSubject(SubjectCombinationsWithCollege model)
        {
            int result = 0;

            try
            {
                DataTable mergedt = new();
                mergedt.Columns.Add("StudentId", typeof(System.Int32));
                mergedt.Columns.Add("CollegeId", typeof(System.String));
                mergedt.Columns.Add("CourseId", typeof(System.Int32));
                mergedt.Columns.Add("PreferenceChoice", typeof(System.String));
                mergedt.Columns.Add("CombinationSet", typeof(System.String));
                mergedt.Columns.Add("SubjectId", typeof(System.String));

                for (int i = 0; i < model.SubjectCombinations.Count; i++)
                {
                    DataRow dr = mergedt.NewRow();
                    dr["StudentId"] = model.StudentId;
                    dr["CollegeId"] = model.SubjectCombinations[i].CollegeId;
                    dr["CourseId"] = model.SubjectCombinations[i].CollegeCourseId;
                    dr["PreferenceChoice"] = model.SubjectCombinations[i].PreferenceChoice;
                    dr["CombinationSet"] = model.SubjectCombinations[i].combinationSet;
                    if (model.SubjectCombinations[i].Subjects == null || model.SubjectCombinations[i].Subjects.Count == 0)
                    {
                        dr["SubjectId"] = null;
                        mergedt.Rows.Add(dr);
                    }
                    else
                    {
                        for (int j = 0; j < model.SubjectCombinations[i].Subjects.Count; j++)
                        {
                            if (j == 0)
                            {
                                dr["SubjectId"] = model.SubjectCombinations[i].Subjects[j].SubjectId;
                                mergedt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr1 = mergedt.NewRow();
                                dr1["StudentId"] = model.StudentId;
                                dr1["CollegeId"] = model.SubjectCombinations[i].CollegeId;
                                dr1["CourseId"] = model.SubjectCombinations[i].CollegeCourseId;
                                dr1["PreferenceChoice"] = model.SubjectCombinations[i].PreferenceChoice;
                                dr1["CombinationSet"] = model.SubjectCombinations[i].combinationSet;
                                dr1["SubjectId"] = model.SubjectCombinations[i].Subjects[j].SubjectId;
                                mergedt.Rows.Add(dr1);
                            }
                        }
                    }
                }
                //ListToDataTableExtension lst = new ListToDataTableExtension();
                //DataTable table = new DataTable();
                //if (action.Subjects != null)

                //{
                //    table = lst.ToDataTable<SubjectDetail>(action.Subjects);

                //}
                //else
                //{
                //    table.Columns.Add("GroupId", typeof(System.Int32));
                //    table.Columns.Add("GroupName", typeof(System.String));
                //    table.Columns.Add("SubjectId", typeof(System.Int32));
                //    table.Columns.Add("SubjectName", typeof(System.String));

                //}
                var values = new
                {
                    StudentId = model.StudentId,
                    Subjects = mergedt
                };
                var procedure = Procedure.UpdateCourseChoiceWithSubject;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Course ChoiceWith Subject: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<SelectedCourseTotalFee> GetCourseChoiceFee(CourseChoiceFee model)
        {
            SelectedCourseTotalFee feeDetails = new SelectedCourseTotalFee();

            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();
                DataTable table = new DataTable();
                table = lst.ToDataTable<SelectedCourse>(model.SelectedCourses);
                var values = new
                {
                    CollegeId = model.CollegeId,
                    CourseChoices = table
                };
                var procedure = Procedure.GetCourseChoiceFee;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await (connection.QuerySingleAsync<SelectedCourseTotalFee>(procedure, values, commandType: CommandType.StoredProcedure));
                    feeDetails = _mapper.Map<SelectedCourseTotalFee>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Update Course Choice: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return feeDetails;
        }

        public async Task<List<GetEligibleCourseByStudentID>> getEligibleCourseByStudentIDs(int CollegeId, int StudentId)
        {

            List<GetEligibleCourseByStudentID> CourseList = new List<GetEligibleCourseByStudentID>();
            try
            {
                var procedure = Procedure.GetEligibleCourse;
                var value = new
                {
                    CollegeId = CollegeId,
                    StudentId = StudentId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<GetEligibleCourseByStudentID>(procedure, value, commandType: CommandType.StoredProcedure);
                    CourseList = _mapper.Map<List<GetEligibleCourseByStudentID>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Eligible Course By Student Id:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return CourseList;

        }

        public async Task<int> checkCollegeSubject(int collegeID, int courseId)
        {
            int result = 0;

            try
            {
                var values = new
                {
                    collegeID = collegeID,
                    courseId = courseId
                };
                var procedure = Procedure.CheckCollegeCourseSubject;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Get count College Course Subject: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<List<ObjectionListByStudentModel>> GetObjectionsByStudentID(int StudentId)
        {

            List<ObjectionListByStudentModel> Objectlist = new List<ObjectionListByStudentModel>();
            try
            {
                var procedure = Procedure.GetObjectListByStudentId;
                var value = new
                {

                    StudentId = StudentId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {

                    var obj = await con.QueryAsync<ObjectionListByStudentModel>(procedure, value, commandType: CommandType.StoredProcedure);
                    Objectlist = _mapper.Map<List<ObjectionListByStudentModel>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Objection list By Student Id:", ex.Message, ex.HResult, ex.StackTrace);
            }
            return Objectlist;

        }

        public async Task<StudentSeatBookedOffered> GetAdmissionSeatDetails(int StudentId)
        {
            StudentSeatBookedOffered list = new StudentSeatBookedOffered();
            try
            {
                var procedure = Procedure.GetAdmissionSeatDetailsByStudentId;
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
                var procedure = Procedure.GetStudentReceiptByStudentId;
                var value = new
                {
                    StudentId = StudentId,
                    TransactionId = transactionId
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
                var procedure = Procedure.GetStudentReceiptListByStudentId;
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
