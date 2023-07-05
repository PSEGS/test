using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Utility.DataConfig;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Web.Middlewares
{
    public class AppMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IWritableOptions<Domain.ViewModel.AppVersion> _writableLocations;
        public AppMiddleware(RequestDelegate next, IConfiguration configuration, IWritableOptions<Domain.ViewModel.AppVersion> writableLocations)
        {
            _next = next;
            _configuration = configuration;
            _writableLocations = writableLocations;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _writableLocations.Update(opt => {
                    opt.API_Version_DB = GetAppVersionFromDB(_configuration);
                });

                if (context.Request == null || string.IsNullOrEmpty(Convert.ToString(context.Request.Headers["x-api-version"])))
                {
                    await BindResponse(context, "x-api-version is missing.");
                }
                else if ((Convert.ToString(context.Request.Headers["x-api-version"]) != _configuration["AppVersion:API_Version_APP"]) ||
                    Convert.ToString(context.Request.Headers["x-api-version"]) != _configuration["AppVersion:API_Version_DB"])
                {
                    await BindResponse(context, "API version mismatch.");
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
            catch (Exception ex)
            {
                //log exception
            }
        }

        public static Task BindResponse(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new ServiceResult()
            {
                Message = message,
                ResultData = "",
                Status = false,
                StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest)
            });

            return context.Response.WriteAsync(result);
        }

        public static string GetAppVersionFromDB(IConfiguration _configuration)
        {
            string version = string.Empty;
            using (var con = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = con;
                command.CommandText = Procedure.GetAppVersion;

                
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        version = reader.GetString(0);
                    }
                }

                return version;
            }
        }
    }
}
