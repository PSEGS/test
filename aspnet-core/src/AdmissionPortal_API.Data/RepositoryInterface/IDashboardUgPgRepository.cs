using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IDashboardUgPgRepository 
    {
        Task<DashboardUg> GetDashboardUg(string type,string UserID);
        Task<List<DashboardChartUg>> GetDashboardChartUg(string type);
        Task<DashboardPg> GetDashboardPg(string type);
        Task<List<DashboardChartUg>> GetDashboardChartPg(string type);
    }
}
