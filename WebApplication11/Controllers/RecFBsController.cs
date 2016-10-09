using System;
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
    public class RecFBsController : DefaultController
    {
        private Entities db = new Entities();

        // GET: RecFBs
        public ActionResult Index()
        {
            var recFBs = db.RecFBs.Include(r => r.Member);
            return View(recFBs.ToList());
        }

        // GET: RecFBs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecFB recFB = db.RecFBs.Find(id);
            if (recFB == null)
            {
                return HttpNotFound();
            }
            return View(recFB);
        }

        // GET: RecFBs/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberName");
            return View();
        }

        // POST: RecFBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MemberId,Description,InsertDate,UpdateDate,CancelFlg")] RecFB recFB)
        {
            recFB.MemberId = db.Members.Where(n => n.MemberName == User.Identity.Name).First().MemberId;
            recFB.InsertDate = DateTime.Now;
            recFB.UpdateDate = DateTime.Now;
            recFB.CancelFlg = 0;


            if (ModelState.IsValid)
            {
                db.RecFBs.Add(recFB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberName", recFB.MemberId);
            return View(recFB);
        }

        // GET: RecFBs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecFB recFB = db.RecFBs.Find(id);
            if (recFB == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberName", recFB.MemberId);
            return View(recFB);
        }

        // POST: RecFBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MemberId,Description,InsertDate,UpdateDate,CancelFlg")] RecFB recFB)
        {
            RecFB r = db.RecFBs.Find(recFB.Id);
            r.Description = recFB.Description;
            r.UpdateDate = DateTime.Now;
            r.CancelFlg = recFB.CancelFlg;

            if (ModelState.IsValid)
            {
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberName", recFB.MemberId);
            return View(recFB);
        }

        // GET: RecFBs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecFB recFB = db.RecFBs.Find(id);
            if (recFB == null)
            {
                return HttpNotFound();
            }
            return View(recFB);
        }

        // POST: RecFBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecFB recFB = db.RecFBs.Find(id);
            db.RecFBs.Remove(recFB);
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
