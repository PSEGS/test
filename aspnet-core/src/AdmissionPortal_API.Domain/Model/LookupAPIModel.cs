using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
  public   class LookupAPIModel
    {
        public class CollegeType
        {
            public string collegeTypeID { get; set; }
            public string CollegeTypeDescription { get; set; }
        }
        public class CollegeMode
        {
            public string CollegeModeID { get; set; }
            public string CollegeModeDescription { get; set; }
        }
        public class CourseTypeModel
        {
            public int CourseTypeId { get; set; }
            public string CourseType { get; set; }
        }
        public class LookUpTypeModel
        {
            public int LookupId { get; set; }
            public string LookupType { get; set; }
        }
        
        public class ReservationTypeModel
        {
            public int ID { get; set; }
            public string Description { get; set; }
        }
    }
}
