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
    public class GroupPermissionsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        int pageSize = 10;
        // GET: Admin/GroupPermissions
        public async Task<ActionResult> Index(int? page, string search, string message)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            IQueryable<GroupPermission> groupPermissions = db.GroupPermissions.Include(g => g.PermissionGroup).Include(g => g.Permission);

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            if (!string.IsNullOrEmpty(search))
            {
                groupPermissions = groupPermissions.Where(c => c.PermissionGroup.GroupName.ToLower().Contains(search.Trim().ToLower()));
            }
            groupPermissions = groupPermissions.OrderByDescending(c => c.CreatedDate);
            //Paging		     
            //int pageNumber = page ?? 1;
            ViewBag.Title = "Menu phân quyền -";
            ViewBag.search = search;
            return View(await groupPermissions.ToListAsync());                 
        }

        // GET: Admin/GroupPermissions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupPermission groupPermission = await db.GroupPermissions.FindAsync(id);
            if (groupPermission == null)
            {
                return HttpNotFound();
            }
            return View(groupPermission);
        }

        // GET: Admin/GroupPermissions/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.PermissionGroups, "Id", "GroupName");
            ViewBag.PermissionId = new SelectList(db.Permissions, "Id", "PermissionName");
            return View();
        }

        // POST: Admin/GroupPermissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,GroupId,PermissionId,CreatedDate,CreatedBy")] GroupPermission groupPermission)
        {
            if (ModelState.IsValid)
            {
                db.GroupPermissions.Add(groupPermission);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.PermissionGroups, "Id", "GroupName", groupPermission.GroupId);
            ViewBag.PermissionId = new SelectList(db.Permissions, "Id", "PermissionName", groupPermission.PermissionId);
            return View(groupPermission);
        }

        // GET: Admin/GroupPermissions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupPermission groupPermission = await db.GroupPermissions.FindAsync(id);
            if (groupPermission == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.PermissionGroups, "Id", "GroupName", groupPermission.GroupId);
            ViewBag.PermissionId = new SelectList(db.Permissions, "Id", "PermissionName", groupPermission.PermissionId);
            return View(groupPermission);
        }

        // POST: Admin/GroupPermissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,GroupId,PermissionId,CreatedDate,CreatedBy")] GroupPermission groupPermission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupPermission).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.GroupId = new SelectList(db.PermissionGroups, "Id", "GroupName", groupPermission.GroupId);
            ViewBag.PermissionId = new SelectList(db.Permissions, "Id", "PermissionName", groupPermission.PermissionId);
            return View(groupPermission);
        }

        // GET: Admin/GroupPermissions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupPermission groupPermission = await db.GroupPermissions.FindAsync(id);
            if (groupPermission == null)
            {
                return HttpNotFound();
            }
            return View(groupPermission);
        }

        // POST: Admin/GroupPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GroupPermission groupPermission = await db.GroupPermissions.FindAsync(id);
            db.GroupPermissions.Remove(groupPermission);
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
