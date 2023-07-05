using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class UploadDocuments
    {
        [JsonIgnore]
        public int? StudentId { get; set; }
        public string TenthSerialNumber { get; set; }
        public string TwelvethSerialNumber { get; set; }
        public string TenthCertificate { get; set; }
        [JsonIgnore]
        public string TenthCertificateReference { get; set; }
        public string TwelvethCertificate { get; set; }
        [JsonIgnore]
        public string TwelvethCertificateReference { get; set; }
        public string Photo { get; set; }
        [JsonIgnore]
        public string PhotoReference { get; set; }
        public string Signature { get; set; }
        [JsonIgnore]
        public string SignatureReference { get; set; }
        public string NCC { get; set; }
        [JsonIgnore]
        public string NCCReference { get; set; }
        public string NSS { get; set; }
        [JsonIgnore]
        public string NSSReference { get; set; }
        public string YouthWelfare { get; set; }
        [JsonIgnore]
        public string YouthWelfareReference { get; set; }
        public string BC { get; set; }
        [JsonIgnore]
        public string BCReference { get; set; }
        public string SC { get; set; }
        [JsonIgnore]
        public string SCReference { get; set; }
        public string Income { get; set; }
        [JsonIgnore]
        public string IncomeReference { get; set; }
        public string Residence { get; set; }
        [JsonIgnore]
        public string ResidenceReference { get; set; }
        public string Migration { get; set; }
        [JsonIgnore]
        public string MigrationReference { get; set; }
        public string SchoolLeaving { get; set; }
        [JsonIgnore]
        public string SchoolLeavingReference { get; set; }
        public string ResidenceSerialNumber { get; set; }
        public string IncomeSerialNumber { get; set; }

        public string casteSerialNumber { get; set; }
        public string physicalDisability { get; set; }
        [JsonIgnore]
        public string physicalDisabilityReference { get; set; }
        public string BorderAreaCertificateSerialNumber { get; set; }
        [JsonIgnore]
        public string BorderAreaCertificateReference { get; set; }
        public string BorderAreaCertificate { get; set; }
        public string FreedomFighter { get; set; }
        [JsonIgnore]
        public string FreedomFighterReference { get; set; }
        public string Sports { get; set; }
        [JsonIgnore]
        public string SportsReference { get; set; }

        public string ExServiceMan { get; set; }
        [JsonIgnore]
        public string ExServiceManReference { get; set; }
        public string KashmiriMigrant { get; set; }
        [JsonIgnore]
        public string KashmiriMigrantReference { get; set; }
        public string Character { get; set; }
        [JsonIgnore]
        public string CharacterReference { get; set; }
        public string Rural { get; set; }
        [JsonIgnore]
        public string RuralReference { get; set; }
    }

    public class DocumentDetail
    {
        public string DocumentType { get; set; }
        public string DocumentReference { get; set; }
    }
}
