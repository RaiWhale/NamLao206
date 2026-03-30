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
using NamLao206.Areas.TransportFiles.Bussiness;
using NamLao206.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;


namespace NamLao206.Areas.TransportFiles.Controllers
{
    [Authorize]
    public class TransportFilesController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
		int pageSize = 50;
        // GET: TransportFiles/TransportFiles
        public ActionResult HopThu(int? page, string search, string message, string loai)
        {
			if (string.IsNullOrWhiteSpace(loai))  loai = "van-ban-den"; 
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
                IQueryable<TransportFile> transportFiles = db.TransportFiles
						.Where(x => x.Transports.Any(t => t.ReceiverUserId == acc.EmployeeId));
       
                 // Xử lý theo loại
                switch (loai)
                {
                    case "van-ban-den":
                    case "1":
                        ViewBag.TieuDe = "Văn Bản Đến";
                        // Văn bản đến: người dùng là người nhận
                        transportFiles = transportFiles.Where(x => 
									x.Transports.Any(t => t.ReceiverUserId == acc.EmployeeId) 
									&& x.IsActive == true);
                        break;

                    case "van-ban-di":
                    case "2":
                        ViewBag.TieuDe = "Văn Bản Đi";
                        // Văn bản đi: người dùng là người tạo
                        transportFiles = transportFiles.Where(x => 
									x.CreateUserId == userId && x.IsActive == true);
                        break;

                    case "cong-van-khan":
                    case "3":
                        ViewBag.TieuDe = "Công văn khẩn";
                        // Công văn khẩn: người nhận và có flag khẩn cấp
                        transportFiles = transportFiles.Where(x => 
									x.Transports.Any(t => t.ReceiverUserId == acc.EmployeeId)
									&& x.KhanCap == true
									&& x.IsActive == true);
                        break;

                    case "thung-rac":
                    case "4":
                        ViewBag.TieuDe = "Thùng rác";
                        // Thùng rác: các bản ghi đã xóa (IsActive = false)
                        transportFiles = transportFiles.Where(x =>
                                    x.Transports.Any(t => t.ReceiverUserId == acc.EmployeeId)
                                    && x.IsActive == false);
                        break;

                    default:
                        ViewBag.TieuDe = "Tất cả văn bản";
                        // Mặc định: lấy tất cả văn bản đến
                        transportFiles = transportFiles.Where(x =>
                                     x.Transports.Any(t => t.ReceiverUserId == acc.EmployeeId)
                                     && x.IsActive == true);
                        break;
                }
                // Áp dụng tìm kiếm nếu có
                if (!string.IsNullOrWhiteSpace(search))
                {
                    string searchLower = search.Trim().ToLower();
                    transportFiles = transportFiles.Where(x => x.Mota.ToLower().Contains(searchLower));
                }

                // Sắp xếp và lấy danh sách
                transportFiles = transportFiles.OrderByDescending(x => x.CreateDate);

                // Gán thông báo (nếu có)
                ViewBag.ThuChuaXem = db.Transports
					.Where(x => x.ReceiverUserId == acc.EmployeeId 
										&& x.DaXem == false)
					.OrderByDescending(x => x.TransportDate).ToList().Count();
                ViewBag.ThuChuaXemKhan = db.Transports
                    .Where(x => x.ReceiverUserId == acc.EmployeeId
                                      && x.TransportFile.KhanCap == true
                                      && x.IsActive == true
									  && x.DaXem == false)
                    .OrderByDescending(x => x.TransportDate).ToList().Count();
                ViewBag.LoaiHienTai = loai;
                ViewBag.Search = search;
                ViewBag.Message = message;
                int pageNumber = page ?? 1;
                return View(transportFiles.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nên sử dụng logging framework như Serilog)
                System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy danh sách Transport: {ex.Message}");
                ViewBag.Message = "Đã xảy ra lỗi khi lấy danh sách. Vui lòng thử lại.";
                return View(new List<Transport>());
            }
        }

        public ActionResult HopThuDen(int? page, string search, string message)
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
					.Where(x => x.ReceiverUserId == acc.EmployeeId && x.IsActive == true);

				// Áp dụng tìm kiếm nếu có
				if (!string.IsNullOrWhiteSpace(search))
				{
					string searchLower = search.Trim().ToLower();
					transports = transports.Where(x => x.TransportFile != null && x.TransportFile.Mota.ToLower().Contains(searchLower));
				}

				// Sắp xếp và lấy danh sách
				transports = transports.OrderByDescending(x => x.TransportFile.CreateDate);

				// Gán thông báo (nếu có)
				ViewBag.Message = message;
				int pageNumber = page ?? 1;
				return View(transports.ToPagedList(pageNumber, pageSize));
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy danh sách Transport: {ex.Message}");
				ViewBag.Message = "Đã xảy ra lỗi khi lấy danh sách. Vui lòng thử lại.";
				return View(new List<Transport>());
			}
        }

        public ActionResult HopThuDi(int? page, string search, string message)
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
					.Where(x => x.CreateUserId == userId && x.IsActive == true);

				// Áp dụng tìm kiếm nếu có
				if (!string.IsNullOrEmpty(search))
				{
					string searchLower = search.Trim().ToLower(); // Chuẩn hóa chuỗi tìm kiếm một lần
					transportFiles = transportFiles.Where(x => x.Mota.ToLower().Contains(searchLower));			
				}

				// Sắp xếp và lấy danh sách
				transportFiles = transportFiles.OrderByDescending(x => x.CreateDate);

				// Gán thông báo (nếu có)
				ViewBag.Message = message;
				int pageNumber = page ?? 1;
				return View(transportFiles.ToPagedList(pageNumber, pageSize));		
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy danh sách TransportFile: {ex.Message}");
				ViewBag.Message = "Đã xảy ra lỗi khi lấy danh sách. Vui lòng thử lại.";
				return View(new List<TransportFile>());
			}		
        }
		public ActionResult PheDuyet(int? page, string search, string message)
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
					.Where(x => (x.CreateUserId == userId || x.NguoiPheDuyetId == userId) && x.PheDuyet == false && x.IsActive == true);

				// Áp dụng tìm kiếm nếu có
				if (!string.IsNullOrEmpty(search))
				{
					string searchLower = search.Trim().ToLower(); // Chuẩn hóa chuỗi tìm kiếm một lần
					transportFiles = transportFiles.Where(x => x.Mota.ToLower().Contains(searchLower));
				}

				// Sắp xếp và lấy danh sách
				transportFiles = transportFiles.OrderByDescending(x => x.CreateDate);

				// Gán thông báo (nếu có)
				ViewBag.Message = message;
				int pageNumber = page ?? 1;
				return View(transportFiles.ToPagedList(pageNumber, pageSize));
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy danh sách TransportFile: {ex.Message}");
				ViewBag.Message = "Đã xảy ra lỗi khi lấy danh sách. Vui lòng thử lại.";
				return View(new List<TransportFile>());
			}		
        }
        public ActionResult CongVanKhan(int? page, string search, string message)
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
					.Where(x => x.ReceiverUserId == acc.EmployeeId && x.TransportFile.KhanCap == true && x.IsActive == true);

				// Áp dụng tìm kiếm nếu có
				if (!string.IsNullOrEmpty(search))
				{
					string searchLower = search.Trim().ToLower();
					transports = transports.Where(x => x.TransportFile != null && x.TransportFile.Mota.ToLower().Contains(searchLower));
				}

				// Sắp xếp và lấy danh sách
				transports = transports.OrderByDescending(x => x.TransportFile.CreateDate);


				// Gán thông báo (nếu có)
				ViewBag.Message = message;
				int pageNumber = page ?? 1;
				return View(transports.ToPagedList(pageNumber, pageSize));
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy danh sách Transport: {ex.Message}");
				ViewBag.Message = "Đã xảy ra lỗi khi lấy danh sách. Vui lòng thử lại.";
				return View(new List<Transport>());
			}		
        }
		public ActionResult ThungRac(int? page, string search, string message)
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
					.Where(x => x.ReceiverUserId == acc.EmployeeId && x.IsActive == false);

				// Áp dụng tìm kiếm nếu có
				if (!string.IsNullOrEmpty(search))
				{
					string searchLower = search.Trim().ToLower();
					transports = transports.Where(x => x.TransportFile != null && x.TransportFile.Mota.ToLower().Contains(searchLower));
				}

				// Sắp xếp và lấy danh sách
				transports = transports.OrderByDescending(x => x.TransportFile.CreateDate);

				// Gán thông báo (nếu có)
				ViewBag.Message = message;
				int pageNumber = page ?? 1;
				return View(transports.ToPagedList(pageNumber, pageSize));
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
		public async Task<ActionResult> Details(int? id, string flag, string loai)
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
				if ((!string.IsNullOrWhiteSpace(flag) && flag.Contains("HopThuDen")) || (!string.IsNullOrWhiteSpace(loai) && loai.Contains("van-ban-den")))
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
					ViewBag.Note = transport.Note ?? "";
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
				PopulateDropdowns();
				return View(transportFile);
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nên sử dụng logging framework như Serilog)
				System.Diagnostics.Debug.WriteLine($"Lỗi khi xem chi tiết TransportFile: {ex.Message}");
				return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Đã xảy ra lỗi khi xem chi tiết. Vui lòng thử lại.");
			}
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public async Task<ActionResult> Details([Bind(Include = "Id,CreateDate,Mota,NguoiPheDuyetId")] TransportFile transportFile,
			List<string> lstReceiverUserId,
			List<string> lstReceiverRoomId,
			List<string> lstReceiverUnitId,
			string customRadio_10, string Note)
		{
			try
			{
				// Kiểm tra và lấy thông tin người dùng
				if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int createUserId))
				{
					ModelState.AddModelError("", "Không thể xác định người dùng. Vui lòng đăng nhập lại.");
					PopulateDropdowns();
					return View(transportFile);
				}		
				// Sử dụng transaction để đảm bảo tính nhất quán
				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						// Cập nhật thông tin TransportFile
						var transportF = await db.TransportFiles
							.FirstOrDefaultAsync(x => x.Id == transportFile.Id);					
						transportF.ModifiedDate = DateTime.Now;
						transportF.ModifiedUserId = createUserId;						
						await db.SaveChangesAsync();
						// Xử lý danh sách người nhận dựa trên customRadio_10
						await ProcessReceivers(transportFile, lstReceiverUserId, lstReceiverRoomId, lstReceiverUnitId, customRadio_10, Note);

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

				return RedirectToAction("HopThu", new { message = ViewBag.Message });
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
        public async Task<ActionResult> ChuyenTiep(TransportFile transportFile,List<string> lstReceiverUserId, string Note)
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
							var transport = CreateTransport(transportFile, receiverUserId, Note);
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

			return RedirectToAction("HopThu", new { message = ViewBag.Message });
        }
        // GET: TransportFiles/TransportFiles/Create
        public ActionResult Create()
        {
			PopulateDropdowns();
			return View();
        }

        // POST: TransportFiles/TransportFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Id,tenFile,Mota,KhanCap,NgayBanHanh,NguoiPheDuyetId,SoTrang,DoMat")] TransportFile transportFile, 
            List<string> lstReceiverUserId, 
            List<string> lstReceiverRoomId, 
            List<string> lstReceiverUnitId, 
            string customRadio_10, string Note)
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
				// Gán NguoiPheDuyetId từ EmployeeId (giả sử đầu vào là EmployeeId)
				if (transportFile.NguoiPheDuyetId.HasValue)
				{
					var accountId = db.Accounts
						.Where(x => x.EmployeeId == transportFile.NguoiPheDuyetId)
						.Select(x => x.Id)
						.SingleOrDefault();

					if (accountId == 0)
					{
						throw new Exception("Không tìm thấy tài khoản tương ứng với NguoiPheDuyetId.");
					}

					transportFile.NguoiPheDuyetId = accountId;
				}

				// Kiểm tra khi PheDuyet = true
				if (transportFile.PheDuyet == true)
				{
					if (!transportFile.NguoiPheDuyetId.HasValue)
					{
						throw new Exception("Vui lòng chọn người phê duyệt.");
					}

					if (transportFile.NguoiPheDuyetId != createUserId)
					{
						throw new Exception("Người phê duyệt phải là người dùng hiện tại khi phê duyệt được bật.");
					}
				}

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
						await ProcessReceivers(transportFile, lstReceiverUserId, lstReceiverRoomId, lstReceiverUnitId, customRadio_10, Note);

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
				ViewBag.Message = "Thêm mới thành công!";
				return RedirectToAction("HopThu", new { message = ViewBag.Message });			
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
        public ActionResult Edit([Bind(Include = "Id,tenFile,url,CreateDate,ModifiedDate,CreateUserId,ModifiedUserId,NguoiPheDuyetId,IsActive,Mota")] TransportFile transportFile)
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
                        if (acc.LevelId != 3)
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
			return RedirectToAction("HopThu", new { message = ViewBag.Message, loai = "van-ban-di" });
		}
		// Phương thức để lưu file đính kèm
		private async Task SaveAttachedFiles(TransportFile transportFile)
		{
			if (Request.Files.Count == 0) return;
			int file_count = 0;
			string dir = Path.Combine(Server.MapPath("~/Uploads/HopThu"), transportFile.Id.ToString());
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

			for (int i = 0; i < Request.Files.Count; i++)
			{
				try
				{
					HttpPostedFileBase file = Request.Files[i];
					if (file == null || file.ContentLength == 0) continue;
					if (!string.IsNullOrEmpty(file.FileName))
					{
						// Kiểm tra loại file và kích thước (ví dụ: chỉ cho phép PDF, tối đa 10MB)
						string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".rar", ".zip" };
						//int maxFileSize = 50 * 1024 * 1024; // 10MB
						string fileExtension = Path.GetExtension(file.FileName).ToLower();
						if (!allowedExtensions.Contains(fileExtension))
						{
							throw new Exception($"Loại file {fileExtension} không được phép. Chỉ cho phép: {string.Join(", ", allowedExtensions)}.");
						}
						//if (file.ContentLength > maxFileSize)
						//{
						//	throw new Exception($"Kích thước file vượt quá giới hạn ({maxFileSize / 1024 / 1024}MB).");
						//}
						string filename = $"{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
						string filePath = Path.Combine(dir, filename);
						file.SaveAs(filePath);

						db.TransportFileUrls.Add(new TransportFileUrl
						{
							Url = filename,
							TransportFilesId = transportFile.Id
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
		}
		// Phương thức để xử lý danh sách người nhận
		private async Task ProcessReceivers(
			TransportFile transportFile,
			List<string> lstReceiverUserId,
			List<string> lstReceiverRoomId,
			List<string> lstReceiverUnitId,
			string receiverType, string Note)
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
				transports.AddRange(userIds.Select(userId => CreateTransport(transportFile, userId, Note)));
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
				transports.AddRange(userIds.Select(userId => CreateTransport(transportFile, userId, Note)));

				// Thêm người dùng bổ sung (nếu có)
				if (lstReceiverUserId?.Any() == true)
				{
					var bonusUserIds = transports.Select(t => t.ReceiverUserId).ToHashSet();
					var additionalUserIds = lstReceiverUserId
						.Select(int.Parse)
						.Where(userId => !bonusUserIds.Contains(userId))
						.ToList();

					transports.AddRange(additionalUserIds.Select(userId => CreateTransport(transportFile, userId, Note)));
				}
			}
			else if (receiverType == "option1" && lstReceiverUserId?.Any() == true)
			{
				// Tạo Transport cho từng người dùng được chọn
				transports.AddRange(lstReceiverUserId
					.Select(int.Parse)
					.Select(userId => CreateTransport(transportFile, userId, Note)));
			}

			// Lấy danh sách userid đã có trong cơ sở dữ liệu cho transportFile.Id
			var existingUserIds = await db.Transports
				.Where(x => x.FileId == transportFile.Id && x.ReceiverUserId != null)
				.Select(x => x.ReceiverUserId.Value)
				.ToListAsync();

			// Lọc ra các Transport mới mà userid chưa có trong cơ sở dữ liệu
			var newTransports = transports
				.Where(t => !existingUserIds.Contains(t.ReceiverUserId.Value))
				.ToList();

			// Thêm tất cả Transport vào database
			if (newTransports.Any())
			{
				db.Transports.AddRange(newTransports);
				await db.SaveChangesAsync();
			}
		}
		private Transport CreateTransport(TransportFile transportFile, int receiverUserId, string Note)
		{
			return new Transport
			{
				FileId = transportFile.Id,
				TransportDate = DateTime.Now,
				ReceiverUserId = receiverUserId,
				Note = Note,
				DaXem = false,
				IsActive = true
			};
		
		}
		private void PopulateDropdowns()
		{
			ViewBag.Employees = new SelectList(db.Employees.Where(x => x.LevelId != 3 && x.LevelId != 4),"Id","Name");
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

				const string baseUrl = "https://www.namlao206.vn/Uploads/HopThu";
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
		public JsonResult DeleteJqueryHopThuDen(List<int> id)
		{
			try
			{
				// Validate input
				if (id == null || !id.Any())
				{
					return Json(new { success = false, message = "Không tìm thấy thư." });
				}

				// Get user ID from authenticated user
				if (!int.TryParse(User.Identity.Name, out int userId))
				{
					return Json(new { success = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại." });
				}

				var acc = db.Accounts.FirstOrDefault(x => x.Id == userId);
				if (acc == null)
				{
					return Json(new { success = false, message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên." });
				}

				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						var transportsToUpdate = db.Transports
							.Where(tf => id.Contains(tf.Id))
							.ToList();

						if (!transportsToUpdate.Any())
						{
							transaction.Rollback();
							return Json(new { success = false, message = "Chưa chọn thư để xóa." });
						}

						foreach (var transport in transportsToUpdate)
						{
							if (transport.IsActive == true)
							{
								transport.IsActive = false; // Soft delete by setting IsActive to false
							}
							else
							{
								db.Transports.Remove(transport); // Hard delete if already inactive
							}
						}

						db.SaveChanges();
						transaction.Commit();

						return Json(new { success = true, message = "Xóa thành công." });
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						// Log the exception (use your logging framework)
						System.Diagnostics.Debug.WriteLine($"Error in DeleteJquery: {ex.Message}");
						return Json(new { success = false, message = "Error during operation: " + ex.Message });
					}
				}
			}
			catch (Exception ex)
			{
				// Log the exception (use your logging framework)
				System.Diagnostics.Debug.WriteLine($"Error in DeleteJquery: {ex.Message}");
				return Json(new { success = false, message = "Unexpected error: " + ex.Message });
			}
		}
        public JsonResult DeleteJqueryHopThuDi(List<int> id)
        {
            try
            {
                // Validate input
                if (id == null || !id.Any())
                {
                    return Json(new { success = false, message = "Không tìm thấy thư." });
                }

                // Get user ID from authenticated user
                if (!int.TryParse(User.Identity.Name, out int userId))
                {
                    return Json(new { success = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại." });
                }

                var acc = db.Accounts.FirstOrDefault(x => x.Id == userId);
                if (acc == null)
                {
                    return Json(new { success = false, message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên." });
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var transportFilesToUpdate = db.TransportFiles.Include(tf => tf.Transports)
                            .Where(tf => id.Contains(tf.Id) && tf.IsActive == true)
                            .ToList();

                        if (!transportFilesToUpdate.Any())
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = "Chưa chọn thư để xóa." });
                        }

                        foreach (var transportFiles in transportFilesToUpdate)
                        {
                            if (transportFiles.IsActive == true)
                            {
                                transportFiles.IsActive = false; // Soft delete by setting IsActive to false
                            }
                            else
                            {
                                db.Transports.RemoveRange(transportFiles.Transports);
                                db.TransportFileUrls.Remove(db.TransportFileUrls.FirstOrDefault(x => x.TransportFilesId == transportFiles.Id));                            
                                db.TransportFiles.Remove(transportFiles); // Hard delete if already inactive
                            }
                        }

                        db.SaveChanges();
                        transaction.Commit();

                        return Json(new { success = true, message = "Xóa thành công." });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log the exception (use your logging framework)
                        System.Diagnostics.Debug.WriteLine($"Error in DeleteJquery: {ex.Message}");
                        return Json(new { success = false, message = "Error during operation: " + ex.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                System.Diagnostics.Debug.WriteLine($"Error in DeleteJquery: {ex.Message}");
                return Json(new { success = false, message = "Unexpected error: " + ex.Message });
            }
        }

        public JsonResult DeleteJqueryHopThu(List<int> id, string loai)
        {
            try
            {
                // Validate input
                if (id == null || !id.Any())
                {
                    return Json(new { success = false, message = "Không tìm thấy thư." });
                }

                // Get user ID from authenticated user
                if (!int.TryParse(User.Identity.Name, out int userId))
                {
                    return Json(new { success = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại." });
                }

                var acc = db.Accounts.FirstOrDefault(x => x.Id == userId);
                if (acc == null)
                {
                    return Json(new { success = false, message = "Tài khoản không tồn tại hoặc không liên kết với nhân viên." });
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool success = false;
                        string message = "";

                        switch (loai)
                        {
                            case "van-ban-den":
                            case "1":
                                success = EmailBussiness.DeleteVanBanDen(id, out message);
                                break;
                            case "van-ban-di":
                            case "2":
                                success = EmailBussiness.DeleteVanBanDi(id, out message);
                                break;
                            case "cong-van-khan":
                            case "3":
                                success = EmailBussiness.DeleteCongVanKhan(id, out message);
                                break;
                            case "thung-rac":
                            case "4":
                                success = EmailBussiness.DeleteFromThungRac(id, out message);
                                break;
                            default:
                                return Json(new { success = false, message = "Loại văn bản không hợp lệ." });
                        }

                        if (!success)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = message });
                        }

                  
                        transaction.Commit();

                        return Json(new { success = true, message = "Xóa thành công." });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        System.Diagnostics.Debug.WriteLine($"Error in DeleteJqueryHopThu: {ex.Message}");
                        return Json(new { success = false, message = "Lỗi trong quá trình xử lý: " + ex.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DeleteJqueryHopThu: {ex.Message}");
                return Json(new { success = false, message = "Lỗi không xác định: " + ex.Message });
            }
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
