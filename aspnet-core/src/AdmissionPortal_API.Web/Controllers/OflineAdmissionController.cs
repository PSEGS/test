using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.OfflineAdmisson;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/OfflineAdmission")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]


    public class OflineAdmissionController : Controller
    {
        private readonly Service.ServiceInterface.IOfflineAdmission _offlineAdmissionService;
        private readonly ILogError _logError;
        public OflineAdmissionController(Service.ServiceInterface.IOfflineAdmission offlineAdmissionService, ILogError logError)
        {
            _offlineAdmissionService = offlineAdmissionService;
            _logError = logError;
        }
        [HttpPost]
        [Route("OfflineAdmisson")]
        public async Task<IActionResult> OfflineAdmisson([FromBody] OfflineAdmissionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.CollegeId = Convert.ToInt32(identity.FindFirst("universityCollegeId").Value);
                        if(model.StudentType == "UG")
                        {
                            model.RegistrationNumber = "UO" + model.Passingyear.Substring(model.Passingyear.Length - 2) + DateTime.Today.Year.ToString().Substring(DateTime.Today.Year.ToString().Length - 2) + RandomStringGenerator.GenerateRandomString(10, "Numeric");

                        }
                        else
                        {
                            model.RegistrationNumber = "PO" + model.Passingyear.Substring(model.Passingyear.Length - 2) + DateTime.Today.Year.ToString().Substring(DateTime.Today.Year.ToString().Length - 2) + RandomStringGenerator.GenerateRandomString(10, "Numeric");

                        }

                    }
                    else
                    {
                        model.CollegeId = 0;
                    }
                    ServiceResult data = await _offlineAdmissionService.AddOfflineAdmisson(model);
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
                _logError.WriteTextToFile("Offline Admisson : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }


    }
}
