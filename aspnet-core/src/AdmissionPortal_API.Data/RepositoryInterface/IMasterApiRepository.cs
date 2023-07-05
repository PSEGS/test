using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IMasterApiRepository
    {
        Task<List<Board>> GetBoard();
        Task<List<GetReligion>> GetReligion();
        Task<List<GetReservationCategory>> GetReservationCategory();
        Task<List<GetReservationSubCategory>> GetReservationSubCategory(int id, int CatId);
        Task<List<Common>> GetOccupation();
        Task<List<Common>> GetFatherOccupation();        
        Task<List<Common>> GetHouseholdIncome();
        Task<List<BankMaster>> BankMaster();
        Task<List<Common>> GetBoardSubject();
        Task<islockDetails> GetIsLockDetails(int collegeid);
        Task<List<Country>> GetCountries();
    }
}
