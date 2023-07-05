using AdmissionPortal_API.Domain.ApiModel.CitizenManagement;
using AdmissionPortal_API.Domain.ApiModel.University;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/university")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class UniversityController : Controller
    {
        private readonly IUniversityService _universityService;
        private readonly ILogError _logError;
        public UniversityController(IUniversityService universityService, ILogError logError)
        {
            _universityService = universityService;
            _logError = logError;
        }

        /// <summary>
        /// Create University
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateUniversity([FromBody] AddUniversity model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.CreatedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.CreatedBy = 1;
                    }
                    ServiceResult data = await _universityService.CreateUniversity(model);
                    if (data.Status == true)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(data));
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create University : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex));
            }
        }

        [HttpPost]
        [Route("UpdateUniversity")]

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUniversity([FromBody] UpdateUniversity model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.CreatedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.CreatedBy = 1;

                    }
                    ServiceResult data = await _universityService.UpdateUniversity(model);
                    if (data.StatusCode == 200)
                    {
                        return await Task.Run(() => Ok(data));
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(data));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update college : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }




        /// <summary>
        /// Get All University
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllUniversity(int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder, Boolean onBoard)
        {
            try
            {
                var data = await _universityService.GetAllUniversity(pageNumber, pageSize, searchKeyword, sortBy, sortOrder,onBoard);
                if (data.Status)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return BadRequest(data);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All University : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get   Colleges by Id
        /// </summary>
        /// <param name="Collegeid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{universityId}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetuniversityByidAsync(int universityId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _universityService.GetUniversityById(universityId);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get College By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Delete Citizen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{universityId}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUniversity(int universityId)
        {
            try
            {
                int UserId = 1;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    UserId = Convert.ToInt32(identity.FindFirst("UserID").Value);

                }

                var data = await _universityService.DeleteUniversity(universityId, UserId);
                if (data.StatusCode == 200)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return await Task.Run(() => BadRequest(data));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Delete University : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get All Universities
        /// </summary>
        [HttpGet("GetAllUniversities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUniversities()
        {
            try
            {
                var data = await _universityService.GetAllUniversities();
                if (data.Status)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return BadRequest(data);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All University without param : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get All Universities for pg registration
        /// </summary>
        [HttpGet("GetAllUniversitiesPG")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUniversitiesPG()
        {
            try
            {
                var data = await _universityService.GetAllUniversitiesPG();
                if (data.Status)
                {
                    return await Task.Run(() => Ok(data));
                }
                else
                {
                    return BadRequest(data);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Get All University without param : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

