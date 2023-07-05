using AdmissionPortal_API.Domain.ApiModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.StudentPG
{
    public class StudentMasterPG
    {
        public int Student_ID { get; set; }
        public string FullName { get; set; }
        public string Father_Name { get; set; }
        public string FatherOccupation { get; set; }
        public string Mother_Name { get; set; }
        public string MotherOccupation { get; set; }
        public string GaurdianName { get; set; }
        public string GaurdianContactNumber { get; set; }
        public string GaurdianEmailId { get; set; }
        public string Email { get; set; }
        public string RegistrationNumber { get; set; }
        public string Mobile { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public string DateOfBirth { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public int Religion { get; set; }
        public int Maritalstatus { get; set; }
        public int? BloodGroup { get; set; }
        public int? HouseholdIncome { get; set; }
        public string GapYear { get; set; }
        public bool? HostelRequired { get; set; }
        public string ModeOfTransport { get; set; }
        public bool? ParkingRequired { get; set; }
        public int Permanent_Region { get; set; }
        public string Permanent_Address { get; set; }
        public int Permanent_Address_State_Id { get; set; }
        public int Permanent_Address_District_Id { get; set; }
        public int Permanent_Address_Tehsil_Id { get; set; }
        public int Permanent_Address_Block_Id { get; set; }
        public int Permanent_Address_Village_Id { get; set; }
        public string PermanentStateName { get; set; }
        public string PermanentDistrictName { get; set; }
        public string Permanent_Region_Name { get; set; }
        public string Permanent_PinCode { get; set; }
        public int Communication_Region { get; set; }
        public string Communication_Address { get; set; }
        public int Communication_Address_State_Id { get; set; }
        public int Communication_Address_District_Id { get; set; }
        public int Communication_Address_Tehsil_Id { get; set; }
        public int Communication_Address_Block_Id { get; set; }
        public int Communication_Address_Village_Id { get; set; }
        public string Communication_Pincode { get; set; }
        public string CommunicationStateName { get; set; }
        public string CommunicationDistrictName { get; set; }
        public string Communication_Region_Name { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankIFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string Bank { get; set; }
        public string BankMICRCode { get; set; }
        public string PhotoBlobReference { get; set; }
        public string SignatureBlobReference { get; set; }
        public bool? NCC { get; set; } = null;
        public bool? NSS { get; set; } = null;
        public string AadhaarID { get; set; }
        public string LandlineNo { get; set; }
        public bool? PunjabiInMetric { get; set; }
        public bool? AreuNRI { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public string Passport_Expiry_Date { get; set; }
        public bool? OnlyGirlChild { get; set; }
        public bool? AreYouCancerAIDSThalassemiaPatient { get; set; }
        public string NCCOption { get; set; }
        public string NSSOption { get; set; }
        public bool? AdvanceYouthLeadershipTrainingCamp { get; set; }
        public bool? YouthLeadershipTrainingCamp { get; set; }
        public bool? AdvancedMountaineering { get; set; }
        public bool? HikingTraining { get; set; }
        public bool? Mountaineering { get; set; }
        public bool? ZonalYouthFestival { get; set; }
        public bool? UniversityLevelYouthFestival { get; set; }
        public bool? InterUniversityNationalYouthFestival { get; set; }
        public bool? honorsExamination { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Delete { get; set; }
        public string Created_On { get; set; }
        public string Created_By { get; set; }
        public string GenderName { get; set; }
        public string BloodGroupName { get; set; }
        public string MaritalStatusName { get; set; }
        public string ReligionName { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public bool IsLocked { get; set; }
        public int ReservationCategory { get; set; }
        public string ReservationCategoryName { get; set; }
        public int Category { get; set; }
        public string CategoryName { get; set; }
        public int Caste { get; set; }
        public string CasteName { get; set; }
        public bool AllInformationProvidedIdCorrect { get; set; }
        public bool? AntiRaggingDeclared { get; set; }
        public bool? IsKashmiriMigrant { get; set; }
        public bool? IsPunjabDomicile { get; set; }
        public bool IsSameAsPermanentAddress { get; set; }
        public string ScBc { get; set; }
        public bool IsPhysicalDisable { get; set; }
        public string HouseholdIncomeValue { get; set; }
        public string MotherOccupationValue { get; set; }
        public string FatherOccupationValue { get; set; }
        public string UniversityId { get; set; }
        public string UniversityName { get; set; }
        public string UniversityRollNo { get; set; }
        public string UniversityPassingYear { get; set; }
        public int Nationality { get; set; }
        public string NationalityName { get; set; }
        public int? PermanentCountryId { get; set; }
        public string PermanentCountryName { get; set; }
        public int? CommunicationCountryId { get; set; }
        public string CommunicationCountryName { get; set; }
        public bool? IsFromBorderArea { get; set; }
        public int? VerificationStatus { get; set; }
        public string VerifierRemarks { get; set; }
        public bool? IsAlreadyRegisteredwithUniversity { get; set; }
        public int AlreadyRegisteredUniversityId { get; set; }
        public string AlreadyRegisteredNo { get; set; }
        public string AlreadyRegisteredUniversity { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentResponseDate { get; set; }
        public string PaymentStatus { get; set; }
        public Boolean? IsVerifed { get; set; }
        public Boolean IsCourseUnlock { get; set; }
    }
}
