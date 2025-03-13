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
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;
using Microsoft.Win32;
using NamLao206.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;

namespace NamLao206.Areas.TransportFiles.Controllers
{
    [Authorize]
    public class TransportFilesController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: TransportFiles/TransportFiles
        public ActionResult HopThuDen(string search, string message)
        {
            IQueryable<Transport> transportFiles;
            var userId = int.Parse(User.Identity.Name);            
            var acc = db.Accounts.Where(x=>x.Id == userId).SingleOrDefault();
            //transportFiles = db.TransportFiles.Include(x=>x.Transports).Where(t => t.Transports.Any(x => x.ReceiverUserId == acc.EmployeeId));
            transportFiles = db.Transports.Where(x => x.ReceiverUserId == acc.EmployeeId);
            if (!string.IsNullOrEmpty(search))
            {
                transportFiles = transportFiles.Where(x => x.TransportFile.tenFile.Trim().ToLower().Contains(search.Trim().ToLower()));
            }

			//var transportFiles = db.TransportFiles.Where(x => x.Transports.Any(y => ("," + y.ReceiverUserId + ",").Contains("," + userId + ",")));            
			ViewBag.Message = message;
			return View(transportFiles.OrderByDescending(x => x.TransportFile.CreateDate).ToList());
        }

        public ActionResult HopThuDi(string search, string message)
        {
            IQueryable<TransportFile> transportFiles;
            var userId = int.Parse(User.Identity.Name);
            //transportFiles = db.TransportFiles.Where(x => x.CreateUserId == userId);
            transportFiles = db.TransportFiles.Where(x => x.CreateUserId == userId);
            if (!string.IsNullOrEmpty(search))
            {
                transportFiles = transportFiles.Where(x => x.tenFile.Trim().ToLower().Contains(search.Trim().ToLower()));
            }
            ViewBag.Message = message;
			return View(transportFiles.OrderByDescending(x => x.CreateDate).ToList());
        }
        public ActionResult CongVanKhan(string search)
        {
            IQueryable<Transport> transportFiles;
            var userId = int.Parse(User.Identity.Name);
            var acc = db.Accounts.Where(x => x.Id == userId).SingleOrDefault();
            //transportFiles = db.TransportFiles.Include(x => x.Transports).Where(t => t.Transports.Any(x => x.ReceiverUserId == acc.EmployeeId) && t.KhanCap == true);
            transportFiles = db.Transports.Where(x => x.ReceiverUserId == acc.EmployeeId && x.TransportFile.KhanCap == true);
            if (!string.IsNullOrEmpty(search))
            {
                transportFiles = transportFiles.Where(x => x.TransportFile.tenFile.Trim().ToLower().Contains(search.Trim().ToLower()));
            }
            return View(transportFiles.OrderByDescending(x => x.TransportFile.CreateDate).ToList());
        }        
        // GET: TransportFiles/TransportFiles/Details/5
        public ActionResult Details(int? id, string flag)
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
            else
            {
                if(flag.Contains("HopThuDen"))
                {
                    var userId = int.Parse(User.Identity.Name);
                    var acc = db.Accounts.Where(x => x.Id == userId).SingleOrDefault();
                    var transport = db.Transports.Where(x => x.FileId == id && x.ReceiverUserId == acc.EmployeeId).SingleOrDefault();
                    transport.DaXem = true;
                    db.SaveChanges();
                }                
            }
            var lstReceiverUserId = db.Transports.Where(x => x.FileId == id).ToList();
            foreach (var item in lstReceiverUserId)
            {
                var ReceiverUserName = db.Employees.Where(x => x.Id == item.ReceiverUserId).Select(x=>x.Name).SingleOrDefault();
				ViewBag.NguoiNhan += ReceiverUserName + ","; 
            }
          
