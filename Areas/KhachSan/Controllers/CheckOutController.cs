using System;
using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    public class CheckOutVM
    {
        public int DatPhongId { get; set; }
        public decimal TienGiamGia { get; set; }
        public string GhiChu { get; set; }
        public bool HasKiemPhong { get; set; }
        public decimal PhiMinibar { get; set; }
        public decimal PhiHuHong { get; set; }
        public decimal PhiMatDo { get; set; }
        public decimal PhiVeSinh { get; set; }
        public int KiemPhongTrangThai { get; set; }
        public string KiemPhongGhiChu { get; set; }

        public CheckOutVM()
        {
            KiemPhongTrangThai = 1;
        }
    }

    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly namlao206_websiteEntities _db = new namlao206_websiteEntities();

        private int GetDonViId()
        {
            int accId;
            if (!int.TryParse(User.Identity != null ? User.Identity.Name : null, out accId)) return 0;
            var acc = _db.Accounts.Select(a => new { a.Id, a.EmployeeId }).FirstOrDefault(a => a.Id == accId);
            if (acc == null) return 0;
            var emp = _db.Employees.Select(e => new { e.Id, e.KhoaphongId }).FirstOrDefault(e => e.Id == acc.EmployeeId);
            if (emp == null) return 0;
            var pb = _db.DM_PhongBans.Where(p => p.Id == emp.KhoaphongId).Select(p => new { p.donvi_Id }).FirstOrDefault();
            return pb != null && pb.donvi_Id.HasValue ? pb.donvi_Id.Value : 0;
        }

        private int GetAccId()
        {
            int id;
            int.TryParse(User.Identity != null ? User.Identity.Name : null, out id);
            return id;
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
            var donViId = GetDonViId();

        if (id.HasValue)
        {
            return RedirectToAction("ChiTiet", new { id = id.Value });
        }

            var datPhongs = _db.KhachSan_DatPhong
                .Where(d => d.TrangThai == 2)
                .OrderBy(d => d.ThoiGianCheckIn)
                .ToList();

        // Lọc theo DonVi
            datPhongs = datPhongs.Where(d => d.KhachSan_Phong != null && d.KhachSan_Phong.DonViId == donViId).ToList();

            ViewBag.Title = "Danh sách phòng đang ở";
            return View(datPhongs);
        }

        [HttpGet]
        public ActionResult ChiTiet(int id)
        {
            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == id);
            if (datPhong == null) return HttpNotFound();

            var dichVus = _db.KhachSan_DichVuPhuThu
                .Where(dv => dv.DatPhongId == id)
                .OrderBy(dv => dv.NgayPhatSinh)
                .ToList();

            var thanhToans = _db.KhachSan_ThanhToan
                .Where(tt => tt.DatPhongId == id)
                .OrderBy(tt => tt.NgayThu)
                .ToList();

            ViewBag.DichVus = dichVus;
            ViewBag.ThanhToans = thanhToans;
            ViewBag.Title = string.Format("Chi tiết - {0}", datPhong.MaPhieu);
            return View(datPhong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanCheckOut(CheckOutVM model)
        {
            var accId = GetAccId();

            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == model.DatPhongId);
            if (datPhong == null) return HttpNotFound();

        decimal phiKiemPhong = 0;

        if (model.HasKiemPhong)
        {
            phiKiemPhong = model.PhiMinibar + model.PhiHuHong + model.PhiMatDo + model.PhiVeSinh;
            var kiemPhong = new KhachSan_KiemPhong
            {
                DatPhongId = datPhong.Id,
                PhongId = datPhong.PhongId,
                NhanVienKiemId = accId,
                ThoiGianKiem = DateTime.Now,
                TrangThai = model.KiemPhongTrangThai,
                PhiMinibar = model.PhiMinibar,
                PhiHuHong = model.PhiHuHong,
                PhiMatDo = model.PhiMatDo,
                PhiVeSinh = model.PhiVeSinh,
                GhiChu = model.KiemPhongGhiChu
            };
                _db.KhachSan_KiemPhong.Add(kiemPhong);

            // Cộng phí kiểm phòng vào dịch vụ phụ thu
            datPhong.TienPhuThu += phiKiemPhong;
        }

        datPhong.TienGiamGia = model.TienGiamGia;
        datPhong.TongTien = datPhong.TienPhong + datPhong.TienPhuThu - datPhong.TienGiamGia;
        datPhong.TrangThai = 3; // CheckedOut
        datPhong.ThoiGianCheckOutThucTe = DateTime.Now;
        datPhong.NhanVienCheckOutId = accId;
        if (!string.IsNullOrEmpty(model.GhiChu)) datPhong.GhiChu = model.GhiChu;

            var phong = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == datPhong.PhongId);
            if (phong == null) return HttpNotFound();
        var trangThaiCu = phong.TrangThai;
        phong.TrangThai = 3; // Chờ dọn

        _db.SaveChanges();

            _db.KhachSan_TrangThaiLichSu.Add(new KhachSan_TrangThaiLichSu
        {
            PhongId = phong.Id,
            TrangThaiCu = trangThaiCu,
            TrangThaiMoi = 3,
            NguoiThayDoiId = accId,
            ThoiGian = DateTime.Now,
            GhiChu = string.Format("Check-out {0}", datPhong.MaPhieu)
        });
        _db.SaveChanges();

            TempData["Success"] = string.Format("Check-out thành công - {0}", datPhong.MaPhieu);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
