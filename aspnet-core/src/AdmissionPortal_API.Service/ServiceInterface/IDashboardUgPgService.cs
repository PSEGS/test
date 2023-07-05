using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IDashboardUgPgService
    {
        Task<ServiceResult> GetDashboardUg(string type, string UserID);
        Task<ServiceResult> GetDashboardChartUg(string type);
        Task<ServiceResult> GetDashboardPg(string type);
        Task<ServiceResult> GetDashboardChartPg(string type);
    }
}
