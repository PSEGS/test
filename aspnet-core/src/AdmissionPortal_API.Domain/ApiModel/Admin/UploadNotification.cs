using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Admin
{
    public class UploadNotification
    {
        public string Notification { get; set; }
        [JsonIgnore]
        public string NotificationReference { get; set; }
        
        public string Description { get; set; }

        public string UserId { get; set; }

    }

    public class NotificaitonModel
    {
        [Required]
        public string NotificationReference { get; set; }

    }
    
    public class UpdateNotificaitonModel
    {
        public int Id { get; set; }
    
        public bool Status { get; set; }
    }
}
