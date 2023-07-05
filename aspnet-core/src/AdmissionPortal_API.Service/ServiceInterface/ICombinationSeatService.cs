using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface ICombinationSeatService
    {
        Task<ServiceResult> AddCombinationSeat(CombinationSeat model);
        Task<ServiceResult> getCoursesFeeHeadByHeadTypeId(Int32 collegeId,Int32 courseId);
    }
}
