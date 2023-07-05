using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class DashboardUg
    {
        public string Universities { get; set; }
        public string Colleges { get; set; }
        public string TotalRegistrations { get; set; }
        public string TotalApplications { get; set; }
        public string ReserveCategory { get; set; }
        public string ScStBcObc { get; set; }
        public string FemaleApplicants { get; set; }
        public string TotalAdmissions { get; set; }

    }
    public class DashboardChartUg
    {
        public string RegistrationStatus { get; set; }
        public string VerificationStatus { get; set; }
        public string TopColleges { get; set; }
        public string TopCourses { get; set; }
        public string ApplicantSexRatio { get; set; }
        public string ReservationRatio { get; set; }
    }

    public class DashboardPg
    {
        public string Universities { get; set; }
        public string Colleges { get; set; }
        public string TotalRegistrations { get; set; }
        public string TotalApplications { get; set; }
        public string ReserveCategory { get; set; }
        public string ScStBcObc { get; set; }
        public string FemaleApplicants { get; set; }
        public string TotalAdmissions { get; set; }
    }
    public class DashboardChartPg
    {
        public string RegistrationStatus { get; set; }
        public string VerificationStatus { get; set; }
        public string TopColleges { get; set; }
        public string TopCourses { get; set; }
        public string ApplicantSexRatio { get; set; }
        public string ReservationRatio { get; set; }
    }
}
