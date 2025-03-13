using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class DM_DonVisController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/DM_DonVis
        public ActionResult Index()
        {
            return View(db.DM_DonVis.ToList());
        }

        // GET: Admin/DM_DonVis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_DonVis dM_DonVis = db.DM_DonVis.Find(id);
            if (dM_DonVis == null)
            {
                return HttpNotFound();
            }
            return View(dM_DonVis);
        }

        // GET: Admin/DM_DonVis/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/DM_DonVis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenDonVi,Description,Parent_Id")] DM_DonVis dM_DonVis)
        {
            if (ModelState.IsValid)
            {
                dM_DonVis.IsActive = true;
                dM_DonVis.CreateDate = DateTime.Now;
                db.DM_DonVis.Add(dM_DonVis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dM_DonVis);
        }

        // GET: Admin/DM_DonVis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_DonVis dM_DonVis = db.DM_DonVis.Find(id);
            if (dM_DonVis == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_DonVis);
        }

        // POST: Admin/DM_DonVis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenDonVi,IsActive,Description,CreateDate,Parent_Id")] DM_DonVis dM_DonVis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_DonVis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dM_DonVis);
        }

        // GET: Admin/DM_DonVis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_DonVis dM_DonVis = db.DM_DonVis.Find(id);
            if (dM_DonVis == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_DonVis);
        }

        // POST: Admin/DM_DonVis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_DonVis dM_DonVis = db.DM_DonVis.Find(id);
            db.DM_DonVis.Remove(dM_DonVis);
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
