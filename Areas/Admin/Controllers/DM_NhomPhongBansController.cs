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
    public class DM_NhomPhongBansController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/DM_NhomPhongBans
        public ActionResult Index()
        {
            return View(db.DM_NhomPhongBans.ToList());
        }

        // GET: Admin/DM_NhomPhongBans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_NhomPhongBans dM_NhomPhongBans = db.DM_NhomPhongBans.Find(id);
            if (dM_NhomPhongBans == null)
            {
                return HttpNotFound();
            }
            return View(dM_NhomPhongBans);
        }

        // GET: Admin/DM_NhomPhongBans/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/DM_NhomPhongBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nhomkhoa,ParentId")] DM_NhomPhongBans dM_NhomPhongBans)
        {
            if (ModelState.IsValid)
            {
                db.DM_NhomPhongBans.Add(dM_NhomPhongBans);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dM_NhomPhongBans);
        }

        // GET: Admin/DM_NhomPhongBans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_NhomPhongBans dM_NhomPhongBans = db.DM_NhomPhongBans.Find(id);
            if (dM_NhomPhongBans == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_NhomPhongBans);
        }

        // POST: Admin/DM_NhomPhongBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nhomkhoa,ParentId")] DM_NhomPhongBans dM_NhomPhongBans)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_NhomPhongBans).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dM_NhomPhongBans);
        }

        // GET: Admin/DM_NhomPhongBans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_NhomPhongBans dM_NhomPhongBans = db.DM_NhomPhongBans.Find(id);
            if (dM_NhomPhongBans == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_NhomPhongBans);
        }

        // POST: Admin/DM_NhomPhongBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_NhomPhongBans dM_NhomPhongBans = db.DM_NhomPhongBans.Find(id);
            db.DM_NhomPhongBans.Remove(dM_NhomPhongBans);
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
