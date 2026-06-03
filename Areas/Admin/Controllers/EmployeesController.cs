using PagedList;
using NamLao206.Models;
using NamLao206.Models.ViewModels;
using NamLao206.Utils;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        private static string UploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", "Avatars");
        int pageSize = 10;
        // GET: Admin/Employee
        public ActionResult Index(int? page, string search, string message)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            IQueryable<Employee> employees = db.Employees
				.Include(d => d.DM_Chucvus)
				.Include(d => d.DM_Donvihanhchinhs)
				.Include(d => d.DM_Hocvis)
				.Include(d => d.DM_PhongBans)		
				.Include(d => d.DM_Nghenghieps)
				.Include(d => d.Level);
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			if (!string.IsNullOrEmpty(search))
			{
				employees = employees.Where(c => c.Name.ToLower().Contains(search.Trim().ToLower()));
			}
			employees = employees.OrderByDescending(c => c.CreatedDate);
			//Paging		     
			int pageNumber = page ?? 1;
			ViewBag.Title = "Nhân viên -";
			ViewBag.search = search;		
			return View(employees.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return PartialView(employee);
        }

        // GET: Admin/Employee/Create
        public ActionResult Create()
        {
			// 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }            
			ViewBag.CityId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten");
            ViewBag.ChucvuId = new SelectList(db.DM_Chucvus, "Id", "Chucvu");
            ViewBag.HocviId = new SelectList(db.DM_Hocvis, "Id", "tenHocvi");
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa");
            ViewBag.NghenghiepId = new SelectList(db.DM_Nghenghieps, "Id", "Nghenghiep");
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName");
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "GioiTinh");
            return PartialView();
        }

        // POST: Admin/Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(
	[Bind(Include = "Id,Name,Phone,Email,Address,LevelId,KhoaphongId,NghenghiepId,ChucvuId,HocviId,CityId,DistrictId,WardId,Avatar,GenderId,Birthday")] Employee employee,
	RegisterVM data,
	string LoginName,
	string Password,
	HttpPostedFileBase pic)
		{
			if (ModelState.IsValid)
			{
			try
			{
				// Handle avatar upload
					if (pic != null && pic.ContentLength > 0)
					{
						string filename = $"{DateTime.Now.Ticks}_{pic.FileName.Split('/').Last()}";
						string path = UploadPath;
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
					// Thêm nhân viên vào database
					db.Employees.Add(employee);
					await db.SaveChangesAsync();

				// Create account
				var account = NamLao206.AutoMapperConfig.Mapper.Map<Account>(data);
				account.EmployeeId = employee.Id;
				account.Password = MySecurity.Encrypt(Password);
				account.IsActive = true;
				db.Accounts.Add(account);
				await db.SaveChangesAsync();

					// Thiết lập thông báo thành công
					ViewBag.Message = "Thêm mới thành công!";
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

		// GET: Admin/Employee/Edit/5
		public async Task<ActionResult> Edit(int? id)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
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

			ViewBag.CityId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten", employee.CityId);
			ViewBag.DistrictId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == employee.CityId), "Id", "Ten", employee.DistrictId);
			ViewBag.WardId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == employee.DistrictId), "Id", "Ten", employee.WardId);
			ViewBag.ChucvuId = new SelectList(db.DM_Chucvus, "Id", "Chucvu", employee.ChucvuId);
            ViewBag.HocviId = new SelectList(db.DM_Hocvis, "Id", "tenHocvi", employee.HocviId);
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", employee.KhoaphongId);
            ViewBag.NghenghiepId = new SelectList(db.DM_Nghenghieps, "Id", "Nghenghiep", employee.NghenghiepId);
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
			ViewBag.GenderId = new SelectList(db.Genders, "Id", "GioiTinh", employee.GenderId);
			return PartialView(employee);
        }

        // POST: Admin/Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Phone,Email,Address,LevelId,IsActive,KhoaphongId,NghenghiepId,ChucvuId,HocviId,CreatedDate,CityId,DistrictId,WardId,Avatar,GenderId,Birthday")] Employee employee
            ,RegisterVM data
            , HttpPostedFileBase pic)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			if (ModelState.IsValid)
            {
				try
				{
					// Xử lý upload ảnh
					if (pic != null && pic.ContentLength > 0)
					{
						string filename = $"{DateTime.Now.Ticks}_{pic.FileName.Split('/').Last()}";
						string path = UploadPath;
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						pic.SaveAs(Path.Combine(path, filename));
						// Xóa ảnh cũ nếu có
						if (!string.IsNullOrEmpty(employee.Avatar))
						{
							string oldAvatarPath = Path.Combine(UploadPath, employee.Avatar);
							if (System.IO.File.Exists(oldAvatarPath))
							{
								System.IO.File.Delete(oldAvatarPath);
							}
						}
						employee.Avatar = filename;
					}
					db.Entry(employee).State = EntityState.Modified;
					var acc = db.Accounts.SingleOrDefault(x => x.EmployeeId == employee.Id);
					if (acc != null)
					{
						acc.LevelId = data.LevelId;
						acc.LoginName = data.LoginName;						
						db.Entry(acc).State = EntityState.Modified;
                    }	
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

        // GET: Admin/Employee/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await  db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return PartialView(employee);
        }

        // POST: Admin/Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			var acc = db.Accounts.SingleOrDefault(x => x.EmployeeId == id);
            if (acc != null)
            {
                db.Accounts.Remove(acc);
            }
            Employee employee = db.Employees.Find(id);
			// Xóa ảnh cũ nếu có
			if (!string.IsNullOrEmpty(employee.Avatar))
			{
				string oldAvatarPath = Path.Combine(UploadPath, employee.Avatar);
				if (System.IO.File.Exists(oldAvatarPath))
				{
					System.IO.File.Delete(oldAvatarPath);
				}
			}
			db.Employees.Remove(employee);
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
