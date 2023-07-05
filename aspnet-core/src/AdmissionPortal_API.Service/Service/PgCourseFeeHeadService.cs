using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.MessageConfig;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.PgCourseFeeHead;
using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace AdmissionPortal_API.Service.Service
{
    public class PgCourseFeeHeadService : IPgCourseFeeHeadService
    {
        private readonly IPgCourseFeeHeadRepository _pgCourseFeeHead;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;

        public PgCourseFeeHeadService(IPgCourseFeeHeadRepository pgCourseFeeHead, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _pgCourseFeeHead = pgCourseFeeHead;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }
        public async Task<ServiceResult> AddCourseFeeHead(List<AddPgCourseFeeHead> model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<List<AddPgCourseFeeHead>>(model);
                var _result = _pgCourseFeeHead.AddMapping(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.AlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Create College Mapping: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> AddCourseFeeHeadWithCollegeType(List<AddPgCourseFeeHeadFee> model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<List<AddPgCourseFeeHeadFee>>(model);
                var _result = _pgCourseFeeHead.AddCourseFeeHeadFee(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.AlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Create Course Head Fee: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> AddCourseWaveoffFee(PgCourseFeeWaveOff model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<PgCourseFeeWaveOff>(model);
                var _result = _pgCourseFeeHead.AddCourseWaveOff(obj);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData == 1)
                    {
                        serviceResult.Message = MessageConfig.RecordSaved;
                        serviceResult.Status = true;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else if (serviceResult.ResultData == 2)
                    {
                        serviceResult.Message = MessageConfig.AlreadyExists;
                        serviceResult.ResultData = 2;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        serviceResult.Message = MessageConfig.ErrorOccurred;
                        serviceResult.ResultData = null;
                        serviceResult.Status = false;
                        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Create Course Wave off Fee: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetHeadByLoginType(int Id, int CollegeType, Int32 CreatedBy,Int32 CourseFundType)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _pgCourseFeeHead.GetAllFeeHeadByLoginType(Id, CollegeType, CreatedBy, CourseFundType);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = MessageConfig.Success;
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
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get All Course   fee by college id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetAllFeeDetailsCourseID(Int32 universityId, Int32 collegeID, Int32 CourseID, string mode)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _pgCourseFeeHead.GetALLFeeDetailsByCourseID(universityId, collegeID, CourseID, mode);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = MessageConfig.Success;
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
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get All Course wise fee Details : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public Task<ServiceResult> GetAllFeeHead()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> GetcoursefeeBYCollegeId(int Id)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _pgCourseFeeHead.GetCourseFeebyColegeId(Id);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = MessageConfig.Success;
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
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get All Course   fee by college id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetCoursesFeeHeadByHeadTypeId(int Id)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _pgCourseFeeHead.GetCourseFeebyColegeId(Id);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = MessageConfig.Success;
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
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get All fee Head by Logintype : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetFeeWaveOffDetails(int HeadId, int CollegeType, int UniversityCollegeId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _pgCourseFeeHead.GetWaveOffDetailsById(HeadId, CollegeType, UniversityCollegeId);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {
                        serviceResult.Message = MessageConfig.Success;
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
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("Get wave Off Details by id : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> LockUnlockCourseFeeHeadByCollegeID(int collegeId, int Status, int modifiedBy)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _pgCourseFeeHead.LockUnlockCourseFeeHeadByCollegeID(collegeId, Status, modifiedBy);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
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
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("College PG Course Head Fee Lock Unlock : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> LockUnlockCourseFeeHeadByUniversityID(int UniversityId, int Status, int modifyBy)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var _result = _pgCourseFeeHead.LockUnlockCourseFeeHeadByUniversityID(UniversityId, Status, modifyBy);

                if (_result != null)
                {
                    serviceResult.ResultData = _result.Result;
                    if (serviceResult.ResultData != null)
                    {

                        serviceResult.Message = MessageConfig.Success;
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
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                _logError.WriteTextToFile("College PG Course Head Fee Lock Unlock : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }
    }
}
