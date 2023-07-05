using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogError _logError;

        public DepartmentService(IDepartmentRepository departmentRepository,ILogError logError)
        {
            _logError = logError;
            _departmentRepository = departmentRepository;
        }
        public ServiceResult GetAllDepartment()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _departmentRepository.GetAllAsync();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {

                _logError.WriteTextToFile("Get All Department : " , ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return serviceResult;
        }
    }
}

