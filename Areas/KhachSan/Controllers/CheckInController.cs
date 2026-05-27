using System;
using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    public class CheckInVM
    {
        public string CCCD { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string QuocTich { get; set; }
        public bool? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int PhongId { get; set; }
        public int LoaiGia { get; set; }
        public int SoNguoi { get; set; }
        public DateTime? ThoiGianCheckOutDuKien { get; set; }
        public int NguonBooking { get; set; }
        public string GhiChu { get; set; }

        public CheckInVM()
        {
            SoNguoi = 1;
            NguonBooking = 1;
        }
    }

    [Authorize]
    public class CheckInController : Controller
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
        public ActionResult Index(int? phongId)
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
            ViewBag.PhongIdSelected = phongId;
            ViewBag.Title = "Check-In";
            return View(new CheckInVM());
        }

        [HttpGet]
        public ActionResult TraCuuCCCD(string cccd)
        {
            var kh = _db.KhachSan_KhachHang.FirstOrDefault(k => k.CCCD == cccd);
            if (kh == null) return Json(new { found = false }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                found = true,
                id = kh.Id,
                hoTen = kh.HoTen,
                soDienThoai = kh.SoDienThoai,
                email = kh.Email,
                diaChi = kh.DiaChi,
                quocTich = kh.QuocTich,
                gioiTinh = kh.GioiTinh,
                ngaySinh = kh.NgaySinh.HasValue ? kh.NgaySinh.Value.ToString("yyyy-MM-dd") : null
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(CheckInVM model)
        {
            if (!ModelState.IsValid)
            {
                var donViIdErr = GetDonViId();
                ViewBag.Phongs = _db.KhachSan_Phong
                    .Where(p => p.DonViId == donViIdErr && p.IsActive && p.TrangThai == 0)
                    .OrderBy(p => p.SoPhong)
                    .ToList();
                ViewBag.LoaiPhongs = _db.KhachSan_LoaiPhong.Where(lp => lp.DonViId == donViIdErr && lp.IsActive).ToList();
                ViewBag.Title = "Check-In";
                return View("Index", model);
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
                kh.SoDienThoai = model.SoDienThoai;
                kh.Email = model.Email;
                kh.DiaChi = model.DiaChi;
                kh.QuocTich = model.QuocTich;
                kh.GioiTinh = model.GioiTinh;
                kh.NgaySinh = model.NgaySinh;
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
                NhanVienCheckInId = accId,
                ThoiGianCheckIn = DateTime.Now,
                ThoiGianCheckOutDuKien = model.ThoiGianCheckOutDuKien,
                LoaiGia = model.LoaiGia,
                SoNguoi = model.SoNguoi,
                TienPhong = tienPhong,
                TienPhuThu = 0,
                TienGiamGia = 0,
                TongTien = tienPhong,
                DaThanhToan = 0,
                TrangThai = 2,
                NguonBooking = model.NguonBooking,
                GhiChu = model.GhiChu,
                NgayTao = DateTime.Now
            };
            _db.KhachSan_DatPhong.Add(datPhong);

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
                GhiChu = string.Format("Check-in {0}", maPhieu)
            });
            _db.SaveChanges();

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