            return View(transportFiles);
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
        public ActionResult ChuyenTiep(TransportFile transportFile,List<string> lstReceiverUserId)
        {
            if (lstReceiverUserId != null)
            {
                foreach (var item in lstReceiverUserId)
                {
                    var checkExists = db.Transports.Where(x => x.ReceiverUserId.ToString().Equals(item) && x.FileId == transportFile.Id).SingleOrDefault();
                    if (checkExists == null)
                    {
                        Transport transport = new Transport
                        {
                            FileId = transportFile.Id,
                            TransportDate = DateTime.Now,
                            ReceiverUserId = int.Parse(item),
                            DaXem = false,
                            IsActive = true
                        };
                        db.Transports.Add(transport);
                    }
                }
                db.SaveChanges();
      
            }
            return RedirectToAction("HopThuDen");
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
        public ActionResult Create([Bind(Include = "Id,tenFile,Mota,KhanCap,NgayBanHanh,NguoiPheDuyetId,SoTrang,DoMat")] TransportFile transportFile, List<string> lstReceiverUserId, List<string> lstReceiverRoomId, List<string> lstReceiverUnitId, string customRadio_10 )
        {
            if (ModelState.IsValid)
            {
             
                transportFile.CreateDate = DateTime.Now;
                transportFile.CreateUserId = int.Parse(User.Identity.Name);
				transportFile.NguoiPheDuyetId = db.Accounts.Where(x => x.EmployeeId == transportFile.NguoiPheDuyetId).Select(x=>x.Id).SingleOrDefault();
				transportFile.IsActive = true;           
                db.TransportFiles.Add(transportFile);
                db.SaveChanges();

                if (Request.Files.Count > 0)
                {
                    int file_count = 0;
                    string dir = Server.MapPath("~/Content/Uploads/HopThu") + "\\" + transportFile.Id + "\\";
                    if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
                    TransportFileUrl transportFileUrl = new TransportFileUrl();
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        try
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            if (!string.IsNullOrEmpty(file.FileName))
                            {
                                string filename = DateTime.Now.Ticks + "_" + file.FileName.Split('/').Last();
                                file.SaveAs(dir + filename);
                                transportFileUrl.Url = filename;
                                transportFileUrl.TransportFilesId = transportFile.Id;
                                db.TransportFileUrls.Add(transportFileUrl);
                                db.SaveChanges();
                                file_count++;
                            }
                        }
                        catch { }
                    }             
                }
                if (lstReceiverUnitId != null)
                {
                    foreach (var item in lstReceiverUnitId)
                    {
                        var lstRoomId = db.DM_PhongBans.Where(x => x.donvi_Id.ToString().Equals(item)).Select(x => x.Id).ToList();
                        foreach (var roomId in lstRoomId)
                        {
                            var lstUserId = db.Accounts.Where(x => x.Employee.KhoaphongId == roomId)
                                .Select(x => x.EmployeeId)
                                .ToList();
                            foreach (var userId in lstUserId)
                            {
                                Transport transport = new Transport
                                {
                                    FileId = transportFile.Id,
                                    TransportDate = transportFile.CreateDate,
                                    ReceiverUserId = userId,
                                    DaXem = false,
                                    IsActive = true
                                };
                                db.Transports.Add(transport);
                            }
                            db.SaveChanges();
                        }
                    }
                }
                else if (lstReceiverRoomId != null)
                {
                    foreach (var item in lstReceiverRoomId)
                    {
                        var lstUserId = db.Accounts.Where(x => x.Employee.KhoaphongId.ToString() == item)
                            .Select(x=>x.EmployeeId)
                            .ToList();
                        foreach(var userId in lstUserId)
                        {
                            Transport transport = new Transport
                            {
                                FileId = transportFile.Id,
                                TransportDate = transportFile.CreateDate,
                                ReceiverUserId = userId,
                                DaXem = false,
                                IsActive = true
                            };
                            db.Transports.Add(transport);
                        }
                        db.SaveChanges();
                    }
                    if (lstReceiverUserId != null)
                    {
                        foreach (var item in lstReceiverUserId)
                        {
                            var checkExists = db.Transports.Where(x => x.ReceiverUserId.ToString().Equals(item) && x.FileId == transportFile.Id).SingleOrDefault();
                            if(checkExists == null)
                            {
                                Transport transport = new Transport
                                {
                                    FileId = transportFile.Id,
                                    TransportDate = transportFile.CreateDate,
                                    ReceiverUserId = int.Parse(item),
                                    DaXem = false,
                                    IsActive = true
                                };
                                db.Transports.Add(transport);
                            }                      
                        }
                        db.SaveChanges();
                    }
                }
                else if (lstReceiverUserId != null)
                {
                    foreach (var item in lstReceiverUserId)
                    {
                        Transport transport = new Transport
                        {
                            FileId = transportFile.Id,
                            TransportDate = transportFile.CreateDate,
                            ReceiverUserId = int.Parse(item),
                            DaXem = false,
                            IsActive = true
                        };
                        db.Transports.Add(transport);
                    }
                    db.SaveChanges();
                }             
             

                return RedirectToAction("HopThuDen");
            }

            ViewBag.Employees = new SelectList(db.Employees, "Id", "Name");
            ViewBag.DM_NhomPhongBans = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa");
            ViewBag.DM_PhongBans = new SelectList(db.DM_PhongBans, "Id", "TenKhoa");
            ViewBag.DM_DonVis = new SelectList(db.DM_DonVis, "Id", "TenDonVi");
            return View(transportFile);
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

