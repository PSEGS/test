using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class FeatureNavigationMaster
    {
        public int Feature_Id { get; set; }
        public string Feature_Title { get; set; }
        public int Navigation_Id { get; set; }
        public string Navigation_Title { get; set; }
        public int ParentNavigationId { get; set; }
        public string NavigationUrl { get; set; }
        public int NavigationPosition { get; set; }
        public string CssClass { get; set; }

    }
    public class NavigationList
    {
        public int Navigation_Id { get; set; }
        public string Navigation_Title { get; set; }
        public int ParentNavigationId { get; set; }
        public string NavigationUrl { get; set; }
        public int NavigationPosition { get; set; }
        public string CssClass { get; set; }
    }

    public class FeatureMapping
    {
        public int Feature_Id { get; set; }
        public string Feature_Title { get; set; }
        public List<NavigationList> NavList { get; set; }

    }
}

