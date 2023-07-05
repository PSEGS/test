using AdmissionPortal_API.Domain.ApiModel.Objection;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/objection")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class ObjectionController : Controller
    {
        private readonly IObjectionService _objectionService;
        private readonly ILogError _logError;
        public ObjectionController(IObjectionService objectionService, ILogError logError)
        {
            _objectionService = objectionService;
            _logError = logError;
        }

        /// <summary>
        /// Create objection
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddObjection")]
        public async Task<IActionResult> CreateObjectionAsync([FromBody] AddObjection model)
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
                        model.CreatedBy = 0;
                        model.CollegeId = 0;

                    }
                    ServiceResult data = await _objectionService.CreateObjectionAsync(model);
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
                    return await Task.Run(() => BadRequest(ModelState));

                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Create objection : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }


        /// <summary>
        /// Get All College
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetAllObjectionByRegId/{RegId}")]

        public async Task<IActionResult> GetAllObjectionsByRegId(string RegId)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _objectionService.GetAllObjectionsByRegId(RegId);
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
                _logError.WriteTextToFile("Get all objections by reg id  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetAllObjectionByCollegeId/{CollegeId}")]

        public async Task<IActionResult> GetAllObjectionsByCollegeId(string CollegeId)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _objectionService.GetAllObjectionsByCollegeId(CollegeId);
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
                _logError.WriteTextToFile("Get All Objections by college id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get objection by Id
        /// </summary>
        /// <param name="Collegeid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{ObjectionId}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetObjectionByIdAsync(int ObjectionId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _objectionService.GetObjectionById(ObjectionId);
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
                _logError.WriteTextToFile("Get Objection By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}

