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
    public class ThietBi_ChiTietController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/ThietBi_ChiTiet
        public ActionResult Index(int? page, string search, string message)
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
            IQueryable<ThietBi_ChiTiet> thietBi_ChiTiets = db.ThietBi_ChiTiet.AsNoTracking()
                .Where(x => x.Donvi_Id == acc.Employee.DM_PhongBans.donvi_Id && x.IsActive == true);

            return View(thietBi_ChiTiets.ToList());
        }

        // GET: TransportFiles/ThietBi_ChiTiet/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBi_ChiTiet thietBi_ChiTiet = await db.ThietBi_ChiTiet.FindAsync(id);
            if (thietBi_ChiTiet == null)
            {
                return HttpNotFound();
            }
            return View(thietBi_ChiTiet);
        }

        // GET: TransportFiles/ThietBi_ChiTiet/Create
        public ActionResult Create()
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
 
            ViewBag.Phongban_Id = new SelectList(db.DM_PhongBans.Where(x=>x.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenKhoa");
            ViewBag.ThietBi_Id = new SelectList(db.DM_ThietBi, "Id", "BienSo");
            ViewBag.TinhTrang = new SelectList(db.StatusProjects, "Id", "StatusName");
            return PartialView();
        }

        // POST: TransportFiles/ThietBi_ChiTiet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ThietBi_ChiTiet_Id,ThietBi_Id,Donvi_Id,Phongban_Id,Ma_So_Rieng,TinhTrang" +
            ",SoLuong,ngay_mua,ngay_bat_dau_su_dung,ngay_bao_tri_cuoi,ngay_bao_tri_tiep_theo,ghi_chu" +
            ",IsActive,ngay_tao,ngay_cap_nhat,NguoiTao_Id,NguoiCapNhat_Id")] ThietBi_ChiTiet thietBi_ChiTiet)
        {
            if (ModelState.IsValid)
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
                thietBi_ChiTiet.Donvi_Id = acc.Employee.DM_PhongBans.donvi_Id;
                thietBi_ChiTiet.ngay_tao = DateTime.Now;
                thietBi_ChiTiet.IsActive = true;

                db.ThietBi_ChiTiet.Add(thietBi_ChiTiet);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Phongban_Id = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", thietBi_ChiTiet.Phongban_Id);
            ViewBag.ThietBi_Id = new SelectList(db.DM_ThietBi, "Id", "BienSo", thietBi_ChiTiet.ThietBi_Id);
            ViewBag.TinhTrang = new SelectList(db.StatusProjects, "Id", "StatusName", thietBi_ChiTiet.TinhTrang);
            return View(thietBi_ChiTiet);
        }

        // GET: TransportFiles/ThietBi_ChiTiet/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            ThietBi_ChiTiet thietBi_ChiTiet = await db.ThietBi_ChiTiet.FindAsync(id);
            if (thietBi_ChiTiet == null)
            {
                return HttpNotFound();
            }
            ViewBag.NguoiCapNhat_Id = new SelectList(db.Accounts, "Id", "LoginName", thietBi_ChiTiet.NguoiCapNhat_Id);
            ViewBag.NguoiTao_Id = new SelectList(db.Accounts, "Id", "LoginName", thietBi_ChiTiet.NguoiTao_Id);
            ViewBag.Donvi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", thietBi_ChiTiet.Donvi_Id);
            ViewBag.Phongban_Id = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", thietBi_ChiTiet.Phongban_Id);
            ViewBag.ThietBi_Id = new SelectList(db.DM_ThietBi, "Id", "BienSo", thietBi_ChiTiet.ThietBi_Id);
            ViewBag.TinhTrang = new SelectList(db.StatusProjects, "Id", "StatusName", thietBi_ChiTiet.TinhTrang);
            return PartialView(thietBi_ChiTiet);
        }

        // POST: TransportFiles/ThietBi_ChiTiet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ThietBi_ChiTiet_Id,ThietBi_Id,Donvi_Id,Phongban_Id,Ma_So_Rieng,TinhTrang,SoLuong,ngay_mua,ngay_bat_dau_su_dung,ngay_bao_tri_cuoi,ngay_bao_tri_tiep_theo,ghi_chu,IsActive,ngay_tao,ngay_cap_nhat,NguoiTao_Id,NguoiCapNhat_Id")] ThietBi_ChiTiet thietBi_ChiTiet)
        {
            if (ModelState.IsValid)
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
                db.Entry(thietBi_ChiTiet).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Phongban_Id = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", thietBi_ChiTiet.Phongban_Id);
            ViewBag.ThietBi_Id = new SelectList(db.DM_ThietBi, "Id", "BienSo", thietBi_ChiTiet.ThietBi_Id);
            ViewBag.TinhTrang = new SelectList(db.StatusProjects, "Id", "StatusName", thietBi_ChiTiet.TinhTrang);
            return View(thietBi_ChiTiet);
        }

        // GET: TransportFiles/ThietBi_ChiTiet/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            ThietBi_ChiTiet thietBi_ChiTiet = await db.ThietBi_ChiTiet.FindAsync(id);
            if (thietBi_ChiTiet == null)
            {
                return HttpNotFound();
            }
            return PartialView(thietBi_ChiTiet);
        }

        // POST: TransportFiles/ThietBi_ChiTiet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
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
            ThietBi_ChiTiet thietBi_ChiTiet = await db.ThietBi_ChiTiet.FindAsync(id);
            db.ThietBi_ChiTiet.Remove(thietBi_ChiTiet);
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