        public ActionResult XuatExcel(DateTime? tuNgay, DateTime? denNgay)
        {
            string lstPhongBan = "";
			string lstCoQuan = "";
            string coQuan = "";
            int sttDic = 1;
            int STT = 1;
            int sttCell = 10;
            // Dữ liệu mẫu cần xuất ra Excel (thay thế bằng dữ liệu thực tế của bạn)    
            var transportFiles = db.TransportFiles.Where(x => x.NgayBanHanh >= tuNgay && x.NgayBanHanh <= denNgay).OrderByDescending(x=>x.NgayBanHanh).ToList();

            // For non-commercial use:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Tạo một gói Excel mới
            try
            {
                using (var package = new ExcelPackage())
                {
                    // Tạo một worksheet mới
                    var worksheet = package.Workbook.Worksheets.Add("CongVanDen");
                    // Định dạng chung

                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells.Style.Font.Size = 13;


                    // Định dạng tiêu đề (hàng 1)           
                    worksheet.Cells["A1:J1"].Style.Font.Bold = true;
                    worksheet.Cells["A1:J1"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    // Đặt màu nền cho vùng A1:B10 thành màu vàng
                    worksheet.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:J1"].Style.Fill.SetBackground(System.Drawing.Color.Yellow);

                    // Định dạng viền cho tất cả các cột
                    int totalColumns = worksheet.Dimension.Columns;
                    for (int col = 1; col <= totalColumns; col++)
                    {
                        worksheet.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Column(col).Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Column(col).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Column(col).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Column(col).Style.Font.Size = 13;
                        worksheet.Column(col).AutoFit();
                    }
                    worksheet.Column(3).Style.Numberformat.Format = "dd-MM-yyyy";
                    // Xác định tiêu đề các cột
                    worksheet.Cells["A1"].Value = "STT";
                    worksheet.Cells["B1"].Value = "Số công văn";
                    worksheet.Cells["C1"].Value = "Ngày ban hành";
                    worksheet.Cells["D1"].Value = "Số trang";
                    worksheet.Cells["E1"].Value = "Trích yếu";
                    worksheet.Cells["F1"].Value = "Người phê duyệt";
                    worksheet.Cells["G1"].Value = "Độ mật";
                    worksheet.Cells["H1"].Value = "Phòng nhận";
                    worksheet.Cells["I1"].Value = "Cơ quan nhận";
                    worksheet.Cells["J1"].Value = "Ghi chú";
                    worksheet.Cells["A1:J1"].AutoFitColumns();
                    // Điền dữ liệu vào worksheet
                    int row = 2;
                    foreach (var item in transportFiles)
                    {
						var lstReceiverId = db.Transports.Where(x => x.FileId == item.Id)
								 .Select(x => x.ReceiverUserId)
								 .ToList();
						var query = from receiverId in lstReceiverId
									join employee in db.Employees on receiverId equals employee.Id
									select new
									{
										PhongBan = employee.DM_PhongBans.TenKhoa,
										CoQuan = employee.DM_PhongBans.DM_DonVis.TenDonVi
									};								
						lstPhongBan = string.Join(",", query.Select(x => x.PhongBan).Distinct().ToList());
						lstCoQuan = string.Join(",", query.Select(x => x.CoQuan).Distinct().ToList());
					
						Dictionary<int, string> dicCells = new Dictionary<int, string>()
                    {
                        {1,STT.ToString()},
                        {2,item.tenFile},
                        {3,item.NgayBanHanh.ToString()},
                        {4,item.SoTrang.ToString()},
                        {5,item.Mota},
                        {6,item.Account2.Employee.Name},
                        {7,item.DoMat},
                        {8,lstPhongBan},
                        {9,lstCoQuan}
					};
                        List<string> lstURL = db.TransportFileUrls.Where(x => x.TransportFilesId == item.Id)
                            .Select(x => "https://www.namlao206.vn/Content/Uploads/HopThu" + "\\" + item.Id + "\\" + x.Url).ToList();
              

                        Dictionary<string, string> viewurl = new Dictionary<string, string>();

                        foreach (var url in lstURL)
                        {
                            sttDic = 1;
                            viewurl.Add(url, "File" + sttDic);
							sttDic++;
						}
						
						// Tạo liên kết và đặt tên hiển thị
						foreach (var entry in dicCells)
                        {
                            worksheet.Cells[row, entry.Key].Value = entry.Value;
                        }

                        foreach (var test in viewurl)
                        {                        
                            var hyperLink = new ExcelHyperLink(test.Key, UriKind.Absolute);
                            hyperLink.Display = test.Value;
                            worksheet.Cells[row, sttCell].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
                            worksheet.Cells[row, sttCell].Style.Font.UnderLine = true;
                            worksheet.Cells[row, sttCell].Hyperlink = hyperLink;
							sttCell++;
						}
                        sttCell = 10;
						row++;
                        STT++;
                    }
                    // Lưu vào bộ nhớ
                    using (var memoryStream = new MemoryStream())
                    {
                        package.SaveAs(memoryStream);
                        memoryStream.Position = 0;

                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSach.xlsx");
                    }
                }

            }
            catch (ObjectDisposedException ex)
            {
                // Xử lý lỗi, ví dụ: ghi log lỗi và trả về một thông báo lỗi cho người dùng
                return ViewBag.Message(ex.Message);
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
