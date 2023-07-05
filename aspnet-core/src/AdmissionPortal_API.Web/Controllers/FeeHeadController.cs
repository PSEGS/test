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
using static AdmissionPortal_API.Domain.Model.FeeHead;

namespace AdmissionPortal_API.Web.Controllers
{
    [Route("api/FeeHead")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class FeeHeadController : Controller
    {
        private readonly IFeeHeadService _feeheadService;
        private readonly ILogError _logError;
        public FeeHeadController(IFeeHeadService feeheadService, ILogError logError)
        {
            _feeheadService = feeheadService;
            _logError = logError;
        }
        [HttpPost]
        [Route("AddFeeHead")]
        public async Task<IActionResult> CreateFeeheadAsync([FromBody] AddFeeHead model)
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
                    ServiceResult data = await _feeheadService.CreateFeeHeadAsync(model);
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
                _logError.WriteTextToFile("Create FeeHead : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpPost]
        [Route("UpdateFeeHead")]
        public async Task<IActionResult> Updatefeehead([FromBody] UpdateFeeHead model)
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
                    ServiceResult data = await _feeheadService.UpdateFeeHead(model);
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
                _logError.WriteTextToFile("update fee head : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteFeeHead(int Id)
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

                ServiceResult data = await _feeheadService.DeleteFeeHead(Id, userid);

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
                _logError.WriteTextToFile("Delete FeeHead : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetAllFeeHead")]
        public async Task<IActionResult> GetAllFeeHead(int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _feeheadService.GetAllFeeHead(pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get ALL FeeHead  : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetFeeHeadById/{Id}")]
        public async Task<IActionResult> GetFeeheadByidAsync(int Id)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _feeheadService.GetFeeHeadById(Id);
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
                _logError.WriteTextToFile("Get Fee head By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
