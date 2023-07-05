using AdmissionPortal_API.Domain.ApiModel.EmployeeManagement;
using AdmissionPortal_API.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IEmployeeRepository : IGenericRepository<EmployeeMaster>
    {
        Task<List<EmployeeMaster>> GetAllEmployee(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder);

        Task<string> ForgotPassword(string email);

        Task<int> ResetPassword(string OTP, string password);
        Task<EmployeeLoginMaster> GetEmployeeLogin(EmployeeLoginMaster entity);
        Task<List<EmployementTypes>> GetEmployementType();

        Task<EmployeeMaster> GetEmployeeById(int EmployeeID);
    }
}

