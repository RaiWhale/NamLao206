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
    [Authorize]
    public class KhaoSatsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/KhaoSats
        public async Task<ActionResult> Index()
        {
            var khaoSats = db.KhaoSats.Include(k => k.Account).Include(k => k.Account1).Include(k => k.DocumentType).Include(k => k.Supplier).Include(k => k.Project).Include(k => k.StatusProject).Include(k => k.Unit);
            return View(await khaoSats.ToListAsync());
        }

        // GET: TransportFiles/KhaoSats/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhaoSat khaoSat = await db.KhaoSats.FindAsync(id);
            if (khaoSat == null)
            {
                return HttpNotFound();
            }
            return PartialView(khaoSat);
        }

        // GET: TransportFiles/KhaoSats/Create
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
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == projectID), "Id", "TenDuAn");
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName");
            ViewBag.DonViKhaoSatId = new SelectList(db.Suppliers.Where(x => x.DonviId == project.DonViId), "Id", "SupplierName");
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName");
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName");
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
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == projectID), "Id", "TenDuAn");
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName");
            ViewBag.DonViKhaoSatId = new SelectList(db.Suppliers.Where(x => x.DonviId == project.DonViId), "Id", "SupplierName");
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName");
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName");
            return PartialView();
        }
        // POST: TransportFiles/KhaoSats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DonViKhaoSatId,ProjectID,ContractID" +
            ",GiaTriHopDong,GiaTriPLHopDong,LandArea,WaterArea,LandAreaKhaoSat,WaterAreaKhaoSat" +
            ",UnitId,GiaTriThamDinh,GiaTriDuToan,GiaTriKhaoSat,GiaTriDuToanPheDuyet" +
            ",GiaTriKhaoSatPheDuyet,GiaTriThamDinhPheDuyet,KetQuaKhaoSat,ChiPhiGiamSat" +
            ",NguoiGiamSat,TinhTrangDuAn,TinhTrangCongNo,IsActive,Note")] KhaoSat khaoSat)
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
                // 3. Gán thông tin người tạo
                khaoSat.CreateUserId = acc.Id;
                khaoSat.CreateDate = DateTime.Now;
                khaoSat.IsActive = true; // Mặc định là hoạt động
                db.KhaoSats.Add(khaoSat);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("InformationProject", "Projects", new { projectID = khaoSat.ProjectID, message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects", new { projectID = khaoSat.ProjectID, message = ViewBag.Message });
        }

        // GET: TransportFiles/KhaoSats/Edit/5
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
            KhaoSat khaoSat = await db.KhaoSats.FindAsync(id);
            if (khaoSat == null)
            {
                return HttpNotFound();
            }

            ViewBag.DonViKhaoSatId = new SelectList(db.Suppliers.Where(x => x.DonviId == khaoSat.Project.DonViId), "Id", "SupplierName", khaoSat.DonViKhaoSatId);
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == khaoSat.ProjectID), "Id", "TenDuAn", khaoSat.ProjectID);
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", khaoSat.ContractID);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName", khaoSat.UnitId);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", khaoSat.TinhTrangDuAn);
            return PartialView(khaoSat);
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
            KhaoSat khaoSat = await db.KhaoSats.FindAsync(id);
            if (khaoSat == null)
            {
                return HttpNotFound();
            }

            ViewBag.DonViKhaoSatId = new SelectList(db.Suppliers.Where(x => x.DonviId == khaoSat.Project.DonViId), "Id", "SupplierName", khaoSat.DonViKhaoSatId);
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x => x.Id == khaoSat.ProjectID), "Id", "TenDuAn", khaoSat.ProjectID);
            ViewBag.ContractID = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", khaoSat.ContractID);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "UnitName", khaoSat.UnitId);
            ViewBag.TinhTrangDuAn = new SelectList(db.StatusProjects, "Id", "StatusName", khaoSat.TinhTrangDuAn);
            return PartialView(khaoSat);
        }
        // POST: TransportFiles/KhaoSats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DonViKhaoSatId,ProjectID,ContractID,GiaTriHopDong,GiaTriPLHopDong,LandArea,WaterArea,LandAreaKhaoSat,WaterAreaKhaoSat,UnitId,GiaTriThamDinh,GiaTriDuToan" +
            ",GiaTriKhaoSat,GiaTriDuToanPheDuyet,GiaTriKhaoSatPheDuyet" +
            ",GiaTriThamDinhPheDuyet,KetQuaKhaoSat,ChiPhiGiamSat" +
            ",NguoiGiamSat,TinhTrangDuAn,TinhTrangCongNo,IsActive,Note,CreateUserId,CreateDate")] KhaoSat khaoSat)
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
                khaoSat.ModifiedUserId = acc.Id;
                khaoSat.ModifiedDate = DateTime.Now;
                db.Entry(khaoSat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";
                return RedirectToAction("InformationProject","Projects"
                    , new { projectID = khaoSat.ProjectID, message = ViewBag.Message });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects"
                , new { projectID = khaoSat.ProjectID, message = ViewBag.Message });
        }

        // GET: TransportFiles/KhaoSats/Delete/5
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
            KhaoSat khaoSat = await db.KhaoSats.FindAsync(id);
            if (khaoSat == null)
            {
                return HttpNotFound();
            }
            return PartialView(khaoSat);
        }

        // POST: TransportFiles/KhaoSats/Delete/5
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
            KhaoSat khaoSat = await db.KhaoSats.FindAsync(id);
            khaoSat.IsActive = false; // Đánh dấu là không hoạt động thay vì xóa
            db.Entry(khaoSat).State = EntityState.Modified;        
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("InformationProject", "Projects"
                , new { projectID = khaoSat.ProjectID, message = ViewBag.Message });
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
