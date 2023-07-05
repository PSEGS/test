using AdmissionPortal_API.Domain.ApiModel.WorkflowStageAction;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IWorkflowStageActionService
    {
        ServiceResult CreateWFStageAction(AddWorkflowStageAction model);

        ServiceResult UpdateWFStageAction(UpdateWorkflowStageAction model);

        ServiceResult DeleteWFStageAction(long actionId);

        ServiceResult GetAllWFStageAction(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        ServiceResult GetWFStageActionById(long actionId);
    }
}

