using AdmissionPortal_API.Domain.ExternalAPI;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
  public   interface IExternalAPI
    {        
        Task<ServiceResult> GetPSEBAsync(CBSESchoolDetails entity);
         Task<ServiceResult> GetCBSEAsync(CBSESchoolDetails entity);
         Task<ServiceResult> GetbsehrAsync(CBSESchoolDetails entity);
        Task<ServiceResult> GetRJBSEAsync(CBSESchoolDetails entity); 
        Task<int> StudenAlreadyRegisted(CBSESchoolDetails entity);
        Task<ServiceResult> StudentMarksVerification();
        Task<ServiceResult> ValidateDocument(DocumentValidate entity,Int32 StudentId,string type);
    }
}
