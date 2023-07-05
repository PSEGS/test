using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.RoleNavigation
{
    public class UpdateRoleNavigation
    {
        [Required(ErrorMessage = "Mapping is required")]
        public int MappingId { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Navigation is required")]
        public int NavigationId { get; set; }

    }
}

