using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Utility.ErrorLog;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Utility.Notification
{
    public class SMSUtility : ISms
    {
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public SMSUtility(IConfiguration configuration, ILogError logError)
        {
            _configuration = configuration;
            _logError = logError;
        }
        public async Task<ServiceResult> SendSmsAsync(string Moblile, string Message, string TemplateId)
        {
            ServiceResult responseResult = new ServiceResult();
            responseResult.Message = "message sent to " + Moblile;
            responseResult.Status = true;
            responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
            try
            {
                using (var messageclient = new HttpClient())
                {
                    object jsonbody = new { mobile_no = Moblile, message = Message, template_id = TemplateId, is_unicode = false };
                    messageclient.BaseAddress = new Uri("https://eapi.punjab.gov.in");
                    messageclient.DefaultRequestHeaders.Accept.Clear();
                    messageclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    messageclient.DefaultRequestHeaders.Add("Server-Key", "6eNRiZjTg53A5xEXqIOe8lt3dXEBJ4C4pMi174M6u4Vz2V1OS5IjLNFytVeE9pAc");
                    var contentbody = new StringContent(JsonConvert.SerializeObject(jsonbody), Encoding.UTF8, "application/json");
                    //sms/v1
                    HttpResponseMessage responsemessage = messageclient.PostAsync("/smapi/sms", contentbody).Result;
                    if (responsemessage.IsSuccessStatusCode)
                    {
                        responseResult.Message = "message sent to " + Moblile;
                        responseResult.Status = true;
                        responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                    else
                    {
                        responseResult.Message = responsemessage.Content.ReadAsStringAsync().Result;
                        responseResult.Status = false;
                        responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                    }
                }

            }
            catch (Exception ex)
            {
                responseResult.Message = ex.Message.ToString();
                responseResult.Status = false;
                responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
                _logError.WriteTextToFile("SMS Error", Moblile, responseResult.StatusCode, "smserror");
                // _logError.WriteTextToFile("SMS Error", ex.Message.ToString(), ex.HResult, ex.StackTrace);
            }
            return await Task.Run(() => responseResult);
        }
        //public async Task<ServiceResult> SendSmsAsync(string Moblile, string Message, string TemplateId)
        //{
        //    ServiceResult responseResult = new ServiceResult();
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            object json = new { Access_Key = "Loa48YherqIoXb92Gte3", Public_Key = "EdfRhswWcXkRaE4rcPo9" };
        //            client.BaseAddress = new Uri("http://eapi.punjab.gov.in/");
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            var content = new StringContent(JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8, "application/json");
        //            HttpResponseMessage response = client.PostAsync("g2gapiaccess/generateToken", content).Result;

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string jsonstring = response.Content.ReadAsStringAsync().Result;
        //                var result = JsonConvert.DeserializeObject<messageApiResponse>(jsonstring);
        //                if (result.sys_message.Length > 0)
        //                {
        //                    using (var messageclient = new HttpClient())
        //                    {
        //                        object jsonbody = new { mobileno = Moblile, message = Message, Template_Id = TemplateId };
        //                        messageclient.BaseAddress = new Uri("http://eapi.punjab.gov.in/");
        //                        messageclient.DefaultRequestHeaders.Accept.Clear();
        //                        messageclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                        messageclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.sys_message);
        //                        var contentbody = new StringContent(JsonConvert.SerializeObject(jsonbody), System.Text.Encoding.UTF8, "application/json");
        //                        //sms/v1
        //                        HttpResponseMessage responsemessage = messageclient.PostAsync("sms/v1", contentbody).Result;
        //                        if (responsemessage.IsSuccessStatusCode)
        //                        {
        //                            responseResult.Message = "message sent to " + Moblile;
        //                            responseResult.Status = true;
        //                            responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //                        }
        //                        else
        //                        {
        //                            responseResult.Message = responsemessage.Content.ReadAsStringAsync().Result;
        //                            responseResult.Status = false;
        //                            responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    responseResult.Message = response.Content.ReadAsStringAsync().Result;
        //                    responseResult.Status = false;
        //                    responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
        //                    _logError.WriteTextToFile("SMS Error", response.Content.ReadAsStringAsync().Result, responseResult.StatusCode, "smserror");
        //                }
        //            }
        //            else
        //            {
        //                responseResult.Message = response.Content.ReadAsStringAsync().Result;
        //                responseResult.Status = false;
        //                responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
        //                _logError.WriteTextToFile("SMS Error", response.Content.ReadAsStringAsync().Result, responseResult.StatusCode, "smserror");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseResult.Message = ex.Message.ToString();
        //        responseResult.Status = false;
        //        responseResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
        //        _logError.WriteTextToFile("SMS Error", ex.Message.ToString(), ex.HResult, ex.StackTrace);
        //    }
        //    return await Task.Run(() => responseResult);
        //}




        public async Task<string> GenerateRandomOTP()
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;
            int iOTPLength = 6;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

    }
    public class messageApiResponse
    {
        public string response { get; set; }
        public string data { get; set; }
        public string sys_message { get; set; }
        public string response_code { get; set; }

    }
}
