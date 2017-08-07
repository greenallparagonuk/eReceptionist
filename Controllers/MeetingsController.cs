using System;
using System.Linq;
using eReceptionist.Data;
using eReceptionist.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eReceptionist.Controllers
{
    public class MeetingsController : Controller
    {
        DbContext data;
        public MeetingsController(DbContext _data)
        {
            data = _data;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetMeetings()
        {
            var startDay = DateTime.Now.Date.AddDays(-31);
            //var endDay = DateTime.Now.Date.AddDays(31);
            //data.meetings.Find(c => DateTime.Parse(c.startDate) > startDay)
            var raw = data.meetings.FindAll().ToList();
            var result = raw.Where(c => DateTime.Parse(c.startDate) > startDay);
            return Json(result);
        }
        public IActionResult AddMeeting(string values)
        {
            var newMeeting = new Meetings();

            JsonConvert.PopulateObject(values, newMeeting);

            newMeeting.customerName = data.VisitorCompany.FindById(new ObjectId(newMeeting.customerId)).name;
            //newMeeting.staffMemberName = data.People.FindById(new ObjectId(newMeeting.staffMember)).name; 
            newMeeting.staffMemberName = newMeeting.staffMember;    

            data.meetings.Insert(newMeeting);
            newMeeting.ids = newMeeting._id.ToString();
            data.meetings.Update(newMeeting);

            return Ok();
        }
        public IActionResult updateMeeting(string values)
        {
            var newMeeting = new Meetings();

            JsonConvert.PopulateObject(values, newMeeting);
            newMeeting._id = new ObjectId(newMeeting.ids);
            newMeeting.customerName = data.VisitorCompany.FindById(new ObjectId(newMeeting.customerId)).name;
            //newMeeting.staffMemberName = data.People.FindById(new ObjectId(newMeeting.staffMember)).name;
            newMeeting.staffMemberName = newMeeting.staffMember;   
            data.meetings.Update(newMeeting);

            return Ok();
        }
    }
}