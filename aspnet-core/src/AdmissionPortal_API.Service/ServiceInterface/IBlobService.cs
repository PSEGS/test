using AdmissionPortal_API.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Service.ServiceInterface
{

    public interface IBlobService
    {
        Task<ServiceResult> UploadBlobUsingFileReferenceType(string accessKey, string containerName, string base64, string entityID = "");
        Task<ServiceResult> UploadProspectusBlobUsingFileReferenceType(string accessKey, string containerName, string base64, string entityID = "");

        Task<ServiceResult> FetchBlobUsingFileReferenceType(string accessKey, string containerName, string blobFileReference);
        Task<ServiceResult> FetchSignedUrlUsingFileReferenceType(string accessKey, string containerName, string blobFileReference);

        Task<ServiceResult> DeleteBlobUsingFileReferenceType(string accessKey, string containerName, string blobFileReference);

        Task<ServiceResult> DeleteBlobUsingFileReferenceType(string accessKey, string containerName, List<string> blobFileReference);

    }
}

