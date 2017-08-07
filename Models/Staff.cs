using System.Collections.Generic;

namespace eReceptionist.Models
{
    public class Staff
    {
        public string objectId {get; set;}
        public string displayName {get; set;}
        public string mail {get; set;}
        public string mailNickname {get; set;}
        public List<string> otherMails {get;set;}
    }
}