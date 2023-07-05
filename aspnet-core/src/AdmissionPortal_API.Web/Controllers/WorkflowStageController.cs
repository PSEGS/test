using AdmissionPortal_API.Domain.ApiModel.WorkflowStage;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/workflowStage")]
    public class WorkflowStageController : Controller
    {
        private readonly ILogError _logError;
        private readonly IWorkflowStageService _workflowStageService;
        public WorkflowStageController(ILogError logError, IWorkflowStageService workflowStageService)
        {
            _logError = logError;
            _workflowStageService = workflowStageService;
        }

        /// <summary>
        /// Create Workflow Stage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateWFStage([FromBody] AddWorkflowStage model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = _workflowStageService.CreateWFStage(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create WF Stage : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update Workflow Stage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut]
        public IActionResult UpdateWFStage([FromBody] UpdateWorkflowStage model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _workflowStageService.UpdateWFStage(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update WF Stage : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete Workflow Stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{stageId}")]
        public IActionResult DeleteWFStage(int stageId)
        {
            try
            {
                var data = _workflowStageService.DeleteWFStage(stageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete WF Stage : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get All Stage
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllWFStage(int pageNumber = 1, int pageSize = 10, string searchKeyword = "", string sortBy = "", string sortOrder = "")
        {
            try
            {
                var data = _workflowStageService.GetAllWFStage(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All WF Stage : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get Workflow Stage By Id
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{stageId}")]
        public IActionResult GetWFStageById(long stageId)
        {
            try
            {
                var data = _workflowStageService.GetWFStageById(stageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get WF Stage By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

