using System.Linq;
using System.Net;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly namlao206_websiteEntities _db = new namlao206_websiteEntities();

        public ActionResult Index()
        {
            int accId;
            if (!int.TryParse(User.Identity != null ? User.Identity.Name : null, out accId))
                return RedirectToAction("Login", "Login", new { area = "" });

            var acc = _db.Accounts.Select(a => new { a.Id, a.EmployeeId })
                .FirstOrDefault(a => a.Id == accId);
            if (acc == null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var emp = _db.Employees.Select(e => new { e.Id, e.KhoaphongId })
                .FirstOrDefault(e => e.Id == acc.EmployeeId);
            if (emp == null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var pb = _db.DM_PhongBans
                .Where(p => p.Id == emp.KhoaphongId)
                .Select(p => new { p.donvi_Id })
                .FirstOrDefault();
            int donViId = pb != null && pb.donvi_Id.HasValue ? pb.donvi_Id.Value : 0;

            var phongs = _db.KhachSan_Phong
                .Where(p => p.DonViId == donViId && p.IsActive)
                .OrderBy(p => p.Tang)
                .ThenBy(p => p.SoPhong)
                .ToList();

            var phongIds = phongs.Select(p => p.Id).ToList();
            var datPhongs = _db.KhachSan_DatPhong
                .Where(d => phongIds.Contains(d.PhongId) && (d.TrangThai == 1 || d.TrangThai == 2))
                .ToDictionary(d => d.PhongId);

            ViewBag.DatPhongs = datPhongs;
            ViewBag.Title = "Sơ đồ phòng";
            return View(phongs);
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
