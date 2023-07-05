using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.WorkflowProcess;
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
    public class WorkflowProcessRepository:IWorkflowProcessRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;
        private readonly IConfiguration _configuration;
        public WorkflowProcessRepository(IMapper mapper, IConfiguration configuration,ILogError logError)
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
        public async Task<int> AddAsync(WorkflowProcessMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.SaveWorkflowProcess;
                var values = _mapper.Map<AddWorkflowProcess>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Save Workflow Process  : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int processId)
        {
            int result = 0;

            try
            {
                var procedure = Procedure.DeleteWorkflowProcess;
                var values = new { ProcessId = processId};
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete Workflow Process  : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }

        
        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<WorkflowProcessMaster>> GetAllWFProcess(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            List<WorkflowProcessMaster> lstWorkflow = new List<WorkflowProcessMaster>();
            try
            {
                var procedure = Procedure.GetAllWorkflowProcess;
                var value = new { PageNumber=pageNumber,PageSize=pageSize , SearchKeyword = searchKeyword, SortBy = sortBy, SortOrder = sortOrder };
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await connection.QueryAsync(procedure,value ,commandType: CommandType.StoredProcedure);
                    lstWorkflow = _mapper.Map<List<WorkflowProcessMaster>>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All Workflow Process  : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return lstWorkflow;
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public async Task<WorkflowProcessMaster> GetByIdAsync(int processId)
        {

            WorkflowProcessMaster workflowProcess = new WorkflowProcessMaster();
            try
            {
                var procedure = Procedure.GetWorkflowProcessById;
                var value = new
                {
                    ProcessId = processId
                };
                using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    var obj = await con.QuerySingleAsync<WorkflowProcessMaster>(procedure, value, commandType: CommandType.StoredProcedure);
                    workflowProcess = _mapper.Map<WorkflowProcessMaster>(obj);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Work flow Process By Id:" , ex.Message, ex.HResult, ex.StackTrace);
            }
            return workflowProcess;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WorkflowProcessMaster entity)
        {
            int result = 0;
            try
            {
                var procedure = Procedure.UpdateWorkflowProcess;
                var values = _mapper.Map<UpdateWorkflowProcess>(entity);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    
                    result = Convert.ToInt16(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update Workflow Process  : " , ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
    }
}

