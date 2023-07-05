using AdmissionPortal_API.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Role
{
    public  class RoleNavigationMapping
    {
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }
        public List<Navigations> NavList { get; set; }
    }

    public class Navigations
    {
        public int NavigationIds { get; set; }
    }
    
}

