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
    public class DM_hinhthuckinhdoanhController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/DM_hinhthuckinhdoanh
        public ActionResult Index()
        {
            return View(db.DM_hinhthuckinhdoanh.ToList());
        }

        // GET: Admin/DM_hinhthuckinhdoanh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_hinhthuckinhdoanh dM_hinhthuckinhdoanh = db.DM_hinhthuckinhdoanh.Find(id);
            if (dM_hinhthuckinhdoanh == null)
            {
                return HttpNotFound();
            }
            return View(dM_hinhthuckinhdoanh);
        }

        // GET: Admin/DM_hinhthuckinhdoanh/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/DM_hinhthuckinhdoanh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Hinhthuc,mota,IsActive")] DM_hinhthuckinhdoanh dM_hinhthuckinhdoanh)
        {
            if (ModelState.IsValid)
            {                
                db.DM_hinhthuckinhdoanh.Add(dM_hinhthuckinhdoanh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dM_hinhthuckinhdoanh);
        }

        // GET: Admin/DM_hinhthuckinhdoanh/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_hinhthuckinhdoanh dM_hinhthuckinhdoanh = db.DM_hinhthuckinhdoanh.Find(id);
            if (dM_hinhthuckinhdoanh == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_hinhthuckinhdoanh);
        }

        // POST: Admin/DM_hinhthuckinhdoanh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Hinhthuc,mota,IsActive")] DM_hinhthuckinhdoanh dM_hinhthuckinhdoanh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_hinhthuckinhdoanh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dM_hinhthuckinhdoanh);
        }

        // GET: Admin/DM_hinhthuckinhdoanh/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_hinhthuckinhdoanh dM_hinhthuckinhdoanh = db.DM_hinhthuckinhdoanh.Find(id);
            if (dM_hinhthuckinhdoanh == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_hinhthuckinhdoanh);
        }

        // POST: Admin/DM_hinhthuckinhdoanh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_hinhthuckinhdoanh dM_hinhthuckinhdoanh = db.DM_hinhthuckinhdoanh.Find(id);
            db.DM_hinhthuckinhdoanh.Remove(dM_hinhthuckinhdoanh);
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
