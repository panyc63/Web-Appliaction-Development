using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment.DAL;
using Assignment.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;

namespace Assignment.Controllers
{
    public class FlightpersonnelController : Controller
    {
        private StaffDAL staffContext = new StaffDAL();
        private FlightScheduleDAL scheduleContext = new FlightScheduleDAL();
        public IActionResult homepage()
        {
            return View();
        }
        public IActionResult viewstaff()
        {
            List<Staff> staffList = staffContext.GetAllStaff();
            return View(staffList);
        }

        //add new personnels
        public ActionResult Create()
        {
            return View();
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("viewstaff");
            }
            Staff staff = staffContext.GetDetails(id.Value);
            if (staff == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("viewstaff");
            }
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Staff staff)
        {
            //in case of the need to return to Edit.cshtml view
            if (ModelState.IsValid)
            {
                //Update staff record to database
                staffContext.Update(staff);
                return RedirectToAction("viewstaff");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(staff);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Staff staff)
        {
            //in case of the need to return to Create.cshtml view
            if (ModelState.IsValid)
            {
                //Add staff record to database
                staff.StaffID = staffContext.Add(staff);
                //Redirect user to Staff/Index view
                return RedirectToAction("viewstaff");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(staff);
            }
        }

        //Personnel scheduler page
        public ActionResult CreateSchedule(int? id)
        {
            List<FlightSchedule> scheduleList = scheduleContext.GetFlightSchedules();
            return View(scheduleList);
        }

        //Add personnel to a schedule
        public ActionResult AddSchedule(int? id)
        {
            if (id != null)
            {
                ViewData["selectedScheduleID"] = id.Value;
            }
            else
            {
                ViewData["selectedScheduleID"] = "";
            }
            PilotAttendant pilotattendant = new PilotAttendant();
            pilotattendant.Staff1 = staffContext.GetAllPilotStaff();
            pilotattendant.Staff2 = staffContext.GetAllAttendantStaff();
            return View(pilotattendant);
        }


        public ActionResult AssignPilot(int? id)
        {
            
            ViewData["selectedPilotID"] = id.Value;        
            return View();
        }
        public ActionResult AssignAttendant(int? id)
        {
            ViewData["selectedPilotID"] = id.Value;
            return View();
        }

        //add into flightcrew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPilot(FlightCrew crew)
        {
            if (ModelState.IsValid)
            {
                //Add staff record to database
                crew.ScheduleID = staffContext.AddSchedule(crew);
                crew.StaffID = staffContext.Updatestatus(crew);
                //Redirect user to Staff/Index view
                return RedirectToAction("CreateSchedule");

            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(crew);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignAttendant(FlightCrew crew)
        {
            if (ModelState.IsValid)
            {
                //Add staff record to database
                crew.ScheduleID = staffContext.AddSchedule(crew);
                //Redirect user to Staff/Index view
                return RedirectToAction("CreateSchedule");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(crew);
            }
        }
    }
}
