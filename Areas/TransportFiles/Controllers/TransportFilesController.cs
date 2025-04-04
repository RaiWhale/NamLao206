using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;
using Microsoft.Win32;
using NamLao206.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using static System.Net.WebRequestMethods;

namespace NamLao206.Areas.TransportFiles.Controllers
{
    [Authorize]
    public class TransportFilesController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: TransportFiles/TransportFiles
        public ActionResult HopThuDen(string search, string message)
        {
			try
			{
				// Kiểm tra và lấy thông tin người dùng
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

				// Truy vấn danh sách Transport với các bảng liên quan
				IQueryable<Transport> transports = db.Transports
					.Include(x => x.TransportFile) // Tải trước TransportFile để tránh N+1 query
					.Where(x => x.ReceiverUserId == acc.EmployeeId);

				// Áp dụng tìm kiếm nếu có
				if (!string.IsNullOrEmpty(search))
				{
					string searchLower = search.Trim().ToLower(); // Chuẩn hóa chuỗi tìm kiếm một lần
					transports = transports.Where(x => x.TransportFile.tenFile.ToLower().Contains(searchLower));
				}

				// Sắp xếp và lấy danh sách
				var transportList = transports
					.OrderByDescending(x => x.TransportFile.CreateDate)
					.ToList();

				// Gán thông báo (nếu có)
				ViewBag.Message = message;

				return View(transportList);
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy danh sách Transport: {ex.Message}");
				ViewBag.Message = "Đã xảy ra lỗi khi lấy danh sách. Vui lòng thử lại.";
				return View(new List<Transport>());
			}
        }

        public ActionResult HopThuDi(string search, string message)
        {
			try
			{
				// Kiểm tra và lấy thông tin người dùng
				if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
				{
					ViewBag.Message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
					return RedirectToAction("Login", "Login", new { area = "" });
				}

				// Truy vấn danh sách TransportFile
				IQueryable<TransportFile> transportFiles = db.TransportFiles
					.Where(x => x.CreateUserId == userId);

				// Áp dụng tìm kiếm nếu có
				if (!string.IsNullOrEmpty(search))
				{
					string searchLower = search.Trim().ToLower(); // Chuẩn hóa chuỗi tìm kiếm một lần
					transportFiles = transportFiles.Where(x => x.tenFile.ToLower().Contains(searchLower));
				}

				// Sắp xếp và lấy danh sách
				var transportFilesList = transportFiles
					.OrderByDescending(x => x.CreateDate)
					.ToList();

				// Gán thông báo (nếu có)
				ViewBag.Message = message;

				return View(transportFilesList);
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy danh sách TransportFile: {ex.Message}");
				ViewBag.Message = "Đã xảy ra lỗi khi lấy danh sách. Vui lòng thử lại.";
				return View(new List<TransportFile>());
			}		
        }
        public ActionResult CongVanKhan(string search, string message)
        {
			try
			{
				// Kiểm tra và lấy thông tin người dùng
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

				// Truy vấn danh sách Transport với các bảng liên quan
				IQueryable<Transport> transports = db.Transports
					.Include(x => x.TransportFile) // Tải trước TransportFile để tránh N+1 query
					.Where(x => x.ReceiverUserId == acc.EmployeeId && x.TransportFile.KhanCap == true);

				// Áp dụng tìm kiếm nếu có
				if (!string.IsNullOrEmpty(search))
				{
					string searchLower = search.Trim().ToLower(); // Chuẩn hóa chuỗi tìm kiếm một lần
					transports = transports.Where(x => x.TransportFile.tenFile.ToLower().Contains(searchLower));
				}

				// Sắp xếp và lấy danh sách
				var transportList = transports
					.OrderByDescending(x => x.TransportFile.CreateDate)
					.ToList();

				// Gán thông báo (nếu có)
				ViewBag.Message = message;

				return View(transportList);
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy danh sách Transport: {ex.Message}");
				ViewBag.Message = "Đã xảy ra lỗi khi lấy danh sách. Vui lòng thử lại.";
				return View(new List<Transport>());
			}		
        }
		// GET: TransportFiles/TransportFiles/Details/5
		public async Task<ActionResult> Details(int? id, string flag)
		{
			// Kiểm tra id hợp lệ
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// Lấy TransportFile với các bảng liên quan
			var transportFile = await db.TransportFiles
				.Include(x => x.Transports) // Tải trước Transports nếu cần trong view
				.FirstOrDefaultAsync(x => x.Id == id);

			if (transportFile == null)
			{
				return HttpNotFound();
			}

			try
			{
				// Cập nhật trạng thái DaXem nếu flag chứa "HopThuDen"
				if (!string.IsNullOrEmpty(flag) && flag.Contains("HopThuDen"))
				{
					// Kiểm tra và lấy thông tin người dùng
					if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userId))
					{
						return RedirectToAction("Login", "Login", new { area = "" });
					}

					// Lấy thông tin tài khoản
					var acc = await db.Accounts
						.FirstOrDefaultAsync(x => x.Id == userId);

					if (acc == null)
					{
						return RedirectToAction("Login", "Login", new { area = "" });
					}

					// Cập nhật trạng thái DaXem
					var transport = await db.Transports
						.FirstOrDefaultAsync(x => x.FileId == id && x.ReceiverUserId == acc.EmployeeId);

					if (transport != null)
					{
						transport.DaXem = true;
						await db.SaveChangesAsync();
					}
				}

