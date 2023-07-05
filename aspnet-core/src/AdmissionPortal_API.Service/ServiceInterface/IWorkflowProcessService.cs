using AdmissionPortal_API.Domain.ApiModel.WorkflowProcess;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IWorkflowProcessService
    {
        ServiceResult CreateWFProcess(AddWorkflowProcess model);

        ServiceResult UpdateWFProcess(UpdateWorkflowProcess model);

        ServiceResult DeleteWFProcess(int processId);

        ServiceResult GetAllWFProcess(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        ServiceResult GetWFProcessById(int processId);
    }
}

