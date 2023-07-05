using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class NotificationMaster
    {

        public int Id { get; set; }
        public string Notification { get; set; }
       
        public string NotificationReference { get; set; }

        public string Description { get; set; }
        public string NotificationType { get; set; }

        public string UserId { get; set; }
    }
    public class NotificationsMaster
    {

        public int Id { get; set; }
        public string Notification { get; set; }
       
        public string NotificationReference { get; set; }

        public string Description { get; set; }
        public string NotificationType { get; set; }

        public string UserId { get; set; }

        public string CreatedDate { get; set; }

        public int TotalCount { get; set; }
    }
}
