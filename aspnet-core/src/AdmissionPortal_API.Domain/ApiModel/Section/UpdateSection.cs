using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Section
{
    public class UpdateSection
    {
        [Required]
        public int SectionId { get; set; }
        [Required]
        public int CollegeCourseMappingId { get; set; }
        [Required]
        public int SectionTypeId { get; set; }
        [JsonIgnore]
        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }

    }
}
