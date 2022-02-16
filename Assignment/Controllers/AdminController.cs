using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        public ActionResult Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
           (HttpContext.Session.GetString("Role") != "admin"))
            {
                return RedirectToAction("LogIn", "Home");
            }

            return View();

        }

        
    }
}
