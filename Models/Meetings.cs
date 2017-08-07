using System;
using LiteDB;

namespace eReceptionist.Models
{
    public class Meetings
    {
        public ObjectId _id { get; set; }
        public string ids { get; set; }
        public string text { get; set; }
        public bool allDay { get; set; }
        public string startDate { get; set; }        
        public string endDate { get; set; }
        public string description { get; set; }    
        public string customerId { get; set; }
        public string customerName { get; set; }
        public string staffMember { get; set; }
        public string staffMemberName { get; set; }
        public DateTime _startDate { get { return DateTime.Parse(startDate); }}
    }
}