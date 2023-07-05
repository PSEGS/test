using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ILogService
    {
        ServiceResult GetAllLog(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder, DateTime? startDate, DateTime? endDate,string type);

    }
}

