using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdmissionPortal_API.Domain.ApiModel.University
{
    public class AddUniversity
    {
        [Display(Name = "University Name")]
        [Required(ErrorMessage = "Name is required")]
        public string UniversityName { get; set; }

        [Required(ErrorMessage = "University Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "University Email is not valid")]
        public string UniversityEmail { get; set; }

        [Required(ErrorMessage = "University Number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]

        public string UniversityPhoneNumber { get; set; }

        [Required(ErrorMessage = "Nodal Officer is required")]
        public string NodalOfficer { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]

        public string Mobile { get; set; }
        public string Website { get; set; }
        public string PermanentAddress { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public long CreatedBy { get; set; }

    }
}

