using System.ComponentModel.DataAnnotations;

namespace AdmissionPortal_API.Domain.ApiModel.Role
{
    public class UpdateRole
    {
        [Required(ErrorMessage = "Role Id is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role title is required")]
        public string RoleTitle { get; set; }

        [Required(ErrorMessage = "Stake Holder is required")]
        public long StakeHolderId { get; set; }
        [Required]
        public bool IsActive { get; set; }
       
        public string NavigationIds { get; set; }

    }
}

