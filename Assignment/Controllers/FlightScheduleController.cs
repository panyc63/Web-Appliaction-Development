using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assignment.Models;
using Assignment.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class FlightScheduleController : Controller
    {
        private FlightScheduleDAL flightContext = new FlightScheduleDAL();
        private AircraftDAL aircraftContext = new AircraftDAL();

        // GET: FlightScheduleController
        public ActionResult Index()
        {
            return View();
        }
        //GET
        [HttpGet]
        public ActionResult UpdateStatus(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            FlightSchedule flightSchedule = flightContext.getFlightDetails(id.Value);
            if ( flightSchedule == null)
            {return RedirectToAction("Index", "Home");}
            ViewData["statusList"] = getStatus();
            return View(flightSchedule);
        }

        //POST
        [HttpPost]
        public ActionResult UpdateStatus(FlightSchedule flightSchedule)
        {

            if (ModelState.IsValid)
            {
                //Update staff record to database
                flightContext.Update(flightSchedule);
                return RedirectToAction("ViewFlightSchedule");
            }
            ViewData["statusList"] = getStatus();
            return View();
        }


        private List<SelectListItem> GetStatus()
        {
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem
            {
                Value = "Full",
                Text = "Full"
            });
            status.Add(new SelectListItem
            {
                Value = "Delayed",
                Text = "Delayed"
            });
            status.Add(new SelectListItem
            {
                Value = "Cancel",
                Text = "Cancel"
            });
            return status;
        }
        public List<SelectListItem> getAircraft()
        {
            List<SelectListItem> aircraft = new List<SelectListItem>();
            for (int i = 1;i<=aircraftContext.GetAllAircraft().Count;i++)
            {
                aircraft.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text= i.ToString()
                });
            }
            return aircraft;
        }

        public List<SelectListItem> getRoute()
        {
            List<SelectListItem> routes = new List<SelectListItem>();
            for (int i = 1; i <= flightContext.GetAllRoutes().Count; i++)
            {
                routes.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
            return routes;
        }

        public ActionResult Home()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        public ActionResult ViewFlight()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public ActionResult ViewFlightRoute()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Admin" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "admin"))
            {
                return RedirectToAction("Index", "Home");
            }


            List<FlightRoute> routeList = flightContext.GetAllRoutes();
            return View(routeList);

        }
        public ActionResult ViewFlightSchedule()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Admin" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "admin"))
            {
                return RedirectToAction("Index", "Home");
            }


            List<FlightSchedule> scheduleList = flightContext.GetFlightSchedules();
            return View(scheduleList);

        }

        private List<SelectListItem> getStatus()
        {
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem
            {
                Value = "Opened",
                Text = "Opened"
            });
            status.Add(new SelectListItem
            {
                Value = "Full",
                Text = "Full"
            });
            status.Add(new SelectListItem
            {
                Value = "Delayed",
                Text = "Delayed"
            });
            status.Add(new SelectListItem
            {
                Value = "Cancelled",
                Text = "Cancelled"
            });
            return status;
        }

        [HttpGet]
        public ActionResult CreateFlightRoute()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            //FlightRoute route =  
            //Staff staff = staffContext.GetDetails(id);
            //StaffViewModel staffVM = MapToStaffVM(staff);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlightRoute(FlightRoute flightRoute)
        {
            if (ModelState.IsValid)
            {

                //Add staff record to database
                flightRoute.RouteID = flightContext.AddRoute(flightRoute);
                //Redirect user to Staff/Index view
                return RedirectToAction("Home");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(flightRoute);
            }
        }

        public ActionResult CreateFlightSchedule()
        {
            ViewData["route"] = getRoute();
            ViewData["aircraft"] = getAircraft();

            ViewData["status"] = getStatus();
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            //FlightRoute route =  
            //Staff staff = staffContext.GetDetails(id);
            //StaffViewModel staffVM = MapToStaffVM(staff);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlightSchedule(FlightSchedule flightSchedule)
        {
            ViewData["status"] = getStatus();
            ViewData["route"] = getRoute();
            ViewData["aircraft"] = getAircraft();

            if (ModelState.IsValid)
            {
                DateTime addDate = DateTime.Now.AddDays(1);
                if (flightSchedule.DepartureDateTime < addDate)
                {
                    TempData["Message"] = "Invalid Departure Date! Please ensure it is 1 day from today";
                    // Redirect user back to the index view through an action
                    return View();
                }

                
                FlightRoute flightRoute = flightContext.getSpecificRoute(flightSchedule.RouteID);
                int fduration = flightRoute.FlightDuration;
                if (fduration != null)
                {
                    int Duration = Convert.ToInt32(flightRoute.FlightDuration);
                    flightSchedule.ArrivalDateTime = flightSchedule.DepartureDateTime.Value.AddHours(Convert.ToDouble(Duration));
                    //Add staff record to database
                    


                    flightSchedule.ScheduleID = flightContext.AddSchedule(flightSchedule);
                    //Redirect user to Staff/Index view
                    return RedirectToAction("ViewFlightSchedule");

                }
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Details, please ensure it is entered correctly";
                // Redirect user back to the index view through an action
                return View();               
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View();
            }
        }
    }
}
