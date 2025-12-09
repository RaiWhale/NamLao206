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
    public class GiamSatThiCongsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/GiamSatThiCongs
        public async Task<ActionResult> Index()
        {
            var giamSatThiCongs = db.GiamSatThiCongs
                .Include(g => g.Account)
                .Include(g => g.Account1)
                .Include(g => g.DocumentType)
                .Include(g => g.Supplier)
                .Include(g => g.Project)
                .Include(g => g.StatusProject);
            return View(await giamSatThiCongs.ToListAsync());
        }

        // GET: TransportFiles/GiamSatThiCongs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiamSatThiCong giamSatThiCong = await db.GiamSatThiCongs.FindAsync(id);
            if (giamSatThiCong == null)
            {
                return HttpNotFound();
            }
            return View(giamSatThiCong);
        }

        // GET: TransportFiles/GiamSatThiCongs/Create
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
            ViewBag.DonViGiamSatId = new SelectList(db.Suppliers.Where(x => x.DonviId == project.DonViId), "Id", "SupplierName");
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
            ViewBag.DonViGiamSatId = new SelectList(db.Suppliers.Where(x => x.DonviId == project.DonViId), "Id", "SupplierName");
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == projectID), "Id", "TenDuAn");
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName");
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName");
            return PartialView();
        }
        // POST: TransportFiles/GiamSatThiCongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DonViGiamSatId,ProjectID" +
            ",ContractID,GiaTriHopDong,GiaTriPLHopDong,LandVolumeContract,WaterVolumeContract" +
            ",LandVolumeNghiemThu,WaterVolumeNghiemThu,GiamSat,TinhTrangDuAn,TinhTrangCongNo,IsActive" +
            ",CreateUserId,CreateDate,Note,UnitId")] GiamSatThiCong giamSatThiCong)
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
                giamSatThiCong.CreateUserId = acc.Id;
                giamSatThiCong.CreateDate = DateTime.Now;
                giamSatThiCong.IsActive = true; // Mặc định là hoạt động
                db.GiamSatThiCongs.Add(giamSatThiCong);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("InformationProject", "Projects"
                    , new { projectID = giamSatThiCong.ProjectID, message = ViewBag.Message });            
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects", new { projectID = giamSatThiCong.ProjectID, message = ViewBag.Message });
        }

        // GET: TransportFiles/GiamSatThiCongs/Edit/5
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
            GiamSatThiCong giamSatThiCong = await db.GiamSatThiCongs.FindAsync(id);
            if (giamSatThiCong == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", giamSatThiCong.ContractID);
            ViewBag.DonViGiamSatId = new SelectList(db.Suppliers.Where(x => x.DonviId == giamSatThiCong.Project.DonViId), "Id", "SupplierName", giamSatThiCong.DonViGiamSatId);
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == giamSatThiCong.ProjectID), "Id", "TenDuAn", giamSatThiCong.ProjectID);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", giamSatThiCong.TinhTrangDuAn);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName", giamSatThiCong.UnitId);
            return PartialView(giamSatThiCong);
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
            GiamSatThiCong giamSatThiCong = await db.GiamSatThiCongs.FindAsync(id);
            if (giamSatThiCong == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", giamSatThiCong.ContractID);
            ViewBag.DonViGiamSatId = new SelectList(db.Suppliers.Where(x => x.DonviId == giamSatThiCong.Project.DonViId), "Id", "SupplierName", giamSatThiCong.DonViGiamSatId);
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == giamSatThiCong.ProjectID), "Id", "TenDuAn", giamSatThiCong.ProjectID);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", giamSatThiCong.TinhTrangDuAn);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName", giamSatThiCong.UnitId);
            return PartialView(giamSatThiCong);
        }
        // POST: TransportFiles/GiamSatThiCongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DonViGiamSatId,ProjectID,ContractID" +
            ",GiaTriHopDong,GiaTriPLHopDong,LandVolumeContract,WaterVolumeContract,LandVolumeNghiemThu" +
            ",WaterVolumeNghiemThu,GiamSat,TinhTrangDuAn,TinhTrangCongNo,IsActive,CreateUserId,CreateDate" +
            ",ModifiedDate,ModifiedUserId,Note,UnitId")] GiamSatThiCong giamSatThiCong)
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
                giamSatThiCong.ModifiedUserId = acc.Id;
                giamSatThiCong.ModifiedDate = DateTime.Now;
                db.Entry(giamSatThiCong).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("InformationProject", "Projects"
                    , new { projectID = giamSatThiCong.ProjectID, message = ViewBag.Message });                         
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects"
                , new { projectID = giamSatThiCong.ProjectID, message = ViewBag.Message });
        }

        // GET: TransportFiles/GiamSatThiCongs/Delete/5
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
            GiamSatThiCong giamSatThiCong = await db.GiamSatThiCongs.FindAsync(id);
            if (giamSatThiCong == null)
            {
                return HttpNotFound();
            }
            return PartialView(giamSatThiCong);
        }

        // POST: TransportFiles/GiamSatThiCongs/Delete/5
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
            GiamSatThiCong giamSatThiCong = await db.GiamSatThiCongs.FindAsync(id);
            giamSatThiCong.IsActive = false; // Đánh dấu là không hoạt động thay vì xóa
            db.Entry(giamSatThiCong).State = EntityState.Modified;
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("InformationProject", "Projects"
                , new { projectID = giamSatThiCong.ProjectID, message = ViewBag.Message });              
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
