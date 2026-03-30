using NamLao206.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class DM_PhongBansController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        private static string UploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", "KhoaPhongs");
        int pageSize = 10;
        // GET: Admin/DM_PhongBans
		public ActionResult Index(int? page, string search, string message)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
            IQueryable<DM_PhongBans> dM_Khoaphongs = db.DM_PhongBans.Include(d => d.DM_NhomPhongBans).Include(d => d.Picture);
			if (!string.IsNullOrEmpty(search))
			{
				dM_Khoaphongs = dM_Khoaphongs.Where(d => d.TenKhoa.Trim().ToLower().Contains(search.Trim().ToLower()) || d.ChucNang.Trim().ToLower().Contains(search.Trim().ToLower()));
				ViewBag.Search = search;
			}
			else
			{
				ViewBag.Search = "";
			}
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}
			dM_Khoaphongs = dM_Khoaphongs.OrderBy(x => x.Id);	
			//Paging
			int pageNumber = page ?? 1;
			ViewBag.Title = "Khoa / Phòng ban -";
            return View(dM_Khoaphongs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/DM_PhongBans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_PhongBans DM_PhongBans = db.DM_PhongBans.Find(id);
            if (DM_PhongBans == null)
            {
                return HttpNotFound();
            }
            return PartialView(DM_PhongBans);
        }

        // GET: Admin/DM_PhongBans/Create
		public ActionResult Create(int? page)
        {
            ViewBag.NhomKhoaId = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa");
            ViewBag.page = page;
            ViewBag.donvi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi");
            return View();
        }

        // POST: Admin/DM_PhongBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TenKhoa,ChucNang,NhomKhoaId,PictureId,Description,CreateDate,donvi_Id")] DM_PhongBans dM_Khoaphongs, int? page)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			ViewBag.Title = "Khoa / Phòng ban -";
			if (ModelState.IsValid)
			{
                try
                {
					// 2. Kiểm tra xem tên khoa đã tồn tại chưa
					var existingKhoa = db.DM_PhongBans.FirstOrDefault(k => k.TenKhoa.Trim().ToLower() == dM_Khoaphongs.TenKhoa.Trim().ToLower());
					if (existingKhoa == null)
					{
						dM_Khoaphongs.CreateDate = DateTime.Now;
						// 3. Nếu chưa tồn tại, thêm mới khoa
						db.DM_PhongBans.Add(dM_Khoaphongs);
						await db.SaveChangesAsync();
						if (Request.Files.Count > 0)
						{
							int file_count = 0;
							string uploadPath = Path.Combine(UploadPath, dM_Khoaphongs.Id.ToString());
							if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);// Creates directory if it doesn't exist
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
							for (int i = 0; i < Request.Files.Count; i++)
							{
                                try
                                {
                                    HttpPostedFileBase file = Request.Files[i];

                                    if (file == null || file.ContentLength == 0) continue;

                                    // Kiểm tra size
                                    if (file.ContentLength > 10 * 1024 * 1024)
                                    {
                                        ModelState.AddModelError("", $"File too large (max 10MB)");
                                        continue;
                                    }

                                    // Lấy và kiểm tra extension
                                    string extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                                    if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                                    {
                                        ModelState.AddModelError("", $"Invalid file type");
                                        continue;
                                    }

                                    // Tạo tên file AN TOÀN
                                    string safeFileName = Path.GetFileName(file.FileName) ?? "file";
                                    string baseName = Path.GetFileNameWithoutExtension(safeFileName);

                                    // Loại bỏ ký tự nguy hiểm
                                    baseName = new string(baseName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
                                    baseName = baseName.Length > 50 ? baseName.Substring(0, 50) : baseName;

                                    string fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{baseName}_{Guid.NewGuid():N}{extension}";
                                    string filePath = Path.Combine(uploadPath, fileName);

                                    // Lưu file
                                    using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
                                    {
                                        await file.InputStream.CopyToAsync(fileStream);
                                    }

                                    // Lưu DB
                                    db.Pictures.Add(new Models.Picture
                                    {
                                        Url = fileName,
                                        KhoaphongId = dM_Khoaphongs.Id
                                    });

                                    file_count++;
                                }
                                catch (Exception ex)
                                {
                                    // KHÔNG BAO GIỜ bỏ trống catch
                                    ModelState.AddModelError("", $"Upload error: {ex.Message}");
                                    // Logger.Error(ex, "Upload failed");
                                }
                            }
							if (file_count > 0)
							{
								await db.SaveChangesAsync();
							}
							ViewBag.Message = "Tạo mới thành công!";
							return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
						}
					}
				}
				catch (Exception ex)
				{
					// Ghi log lỗi và thiết lập thông báo lỗi
					System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo nhân viên: {ex.Message}");
					ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
				}
			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
		}

        // GET: Admin/DM_PhongBans/Edit/5
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
            DM_PhongBans dM_Khoaphongs = await db.DM_PhongBans.FindAsync(id);
            if (dM_Khoaphongs == null)
            {
                return HttpNotFound();
            }
			ViewBag.Title = "Khoa / Phòng ban -";
			ViewBag.page = page;
            ViewBag.NhomKhoaId = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa", dM_Khoaphongs.NhomKhoaId);
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Url", dM_Khoaphongs.PictureId);
            ViewBag.donvi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", dM_Khoaphongs.donvi_Id);
            return View(dM_Khoaphongs);
        }

        // POST: Admin/DM_PhongBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,TenKhoa,ChucNang,NhomKhoaId,PictureId,Description,donvi_Id,CreateDate")] DM_PhongBans dM_Khoaphongs, int? page)
        {
			// 1. Kiểm tra xác thực người dùng
			if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
			{
				ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
				return RedirectToAction("Login", "Account");
			}
			if (ModelState.IsValid)
            {
				try {
					db.Entry(dM_Khoaphongs).State = EntityState.Modified;
					await db.SaveChangesAsync();
					if (Request.Files.Count > 0)
					{
						int file_count = 0;
						string uploadPath = Path.Combine(UploadPath, dM_Khoaphongs.Id.ToString());
						if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);// Creates directory if it doesn't exist
						var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
						for (int i = 0; i < Request.Files.Count; i++)
						{
							try
							{
								HttpPostedFileBase file = Request.Files[i];
								if (!string.IsNullOrEmpty(file.FileName))
								{
									string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
									if (!allowedExtensions.Contains(extension))
									{
										ModelState.AddModelError("", $"File {file.FileName} has an invalid extension.");
										continue;
									}
									// Sanitize and generate unique file name
									string fileName = $"{DateTime.UtcNow.Ticks}_{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid().ToString("N").Substring(0, 8)}{extension}";
									string filePath = Path.Combine(uploadPath, fileName);

									// Save file asynchronously
									using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
									{
										await file.InputStream.CopyToAsync(fileStream);
									}
									db.Pictures.Add(new Models.Picture
									{
										Url = fileName,
										KhoaphongId = dM_Khoaphongs.Id
									});
									file_count++;
								}
							}
							catch { }
						}
						if (file_count > 0)
						{
							await db.SaveChangesAsync();
						}
						ViewBag.Message = "Sửa thành công!";
						return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
					}
				}
				catch (Exception ex)
				{
					// Ghi log lỗi và thiết lập thông báo lỗi
					System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo nhân viên: {ex.Message}");
					ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.");
				}

			}
			ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
			return RedirectToAction("Index", new { message = ViewBag.Message, page = page });
		}

        // GET: Admin/DM_PhongBans/Delete/5
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
            DM_PhongBans dM_Khoaphongs = await db.DM_PhongBans.FindAsync(id);
            if (dM_Khoaphongs == null)
            {
                return HttpNotFound();
            }
			ViewBag.page = page;
			return PartialView(dM_Khoaphongs);
        }

		// POST: Admin/DM_Khoaphongs/Delete/5
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
			// 2. Kiểm tra xem khoa có tồn tại không
			DM_PhongBans dM_Khoaphongs = await db.DM_PhongBans.FindAsync(id);
            db.DM_PhongBans.Remove(dM_Khoaphongs);
			string uploadPath = Path.Combine(UploadPath, dM_Khoaphongs.Id.ToString());		
            if (Directory.Exists(uploadPath))
            {
                Directory.Delete(uploadPath, true);
            }
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
