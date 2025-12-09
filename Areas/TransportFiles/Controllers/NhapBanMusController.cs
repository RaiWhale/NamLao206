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
    public class NhapBanMusController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/NhapBanMus
        public async Task<ActionResult> Index(string search, string message)
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
            // 3. Lấy danh sách dự án theo đơn vị
            IQueryable<NhapBanMu> nhapBanMus = db.NhapBanMus.AsNoTracking()
                .Include(p => p.Account)              
                .Include(p => p.DM_DonVis)
                .Include(p => p.Supplier)
                .Include(p => p.StatusProject)
                .Where(x => x.DonVi_Id == acc.Employee.DM_PhongBans.donvi_Id && x.IsActive == true);
            // 4. Xử lý tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                nhapBanMus = nhapBanMus
                    .Where(x => x.TenPhieu.ToLower().Contains(search) || x.MaPhieu.ToLower().Contains(search) || x.Note.ToLower().Contains(search));
            }
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }

            ViewBag.Title = "Dự án - ";
            ViewBag.DonVi = acc.Employee.DM_PhongBans.DM_DonVis;
            return View(nhapBanMus.ToList());
        }

        // GET: TransportFiles/NhapBanMus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhapBanMu nhapBanMu = await db.NhapBanMus.FindAsync(id);
            if (nhapBanMu == null)
            {
                return HttpNotFound();
            }
            return View(nhapBanMu);
        }

        // GET: TransportFiles/NhapBanMus/Create
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

            PopulateDropdowns(acc);
            return PartialView();
        }

        // POST: TransportFiles/NhapBanMus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TenPhieu,MaPhieu,Team_Id,LoaiMu,NgayNhap" +
            ",NguoiPheDuyet_EMP_Id,TroLyKeHoach_EMP_Id,KhoiLuongThuVao,KhoiLuongTTTC,KhoiLuongThuVaoLast,KhoiLuongTTTCLast" +
            ",LoaiHs,LoaiTK,DonGia,Note,SoDienThoai,KeToan_EMP_Id,KhoiLuongTTL,DanhGiaCLMu,DoiTac_Id,DonViTienTe_Id,NguoiCan_EMP_Id,TinhTrang")] NhapBanMu nhapBanMu)
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
                var maPhieu = db.NhapBanMus
                               .Where(x => x.MaPhieu == nhapBanMu.MaPhieu && x.DonVi_Id == acc.Employee.DM_PhongBans.donvi_Id && x.IsActive == true)
                               .FirstOrDefault();
                if (maPhieu != null)
                {
                    ViewBag.Message = "Mã dự án đã tồn tại trong đơn vị này!";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                nhapBanMu.NguoiNhap_ACC_Id = acc.Id;
                nhapBanMu.DonVi_Id = acc.Employee.DM_PhongBans.donvi_Id;
                db.NhapBanMus.Add(nhapBanMu);                                     
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: TransportFiles/NhapBanMus/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhapBanMu nhapBanMu = await db.NhapBanMus.FindAsync(id);
            if (nhapBanMu == null)
            {
                return HttpNotFound();
            }
            ViewBag.KeToan_EMP_Id = new SelectList(db.Employees.Where(x => x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name", nhapBanMu.KeToan_EMP_Id);
            ViewBag.NguoiPheDuyet_EMP_Id = new SelectList(db.Employees.Where(x => x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name", nhapBanMu.NguoiPheDuyet_EMP_Id);
            ViewBag.TroLyKeHoach_EMP_Id = new SelectList(db.Employees.Where(x => x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name", nhapBanMu.TroLyKeHoach_EMP_Id);
            ViewBag.DanhGiaCLMu = new SelectList(db.StatusProjects.Where(x => x.PhanLoai == "2"), "Id", "StatusName", nhapBanMu.DanhGiaCLMu);
            ViewBag.DonViTienTe_Id = new SelectList(db.Units.Where(x => x.PhanLoai == "3"), "Id", "UnitName", nhapBanMu.DonViTienTe_Id);
            ViewBag.LoaiTK = new SelectList(db.Units.Where(x => x.PhanLoai == "2"), "Id", "UnitName", nhapBanMu.LoaiTK);        
            ViewBag.DoiTac_Id = new SelectList(db.Suppliers.Where(x => x.DonviId == acc.Employee.DM_PhongBans.donvi_Id), "Id", "SupplierName", nhapBanMu.DoiTac_Id);
            ViewBag.Team_Id = new SelectList(db.Teams.Where(x => x.DonviId == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TeamName", nhapBanMu.Team_Id);
            return PartialView(nhapBanMu);
        }

        // POST: TransportFiles/NhapBanMus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenPhieu,Team_Id,LoaiMu" +
            ",NgayNhap,NguoiPheDuyet_EMP_Id,TroLyKeHoach_EMP_Id,KhoiLuongThuVao,KhoiLuongTTTC,KhoiLuongThuVaoLast,KhoiLuongTTTCLast" +
            ",LoaiHs,LoaiTK,DonGia,Note,SoDienThoai,KeToan_EMP_Id,KhoiLuongTTL,DanhGiaCLMu,DoiTac_Id,DonViTienTe_Id,NguoiCan_EMP_Id,TinhTrang")] NhapBanMu nhapBanMu)
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
                var maPhieu = db.NhapBanMus
                            .Where(x => x.MaPhieu == nhapBanMu.MaPhieu && x.DonVi_Id == acc.Employee.DM_PhongBans.donvi_Id && x.IsActive == true)
                            .FirstOrDefault();
                if (maPhieu != null)
                {
                    ViewBag.Message = "Mã dự án đã tồn tại trong đơn vị này!";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                nhapBanMu.ModifiedAccount_Id = acc.Id;
                nhapBanMu.ModifiedDate = DateTime.Now;
                db.Entry(nhapBanMu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });                       
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: TransportFiles/NhapBanMus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhapBanMu nhapBanMu = await db.NhapBanMus.FindAsync(id);
            if (nhapBanMu == null)
            {
                return HttpNotFound();
            }
            return View(nhapBanMu);
        }

        // POST: TransportFiles/NhapBanMus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NhapBanMu nhapBanMu = await db.NhapBanMus.FindAsync(id);
            db.NhapBanMus.Remove(nhapBanMu);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private void PopulateDropdowns(Account acc)
        {          
            ViewBag.LoaiHs = new SelectList(db.DocumentTypes.Where(x => x.PhanLoai == "2"), "Id", "DocumentTypeName");
            ViewBag.KeToan_EMP_Id = new SelectList(db.Employees.Where(x => x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name");
            ViewBag.NguoiPheDuyet_EMP_Id = new SelectList(db.Employees.Where(x => x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name");
            ViewBag.TroLyKeHoach_EMP_Id = new SelectList(db.Employees.Where(x => x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name");
            ViewBag.DanhGiaCLMu = new SelectList(db.StatusProjects.Where(x => x.PhanLoai == "2"), "Id", "StatusName");
            ViewBag.DonViTienTe_Id = new SelectList(db.Units.Where(x => x.PhanLoai == "3"), "Id", "UnitName");
            ViewBag.LoaiTK = new SelectList(db.Units.Where(x => x.PhanLoai == "2"), "Id", "UnitName");
            ViewBag.DoiTac_Id = new SelectList(db.Suppliers.Where(x => x.DonviId == acc.Employee.DM_PhongBans.donvi_Id), "Id", "SupplierName");
            ViewBag.Team_Id = new SelectList(db.Teams.Where(x => x.DonviId == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TeamName");
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
