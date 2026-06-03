using System;
using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    [Authorize]
    public class LoaiPhongController : Controller
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

        public ActionResult Index(string message)
        {
            int donViId = GetDonViId();
            var list = _db.KhachSan_LoaiPhong
                .Where(l => l.DonViId == donViId && l.IsActive)
                .OrderBy(l => l.TenLoai)
                .ToList();
            ViewBag.Title = "Loại phòng";
            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Thêm loại phòng";
            return PartialView(new KhachSan_LoaiPhong());
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
        public ActionResult Create(KhachSan_LoaiPhong model)
        {
            int donViId = GetDonViId();
            if (donViId == 0) return new HttpUnauthorizedResult();

            if (_db.KhachSan_LoaiPhong.Any(l => l.DonViId == donViId && l.TenLoai == model.TenLoai && l.IsActive))
                ModelState.AddModelError("TenLoai", "Tên loại phòng đã tồn tại.");

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Thêm loại phòng";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }

            model.DonViId = donViId;
            model.IsActive = true;
            model.NgayTao = DateTime.Now;
            _db.KhachSan_LoaiPhong.Add(model);
            _db.SaveChanges();
            TempData["Success"] = "Đã thêm loại phòng thành công.";
            if (Request.IsAjaxRequest()) return Content("success");
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            int donViId = GetDonViId();
            var item = _db.KhachSan_LoaiPhong.FirstOrDefault(l => l.Id == id && l.DonViId == donViId);
            if (item == null) return HttpNotFound();
            ViewBag.Title = "Chỉnh sửa loại phòng";
            return PartialView(item);
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, KhachSan_LoaiPhong model)
        {
            int donViId = GetDonViId();
            var item = _db.KhachSan_LoaiPhong.FirstOrDefault(l => l.Id == id && l.DonViId == donViId);
            if (item == null) return HttpNotFound();

            if (_db.KhachSan_LoaiPhong.Any(l => l.DonViId == donViId && l.TenLoai == model.TenLoai && l.IsActive && l.Id != id))
                ModelState.AddModelError("TenLoai", "Tên loại phòng đã tồn tại.");

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Chỉnh sửa loại phòng";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }

            item.TenLoai = model.TenLoai;
            item.GiaGioNgan = model.GiaGioNgan;
            item.GiaQuaDem = model.GiaQuaDem;
            item.GiaTheoNgay = model.GiaTheoNgay;
            item.SoNguoiToiDa = model.SoNguoiToiDa;
            item.MoTa = model.MoTa;
            _db.SaveChanges();
            TempData["Success"] = "Đã cập nhật loại phòng thành công.";
            if (Request.IsAjaxRequest()) return Content("success");
            return RedirectToAction("Index");
        }

    [HttpGet]
    public ActionResult Delete(int id)
    {
        int donViId = GetDonViId();
        var item = _db.KhachSan_LoaiPhong.FirstOrDefault(l => l.Id == id && l.DonViId == donViId);
        if (item == null) return HttpNotFound();
        return PartialView(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        int donViId = GetDonViId();
        var item = _db.KhachSan_LoaiPhong.FirstOrDefault(l => l.Id == id && l.DonViId == donViId);
        if (item == null) return HttpNotFound();
        item.IsActive = false;
        _db.SaveChanges();
        TempData["Success"] = "Đã xóa loại phòng.";
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
