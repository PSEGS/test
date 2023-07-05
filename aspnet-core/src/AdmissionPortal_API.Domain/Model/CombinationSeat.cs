using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
   public  class CombinationSeat
    {
        [JsonIgnore]
        public int CollegeId { get; set; }
        [Required (ErrorMessage ="Please provied Course Name")]
        public int CourseID { get; set; }
        [JsonIgnore]
        public int createdBy { get; set; }
        public List<CombinationListforSeat> combinationListforSeats { get; set; }
    }
    public class CombinationListforSeat
    {
        public int CombinationId { get; set; }
        public int Seat { get; set; }
    }
    public class CombinationSeatDetails
    {
        public int CombinationId { get; set; }
        public string Combination { get; set; }
        public int Seat { get; set; }
        public decimal Fees { get; set; }
    }
}
