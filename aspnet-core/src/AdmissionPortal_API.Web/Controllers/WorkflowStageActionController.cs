using AdmissionPortal_API.Domain.ApiModel.WorkflowStageAction;
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
    [Route("api/workflowStageAction")]
    public class WorkflowStageActionController : Controller
    {
        private readonly ILogError _logError;
        private readonly IWorkflowStageActionService _workflowStageActionService;
        public WorkflowStageActionController(ILogError logError, IWorkflowStageActionService workflowStageActionService)
        {
            _logError = logError;
            _workflowStageActionService = workflowStageActionService;
        }

        /// <summary>
        /// Create Workflow StageAction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateWFStageAction([FromBody] AddWorkflowStageAction model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = _workflowStageActionService.CreateWFStageAction(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create WF Stage Action : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update Workflow StageAction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut]
        public IActionResult UpdateWFStageAction([FromBody] UpdateWorkflowStageAction model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _workflowStageActionService.UpdateWFStageAction(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update WF Stage Action : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete Workflow StageAction
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{actionId}")]
        public IActionResult DeleteWFStageAction(long actionId)
        {
            try
            {
                var data = _workflowStageActionService.DeleteWFStageAction(actionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete WF Stage Action : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get All Stage Action
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllWFStageAction(int pageNumber = 1, int pageSize = 10, string searchKeyword = "", string sortBy = "", string sortOrder = "")
        {
            try
            {
                var data = _workflowStageActionService.GetAllWFStageAction(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All WF Stage Action : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get Workflow StageAction By Id
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{actionId}")]
        public IActionResult GetWFStageActionById(long actionId)
        {
            try
            {
                var data = _workflowStageActionService.GetWFStageActionById(actionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get WF Stage Action By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

