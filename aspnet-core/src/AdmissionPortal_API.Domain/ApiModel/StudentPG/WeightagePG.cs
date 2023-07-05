using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.StudentPG
{
    public class WeightagePG
    {
        [JsonIgnore]
        public int StudentId { get; set; }
        public bool NCC { get; set; }
        public string NCCOption { get; set; }
        public bool NSS { get; set; }
        public string NSSOption { get; set; }
        public bool AdvanceYouthLeadershipTrainingCamp { get; set; }
        public bool YouthLeadershipTrainingCamp { get; set; }
        public bool AdvancedMountaineering { get; set; }
        public bool HikingTraining { get; set; }
        public bool Mountaineering { get; set; }
        public bool ZonalYouthFestival { get; set; }
        public bool UniversityLevelYouthFestival { get; set; }
        public bool InterUniversityNationalYouthFestival { get; set; }
        public bool IsPunjabDomicile { get; set; }
        public bool? honorsExamination { get; set; }
        public Int32 reservationCategory { get; set; }
        public Int32 caste { get; set; }
        public Int32 casteCategory { get; set; }
        

    }

}
