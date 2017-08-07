using System;
using LiteDB;

namespace eReceptionist.Models
{
    public class visitorlog
    {
        public ObjectId _id { get; set; }
        public ObjectId visitorId { get; set; }
        public string name { get; set; }
        public string direction { get; set; }
        public DateTime now { get; set; }
    }
}