using AdmissionPortal_API.Domain.ApiModel.Section;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/section")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly ILogError _logError;
        public SectionController(ISectionService sectionService, ILogError logError)
        {
            _sectionService = sectionService;
            _logError = logError;
        }

        [HttpPost]
        [Route("CreateSection")]
        public async Task<IActionResult> CreateSectionAsync([FromBody] AddSection model)
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
                    ServiceResult data = await _sectionService.CreateSection(model);
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
                _logError.WriteTextToFile("Create section: ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpPost]
        [Route("UpdateSection")]
        public async Task<IActionResult> UpdateSectionAsync([FromBody] UpdateSection model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.ModifiedBy = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.ModifiedBy = 1;

                    }
                    ServiceResult data = await _sectionService.UpdateSection(model);
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
                _logError.WriteTextToFile("Update Section : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{sectionId}")]
        public async Task<IActionResult> DeleteSectionAsync(int sectionId)
        {
            try
            {
                Int32 userid = 0;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    userid = Convert.ToInt32(identity.FindFirst("UserID").Value);

                }
                else
                {
                    userid = 1;
                }
                ServiceResult data = await _sectionService.DeleteSection(sectionId);

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
                _logError.WriteTextToFile("Delete Section : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{sectionId}")]
        public async Task<IActionResult> GetSectionByIdAsync(int sectionId)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _sectionService.GetSectionById(sectionId);
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
                _logError.WriteTextToFile("Get Section By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllSection")]
        public async Task<IActionResult> GetAllSection(int? pageNumber, int? pageSize, string? searchKeyword, string? sortBy, string? sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _sectionService.GetAllSection(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get ALL Section  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
