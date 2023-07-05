using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.CollegeGroup
{
     public  class collegegroup
    {
        public collegegroup()
        {
            collegGroupSubjects = new List<CollegGroupSubject>();
        }
        [Required (ErrorMessage ="PLease provied College")]
        public int collegeId { get; set; }
        [Required (ErrorMessage ="Please provied Group")]
        public int groupId { get; set; }
        [Required(ErrorMessage = "Please provied Course")]
        public int CourseId { get; set; }
        [JsonIgnore]
        public int CreatedBy { get; set; }
        public List<CollegGroupSubject> collegGroupSubjects { get; set; }
    }

    public class CollegGroupSubject
    {
        [Required (ErrorMessage ="Please provied Subject")]
        public int SubjectId { get; set; }

        public Boolean? IsPractical { get; set; } = false;
        [Required(ErrorMessage = "Please provied Practical Fee")]

        public decimal PracticalFee { get; set; }
        [Required(ErrorMessage = "Please provied Subject Seat")]

        public int SubjectSeat { get; set; }
    }
    public  class GroupDetails
    {
        public GroupDetails()
        {
            collegesubjectlists = new List<collegesubjectlist>();
        }

        public Groups groups { get; set; }
        public List<collegesubjectlist> collegesubjectlists { get; set; }
    }
    public class Groups
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
    public class GroupsListCombinedSubjects
    {
        public int Sno { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public IList<groupsubjectDetails> GroupsubjectDetails { get; set; }

        public GroupsListCombinedSubjects()
        {
            GroupsubjectDetails = new List<groupsubjectDetails>();
        }
    }
    public class GroupsList
    {
        public int Sno { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public Boolean IsLock { get; set; }
    }
    public class collegesubjectlist
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
    }
    public class groupsubjectDetails
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Boolean IsPractical { get; set; }
        public decimal PracticalFee { get; set; }
        public int SubjectSeat { get; set; }
    }
    public class groupsubjectDetailsForEdit
    {

        public groupsubjectDetailsForEdit()
        {
            collegesubjectlists = new List<goruplist>();
        }

        public Groups groups { get; set; }
        public List<goruplist> collegesubjectlists { get; set; }
    }

    public class goruplist
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Boolean IsPractical  { get; set; }
        public decimal PracticalFee { get; set; }
        public int SubjectSeat { get; set; }
        public Boolean Selected { get; set; }
    }

}
