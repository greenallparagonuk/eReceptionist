using System;
using System.Collections.Generic;
using System.Linq;
using eReceptionist.Data;
using eReceptionist.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace eReceptionist.Controllers
{
    public class ReportsController : Controller
    {
        DbContext data;
        public ReportsController(DbContext _data)
        {
            data = _data;
        }
        public IActionResult Fire()
        {
            return View();
        }
        public IActionResult VisitorsIn()
        {
            var thisMorning = DateTime.Now.Date;

            var result = data.VisitorLog.Find(c => c.now > thisMorning)
                .GroupBy(c => c.name, (id, lastlog) => new {
                    ID = id, 
                    lastlog = lastlog.OrderByDescending(c => c.now).First().now, 
                    
                    direction = lastlog.OrderByDescending(c => c.now).First().direction})
                .ToList().Where(c => c.direction == "In");

            return Json(result);
        }
        public IActionResult StaffIn()
        {
            var thisMorning = DateTime.Now.Date;

            var result = data.StaffLog.Find(c => c.now > thisMorning)
                .GroupBy(c => c.name, (id, lastlog) => new {
                    ID = id, 
                    lastlog = lastlog.OrderByDescending(c => c.now).First().now, 
                    
                    direction = lastlog.OrderByDescending(c => c.now).First().direction})
                .ToList().Where(c => c.direction == "Logged In");

            return Json(result);
        }
    }
}