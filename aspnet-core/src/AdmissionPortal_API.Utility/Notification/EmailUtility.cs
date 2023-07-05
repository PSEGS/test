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
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Utility.Notification
{
    public class EmailUtility : IEmail
    {
        private readonly IConfiguration _configuration;
        private readonly ILogError _logError;
        public EmailUtility(IConfiguration configuration, ILogError logError)
        {
            _configuration = configuration;
            _logError = logError;
        }
        public async Task<ServiceResult> SendEmailAsync(string EmailTo, string Message, string Subject)
        {
            ServiceResult result = new ServiceResult();

            result.Message = "mail sent ";
            result.Status = true;
            result.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
            try
            {
                var Sender = _configuration.GetValue<string>("EmailConfiguration:Sender");
                var Header_Key = _configuration.GetValue<string>("EmailConfiguration:ServerKey");
                var ApiUrl = _configuration.GetValue<string>("EmailConfiguration:Url");
                var mailObject = new
                {
                    recipient_email = EmailTo,
                    subject = Subject,
                    body = Message,
                    sender = Sender
                };

                string json = JsonConvert.SerializeObject(mailObject);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Server-Key", Header_Key);
                var response = await client.PostAsync(ApiUrl, data);
                string emailResult = await response.Content.ReadAsStringAsync();
                dynamic rslt = JObject.Parse(emailResult);
                result.Message = rslt.message;
                result.Status = rslt.status;
                result.StatusCode = rslt.code;
                result.ResultData = rslt.data;
                client.Dispose();
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Sent Email Error : ", ex.Message, ex.HResult, ex.StackTrace);
                result.Message = ex.Message.ToString();
                result.Status = false;
                result.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                result.ResultData = null;
            }
            return await Task.Run(() => result);
        }
    }
}
