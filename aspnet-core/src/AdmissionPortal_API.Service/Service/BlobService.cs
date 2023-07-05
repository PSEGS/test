using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Azure.Storage.Blobs;
using AdmissionPortal_API.Service.ServiceInterface;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Sas;

namespace AdmissionPortal_API.Service.Service
{
    public class BlobService : IBlobService
    {

        private IConfiguration _configuration;
        public BlobService(IConfiguration _Configuration)
        {
            _configuration = _Configuration;
        }

        /// <summary>
        /// UploadBlobUsingFileReferenceType
        /// </summary>
        /// <param name="accessKey">Azure storage connection string</param>
        /// <param name="containerName">Name of Azure storage container</param>
        /// <param name="base64">File content base64 string</param>
        /// <param name="entityID">Optional ID of entity for which the document is stored.
        /// This ID will be added in th file name as it is created on Azure storage</param>
        /// <returns></returns>
        public async Task<ServiceResult> UploadBlobUsingFileReferenceType(string accessKey, string containerName, string base64, string entityID = "")
        {
            ServiceResult _responseModel = new ServiceResult();
            try
            {
                string mimeType = GetFileExtension(base64);
                byte[] bytes = System.Convert.FromBase64String(base64);
                if (bytes.Length > 500000) // 500 KB
                {
                    _responseModel.StatusCode = 3;
                    _responseModel.ResultData = null;
                    _responseModel.Message = "Please upload document upto 500 KB.";
                    return _responseModel;
                }
                if (mimeType.ToLower() == "jpg" || mimeType.ToLower() == "png" || mimeType.ToLower() == "pdf")
                {
                    //Check if file content is null
                    if (string.IsNullOrEmpty(base64))
                    {
                        _responseModel.StatusCode = 0;
                        _responseModel.Message = "Base64 can't be null or empty.";
                        return _responseModel;
                    }

                    //Check mime type validation
                    //string mimeType = GetFileExtension(base64);
                    //if (string.IsNullOrEmpty(mimeType))
                    //{
                    //    _responseModel.StatusCode = 0;
                    //    _responseModel.Message = "Invalid Base64 string";
                    //    return _responseModel;
                    //}

                    //GENERATE BLOB REFERENCE
                    DateTime currentTime = DateTime.Now;
                    string YearDirectoryName = currentTime.Year.ToString();
                    string MonthSubDirectoryName = currentTime.ToString("MMMM").ToLower();
                    string DateSubDirectoryName = currentTime.Date.ToString("yyyy-MM-dd").Replace("-", "");

                    string blobFileReference = string.Empty;
                    if (string.IsNullOrEmpty(entityID))
                    {
                        blobFileReference = YearDirectoryName + "/" + MonthSubDirectoryName + "/" + DateSubDirectoryName + "/" + Guid.NewGuid().ToString().Replace("-", "_") + "_dt_" + DateTime.Now.ToString("ddMMyyyy_hhmm_ss_fff") + "." + mimeType;
                    }
                    else
                    {
                        blobFileReference = YearDirectoryName + "/" + MonthSubDirectoryName + "/" + DateSubDirectoryName + "/" + entityID + "_" + Guid.NewGuid().ToString().Replace("-", "_") + "_dt_" + DateTime.Now.ToString("ddMMyyyy_hhmm_ss_fff") + "." + mimeType;
                    }

                    if (string.IsNullOrEmpty(blobFileReference))
                    {
                        _responseModel.StatusCode = 0;
                        _responseModel.Message = "File not uploaded.";
                        return _responseModel;
                    }

                    BlobContainerClient container = new BlobContainerClient(accessKey, containerName);

                    //Create container if not existing
                    await container.CreateIfNotExistsAsync();

                    BlobClient blob = container.GetBlobClient(blobFileReference);

                    //Convert base64 to byte arrary
                    var buffer = Convert.FromBase64String(base64);

                    //use byte array as stream and upload
                    using (var stream = new MemoryStream(buffer, writable: false))
                    {
                        await blob.UploadAsync(stream);
                    }

                    _responseModel.StatusCode = 1;
                    _responseModel.ResultData = blobFileReference;
                    _responseModel.Message = "File uploaded successfully.";
                }
                else
                {
                    _responseModel.StatusCode = 2;
                    _responseModel.ResultData = null;
                    _responseModel.Message = "Only .jpg ,.png and pdf format is allowed";
                }
            }
            catch (Exception ex)
            {
                _responseModel.StatusCode = -1;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;
        }

        public async Task<ServiceResult> UploadProspectusBlobUsingFileReferenceType(string accessKey, string containerName, string base64, string entityID = "")
        {
            ServiceResult _responseModel = new ServiceResult();

            try
            {
                string mimeType = GetFileExtension(base64);
                byte[] bytes = System.Convert.FromBase64String(base64);
                if (bytes.Length > 20480000) // 20MB
                {
                    _responseModel.StatusCode = 3;
                    _responseModel.ResultData = null;
                    _responseModel.Message = "Please upload document upto 15 MB.";
                    return _responseModel;
                }
                if (mimeType.ToLower() == "jpg" || mimeType.ToLower() == "png" || mimeType.ToLower() == "pdf")
                {
                    //Check if file content is null
                    if (string.IsNullOrEmpty(base64))
                    {
                        _responseModel.StatusCode = 0;
                        _responseModel.Message = "Base64 can't be null or empty.";
                        return _responseModel;
                    }

                    //Check mime type validation
                    //string mimeType = GetFileExtension(base64);
                    //if (string.IsNullOrEmpty(mimeType))
                    //{
                    //    _responseModel.StatusCode = 0;
                    //    _responseModel.Message = "Invalid Base64 string";
                    //    return _responseModel;
                    //}

                    //GENERATE BLOB REFERENCE
                    DateTime currentTime = DateTime.Now;
                    string YearDirectoryName = currentTime.Year.ToString();
                    string MonthSubDirectoryName = currentTime.ToString("MMMM").ToLower();
                    string DateSubDirectoryName = currentTime.Date.ToString("yyyy-MM-dd").Replace("-", "");

                    string blobFileReference = string.Empty;
                    if (string.IsNullOrEmpty(entityID))
                    {
                        blobFileReference = YearDirectoryName + "/" + MonthSubDirectoryName + "/" + DateSubDirectoryName + "/" + Guid.NewGuid().ToString().Replace("-", "_") + "_dt_" + DateTime.Now.ToString("ddMMyyyy_hhmm_ss_fff") + "." + mimeType; ;
                    }
                    else
                    {
                        blobFileReference =  "CollegeCheque/" + entityID + "_" + Guid.NewGuid().ToString().Replace("-", "_") + "_dt_" + DateTime.Now.ToString("ddMMyyyy_hhmm_ss_fff") + "." + mimeType; ;
                    }

                    if (string.IsNullOrEmpty(blobFileReference))
                    {
                        _responseModel.StatusCode = 0;
                        _responseModel.Message = "File not uploaded.";
                        return _responseModel;
                    }

                    BlobContainerClient container = new BlobContainerClient(accessKey, containerName);

                    //Create container if not existing
                    await container.CreateIfNotExistsAsync();

                    BlobClient blob = container.GetBlobClient(blobFileReference);

                    //Convert base64 to byte arrary
                    var buffer = Convert.FromBase64String(base64);

                    //use byte array as stream and upload
                    using (var stream = new MemoryStream(buffer, writable: false))
                    {
                        await blob.UploadAsync(stream);
                    }

                    _responseModel.StatusCode = 1;
                    _responseModel.ResultData = blobFileReference;
                    _responseModel.Message = "File uploaded successfully.";
                }
                else
                {
                    _responseModel.StatusCode = 2;
                    _responseModel.ResultData = null;
                    _responseModel.Message = "Only .jpg ,.png and pdf format is allowed";
                }
            }
            catch (Exception ex)
            {
                _responseModel.StatusCode = -1;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;
        }
        /// <summary>
        /// FetchBlobUsingFileReferenceType
        /// </summary>
        /// <param name="accessKey">Azure storage connection string</param>
        /// <param name="containerName">Name of Azure storage container</param>
        /// <param name="blobFileReference">blobFileReference on Azure storage</param>
        /// <returns></returns>
        public async Task<ServiceResult> FetchBlobUsingFileReferenceType(string accessKey, string containerName, string blobFileReference)
        {
            ServiceResult _responseModel = new ServiceResult();

            try
            {
                if (string.IsNullOrEmpty(blobFileReference))
                {
                    _responseModel.StatusCode = 0;
                    _responseModel.Message = "blobFileReference can't be null or empty.";
                    return _responseModel;
                }

                BlobContainerClient container = new BlobContainerClient(accessKey, containerName);
                BlobClient blob = container.GetBlobClient(blobFileReference);
                BlobSasBuilder blobSasBuilder = new(BlobSasPermissions.Read, DateTime.Now.AddMinutes(30));

                if (await blob.ExistsAsync())
                {
                    MemoryStream stream = new MemoryStream();
                    await blob.DownloadToAsync(stream);
                    string base64 = Convert.ToBase64String(stream.ToArray());
                    string mimeType = GetFileExtension(base64);

                    //var hh = blob.GenerateSasUri(blobSasBuilder);
                    // Dictionary<string, string> resData = new Dictionary<string, string>();
                    //resData.Add("base64", base64);

                    _responseModel.StatusCode = 1;
                    _responseModel.ResultData = base64;
                }
                else
                {
                    _responseModel.StatusCode = 0;
                    _responseModel.Message = "File doesn't exist on blob.";
                }

            }
            catch (Exception ex)
            {
                _responseModel.StatusCode = -1;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;
        }

        /// <summary>
        /// DeleteBlobUsingFileReferenceType - delete single file
        /// </summary>
        /// <param name="accessKey">Azure storage connection string</param>
        /// <param name="containerName">Name of Azure storage container</param>
        /// <param name="blobFileReference">blobFileReference on Azure storage</param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteBlobUsingFileReferenceType(string accessKey, string containerName, string blobFileReference)
        {
            ServiceResult _responseModel = new ServiceResult();

            try
            {
                if (string.IsNullOrEmpty(blobFileReference))
                {
                    _responseModel.StatusCode = 0;
                    _responseModel.Message = "BlobFileReference can't be null or empty.";
                    return _responseModel;
                }

                BlobContainerClient container = new BlobContainerClient(accessKey, containerName);
                BlobClient blob = container.GetBlobClient(blobFileReference);

                if (await blob.DeleteIfExistsAsync())
                {
                    _responseModel.StatusCode = 1;
                    _responseModel.Message = "File deleted successfully";
                }
                else
                {
                    _responseModel.StatusCode = 0;
                    _responseModel.Message = "File doesn't exist on blob.";
                }
            }
            catch (Exception ex)
            {
                _responseModel.StatusCode = -1;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;
        }

        /// <summary>
        /// DeleteBlobUsingFileReferenceType - delete multiple files
        /// </summary>
        /// <param name="accessKey">Azure storage connection string</param>
        /// <param name="containerName">Name of Azure storage container</param>
        /// <param name="blobFileReference">List<string> of blobFileReference on Azure storage</param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteBlobUsingFileReferenceType(string accessKey, string containerName, List<string> blobFileReference)
        {
            ServiceResult _responseModel = new ServiceResult();
            try
            {
                if (blobFileReference.Count < 1)
                {
                    _responseModel.StatusCode = 0;
                    _responseModel.Message = "fileReferenceType or blobFileReference can't be null or empty.";
                    return _responseModel;
                }

                BlobContainerClient container = new BlobContainerClient(accessKey, containerName);

                List<string> blobFileReferenceDeleted = new List<string>();
                List<string> blobFileReferenceNotExists = new List<string>();

                foreach (string reference in blobFileReference)
                {
                    BlobClient blob = container.GetBlobClient(reference);

                    if (await blob.DeleteIfExistsAsync())
                    {
                        blobFileReferenceDeleted.Add(reference);
                    }
                    else
                    {
                        blobFileReferenceNotExists.Add(reference);
                    }
                }

                Dictionary<string, List<string>> blobResponseData = new Dictionary<string, List<string>>();
                blobResponseData.Add("ReferencesDeleted", blobFileReferenceDeleted);
                blobResponseData.Add("ReferencesNotExists", blobFileReferenceNotExists);

                _responseModel.StatusCode = 1;
                _responseModel.ResultData = blobResponseData;
                _responseModel.Message = "Blob reference processed successfully";

            }
            catch (Exception ex)
            {
                _responseModel.StatusCode = -1;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;
        }

        #region FILE EXTENSIONS
        private string GetFileExtension(string base64)
        {
            var data = base64.Substring(0, 5);
            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region GET SIGNED URL
        public async Task<ServiceResult> FetchSignedUrlUsingFileReferenceType(string accessKey, string containerName, string blobFileReference)
        {
            ServiceResult _responseModel = new ServiceResult();

            try
            {
                if (string.IsNullOrEmpty(blobFileReference))
                {
                    _responseModel.StatusCode = 0;
                    _responseModel.Message = "blobFileReference can't be null or empty.";
                    return _responseModel;
                }

                BlobContainerClient container = new BlobContainerClient(accessKey, containerName);
                BlobClient blob = container.GetBlobClient(blobFileReference);

                if (await blob.ExistsAsync() && container.CanGenerateSasUri)
                {
                    BlobSasBuilder sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = container.Name,
                        Resource = "c"
                    };

                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(60);
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

                    Uri sasUri = container.GenerateSasUri(sasBuilder);
                    string url = Convert.ToString(sasUri);
                    string urlo = url.Replace("educationdata", "educationdata/" + blobFileReference);
                    _responseModel.StatusCode = 1;
                    _responseModel.ResultData = urlo.ToString();
                }
                else
                {
                    _responseModel.StatusCode = 0;
                    _responseModel.Message = "File doesn't exist on blob.";
                }

            }
            catch (Exception ex)
            {
                _responseModel.StatusCode = -1;
                _responseModel.Message = ex.Message;
            }
            return _responseModel;
        }
        #endregion
    }
}

