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
    public class ProjectsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

		// GET: TransportFiles/Projects
		public ActionResult Index(string search, string message)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			// 2. Lấy thông tin tài khoản
			var acc =  db.Accounts
				.Where(x => x.Id == userId)
				.SingleOrDefault();
			if (acc == null)
			{
				ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			// 3. Lấy danh sách dự án theo đơn vị
			IQueryable<Project> projects = db.Projects.AsNoTracking()
				.Include(p => p.Account)
				.Include(p => p.Account1)
				.Include(p => p.DM_DonVis)
				.Include(p => p.Supplier)
				.Include(p => p.StatusProject)
				.Where(x => x.DonViId == acc.Employee.DM_PhongBans.donvi_Id && x.IsActive == true);
			// 4. Xử lý tìm kiếm
			if (!string.IsNullOrEmpty(search))
			{
				search = search.Trim().ToLower();
				projects =  projects
					.Where(x => x.TenDuAn.ToLower().Contains(search) || x.MaDuAn.ToLower().Contains(search) || x.Note.ToLower().Contains(search));
			}
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}

			ViewBag.Title = "Dự án - ";
            ViewBag.DonVi = acc.Employee.DM_PhongBans.DM_DonVis;
            return View(projects.ToList());
        }
		public ActionResult InformationProject(int? projectID, string message)
		{
			if (projectID == null)
			{
                ViewBag.Message = "Không tìm thấy dự án ID!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            Project project = db.Projects.Find(projectID);
            if (project == null)
            {
                ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.Title = "Thông tin dự án - ";
            ViewBag.Message = message;
            return View(project);
        }

        // GET: TransportFiles/Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        // GET: TransportFiles/Projects/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            // Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();

            if (acc == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            PopulateDropdowns(acc);
            ViewBag.DonVi = acc.Employee.DM_PhongBans.DM_DonVis;
            return PartialView();
        }
        // GET: TransportFiles/Projects/CreateBomMin
        public ActionResult CreateBomMin()
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}

			// Lấy thông tin tài khoản
			var acc = db.Accounts
				.Where(x => x.Id == userId)
				.SingleOrDefault();

			if (acc == null)
			{
				ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}						
            PopulateDropdowns(acc);
			return PartialView();
        }

        public ActionResult CreateCumBan()
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            // Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();

            if (acc == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
          
            PopulateDropdowns(acc);
            return PartialView();
        }
        // POST: TransportFiles/Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public async Task<ActionResult> Create([Bind(Include = "Id,TenDuAn,MaDuAn,DonViId,XaId,HuyenId" +
			",TinhId,InvestorId,ProjectValue,GiamSat,StartDate,EndDate,TinhTrangDuAn,TinhTrangCongNo" +
			",ContractId,Note,Longtitude,Latitude,CongNo,DiaChi,LoaiDuAn")] Project project)
        {
            if (ModelState.IsValid)
			{ 
				if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
				{
					ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}

				// Lấy thông tin tài khoản
				var acc = db.Accounts
					.Where(x => x.Id == userId)
					.SingleOrDefault();

				if (acc == null)
				{
					ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
				var maduan = db.Projects
                    .Where(x => x.MaDuAn == project.MaDuAn && x.DonViId == acc.Employee.DM_PhongBans.donvi_Id && x.IsActive == true)
                    .FirstOrDefault();
                if (maduan != null)
				{
                    ViewBag.Message = "Mã dự án đã tồn tại trong đơn vị này!";
                    return RedirectToAction("Index", new { message = ViewBag.Message });
                }
                project.LoaiDuAn = acc.Employee.DM_PhongBans.DM_DonVis.CreateBranch;
                project.CreateUserId = userId;
                project.CreateDate = DateTime.Now;
                project.IsActive = true;
				db.Projects.Add(project);
				await db.SaveChangesAsync();
				ViewBag.Message = "Thêm mới thành công!";
				return RedirectToAction("InformationProject", new {projectID = project.Id, message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}
        public async Task<ActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // Lấy thông tin tài khoản
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
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.TinhId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten");
            ViewBag.DonViId = new SelectList(db.DM_DonVis.Where(x => x.Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenDonVi", project.DonViId);
            ViewBag.InvestorId = new SelectList(db.Suppliers, "Id", "SupplierName", project.InvestorId);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", project.TinhTrangDuAn);
            ViewBag.ContractId = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", project.ContractId);
            ViewBag.DonVi = acc.Employee.DM_PhongBans.DM_DonVis;
            return PartialView(project);
        }
        // GET: TransportFiles/Projects/Edit/5
        public async Task<ActionResult> EditBomMin(int? id)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			// Lấy thông tin tài khoản
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
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
			ViewBag.TinhId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten");
			ViewBag.DonViId = new SelectList(db.DM_DonVis.Where(x => x.Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenDonVi", project.DonViId);	
            ViewBag.InvestorId = new SelectList(db.Suppliers, "Id", "SupplierName", project.InvestorId);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", project.TinhTrangDuAn);
			ViewBag.ContractId = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", project.ContractId);
			return PartialView(project);
        }
        public async Task<ActionResult> EditCumBan(int? id)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // Lấy thông tin tài khoản
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
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.TinhId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten");
            ViewBag.DonViId = new SelectList(db.DM_DonVis.Where(x => x.Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenDonVi", project.DonViId);
            ViewBag.InvestorId = new SelectList(db.Suppliers, "Id", "SupplierName", project.InvestorId);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", project.TinhTrangDuAn);
            ViewBag.ContractId = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", project.ContractId);
            return PartialView(project);
        }
        // POST: TransportFiles/Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,TenDuAn,MaDuAn,DonViId,XaId,HuyenId" +
			",TinhId,InvestorId,ProjectValue,GiamSat,StartDate,EndDate,TinhTrangDuAn,TinhTrangCongNo" +
            ",ContractId,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,IsActive,Note,Longtitude,Latitude,CongNo,DiaChi,LoaiDuAn")] Project project)
        {
            if (ModelState.IsValid)
            {
				if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
				{
					ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
				// Lấy thông tin tài khoản
				var acc = db.Accounts
					.Where(x => x.Id == userId)
					.SingleOrDefault();
				if (acc == null)
				{
					ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
				//else if (acc.Id != project.Account.Id)
				//{
				//	ViewBag.Message = "Bạn không có quyền sửa dự án này!";
				//	return RedirectToAction("Index", new { message = ViewBag.Message });
				//}

				project.ModifiedUserId = userId;
				project.ModifiedDate = DateTime.Now;

				// Lưu thông tin vào cơ sở dữ liệu
				db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
				ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("InformationProject", new { projectID = project.Id, message = ViewBag.Message });
            }		
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

		// GET: TransportFiles/Projects/Delete/5
		public async Task<ActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return PartialView(project);
        }

        // POST: TransportFiles/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			// Kiểm tra xem dự án có tồn tại không
			Project project = await db.Projects.FindAsync(id);
			// Lấy thông tin tài khoản
			var acc = db.Accounts
				.Where(x => x.Id == userId)
				.SingleOrDefault();
			if (acc == null)
			{
				ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			else if (acc.Id != project.CreateUserId)
			{
				ViewBag.Message = "Bạn không có quyền xóa dự án này!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			project.IsActive = false; // Đánh dấu dự án là không hoạt động
            foreach (var khaoSat in project.KhaoSats.ToList())
            {
                khaoSat.IsActive = false;
            }
            foreach (var giamSatThiCong in project.GiamSatThiCongs.ToList())
            {
                giamSatThiCong.IsActive = false;
            }
            foreach (var nghiemThu in project.NghiemThus.ToList())
            {
                nghiemThu.IsActive = false;
            }
            foreach (var hoSoPhapLy in project.HoSoPhapLys.ToList())
            {
                hoSoPhapLy.IsActive = false;
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            db.Entry(project).State = EntityState.Modified;                 
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("Index");
        }
		private void PopulateDropdowns(Account acc)
		{
            ViewBag.DonViId = new SelectList(db.DM_DonVis.Where(x => x.Id == acc.Employee.DM_PhongBans.donvi_Id), "Id", "TenDonVi");
            ViewBag.TinhId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten");			
			ViewBag.InvestorId = new SelectList(db.Suppliers.Where(x => x.DonviId == acc.Employee.DM_PhongBans.donvi_Id), "Id", "SupplierName");
			ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects.Where(x => x.PhanLoai == "1"), "Id", "StatusName");
			ViewBag.ContractId = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName");
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
