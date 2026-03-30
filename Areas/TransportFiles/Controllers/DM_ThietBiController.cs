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
    public class DM_ThietBiController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/DM_ThietBi
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
            IQueryable<DM_ThietBi> dM_ThietBis = db.DM_ThietBi.AsNoTracking()
                .Where(x => x.DonVi_Id == acc.Employee.DM_PhongBans.donvi_Id && x.IsActive == true);
            // 4. Xử lý tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                dM_ThietBis = dM_ThietBis
                    .Where(x => x.DM_LoaiThietBi.Ten_Loai.ToLower().Contains(search) || x.MaSoRieng.ToLower().Contains(search) || x.SoMay.ToLower().Contains(search));
            }
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            return View(dM_ThietBis.ToList());
        }

        // GET: TransportFiles/DM_ThietBi/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_ThietBi dM_ThietBi = await db.DM_ThietBi.FindAsync(id);
            if (dM_ThietBi == null)
            {
                return HttpNotFound();
            }
            return View(dM_ThietBi);
        }

        // GET: TransportFiles/DM_ThietBi/Create
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

            ViewBag.NguoiSuDung_Id = new SelectList(db.Accounts, "Id", "LoginName");
            ViewBag.LoaiThietBi_id = new SelectList(db.DM_LoaiThietBi, "Loaithietbi_Id", "Ten_Loai");
            ViewBag.Phongban_Id = new SelectList(db.DM_PhongBans.Where(x => x.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenKhoa");
            ViewBag.TinhTrang_Id = new SelectList(db.StatusProjects.Where(x => x.PhanLoai == "3"), "Id", "StatusName");
            return PartialView();
        }

        // POST: TransportFiles/DM_ThietBi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DonVi_Id,NgayNhap,LoaiThietBi_id,BienSo,NhanHieu,SoKhung,SoMay,NamSanXuat" +
            ",XuatXu,NguoiSuDung_Id,TinhTrangKyThuat,HoSoPhapLy_Id,GhiChu,CreateDate,CreateUser_Id,IsActive" +
            ",ThongSoKyThuat,HinhAnh,NgayCapNhat,NguoiCapNhat_Id,TinhTrang_Id")] DM_ThietBi dM_ThietBi)
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
                var maLoaiThietBi = db.DM_LoaiThietBi.Where(x => x.Loaithietbi_Id == dM_ThietBi.LoaiThietBi_id).SingleOrDefault();
                dM_ThietBi.MaSoRieng = maLoaiThietBi.Ma_Loai + "-" + acc.Id + "-" + acc.EmployeeId;
                dM_ThietBi.DonVi_Id = acc.Employee.DM_PhongBans.donvi_Id.Value;
                dM_ThietBi.CreateDate = DateTime.Now;
                dM_ThietBi.CreateUser_Id = acc.Id;
                dM_ThietBi.IsActive = true;

                db.DM_ThietBi.Add(dM_ThietBi);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: TransportFiles/DM_ThietBi/Edit/5
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
            DM_ThietBi dM_ThietBi = await db.DM_ThietBi.FindAsync(id);
            if (dM_ThietBi == null)
            {
                return HttpNotFound();
            }
         
            ViewBag.NguoiSuDung_Id = new SelectList(db.Accounts, "Id", "LoginName", dM_ThietBi.NguoiSuDung_Id);
            ViewBag.LoaiThietBi_id = new SelectList(db.DM_LoaiThietBi, "Loaithietbi_Id", "Ten_Loai", dM_ThietBi.LoaiThietBi_id);
            ViewBag.Phongban_Id = new SelectList(db.DM_PhongBans.Where(x => x.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenKhoa", dM_ThietBi.PhongBan_Id);
            ViewBag.TinhTrang_Id = new SelectList(db.StatusProjects.Where(x => x.PhanLoai == "3"), "Id", "StatusName", dM_ThietBi.TinhTrang_Id);
            return PartialView(dM_ThietBi);
        }

        // POST: TransportFiles/DM_ThietBi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DonVi_Id,NgayNhap,LoaiThietBi_id,BienSo,NhanHieu,SoKhung,SoMay,NamSanXuat,XuatXu,NguoiSuDung_Id,TinhTrangKyThuat,HoSoPhapLy_Id,GhiChu,CreateDate,CreateUser_Id,IsActive,ThongSoKyThuat,HinhAnh,NgayCapNhat,NguoiCapNhat_Id")] DM_ThietBi dM_ThietBi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_ThietBi).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.NguoiCapNhat_Id = new SelectList(db.Accounts, "Id", "LoginName", dM_ThietBi.NguoiCapNhat_Id);
            ViewBag.CreateUser_Id = new SelectList(db.Accounts, "Id", "LoginName", dM_ThietBi.CreateUser_Id);
            ViewBag.NguoiSuDung_Id = new SelectList(db.Accounts, "Id", "LoginName", dM_ThietBi.NguoiSuDung_Id);
            ViewBag.DonVi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", dM_ThietBi.DonVi_Id);
            ViewBag.LoaiThietBi_id = new SelectList(db.DM_LoaiThietBi, "Loaithietbi_Id", "Ma_Loai", dM_ThietBi.LoaiThietBi_id);
            ViewBag.HoSoPhapLy_Id = new SelectList(db.HoSoPhapLys, "Id", "STT", dM_ThietBi.HoSoPhapLy_Id);
            return View(dM_ThietBi);
        }

        // GET: TransportFiles/DM_ThietBi/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_ThietBi dM_ThietBi = await db.DM_ThietBi.FindAsync(id);
            if (dM_ThietBi == null)
            {
                return HttpNotFound();
            }
            return View(dM_ThietBi);
        }

        // POST: TransportFiles/DM_ThietBi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DM_ThietBi dM_ThietBi = await db.DM_ThietBi.FindAsync(id);
            db.DM_ThietBi.Remove(dM_ThietBi);
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
