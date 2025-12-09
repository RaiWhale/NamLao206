using PagedList;
using NamLao206.Models;
using NamLao206.Models.ViewModels;
using NamLao206.Utils;
using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        private static string UploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", "News");
        int pageSize = 10;
        // GET: Admin/News
        public ActionResult Index(int? page, string search, string message)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			IQueryable<News> news = db.News.Include(n => n.Account).Include(n => n.SubMenu).Include(n => n.Topic);
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			if (search != null && search.Trim() != "")
            {
                news = news.Where(s => s.Title.Trim().ToLower().Contains(search.Trim().ToLower()));
            }
			news = news.OrderByDescending(x => x.uutien).ThenByDescending(x => x.DateUp);
			ViewBag.search = search;
            //Paging
            int pageNumber = page ?? 1;

			ViewBag.Title = "Tin tức -";
			return View(news.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return PartialView(news);
        }

		// GET: Admin/News/Create

		public ActionResult Create(int? page)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			ViewBag.Title = "Đăng tin ";
            ViewBag.cosoId = new SelectList(db.Transports, "Id", "Coso");   
            ViewBag.TopicId = new SelectList(db.Topics.Where(x => x.NhomNews != 1), "Id", "TopicName");
            ViewBag.SubMenuId = new SelectList(db.SubMenus, "Id", "subMenuName");
			ViewBag.page = page;
			return View();
        }

		public ActionResult Create2(int? page)
        {

            ViewBag.Title = "Đăng tin";
            ViewBag.cosoId = new SelectList(db.Transports, "Id", "Coso");
            ViewBag.TopicId = new SelectList(db.Topics.Where(x => x.NhomNews != 1), "Id", "TopicName");
            ViewBag.SubMenuId = new SelectList(db.SubMenus, "Id", "subMenuName");
			ViewBag.page = page;
			return View();
        }
        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Summary,Details,TopicId,AdminId,Views,Picture,SubMenuId,cosoId,TitleChange,DateUp,Author,uutien")] News news, HttpPostedFileBase pic, string DateUp, int? page)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			if (ModelState.IsValid)
            {
                try
                {
					// 2. Kiểm tra xem người dùng có quyền tạo tin tức hay không
					var acc = db.Accounts
					.Where(x => x.Id == userId)
					.SingleOrDefault();
					// Xử lý upload ảnh
					string filename = "";
					if (pic != null && pic.ContentLength > 0)
					{
						filename = $"{DateTime.Now.Ticks}_{pic.FileName.Split('/').Last()}";		
						if (!Directory.Exists(UploadPath))
						{
							Directory.CreateDirectory(UploadPath);
						}
						pic.SaveAs(Path.Combine(UploadPath, filename));

					}				
					news.TitleChange = MySecurity.RemoveDiacritics(news.Title);			
					news.Duyet = true;
					news.Picture = filename;
					news.AdminId = acc.Id;
					db.News.Add(news);
					await db.SaveChangesAsync();
					ViewBag.Message = "Tạo mới thành công!";
					return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
				}
				catch (Exception ex)
				{
					// Ghi log lỗi và thiết lập thông báo lỗi
					System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo nhân viên: {ex.Message}");
					ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
				}			
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message, page = page});
		}

		// GET: Admin/News/Edit/5
		public async Task<ActionResult> Edit(int? id, int? page)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page;	
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName", news.TopicId);
            ViewBag.SubMenuId = new SelectList(db.SubMenus.Where(x => x.TopicId == news.TopicId), "Id", "subMenuName", news.SubMenuId);
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Summary,Details,TopicId,AdminId,Views,Picture,SubMenuId,cosoId,uutien,Duyet,Author,DateUp")] News news, HttpPostedFileBase pic, int? page)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			if (ModelState.IsValid)
            {
                try
                {
					// 2. Kiểm tra xem người dùng có quyền tạo tin tức hay không
					var acc = db.Accounts
					.Where(x => x.Id == userId)
					.SingleOrDefault();
					// Xử lý upload ảnh
					string filename = "";
					if (pic != null && pic.ContentLength > 0)
					{
						filename = $"{DateTime.Now.Ticks}_{pic.FileName.Split('/').Last()}";
						if (!Directory.Exists(UploadPath))
						{
							Directory.CreateDirectory(UploadPath);
						}
						pic.SaveAs(Path.Combine(UploadPath, filename));

						// Xóa ảnh cũ nếu có
						if (!string.IsNullOrEmpty(news.Picture))
						{
							string oldPicturePath = Path.Combine(UploadPath, news.Picture);
							if (System.IO.File.Exists(oldPicturePath))
							{
								System.IO.File.Delete(oldPicturePath);
							}
						}
					}								
					news.Picture = filename;
					news.AdminId = acc.Id;
					news.TitleChange = MySecurity.RemoveDiacritics(news.Title);
					news.DateModified = DateTime.Now;
					db.Entry(news).State = EntityState.Modified;
					await db.SaveChangesAsync();
					ViewBag.Message = "Sửa thành công!";
					return RedirectToAction("Index", new { message = ViewBag.Message, page = page });

				}
				catch (Exception ex)
				{
				// Ghi log lỗi và thiết lập thông báo lỗi
				System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo nhân viên: {ex.Message}");
				ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
			}
		}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new
			{
				message = ViewBag.Message,
				page = page
			});
		}

		// GET: Admin/News/Delete/5
		public async Task<ActionResult> Delete(int? id, int? page)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
			ViewBag.page = page;
			return PartialView(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id, int? page)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}

			News news = await db.News.FindAsync(id);
			// Xóa ảnh cũ nếu có
			if (!string.IsNullOrEmpty(news.Picture))
			{
				string oldPicturePath = Path.Combine(UploadPath, news.Picture);
				if (System.IO.File.Exists(oldPicturePath))
				{
					System.IO.File.Delete(oldPicturePath);
				}
			}
			db.News.Remove(news);   
			await db.SaveChangesAsync();
			ViewBag.Message = "Xóa thành công!";
			return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
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
