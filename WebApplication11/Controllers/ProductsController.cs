using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    public class ProductsController : DefaultController
    {
        private Entities db = new Entities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/DetailByCategory/
        public ActionResult DetailByCategory(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);
            ViewBag.CategoryName = category.CategoryName;

            List<Product> products = db.Products.Where(x => x.CategoryNo == id).ToList();

            int pageNumber = page ?? 1;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            IPagedList<Product> pagedList = products.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: Products/Specials/
        public ActionResult Specials(int? page)
        {
            List<Product> products = db.Products.Where(x => x.CancelFlg == 1).ToList();

            int pageNumber = page ?? 1;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            IPagedList<Product> pagedList = products.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryNo = new SelectList(db.Categories, "CategoryNo", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductName,CategoryNo,Discription,Picture,Quantity,InStock,Price,MemberPrice,Weight,InsertDate,UpdateDate,CancelFlg")] Product product)
        {
            var files = Request.Files;
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var picFileName = "p_" + Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(Server.MapPath("~/Image/"), picFileName);
                    file.SaveAs(filePath);
                    product.Picture = picFileName;
                }
            }
            else {
                product.Picture = "vortex.png";
            }

            product.Quantity = 1;
            product.InsertDate = DateTime.Now;
            product.UpdateDate = DateTime.Now;
            product.CancelFlg = 0;

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryNo = new SelectList(db.Categories, "CategoryNo", "CategoryName", product.CategoryNo);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryNo = new SelectList(db.Categories, "CategoryNo", "CategoryName", product.CategoryNo);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductName,CategoryNo,Discription,Picture,Quantity,InStock,Price,MemberPrice,Weight,InsertDate,UpdateDate,CancelFlg")] Product product)
        {
            Product p = db.Products.Find(product.Id);
           
            var files = Request.Files;
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var picFileName = "p_" + Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(Server.MapPath("~/Image/"), picFileName);
                    file.SaveAs(filePath);
                    p.Picture = picFileName;
                }
            }
            p.ProductName = product.ProductName;
            p.CategoryNo = product.CategoryNo;
            p.Discription = product.Discription;
            p.InStock = product.InStock;
            p.Price = product.Price;
            p.MemberPrice = product.MemberPrice;
            p.Weight = product.Weight;
            p.UpdateDate = DateTime.Now;
            p.CancelFlg = product.CancelFlg;

            if (ModelState.IsValid)
            {
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryNo = new SelectList(db.Categories, "CategoryNo", "CategoryName", product.CategoryNo);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
