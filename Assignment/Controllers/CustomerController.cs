using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assignment.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Assignment.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerDAL CustomerContext = new CustomerDAL();

        public ActionResult CustomerHomePage()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("LogIn", "Home");
            }

            string loginid = HttpContext.Session.GetString("LoginID");
            string custname = CustomerContext.GetCustomerName(loginid);
            HttpContext.Session.SetString("CustName", custname);
            return View();
        }

        public ActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("Role") != "Customer")
            {
                return RedirectToAction("LogIn", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Customer customer)
        {
            customer.Password = HttpContext.Session.GetString("Password");
            if (customer.OldPassword == customer.Password && customer.NewPassword == customer.ConfirmPassword)
            {
                string loginid = HttpContext.Session.GetString("LoginID");
                customer.EmailAddr = loginid;
                customer.Password = customer.NewPassword;
                customer.OldPassword = null;
                CustomerContext.UpdateCustomerPassword(customer);
                TempData["Message"] = "Your password has been changed successfully!";
                customer.NewPassword = null;
                customer.ConfirmPassword = null;
                return RedirectToAction("ChangePassword");
            }
            else
            {
                TempData["Message"] = "You have entered invalid credentials. Please try again!";
                return View(customer);
            }
        }


        public IActionResult BookTicket()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("LogIn", "Home");
            }

            ViewData["CountryList"] = GetCountries();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookTicket(FlightRoute route)
        {
            if (ModelState.IsValid)
            {
                List<CustomerFS> FlightSchedules = CustomerContext.SearchFlightSchedule(route.DepartureCountry,route.ArrivalCountry);
                HttpContext.Session.SetString("departure", route.DepartureCountry);
                HttpContext.Session.SetString("arrival", route.ArrivalCountry);
                if (FlightSchedules.Count > 0)
                {
                    return RedirectToAction("AvailableSchedules", "Customer");
                }
                else
                {
                    TempData["Message"] = "No available flight schedules found!";
                    return RedirectToAction("BookTicket", "Customer"); ;
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult AvailableSchedules()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("LogIn", "Home");
            }

            string departure = HttpContext.Session.GetString("departure");
            string arrival = HttpContext.Session.GetString("arrival");
            List<CustomerFS> FlightSchedules = CustomerContext.SearchFlightSchedule(departure,arrival);
            return View(FlightSchedules);
        }

        public IActionResult CreateBooking(int id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("LogIn", "Home");
            }

            Booking booking = new Booking();
            booking.ScheduleID = id;
            ViewData["CountryList"] = GetCountries();
            ViewData["SeatClassList"] = GetSeatClass();
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBooking(Booking booking)
        {
            string loginid = HttpContext.Session.GetString("LoginID");
            int customerid = CustomerContext.FindCustomerID(loginid);
            booking.CustomerID = customerid;
            booking.DateTimeCreated = DateTime.Now;
            if (booking.SeatClass == "Economy")
            {
                booking.AmtPayable = CustomerContext.FindEconomyPrice(booking.ScheduleID);
            }
            else
            {
                booking.AmtPayable = CustomerContext.FindBusinessPrice(booking.ScheduleID);
            }

            if (ModelState.IsValid)
            {
                CustomerContext.BookATicket(booking);
                TempData["Message"] = "Successfully booked a ticket";
            }
            else
            {
                TempData["Message"] = "Error";
            }
            return RedirectToAction("CreateBooking", "Customer");
        }

        public IActionResult ViewBookedTicket()
        {
            string loginid = HttpContext.Session.GetString("LoginID");
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("LogIn", "Home");
            }

            List<Booking> BookingList = CustomerContext.GetSpecificBooking(loginid);
            return View(BookingList);
        }

        public IActionResult ViewBooking(int? id)
        {
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("ViewBookedTicket");
            }
            Overall overall = CustomerContext.GetOverallDetails(id.Value);
            if (overall == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("ViewBookedTicket");
            }
            return View(overall);
        }

        public IActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Index", "Home");
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
                Value = "United Kingdom",
                Text = "United Kingdom"
            });
            countries.Add(new SelectListItem
            {
                Value = "Australia",
                Text = "Australia"
            });
            return countries;
        }

        private List<SelectListItem> GetSeatClass()
        {
            List<SelectListItem> SeatClass = new List<SelectListItem>();
            SeatClass.Add(new SelectListItem
            {
                Value = "Economy",
                Text = "Economy"
            });
            SeatClass.Add(new SelectListItem
            {
                Value = "Business",
                Text = "Business"
            });
            
            return SeatClass;
        }
    }
}