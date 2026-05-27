using System;
using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    [Authorize]
    public class BookingController : Controller
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
        public ActionResult Index()
        {
            var donViId = GetDonViId();
            var bookings = _db.KhachSan_DatPhong
                .Where(d => (d.TrangThai == 0 || d.TrangThai == 1) && d.KhachSan_Phong.DonViId == donViId)
                .OrderByDescending(d => d.NgayTao)
                .ToList();

            ViewBag.Title = "Đặt phòng trước";
            return View(bookings);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var donViId = GetDonViId();
            var phongs = _db.KhachSan_Phong
                .Where(p => p.DonViId == donViId && p.IsActive && p.TrangThai == 0)
                .OrderBy(p => p.Tang).ThenBy(p => p.SoPhong)
                .ToList();

            var loaiPhongs = _db.KhachSan_LoaiPhong
                .Where(lp => lp.DonViId == donViId && lp.IsActive)
                .ToList();

            ViewBag.Phongs = phongs;
            ViewBag.LoaiPhongs = loaiPhongs;
            ViewBag.Title = "Đặt phòng trước";
            return View(new CheckInVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CheckInVM model)
        {
            if (!ModelState.IsValid)
            {
                var donViIdErr = GetDonViId();
                ViewBag.Phongs = _db.KhachSan_Phong
                    .Where(p => p.DonViId == donViIdErr && p.IsActive && p.TrangThai == 0)
                    .OrderBy(p => p.SoPhong).ToList();
                ViewBag.LoaiPhongs = _db.KhachSan_LoaiPhong.Where(lp => lp.DonViId == donViIdErr && lp.IsActive).ToList();
                ViewBag.Title = "Đặt phòng trước";
                return View(model);
            }

            var accId = GetAccId();

            var kh = _db.KhachSan_KhachHang.FirstOrDefault(k => k.CCCD == model.CCCD);
            if (kh == null)
            {
                kh = new KhachSan_KhachHang
                {
                    CCCD = model.CCCD,
                    HoTen = model.HoTen,
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email,
                    DiaChi = model.DiaChi,
                    QuocTich = model.QuocTich,
                    GioiTinh = model.GioiTinh,
                    NgaySinh = model.NgaySinh,
                    NgayTao = DateTime.Now
                };
                _db.KhachSan_KhachHang.Add(kh);
            }
            else
            {
                kh.HoTen = model.HoTen;
                kh.NgayCapNhat = DateTime.Now;
            }
            _db.SaveChanges();

            var phong = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == model.PhongId);
            if (phong == null) return HttpNotFound();

            var loaiPhong = _db.KhachSan_LoaiPhong.FirstOrDefault(lp => lp.Id == phong.LoaiPhongId);
            if (loaiPhong == null) return HttpNotFound();

            decimal tienPhong;
            switch (model.LoaiGia)
            {
                case 1:
                    tienPhong = loaiPhong.GiaGioNgan;
                    break;
                case 2:
                    tienPhong = loaiPhong.GiaQuaDem;
                    break;
                case 3:
                    tienPhong = loaiPhong.GiaTheoNgay;
                    break;
                default:
                    tienPhong = loaiPhong.GiaGioNgan;
                    break;
            }

            var prefix = "KS" + DateTime.Now.ToString("yyMM");
            var count = _db.KhachSan_DatPhong.Count(d => d.MaPhieu.StartsWith(prefix)) + 1;
            var maPhieu = prefix + count.ToString("D3");

            var datPhong = new KhachSan_DatPhong
            {
                MaPhieu = maPhieu,
                PhongId = model.PhongId,
                KhachHangId = kh.Id,
                ThoiGianCheckIn = model.ThoiGianCheckOutDuKien ?? DateTime.Now.AddHours(1),
                ThoiGianCheckOutDuKien = model.ThoiGianCheckOutDuKien,
                LoaiGia = model.LoaiGia,
                SoNguoi = model.SoNguoi,
                TienPhong = tienPhong,
                TienPhuThu = 0,
                TienGiamGia = 0,
                TongTien = tienPhong,
                DaThanhToan = 0,
                TrangThai = 0,
                NguonBooking = model.NguonBooking,
                GhiChu = model.GhiChu,
                NgayTao = DateTime.Now
            };
            _db.KhachSan_DatPhong.Add(datPhong);

            var trangThaiCu = phong.TrangThai;
            phong.TrangThai = 1;

            _db.SaveChanges();

            _db.KhachSan_TrangThaiLichSu.Add(new KhachSan_TrangThaiLichSu
            {
                PhongId = phong.Id,
                TrangThaiCu = trangThaiCu,
                TrangThaiMoi = 1,
                NguoiThayDoiId = accId,
                ThoiGian = DateTime.Now,
                GhiChu = string.Format("Đặt phòng trước {0}", maPhieu)
            });
            _db.SaveChanges();

            TempData["Success"] = string.Format("Đặt phòng thành công - {0}", maPhieu);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhan(int id)
        {
            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == id);
            if (datPhong == null) return HttpNotFound();

        datPhong.TrangThai = 1; // Đã XN
        _db.SaveChanges();

        TempData["Success"] = "Đã xác nhận đặt phòng.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Huy(int id, string lyDo)
        {
            var accId = GetAccId();
            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == id);
            if (datPhong == null) return HttpNotFound();

        datPhong.TrangThai = 4; // Hủy
        datPhong.LyDoHuy = lyDo;
        datPhong.NguoiHuyId = accId;
        datPhong.ThoiGianHuy = DateTime.Now;

            var phong = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == datPhong.PhongId);
            if (phong == null) return HttpNotFound();
        var trangThaiCu = phong.TrangThai;
        phong.TrangThai = 0; // Trống lại

        _db.SaveChanges();

            _db.KhachSan_TrangThaiLichSu.Add(new KhachSan_TrangThaiLichSu
        {
            PhongId = phong.Id,
            TrangThaiCu = trangThaiCu,
            TrangThaiMoi = 0,
            NguoiThayDoiId = accId,
            ThoiGian = DateTime.Now,
            GhiChu = string.Format("Hủy booking {0}: {1}", datPhong.MaPhieu, lyDo)
        });
        _db.SaveChanges();

        TempData["Success"] = "Đã hủy đặt phòng.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult XacNhanCheckIn(int id)
        {
            var accId = GetAccId();
            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == id);
            if (datPhong == null) return HttpNotFound();

        datPhong.TrangThai = 2; // Đang ở
        datPhong.NhanVienCheckInId = accId;
        datPhong.ThoiGianCheckIn = DateTime.Now;

            var phong = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == datPhong.PhongId);
            if (phong == null) return HttpNotFound();
        var trangThaiCu = phong.TrangThai;
        phong.TrangThai = 2;

        _db.SaveChanges();

            _db.KhachSan_TrangThaiLichSu.Add(new KhachSan_TrangThaiLichSu
        {
            PhongId = phong.Id,
            TrangThaiCu = trangThaiCu,
            TrangThaiMoi = 2,
            NguoiThayDoiId = accId,
            ThoiGian = DateTime.Now,
            GhiChu = string.Format("Nhận phòng từ booking {0}", datPhong.MaPhieu)
        });
        _db.SaveChanges();

        TempData["Success"] = "Đã nhận phòng thành công.";
            return RedirectToAction("Index", "Dashboard");
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
