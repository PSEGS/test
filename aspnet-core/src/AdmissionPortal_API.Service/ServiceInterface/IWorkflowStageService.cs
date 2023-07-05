using AdmissionPortal_API.Domain.ApiModel.WorkflowStage;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IWorkflowStageService
    {
        ServiceResult CreateWFStage(AddWorkflowStage model);

        ServiceResult UpdateWFStage(UpdateWorkflowStage model);

        ServiceResult DeleteWFStage(long stageId);

        ServiceResult GetAllWFStage(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        ServiceResult GetWFStageById(long stageId);
    }
}

