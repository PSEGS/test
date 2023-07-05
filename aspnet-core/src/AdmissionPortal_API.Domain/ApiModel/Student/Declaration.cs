using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class Declaration
    {
        [JsonIgnore]
        public int StudentId { get; set; }
        public bool IsLocked { get; set; }
        public bool AllInformationProvidedIsCorrect { get; set; }
        public bool AntiRaggingDeclared { get; set; }
    }
}
