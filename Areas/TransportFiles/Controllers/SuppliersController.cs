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
	public class SuppliersController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/Suppliers
        public ActionResult Index(string message, string search)
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
			// Lấy danh sách nhà cung cấp theo đơn vị			
			IQueryable<Supplier> suppliers = db.Suppliers.Include(s => s.Account).Include(s => s.Account1)
				.Where(x => x.DonviId == acc.Employee.DM_PhongBans.donvi_Id);
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			if (!string.IsNullOrEmpty(search))
			{				
				suppliers = suppliers
					.Where(x => x.SupplierName.Contains(search) || x.Address.Contains(search) || x.Phone.Contains(search));		
			}
			ViewBag.Title = "Nhà cung cấp - ";
			return View(suppliers.ToList());
        }

        // GET: TransportFiles/Suppliers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier);
        }

        // GET: TransportFiles/Suppliers/Create
        public ActionResult Create()
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}		
			return PartialView();
        }

        // POST: TransportFiles/Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SupplierName,Address,Phone,NguoiDaiDien,IsActive,CreateUserId,DonviId,CreateDate,ModifiedDate,ModifiedUserId,Note")] Supplier supplier)
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
				supplier.CreateUserId = userId;
				supplier.CreateDate = DateTime.Now;
				supplier.IsActive = true;
                supplier.DonviId = acc.Employee.DM_PhongBans.donvi_Id;
				db.Suppliers.Add(supplier);
                await db.SaveChangesAsync();
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: TransportFiles/Suppliers/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }    
            return PartialView(supplier);
        }

        // POST: TransportFiles/Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SupplierName,Address,Phone,NguoiDaiDien,IsActive,CreateUserId,DonviId,CreateDate,ModifiedDate,ModifiedUserId,Note")] Supplier supplier)
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
				supplier.ModifiedUserId = userId;
				supplier.ModifiedDate = DateTime.Now;		
				db.Entry(supplier).State = EntityState.Modified;
                await db.SaveChangesAsync();
				ViewBag.Message = "Sửa thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: TransportFiles/Suppliers/Delete/5
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
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier);
        }

        // POST: TransportFiles/Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			Supplier supplier = await db.Suppliers.FindAsync(id);
			// Lấy thông tin tài khoản
			var acc = db.Accounts
				.Where(x => x.Id == userId)
				.SingleOrDefault();
			if (acc == null)
			{
				ViewBag.Message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên.";
				return RedirectToAction("Login", "Login", new { area = "" });
			}
			else if (acc.Id != supplier.CreateUserId)
			{
				ViewBag.Message = "Bạn không có quyền xóa nhà cung cấp!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}		
            db.Suppliers.Remove(supplier);
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
