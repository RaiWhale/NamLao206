using System.Linq;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.KhachSan.Controllers
{
    [Authorize]
    public class KhachHangController : Controller
    {
        private readonly namlao206_websiteEntities _db = new namlao206_websiteEntities();

        [HttpGet]
        public ActionResult Index(string q)
        {
            var query = _db.KhachSan_KhachHang.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(k =>
                    k.CCCD.Contains(q) ||
                    k.HoTen.Contains(q));
            }

            var list = query.OrderByDescending(k => k.NgayTao).Take(200).ToList();
            ViewBag.Q = q;
            ViewBag.Title = "Danh sách khách hàng";
            return View(list);
        }

        [HttpGet]
        public ActionResult ChiTiet(int id)
        {
            var kh = _db.KhachSan_KhachHang.FirstOrDefault(k => k.Id == id);
            if (kh == null) return HttpNotFound();

            var lichSu = _db.KhachSan_DatPhong
                .Where(d => d.KhachHangId == id)
                .OrderByDescending(d => d.ThoiGianCheckIn)
                .ToList();

            ViewBag.LichSu = lichSu;
            ViewBag.Title = string.Format("Khách hàng - {0}", kh.HoTen);
            return View(kh);
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
