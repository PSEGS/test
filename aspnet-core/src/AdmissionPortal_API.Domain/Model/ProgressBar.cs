using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class ProgressBar
    {
        public int ProgressPercentage { get; set; }
        public bool IsPersonalDetailsCompleted { get; set; }
        public bool IsBankDetailsCompleted { get; set; }
        public bool IsAddressDetailsCompleted { get; set; }
        public bool IsAcademicCompleted { get; set; }
        public bool IsWeightageCompleted { get; set; }
        public bool IsDocumentUploadedCompleted { get; set; }
        public bool IsChoiceOfCourseCompleted { get; set; }
        public bool IsPreferenceLocked { get; set; }
        public bool IsDeclarationCompleted { get; set; }
        public bool IsPaymentCompleted { get; set; }
        public bool IsPreviewCompleted { get; set; }
        public Boolean IsLock { get; set; }
        public bool SeatStatus { get; set; }
        public bool isSeatCancelled { get; set; }
    }
}
