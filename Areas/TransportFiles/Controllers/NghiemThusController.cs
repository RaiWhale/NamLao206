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

namespace NamLao206.Areas.TransportFiles.Controllers
{
    [Authorize]
	public class NghiemThusController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/NghiemThus
        public async Task<ActionResult> Index()
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}

			var nghiemThus = db.NghiemThus.Include(n => n.Account).Include(n=>n.Unit).Include(n => n.Account1).Include(n => n.Project);
            return View(await nghiemThus.ToListAsync());
        }

        // GET: TransportFiles/NghiemThus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NghiemThu nghiemThu = await db.NghiemThus.FindAsync(id);
            if (nghiemThu == null)
            {
                return HttpNotFound();
            }
            return View(nghiemThu);
        }

        // GET: TransportFiles/NghiemThus/Create
        public ActionResult CreateBomMin(int? projectID)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
          
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == projectID), "Id", "TenDuAn");                   
            ViewBag.PhaseId = new SelectList(db.Phases, "Id", "PhaseName");
            ViewBag.UnitId = new SelectList(db.Units.Where(x => x.PhanLoai == "1"), "Id", "UnitName");
            return PartialView();
        }
        public ActionResult CreateCumBan(int? projectID)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == projectID), "Id", "TenDuAn");
            ViewBag.PhaseId = new SelectList(db.Phases, "Id", "PhaseName");
            ViewBag.UnitId = new SelectList(db.Units.Where(x => x.PhanLoai == "1"), "Id", "UnitName");
            return PartialView();
        }
        // POST: TransportFiles/NghiemThus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProjectID,PhaseId" +
            ",PhaseValue,PhaseLand,PhaseWater,DoanhThu,DaThanhToan,NoConLai" +
            ",IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note,UnitId")] NghiemThu nghiemThu)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
            // Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (ModelState.IsValid)
            {			
				nghiemThu.CreateUserId = acc.Id;
				nghiemThu.CreateDate = DateTime.Now;
                nghiemThu.IsActive = true;                
				db.NghiemThus.Add(nghiemThu);
                await db.SaveChangesAsync();
				ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("InformationProject", "Projects"
                     , new { projectID = nghiemThu.ProjectID, message = ViewBag.Message });
            }
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects"
                , new { projectID = nghiemThu.ProjectID, message = ViewBag.Message });
        }

        // GET: TransportFiles/NghiemThus/Edit/5
        public async Task<ActionResult> EditBomMin(int? id)
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
            NghiemThu nghiemThu = await db.NghiemThus.FindAsync(id);
            if (nghiemThu == null)
            {
                return HttpNotFound();
            }	
			ViewBag.ProjectID = new SelectList(db.Projects, "Id", "TenDuAn", nghiemThu.ProjectID);
            ViewBag.PhaseId = new SelectList(db.Phases, "Id", "PhaseName", nghiemThu.PhaseId);
            ViewBag.UnitId = new SelectList(db.Units.Where(x => x.PhanLoai == "1"), "Id", "UnitName", nghiemThu.UnitId);
            return PartialView(nghiemThu);
        }
        public async Task<ActionResult> EditCumBan(int? id)
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
            NghiemThu nghiemThu = await db.NghiemThus.FindAsync(id);
            if (nghiemThu == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "Id", "TenDuAn", nghiemThu.ProjectID);
            ViewBag.PhaseId = new SelectList(db.Phases, "Id", "PhaseName", nghiemThu.PhaseId);
            ViewBag.UnitId = new SelectList(db.Units.Where(x => x.PhanLoai == "1"), "Id", "UnitName", nghiemThu.UnitId);
            return PartialView(nghiemThu);
        }
        // POST: TransportFiles/NghiemThus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProjectID,PhaseId,PhaseValue" +
            ",PhaseLand,PhaseWater,DoanhThu,DaThanhToan,NoConLai,IsActive,CreateUserId,CreateDate" +
            ",ModifiedDate,ModifiedUserId,Note,UnitId")] NghiemThu nghiemThu)
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
				nghiemThu.ModifiedUserId = acc.Id;
				nghiemThu.ModifiedDate = DateTime.Now;			
				db.Entry(nghiemThu).State = EntityState.Modified;
                await db.SaveChangesAsync();
				ViewBag.Message = "Sửa thành công!";
				return RedirectToAction("Index", "Projects", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", "Projects", new { message = ViewBag.Message });
		}

        // GET: TransportFiles/NghiemThus/Delete/5
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
            NghiemThu nghiemThu = await db.NghiemThus.FindAsync(id);
            if (nghiemThu == null)
            {
                return HttpNotFound();
            }
            return PartialView(nghiemThu);
        }

        // POST: TransportFiles/NghiemThus/Delete/5
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
            NghiemThu nghiemThu = await db.NghiemThus.FindAsync(id);
            nghiemThu.IsActive = false; // Đánh dấu là không hoạt động thay vì xóa
            db.Entry(nghiemThu).State = EntityState.Modified;
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("InformationProject", "Projects"
                , new { projectID = nghiemThu.ProjectID, message = ViewBag.Message });
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
