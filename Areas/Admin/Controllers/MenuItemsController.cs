using NamLao206.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    public class MenuItemsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        int pageSize = 10;
        // GET: Admin/MenuItems
        public async Task<ActionResult> Index(int? page, string search, string message)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            IQueryable<MenuItem> menuItems = db.MenuItems
                  .Include(d => d.DM_DonVis);
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            if (!string.IsNullOrEmpty(search))
            {
                menuItems = menuItems.Where(c => c.MenuName.ToLower().Contains(search.Trim().ToLower()));
            }
            menuItems = menuItems.OrderByDescending(c => c.CreatedDate);
            //Paging		     
            //int pageNumber = page ?? 1;
            ViewBag.Title = "Menu phân quyền -";
            ViewBag.search = search;
            return View(await menuItems.ToListAsync());
        }

        // GET: Admin/MenuItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = await db.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // GET: Admin/MenuItems/Create
        public ActionResult Create()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ParentId = new SelectList(db.MenuItems.Where(x => x.IsActive == true), "Id", "MenuName");
            ViewBag.Departments = new SelectList(db.DM_DonVis.Where(x => x.IsActive == true), "Id", "TenDonVi");
            return PartialView();
        }

        // POST: Admin/MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MenuName,MenuUrl,ParentId,IconClass,PermissionCode,DisplayOrder,DepartmentId")] MenuItem menuItem)
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
                menuItem.CreatedDate = DateTime.Now;
                menuItem.IsActive = true;
                db.MenuItems.Add(menuItem);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }

            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: Admin/MenuItems/Edit/5
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
            MenuItem menuItem = await db.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.MenuItems.Where(x => x.IsActive == true), "Id", "MenuName", menuItem.IsActive);
            ViewBag.DepartmentId = new SelectList(db.DM_DonVis.Where(x => x.IsActive == true), "Id", "TenDonVi", menuItem.DepartmentId);
            return PartialView(menuItem);
        }

        // POST: Admin/MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MenuName,MenuUrl,ParentId,IconClass,PermissionCode,DisplayOrder,IsActive,DepartmentId")] MenuItem menuItem)
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
                // Gán thông tin người sửa
                db.Entry(menuItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: Admin/MenuItems/Delete/5
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
            MenuItem menuItem = await db.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return PartialView(menuItem);
        }

        // POST: Admin/MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            MenuItem menuItem = await db.MenuItems.FindAsync(id);
            menuItem.IsActive = false;
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
