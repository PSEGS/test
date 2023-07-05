using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.EmployeeRoleMapping
{
    public class AddEmployeeRoleMapping
    {
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}

