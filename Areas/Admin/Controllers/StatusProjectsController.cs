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

namespace NamLao206.Areas.Admin.Controllers
{
	[Authorize]
	public class StatusProjectsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/StatusProjects
        public async Task<ActionResult> Index(string message, string search)
        {
            IQueryable<StatusProject> statusProjects = db.StatusProjects.Include(s => s.Account).Include(s => s.Account1);
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			if (!string.IsNullOrEmpty(search))
			{
				statusProjects = statusProjects
					.Where(x => x.StatusName.Contains(search) || x.Note.Contains(search));
			}
			ViewBag.Title = "Trạng thái dự án - ";
			return View(await statusProjects.ToListAsync());
        }

        // GET: Admin/StatusProjects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusProject statusProject = await db.StatusProjects.FindAsync(id);
            if (statusProject == null)
            {
                return HttpNotFound();
            }
            return View(statusProject);
        }

        // GET: Admin/StatusProjects/Create
        public ActionResult Create()
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			return PartialView();
        }

        // POST: Admin/StatusProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StatusName,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] StatusProject statusProject)
        {
            if (ModelState.IsValid)
            {
				// Set the CreateUserId to the current user's ID
				if (User.Identity.IsAuthenticated && int.TryParse(User.Identity.Name, out int userId))
				{
					statusProject.CreateUserId = userId;
				}
				else
				{
					ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
				// Set the CreateDate to the current date and time
				statusProject.CreateDate = DateTime.Now;		
				statusProject.IsActive = true;
				db.StatusProjects.Add(statusProject);
                await db.SaveChangesAsync();
				ViewBag.Message = "Thêm mới thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/StatusProjects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusProject statusProject = await db.StatusProjects.FindAsync(id);
            if (statusProject == null)
            {
                return HttpNotFound();
            }   
            return PartialView(statusProject);
        }

        // POST: Admin/StatusProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StatusName,IsActive,CreateUserId,CreateDate,ModifiedDate,ModifiedUserId,Note")] StatusProject statusProject)
        {
            if (ModelState.IsValid)
            {
				// Set the ModifiedUserId to the current user's ID
				if (User.Identity.IsAuthenticated && int.TryParse(User.Identity.Name, out int userId))
				{
					statusProject.ModifiedUserId = userId;
				}
				else
				{
					ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
				// Set the ModifiedDate to the current date and time
				statusProject.ModifiedDate = DateTime.Now;
				// Update the StatusProject in the database
				db.Entry(statusProject).State = EntityState.Modified;
                await db.SaveChangesAsync();
				ViewBag.Message = "Sửa thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/StatusProjects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusProject statusProject = await db.StatusProjects.FindAsync(id);
            if (statusProject == null)
            {
                return HttpNotFound();
            }
            return View(statusProject);
        }

        // POST: Admin/StatusProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StatusProject statusProject = await db.StatusProjects.FindAsync(id);
            db.StatusProjects.Remove(statusProject);
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
