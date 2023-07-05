using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.CommonEnam
{
  public   class VerificationSectionEnum
    {
        public enum VerificationSectionType
        {
            PersonalDetails = 1,
            AcademicDetails = 2,
            Weightages = 3,
            ReservationDetails = 4,
            ActionHistory = 5
        }
    }
}
