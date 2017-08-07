using System.ComponentModel;
using eReceptionist.Services;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eReceptionist.Models
{
    public class CompanyDetails
    {
        public ObjectId _id { get; set; }
        [DisplayName("Your Company Name")]
        public string name { get; set; }
        [DisplayName("Your Company Telephone Number")]
        public string tel { get; set; }
        public string SMTPHost { get; set; }
        public string SMTPFrom { get; set; }
        public string SMTPSubject { get; set; }
        public string SMTPBody { get; set; }
    }
    public class CompanyDetails_simple
    {
        public string _id { get; set; }
        [DisplayName("Your Company Name")]
        public string name { get; set; }
        [DisplayName("Your Company Telephone Number")]
        public string tel { get; set; }
        public string SMTPHost { get; set; }
        public string SMTPFrom { get; set; }
        public string SMTPSubject { get; set; }
        public string SMTPBody { get; set; }
    }
}