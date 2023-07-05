using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using AdmissionPortal_API.Domain.Model;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using DapperExtensions.Mapper;
using Dapper.Contrib.Extensions;

namespace AdmissionPortal_API.Domain.ApiModel.Navigation
{
    public class AddNavigation
    {       
        [Required(ErrorMessage = "Navigation Title is Required")]
        public string NavigationTitle { get; set; }
        [Required(ErrorMessage = "Navigation Url is Required")]

        public string NavigationUrl { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public string NavigationDescription { get; set; }
        public int NavigationPosition { get; set; }
        public int ParentNavigationId { get; set; }

    }

    


}

