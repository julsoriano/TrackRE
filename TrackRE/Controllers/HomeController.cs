﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrackRE.Models;

namespace TrackRE.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Properties.ToList());
            // return RedirectToAction("IndexView", "PropertyTypes");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            // Throw a test error so that we can see that it is handled by Elmah
            // To test go to the ~/elmah.axd page to see if the error is being logged correctly
            // throw new Exception("A test exception for ELMAH");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}