using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.EmployeeManagement
{
    public class SearchEmployee
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}

