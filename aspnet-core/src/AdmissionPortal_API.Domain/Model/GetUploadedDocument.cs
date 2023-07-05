using AdmissionPortal_API.Domain.ApiModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class GetUploadedDocument
    {
        public int StudentId { get; set; }
        public string TenthSerialNumber { get; set; }
        public string TwelvethSerialNumber { get; set; }
        public string ResidenceSerialNumber { get; set; }
        public string IncomeSerialNumber { get; set; }
        public string BorderAreaCertificateSerialNumber { get; set; }
        public string DocumentType { get; set; }
        public string DocumentReference { get; set; }
        public bool IsManualData { get; set; }
        public string casteSerialNumber { get; set; }
        public bool isResidenceSerialNumberVerified { get; set; }
        public bool isIncomeSerialNumberVerified { get; set; }
        public bool isCasteSerialNumberVerified { get; set; }
        public bool isFromBorderArea { get; set; }
        public string bcSerialNumber { get; set; }
        public Boolean isBcSerialNumberVerified { get; set; }
        public bool IsRural { get; set; }
    }

    public class UploadedDocumentDetail
    {
        public int StudentId { get; set; }
        public string TenthSerialNumber { get; set; }
        public string TwelvethSerialNumber { get; set; }
        public string ResidenceSerialNumber { get; set; }
        public string IncomeSerialNumber { get; set; }
        public string TenthCertificate { get; set; }
        public string TwelvethCertificate { get; set; }
        public string Photo { get; set; }
        public string Signature { get; set; }
        public string NCC { get; set; }
        public string NSS { get; set; }
        public string YouthWelfare { get; set; }
        public string BC { get; set; }
        public string SC { get; set; }
        public string Income { get; set; }
        public string Residence { get; set; }
        public string Migration { get; set; }
        public string SchoolLeaving { get; set; }
        public string base64Photo { get; set; }
        public string base64Signature { get; set; }
        public bool IsManualData { get; set; }
        public string casteSerialNumber { get; set; }
        public string physicalDisability { get; set; }
        public bool isResidenceSerialNumberVerified { get; set; }
        public bool isIncomeSerialNumberVerified { get; set; }
        public bool isCasteSerialNumberVerified { get; set; }
        public bool isFromBorderArea { get; set; }
        public string BorderAreaCertificateSerialNumber { get; set; }
        public string BorderAreaCertificate { get; set; }
        public string Sports { get; set; }
        public string FreedomFighter { get; set; }
        public string ExServiceMan { get; set; }
        public string KashmiriMigrant { get; set; }
        public string Character { get; set; }
        public string bcSerialNumber { get; set; }
        public Boolean isBcSerialNumberVerified { get; set; }
        public bool isRural { get; set; }
        public string Rural { get; set; }
    }
    public class GetEligibleCourseByStudentID
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
    public class ObjectionListByStudentModel
    {
        public string Objection { get; set; }
    }
}
