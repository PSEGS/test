using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace AdmissionPortal_API.Domain.ApiModel.StudentPG
{
    public class UploadDocumentsPG
    {
        [JsonIgnore]
        public int StudentId { get; set; }
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
        //public string physicalDisability { get; set; }
        //[JsonIgnore]
        //public string physicalDisabilityReference { get; set; }
        public string graduation { get; set; }
        [JsonIgnore]
        public string graduationReference { get; set; }

        public string freedomFighter { get; set; }
        [JsonIgnore]
        public string freedomFighterReference { get; set; }
        public string PWD { get; set; }
        [JsonIgnore]
        public string PWDReference { get; set; }
        public string ESM { get; set; }
        [JsonIgnore]
        public string ESMReference { get; set; }
        public string Sports { get; set; }
        [JsonIgnore]
        public string SportsReference { get; set; }
        public string kashmiriMigrant { get; set; }
        [JsonIgnore]
        public string kashmiriMigrantReference { get; set; }
        public string BorderAreaCertificateSerialNumber { get; set; }
        [JsonIgnore]
        public string BorderAreaCertificateReference { get; set; }
        public string BorderAreaCertificate { get; set; }

        [JsonIgnore]
        public string PatientProofReference { get; set; }
        public string PatientProof { get; set; }

        [JsonIgnore]
        public string NriPassportReference { get; set; }
        public string NriPassport { get; set; }
        [JsonIgnore]
        public string AdvanceYouthLeadershiptrainingcampReference { get; set; }
        public string AdvanceYouthLeadershipTrainingCamp { get; set; }

        [JsonIgnore]
        public string YouthLeadershipTrainingCampReference { get; set; }
        public string YouthLeadershipTrainingCamp { get; set; }
        [JsonIgnore]
        public string AdvancedMountaineeringReference { get; set; }
        public string AdvancedMountaineering { get; set; }
        [JsonIgnore]
        public string HikingTrainingReference { get; set; }
        public string HikingTraining { get; set; }
        [JsonIgnore]
        public string MountaineeringReference { get; set; }
        public string Mountaineering { get; set; }
        [JsonIgnore]
        public string ZonalYouthFestivalReference { get; set; }
        public string ZonalYouthFestival { get; set; }
        [JsonIgnore]
        public string UniversityLevelYouthFestivalReference { get; set; }
        public string UniversityLevelYouthFestival { get; set; }
        [JsonIgnore]
        public string InterUniversityNationalYouthFestivalReference { get; set; }
        public string InterUniversityNationalYouthFestival { get; set; }

        [JsonIgnore]
        public string CharacterReference { get; set; }
        public string Character { get; set; }

        public string bcSerialNumber { get; set; }
        public string Rural { get; set; }
        [JsonIgnore]
        public string RuralReference { get; set; }
    }

    public class DocumentDetailPG
    {
        public string DocumentType { get; set; }
        public string DocumentReference { get; set; }
    }
}
