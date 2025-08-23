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
	public class UnitsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/Units
        public async Task<ActionResult> Index(string message, string search)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			IQueryable<Unit> units = db.Units.Include(u => u.Account).Include(u => u.Account1);
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			if (!string.IsNullOrEmpty(search))
			{
				units = units.Where(x => x.UnitName.Contains(search));
			}
			ViewBag.Title = "Đơn vị tính - ";		
            return View(await units.ToListAsync());
        }

        // GET: Admin/Units/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = await db.Units.FindAsync(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // GET: Admin/Units/Create
        public ActionResult Create()
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			return PartialView();
        }

        // POST: Admin/Units/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UnitName,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] Unit unit)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}

			if (ModelState.IsValid)
            {
				//Lấy thông tin tài khoản
				var acc = db.Accounts
					.Where(x => x.Id == userId)
					.SingleOrDefault();
				if (acc == null)
				{
					ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
				//Gán thông tin tài khoản vào đơn vị
				unit.CreateUserId = acc.Id;		
				unit.CreateDate = DateTime.Now;
                unit.IsActive = true;
				db.Units.Add(unit);
                await db.SaveChangesAsync();
				ViewBag.Message = "Thêm mới thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/Units/Edit/5
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
            Unit unit = await db.Units.FindAsync(id);
            if (unit == null)
            {
                return HttpNotFound();
            }        
            return PartialView(unit);
        }

        // POST: Admin/Units/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UnitName,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] Unit unit)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}

			if (ModelState.IsValid)
            {
				//Lấy thông tin tài khoản
				var acc = db.Accounts
					.Where(x => x.Id == userId)
					.SingleOrDefault();
				if (acc == null)
				{
					ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
				//Gán thông tin tài khoản vào đơn vị
				unit.ModifiedUserId = acc.Id;
				unit.ModifiedDate = DateTime.Now;				
				db.Entry(unit).State = EntityState.Modified;
                await db.SaveChangesAsync();
				ViewBag.Message = "Sửa thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/Units/Delete/5
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
            Unit unit = await db.Units.FindAsync(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return PartialView(unit);
        }

        // POST: Admin/Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			Unit unit = await db.Units.FindAsync(id);
            db.Units.Remove(unit);
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
