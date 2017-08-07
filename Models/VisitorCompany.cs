using System.Collections.Generic;
using System.ComponentModel;
using LiteDB;

namespace eReceptionist.Models
{
    public class VisitorCompany
    {
        public VisitorCompany()
        {
            visitors = new List<People>();
        }
        public ObjectId _id { get; set; }
        public string ids { get { return (_id!=null?_id.ToString():""); } }
        [DisplayName("Name")]
        public string name { get; set; }
        public List<People> visitors { get; set; }
    }
}