using System.ComponentModel.DataAnnotations;

namespace AdmissionPortal_API.Domain.ApiModel.Role
{
    public class AddRole
    {
        [Required(ErrorMessage = "Role title is required")]
        public string RoleTitle { get; set; }
        [Required(ErrorMessage = "StakeHolder is required")]
        public long StakeHolderId { get; set; }
    }
}

