using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NamLao206.Models;


namespace NamLao206.Areas.TransportFiles.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        public static string tuNgay = DateTime.Now.ToString("dd/MM/yyyy");
        public static string denNgay = DateTime.Now.ToString("dd/MM/yyyy");
        public static DateTime currentDay = DateTime.Now;
        public static string currentDayString = DateTime.Now.ToString("yyyy-MM-dd");
        public ActionResult StartView()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // 2. Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
        
            return View();
        }
        // GET: TransportFiles/Dashboard
        public ActionResult Index()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // 2. Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // 3. Lấy danh sách dự án theo đơn vị
            ViewBag.Projects = db.Projects.AsNoTracking()
                .Include(p => p.Account)
                .Include(p => p.Account1)
                .Include(p => p.DM_DonVis)
                .Include(p => p.Supplier)
                .Include(p => p.StatusProject)
                .Where(x => x.DonViId == acc.Employee.DM_PhongBans.donvi_Id && x.IsActive == true);


            ViewBag.Title = "Dự án - ";
            ViewBag.DonVi = acc.Employee.DM_PhongBans.DM_DonVis;
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetTopInformation()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                return Json(new { success = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            }
            // 2. Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin tài khoản." }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                // Gọi stored procedure bất đồng bộ
                var rs = await Task.Run(() => db.sp_GetDataForDashBoard("GetTopInformation", currentDayString, currentDayString, acc.Employee.DM_PhongBans.donvi_Id).ToList());

                // Kiểm tra kết quả
                if (rs == null || !rs.Any())
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu cho ngày hiện tại." }, JsonRequestBehavior.AllowGet);
                }
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu có hệ thống logging)
                // _logger.LogError(ex, "Lỗi khi lấy thông tin dashboard.");

                return Json(new { success = false, message = "Đã xảy ra lỗi khi lấy dữ liệu. Vui lòng thử lại sau." + ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> TangTruongLoiNhuan()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                return Json(new { success = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            }
            // 2. Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin tài khoản." }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                // Gọi stored procedure bất đồng bộ
                var rs = await Task.Run(() => db.sp_GetDataForDashBoard("TangTruongLoiNhuan", currentDayString, currentDayString, acc.Employee.DM_PhongBans.donvi_Id).ToList());

                // Kiểm tra kết quả
                if (rs == null || !rs.Any())
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu cho ngày hiện tại." }, JsonRequestBehavior.AllowGet);
                }
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu có hệ thống logging)
                // _logger.LogError(ex, "Lỗi khi lấy thông tin dashboard.");

                return Json(new { success = false, message = "Đã xảy ra lỗi khi lấy dữ liệu. Vui lòng thử lại sau." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetSoLoiNhuan()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                return Json(new { success = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            }
            // 2. Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin tài khoản." }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                // Gọi stored procedure bất đồng bộ
                var rs = await Task.Run(() => db.sp_GetDataForDashBoard("GetSoLoiNhuan", currentDayString, currentDayString, acc.Employee.DM_PhongBans.donvi_Id).ToList());

                // Kiểm tra kết quả
                if (rs == null || !rs.Any())
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu cho ngày hiện tại." }, JsonRequestBehavior.AllowGet);
                }
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu có hệ thống logging)
                // _logger.LogError(ex, "Lỗi khi lấy thông tin dashboard.");

                return Json(new { success = false, message = "Đã xảy ra lỗi khi lấy dữ liệu. Vui lòng thử lại sau." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> HoatDongNhanSu()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                return Json(new { success = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại." }, JsonRequestBehavior.AllowGet);
            }
            // 2. Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin tài khoản." }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                // Gọi stored procedure bất đồng bộ
                var rs = await Task.Run(() => db.sp_GetDataForDashBoard("NhanSu", currentDayString, currentDayString, acc.Employee.DM_PhongBans.donvi_Id).ToList());

                // Kiểm tra kết quả
                if (rs == null || !rs.Any())
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu cho ngày hiện tại." }, JsonRequestBehavior.AllowGet);
                }
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nếu có hệ thống logging)
                // _logger.LogError(ex, "Lỗi khi lấy thông tin dashboard.");

                return Json(new { success = false, message = "Đã xảy ra lỗi khi lấy dữ liệu. Vui lòng thử lại sau." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}