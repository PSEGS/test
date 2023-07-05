using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Utility.Notification
{
   public interface ISms
    {
        Task<ServiceResult> SendSmsAsync(string Moblile, string Message, string TemplateId);
    }
}
