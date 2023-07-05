using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/LookUpAPI")]
    public class LookupAPIController : Controller
    {
        public readonly ILookUpApi _lookupAPIService;
        private readonly ILogError _logError;
        public LookupAPIController(ILookUpApi lookUpAPIRepository, ILogError logError)
        {
            _lookupAPIService = lookUpAPIRepository;
            _logError = logError;
        }
        [HttpGet]
        [Route("CollegeType")]

        public async Task<IActionResult> GetCollegeType()
        {
            try
            {
                ServiceResult objData = new ServiceResult();
                ServiceResult data = await _lookupAPIService.GetAllCollegeType();
                
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("College Type : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }
        [HttpGet]
        [Route("CollegeMode")]
        public async Task<IActionResult> GetCollegeMode()
        {
            try
            {
                ServiceResult data = await _lookupAPIService.GetAllCollegeMode();
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("College Mode : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }
        [HttpGet]
        [Route("CourseType")]
        public async Task<IActionResult> CourseType()
        {
            try
            {
                ServiceResult data = await _lookupAPIService.GetAllCourseType();
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("College Mode : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }
        [HttpGet]
        [Route("GetLookupType/{type}")]
        public async Task<IActionResult> GetLookupType(string type, string level)
        {
            try
            {
                ServiceResult data = await _lookupAPIService.GetLookupType(type, level);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("College Mode : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpGet]
        [Route("GetReservationCategorys/{RegId}")]
        public async Task<IActionResult> GetReservationCategorys(string RegId)
        {
            try
            {
                ServiceResult data = await _lookupAPIService.GetReservationCategorys(RegId);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Reservation Categorys : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpGet]
        [Route("GetReservationCategorysPG/{RegId}")]
        public async Task<IActionResult> GetReservationCategorysPG(string RegId)
        {
            try
            {
                ServiceResult data = await _lookupAPIService.GetReservationCategorysPG(RegId);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Reservation Categorys : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpGet]
        [Route("GetOfflineAdmissionReservationCategorys")]
        public async Task<IActionResult> GetOfflineAdmissionReservationCategorys()
        {
            try
            {
                ServiceResult data = await _lookupAPIService.GetOfflineAdmissionReservationCategorys();
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get Offline Admission Reservation Categorys : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }
        [HttpGet]
        [Route("testconnection")]
        public async Task<IActionResult> testconnection()
        {
            try
            {
                ServiceResult data = await _lookupAPIService.testconnection();
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else if (data.StatusCode == 404)
                {
                    return await Task.Run(() => NotFound(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("College Mode : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }
    }
}
