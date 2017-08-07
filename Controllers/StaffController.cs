using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using eReceptionist.Data;
using eReceptionist.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Linq;
using eReceptionist.Services;

namespace eReceptionist.Controllers
{
    public class StaffController : Controller
    {
        DbContext data;
        SendGridMessage _email;
        public StaffController(DbContext db, SendGridMessage email)
        {            
            data = db;
            _email = email;
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
        public async Task<IActionResult> Index()
        {
            var groups = await GetUsers();
            var jsonData = JsonConvert.DeserializeObject<ActiveDirectoryGroups>(groups);
            foreach(var item in jsonData.value)
            {
                var dataItem = data.StaffLog.Find(c => c.staffId == item.objectId).OrderByDescending(c => c.now).FirstOrDefault();
                if(dataItem != null)
                    item.LogDesc = dataItem.direction;
            }
            return View(jsonData.value);
        }
        public async Task<IActionResult> GetUsersJson()
        {
            var groups = await GetUsers();
            var jsonData = JsonConvert.DeserializeObject<ActiveDirectoryGroups>(groups);
            foreach (var item in jsonData.value)
            {
                var dataItem = data.StaffLog.Find(c => c.staffId == item.objectId).OrderByDescending(c => c.now).FirstOrDefault();
                if (dataItem != null)
                    item.LogDesc = dataItem.direction;
            }
            return Json(jsonData.value);
        }
        public async Task<string> GetUser(string id)
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
            var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, string.Format("https://graph.windows.net/myorganization/users/{0}?api-version=1.6", id));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        [HttpPost]
        public async Task<IActionResult> Login(string id)
        {
            var userDetails = await GetUser(id);
            var jsonData = JsonConvert.DeserializeObject<Staff>(userDetails);
            var userName = jsonData.displayName;
            //var userMailAddress = (jsonData.mail!=null?jsonData.mail:(jsonData.mailNickname.Contains("@")?jsonData.mailNickname:(jsonData.otherMails.Count()>0?jsonData.otherMails.FirstOrDefault():"andrewgreenall@hotmail.com")));
            data.StaffLog.Insert(new stafflog() { staffId = id, name = userName, direction = "Logged In", now = DateTime.Now });   
            //await _email.SendEmailAsync(userMailAddress, "Test from SendGrid", string.Format("User - {0} has logged in", userName));
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout(string id)
        {
            var userDetails = await GetUser(id);
            var jsonData = JsonConvert.DeserializeObject<Staff>(userDetails);
            var userName = jsonData.displayName;
            data.StaffLog.Insert(new stafflog() { staffId = id, name = userName, direction = "Logged Out", now = DateTime.Now });
            return RedirectToAction("Index", "Home");
        }
        public IActionResult StaffLog()
        {
            return View();
        }
        public IActionResult GetStaffLog()
        {
            var dateFilter = DateTime.Now.AddDays(-3);
            return Json(data.StaffLog.Find(c => c.now > dateFilter));
        }
    }
}