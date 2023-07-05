using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.OfflineAdmisson
{
  public  class OfflineAdmissionModel
    {
        public string StudentType { get; set; }
        public string ApplicantName { get; set; }
        public int BoardId { get; set; }
        public string RollNo { get; set; }
        public string Passingyear { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Fathername { get; set; }
        public string Mothername { get; set; }
        public string aadhar { get; set; }
        public string Dob { get; set; }
        public bool IsDomicile { get; set; }
        public string Gender { get; set; }
        public int ReservationCategoryID { get; set; }
        public int CasteCategoryId { get; set; }
        [JsonIgnore]
        public int CollegeId { get; set; }
        public int CourseId { get; set; }
        public int? CombinationId { get; set; }
        
        public string examination { get; set; }
        [JsonIgnore]
        public string RegistrationNumber { get; set; }
    }
}
