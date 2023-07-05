using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ExternalAPI;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using AdmissionPortal_API.Utility.Notification;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.UserTypeEnum;

namespace AdmissionPortal_API.Service.Service
{
    public class ExternalAPIService : IExternalAPIService
    {
        private readonly IMapper _mapper;
        private readonly IExternalAPI _ExternalAPIService;
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        private readonly IBlobService _blobService;
        public ExternalAPIService(IMapper mapper, IExternalAPI ExternalAPIService, IConfiguration configuration, ILogError logError, IBlobService blobService)
        {
            _logError = logError;
            _mapper = mapper;
            _ExternalAPIService = ExternalAPIService;
            _configuration = configuration;
            _blobService = blobService;

        }
        public async Task<int> AddBoardSubjectAsync(BoardSubject entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<BoardSubject>(entity);
                var procedure = Procedure.SaveBoardSubject;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("add board subject : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }
        public async Task<ServiceResult> GetStudentInfoFromBoard(string Type, CBSESchoolDetails model)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var obj = _mapper.Map<CBSESchoolDetails>(model);
                ServiceResult result = new ServiceResult();
                int resData = 0;
                resData = await _ExternalAPIService.StudenAlreadyRegisted(model);
                if (resData == 1)
                {
                    result.StatusCode = 0;
                    result.ResultData = null;
                }
                else
                {
                    switch (Type)
                    {
                        case "PSEBS":
                            result = await _ExternalAPIService.GetPSEBAsync(obj);
                            break;
                        case "CBSE":
                            result = await _ExternalAPIService.GetCBSEAsync(obj);
                            break;
                        case "RJBSE":
                            result = await _ExternalAPIService.GetRJBSEAsync(obj);
                            break;
                        case "HR":
                            result = await _ExternalAPIService.GetbsehrAsync(obj);
                            break;
                        default:
                            result.Message = MessageConfig.Success;
                            result.Status = true;
                            result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                            break;
                    }
                }

                if (result.StatusCode == 0)
                {
                    serviceResult.Message = MessageConfig.RollNoExist;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {
                    if (result.ResultData != null)
                    {
                        josn apiResult = result.ResultData;
                        if (apiResult != null)
                        {
                            if (apiResult.Certificate.CertificateData.Performance.Subjects != null)
                            {
                                foreach (var item in apiResult.Certificate.CertificateData.Performance.Subjects.Subject)
                                {
                                    var subjectName = item.name;
                                    BoardSubject boardSubject = new BoardSubject();
                                    boardSubject.Subject = item.name;
                                    boardSubject.Type = Type;
                                    await AddBoardSubjectAsync(boardSubject);
                                }
                            }
                        }
                    }
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result.ResultData;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("PSEB API : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> ValidateDocument(DocumentValidate model, Int32 StudentId, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var result = await _ExternalAPIService.ValidateDocument(model, StudentId, type);
                if (result.StatusCode == 404)
                {
                    serviceResult.Message = MessageConfig.CandidateNotFound;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);

                }
                else
                {
                    serviceResult.Message = result.Message;
                    serviceResult.ResultData = result.ResultData;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Validate Document : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> StudentMarksVerification()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //var obj = _mapper.Map<CBSESchoolDetails>(model);
                ServiceResult result = new ServiceResult();
                
                result = await _ExternalAPIService.StudentMarksVerification();

                if (result.ResultData != null)
                {

                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = result.ResultData;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Marks Verification : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }

    }
}
