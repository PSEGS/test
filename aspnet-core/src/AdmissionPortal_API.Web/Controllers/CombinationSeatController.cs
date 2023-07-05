using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/CombinationSeat")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class CombinationSeatController : Controller
    {
        private readonly ICombinationSeatService _CombinationseatService;
        private readonly ILogError _logError;
        public CombinationSeatController(ICombinationSeatService CombinationseatService, ILogError logError)
        {
            _CombinationseatService = CombinationseatService;
            _logError = logError;
        }
        [HttpPost]
        [Route("AddCombinationSeat")]
        public async Task<IActionResult> AddCombinationSeat([FromBody] CombinationSeat model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.createdBy = Convert.ToInt32(identity.FindFirst("UserID").Value);
                        model.CollegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);

                    }
                    else
                    {
                        model.createdBy = 1;
                        model.CollegeId = 1;

                    }
                    ServiceResult data = await _CombinationseatService.AddCombinationSeat(model);
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
                _logError.WriteTextToFile("Create Combination Seat : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }
        [HttpGet]
        [Route("GetCombinationDetails")]
        public async Task<IActionResult> GetCombinationDetails(int CourseId = 0)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int collegeId = 0;
                string Logintype = string.Empty;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;

                    collegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value); 
                }
                else
                {
                    collegeId = 1;
                    

                }
                data = await _CombinationseatService.getCoursesFeeHeadByHeadTypeId(collegeId, CourseId);
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
                _logError.WriteTextToFile("Get ALL Combination  Details By Course  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
