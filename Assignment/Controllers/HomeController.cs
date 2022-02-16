using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Assignment.Models;
using Microsoft.AspNetCore.Http;
using Assignment.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using Newtonsoft.Json;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CustomerDAL CustomerContext = new CustomerDAL();
        private FlightScheduleDAL LoginContext = new FlightScheduleDAL();
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Deals()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Register()
        {
            ViewData["CountryList"] = GetCountries();
            return View();
        }

       

        public ActionResult Create()
        {
            ViewData["CountryList"] = GetCountries();
            return View();
        }

        private List<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            countries.Add(new SelectListItem
            {
                Value = "Singapore",
                Text = "Singapore"
            });
            countries.Add(new SelectListItem
            {
                Value = "Malaysia",
                Text = "Malaysia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Australia",
                Text = "Australia"
            });
            countries.Add(new SelectListItem
            {
                Value = "United Kingdom",
                Text = "United Kingdom"
            });
            return countries;
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer)
        {
            //Get country list for drop-down list
            //in case of the need to return to Create.cshtml view
            customer.NewPassword = "null";
            customer.OldPassword = customer.Password;
            customer.ConfirmPassword = "null";
            if (ModelState.IsValid)
            {

                //Add customer record to database
                customer.CustomerID = CustomerContext.Add(customer);
                TempData["Message"] = "You have registered successfully. You are now able to log in and your password is p@55Cust. You may change it later when you have logged in successfully.";
                //Redirect user to Customer/Index view
                return RedirectToAction("Register","Home");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                ViewData["CountryList"] = GetCountries();
                return View(customer);
            }

        }

        public IActionResult AdminHomePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(IFormCollection formData)
        {
            // Read inputs from textboxes
            // Email address converted to lowercase
            string loginID = formData["txtLoginID"].ToString();
            string password = formData["txtPassword"].ToString();
            List<Customer> CustomerList = CustomerContext.GetAllCustomers();
            Debug.WriteLine(CustomerList.Count());
            Staff s1 = LoginContext.checkLogin(loginID, password);
            foreach (Customer c in CustomerList)
            {

                if (c.EmailAddr == loginID && c.Password == password)
                {
                    // Store Login ID in session with the key “LoginID”
                    HttpContext.Session.SetString("LoginID", loginID);
                    HttpContext.Session.SetString("Password", password);
                    // Store user role “Staff” as a string in session with the key “Role”
                    HttpContext.Session.SetString("Role", "Customer");
                    return RedirectToAction("CustomerHomePage", "Customer");
                }
                else
                {
                    continue;
                }
            }
            if (s1.Vocation == "Administrator")
            {
                // Store Login ID in session with the key “LoginID”
                HttpContext.Session.SetString("LoginID", loginID);
                // Store user role “Staff” as a string in session with the key “Role”
                HttpContext.Session.SetString("Role", "admin");
                return RedirectToAction("AdminHomePage", "Home");
            }
            // Store an error message in TempData for display at the index view
            TempData["Message"] = "Invalid Login Credentials! Please try again!";
            // Redirect user back to the index view through an action
            return RedirectToAction("LogIn");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //GET
        public async Task<ActionResult> WorldTimeAPI()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://worldtimeapi.org");
            HttpResponseMessage response = await client.GetAsync("api/timezone");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                string[] countries = JsonConvert.DeserializeObject<string[]>(data);
                List<APICountry> countryList = new List<APICountry>();
                
                foreach (string name in countries)
                {
                    string[] tempArr = name.Split(",");
                    name.Trim(new char[] { '[', ']', ',' });
                    APICountry api = new APICountry();
                    api.countries = name;
                    countryList.Add(api);
                }

                HttpClient client2 = new HttpClient();
                client2.BaseAddress = new Uri("https://worldtimeapi.org/api/timezone/");
                WorldTimeAPI worldtime = new WorldTimeAPI();
                List<WorldTimeAPI> worldtimeList = new List<WorldTimeAPI>();
                for (int i = 0; i < countryList.Count; i++)
                {
                    string country = countryList[i].countries;
                    HttpResponseMessage response2 = await client2.GetAsync(country);
                    string data2 = await response2.Content.ReadAsStringAsync();
                    worldtime = JsonConvert.DeserializeObject<WorldTimeAPI>(data2);

                    worldtimeList.Add(worldtime);
                
                }
                return View(worldtimeList);
            }

            return View();
        }


    }
}
