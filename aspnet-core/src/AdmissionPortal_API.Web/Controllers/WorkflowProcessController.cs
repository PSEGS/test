using AdmissionPortal_API.Domain.ApiModel.WorkflowProcess;
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
    [Route("api/workflowProcess")]
    public class WorkflowProcessController : Controller
    {
        private readonly ILogError _logError;
        private readonly IWorkflowProcessService _workflowProcessService;
        public WorkflowProcessController(ILogError logError, IWorkflowProcessService workflowProcessService)
        {
            _logError = logError;
            _workflowProcessService = workflowProcessService;
        }

        /// <summary>
        /// Create Workflow Process
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateWFProcess([FromBody] AddWorkflowProcess model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceResult data = _workflowProcessService.CreateWFProcess(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create WF Process : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update Workflow Process
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut]
        public IActionResult UpdateWFProcess([FromBody] UpdateWorkflowProcess model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _workflowProcessService.UpdateWFProcess(model);
                    return Ok(data);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update WF Process : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete Workflow Process
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{processId}")]
        public IActionResult DeleteWFProcess(int processId)
        {
            try
            {
                var data = _workflowProcessService.DeleteWFProcess(processId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete WF Process : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get All Process
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllWFProcess(int pageNumber=1, int pageSize=10, string searchKeyword="", string sortBy="", string sortOrder="")
        {
            try
            {
                var data = _workflowProcessService.GetAllWFProcess(pageNumber,pageSize,searchKeyword,sortBy,sortOrder);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All WF Process : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get Workflow Process By Id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{processId}")]
        public IActionResult GetWFProcessById(int processId)
        {
            try
            {
                var data = _workflowProcessService.GetWFProcessById(processId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get WF Process By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

