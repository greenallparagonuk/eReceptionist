using System;
using LiteDB;

namespace eReceptionist.Models
{
    public class stafflog
    {
        public ObjectId _id { get; set; }
        public string staffId { get; set; }
        public string name { get; set; }
        public string direction { get; set; }
        public DateTime now { get; set; }
    }
}