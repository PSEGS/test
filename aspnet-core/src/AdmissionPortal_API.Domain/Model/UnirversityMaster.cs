using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AdmissionPortal_API.Domain.Model
{
    public class UniversityMaster
    {
        public int UniversityId { get; set; }

        [Required(ErrorMessage = "University Name is required")]
        public string UniversityName { get; set; }

        [Required(ErrorMessage = "University Number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string UniversityPhoneNumber { get; set; }

        [Required(ErrorMessage = "University Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "University Email is not valid")]
        public string UniversityEmail { get; set; }
        public string nodalOfficer { get; set; }


        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public string Website { get; set; }
        public string PermanentAddress { get; set; }
        [JsonIgnore]
        public long CreatedBy { get; set; }

        [JsonIgnore]

        public string Password { get; set; }
    }
    public class updateuniversityMaster
    {
        public int UniversityId { get; set; }

        [Required(ErrorMessage = "University Name is required")]
        public string UniversityName { get; set; }

        [Required(ErrorMessage = "University Number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string UniversityPhoneNumber { get; set; }
        [Required(ErrorMessage = "University Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string universityemail { get; set; }

        public string nodalOfficer { get; set; }
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public string Website { get; set; }
        public string PermanentAddress { get; set; }
        // public long UpdatedBy { get; set; }

        //public int UniversityType { get; set; }
        //public string nodalOfficer { get; set; }
        public long CreatedBy { get; set; }

    }

    public class GetUniversity
    {
        public int UniversityId { get; set; }

        public int sno { get; set; }
        public string UniversityName { get; set; }

        public string UniversityPhoneNumber { get; set; }

        [Required(ErrorMessage = "University Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "University Email is not valid")]
        public string UniversityEmail { get; set; }
        public string nodalOfficer { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string PermanentAddress { get; set; }
        public Int32 TotalCount { get; set; }
        public string username { get; set; }
        public string IsActive { get; set; }
    }

    public class GetAllUniversity
    {
        public int UniversityId { get; set; }

        public string UniversityName { get; set; }
    }
}

