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
using System.IO;

namespace NamLao206.Areas.TransportFiles.Controllers
{
    public class HoSoPhapLiesController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/HoSoPhapLies
        public async Task<ActionResult> Index()
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            var hoSoPhapLys = db.HoSoPhapLys.Include(h => h.Account).Include(h => h.Account1).Include(h => h.DocumentType).Include(h => h.Project).Include(h => h.DM_AddBangs);
            return View(await hoSoPhapLys.ToListAsync());
        }

        // GET: TransportFiles/HoSoPhapLies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoSoPhapLy hoSoPhapLy = await db.HoSoPhapLys.FindAsync(id);
            if (hoSoPhapLy == null)
            {
                return HttpNotFound();
            }
            return View(hoSoPhapLy);
        }

        // GET: TransportFiles/HoSoPhapLies/Create
        public ActionResult Create(int? projectID, int? NhapBanMu_Id)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName");
            ViewBag.ProjectID = new SelectList(db.Projects.Where(x=>x.Id == projectID), "Id", "TenDuAn");
            ViewBag.NhapBanMu_Id = new SelectList(db.NhapBanMus.Where(x => x.Id == NhapBanMu_Id), "Id", "TenPhieu");
            ViewBag.AddBangId = new SelectList(db.DM_AddBangs, "Id", "TenBang");
            ViewBag.Title = "Lưu trữ hồ sơ";
            return PartialView();
        }

        // POST: TransportFiles/HoSoPhapLies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,STT,ProjectID,NhapBanMu_Id,DocumentTypeId,TenHoSo,AddBangId,Url,IsActive,Note")] HoSoPhapLy hoSoPhapLy
            , HttpPostedFileBase file)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // 2. Lấy thông tin người dùng từ session
            var user = db.Accounts.Find(userId);
            if (user == null) {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Handle avatar upload
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = $"{DateTime.Now.Ticks}_{file.FileName.Split('/').Last()}";
                        string path = Server.MapPath("~/Uploads/" + hoSoPhapLy.ProjectID + "/" + hoSoPhapLy.AddBangId);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        file.SaveAs(Path.Combine(path, filename));
                        hoSoPhapLy.Url = filename;
                        // 3. Thiết lập thông tin người dùng cho hoSoPhapLy
                        hoSoPhapLy.CreateUserId = user.Id;
                        hoSoPhapLy.CreateDate = DateTime.Now;
                        hoSoPhapLy.IsActive = true;
                        db.HoSoPhapLys.Add(hoSoPhapLy);
                        await db.SaveChangesAsync();
                        ViewBag.Message = "Tạo mới thành công!";
                    }
                    else
                    {
                        ViewBag.Message = "Chưa chọn file để lưu!";
                    }
                    return RedirectToAction("InformationProject", "Projects", new { message = ViewBag.Message, projectID = hoSoPhapLy.ProjectID });
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi và thiết lập thông báo lỗi
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo nhân viên: {ex.Message}");
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
                }

            }
            ViewBag.Message = "Lỗi khi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects", new { message = ViewBag.Message, projectID = hoSoPhapLy.ProjectID });
        }

        // GET: TransportFiles/HoSoPhapLies/Edit/5
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
            HoSoPhapLy hoSoPhapLy = await db.HoSoPhapLys.FindAsync(id);
            if (hoSoPhapLy == null)
            {
                return HttpNotFound();
            }          
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", hoSoPhapLy.DocumentTypeId);
            ViewBag.ProjectID = new SelectList(db.Projects, "Id", "TenDuAn", hoSoPhapLy.ProjectID);
            ViewBag.AddBangId = new SelectList(db.DM_AddBangs, "Id", "TenBang", hoSoPhapLy.AddBangId);
            return PartialView(hoSoPhapLy);
        }

        // POST: TransportFiles/HoSoPhapLies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,STT,ProjectID,DocumentTypeId,TenHoSo,AddBangId,Url,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] HoSoPhapLy hoSoPhapLy
            , HttpPostedFileBase file)
        { // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            // 2. Lấy thông tin người dùng từ session
            var user = db.Accounts.Find(userId);
            if (user == null)
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            if (ModelState.IsValid)
            {            
                try
                {
                    // Handle avatar upload
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Server.MapPath("~/Uploads/" + hoSoPhapLy.Project.TenDuAn + "/" + hoSoPhapLy.DM_AddBangs.TenBang);
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(hoSoPhapLy.Url))
                        {
                            string oldFilePath = Path.Combine(path
                                , hoSoPhapLy.Url);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                        string filename = $"{DateTime.Now.Ticks}_{file.FileName.Split('/').Last()}";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        file.SaveAs(Path.Combine(path, filename));
                        hoSoPhapLy.Url = filename;

                    }                
                    // 3. Thiết lập thông tin người dùng cho hoSoPhapLy
                    hoSoPhapLy.ModifiedUserId = user.Id;
                    hoSoPhapLy.ModifiedDate = DateTime.Now;
                    db.Entry(hoSoPhapLy).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.Message = "Sửa thành công!";
                    return RedirectToAction("InformationProject", "Projects", new { message = ViewBag.Message, projectID = hoSoPhapLy.ProjectID });
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi và thiết lập thông báo lỗi
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo nhân viên: {ex.Message}");
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
                }

            }
            ViewBag.Message = "Lỗi khi nhập liệu!";
            return RedirectToAction("InformationProject", "Projects", new { message = ViewBag.Message, projectID = hoSoPhapLy.ProjectID });
        }

        // GET: TransportFiles/HoSoPhapLies/Delete/5
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
            HoSoPhapLy hoSoPhapLy = await db.HoSoPhapLys.FindAsync(id);
            if (hoSoPhapLy == null)
            {
                return HttpNotFound();
            }
            return PartialView(hoSoPhapLy);
        }

        // POST: TransportFiles/HoSoPhapLies/Delete/5
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

            HoSoPhapLy hoSoPhapLy = await db.HoSoPhapLys.FindAsync(id);
            string path = Server.MapPath("~/Uploads/" + hoSoPhapLy.Project.TenDuAn + "/" + hoSoPhapLy.DM_AddBangs.TenBang);
            // Xóa ảnh cũ nếu có
            if (!string.IsNullOrEmpty(hoSoPhapLy.Url))
            {
                string oldFilePath = Path.Combine(path
                    , hoSoPhapLy.Url);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            db.HoSoPhapLys.Remove(hoSoPhapLy);
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("InformationProject", "Projects", new { message = ViewBag.Message, projectID = hoSoPhapLy.ProjectID });
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
