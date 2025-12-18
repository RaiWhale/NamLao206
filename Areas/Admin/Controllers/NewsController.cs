using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


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
                search = search.Trim().ToLower();
                var keywords = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                news = news.Where(s =>
                        keywords.All(k =>
                            s.Title.Trim().ToLower().Contains(k)
                        )
                    ).OrderByDescending(x => x.DateUp);
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
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 2. Kiểm tra xem người dùng có quyền tạo tin tức hay không
                        var acc = db.Accounts
                        .Where(x => x.Id == userId)
                        .SingleOrDefault();
                        // Xử lý upload ảnh
                        string pictureFilename = string.Empty;
                        if (pic != null && pic.ContentLength > 0)
                        {
                            if (pic.ContentLength > 5 * 1024 * 1024) // Example: Limit to 5MB
                            {
                                ModelState.AddModelError("", "Ảnh quá lớn.");
                                return RedirectToAction("Index", new { page });
                            }
                            // Validate content type (e.g., image only)
                            if (!pic.ContentType.StartsWith("image/"))
                            {
                                ModelState.AddModelError("", "Chỉ chấp nhận file ảnh.");
                                return RedirectToAction("Index", new { page });
                            }

                            pictureFilename = $"{DateTime.Now.Ticks}_{Path.GetFileName(pic.FileName)}";
                            string picturesPath = Server.MapPath("~/Uploads/News/Pictures");
                            Directory.CreateDirectory(picturesPath); // Creates if not exists
                            pic.SaveAs(Path.Combine(picturesPath, pictureFilename));
                        }

                        news.TitleChange = MySecurity.RemoveDiacritics(news.Title);
                        news.Duyet = true;
                        news.Picture = pictureFilename;
                        news.AdminId = acc.Id;

                        db.News.Add(news);
                        await db.SaveChangesAsync(); // Save news first to get its ID

                        // Handle multiple files (excluding pic if it's in Request.Files)
                        int fileCount = 0;
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName) && file != pic) // Skip pic if duplicate
                            {
                                // Similar validations as pic (size, type - e.g., PDF only?)
                                if (file.ContentLength > 10 * 1024 * 1024 || !file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                                {
                                    continue; // Or throw error
                                }

                                var storageFile = new StorageFile(); // New instance per file
                                string filesDir = Path.Combine(Server.MapPath("~/Uploads/News/Files"), news.Id.ToString()); // Use news.Id (now set)
                                Directory.CreateDirectory(filesDir);

                                string fileFilename = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
                                file.SaveAs(Path.Combine(filesDir, fileFilename));

                                storageFile.TenFile = file.FileName;
                                storageFile.LoaiFile = file.ContentType;
                                storageFile.News_Id = news.Id; // Now correct
                                storageFile.CreateDate = DateTime.Now;
                                storageFile.CreateUser_Id = acc.Id;
                                storageFile.IsActive = true;
                                storageFile.TenFile_Phu = MySecurity.RemoveDiacritics(fileFilename);
                                storageFile.Url = $"/Uploads/News/Files/{news.Id}/{fileFilename}";
                                db.StorageFiles.Add(storageFile);
                                fileCount++;
                            }
                        }

                        if (fileCount > 0)
                        {
                            await db.SaveChangesAsync();
                        }

                        transaction.Commit();
                        ViewBag.Message = "Tạo mới thành công!";
                        return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo nhân viên: {ex.Message}");
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
                        return RedirectToAction("Index", new { page });
                    }
              
                }
             
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
        }
		// GET: Admin/News/Edit/5
        public async Task<ActionResult> Edit(int? id, int? page, string message)
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
                // Load existing news để keep old values nếu cần (ví dụ: Picture cũ)
                using (var transaction = db.Database.BeginTransaction())
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
                            if (pic.ContentLength > 5 * 1024 * 1024)  // Giới hạn 5MB như Create
                            {
                                ModelState.AddModelError("", "Ảnh quá lớn.");
                                return RedirectToAction("Index", new { page });
                            }
                            if (!pic.ContentType.StartsWith("image/"))  // Chỉ image
                            {
                                ModelState.AddModelError("", "Chỉ chấp nhận file ảnh.");
                                return RedirectToAction("Index", new { page });
                            }

                            string pictureFilename = $"{DateTime.Now.Ticks}_{Path.GetFileName(pic.FileName)}";
                            string picturesPath = Server.MapPath("~/Uploads/News/Pictures");  // Đồng bộ path với Create (thay ~/Content/Uploads/News)
                            Directory.CreateDirectory(picturesPath);

                            pic.SaveAs(Path.Combine(picturesPath, pictureFilename));

                            // Xóa ảnh cũ nếu có
                            if (!string.IsNullOrEmpty(news.Picture))
                            {
                                string oldPicturePath = Path.Combine(picturesPath, news.Picture);
                                if (System.IO.File.Exists(oldPicturePath))
                                {
                                    System.IO.File.Delete(oldPicturePath);
                                }
                            }
                            news.Picture = $"/Uploads/News/Files/{news.Id}/{pictureFilename}";
                        }
                        news.AdminId = acc.Id;
                        news.TitleChange = MySecurity.RemoveDiacritics(news.Title);
                        news.DateModified = DateTime.Now;
                        db.Entry(news).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        // Handle multiple files (tương tự Create, nhưng cho Edit: add new hoặc update existing nếu cần)
                        int fileCount = 0;
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName) && file != pic)  // Skip pic nếu duplicate
                            {
                                if (file.ContentLength > 10 * 1024 * 1024 || !file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;  // Hoặc add error
                                }

                                var storageFile = new StorageFile();  // New instance per file
                                string filesDir = Path.Combine(Server.MapPath("~/Uploads/News/Files"), news.Id.ToString());
                                Directory.CreateDirectory(filesDir);

                                string fileFilename = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
                                file.SaveAs(Path.Combine(filesDir, fileFilename));

                                storageFile.TenFile = fileFilename;
                                storageFile.LoaiFile = file.ContentType;
                                storageFile.News_Id = news.Id;
                                storageFile.CreateDate = DateTime.Now;
                                storageFile.CreateUser_Id = acc.Id;
                                storageFile.IsActive = true;
                                storageFile.TenFile_Phu = MySecurity.RemoveDiacritics(fileFilename);
                                storageFile.Url = $"/Uploads/News/Files/{news.Id}/{fileFilename}";
                                db.StorageFiles.Add(storageFile);
                                fileCount++;
                            }
                        }

                        if (fileCount > 0)
                        {
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            await db.SaveChangesAsync();  // Save nếu không có files mới
                        }

                        transaction.Commit();
                        TempData["Message"] = "Sửa thành công!";
                        return RedirectToAction("Index", new { page });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Cleanup uploaded files on error (implement helper to delete new pic/files)

                        // Logging
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi sửa tin tức: {ex.Message}");
                        // Hoặc dùng ILogger

                        TempData["Message"] = "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.";
                        return RedirectToAction("Index", new { page });
                    }
                }
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
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
            var files = db.StorageFiles.Where(x => x.News_Id == news.Id).ToList();
            foreach (var file in files)
            {
                // Xóa file vật lý
                string filePath = Server.MapPath(file.Url);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                db.StorageFiles.Remove(file); // Xóa record DB
            }
            db.News.Remove(news);
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
        }

        public async Task<ActionResult> DeleteFile(int? id)
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
           StorageFile storageFile = await db.StorageFiles.FindAsync(id);
            if (storageFile == null)
            {
                return HttpNotFound();
            }
        
            return PartialView(storageFile);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("DeleteFile")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFileConfirmed(int id)
        {
            // 1. Kiểm tra xác thực người dùng
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
            {
                ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }

            StorageFile storageFile = await db.StorageFiles.FindAsync(id);
            int newsId = storageFile.News_Id ?? 0;


            // Xóa file vật lý
            string filePath = Server.MapPath(storageFile.Url);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            db.StorageFiles.Remove(storageFile); // Xóa record DB
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("Edit", new { id = newsId, message = ViewBag.Message});
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
