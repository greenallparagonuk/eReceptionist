using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using eReceptionist.Models;
using eReceptionist.Data;

namespace eReceptionist.Controllers
{
    public class HomeController : Controller
    {
        DbContext data;
        public HomeController(DbContext db)
        {
            //var hasSettings = db.Company.FindAll().Any();
            data = db;
        }
        public IActionResult Index()
        {
            //var claims = ((ClaimsIdentity)User.Identity).Claims;
            //return View(claims);
            //return View();
            //HttpContext.Session["companyName"] = "";
            
            var companyDetails = data.Company;
            var companyfind = companyDetails.FindAll();
            CompanyDetails companyfindfirst = null;
            if (companyfind.Count() > 0)
                companyfindfirst = companyfind.First();
            else
                companyfindfirst = new CompanyDetails() { name = "", tel = "" };
            
            return View(companyfindfirst);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        public async Task<string> GetUsers()
        {
            // Get OAuth token using client credentials 
            //Properties->Directory ID
            string tenantName = "498e051b-3bb0-4196-ad24-74adb047aa20";
            string authString = "https://login.microsoftonline.com/" + tenantName;
            AuthenticationContext authenticationContext = new AuthenticationContext(authString, false);
            // Config for OAuth client credentials  
            //App Registrations->Application ID
            string clientId = "e9e12352-ef0d-47fe-a5a5-9033e7280b0d";
            //App Registrations->Application->Keys->Value
            string key = "PqYB2P4XagwU7p/h5vQ/S7VgsIYyt31S4L6leQbMj7A=";
            ClientCredential clientCred = new ClientCredential(clientId, key);
            string resource = "https://graph.windows.net";
            AuthenticationResult authenticationResult;
            try
            {
                authenticationResult = await authenticationContext.AcquireTokenAsync(resource, clientCred);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

            var client = new HttpClient();
            //var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, "https://graph.windows.net/myorganization/groups?api-version=1.6");
            //group objectId = 5af15d48-ed77-4d5f-8a5d-a627e867b30e
            var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, "https://graph.windows.net/myorganization/groups/5af15d48-ed77-4d5f-8a5d-a627e867b30e/members?api-version=1.6");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        public async Task<IActionResult> Contact()
        {
            ViewData["Message"] = "Our Contacts";
            //https://graph.microsoft.com/v1.0/groups/5af15d48-ed77-4d5f-8a5d-a627e867b30e/members

            var groups = await GetUsers();
            var jsonData = JsonConvert.DeserializeObject<ActiveDirectoryGroups>(groups);
            return View(jsonData.value);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
