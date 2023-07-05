using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Profile
{
    public class ProfileImage
    {
        [JsonIgnore]
        public string ImageFile { get; set; }
        [Required]
        public string ImageCode { get; set; }
        [JsonIgnore]
        public string ImageReference { get; set; }
    }
}

