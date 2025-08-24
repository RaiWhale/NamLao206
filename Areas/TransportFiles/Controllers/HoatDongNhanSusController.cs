using NamLao206.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Areas.TransportFiles.Controllers
{
    public class HoatDongNhanSusController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        int pageSize = 10;
        // GET: TransportFiles/HoatDongNhanSus
        public  ActionResult Index(int? page, string message, string search)
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
            IQueryable<HoatDongNhanSu> hoatDongNhanSusQuery = db.HoatDongNhanSus
                .Include(h => h.Account)
                .Include(h => h.Account1)
                .Include(h => h.DM_DonVis)
                .Include(h => h.Employee)
                .Where(x => x.IsActive == true && x.DonViId == acc.Employee.DM_PhongBans.donvi_Id).OrderByDescending(x => x.CreateDate); ;
            //if (!string.IsNullOrEmpty(search))
            //{
            //    search = search.Trim().ToLower();
            //    var keywords = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //    hoatDongNhanSusQuery = hoatDongNhanSusQuery.Where(s =>
            //        keywords.All(k =>
            //            s.CreateDate.Trim().ToLower().Contains(k)
            //        )
            //    ).OrderByDescending(x => x.Name);
            //}

            int pageNumber = page ?? 1;
            ViewBag.Title = "Báo cáo hàng ngày";
            ViewBag.search = search;
            ViewBag.Message = message;
            return View(hoatDongNhanSusQuery.ToPagedList(pageNumber, pageSize));
        }

        // GET: TransportFiles/HoatDongNhanSus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoatDongNhanSu hoatDongNhanSu = await db.HoatDongNhanSus.FindAsync(id);
            if (hoatDongNhanSu == null)
            {
                return HttpNotFound();
            }
            return View(hoatDongNhanSu);
        }

        // GET: TransportFiles/HoatDongNhanSus/Create
        public ActionResult Create()
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

            ViewBag.DonViId = new SelectList(db.DM_DonVis.Where(x=>x.Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenDonVi");
            ViewBag.NguoiTruc_Id = new SelectList(db.Employees.Where(x=>x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name");
            return PartialView();
        }

        // POST: TransportFiles/HoatDongNhanSus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DonViId,NguoiTruc_Id,TongNhanSu,CongTac,NghiPhep,DiHoc,LyDoKhac,Note")] HoatDongNhanSu hoatDongNhanSu)
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
            if (ModelState.IsValid)
            {
                try
                {               
                        hoatDongNhanSu.CreateUserId = userId;
                        hoatDongNhanSu.CreateDate = DateTime.Now;
                        hoatDongNhanSu.IsActive = true;
                        db.HoatDongNhanSus.Add(hoatDongNhanSu);
                        await db.SaveChangesAsync();
                        // Thiết lập thông báo thành công
                        ViewBag.Message = "Tạo thành công";
                        return RedirectToAction("Index", new { message = ViewBag.Message });                               
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần
                    ViewBag.Message = "Lỗi khi tạo hoạt động nhân sự: " + ex.Message;
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });      
        }

        // GET: TransportFiles/HoatDongNhanSus/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoatDongNhanSu hoatDongNhanSu = await db.HoatDongNhanSus.FindAsync(id);
            if (hoatDongNhanSu == null)
            {
                return HttpNotFound();
            }
            ViewBag.DonViId = new SelectList(db.DM_DonVis.Where(x => x.Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenDonVi");
            ViewBag.NguoiTruc_Id = new SelectList(db.Employees.Where(x => x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "Name");
            return PartialView(hoatDongNhanSu);
        }

        // POST: TransportFiles/HoatDongNhanSus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DonViId,NguoiTruc_Id,TongNhanSu,CongTac,NghiPhep,DiHoc,LyDoKhac,Note")] HoatDongNhanSu hoatDongNhanSu)
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
            if (ModelState.IsValid)
            {
                try
                {
                    hoatDongNhanSu.ModifiedUserId = userId;
                    hoatDongNhanSu.ModifiedDate = DateTime.Now;

                    db.Entry(hoatDongNhanSu).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.Message = "Sửa thành công";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần
                    ViewBag.Message = "Lỗi khi cập nhật hoạt động nhân sự: " + ex.Message;
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }

            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: TransportFiles/HoatDongNhanSus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoatDongNhanSu hoatDongNhanSu = await db.HoatDongNhanSus.FindAsync(id);
            if (hoatDongNhanSu == null)
            {
                return HttpNotFound();
            }
            return PartialView(hoatDongNhanSu);
        }

        // POST: TransportFiles/HoatDongNhanSus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HoatDongNhanSu hoatDongNhanSu = await db.HoatDongNhanSus.FindAsync(id);
            hoatDongNhanSu.IsActive = false;
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
