using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.WorkflowStage;
using AdmissionPortal_API.Domain.Model;
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

namespace AdmissionPortal_API.Data.Repository
{
    public class WorkflowStageRepository : IWorkflowStageRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public WorkflowStageRepository(IMapper mapper, IConfiguration configuration,ILogError logError)
        {
            _logError = logError;
            _mapper = mapper;
            _configuration = configuration;
        }


        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(WorkflowStageMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.SaveWorkflowStage;
                var values = _mapper.Map<AddWorkflowStage>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Save Workflow Stage  : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public Task<int> DeleteAsync(int stageId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public async Task<int> DeleteWFStageAsync(long stageId)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeleteWorkflowStage;
                var values = new { StageId = stageId };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Workflow Stage  : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        
        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public async Task<WorkflowStageMaster> GetWFStageByIdAsync(long stageId)
        {
            WorkflowStageMaster workflowStage = new WorkflowStageMaster();
            try
            {
                var procedure = Procedure.GetWorkflowStageById;
                var value = new
                {
                    Stage_Id = stageId,
                    OperationType = "StageId"
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QuerySingleAsync<WorkflowStageMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    workflowStage = _mapper.Map<WorkflowStageMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get WOrk flow Stage By Id:" , ex.Message, ex.HResult, ex.StackTrace);
            }
            return workflowStage;
        }


        public Task<WorkflowStageMaster> GetByIdAsync(int stageId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WorkflowStageMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UpdateWorkflowStage;
                var values = _mapper.Map<UpdateWorkflowStage>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Workflow Stage  : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<WorkflowStageMaster>> GetAllWFStage(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {

            List<WorkflowStageMaster> lstWorkflow = new List<WorkflowStageMaster>();
            try
            {
                var procedure = Procedure.GetAllWorkflowStage;
                var value = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchKeyword = searchKeyword,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var obj = await connection.QueryAsync(procedure, value, commandType: CommandType.StoredProcedure);
                    lstWorkflow = _mapper.Map<List<WorkflowStageMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Workflow Stage  : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstWorkflow;
        }
    }
}

