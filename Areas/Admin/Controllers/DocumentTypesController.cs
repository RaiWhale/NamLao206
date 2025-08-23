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
    public class DocumentTypesController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/DocumentTypes
        public async Task<ActionResult> Index(string message, string search)
        {
			IQueryable<DocumentType> documentTypes = db.DocumentTypes.Include(d => d.Account);
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			if (!string.IsNullOrEmpty(search))
			{
				documentTypes = db.DocumentTypes.Include(d => d.Account)
					.Where(x => x.DocumentTypeName.Contains(search) || x.Note.Contains(search));
			}
			ViewBag.Title = "Loại tài liệu - ";				
            return View(await documentTypes.ToListAsync());
        }

        // GET: Admin/DocumentTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = await db.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        // GET: Admin/DocumentTypes/Create
        public ActionResult Create()
        {		
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}		
			return PartialView();
        }

        // POST: Admin/DocumentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DocumentTypeName,Note")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
				// Set the CreateUserId to the current user's ID
				if (User.Identity.IsAuthenticated && int.TryParse(User.Identity.Name, out int userId))
				{
					documentType.CreateUserId = userId;
				}
				else
				{
					ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
			
				documentType.CreateDate = DateTime.Now;				
				documentType.IsActive = true;
				// Add the new DocumentType to the database
				db.DocumentTypes.Add(documentType);
                await db.SaveChangesAsync();
				ViewBag.Message = "Thêm mới thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/DocumentTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = await db.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }       
            return PartialView(documentType);
        }

        // POST: Admin/DocumentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DocumentTypeName,IsActive,CreateUserId,CreateDate,Note")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
				// Set the ModifiedUserId to the current user's ID
				if (User.Identity.IsAuthenticated && int.TryParse(User.Identity.Name, out int userId))
				{
					documentType.CreateUserId = userId;
				}
				else
				{
					ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}
				// Set the ModifiedDate to the current date and time
				documentType.CreateDate = DateTime.Now;
				db.Entry(documentType).State = EntityState.Modified;
                await db.SaveChangesAsync();
				ViewBag.Message = "Sửa thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}

			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/DocumentTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = await db.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return PartialView(documentType);
        }

        // POST: Admin/DocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DocumentType documentType = await db.DocumentTypes.FindAsync(id);
            db.DocumentTypes.Remove(documentType);
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
