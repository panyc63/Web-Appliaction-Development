using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assignment.DAL;
using Assignment.Models;


namespace Assignment.Controllers
{
    public class AircraftController : Controller
    {
        private AircraftDAL aircraftContext = new AircraftDAL();

        // GET: Aircraft
        public ActionResult Index()
        {
            return View();

            
        }
        public ActionResult Home()
        {
            return View();
        }
        public IActionResult ViewAircraft()
        {
            List<Aircraft> aircraftList = aircraftContext.GetAllAircraft();
            return View(aircraftList);
        }

        // GET: Aircraft/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Aircraft/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Aircraft/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aircraft aircraft)
        {   //Add staff record to database
            aircraft.AircraftID = aircraftContext.Add(aircraft);
            //Redirect user to Staff/Index view
            return RedirectToAction("ViewAircraft");
        }



        // GET: Aircraft/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("ViewAircraft");
            }
            else
            {
                Aircraft aircraft = aircraftContext.GetDetails(id.Value);
                if (aircraft == null)
                {
                    //Return to listing page, not allowed to edit
                    return RedirectToAction("ViewAircraft");
                }
                return View();
            }
        }

        // POST: Aircraft/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Aircraft aircraft)
        {
            aircraftContext.Update(aircraft);
            return RedirectToAction("ViewAircraft");

        }

        // GET: Aircraft/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Aircraft/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}