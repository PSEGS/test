using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.WorkflowStageAction;
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
    public class WorkflowStageActionRepository : IWorkflowStageActionRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public WorkflowStageActionRepository(IMapper mapper, IConfiguration configuration,ILogError logError)
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
        public async Task<int> AddAsync(WorkflowStageActionMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.SaveWorkflowStageAction;
                var values = _mapper.Map<AddWorkflowStageAction>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Save Workflow Stage Action : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task<int> DeleteWFStageActionAsync(long actionId)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeleteWorkflowStageAction;
                var values = new { ActionId = actionId};
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Workflow Stage Action : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task<WorkflowStageActionMaster> GetWFStageActionByIdAsync(long actionId)
        {
            WorkflowStageActionMaster workflowStageAction = new WorkflowStageActionMaster();
            try
            {
                var procedure = Procedure.GetWorkflowStageActionById;
                var value = new
                {
                    Select_Param = actionId,
                    Select_By="id"
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QuerySingleAsync<WorkflowStageActionMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    workflowStageAction = _mapper.Map<WorkflowStageActionMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get WOrk flow Stage Action By Id:" , ex.Message, ex.HResult, ex.StackTrace);
            }
            return workflowStageAction;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WorkflowStageActionMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UpdateWorkflowStageAction;
                var values = _mapper.Map<UpdateWorkflowStageAction>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Workflow Stage Action : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        public Task<WorkflowStageActionMaster> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkflowStageActionMaster>> GetAllWFStageAction(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<WorkflowStageActionMaster> lstWorkflow = new List<WorkflowStageActionMaster>();
            try
            {
                var procedure = Procedure.GetAllWorkflowStageAction;
                var value = new
                {
                    PageNumber=pageNumber,
                    PageSize=pageSize,
                    Select_By="All",
                    SearchKeyword = searchKeyword,
                    SortBy = sortBy,
                    SortOrder = sortOrder
                };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync(procedure, value,commandType: CommandType.StoredProcedure);
                    lstWorkflow = _mapper.Map<List<WorkflowStageActionMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Workflow Stage Action : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstWorkflow;
        }
    }
}

