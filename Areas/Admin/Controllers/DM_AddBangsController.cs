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
    [Authorize]
    public class DM_AddBangsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/DM_AddBangs
        public async Task<ActionResult> Index(string search, string message)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            IQueryable<DM_AddBangs> addBangs = db.DM_AddBangs;
            if (!string.IsNullOrEmpty(search))
            {
                addBangs = addBangs.Where(c => c.TenBang.Trim().ToLower().Contains(search.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            ViewBag.Title = "Danh sách bảng -";

            return View(await addBangs.ToListAsync());
        }

        // GET: Admin/DM_AddBangs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_AddBangs dM_AddBangs = await db.DM_AddBangs.FindAsync(id);
            if (dM_AddBangs == null)
            {
                return HttpNotFound();
            }
            return View(dM_AddBangs);
        }

        // GET: Admin/DM_AddBangs/Create
        public ActionResult Create()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            return PartialView();
        }

        // POST: Admin/DM_AddBangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TenBang,Note")] DM_AddBangs dM_AddBangs)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }         
            if (ModelState.IsValid)
            {
                // 2. Kiểm tra xem tên bảng đã tồn tại chưa
                if (db.DM_AddBangs.Any(c => c.TenBang.Trim().ToLower() == dM_AddBangs.TenBang.Trim().ToLower()))
                {
                    ModelState.AddModelError("TenBang", "Tên bảng đã tồn tại.");
                }
                // 3. Nếu không có lỗi, thêm mới
                dM_AddBangs.IsActive = true;
                dM_AddBangs.CreateDate = DateTime.Now;
                dM_AddBangs.CreateUserId = userId;
                db.DM_AddBangs.Add(dM_AddBangs);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: Admin/DM_AddBangs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_AddBangs dM_AddBangs = await db.DM_AddBangs.FindAsync(id);
            if (dM_AddBangs == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_AddBangs);
        }

        // POST: Admin/DM_AddBangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenBang,IsActive,Note")] DM_AddBangs dM_AddBangs)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (ModelState.IsValid)
            {
                // 2. Kiểm tra xem tên bảng đã tồn tại chưa
                if (db.DM_AddBangs.Any(c => c.TenBang.Trim().ToLower() == dM_AddBangs.TenBang.Trim().ToLower() && c.Id != dM_AddBangs.Id))
                {
                    ModelState.AddModelError("TenBang", "Tên bảng đã tồn tại.");
                }
                // 3. Nếu không có lỗi, cập nhật
                dM_AddBangs.ModifiedDate = DateTime.Now;
                dM_AddBangs.ModifiedUserId = userId;
                db.Entry(dM_AddBangs).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: Admin/DM_AddBangs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_AddBangs dM_AddBangs = await db.DM_AddBangs.FindAsync(id);
            if (dM_AddBangs == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_AddBangs);
        }

        // POST: Admin/DM_AddBangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // 2. Kiểm tra xem bảng có dữ liệu liên quan không
            if (db.HoSoPhapLys.Any(c => c.AddBangId == id))
            {
                ViewBag.Message = "Không thể xóa bảng này vì có dữ liệu liên quan.";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            // 3. Nếu không có dữ liệu liên quan, xóa bảng
            DM_AddBangs dM_AddBangs = await db.DM_AddBangs.FindAsync(id);
            db.DM_AddBangs.Remove(dM_AddBangs);
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
