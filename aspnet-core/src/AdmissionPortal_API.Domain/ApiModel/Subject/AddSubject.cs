using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Subject
{
    public class AddSubject
    {
        [Required(ErrorMessage = "Subject Name is required")]
        public string SubjectName { get; set; }
    }
}
