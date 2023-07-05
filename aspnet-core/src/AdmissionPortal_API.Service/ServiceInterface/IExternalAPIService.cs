using AdmissionPortal_API.Domain.ExternalAPI;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IExternalAPIService
    {
        Task<ServiceResult> GetStudentInfoFromBoard(string Type,CBSESchoolDetails model);        
        Task<ServiceResult> StudentMarksVerification();
        Task<ServiceResult> ValidateDocument(DocumentValidate model,Int32 StudentId,string type); 
    }
}
