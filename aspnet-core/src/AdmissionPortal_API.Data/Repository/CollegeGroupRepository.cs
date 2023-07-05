using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using AdmissionPortal_API.Domain.ViewModel;
using static AdmissionPortal_API.Domain.Model.CourseModel;
using AdmissionPortal_API.Domain.ApiModel.CollegeGroup;
using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Utility.ExtensionMethod;

namespace AdmissionPortal_API.Data.Repository
{
   public  class CollegeGroupRepository:ICollegeGroup
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public CollegeGroupRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;
        }

        public async Task<int> AddSubjectGroup(collegegroup entity)
        {
            int result = 0;
            try
            {
                ListToDataTableExtension lst = new ListToDataTableExtension();

                string procedure = Procedure.SaveCollegeGroupSubject;
                DataTable table = new DataTable();
                table = lst.ToDataTable<CollegGroupSubject>(entity.collegGroupSubjects);
               
                var values = new
                {
                    collegeId = entity.collegeId,
                    groupId = entity.groupId,
                    courseId = entity.CourseId,
                    MapGroupSubject = table,
                    CreatedBy = entity.CreatedBy

                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    //addcourse = _mapper.Map<AddCourse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Add College Group Subject: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public async Task<List<GroupsList>> GetAllGroup(int courseId, int collegeId, int? pageNumber, int? pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<GroupsList> groupDetails = new List<GroupsList>();
            try
            {
                var procedure = Procedure.CollegeGroups;
                var value = new
                {
                    courseId= courseId,
                    CollegeID = collegeId,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    searchKeyword = searchKeyword,
                    sortBy = sortBy,
                    sortOrder = sortOrder
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GroupsList>(procedure, value, commandType: CommandType.StoredProcedure);
                    groupDetails = _mapper.Map<List<GroupsList>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College groups : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return groupDetails;
        }

        public async Task<GroupDetails> GetGroupDetails(int collegeId, int courseID)
        {
            GroupDetails groupDetails = new GroupDetails();
            try
            {
                var procedure = Procedure.GetGroupWithSubject;
                var value = new
                {
                    CollegeID = collegeId,
                    courseID = courseID
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    groupDetails.collegesubjectlists = obj.Read<collegesubjectlist>().ToList();
                    groupDetails.groups = obj.Read<Groups>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College courses : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return groupDetails;
        }

        public async Task<List<groupsubjectDetails>> GetGroupDetailsByGroupId(int collegeId, int groupid, int courseId)
        {
            List<groupsubjectDetails> groupDetails = new List<groupsubjectDetails>();
            try
            {
                var procedure = Procedure.GroupSubjectDetails;
                var value = new
                {
                    CollegeID = collegeId,
                    groupid = groupid,
                    CourseId= courseId
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<groupsubjectDetails>(procedure, value, commandType: CommandType.StoredProcedure);
                    groupDetails = _mapper.Map<List<groupsubjectDetails>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College group details : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return groupDetails;
        }

        public async Task<groupsubjectDetailsForEdit> GetGroupDetailsByGroupIdEdit(int collegeId, int groupid, int courseId)
        {
            groupsubjectDetailsForEdit groupDetails = new groupsubjectDetailsForEdit();
            try
            {
                var procedure = Procedure.GetGroupWithSubjectEdit;
                var value = new
                {
                    CollegeID = collegeId,
                    groupid = groupid,
                    CourseId= courseId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QueryMultipleAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    groupDetails.groups = obj.Read<Groups>().FirstOrDefault();
                    groupDetails.collegesubjectlists = obj.Read<goruplist>().ToList();
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College courses : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return groupDetails;
        }

        public async Task<List<GroupsListCombinedSubjects>> GetAllGroupCombined(int courseId, int collegeId, Int32 studentID)
        {
            List<GroupsListCombinedSubjects> groupDetails = new List<GroupsListCombinedSubjects>();
            try
            {
                var procedure = Procedure.CollegeGroupsNoPaging;
                var value = new
                {
                    courseId = courseId,
                    CollegeID = collegeId
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<GroupsListCombinedSubjects>(procedure, value, commandType: CommandType.StoredProcedure);
                    groupDetails = _mapper.Map<List<GroupsListCombinedSubjects>>(obj);

                    //TODO: Remove foreach and bind direct mapping for nested collection
                    foreach (var item in groupDetails)
                    {
                        var procedureGroupDetails = Procedure.GroupSubjectDetailsforStudent;
                        var valueGroupDetails = new
                        {
                            CollegeID = collegeId,
                            groupid = item.GroupId,
                            courseId = courseId,
                            studentId=studentID
                        };
                        using (var connectionGroupDetail = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                        {
                            connectionGroupDetail.Open();
                            var objGroupDetails = await connection.QueryAsync<groupsubjectDetails>(procedureGroupDetails, valueGroupDetails, commandType: CommandType.StoredProcedure);
                            item.GroupsubjectDetails = _mapper.Map<List<groupsubjectDetails>>(objGroupDetails);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College groups combined subjects: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return groupDetails;
        }

        public async Task<int> lockUnlockGroupSubjectByCollegeID(int collegeID, int status, int userid)
        {
            int result = 0;
            try
            {
                

                string procedure = Procedure.LockUnLockCollegeCourseGroupSubject;
                 

                var values = new
                {
                    collegeID = collegeID,
                    status = status,
                    userid = userid 

                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                    //addcourse = _mapper.Map<AddCourse>(obj);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Lock UnLock College Course Group Subject: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<ServiceResult> DeleteSubjectGroup(int collegeID, int courseid, int groupid)
        {
            ServiceResult result = new ServiceResult();
            try
            {


                string procedure = Procedure.DeleteGroupByGroupId;


                var values = new
                {
                    CollegeID = collegeID,
                    CourseId = courseid,
                    GroupId = groupid

                };

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync<ServiceResult>(procedure, values, commandType: CommandType.StoredProcedure);
                    result = _mapper.Map<ServiceResult>(obj.FirstOrDefault());
                    
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Delete Subject Group: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}
