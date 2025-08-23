using NamLao206.Models;
using NamLao206.Utils;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Areas.TransportFiles.Controllers
{
    public class EmployeesController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        int pageSize = 10;
        // GET: TransportFiles/Employees
        public ActionResult Index(int? page, string message, string search)
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
            IQueryable<Employee> employees = db.Employees
                .Include(e => e.DM_Chucvus)
                .Include(e => e.DM_Donvihanhchinhs)
                .Include(e => e.DM_Hocvis).Include(e => e.DM_Nghenghieps)
                .Include(e => e.DM_PhongBans).Include(e => e.Level)
                .Include(e => e.Gender)
                 .Where(x => x.GroupId == 2
                 && x.Import == true
                 && x.DM_PhongBans.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id)
                 .OrderBy(x => x.Name);
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                var keywords = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                employees = employees.Where(s =>
                    keywords.All(k =>
                        s.Name.Trim().ToLower().Contains(k)
                    )
                ).OrderByDescending(x => x.Name);
            }

            int pageNumber = page ?? 1;
            ViewBag.Title = "Quản lý nhân viên";
            ViewBag.search = search;
            ViewBag.Message = message;
            return View(employees.ToPagedList(pageNumber, pageSize));
        }

        // GET: TransportFiles/Employees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: TransportFiles/Employees/Create
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

            ViewBag.ChucvuId = new SelectList(db.DM_Chucvus, "Id", "Chucvu");          
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans.Where(x => x.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenKhoa");
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "GioiTinh");
            return PartialView();
        }

        // POST: TransportFiles/Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Phone,Address,KhoaphongId,ChucvuId,GenderId,Birthday,Avatar")] Employee employee, HttpPostedFileBase pic)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                var validEmployee = db.Employees
                    .Where(x => x.Phone == employee.Phone
                    || (x.Name.Contains(employee.Name) && x.Birthday == employee.Birthday))
                    .SingleOrDefault();
                if (validEmployee != null)
                {
                    ViewBag.Message = "Nhân viên đã tồn tại với số điện thoại hoặc tên và ngày sinh này.";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                                          
                try
                {
                    // Handle avatar upload
                    if (pic != null && pic.ContentLength > 0)
                    {
                        string filename = $"{DateTime.Now.Ticks}_{pic.FileName.Split('/').Last()}";
                        string path = Server.MapPath("~/Content/Uploads/Avatars");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        pic.SaveAs(Path.Combine(path, filename));
                        employee.Avatar = filename;
                    }
                    else
                    {
                        employee.Avatar = null;
                    }

                    // Thiết lập ngày tạo
                    employee.CreatedDate = DateTime.Now;
                    employee.IsActive = true;
                    employee.GroupId = 2; // Nhóm nhân viên
                    employee.LevelId = 2; // Mặc định là nhân viên
                    employee.Canhan = true;
                    employee.Import = true; // Đánh dấu là đã nhập   
                    employee.IsActive = true; // Đánh dấu là hoạt động
                    // Thêm nhân viên vào database
                    db.Employees.Add(employee);
                    await db.SaveChangesAsync();

                    // Thiết lập thông báo thành công
                    ViewBag.Message = "Tạo nhân viên thành công";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi và thiết lập thông báo lỗi
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo nhân viên: {ex.Message}");
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
                }
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: TransportFiles/Employees/Edit/5
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
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChucvuId = new SelectList(db.DM_Chucvus, "Id", "Chucvu", employee.ChucvuId);         
     
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans
                .Where(x => x.donvi_Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenKhoa", employee.KhoaphongId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "GioiTinh", employee.GenderId);
            return View(employee);
        }

        // POST: TransportFiles/Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Phone,Address,KhoaphongId,ChucvuId,GenderId,Birthday,Avatar")] Employee employee, HttpPostedFileBase pic)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                var validEmployee = db.Employees
                    .Where(x => (x.Phone == employee.Phone || (x.Name.Contains(employee.Name) && x.Birthday == employee.Birthday))
                    && x.Id != employee.Id)
                    .SingleOrDefault();
                if (validEmployee != null)
                {
                    ViewBag.Message = "Nhân viên đã tồn tại với số điện thoại hoặc tên và ngày sinh này.";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                try
                {
                    // Xử lý upload ảnh
                    if (pic != null && pic.ContentLength > 0)
                    {
                        string filename = $"{DateTime.Now.Ticks}_{pic.FileName.Split('/').Last()}";
                        string path = Server.MapPath("~/Content/Uploads/Avatars");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        pic.SaveAs(Path.Combine(path, filename));
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(employee.Avatar))
                        {
                            string oldAvatarPath = Path.Combine(Server.MapPath("~/Content/Uploads/Avatars"), employee.Avatar);
                            if (System.IO.File.Exists(oldAvatarPath))
                            {
                                System.IO.File.Delete(oldAvatarPath);
                            }
                        }
                        employee.Avatar = filename;
                    }
                    db.Entry(employee).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.Message = "Cập nhật thành công!";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi và thiết lập thông báo lỗi
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật nhân viên: {ex.Message}");
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
                }

            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: TransportFiles/Employees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return PartialView(employee);
        }

        // POST: TransportFiles/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            employee.IsActive = false; // Đánh dấu là không hoạt động thay vì xóa
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
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
