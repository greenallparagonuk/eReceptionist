using System.Linq;
using eReceptionist.Data;
using eReceptionist.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eReceptionist.Controllers
{
    public class CompanyDetailsController : Controller
    {
        DbContext _db;
        public CompanyDetailsController(DbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var companyDetails = _db.Company;
            var companyfind = companyDetails.FindAll();
            CompanyDetails companyfindfirst = null;
            if (companyfind.Count() > 0)
                companyfindfirst = companyfind.First();
            else
                companyfindfirst = new CompanyDetails() { name = "", tel = "", _id = ObjectId.NewObjectId() };

            return View(companyfindfirst);
        }
        public IActionResult Update(CompanyDetails_simple details)
        {
            var company = new CompanyDetails()
            {
                _id = new ObjectId(details._id),
                name = details.name,
                SMTPBody = details.SMTPBody,
                SMTPFrom = details.SMTPFrom,
                SMTPHost = details.SMTPHost,
                SMTPSubject = details.SMTPSubject,
                tel = details.tel
            };
            var data = _db;
            if (data.Company.FindAll().Count() > 0)
                data.Company.Update(company);
            else
                data.Company.Insert(company);
            return RedirectToAction("Index", "Home");
        }
    }
}