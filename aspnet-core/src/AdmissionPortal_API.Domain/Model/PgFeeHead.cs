using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
   public  class PgFeeHead
    {
        public class AddPgFeeHead
        {
            [Required(ErrorMessage ="Please Provide Head Type.") ]
            public int HeadType { get; set; }
            [Required(ErrorMessage = "Please Provide Head Name.")]

            public string HeadName { get; set; }
            [JsonIgnore]
            public int CreatedBy { get; set; }
        }
        public class UpdatePgFeeHead
        {
            [Required(ErrorMessage = "Please Provide Head Id.")]
            public int Id { get; set; }
            [Required(ErrorMessage = "Please Provide Head Type.")]
            public int HeadType { get; set; }
            [Required(ErrorMessage = "Please Provide Head Name.")]

            public string HeadName { get; set; }
            [JsonIgnore]
            public int ModifiedBy { get; set; }
        }
        public class PgFeeHeadDetails
        {
            public int Id { get; set; }
            public int sno { get; set; }
            public int HeadTypeId { get; set; }
            public string HeadType { get; set; }
            public string HeadName { get; set; }
        }
        
    }
}
