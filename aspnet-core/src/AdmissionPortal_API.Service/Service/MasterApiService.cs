using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.MessageConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.Service
{
    public class MasterApiService : IMasterApiService
    {
        private readonly IMasterApiRepository _masterApiRepository;
        private readonly ILogError _logError;
        public MasterApiService(IMasterApiRepository masterApiRepository, ILogError logError)
        {
            _masterApiRepository = masterApiRepository;
            _logError = logError;
        }

        public async Task<ServiceResult> GetBoard()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetBoard();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Board : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetReligion()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetReligion();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Religion : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetReservationCategory()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetReservationCategory();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Reservation Category : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetReservationSubCategory(int id, int CatId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetReservationSubCategory(id, CatId);

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Reservation Sub Category : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetOccupation()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetOccupation();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Occupation : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> GetFatherOccupation()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetFatherOccupation();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Occupation : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> GetBaordSubject()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetOccupation();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Occupation : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> GetBoardSubject()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetBoardSubject();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get board subject : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> GetHouseholdIncome()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetHouseholdIncome();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get HouseholdIncome : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> BankMaster()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.BankMaster();
                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Bank Master : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetIsLockDetails(int collegeid)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetIsLockDetails(collegeid);
                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Bank Master : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetCountries()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _result = _masterApiRepository.GetCountries();

                if (_result != null)
                {
                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = _result.Result;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
                else
                {
                    serviceResult.Message = MessageConfig.ErrorOccurred;
                    serviceResult.ResultData = null;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                serviceResult.Message = MessageConfig.ErrorOccurred;
                serviceResult.ResultData = null;
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("Get Countries : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => serviceResult);
        }
    }
}
