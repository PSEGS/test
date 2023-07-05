using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class Weightage
    {
        [JsonIgnore]
        public int? StudentId { get; set; }
        public bool? NCC { get; set; } = null;
        public string NCCOption { get; set; } = null;
        public bool? NSS { get; set; } = null;
        public string NSSOption { get; set; } = null;
        public bool? AdvanceYouthLeadershipTrainingCamp { get; set; }
        public bool? YouthLeadershipTrainingCamp { get; set; }
        public bool? AdvancedMountaineering { get; set; }
        public bool? HikingTraining { get; set; }
        public bool? Mountaineering { get; set; }
        public bool? ZonalYouthFestival { get; set; }
        public bool? UniversityLevelYouthFestival { get; set; }
        public bool? InterUniversityNationalYouthFestival { get; set; }
        public bool? punjabResident { get; set; }
        public int reservationCategory { get; set; }
        public int casteCategory { get; set; }
        public int caste { get; set; }

    }

}
