using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class CollegeCourseSeat
    {
        [Required(ErrorMessage ="Please Provied College")]
        public int CollegeId { get; set; }
        [Required(ErrorMessage = "Please Provied Course")]

        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please Provied Total Number of Seats")]
        public int Seat { get; set; }
        [JsonIgnore]
        public int CreatedBy { get; set; }

        //[Required(ErrorMessage = "Please Provied Total Number of Cancer Seats")]
        public int CancerSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of AIDS Seats")]
        public int AIDSSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of Thalassemic Seats")]
        public int ThalassemicSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of One Gril Out of Two Girl Child Seats")]
        public int OneGirlSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of NRI Seats")]
        public int NRI { get; set; }
        public int KashmiriRiotVictims { get; set; }
        public int BorderArea { get; set; }
        public int RuralArea { get; set; }

    }

    public class UpdateCollegeCourseSeat
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Provied College")]

        public int CollegeId { get; set; }
        [Required(ErrorMessage = "Please Provied Course")]

        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please Provied Total Number of Seats")]

        public int Seat { get; set; }
        [JsonIgnore]

        public int ModefyBy { get; set; }

        //[Required(ErrorMessage = "Please Provied Total Number of Cancer Seats")]
        public int CancerSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of AIDS Seats")]
        public int AIDSSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of Thalassemic Seats")]
        public int ThalassemicSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of One Gril Out of Two Girl Child Seats")]
        public int OneGirlSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of NRI Seats")]
        public int NRI { get; set; }
        public int KashmiriRiotVictims { get; set; }
        public int BorderArea { get; set; }
        public int RuralArea { get; set; }
    }
    public class CollegeCourseSeatDetails
    {
        public int Id { get; set; }
        public int sno { get; set; }
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string TotalSeat { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 Fee { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of Cancer Seats")]
        public int CancerSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of AIDS Seats")]
        public int AIDSSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of Thalassemic Seats")]
        public int ThalassemicSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of One Gril Out of Two Girl Child Seats")]
        public int OneGirlSeat { get; set; }
        //[Required(ErrorMessage = "Please Provied Total Number of NRI Seats")]
        public int NRI { get; set; }
        public int KashmiriRiotVictims { get; set; }
        public int BorderArea { get; set; }
        public int RuralArea { get; set; }
        public Boolean IsLock { get; set; }
    }
    public class SeatMatrixM
    {
        public string CourseName { get; set; }
        public string TotalSeat { get; set; }
        public string SC { get; set; }
        public string BC { get; set; }
        public string General { get; set; }
        public string PWD { get; set; }
        public string SportsGen { get; set; }
        public string SportsSC { get; set; }
        public string Sports { get; set; }
        public string ExSerGen { get; set; }
        public string ExSerSC { get; set; }
        public string ExSerBc { get; set; }
        public string ExServiceman { get; set; }
        public string FreedomFighters { get; set; }
        public Boolean IsLock { get; set; }
        public int CollegeId { get; set; }

    }
    public class GetReservationQuotaModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string ReservationshortName { get; set; }
        public decimal Percentage { get; set; }
    }
}
