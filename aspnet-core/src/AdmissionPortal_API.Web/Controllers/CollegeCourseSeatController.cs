﻿using AdmissionPortal_API.Domain.Model;
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
    [Route("api/CollegeCourseSeat")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class CollegeCourseSeatController : Controller
    {
        private readonly ICollegeCourseSeatService _seatService;
        private readonly ILogError _logError;
        public CollegeCourseSeatController(ICollegeCourseSeatService SeatService, ILogError logError)
        {
            _seatService = SeatService;
            _logError = logError;
        }
        [HttpPost]
        [Route("AddCollegeCourseSeat")]
        public async Task<IActionResult> CreateCourseAsync([FromBody] CollegeCourseSeat model)
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
                    ServiceResult data = await _seatService.CreateSeatAsync(model);
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
                _logError.WriteTextToFile("Create collage : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpPost]
        [Route("UpdateCollegeCourseSeat")]
        public async Task<IActionResult> UpdateCourse([FromBody] UpdateCollegeCourseSeat model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        model.ModefyBy = Convert.ToInt32(identity.FindFirst("UserID").Value);

                    }
                    else
                    {
                        model.ModefyBy = 1;

                    }
                    ServiceResult data = await _seatService.UpdateSeat(model);
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
                _logError.WriteTextToFile("update College course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
                return await Task.Run(() => BadRequest(ex.Message));

            }
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteCourse(int Id)
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

                ServiceResult data = await _seatService.DeleteSeat(Id, userid);

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
                _logError.WriteTextToFile("Delete College Course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetAllCollegeCourseSeat/{CollegeId}")]
        public async Task<IActionResult> GetAllCollge(int CollegeId, int pageNumber, int pageSize, string searchKeyword, string sortBy, string sortOrder)
        {
            try
            {
                ServiceResult data = new ServiceResult();

                data = await _seatService.GetAllSeat(CollegeId, pageNumber, pageSize, searchKeyword, sortBy, sortOrder);
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
                _logError.WriteTextToFile("Get ALL College Course Seat : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetCollegeCourseSeatById/{Id}")]
        public async Task<IActionResult> GetcourseByidAsync(int Id)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _seatService.GetSeatById(Id);
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
                _logError.WriteTextToFile("Get College Course Seat By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCollegeSeatMatrixById/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCollegeSeatMatrixById(int Id)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _seatService.GetSeatMatrixById(Id);
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
                _logError.WriteTextToFile("Get College Course Seat Matrix By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetReservationQuota")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReservationQuota()
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _seatService.GetReservationQuota();
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
                _logError.WriteTextToFile("Get College Course Seat Matrix By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("GetPGCollegeSeatMatrixById/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPGCollegeSeatMatrixById(int Id)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                data = await _seatService.GetPGSeatMatrixById(Id);
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
                _logError.WriteTextToFile("Get PG College Course Seat Matrix By Id : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("lockUnlockByCollegeID/{collegeID}/{status}")]
        public async Task<IActionResult> LockUnlockCourseSeatsByCollegeID(Int32 collegeID, int status)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int userid = 0;
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
                data = await _seatService.LockUnlockCourseSeatsByCollegeID(collegeID, status, userid);
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
                _logError.WriteTextToFile("College Course seats lock unlock : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("UG-LockUnlockSeatMatrixByCollegeID/{collegeID}/{status}")]
        public async Task<IActionResult> UGlockUnlockSeatMatrixByCollegeID(Int32 collegeID, int status)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int userid = 0;
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
                data = await _seatService.lockUnlockSeatMatrixByCollegeID(collegeID, status, userid);
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
                _logError.WriteTextToFile("College Course seatMatrix lock unlock : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
        [HttpPut]
        [Route("PG-LockUnlockSeatMatrixByCollegeID/{collegeID}/{status}")]
        public async Task<IActionResult> PGLockUnlockSeatMatrixByCollegeID(Int32 collegeID, int status)
        {
            try
            {
                ServiceResult data = new ServiceResult();
                int userid = 0;
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
                data = await _seatService.PGLockUnlockSeatMatrixByCollegeID(collegeID, status, userid);
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
                _logError.WriteTextToFile("College Course seatMatrix lock unlock : ", ex.Message, ex.HResult, ex.StackTrace);
                return BadRequest(ex);
            }
        }
    }
}