				// Lấy danh sách người nhận
				var transports = await db.Transports
					.Where(x => x.FileId == id)
					.Select(x => x.ReceiverUserId)
					.ToListAsync();

				if (transports.Any())
				{
					// Lấy danh sách tên người nhận một lần duy nhất
					var receiverUserIds = transports.ToHashSet();
					var receiverNames = await db.Employees
						.Where(x => receiverUserIds.Contains(x.Id))
						.Select(x => x.Name)
						.ToListAsync();

					// Nối danh sách tên người nhận
					ViewBag.NguoiNhan = string.Join(",", receiverNames); // Loại bỏ dấu phẩy thừa
				}
				else
				{
					ViewBag.NguoiNhan = "Không có người nhận.";
				}

				return View(transportFile);
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi xem chi tiết TransportFile: {ex.Message}");
				return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Đã xảy ra lỗi khi xem chi tiết. Vui lòng thử lại.");
			}
		}
		
        // GET: TransportFiles/TransportFiles/Details/5
        public ActionResult ChuyenTiep(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportFile transportFiles = db.TransportFiles.Find(id);

            if (transportFiles == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employees = new SelectList(db.Employees, "Id", "Name");
            return PartialView(transportFiles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChuyenTiep(TransportFile transportFile,List<string> lstReceiverUserId)
        {
			// Xử lý danh sách người nhận (lstReceiverUserId)
			if (lstReceiverUserId?.Any() == true) // Kiểm tra danh sách không null và không rỗng
			{
				try
				{
					// Lấy danh sách ReceiverUserId đã tồn tại cho transportFile.Id
					var existingReceiverUserIds = db.Transports
						.Where(x => x.FileId == transportFile.Id)
						.Select(x => x.ReceiverUserId)
						.ToList()
						.ToHashSet(); // Sử dụng HashSet để tìm kiếm nhanh

					// Lọc danh sách người nhận để chỉ thêm những người chưa tồn tại
					var newTransports = new List<Transport>();
					foreach (var item in lstReceiverUserId)
					{
						// Kiểm tra item có phải là số nguyên hợp lệ không
						if (!int.TryParse(item, out int receiverUserId))
						{
							// Ghi log lỗi (nên sử dụng logging framework như Serilog)
							System.Diagnostics.Debug.WriteLine($"Giá trị ReceiverUserId không hợp lệ: {item}");
							continue; // Bỏ qua giá trị không hợp lệ
						}

						// Kiểm tra trùng lặp
						if (!existingReceiverUserIds.Contains(receiverUserId))
						{
							var transport = CreateTransport(transportFile, receiverUserId);
							newTransports.Add(transport);
							existingReceiverUserIds.Add(receiverUserId); // Cập nhật HashSet để tránh trùng lặp trong cùng batch
						}
					}

					// Thêm các bản ghi Transport mới
					if (newTransports.Any())
					{
						db.Transports.AddRange(newTransports);
						await db.SaveChangesAsync(); // Sử dụng async để tối ưu hiệu suất
					}
				}
				catch (Exception ex)
				{
					// Ghi log lỗi
					System.Diagnostics.Debug.WriteLine($"Lỗi khi thêm Transport: {ex.Message}");
					ViewBag.Message = ex.Message;
					throw; // Ném lại ngoại lệ để transaction chính (nếu có) xử lý
				}
			}

			return RedirectToAction("HopThuDen", new { message = ViewBag.Message });
        }
        // GET: TransportFiles/TransportFiles/Create
        public ActionResult Create()
        {
            ViewBag.Employees = new SelectList(db.Employees, "Id", "Name");
            ViewBag.DM_NhomPhongBans = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa");
            ViewBag.DM_PhongBans = new SelectList(db.DM_PhongBans, "Id", "TenKhoa");
            ViewBag.DM_DonVis = new SelectList(db.DM_DonVis, "Id", "TenDonVi");
            return View();
        }

        // POST: TransportFiles/TransportFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,tenFile,Mota,KhanCap,NgayBanHanh,NguoiPheDuyetId,SoTrang,DoMat")] TransportFile transportFile, 
            List<string> lstReceiverUserId, 
            List<string> lstReceiverRoomId, 
            List<string> lstReceiverUnitId, 
            string customRadio_10 )
        {
			if (!ModelState.IsValid)
			{
				PopulateDropdowns();
				return View(transportFile);
			}
			try
            {
				// Kiểm tra và lấy thông tin người dùng
				if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int createUserId))
				{
					ModelState.AddModelError("", "Không thể xác định người dùng. Vui lòng đăng nhập lại.");
					PopulateDropdowns();
					return View(transportFile);
				}
				// Cấu hình TransportFile
				transportFile.CreateDate = DateTime.Now;
				transportFile.CreateUserId = createUserId;
				transportFile.NguoiPheDuyetId = db.Accounts
					.Where(x => x.EmployeeId == transportFile.NguoiPheDuyetId)
					.Select(x => x.Id)
					.SingleOrDefault();
				transportFile.IsActive = true;
				// Sử dụng transaction để đảm bảo tính nhất quán
				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						// Thêm TransportFile
						db.TransportFiles.Add(transportFile);
						await db.SaveChangesAsync();

						// Xử lý file đính kèm
						await SaveAttachedFiles(transportFile);

						// Xử lý danh sách người nhận dựa trên customRadio_10
						await ProcessReceivers(transportFile, lstReceiverUserId, lstReceiverRoomId, lstReceiverUnitId, customRadio_10);

						// Commit transaction
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						// Ghi log lỗi (nên sử dụng logging framework như Serilog)
						System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo TransportFile: {ex.Message}");
						//ModelState.AddModelError("", "Đã xảy ra lỗi khi tạo hồ sơ. Vui lòng thử lại.");
						PopulateDropdowns();
						ViewBag.Message = ex.Message;
						return View(transportFile);
					}
				}

				return RedirectToAction("HopThuDen");
			}

			catch (Exception ex)
			{
				// Ghi log lỗi
				System.Diagnostics.Debug.WriteLine($"Lỗi không xác định: {ex.Message}");
				//ModelState.AddModelError("", "Đã xảy ra lỗi không xác định. Vui lòng thử lại.");
				PopulateDropdowns();
				ViewBag.Message = ex.Message;
				return View(transportFile);
			}
        }

        // GET: TransportFiles/TransportFiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportFile transportFile = db.TransportFiles.Find(id);
            if (transportFile == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.ModifiedUserId);
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.CreateUserId);
            return View(transportFile);
        }

        // POST: TransportFiles/TransportFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,tenFile,url,CreateDate,ModifiedDate,CreateUserId,ModifiedUserId,IsActive,Mota")] TransportFile transportFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transportFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ModifiedUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.ModifiedUserId);
            ViewBag.CreateUserId = new SelectList(db.Accounts, "Id", "LoginName", transportFile.CreateUserId);
            return View(transportFile);
        }

        // GET: TransportFiles/TransportFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportFile transportFile = db.TransportFiles.Find(id);
            if (transportFile == null)
            {
                return HttpNotFound();
            }
            return PartialView(transportFile);
        }

        // POST: TransportFiles/TransportFiles/Delete/5
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
                        // Kiểm tra điều kiện xóa
                        if (acc.AccountType != 1)
                        {
                            // Tính thời gian tạo file
                            var timeElapsed = DateTime.Now - transportFile.CreateDate;

                            // Chặn xóa nếu quá 24h
                            if (timeElapsed.TotalHours > 24)
                            {
                                transaction.Rollback();
                                ViewBag.Message = "Bạn chỉ có thể xóa thư trong vòng 24 giờ!";
                                return RedirectToAction("HopThuDi", new { message = ViewBag.Message });
                            }
                        }

                        // Thực hiện xóa
                        db.Transports.RemoveRange(transportFile.Transports);
                        db.TransportFileUrls.Remove(db.TransportFileUrls.FirstOrDefault(x => x.TransportFilesId == id));
						db.TransportFiles.Remove(transportFile);
						db.SaveChanges();					
						string dir = Server.MapPath("~/Content/Uploads/HopThu") + "\\" + transportFile.Id;
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
			return RedirectToAction("HopThuDi", new { message = ViewBag.Message });
		}
		// Phương thức để lưu file đính kèm
		private async Task SaveAttachedFiles(TransportFile transportFile)
		{
			if (Request.Files.Count == 0) return;
	
			string dir = Path.Combine(Server.MapPath("~/Content/Uploads/HopThu"), transportFile.Id.ToString());
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			foreach (string fileKey in Request.Files)
			{
				HttpPostedFileBase file = Request.Files[fileKey];
				if (file == null || file.ContentLength == 0) continue;

				// Kiểm tra loại file và kích thước (ví dụ: chỉ cho phép PDF, tối đa 10MB)
				string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".rar", ".zip" };
				int maxFileSize = 50 * 1024 * 1024; // 10MB
				string fileExtension = Path.GetExtension(file.FileName).ToLower();
				if (!allowedExtensions.Contains(fileExtension))
				{
					throw new Exception($"Loại file {fileExtension} không được phép. Chỉ cho phép: {string.Join(", ", allowedExtensions)}.");
				}
				if (file.ContentLength > maxFileSize)
				{
					throw new Exception($"Kích thước file vượt quá giới hạn ({maxFileSize / 1024 / 1024}MB).");
				}

				string filename = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
				string filePath = Path.Combine(dir, filename);
				file.SaveAs(filePath);

				var transportFileUrl = new TransportFileUrl
				{
					Url = filename,
					TransportFilesId = transportFile.Id
				};
				db.TransportFileUrls.Add(transportFileUrl);
				await db.SaveChangesAsync();
			}
		}
		// Phương thức để xử lý danh sách người nhận
		private async Task ProcessReceivers(
			TransportFile transportFile,
			List<string> lstReceiverUserId,
			List<string> lstReceiverRoomId,
			List<string> lstReceiverUnitId,
			string receiverType)
		{
			var transports = new List<Transport>();

			// Xử lý dựa trên loại người nhận (receiverType)
			if (receiverType == "option3" && lstReceiverUnitId?.Any() == true)
			{
				// Lấy tất cả phòng thuộc các đơn vị được chọn
				var rooms = await db.DM_PhongBans
					.Where(x => lstReceiverUnitId.Contains(x.donvi_Id.ToString()))
					.Select(x => x.Id)
					.ToListAsync();

				// Lấy tất cả người dùng thuộc các phòng
				var userIds = await db.Accounts
					.Where(x => rooms.Contains(x.Employee.KhoaphongId ?? 0))
					.Select(x => x.EmployeeId)
					.Distinct()
					.ToListAsync();

				// Tạo Transport cho từng người dùng
				transports.AddRange(userIds.Select(userId => CreateTransport(transportFile, userId)));
			}
			else if (receiverType == "option2" && lstReceiverRoomId?.Any() == true)
			{
				// Lấy tất cả người dùng thuộc các phòng được chọn
				var userIds = await db.Accounts
					.Where(x => lstReceiverRoomId.Contains(x.Employee.KhoaphongId.ToString()))
					.Select(x => x.EmployeeId)
					.Distinct()
					.ToListAsync();

				// Tạo Transport cho từng người dùng
				transports.AddRange(userIds.Select(userId => CreateTransport(transportFile, userId)));

				// Thêm người dùng bổ sung (nếu có)
				if (lstReceiverUserId?.Any() == true)
				{
					var existingUserIds = transports.Select(t => t.ReceiverUserId).ToHashSet();
					var additionalUserIds = lstReceiverUserId
						.Select(int.Parse)
						.Where(userId => !existingUserIds.Contains(userId))
						.ToList();

					transports.AddRange(additionalUserIds.Select(userId => CreateTransport(transportFile, userId)));
				}
			}
			else if (receiverType == "option1" && lstReceiverUserId?.Any() == true)
			{
				// Tạo Transport cho từng người dùng được chọn
				transports.AddRange(lstReceiverUserId
					.Select(int.Parse)
					.Select(userId => CreateTransport(transportFile, userId)));
			}

			// Thêm tất cả Transport vào database
			if (transports.Any())
			{
				db.Transports.AddRange(transports);
				await db.SaveChangesAsync();
			}
		}
		private Transport CreateTransport(TransportFile transportFile, int receiverUserId)
		{
			return new Transport
			{
				FileId = transportFile.Id,
				TransportDate = transportFile.CreateDate,
				ReceiverUserId = receiverUserId,
				DaXem = false,
				IsActive = true
			};
		
		}
		private void PopulateDropdowns()
		{
			ViewBag.Employees = new SelectList(db.Employees, "Id", "Name");
			ViewBag.DM_NhomPhongBans = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa");
			ViewBag.DM_PhongBans = new SelectList(db.DM_PhongBans, "Id", "TenKhoa");
			ViewBag.DM_DonVis = new SelectList(db.DM_DonVis, "Id", "TenDonVi");
		}
		public async Task<ActionResult> XuatExcel(DateTime? tuNgay, DateTime? denNgay)
		{
			try
			{
				if (!tuNgay.HasValue || !denNgay.HasValue)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Vui lòng cung cấp khoảng thời gian hợp lệ.");
				}

				// Lấy danh sách TransportFiles
				var transportFiles = await db.TransportFiles
					.Include(x => x.Account2)
					.Include(x => x.Transports)
					.Include(x => x.TransportFileUrls)
					.Where(x => x.NgayBanHanh >= tuNgay && x.NgayBanHanh <= denNgay)
					.OrderByDescending(x => x.NgayBanHanh)
					.ToListAsync();

				if (!transportFiles.Any())
				{
					return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Không có dữ liệu để xuất Excel.");
				}

				// Lấy tất cả ReceiverUserId liên quan
				var allReceiverUserIds = transportFiles
					.SelectMany(tf => tf.Transports)
					.Select(t => t.ReceiverUserId)
					.Distinct()
					.ToList();

				// Lấy thông tin phòng ban và cơ quan nhận một lần duy nhất
				var receiverDetails = await (from e in db.Employees
											 join pb in db.DM_PhongBans on e.KhoaphongId equals pb.Id into phongBanGroup
											 from pb in phongBanGroup.DefaultIfEmpty()
											 join dv in db.DM_DonVis on pb.donvi_Id equals dv.Id into donViGroup
											 from dv in donViGroup.DefaultIfEmpty()
											 where allReceiverUserIds.Contains(e.Id)
											 select new
											 {
												 EmployeeId = e.Id,
												 PhongBan = pb != null ? pb.TenKhoa : "N/A",
												 CoQuan = dv != null ? dv.TenDonVi : "N/A"
											 })
					.ToListAsync();

				// Tạo dictionary để tra cứu nhanh
				var receiverLookup = receiverDetails
					.GroupBy(r => r.EmployeeId)
					.ToDictionary(
						g => g.Key,
						g => new
						{
							PhongBan = string.Join(", ", g.Select(x => x.PhongBan).Distinct()),
							CoQuan = string.Join(", ", g.Select(x => x.CoQuan).Distinct())
						});

				const string baseUrl = "https://www.namlao206.vn/Content/Uploads/HopThu";
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using (var package = new ExcelPackage())
				{
					var worksheet = package.Workbook.Worksheets.Add("CongVanDen");
					worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells.Style.Font.Size = 13;

					worksheet.Cells["A1:J1"].Style.Font.Bold = true;
					worksheet.Cells["A1:J1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
					worksheet.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
					worksheet.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

					for (int col = 1; col <= 10; col++)
					{
						worksheet.Column(col).Style.Border.Top.Style = ExcelBorderStyle.Thin;
						worksheet.Column(col).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						worksheet.Column(col).Style.Border.Left.Style = ExcelBorderStyle.Thin;
						worksheet.Column(col).Style.Border.Right.Style = ExcelBorderStyle.Thin;
						worksheet.Column(col).AutoFit();
					}
					worksheet.Column(3).Style.Numberformat.Format = "dd-MM-yyyy";

					var headers = new[]
					{
				"STT", "Số công văn", "Ngày ban hành", "Số trang", "Trích yếu",
				"Người phê duyệt", "Độ mật", "Phòng nhận", "Cơ quan nhận", "Ghi chú"
			};
					for (int col = 1; col <= headers.Length; col++)
					{
						worksheet.Cells[1, col].Value = headers[col - 1];
					}
					worksheet.Cells["A1:J1"].AutoFitColumns();

					int row = 2;
					int stt = 1;
					foreach (var item in transportFiles)
					{
						// Lấy danh sách ReceiverUserId cho TransportFile hiện tại
						var receiverUserIds = item.Transports
							.Select(t => t.ReceiverUserId)
							.Distinct()
							.ToList();

						// Lấy thông tin phòng ban và cơ quan từ lookup
						var receivers = receiverUserIds
							.Where(id => id.HasValue && receiverLookup.ContainsKey(id.Value))
							.Select(id => receiverLookup[id.Value])
							.ToList();

						string phongBanList = string.Join(", ", receivers.Select(r => r.PhongBan).Distinct());
						string coQuanList = string.Join(", ", receivers.Select(r => r.CoQuan).Distinct());

						var rowData = new[]
						{
					stt.ToString(),
					item.tenFile ?? "N/A",
					item.NgayBanHanh.ToString(),
					item.SoTrang.ToString(),
					item.Mota ?? "N/A",
					item.Account2?.Employee?.Name ?? "N/A",
					item.DoMat ?? "N/A",
					phongBanList,
					coQuanList
						};

						for (int col = 1; col <= rowData.Length; col++)
						{
							worksheet.Cells[row, col].Value = rowData[col - 1];
						}

						var fileUrls = item.TransportFileUrls
							.Select((url, index) => new
							{
								Url = $"{baseUrl}/{item.Id}/{url.Url}",
								Display = $"File {index + 1}"
							})
							.ToList();

						int urlColumn = 10;
						foreach (var file in fileUrls)
						{
							if (Uri.TryCreate(file.Url, UriKind.Absolute, out Uri uri))
							{
								var hyperlink = new ExcelHyperLink(file.Url) { Display = file.Display };
								worksheet.Cells[row, urlColumn].Hyperlink = hyperlink;
								worksheet.Cells[row, urlColumn].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
								worksheet.Cells[row, urlColumn].Style.Font.UnderLine = true;
							}
							urlColumn++;
						}

						row++;
						stt++;
					}

					var stream = new MemoryStream();
					package.SaveAs(stream);
					stream.Position = 0;

					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachCongVanDen.xlsx");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Lỗi khi xuất Excel: {ex.Message}");
				return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Đã xảy ra lỗi khi xuất Excel. Vui lòng thử lại.");
			}
		}
		//public ActionResult XuatExcel(DateTime? tuNgay, DateTime? denNgay)
		//      {
		//          string lstPhongBan = "";
		//	string lstCoQuan = "";           
		//          int sttDic = 1;
		//          int STT = 1;
		//          int sttCell = 10;
		//          // Dữ liệu mẫu cần xuất ra Excel (thay thế bằng dữ liệu thực tế của bạn)    
		//          var transportFiles = db.TransportFiles.Where(x => x.NgayBanHanh >= tuNgay && x.NgayBanHanh <= denNgay).OrderByDescending(x=>x.NgayBanHanh).ToList();

		//          // For non-commercial use:
		//          ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

		//          // Tạo một gói Excel mới
		//          try
		//          {
		//              using (var package = new ExcelPackage())
		//              {
		//                  // Tạo một worksheet mới
		//                  var worksheet = package.Workbook.Worksheets.Add("CongVanDen");
		//                  // Định dạng chung

		//                  worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
		//                  worksheet.Cells.Style.Font.Size = 13;


		//                  // Định dạng tiêu đề (hàng 1)           
		//                  worksheet.Cells["A1:J1"].Style.Font.Bold = true;
		//                  worksheet.Cells["A1:J1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
		//                  // Đặt màu nền cho vùng A1:B10 thành màu vàng
		//                  worksheet.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
		//                  worksheet.Cells["A1:J1"].Style.Fill.SetBackground(System.Drawing.Color.Yellow);

		//                  // Định dạng viền cho tất cả các cột
		//                  int totalColumns = worksheet.Dimension.Columns;
		//                  for (int col = 1; col <= totalColumns; col++)
		//                  {
		//                      worksheet.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
		//                      worksheet.Column(col).Style.Border.BorderAround(ExcelBorderStyle.Thin);
		//                      worksheet.Column(col).Style.Border.Top.Style = ExcelBorderStyle.Thin;
		//                      worksheet.Column(col).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
		//                      worksheet.Column(col).Style.Font.Size = 13;
		//                      worksheet.Column(col).AutoFit();
		//                  }
		//                  worksheet.Column(3).Style.Numberformat.Format = "dd-MM-yyyy";
		//                  // Xác định tiêu đề các cột
		//                  worksheet.Cells["A1"].Value = "STT";
		//                  worksheet.Cells["B1"].Value = "Số công văn";
		//                  worksheet.Cells["C1"].Value = "Ngày ban hành";
		//                  worksheet.Cells["D1"].Value = "Số trang";
		//                  worksheet.Cells["E1"].Value = "Trích yếu";
		//                  worksheet.Cells["F1"].Value = "Người phê duyệt";
		//                  worksheet.Cells["G1"].Value = "Độ mật";
		//                  worksheet.Cells["H1"].Value = "Phòng nhận";
		//                  worksheet.Cells["I1"].Value = "Cơ quan nhận";
		//                  worksheet.Cells["J1"].Value = "Ghi chú";
		//                  worksheet.Cells["A1:J1"].AutoFitColumns();
		//                  // Điền dữ liệu vào worksheet
		//                  int row = 2;
		//                  foreach (var item in transportFiles)
		//                  {
		//				var lstReceiverId = db.Transports.Where(x => x.FileId == item.Id)
		//						 .Select(x => x.ReceiverUserId)
		//						 .ToList();
		//				var query = from receiverId in lstReceiverId
		//							join employee in db.Employees on receiverId equals employee.Id
		//							select new
		//							{
		//								PhongBan = employee.DM_PhongBans.TenKhoa,
		//								CoQuan = employee.DM_PhongBans.DM_DonVis.TenDonVi
		//							};								
		//				lstPhongBan = string.Join(",", query.Select(x => x.PhongBan).Distinct().ToList());
		//				lstCoQuan = string.Join(",", query.Select(x => x.CoQuan).Distinct().ToList());

		//				Dictionary<int, string> dicCells = new Dictionary<int, string>()
		//                  {
		//                      {1,STT.ToString()},
		//                      {2,item.tenFile},
		//                      {3,item.NgayBanHanh.ToString()},
		//                      {4,item.SoTrang.ToString()},
		//                      {5,item.Mota},
		//                      {6,item.Account2.Employee.Name},
		//                      {7,item.DoMat},
		//                      {8,lstPhongBan},
		//                      {9,lstCoQuan}
		//			};
		//                      List<string> lstURL = db.TransportFileUrls.Where(x => x.TransportFilesId == item.Id)
		//                          .Select(x => "https://www.namlao206.vn/Content/Uploads/HopThu" + "\\" + item.Id + "\\" + x.Url).ToList();


		//                      Dictionary<string, string> viewurl = new Dictionary<string, string>();

		//                      foreach (var url in lstURL)
		//                      {
		//                          sttDic = 1;
		//                          viewurl.Add(url, "File" + sttDic);
		//					sttDic++;
		//				}

		//				// Tạo liên kết và đặt tên hiển thị
		//				foreach (var entry in dicCells)
		//                      {
		//                          worksheet.Cells[row, entry.Key].Value = entry.Value;
		//                      }

		//                      foreach (var test in viewurl)
		//                      {                        
		//                          var hyperLink = new ExcelHyperLink(test.Key, UriKind.Absolute);
		//                          hyperLink.Display = test.Value;
		//                          worksheet.Cells[row, sttCell].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
		//                          worksheet.Cells[row, sttCell].Style.Font.UnderLine = true;
		//                          worksheet.Cells[row, sttCell].Hyperlink = hyperLink;
		//					sttCell++;
		//				}
		//                      sttCell = 10;
		//				row++;
		//                      STT++;
		//                  }
		//                  // Lưu vào bộ nhớ
		//                  using (var memoryStream = new MemoryStream())
		//                  {
		//                      package.SaveAs(memoryStream);
		//                      memoryStream.Position = 0;

		//                      return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSach.xlsx");
		//                  }
		//              }

		//          }
		//          catch (ObjectDisposedException ex)
		//          {
		//              // Xử lý lỗi, ví dụ: ghi log lỗi và trả về một thông báo lỗi cho người dùng
		//              return ViewBag.Message(ex.Message);
		//          }

		//      }
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
