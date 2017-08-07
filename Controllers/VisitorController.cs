using System;
using System.Collections.Generic;
using System.Linq;
using eReceptionist.Data;
using eReceptionist.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace eReceptionist.Controllers
{
    public class VisitorController : Controller
    {
        DbContext data;
        public VisitorController(DbContext _data)
        {
            data = _data;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Companys()
        {
            return View();
        }
        public IActionResult GetCompanys(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(data.VisitorCompany.FindAll());
            else
                return Json(data.VisitorCompany.Find(c => c.name.Contains(id)));
        }
        [HttpGet]
        public IActionResult AddVisitorCompany()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddVisitorCompany(VisitorCompany company)
        {
            company._id = ObjectId.NewObjectId();
            if (data.VisitorCompany.FindOne(Query.EQ("name", company.name)) == null)
                data.VisitorCompany.Insert(company);
            var companyId = data.VisitorCompany.FindOne(Query.EQ("name", company.name));
            return RedirectToAction("AddVisitor", new { id = companyId._id });
        }
        [HttpGet]
        public IActionResult AddVisitor(string id)
        {
            return View(new VisitorPerson() { visitorCompanyId = new ObjectId(id).ToString() });
        }
        [HttpPost]
        public IActionResult AddVisitor(VisitorPerson id)
        {
            var company = data.VisitorCompany.Find(c => c._id == new ObjectId(id.visitorCompanyId)).FirstOrDefault();
            if (company != null)
            {
                var newItem = new People();
                newItem._id = ObjectId.NewObjectId();
                newItem.name = id.name;
                newItem.EmailAddress = id.EmailAddress;
                newItem.Telephone = id.Telephone;
                company.visitors.Add(newItem);
                data.VisitorCompany.Update(company);
            }
            return RedirectToAction("AddVisitor", new { id = id.visitorCompanyId });
        }
        public IActionResult Contacts(string id)
        {
            var company = data.VisitorCompany.Find(c => c._id == new ObjectId(id)).FirstOrDefault();
            //var result = company.visitors;
            return View(company);
        }
        public IActionResult VisitorSearch(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(null);
            var result = data.VisitorCompany.FindAll().SelectMany(c => c.visitors.Where(x => x.name.ToLower().Contains(id.ToLower())));
            return Json(result);
        }
        public IActionResult Login(string id)
        {
            var timeThisMorning = DateTime.Now.Date;
            var timeEndToday = DateTime.Now.Date.AddDays(1);

            var contact = data.VisitorCompany.FindAll().SelectMany(c => c.visitors.Where(x => x.ids == id)).FirstOrDefault();
            var logCheck = data.VisitorLog.Find(c => c.visitorId == contact._id && c.now > timeThisMorning).OrderByDescending(c => c.now).FirstOrDefault();
            if (logCheck != null)
            {
                Console.WriteLine(logCheck.direction);
                if (logCheck.direction == "In")
                {
                    //sign out
                    LogVisitor(contact, "Out");
                }
                else
                {
                    //sign in
                    LogVisitor(contact, "In");
                }
            }
            else
            {
                //sign in
                LogVisitor(contact, "In");
            }
            var visitorCompanyData = data.VisitorCompany.FindAll().Where(c => c.visitors.Where(x => x.ids == id).Any()).FirstOrDefault();
            Console.WriteLine(visitorCompanyData.name);            
            var contactMeetings = data.meetings.Find(c => c.customerId == visitorCompanyData.ids && c._startDate > timeThisMorning && c._startDate < timeEndToday).OrderBy(c => c._startDate);
            Console.WriteLine(contactMeetings.Count());
            if(logCheck != null && logCheck.direction == "In")
                return View("Logout");
            return View(contactMeetings);
        }
        private void LogVisitor(People contact, string _direction)
        {
            var dataNow = DateTime.Now;
            data.VisitorLog.Insert(new visitorlog() { _id = ObjectId.NewObjectId(), direction = _direction, now = dataNow, name = contact.name, visitorId = contact._id });
        }
        public IActionResult VisitorLog()
        {
            return View();
        }
        public IActionResult GetVisitorLog(string id = "")
        {
            //Console.WriteLine(string.Format("GetVisitorLog: {0}", id));
            var timeThisMorning = DateTime.Now.Date;
            //Console.WriteLine(string.Format("This Morning: {0}", timeThisMorning));
            var result = new List<visitorlog>();
            switch (id.ToLower())
            {
                case "today":
                    //Most likely used more than others
                    result.AddRange(data.VisitorLog.Find(c => c.now > timeThisMorning).ToList());
                    break;
                case "week":
                    var thisWeek = timeThisMorning.AddDays(-7);
                    result.AddRange(data.VisitorLog.Find(c => c.now > thisWeek).ToList());
                    break;
                default:
                    //leave result empty
                    break;
            }
            return Json(result);
        }
    }
}