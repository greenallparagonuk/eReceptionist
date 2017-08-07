using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace eReceptionist.Models
{
    public class People
    {
        public ObjectId _id { get; set; }
        public string ids { get { return (_id!=null?_id.ToString():""); } }
        [DisplayName("Contact Name")]
        [Required]
        public string name { get; set; }
//        public byte[] image { get; set; }
        [DisplayName("Location of image to display for this person")]        
        public string ImageLocation { get; set; }
        [DisplayName("Description")]
        public string Desc { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
        public string LogDesc { get; set; }
        public DateTime LogTime { get; set; }
    }
    public class VisitorPerson 
    {
        public string _id { get; set; }
        public string ids { get { return (_id!=null?_id.ToString():""); } }
        [DisplayName("Contact Name")]
        [Required]
        public string name { get; set; }
//        public byte[] image { get; set; }
        [DisplayName("Location of image to display for this person")]        
        public string ImageLocation { get; set; }
        [DisplayName("Description")]
        public string Desc { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
        public string LogDesc { get; set; }
        public DateTime LogTime { get; set; }
        public string visitorCompanyId {get; set;}
    }
}