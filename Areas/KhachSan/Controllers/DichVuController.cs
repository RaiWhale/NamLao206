using System;
using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    [Authorize]
    public class DichVuController : Controller
    {
        private readonly namlao206_websiteEntities _db = new namlao206_websiteEntities();

        private int GetAccId()
        {
            int id;
            int.TryParse(User.Identity != null ? User.Identity.Name : null, out id);
            return id;
        }

        [HttpGet]
        public ActionResult Index(int id)
        {
            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == id);
            if (datPhong == null) return HttpNotFound();

            var dichVus = _db.KhachSan_DichVuPhuThu
                .Where(dv => dv.DatPhongId == id)
                .OrderBy(dv => dv.NgayPhatSinh)
                .ToList();

            var accept = Request.Headers["Accept"];
            if (!string.IsNullOrEmpty(accept) && accept.Contains("application/json"))
            {
                return Json(dichVus, JsonRequestBehavior.AllowGet);
            }

            ViewBag.DatPhong = datPhong;
            ViewBag.Title = string.Format("Dịch vụ - Phiếu {0}", datPhong.MaPhieu);
            return View(dichVus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDichVu(int datPhongId, string tenDichVu, int soLuong, decimal donGia, string ghiChu)
        {
            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == datPhongId);
            if (datPhong == null) return HttpNotFound();

            var accId = GetAccId();
            var thanhTien = soLuong * donGia;

            var dv = new KhachSan_DichVuPhuThu
            {
                DatPhongId = datPhongId,
                TenDichVu = tenDichVu,
                SoLuong = soLuong,
                DonGia = donGia,
                ThanhTien = thanhTien,
                NgayPhatSinh = DateTime.Now,
                NhanVienId = accId,
                GhiChu = ghiChu
            };
            _db.KhachSan_DichVuPhuThu.Add(dv);

            datPhong.TienPhuThu += thanhTien;
            datPhong.TongTien = datPhong.TienPhong + datPhong.TienPhuThu - datPhong.TienGiamGia;

            _db.SaveChanges();

            TempData["Success"] = "Đã thêm dịch vụ.";
            return RedirectToAction("Index", new { id = datPhongId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaDichVu(int id)
        {
            var dv = _db.KhachSan_DichVuPhuThu.FirstOrDefault(d => d.Id == id);
            if (dv == null) return HttpNotFound();

            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == dv.DatPhongId);
            if (datPhong == null) return HttpNotFound();

            if (datPhong.TrangThai != 2 && datPhong.TrangThai != 1)
            {
                TempData["Error"] = "Không thể xóa dịch vụ của phiếu đã hoàn tất.";
                return RedirectToAction("Index", new { id = dv.DatPhongId });
            }

            datPhong.TienPhuThu -= dv.ThanhTien;
            datPhong.TongTien = datPhong.TienPhong + datPhong.TienPhuThu - datPhong.TienGiamGia;

            _db.KhachSan_DichVuPhuThu.Remove(dv);
            _db.SaveChanges();

            TempData["Success"] = "Đã xóa dịch vụ.";
            return RedirectToAction("Index", new { id = dv.DatPhongId });
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
