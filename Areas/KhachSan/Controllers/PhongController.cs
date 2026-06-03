using System;
using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    [Authorize]
    public class PhongController : Controller
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

        public ActionResult Index()
        {
            int donViId = GetDonViId();
            var list = _db.KhachSan_Phong
                .Where(p => p.DonViId == donViId && p.IsActive)
                .OrderBy(p => p.Tang).ThenBy(p => p.SoPhong)
                .ToList();
            ViewBag.Title = "Quản lý phòng";
            return View(list);
        }

        public ActionResult Create()
        {
            int donViId = GetDonViId();
            ViewBag.LoaiPhongs = new SelectList(
                _db.KhachSan_LoaiPhong.Where(l => l.DonViId == donViId && l.IsActive).OrderBy(l => l.TenLoai),
                "Id", "TenLoai");
            ViewBag.Title = "Thêm phòng";
            return PartialView(new KhachSan_Phong());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(KhachSan_Phong model)
        {
            int donViId = GetDonViId();
            if (donViId == 0) return new HttpUnauthorizedResult();

            if (_db.KhachSan_Phong.Any(p => p.DonViId == donViId && p.SoPhong == model.SoPhong && p.IsActive))
                ModelState.AddModelError("SoPhong", "Số phòng đã tồn tại.");

            if (!ModelState.IsValid)
            {
                ViewBag.LoaiPhongs = new SelectList(
                    _db.KhachSan_LoaiPhong.Where(l => l.DonViId == donViId && l.IsActive).OrderBy(l => l.TenLoai),
                    "Id", "TenLoai", model.LoaiPhongId);
                return PartialView(model);
            }

            model.DonViId = donViId;
            model.IsActive = true;
            model.TrangThai = 0;
            _db.KhachSan_Phong.Add(model);
            _db.SaveChanges();
            TempData["Success"] = "Đã thêm phòng thành công.";
            if (Request.IsAjaxRequest()) return Content("success");
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            int donViId = GetDonViId();
            var item = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == id && p.DonViId == donViId);
            if (item == null) return HttpNotFound();
            ViewBag.LoaiPhongs = new SelectList(
                _db.KhachSan_LoaiPhong.Where(l => l.DonViId == donViId && l.IsActive).OrderBy(l => l.TenLoai),
                "Id", "TenLoai", item.LoaiPhongId);
            ViewBag.Title = "Chỉnh sửa phòng";
            return PartialView(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, KhachSan_Phong model)
        {
            int donViId = GetDonViId();
            var item = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == id && p.DonViId == donViId);
            if (item == null) return HttpNotFound();

            if (_db.KhachSan_Phong.Any(p => p.DonViId == donViId && p.SoPhong == model.SoPhong && p.IsActive && p.Id != id))
                ModelState.AddModelError("SoPhong", "Số phòng đã tồn tại.");

            if (!ModelState.IsValid)
            {
                ViewBag.LoaiPhongs = new SelectList(
                    _db.KhachSan_LoaiPhong.Where(l => l.DonViId == donViId && l.IsActive).OrderBy(l => l.TenLoai),
                    "Id", "TenLoai", model.LoaiPhongId);
                return PartialView(model);
            }

            item.SoPhong = model.SoPhong;
            item.TenPhong = model.TenPhong;
            item.LoaiPhongId = model.LoaiPhongId;
            item.Tang = model.Tang;
            item.MoTa = model.MoTa;
            _db.SaveChanges();
            TempData["Success"] = "Đã cập nhật phòng.";
            if (Request.IsAjaxRequest()) return Content("success");
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DoiTrangThai(int id, int trangThai, string ghiChu)
        {
            int donViId = GetDonViId();
            int accId = GetAccId();
            var phong = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == id && p.DonViId == donViId);
            if (phong == null) return HttpNotFound();

            _db.KhachSan_TrangThaiLichSu.Add(new KhachSan_TrangThaiLichSu
            {
                PhongId = id,
                TrangThaiCu = phong.TrangThai,
                TrangThaiMoi = trangThai,
                NguoiThayDoiId = accId,
                ThoiGian = DateTime.Now,
                GhiChu = ghiChu
            });

            phong.TrangThai = trangThai;
            _db.SaveChanges();
            TempData["Success"] = "Đã cập nhật trạng thái phòng.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            int donViId = GetDonViId();
            var item = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == id && p.DonViId == donViId);
            if (item == null) return HttpNotFound();
            return PartialView(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int donViId = GetDonViId();
            var item = _db.KhachSan_Phong.FirstOrDefault(p => p.Id == id && p.DonViId == donViId);
            if (item == null) return HttpNotFound();
            item.IsActive = false;
            _db.SaveChanges();
            TempData["Success"] = "Đã xóa phòng.";
            if (Request.IsAjaxRequest()) return Content("success");
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
