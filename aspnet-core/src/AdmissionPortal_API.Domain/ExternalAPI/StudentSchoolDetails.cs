using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ExternalAPI
{
    public class StudentSchoolDetails
    {
        [Required(ErrorMessage = "Please provied Rollno")]
        public string rollno
        {
            get; set;
        }
        [Required(ErrorMessage = "Please provied yeartype")]

        public string yeartype { get; set; }

    }
    public class CBSESchoolDetails
    {
        [Required(ErrorMessage = "Please provied Rollno")]
        public string rollno
        {
            get; set;
        }
        [Required(ErrorMessage = "Please provied yeartype")]

        public string yeartype { get; set; }
        [Required(ErrorMessage = "Please provied Student Name")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Please provied Board")]

        public int BoardId { get; set; }

    }
    public class josn
    {
        public Xml Xml { get; set; }
        public Certificate Certificate { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Xml
    {
        public string version { get; set; }
        public string encoding { get; set; }
    }

    public class Address
    {
        public string type { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string house { get; set; }
        public string landmark { get; set; }
        public string locality { get; set; }
        public string vtc { get; set; }
        public string district { get; set; }
        public string pin { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }

    public class Organization
    {
        public string name { get; set; }
        public string code { get; set; }
        public string tin { get; set; }
        public string uid { get; set; }
        public string type { get; set; }
        public Address Address { get; set; }
    }

    public class IssuedBy
    {
        public Organization Organization { get; set; }
    }

    public class Photo
    {
        public string format { get; set; }

       
        public string Text { get; set; }
    }

    public class Person
    {
        public string uid { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string fatherName { get; set; }
        public string swdIndicator { get; set; }
        public string motherName { get; set; }
        public string gender { get; set; }
        public string maritalStatus { get; set; }
        public string religion { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Address Address { get; set; }
        public Photo Photo { get; set; }
    }

    public class IssuedTo
    {
        public Person Person { get; set; }
    }

    public class School
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Examination
    {
        public string name { get; set; }
        public string centerCode { get; set; }
        public string admitCardId { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }

    public class Subject
    {
        public string name { get; set; }
        public string code { get; set; }
        public string marksTheory { get; set; }
        public string marksMaxTheory { get; set; }
        public string marksPractical { get; set; }
        public string marksMaxPractical { get; set; }
        public string marksTotal { get; set; }
        public string marksMax { get; set; }
        public string gp { get; set; }
        public string gpMax { get; set; }
        public string grade { get; set; }
    }

    public class Subjects
    {
        public List<Subject> Subject { get; set; }
    }

    public class Performance
    {
        public string result { get; set; }
        public string marksTotal { get; set; }
        public string marksMax { get; set; }
        public string percentage { get; set; }
        public string cgpa { get; set; }
        public string cgpaMax { get; set; }
        public string resultDate { get; set; }
        public string updateDate { get; set; }
        public Subjects Subjects { get; set; }
    }

    public class CertificateData
    {
        public School School { get; set; }
        public Examination Examination { get; set; }
        public Performance Performance { get; set; }
    }

    public class Certificate
    {
        public string language { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string number { get; set; }
        public string issuedAt { get; set; }
        public string issueDate { get; set; }
        public string validFromDate { get; set; }
        public string status { get; set; }
        public IssuedBy IssuedBy { get; set; }
        public IssuedTo IssuedTo { get; set; }
        public CertificateData CertificateData { get; set; }
    }

    public class Root
    {
        
        public Xml Xml { get; set; }
        public Certificate Certificate { get; set; }
    }
    public class studentmarksRechecking
    {
        public int studentid { get; set; }
        public int ObtainedMarks { get; set; }
        public int TotalMarks { get; set; }
        public string Rollno { get; set; }
        public string Yearofpassing { get; set; }
        public string Fullname { get; set; }
        public string BoardUniversityId { get; set; }

    }
    public class SaveRecheckData
    {
        public string studentid { get; set; }
        public string ObtainedMarks { get; set; }
        public string TotalMarks { get; set; }
        public string Rollno { get; set; }
        public string Yearofpassing { get; set; }
        public string Fullname { get; set; }
        public string BoardUniversityId { get; set; }
        public string NewObtainedMarks { get; set; }
        public string NewTotalMarks { get; set; }
    }




}
