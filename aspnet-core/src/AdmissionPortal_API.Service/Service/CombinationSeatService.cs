using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface; 
using AdmissionPortal_API.Utility.MessageConfig;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.Model.CourseFeeHead;
using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using AdmissionPortal_API.Utility.ErrorLog;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace AdmissionPortal_API.Service.Service
{
    public class CombinationSeatService : ICombinationSeatService
    {
        private readonly ICombinationWiseSeat _CombinationWiseSeat;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        private readonly ILogError _logError;
        public CombinationSeatService(ICombinationWiseSeat CombinationWiseSeat, IMapper mapper, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _CombinationWiseSeat = CombinationWiseSeat;
            _mapper = mapper;
            _blobService = blobService;
            _configuration = configuration;
        }
        public async Task<ServiceResult> AddCombinationSeat(CombinationSeat model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
               
                var _result = _CombinationWiseSeat.AddAsyncCourse(model);

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
                _logError.WriteTextToFile("Create Combination Seats: ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> getCoursesFeeHeadByHeadTypeId(int collegeId, int courseId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _CombinationWiseSeat.GetCourseById(collegeId,courseId);

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
                _logError.WriteTextToFile("Get All Combination Seat : ", ex.Message, ex.HResult, ex.StackTrace); ;
            }
            return await Task.Run(() => serviceResult);
        }
    }
}
