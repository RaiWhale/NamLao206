using System;
using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    [Authorize]
    public class BaoCaoController : Controller
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

        [HttpGet]
        public ActionResult Index(DateTime? from, DateTime? to)
        {
            var donViId = GetDonViId();
            var fromDate = from ?? DateTime.Today.AddDays(-30);
            var toDate = (to ?? DateTime.Today).AddDays(1).AddSeconds(-1);

            var phongIds = _db.KhachSan_Phong
                .Where(p => p.DonViId == donViId)
                .Select(p => p.Id)
                .ToList();

            var rows = _db.KhachSan_DatPhong
                .Where(d =>
                    phongIds.Contains(d.PhongId) &&
                    d.TrangThai == 3 &&
                    d.ThoiGianCheckOutThucTe.HasValue &&
                    d.ThoiGianCheckOutThucTe >= fromDate &&
                    d.ThoiGianCheckOutThucTe <= toDate)
                .ToList()
                .GroupBy(d => d.ThoiGianCheckOutThucTe.Value.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    SoPhieu = g.Count(),
                    DoanhThu = g.Sum(d => d.TongTien)
                })
                .OrderBy(r => r.Ngay)
                .ToList();

            ViewBag.From = fromDate.ToString("yyyy-MM-dd");
            ViewBag.To = to.HasValue ? to.Value.ToString("yyyy-MM-dd") : DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.TotalRevenue = rows.Sum(r => r.DoanhThu);
            ViewBag.TotalBookings = rows.Sum(r => r.SoPhieu);
            ViewBag.AvgRevPerRoom = rows.Count > 0
                ? rows.Sum(r => r.DoanhThu) / rows.Sum(r => r.SoPhieu)
                : 0m;
            ViewBag.Title = "Báo cáo doanh thu";
            return View(rows);
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
