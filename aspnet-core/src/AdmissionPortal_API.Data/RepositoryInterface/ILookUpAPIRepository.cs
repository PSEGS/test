using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.LookupAPIModel;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface  ILookUpAPIRepository 
    {

        Task<List<CollegeType>> GetAllCollegeType();
        Task<List<CollegeMode>> GetAllCollegeMode();
        Task<List<CourseTypeModel>> GetAllCourseType();
        Task<List<LookUpTypeModel>> GetAllLookupType(string type,string level);

        Task<List<ReservationTypeModel>> GetReservationCategorys(string RegId);  
        
        Task<List<ReservationTypeModel>> GetOfflineAdmissionReservationCategorys();

        Task<List<ReservationTypeModel>> GetReservationCategorysPG(string RegId);
        Task<ServiceResult> testconnection();

    }
}
