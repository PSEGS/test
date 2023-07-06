using AdmissionPortal_API.Utility.DataConfig;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Utility.ErrorLog
{
    public class LogError :ILogError
    {
        private readonly IConfiguration _configuration;
        public LogError(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// WriteTextToFile
        /// </summary>
        /// <param name="message"></param>
        public void WriteTextToFile(string functionName, string message, int errorCode,string stackTrace)
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + "\\ErrorLog";
            if (!Directory.Exists(filePath))
            {
                DirectoryInfo di = Directory.CreateDirectory(filePath);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(filePath));
            }

            filePath = filePath + "\\ExceptionError_" + DateTime.Now.ToString("ddMMMyyyy") + ".log";
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(functionName + Environment.NewLine+ message + Environment.NewLine  + errorCode + Environment.NewLine+ stackTrace);
                streamWriter.Close();
            }
            var result= SaveLogError(functionName, message, errorCode,stackTrace);
        }

        public async Task<int> SaveLogError( string functionName, string message, int errorCode, string stackTrace)
        {
            int result = 0;
            var value = new { FunctionName=functionName, ErrorMessage = message, StatusCode = errorCode, StackTrace = stackTrace};
            string procedure = Procedure.SaveLog;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                result = Convert.ToInt32( await connection.ExecuteScalarAsync(procedure, value, commandType: CommandType.StoredProcedure));
            }
            return result;
        }
    }
}
