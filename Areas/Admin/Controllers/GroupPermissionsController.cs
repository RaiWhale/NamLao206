using NamLao206.Models;
using NamLao206.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            ViewBag.GroupId = new SelectList(db.PermissionGroups, "Id", "GroupName");
            ViewBag.PermissionId = new SelectList(db.Permissions, "Id", "PermissionName");
            return PartialView();
        }

        // POST: Admin/GroupPermissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,GroupId,PermissionId,CreatedDate,CreatedBy")] GroupPermission groupPermission)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
         
                // Lấy thông tin tài khoản
                var acc = db.Accounts
                    .Where(x => x.Id == userId)
                    .SingleOrDefault();
                if (acc == null)
                {
                    ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                    return RedirectToAction("Login", "Login", new { area = "" });
                }
                var checkExists = db.GroupPermissions.Where(x => x.PermissionId == groupPermission.PermissionId && x.GroupId == groupPermission.GroupId).SingleOrDefault();
                if (checkExists != null)
                {
                    ViewBag.Message = "Đã tồn tại quyền!";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                groupPermission.CreatedDate = DateTime.Now;
                groupPermission.CreatedBy = userId;
                db.GroupPermissions.Add(groupPermission);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: Admin/GroupPermissions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
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
            return PartialView(groupPermission);
        }

        // POST: Admin/GroupPermissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,GroupId,PermissionId,CreatedDate,CreatedBy")] GroupPermission groupPermission)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (ModelState.IsValid)
            {
                // Lấy thông tin tài khoản
                var acc = db.Accounts
                    .Where(x => x.Id == userId)
                    .SingleOrDefault();
                if (acc == null)
                {
                    ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                    return RedirectToAction("Login", "Login", new { area = "" });
                }
                var checkExists = db.GroupPermissions.Where(x => x.PermissionId == groupPermission.PermissionId && x.GroupId == groupPermission.GroupId).SingleOrDefault();
                if (checkExists != null)
                {
                    ViewBag.Message = "Đã tồn tại quyền!";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                db.Entry(groupPermission).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: Admin/GroupPermissions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupPermission groupPermission = await db.GroupPermissions.FindAsync(id);
            if (groupPermission == null)
            {
                return HttpNotFound();
            }
            return PartialView(groupPermission);
        }

        // POST: Admin/GroupPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            GroupPermission groupPermission = await db.GroupPermissions.FindAsync(id);       
            db.GroupPermissions.Remove(groupPermission);
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
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
