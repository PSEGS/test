 using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{
    public interface IMasterApiService
    {
        Task<ServiceResult> GetBoard();
        Task<ServiceResult> GetReligion();
        Task<ServiceResult> GetReservationCategory();
        Task<ServiceResult> GetReservationSubCategory(int id,int CatId);
        Task<ServiceResult> GetOccupation();
        Task<ServiceResult> GetFatherOccupation();
        Task<ServiceResult> GetHouseholdIncome();
        Task<ServiceResult> BankMaster();
        Task<ServiceResult> GetBoardSubject();
        Task<ServiceResult> GetIsLockDetails(int collegeid);
        Task<ServiceResult> GetCountries(); 
    }
}
