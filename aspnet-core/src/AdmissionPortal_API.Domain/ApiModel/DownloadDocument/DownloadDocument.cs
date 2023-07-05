using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.DownloadDocument
{
    public class DownloadDocument
    {

        public string RegistrationNumber { get; set; }
        public string FullName { get; set; }
        public string College_Id { get; set; }
        public string CollegeName { get; set; }
        public string CourseID { get; set; }
        public string CourseName { get; set; }
        public string PhotoBlobReference { get; set; }
        public string SignatureBlobReferenc { get; set; }
        public string Student_ID { get; set; }
    }
}
