using NamLao206.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class DM_ChucvusController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: Admin/DM_Chucvus
        public ActionResult Index(string search, string message)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }		
            IQueryable<DM_Chucvus> chucvus = db.DM_Chucvus;
			if (!string.IsNullOrEmpty(search))
			{
				chucvus = chucvus.Where(c => c.Chucvu.Trim().ToLower().Contains(search.Trim().ToLower()));
			}
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			ViewBag.Title = "Chức vụ -";
			return View(chucvus.ToList());
        }

        // GET: Admin/DM_Chucvus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Chucvus dM_Chucvus = db.DM_Chucvus.Find(id);
            if (dM_Chucvus == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_Chucvus);
        }

        // GET: Admin/DM_Chucvus/Create
        public ActionResult Create()
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
			return PartialView();
        }

        // POST: Admin/DM_Chucvus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Chucvu")] DM_Chucvus dM_Chucvus)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }
			if (ModelState.IsValid)
            {
				// Kiểm tra xem chức vụ đã tồn tại chưa
				var existingChucvu = db.DM_Chucvus.FirstOrDefault(c => c.Chucvu.ToLower().Trim().Contains(dM_Chucvus.Chucvu.ToLower().Trim()));
				if (existingChucvu != null)
				{
					ViewBag.Message = "Chức vụ đã tồn tại.";
					return RedirectToAction("Index", new { message = ViewBag.Message });
				}
				db.DM_Chucvus.Add(dM_Chucvus);
				await db.SaveChangesAsync();
				ViewBag.Message = "Thêm mới thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/DM_Chucvus/Edit/5
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
            DM_Chucvus dM_Chucvus = await db.DM_Chucvus.FindAsync(id);
            if (dM_Chucvus == null)
            {
                return HttpNotFound();
            }		
            return PartialView(dM_Chucvus);
        }

        // POST: Admin/DM_Chucvus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Chucvu")] DM_Chucvus dM_Chucvus)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login", new { area = "" });
            }

			if (ModelState.IsValid)
            {
				// Kiểm tra xem chức vụ đã tồn tại chưa
				var existingChucvu = db.DM_Chucvus.FirstOrDefault(c => c.Chucvu.ToLower().Trim().Contains(dM_Chucvus.Chucvu.ToLower().Trim()));
				if (existingChucvu != null)
				{
					ViewBag.Message = "Chức vụ đã tồn tại.";
					return RedirectToAction("Index", new { message = ViewBag.Message });
				}
				db.Entry(dM_Chucvus).State = EntityState.Modified;
				await db.SaveChangesAsync();
				ViewBag.Message = "sửa thành công!";
				return RedirectToAction("Index", new { message = ViewBag.Message });
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message });
		}

        // GET: Admin/DM_Chucvus/Delete/5
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
            DM_Chucvus dM_Chucvus = await db.DM_Chucvus.FindAsync(id);
            if (dM_Chucvus == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_Chucvus);
        }

        // POST: Admin/DM_Chucvus/Delete/5
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
			DM_Chucvus dM_Chucvus = await db.DM_Chucvus.FindAsync(id);
            db.DM_Chucvus.Remove(dM_Chucvus);
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
