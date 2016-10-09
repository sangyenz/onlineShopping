using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    public class MembersController : DefaultController
    {
        private Entities db = new Entities();

        // GET: Members
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(string MemberName)
        {
            if (MemberName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = from m in db.Members
                        where m.MemberName == MemberName
                        select m;
            Member member = query.First<Member>();
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        // GET: Members/Create
        public ActionResult Create()
        {
            //List<SelectListItem> KnowWays = new List<SelectListItem>();
            //KnowWays.Add(new SelectListItem { Text = "Advertisement", Value = "Ad" });
            //KnowWays.Add(new SelectListItem { Text = "Through Friends", Value = "TF" });
            //KnowWays.Add(new SelectListItem { Text = "Others", Value = "Others" });

            //ViewData["KnowWay"] = KnowWays;

            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberId,MemberName,FirstName,SurName,DateOfBirth,StreetNo,StreetName,Suburb,City,ZipCode,PhoneNo,KnowWay,ContactWay,InsertDate,LastLogin,CancelFlg")] Member member)
        {
            member.MemberName = User.Identity.Name;
            member.DateOfBirth = Convert.ToDateTime(member.DateOfBirth);
            member.KnowWay = Request.Form["KnowWay"];
            member.ContactWay = Request.Form["ContactWay"];
            member.InsertDate = DateTime.Now;
            member.LastLogin = DateTime.Now;
            member.CancelFlg = 0;

            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Details", new { MemberName = member.MemberName});
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,MemberName,FirstName,SurName,DateOfBirth,StreetNo,StreetName,Suburb,City,ZipCode,PhoneNo,KnowWay,ContactWay,InsertDate,LastLogin,CancelFlg")] Member member)
        {
            Member m = db.Members.Find(member.MemberId);
            m.FirstName = member.FirstName;
            m.SurName = member.SurName;
            m.DateOfBirth = member.DateOfBirth;
            m.StreetNo = member.StreetNo;
            m.StreetName = member.StreetName;
            m.Suburb = member.Suburb;
            m.City = member.City;
            m.ZipCode = member.ZipCode;
            m.PhoneNo = member.PhoneNo;
            m.KnowWay = Request.Form["KnowWay"];
            m.ContactWay = Request.Form["ContactWay"];


            if (ModelState.IsValid)
            {
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { MemberName = m.MemberName});
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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

    }
}
