using NamLao206.Models;
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

namespace NamLao206.Areas.Admin.Controllers
{
    public class TransportFilesController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/TransportFiles
        public async Task<ActionResult> Index(string message, string search)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            IQueryable<TransportFile> transportFiles = db.TransportFiles.Include(u => u.Account).Include(u => u.Account1);
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            if (!string.IsNullOrEmpty(search))
            {
                transportFiles = transportFiles.Where(x => x.tenFile.Contains(search));
            }
            ViewBag.Title = "Mail - ";
            return View(await transportFiles.OrderByDescending(x=>x.NgayBanHanh).ToListAsync());         
        }

        // GET: Admin/TransportFiles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportFile transportFile = await db.TransportFiles.FindAsync(id);
            if (transportFile == null)
            {
                return HttpNotFound();
            }
            return View(transportFile);
        }

        // GET: Admin/TransportFiles/Create
        public ActionResult Create()
        {
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName");
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName");
            ViewBag.NguoiPheDuyetId = new SelectList(db.Accounts, "Id", "LoginName");
            return View();
        }

        // POST: Admin/TransportFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,tenFile,url,CreateDate,ModifiedDate,CreateUserId,ModifiedUserId,IsActive,KhanCap,Mota,NgayBanHanh,SoTrang,NguoiPheDuyetId,DoMat,PheDuyet")] TransportFile transportFile)
        {
            if (ModelState.IsValid)
            {
                db.TransportFiles.Add(transportFile);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.ModifiedUserId);
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.CreateUserId);
            ViewBag.NguoiPheDuyetId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.NguoiPheDuyetId);
            return View(transportFile);
        }

        // GET: Admin/TransportFiles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportFile transportFile = await db.TransportFiles.FindAsync(id);
            if (transportFile == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.ModifiedUserId);
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.CreateUserId);
            ViewBag.NguoiPheDuyetId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.NguoiPheDuyetId);
            return View(transportFile);
        }

        // POST: Admin/TransportFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,tenFile,url,CreateDate,ModifiedDate,CreateUserId,ModifiedUserId,IsActive,KhanCap,Mota,NgayBanHanh,SoTrang,NguoiPheDuyetId,DoMat,PheDuyet")] TransportFile transportFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transportFile).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.ModifiedUserId);
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.CreateUserId);
            ViewBag.NguoiPheDuyetId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.NguoiPheDuyetId);
            return View(transportFile);
        }

        // GET: Admin/TransportFiles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportFile transportFile = await db.TransportFiles.FindAsync(id);
            if (transportFile == null)
            {
                return HttpNotFound();
            }
            return View(transportFile);
        }

        // POST: Admin/TransportFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userId = int.Parse(User.Identity.Name);
            var acc = db.Accounts.FirstOrDefault(x => x.Id == userId);

            // Kiểm tra tài khoản tồn tại
            if (acc == null)
            {
                return RedirectToAction("Login", "Account");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var transportFile = db.TransportFiles.Include(tf => tf.Transports).FirstOrDefault(tf => tf.Id == id);

                    if (transportFile != null)
                    {
                  
                        // Thực hiện xóa
                        db.Transports.RemoveRange(transportFile.Transports);
                        db.TransportFileUrls.Remove(db.TransportFileUrls.FirstOrDefault(x => x.TransportFilesId == id));
                        db.TransportFiles.Remove(transportFile);
                        db.SaveChanges();
                        string dir = Server.MapPath("~/Uploads/HopThu") + "\\" + transportFile.Id;
                        if (Directory.Exists(dir))
                        {
                            Directory.Delete(dir, true);
                        }
                        transaction.Commit();
                        ViewBag.Message = "Xóa thành công!";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Xử lý exception hoặc ghi log tại đây
                    ViewBag.Message = "Lỗi khi xóa file: " + ex.Message;
                    throw;
                }
            }
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
