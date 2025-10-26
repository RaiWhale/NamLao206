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
        int pageSize = 10;
        // GET: Admin/PermissionGroups
        public async Task<ActionResult> Index(int? page, string search, string message)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            IQueryable<PermissionGroup> permissionGroup = db.PermissionGroups;

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            if (!string.IsNullOrEmpty(search))
            {
                permissionGroup = permissionGroup.Where(c => c.GroupName.ToLower().Contains(search.Trim().ToLower()));
            }
            permissionGroup = permissionGroup.OrderByDescending(c => c.CreatedDate);
            //Paging		     
            //int pageNumber = page ?? 1;
            ViewBag.Title = "Menu phân quyền -";
            ViewBag.search = search;
            return View(await permissionGroup.ToListAsync());
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
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            return PartialView();
        }

        // POST: Admin/PermissionGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,GroupName,Description,IsActive")] PermissionGroup permissionGroup)
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
                permissionGroup.IsActive = true;
                permissionGroup.CreatedDate = DateTime.Now;
                permissionGroup.CreatedBy = userId;
                db.PermissionGroups.Add(permissionGroup);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }

            return View(permissionGroup);
        }

        // GET: Admin/PermissionGroups/Edit/5
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
            PermissionGroup permissionGroup = await db.PermissionGroups.FindAsync(id);
            if (permissionGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(permissionGroup);
        }

        // POST: Admin/PermissionGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,GroupName,Description,IsActive")] PermissionGroup permissionGroup)
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
                db.Entry(permissionGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: Admin/PermissionGroups/Delete/5
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
            PermissionGroup permissionGroup = await db.PermissionGroups.FindAsync(id);
            if (permissionGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(permissionGroup);
        }

        // POST: Admin/PermissionGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            PermissionGroup permissionGroup = await db.PermissionGroups.FindAsync(id);
            permissionGroup.IsActive = false;
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
