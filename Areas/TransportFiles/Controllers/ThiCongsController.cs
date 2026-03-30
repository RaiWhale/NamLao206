using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NamLao206.Models;

namespace NamLao206.Areas.TransportFiles.Controllers
{
    public class ThiCongsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/ThiCongs
        public async Task<ActionResult> Index()
        {
            var thiCongs = db.ThiCongs.Include(t => t.Account).Include(t => t.Account1).Include(t => t.DocumentType).Include(t => t.Project).Include(t => t.StatusProject).Include(t => t.Supplier).Include(t => t.Unit);
            return View(await thiCongs.ToListAsync());
        }

        // GET: TransportFiles/ThiCongs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThiCong thiCong = await db.ThiCongs.FindAsync(id);
            if (thiCong == null)
            {
                return HttpNotFound();
            }
       
            return View(thiCong);
        }
        public ActionResult Create(int? projectID)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            else if (projectID == null)
            {
                ViewBag.Message = "Không tìm thấy dự án ID!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
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
            var project = db.Projects.Find(projectID);
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName");
            ViewBag.DonViThiCongId = new SelectList(db.Suppliers.Where(x => x.DonviId == project.DonViId), "Id", "SupplierName");
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == projectID), "Id", "TenDuAn");
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName");
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName");
            ViewBag.DonVi = acc.Employee.DM_PhongBans.DM_DonVis;
            return PartialView();
        }
        // GET: TransportFiles/ThiCongs/Create
        public ActionResult CreateBomMin(int? projectID)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            else if (projectID == null)
            {
                ViewBag.Message = "Không tìm thấy dự án ID!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            var project = db.Projects.Find(projectID);
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName");
            ViewBag.DonViThiCongId = new SelectList(db.Suppliers.Where(x => x.DonviId == project.DonViId), "Id", "SupplierName");
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == projectID), "Id", "TenDuAn");
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName");
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName");
            return PartialView();
        }
        public ActionResult CreateCumBan(int? projectID)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            else if (projectID == null)
            {
                ViewBag.Message = "Không tìm thấy dự án ID!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            var project = db.Projects.Find(projectID);
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName");
            ViewBag.DonViThiCongId = new SelectList(db.Suppliers.Where(x => x.DonviId == project.DonViId), "Id", "SupplierName");
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == projectID), "Id", "TenDuAn");
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName");
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName");
            return PartialView();
        }
        // POST: TransportFiles/ThiCongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DonViThiCongId,ProjectID" +
            ",ContractID,GiaTriHopDong,GiaTriPLHopDong,LandVolumeContract,WaterVolumeContract" +
            ",LandVolumeNghiemThu,WaterVolumeNghiemThu,GiamSat,TinhTrangDuAn,TinhTrangCongNo,IsActive" +
            ",CreateUserId,CreateDate,Note,UnitId")] ThiCong thiCong)
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
                thiCong.CreateUserId = acc.Id;
                thiCong.CreateDate = DateTime.Now;
                thiCong.IsActive = true; // Mặc định là hoạt động
                db.ThiCongs.Add(thiCong);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("InformationProject", "Projects"
                    , new { projectID = thiCong.ProjectID, message = ViewBag.Message });            
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects", new { projectID = thiCong.ProjectID, message = ViewBag.Message });
        }
        public async Task<ActionResult> Edit(int? id)
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
            // Lấy thông tin tài khoản
            var acc = db.Accounts
                .Where(x => x.Id == userId)
                .SingleOrDefault();
            if (acc == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            ThiCong thiCong = await db.ThiCongs.FindAsync(id);
            if (thiCong == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", thiCong.ContractID);
            ViewBag.DonViThiCongId = new SelectList(db.Suppliers.Where(x => x.DonviId == thiCong.Project.DonViId), thiCong.DonViThiCongId);
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == thiCong.ProjectID), "Id", "TenDuAn", thiCong.ProjectID);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", thiCong.TinhTrangDuAn);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName", thiCong.UnitId);
            ViewBag.DonVi = acc.Employee.DM_PhongBans.DM_DonVis;
            return PartialView(thiCong);
        }
        // GET: TransportFiles/ThiCongs/Edit/5
        public async Task<ActionResult> EditBomMin(int? id)
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
            ThiCong thiCong = await db.ThiCongs.FindAsync(id);
            if (thiCong == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", thiCong.ContractID);
            ViewBag.DonViThiCongId = new SelectList(db.Suppliers.Where(x => x.DonviId == thiCong.Project.DonViId), thiCong.DonViThiCongId);
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == thiCong.ProjectID), "Id", "TenDuAn", thiCong.ProjectID);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", thiCong.TinhTrangDuAn);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName", thiCong.UnitId);
            return PartialView(thiCong);
        }
        public async Task<ActionResult> EditCumBan(int? id)
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
            ThiCong thiCong = await db.ThiCongs.FindAsync(id);
            if (thiCong == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", thiCong.ContractID);
            ViewBag.DonViThiCongId = new SelectList(db.Suppliers.Where(x => x.DonviId == thiCong.Project.DonViId), thiCong.DonViThiCongId);
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == thiCong.ProjectID), "Id", "TenDuAn", thiCong.ProjectID);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", thiCong.TinhTrangDuAn);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName", thiCong.UnitId);
            return PartialView(thiCong);
        }
        // POST: TransportFiles/ThiCongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DonViThiCongId,ProjectID,ContractID" +
            ",GiaTriHopDong,GiaTriPLHopDong,LandVolumeContract,WaterVolumeContract,LandVolumeNghiemThu" +
            ",WaterVolumeNghiemThu,GiamSat,TinhTrangDuAn,TinhTrangCongNo,IsActive,CreateUserId,CreateDate" +
            ",ModifiedDate,ModifiedUserId,Note,UnitId")] ThiCong thiCong)
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
            if (ModelState.IsValid)
            {
                // 3. Gán thông tin người sửa
                thiCong.ModifiedUserId = acc.Id;
                thiCong.ModifiedDate = DateTime.Now;
                db.Entry(thiCong).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("InformationProject", "Projects"
                    , new { projectID = thiCong.ProjectID, message = ViewBag.Message });                         
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects"
                , new { projectID = thiCong.ProjectID, message = ViewBag.Message });
        }

        // GET: TransportFiles/ThiCongs/Delete/5
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
            ThiCong thiCong = await db.ThiCongs.FindAsync(id);
            if (thiCong == null)
            {
                return HttpNotFound();
            }
            return PartialView(thiCong);
        }

        // POST: TransportFiles/ThiCongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            ThiCong thiCong = await db.ThiCongs.FindAsync(id);
            thiCong.IsActive = false; // Đánh dấu là không hoạt động thay vì xóa
            db.Entry(thiCong).State = EntityState.Modified;
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("InformationProject", "Projects"
                , new { projectID = thiCong.ProjectID, message = ViewBag.Message });              
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
