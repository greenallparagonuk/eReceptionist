using System.Collections.Generic;

namespace eReceptionist.Models
{
    public class ActiveDirectoryGroups
    {
        //public string odata.metadata {get; set;}
        public List<OrgValues> value { get; set; }
    }
    public class OrgValues
    {
        public OrgValues()
        {
            LogDesc = "Logged Out";
        }
        public string objectId { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string LogDesc { get; set; }
        public string telephoneNumber { get; set; }
        public string mobile { get; set; }
    }
}