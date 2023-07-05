using AdmissionPortal_API.Domain.ApiModel.PGObjection;
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
    [Route("api/pgobjection")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class PGObjectionController : Controller
    {
        private readonly IPGObjectionService _pGObjectionService;
        private readonly ILogError _logError;
        public PGObjectionController(IPGObjectionService pGObjectionService, ILogError logError)
        {
            _pGObjectionService = pGObjectionService;
            _logError = logError;
        }

        /// <summary>
        /// Create objection
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddObjection")]
        public async Task<IActionResult> CreateObjectionAsync([FromBody] AddPgObjection model)
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
                    ServiceResult data = await _pGObjectionService.CreateObjectionAsync(model);
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
                _logError.WriteTextToFile("Create PG objection : ", ex.Message, ex.HResult, ex.StackTrace);
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

                data = await _pGObjectionService.GetAllObjectionsByRegId(RegId);
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
                _logError.WriteTextToFile("Get all PG objections by reg id  : ", ex.Message, ex.HResult, ex.StackTrace);
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

                data = await _pGObjectionService.GetAllObjectionsByCollegeId(CollegeId);
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
                _logError.WriteTextToFile("Get All PG Objections by college id : ", ex.Message, ex.HResult, ex.StackTrace);
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
                data = await _pGObjectionService.GetObjectionById(ObjectionId);
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

