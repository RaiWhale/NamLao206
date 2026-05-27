using System;
using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    [Authorize]
    public class ThanhToanController : Controller
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

            var thanhToans = _db.KhachSan_ThanhToan
                .Where(tt => tt.DatPhongId == id)
                .OrderBy(tt => tt.NgayThu)
                .ToList();

            ViewBag.DatPhong = datPhong;
            ViewBag.ConLai = datPhong.TongTien - datPhong.DaThanhToan;
            ViewBag.Title = string.Format("Thanh toán - {0}", datPhong.MaPhieu);
            return View(thanhToans);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemThanhToan(int datPhongId, decimal soTien, int hinhThuc, string maGiaoDich, string ghiChu)
        {
            var datPhong = _db.KhachSan_DatPhong.FirstOrDefault(d => d.Id == datPhongId);
            if (datPhong == null) return HttpNotFound();

            var accId = GetAccId();

            var tt = new KhachSan_ThanhToan
            {
                DatPhongId = datPhongId,
                SoTien = soTien,
                HinhThuc = hinhThuc,
                LoaiThu = 1,
                MaGiaoDich = maGiaoDich,
                NguoiThuId = accId,
                NgayThu = DateTime.Now,
                GhiChu = ghiChu
            };
            _db.KhachSan_ThanhToan.Add(tt);

            datPhong.DaThanhToan += soTien;
            _db.SaveChanges();

            TempData["Success"] = "Đã ghi nhận thanh toán.";
            return RedirectToAction("Index", new { id = datPhongId });
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
