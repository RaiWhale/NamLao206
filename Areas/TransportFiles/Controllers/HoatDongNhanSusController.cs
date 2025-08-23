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
    public class HoatDongNhanSusController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/HoatDongNhanSus
        public async Task<ActionResult> Index()
        {
            var hoatDongNhanSus = db.HoatDongNhanSus.Include(h => h.Account).Include(h => h.Account1).Include(h => h.DM_DonVis).Include(h => h.Employee);
            return View(await hoatDongNhanSus.ToListAsync());
        }

        // GET: TransportFiles/HoatDongNhanSus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoatDongNhanSu hoatDongNhanSu = await db.HoatDongNhanSus.FindAsync(id);
            if (hoatDongNhanSu == null)
            {
                return HttpNotFound();
            }
            return View(hoatDongNhanSu);
        }

        // GET: TransportFiles/HoatDongNhanSus/Create
        public ActionResult Create()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // 2. Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            ViewBag.DonViId = new SelectList(db.DM_DonVis.Where(x=>x.Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenDonVi");
            ViewBag.NguoiTruc_Id = new SelectList(db.Employees.Where(x=>x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name");
            return View();
        }

        // POST: TransportFiles/HoatDongNhanSus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DonViId,NguoiTruc_Id,TongNhanSu,CongTac,NghiPhep,DiHoc,LyDoKhac,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,IsActive,Note")] HoatDongNhanSu hoatDongNhanSu)
        {
            if (ModelState.IsValid)
            {
                db.HoatDongNhanSus.Add(hoatDongNhanSu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", hoatDongNhanSu.CreateUserId);
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", hoatDongNhanSu.ModifiedUserId);
            ViewBag.DonViId = new SelectList(db.DM_DonVis, "Id", "TenDonVi", hoatDongNhanSu.DonViId);
            ViewBag.NguoiTruc_Id = new SelectList(db.Employees, "Id", "Name", hoatDongNhanSu.NguoiTruc_Id);
            return View(hoatDongNhanSu);
        }

        // GET: TransportFiles/HoatDongNhanSus/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoatDongNhanSu hoatDongNhanSu = await db.HoatDongNhanSus.FindAsync(id);
            if (hoatDongNhanSu == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", hoatDongNhanSu.CreateUserId);
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", hoatDongNhanSu.ModifiedUserId);
            ViewBag.DonViId = new SelectList(db.DM_DonVis, "Id", "TenDonVi", hoatDongNhanSu.DonViId);
            ViewBag.NguoiTruc_Id = new SelectList(db.Employees, "Id", "Name", hoatDongNhanSu.NguoiTruc_Id);
            return View(hoatDongNhanSu);
        }

        // POST: TransportFiles/HoatDongNhanSus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DonViId,NguoiTruc_Id,TongNhanSu,CongTac,NghiPhep,DiHoc,LyDoKhac,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,IsActive,Note")] HoatDongNhanSu hoatDongNhanSu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoatDongNhanSu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", hoatDongNhanSu.CreateUserId);
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", hoatDongNhanSu.ModifiedUserId);
            ViewBag.DonViId = new SelectList(db.DM_DonVis, "Id", "TenDonVi", hoatDongNhanSu.DonViId);
            ViewBag.NguoiTruc_Id = new SelectList(db.Employees, "Id", "Name", hoatDongNhanSu.NguoiTruc_Id);
            return View(hoatDongNhanSu);
        }

        // GET: TransportFiles/HoatDongNhanSus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoatDongNhanSu hoatDongNhanSu = await db.HoatDongNhanSus.FindAsync(id);
            if (hoatDongNhanSu == null)
            {
                return HttpNotFound();
            }
            return View(hoatDongNhanSu);
        }

        // POST: TransportFiles/HoatDongNhanSus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HoatDongNhanSu hoatDongNhanSu = await db.HoatDongNhanSus.FindAsync(id);
            db.HoatDongNhanSus.Remove(hoatDongNhanSu);
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
