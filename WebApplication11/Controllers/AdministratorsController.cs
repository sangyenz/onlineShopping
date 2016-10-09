using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    public class AdministratorsController : DefaultController
    {
        private Entities db = new Entities();

        // GET: /Administrators/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Administrators/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] Administrator administrator)
        {
            if (!ModelState.IsValid)
            {
                return View(administrator);
            }

            List<Administrator> result =db.Administrators.Where(x => x.Email == administrator.Email).ToList();

            if (result != null && result.Count > 0)
            {
                if (result.First().Password == administrator.Password)
                {
                    Session["AdminStatus"] = result.First();
                    return RedirectToAction("Index", "Administrators");
                }
                else
                {
                    ModelState.AddModelError("", "Please confirm your password.");
                    return View(administrator);
                }
            }
            else {
                ModelState.AddModelError("", "Invalid Email.");
                return View(administrator);           
            }
        }

        // POST: /Administrator/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["AdminStatus"] = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: Administrators
        public ActionResult Index()
        {
            return View();
        }

        // GET: Administrators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // GET: Administrators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminId,AdminName,Password,Email,InsertDate,LastLogin,CancelFlg")] Administrator administrator)
        {

            List<Administrator> result = db.Administrators.Where(x => x.Email == administrator.Email).ToList();
            if (result != null && result.Count > 0)
            {
                ModelState.AddModelError("", "The Email has already existed.");
                return View(administrator);
            }


            administrator.AdminName = administrator.Email;
            administrator.InsertDate = DateTime.Now;
            administrator.LastLogin = DateTime.Now;
            administrator.CancelFlg = 0;

            if (ModelState.IsValid)
            {
                db.Administrators.Add(administrator);
                db.SaveChanges();
                Session["AdminStatus"] = administrator;
                return RedirectToAction("Index");
            }

            return View(administrator);
        }

        // GET: Administrators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminId,AdminName,Password,Email,InsertDate,LastLogin,CancelFlg")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administrator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Administrator administrator = db.Administrators.Find(id);
            db.Administrators.Remove(administrator);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void JudgeAdminName(object sender, EventArgs e)
        {
            string adminName = Request["adminName"];
            Administrator a = (Administrator)db.Administrators.Where(x => x.AdminName == adminName);
            if (a != null) {
                if (a.AdminName == adminName) {
                    Response.Write("This email is already existed.");
                }

            }

        }
    }
}
