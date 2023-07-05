using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.RoleNavigation
{
    public class AddRoleNavigation
    {
        [Required(ErrorMessage = "Role Id is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Navigation is required")]
        public int NavigationId { get; set; }
        [JsonIgnore]
        public int CreatedBy { get; set; }

    }
}

