using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.Admin.Controllers
{
    public class PermissionGroupsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/PermissionGroups
        public async Task<ActionResult> Index()
        {
            return View(await db.PermissionGroups.ToListAsync());
        }

        // GET: Admin/PermissionGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermissionGroup permissionGroup = await db.PermissionGroups.FindAsync(id);
            if (permissionGroup == null)
            {
                return HttpNotFound();
            }
            return View(permissionGroup);
        }

        // GET: Admin/PermissionGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/PermissionGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,GroupName,Description,IsActive,CreatedDate,CreatedBy")] PermissionGroup permissionGroup)
        {
            if (ModelState.IsValid)
            {
                db.PermissionGroups.Add(permissionGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(permissionGroup);
        }

        // GET: Admin/PermissionGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermissionGroup permissionGroup = await db.PermissionGroups.FindAsync(id);
            if (permissionGroup == null)
            {
                return HttpNotFound();
            }
            return View(permissionGroup);
        }

        // POST: Admin/PermissionGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,GroupName,Description,IsActive,CreatedDate,CreatedBy")] PermissionGroup permissionGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permissionGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(permissionGroup);
        }

        // GET: Admin/PermissionGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermissionGroup permissionGroup = await db.PermissionGroups.FindAsync(id);
            if (permissionGroup == null)
            {
                return HttpNotFound();
            }
            return View(permissionGroup);
        }

        // POST: Admin/PermissionGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PermissionGroup permissionGroup = await db.PermissionGroups.FindAsync(id);
            db.PermissionGroups.Remove(permissionGroup);
            await db.SaveChangesAsync();
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
