using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication11.Controllers
{
    public class HomeController : DefaultController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Please use the form below to send us a message.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string msg)
        {
            if (ModelState.IsValid) {

                TempData["Confirm"] = true;
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}