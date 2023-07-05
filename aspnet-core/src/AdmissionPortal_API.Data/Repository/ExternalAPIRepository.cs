using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ApiModel.Admin;
using AdmissionPortal_API.Domain.ExternalAPI;
using AdmissionPortal_API.Domain.Model;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Utility.DataConfig;
using AdmissionPortal_API.Utility.EncryptDecrypt;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Utility.ExtensionMethod;
using AdmissionPortal_API.Utility.MessageConfig;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static AdmissionPortal_API.Domain.ExternalAPI.PSEBModel;

namespace AdmissionPortal_API.Data.Repository
{
    public class ExternalAPIRepository : IExternalAPI
    {
        private readonly IMapper _mapper;
        private readonly ILogError _logError;

        private readonly IConfiguration _configuration;
        public ExternalAPIRepository(IConfiguration configuration, IMapper mapper, ILogError logError)
        {
            _mapper = mapper;
            _logError = logError;
            _configuration = configuration;

        }
        public async Task<ServiceResult> GetPSEBAsync(CBSESchoolDetails entity)
        {

            ServiceResult serviceResult = new();

            try
            {

                var client = new RestClient("https://apisetu.gov.in/certificate/v3/pseb/hscer");
                var request = new RestRequest(Method.POST);
                request.AddHeader("accept", "application/pdf");
                request.AddHeader("X-APISETU-CLIENTID", "in.gov.punjab.dgrpg");
                request.AddHeader("X-APISETU-APIKEY", "935f0036a479a45db04e095cf861f740");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\"txnId\":\"c9bf9e57-1685-4c89-bafb-ff5af830be8a\",\"certificateParameters\":{\"rollno\":\"" + entity.rollno + "\",\"year\":\"" + entity.yeartype + "\",\"UID\":\"123412341234\",\"FullName\":\"" + entity.Name + "\"},\"format\":\"xml\",\"consentArtifact\":{\"consent\":{\"consentId\":\"499a5a4a-7dda-4f20-9b67-e24589627061\",\"timestamp\":\"2020-12-17T12:49:06.711Z\",\"dataConsumer\":{\"id\":\"string\"},\"dataProvider\":{\"id\":\"string\"},\"purpose\":{\"description\":\"string\"},\"user\":{\"idType\":\"AADHAAR\",\"idNumber\":\"string\",\"mobile\":\"\",\"email\":\"xyz@gmail.com\"},\"data\":{\"id\":\"string\"},\"permission\":{\"access\":\"VIEWSTORE\",\"dateRange\":{\"from\":\"2020-11-19T07:34:06.711Z\",\"to\":\"2020-12-19T07:34:06.711Z\"},\"frequency\":{\"unit\":\"string\",\"value\":0,\"repeats\":0}}},\"signature\":{\"signature\":\"string\"}}}", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);
                string json = JsonConvert.SerializeXmlNode(doc);
                json = json.Replace("@", "");
                json = json.Replace("swd", "fatherName");

                var list = JsonConvert.DeserializeObject<josn>(json);
               

                if (list.Certificate != null)
                {
                    int n = list.Certificate.CertificateData.Performance.Subjects.Subject.Count;
                    int[] obtAry = new int[n];

                    for (int i = 0; i < list.Certificate.CertificateData.Performance.Subjects.Subject.Count; i++)
                    {
                        int obtMarks = 0;
                        if (!string.IsNullOrEmpty(list.Certificate.CertificateData.Performance.Subjects.Subject[i].name))
                        {
                            int totalMarks = Convert.ToInt32(list.Certificate.CertificateData.Performance.Subjects.Subject[i].marksMax);
                            obtMarks = Convert.ToInt32(list.Certificate.CertificateData.Performance.Subjects.Subject[i].marksTotal);
                            obtAry[i] = GetTotalObtainedMark(obtMarks, totalMarks);
                        }
                    }

                    string str = Convert.ToString(list.Certificate.CertificateData.Performance.result);
                    if(str.Contains("PASS"))
                    {
                        list.Certificate.CertificateData.Performance.result = "Pass";
                    }

                    //var totalobtMarks = (from x in obtAry orderby x descending select x).Take(5).Sum();
                    //list.Certificate.CertificateData.Performance.marksTotal = totalobtMarks.ToString();

                    serviceResult.Message = "Return";
                    serviceResult.ResultData = list;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {

                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = null;
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


            //ServiceResult serviceResult = new ServiceResult();

            //try
            //{
            //    string BaseURL = _configuration["ExternalAPI:PSEBApi"];
            //    var client = new RestClient(BaseURL);
            //    client.Timeout = -1;
            //    var request = new RestRequest(Method.POST);
            //    request.AlwaysMultipartFormData = true;
            //    request.AddParameter("rollno", entity.rollno);
            //    request.AddParameter("doctype", "HSCER");
            //    request.AddParameter("year", entity.yeartype);
            //    request.AddParameter("RequestFrom", "DGR");
            //    IRestResponse response = client.Execute(request);
            //    PSEBResult result = new PSEBResult();
            //    var data = response.Content;
            //    var obj = JsonConvert.DeserializeObject(data);

            //    JObject job = JObject.Parse(obj.ToString());

            //    var ds = job["response"];
            //    data = System.Text.RegularExpressions.Regex.Unescape(data);
            //    data = data.Replace(@"\", string.Empty);
            //    List<Response> list = ds.ToObject<List<Response>>();
            //    // var list = JsonConvert.DeserializeObject<List<PSEBResult>>(ds);



            //    if (list.Count == 0)
            //    {
            //        serviceResult.Message = MessageConfig.NoRecord;
            //        serviceResult.ResultData = null;
            //        serviceResult.Status = false;
            //        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

            //    }
            //    else
            //    {

            //        serviceResult.Message = MessageConfig.Success;
            //        serviceResult.ResultData = list;
            //        serviceResult.Status = true;
            //        serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logError.WriteTextToFile("PSEB API : ", ex.Message, ex.HResult, ex.StackTrace);
            //    serviceResult.Message = ex.Message.ToString();
            //    serviceResult.Status = false;
            //    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            //}
            //return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetCBSEAsync(CBSESchoolDetails entity)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                //var client = new RestClient("https://partners.digitallocker.gov.in/public/verification/api/search/2/xml");
                var client = new RestClient("https://apisetu.gov.in/certificate/v3/cbse/hscer");
                //var client = new RestClient("https://apisetu.gov.in/certificate/v3/cbse/hscer");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("accept", "application/xml");
                request.AddHeader("X-APISETU-CLIENTID", "in.gov.punjab.dgrpg");
                request.AddHeader("X-APISETU-APIKEY", "935f0036a479a45db04e095cf861f740");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\"txnId\":\"f7f1469c-29b0-4325-9dfc-c567200a70f7\",\"format\":\"xml\",\"certificateParameters\":{\"year\":\"" + entity.yeartype + "\",\"rollno\":\"" + entity.rollno + "\",\"FullName\":\"" + entity.Name + "\"},\"consentArtifact\":{\"consent\":{\"consentId\":\"ea9c43aa-7f5a-4bf3-a0be-e1caa24737ba\",\"timestamp\":\"2021-04-27T09:23:09.053Z\",\"dataConsumer\":{\"id\":\"string\"},\"dataProvider\":{\"id\":\"string\"},\"purpose\":{\"description\":\"string\"},\"user\":{\"idType\":\"string\",\"idNumber\":\"string\",\"mobile\":\"9779656539\",\"email\":\"buntykatal12@gmail.com\"},\"data\":{\"id\":\"string\"},\"permission\":{\"access\":\"string\",\"dateRange\":{\"from\":\"2021-04-27T09:23:09.053Z\",\"to\":\"2021-04-27T09:23:09.053Z\"},\"frequency\":{\"unit\":\"string\",\"value\":0,\"repeats\":0}}},\"signature\":{\"signature\":\"string\"}}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);

                //client.Timeout = -1;
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("accept", "application/xml");
                //request.AddHeader("X-APISETU-CLIENTID", "in.gov.punjab.dgrpg");
                //request.AddHeader("X-APISETU-APIKEY", "935f0036a479a45db04e095cf861f740");
                //request.AddHeader("Content-Type", "application/json");
                //request.AddParameter("application/json", "{\"txnId\":\"f7f1469c-29b0-4325-9dfc-c567200a70f7\",\"format\":\"xml\",\"certificateParameters\":{\"year\":\"" + entity.yeartype + "\",\"rollno\":\"" + entity.rollno + "\",\"FullName\":\"" + entity.Name + "\"},\"consentArtifact\":{\"consent\":{\"consentId\":\"ea9c43aa-7f5a-4bf3-a0be-e1caa24737ba\",\"timestamp\":\"2021-04-27T09:23:09.053Z\",\"dataConsumer\":{\"id\":\"string\"},\"dataProvider\":{\"id\":\"string\"},\"purpose\":{\"description\":\"string\"},\"user\":{\"idType\":\"string\",\"idNumber\":\"string\",\"mobile\":\"9779656539\",\"email\":\"buntykatal12@gmail.com\"},\"data\":{\"id\":\"string\"},\"permission\":{\"access\":\"string\",\"dateRange\":{\"from\":\"2021-04-27T09:23:09.053Z\",\"to\":\"2021-04-27T09:23:09.053Z\"},\"frequency\":{\"unit\":\"string\",\"value\":0,\"repeats\":0}}},\"signature\":{\"signature\":\"string\"}}}", ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);

                string json = JsonConvert.SerializeXmlNode(doc);

                json = json.Replace("@", "");
                json = json.Replace("swd", "fatherName");


                //PSEBResult result = new PSEBResult();
                //var data = response.Content;
                //var obj = JsonConvert.DeserializeObject(data);

                //JObject job = JObject.Parse(obj.ToString());

                //var ds = job["response"];
                //data = System.Text.RegularExpressions.Regex.Unescape(data);
                //data = data.Replace(@"\", string.Empty);
                //List<Response> list = ds.ToObject<List<Response>>();
                var list = JsonConvert.DeserializeObject<josn>(json);              
             


                if (list.Certificate != null)
                {
                    int n = list.Certificate.CertificateData.Performance.Subjects.Subject.Count;
                    int[] obtAry = new int[n];

                    for (int i = 0; i < list.Certificate.CertificateData.Performance.Subjects.Subject.Count; i++)
                    {
                        int obtMarks = 0;
                        list.Certificate.CertificateData.Performance.Subjects.Subject[i].marksMax = "100";
                        obtMarks = Convert.ToInt32(list.Certificate.CertificateData.Performance.Subjects.Subject[i].marksTotal);
                        obtAry[i] = GetTotalObtainedMark(obtMarks, 100);


                    }
                    //var totalobtMarks = (from x in obtAry orderby x descending select x).Take(5).Sum();
                    //list.Certificate.CertificateData.Performance.marksTotal = totalobtMarks.ToString();

                    list.Certificate.CertificateData.Performance.marksMax = "500";
                    list.Certificate.CertificateData.Performance.cgpaMax = "500";
                    


                    serviceResult.Message = "Return";
                    serviceResult.ResultData = list;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {

                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = null;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("CBSE API : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> GetRJBSEAsync(CBSESchoolDetails entity)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {

                var client = new RestClient("https://apisetu.gov.in/certificate/v3/rajasthanrajeduboard/hscer");


                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("accept", "application/pdf");
                request.AddHeader("X-APISETU-CLIENTID", "in.gov.punjab.dgrpg");
                request.AddHeader("X-APISETU-APIKEY", "935f0036a479a45db04e095cf861f740");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\"txnId\":\"f7f1469c-29b0-4325-9dfc-c567200a70f7\",\"format\":\"xml\",\"certificateParameters\":{\"ROLL\":\"" + entity.rollno + "\",\"CNAME\":\"" + entity.Name + "\",\"YEAR\":\"" + entity.yeartype + "\"},\"consentArtifact\":{\"consent\":{\"consentId\":\"ea9c43aa-7f5a-4bf3-a0be-e1caa24737ba\",\"timestamp\":\"2021-05-12T09:38:25.505Z\",\"dataConsumer\":{\"id\":\"string\"},\"dataProvider\":{\"id\":\"string\"},\"purpose\":{\"description\":\"string\"},\"user\":{\"idType\":\"string\",\"idNumber\":\"string\",\"mobile\":\"9999999999\",\"email\":\"test@gmail.com\"},\"data\":{\"id\":\"string\"},\"permission\":{\"access\":\"string\",\"dateRange\":{\"from\":\"2021-05-12T09:38:25.505Z\",\"to\":\"2021-05-12T09:38:25.505Z\"},\"frequency\":{\"unit\":\"string\",\"value\":0,\"repeats\":0}}},\"signature\":{\"signature\":\"string\"}}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                //Console.WriteLine(response.Content);

                //client.Timeout = -1;
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("accept", "application/xml");
                //request.AddHeader("X-APISETU-CLIENTID", "in.gov.punjab.dgrpg");
                //request.AddHeader("X-APISETU-APIKEY", "935f0036a479a45db04e095cf861f740");
                //request.AddHeader("Content-Type", "application/json");
                //request.AddParameter("application/json", "{\"txnId\":\"f7f1469c-29b0-4325-9dfc-c567200a70f7\",\"format\":\"xml\",\"certificateParameters\":{\"year\":\"" + entity.yeartype + "\",\"rollno\":\"" + entity.rollno + "\",\"FullName\":\"" + entity.Name + "\"},\"consentArtifact\":{\"consent\":{\"consentId\":\"ea9c43aa-7f5a-4bf3-a0be-e1caa24737ba\",\"timestamp\":\"2021-04-27T09:23:09.053Z\",\"dataConsumer\":{\"id\":\"string\"},\"dataProvider\":{\"id\":\"string\"},\"purpose\":{\"description\":\"string\"},\"user\":{\"idType\":\"string\",\"idNumber\":\"string\",\"mobile\":\"9779656539\",\"email\":\"buntykatal12@gmail.com\"},\"data\":{\"id\":\"string\"},\"permission\":{\"access\":\"string\",\"dateRange\":{\"from\":\"2021-04-27T09:23:09.053Z\",\"to\":\"2021-04-27T09:23:09.053Z\"},\"frequency\":{\"unit\":\"string\",\"value\":0,\"repeats\":0}}},\"signature\":{\"signature\":\"string\"}}}", ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);

                string json = JsonConvert.SerializeXmlNode(doc);

                json = json.Replace("@", "");
                //PSEBResult result = new PSEBResult();
                //var data = response.Content;
                //var obj = JsonConvert.DeserializeObject(data);

                //JObject job = JObject.Parse(obj.ToString());

                //var ds = job["response"];
                //data = System.Text.RegularExpressions.Regex.Unescape(data);
                //data = data.Replace(@"\", string.Empty);
                //List<Response> list = ds.ToObject<List<Response>>();
                var list = JsonConvert.DeserializeObject<josn>(json);



                if (list.Certificate != null)
                {

                    int n = list.Certificate.CertificateData.Performance.Subjects.Subject.Count;
                    int[] obtAry = new int[n];

                    for (int i = 0; i < list.Certificate.CertificateData.Performance.Subjects.Subject.Count; i++)
                    {
                        int obtMarks = 0;
                        int totalMarks = Convert.ToInt32(list.Certificate.CertificateData.Performance.Subjects.Subject[i].marksMax);
                        obtMarks = Convert.ToInt32(list.Certificate.CertificateData.Performance.Subjects.Subject[i].marksTotal);
                        obtAry[i] = GetTotalObtainedMark(obtMarks, totalMarks);

                    }
                    //var totalobtMarks = (from x in obtAry orderby x descending select x).Take(5).Sum();
                    //list.Certificate.CertificateData.Performance.marksTotal = totalobtMarks.ToString();

                    serviceResult.Message = "Return";
                    serviceResult.ResultData = list;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {

                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = null;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("CBSE API : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }
        public async Task<ServiceResult> GetbsehrAsync(CBSESchoolDetails entity)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {

                var client = new RestClient("https://apisetu.gov.in/certificate/v3/bsehr/hscer");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("accept", "application/pdf");
                request.AddHeader("X-APISETU-CLIENTID", "in.gov.punjab.dgrpg");
                request.AddHeader("X-APISETU-APIKEY", "935f0036a479a45db04e095cf861f740");
                request.AddHeader("Content-Type", "application/json");
                //var body = @"{""txnId"":""f7f1469c-29b0-4325-9dfc-c567200a70f7"",""format"":""xml"",""certificateParameters"":{""RROLL"":""3019418582"",""YEAR"":""2019"",""CNAME"":""DHARMESH""},""consentArtifact"":{""consent"":{""consentId"":""ea9c43aa-7f5a-4bf3-a0be-e1caa24737ba"",""timestamp"":""2021-06-09T08:39:21.793Z"",""dataConsumer"":{""id"":""string""},""dataProvider"":{""id"":""string""},""purpose"":{""description"":""string""},""user"":{""idType"":""string"",""idNumber"":""string"",""mobile"":""9999999999"",""email"":""test@gmail.com""},""data"":{""id"":""string""},""permission"":{""access"":""string"",""dateRange"":{""from"":""2021-06-09T08:39:21.793Z"",""to"":""2021-06-09T08:39:21.793Z""},""frequency"":{""unit"":""string"",""value"":0,""repeats"":0}}},""signature"":{""signature"":""string""}}}";
                request.AddParameter("application/json", "{\"txnId\":\"f7f1469c-29b0-4325-9dfc-c567200a70f7\",\"format\":\"xml\",\"certificateParameters\":{\"RROLL\":\"" + entity.rollno + "\",\"YEAR\":\"" + entity.yeartype + "\",\"CNAME\":\"" + entity.Name + "\"},\"consentArtifact\":{\"consent\":{\"consentId\":\"ea9c43aa-7f5a-4bf3-a0be-e1caa24737ba\",\"timestamp\":\"2021-04-27T09:23:09.053Z\",\"dataConsumer\":{\"id\":\"string\"},\"dataProvider\":{\"id\":\"string\"},\"purpose\":{\"description\":\"string\"},\"user\":{\"idType\":\"string\",\"idNumber\":\"string\",\"mobile\":\"9779656539\",\"email\":\"buntykatal12@gmail.com\"},\"data\":{\"id\":\"string\"},\"permission\":{\"access\":\"string\",\"dateRange\":{\"from\":\"2021-04-27T09:23:09.053Z\",\"to\":\"2021-04-27T09:23:09.053Z\"},\"frequency\":{\"unit\":\"string\",\"value\":0,\"repeats\":0}}},\"signature\":{\"signature\":\"string\"}}}", ParameterType.RequestBody);

                //request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                //Console.WriteLine(response.Content);

                //client.Timeout = -1;
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("accept", "application/xml");
                //request.AddHeader("X-APISETU-CLIENTID", "in.gov.punjab.dgrpg");
                //request.AddHeader("X-APISETU-APIKEY", "935f0036a479a45db04e095cf861f740");
                //request.AddHeader("Content-Type", "application/json");
                //request.AddParameter("application/json", "{\"txnId\":\"f7f1469c-29b0-4325-9dfc-c567200a70f7\",\"format\":\"xml\",\"certificateParameters\":{\"year\":\"" + entity.yeartype + "\",\"rollno\":\"" + entity.rollno + "\",\"FullName\":\"" + entity.Name + "\"},\"consentArtifact\":{\"consent\":{\"consentId\":\"ea9c43aa-7f5a-4bf3-a0be-e1caa24737ba\",\"timestamp\":\"2021-04-27T09:23:09.053Z\",\"dataConsumer\":{\"id\":\"string\"},\"dataProvider\":{\"id\":\"string\"},\"purpose\":{\"description\":\"string\"},\"user\":{\"idType\":\"string\",\"idNumber\":\"string\",\"mobile\":\"9779656539\",\"email\":\"buntykatal12@gmail.com\"},\"data\":{\"id\":\"string\"},\"permission\":{\"access\":\"string\",\"dateRange\":{\"from\":\"2021-04-27T09:23:09.053Z\",\"to\":\"2021-04-27T09:23:09.053Z\"},\"frequency\":{\"unit\":\"string\",\"value\":0,\"repeats\":0}}},\"signature\":{\"signature\":\"string\"}}}", ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);

                string json = JsonConvert.SerializeXmlNode(doc);

                json = json.Replace("@", "");
                //PSEBResult result = new PSEBResult();
                //var data = response.Content;
                //var obj = JsonConvert.DeserializeObject(data);

                //JObject job = JObject.Parse(obj.ToString());

                //var ds = job["response"];
                //data = System.Text.RegularExpressions.Regex.Unescape(data);
                //data = data.Replace(@"\", string.Empty);
                //List<Response> list = ds.ToObject<List<Response>>();
                var list = JsonConvert.DeserializeObject<josn>(json);



                if (list.Certificate != null)
                {
                    int n = list.Certificate.CertificateData.Performance.Subjects.Subject.Count;
                    int[] obtAry = new int[n];

                    for (int i = 0; i < list.Certificate.CertificateData.Performance.Subjects.Subject.Count; i++)
                    {
                        int obtMarks = 0;
                        list.Certificate.CertificateData.Performance.Subjects.Subject[i].marksMax = "100";
                        obtMarks = Convert.ToInt32(list.Certificate.CertificateData.Performance.Subjects.Subject[i].marksTotal);
                        obtAry[i] = GetTotalObtainedMark(obtMarks, 100);

                    }
                    //var totalobtMarks = (from x in obtAry orderby x descending select x).Take(5).Sum();
                    //list.Certificate.CertificateData.Performance.marksTotal = totalobtMarks.ToString();

                    list.Certificate.CertificateData.Performance.marksMax = "500";
                    list.Certificate.CertificateData.Performance.cgpaMax = "500";

                    serviceResult.Message = "Return";
                    serviceResult.ResultData = list;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {

                    serviceResult.Message = MessageConfig.Success;
                    serviceResult.ResultData = null;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("CBSE API : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }

        public async Task<ServiceResult> ValidateDocument(DocumentValidate entity, Int32 StudentId, string type)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                CandidateDetails candidate = new CandidateDetails();
                var authenticate_Url = _configuration.GetValue<string>("DocumentVarification:AuthUrl");
                var client = new RestClient(authenticate_Url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\"Username\":\"dgrit\",\"Password\":\"Dgrit@12345\"}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                AuthenticationResponse myDeserializedClass = JsonConvert.DeserializeObject<AuthenticationResponse>(response.Content);
                if (myDeserializedClass.response == 1)
                {
                   var VerifyUrl= _configuration.GetValue<string>("DocumentVarification:VerifyUrl");
                    var clientValidate = new RestClient(VerifyUrl);
                    clientValidate.Timeout = -1;
                    var requestValidate = new RestRequest(Method.POST);
                    requestValidate.AddHeader("Content-Type", "application/json");
                    requestValidate.AddHeader("Session", myDeserializedClass.data.FirstOrDefault().session);
                    requestValidate.AddHeader("Authorization", "Bearer " + myDeserializedClass.data.FirstOrDefault().token);
                    string stringjson = JsonConvert.SerializeObject(entity);
                    requestValidate.AddParameter("application/json", stringjson, ParameterType.RequestBody);
                    IRestResponse responseValidate = clientValidate.Execute(requestValidate);
                    candidate = JsonConvert.DeserializeObject<CandidateDetails>(responseValidate.Content);
                }

                if (candidate.response == 1)
                {
                    SaveValidatedDocument(entity, StudentId, type);
                    serviceResult.Message = candidate.sys_message;
                    serviceResult.ResultData = candidate.data;
                    serviceResult.Status = true;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.OK);

                }
                else
                {

                    serviceResult.Message = candidate.sys_message;
                    serviceResult.ResultData = candidate.data;
                    serviceResult.Status = false;
                    serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
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

        public async void SaveValidatedDocument(DocumentValidate entity, int StudentId, string type)
        {
            int result = 0;
            try
            {
                var values = new { StudentId = StudentId, Candidate_doc_sr_no = entity.Candidate_doc_sr_no, Serviceid = entity.Serviceid };
                var procedure = Procedure.SaveValidatedDocumentSrNo;
                if (type == "PG")
                {
                    procedure = Procedure.SaveValidatedDocumentSrNoPG;
                }
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("SaveValidatedDocument : ", ex.Message, ex.HResult, ex.StackTrace);
            }
        }

        public async Task<int> StudenAlreadyRegisted(CBSESchoolDetails entity)
        {
            int result = 0;
            try
            {
                var values = _mapper.Map<CBSESchoolDetails>(entity);
                var procedure = Procedure.StudenAlreadyRegisted;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    result = Convert.ToInt32(await connection.ExecuteScalarAsync(procedure, values, commandType: CommandType.StoredProcedure));
                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Update College : ", ex.Message, ex.HResult, ex.StackTrace);
            }
            return result;
        }


        static int GetTotalObtainedMark(int obtMarks, int totalmarks)
        {
            try
            {
                int obtMrk = 0;
                return obtMrk = (obtMarks * 100) / totalmarks;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<ServiceResult> StudentMarksVerification()
        {
            ServiceResult serviceResult = new ServiceResult();
            List<SaveRecheckData> lststudent = new List<SaveRecheckData>();
            SaveRecheckData obj = new SaveRecheckData();
            try
            {
                List<studentmarksRechecking> studentmarksRechecking = new List<studentmarksRechecking>();
                var procedure = Procedure.GetStudentMarksForRechecking;
                //_configuration["Passwords:DefaultPassword"]
                //values.Password = Encryption.EncodeToBase64(_configuration["Passwords:DefaultPassword"]);
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var result = await connection.QueryAsync<studentmarksRechecking>(procedure, commandType: System.Data.CommandType.StoredProcedure);
                    studentmarksRechecking = _mapper.Map<List<studentmarksRechecking>>(result);
                }
                for (int j = 0; j < studentmarksRechecking.Count; j++)
                {
                    string rollno = Convert.ToString(studentmarksRechecking[j].Rollno);
                    string Year = Convert.ToString(studentmarksRechecking[j].Yearofpassing);
                    string name = Convert.ToString(studentmarksRechecking[j].Fullname);
                    string getObtainMarks = string.Empty;
                    string gettotalMarks = string.Empty;
                    RestClient client;
                    //var client = new RestClient("https://partners.digitallocker.gov.in/public/verification/api/search/2/xml");
                    if (Convert.ToString(studentmarksRechecking[j].BoardUniversityId) == "50")
                    {
                        client = new RestClient("https://apisetu.gov.in/certificate/v3/pseb/hscer");
                    }
                    else
                    {
                        client = new RestClient("https://apisetu.gov.in/certificate/v3/cbse/hscer");
                    }
                    //var client = new RestClient("https://apisetu.gov.in/certificate/v3/cbse/hscer");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("accept", "application/xml");
                    request.AddHeader("X-APISETU-CLIENTID", "in.gov.punjab.dgrpg");
                    request.AddHeader("X-APISETU-APIKEY", "935f0036a479a45db04e095cf861f740");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", "{\"txnId\":\"f7f1469c-29b0-4325-9dfc-c567200a70f7\",\"format\":\"xml\",\"certificateParameters\":{\"year\":\"" + Year + "\",\"rollno\":\"" + rollno + "\",\"FullName\":\"" + name + "\"},\"consentArtifact\":{\"consent\":{\"consentId\":\"ea9c43aa-7f5a-4bf3-a0be-e1caa24737ba\",\"timestamp\":\"2021-04-27T09:23:09.053Z\",\"dataConsumer\":{\"id\":\"string\"},\"dataProvider\":{\"id\":\"string\"},\"purpose\":{\"description\":\"string\"},\"user\":{\"idType\":\"string\",\"idNumber\":\"string\",\"mobile\":\"9779656539\",\"email\":\"buntykatal12@gmail.com\"},\"data\":{\"id\":\"string\"},\"permission\":{\"access\":\"string\",\"dateRange\":{\"from\":\"2021-04-27T09:23:09.053Z\",\"to\":\"2021-04-27T09:23:09.053Z\"},\"frequency\":{\"unit\":\"string\",\"value\":0,\"repeats\":0}}},\"signature\":{\"signature\":\"string\"}}}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(response.Content);
                        string json = JsonConvert.SerializeXmlNode(doc);
                        json = json.Replace("@", "");
                        var list = JsonConvert.DeserializeObject<josn>(json);
                        if (list.Certificate != null)
                        {
                            getObtainMarks = list.Certificate.CertificateData.Performance.marksTotal;
                            gettotalMarks = list.Certificate.CertificateData.Performance.marksMax == "" ? "500" : list.Certificate.CertificateData.Performance.marksMax;
                        }
                        obj = new SaveRecheckData();
                        obj.studentid = Convert.ToString(studentmarksRechecking[j].studentid);
                        obj.ObtainedMarks = Convert.ToString(studentmarksRechecking[j].ObtainedMarks);
                        obj.TotalMarks = Convert.ToString(studentmarksRechecking[j].TotalMarks);
                        obj.Rollno = Convert.ToString(studentmarksRechecking[j].Rollno);
                        obj.Yearofpassing = Convert.ToString(studentmarksRechecking[j].Yearofpassing);
                        obj.Fullname = Convert.ToString(studentmarksRechecking[j].Fullname);
                        obj.BoardUniversityId = Convert.ToString(studentmarksRechecking[j].BoardUniversityId);
                        obj.NewObtainedMarks = getObtainMarks;
                        obj.NewTotalMarks = gettotalMarks;
                        lststudent.Add(obj);
                    }
                    catch
                    {
                        obj = new SaveRecheckData();
                        obj.studentid = Convert.ToString(studentmarksRechecking[j].studentid);
                        obj.ObtainedMarks = Convert.ToString(studentmarksRechecking[j].ObtainedMarks);
                        obj.TotalMarks = Convert.ToString(studentmarksRechecking[j].TotalMarks);
                        obj.Rollno = Convert.ToString(studentmarksRechecking[j].Rollno);
                        obj.Yearofpassing = Convert.ToString(studentmarksRechecking[j].Yearofpassing);
                        obj.Fullname = Convert.ToString(studentmarksRechecking[j].Fullname);
                        obj.BoardUniversityId = Convert.ToString(studentmarksRechecking[j].BoardUniversityId);
                        obj.NewObtainedMarks = "Not Found";
                        obj.NewTotalMarks = "Not Found";
                        lststudent.Add(obj);
                    }
                    
                }
                var Sprocedure = Procedure.SaveMarksForRechecking;
                ListToDataTableExtension lstSubjectTable = new ListToDataTableExtension();
                DataTable tableSubject = new DataTable();
                tableSubject = lstSubjectTable.ToDataTable<SaveRecheckData>(lststudent);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
                {
                    var values = new
                    {
                        marksverification = tableSubject
                    };

                    int res = Convert.ToInt32(await connection.ExecuteScalarAsync(Sprocedure, values, commandType: CommandType.StoredProcedure));

                }
            }
            catch (Exception ex)
            {
                _logError.WriteTextToFile("Student Marks Verification : ", ex.Message, ex.HResult, ex.StackTrace);
                serviceResult.Message = ex.Message.ToString();
                serviceResult.Status = false;
                serviceResult.StatusCode = Convert.ToInt32(HttpStatusCode.ServiceUnavailable);
            }
            return await Task.Run(() => serviceResult);
        }
    }
}
