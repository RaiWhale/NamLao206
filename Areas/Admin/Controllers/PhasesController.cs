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
	public class PhasesController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/Phases
        public async Task<ActionResult> Index(string message, string search)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			IQueryable<Phase> phases = db.Phases.Include(p => p.Account).Include(p => p.Account1);
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			if (!string.IsNullOrEmpty(search))
			{
				phases = phases.Where(x => x.PhaseName.Contains(search));
			}
			ViewBag.Title = "Giai đoạn - ";		
            return View(await phases.ToListAsync());
        }

        // GET: Admin/Phases/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phase phase = await db.Phases.FindAsync(id);
            if (phase == null)
            {
                return HttpNotFound();
            }
            return View(phase);
        }

        // GET: Admin/Phases/Create
        public ActionResult Create()
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			return PartialView();
        }

        // POST: Admin/Phases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PhaseName,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] Phase phase)
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
				// Gán thông tin người tạo và người sửa
				phase.CreateUserId = acc.Id;
				phase.CreateDate = DateTime.Now;
                phase.IsActive = true; // Mặc định là hoạt động
				db.Phases.Add(phase);
                await db.SaveChangesAsync();
				ViewBag.Message = "Thêm mới thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/Phases/Edit/5
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
            Phase phase = await db.Phases.FindAsync(id);
            if (phase == null)
            {
                return HttpNotFound();
            }
            return PartialView(phase);
        }

        // POST: Admin/Phases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PhaseName,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] Phase phase)
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
				phase.ModifiedUserId = acc.Id;
				phase.ModifiedDate = DateTime.Now;	
				db.Entry(phase).State = EntityState.Modified;
                await db.SaveChangesAsync();
				ViewBag.Message = "Sửa thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/Phases/Delete/5
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
            Phase phase = await db.Phases.FindAsync(id);
            if (phase == null)
            {
                return HttpNotFound();
            }
            return PartialView(phase);
        }

        // POST: Admin/Phases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			Phase phase = await db.Phases.FindAsync(id);
            db.Phases.Remove(phase);
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
