using PagedList;
using NamLao206.Models;
using NamLao206.Models.ViewModels;
using NamLao206.Utils;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();
        int pageSize = 10;
        // GET: Admin/Employee
        public ActionResult Index(int? page, string search)
        {
			ViewBag.Title = "Nhân viên -";
			IQueryable<Employee> employee; //model chinh


			employee = db.Employees.Include(d => d.DM_Chucvus).Include(d => d.DM_Hocvis).Include(d => d.DM_Nghenghieps).Include(d => d.Level).Include(d => d.DM_Donvihanhchinhs).OrderBy(d => d.Id);
            if (search != null && search.Trim() != "")
            {
				employee = employee.Where(s => s.Name.ToLower().Contains(search.Trim().ToLower()));
            }
            ViewBag.search = search;
            //Paging
            int pageNumber = page ?? 1;
            return View(employee.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return PartialView(employee);
        }

        // GET: Admin/Employee/Create
        public ActionResult Create()
        {          
            ViewBag.CityId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten");
            ViewBag.ChucvuId = new SelectList(db.DM_Chucvus, "Id", "Chucvu");
            ViewBag.HocviId = new SelectList(db.DM_Hocvis, "Id", "tenHocvi");
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa");
            ViewBag.NghenghiepId = new SelectList(db.DM_Nghenghieps, "Id", "Nghenghiep");
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName");
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "GioiTinh");
            return View();
        }

        // POST: Admin/Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Phone,Email,Address,LevelId,IsActive,KhoaphongId,NghenghiepId,ChucvuId,HocviId,CreatedDate,CityId,DistrictId,WardId,Avatar,GenderId,Birthday")] Employee employee, RegisterVM data, string LoginName, string Password, HttpPostedFileBase pic)
        {
            if (ModelState.IsValid)
            {
                if (pic != null)
                {
                    string filename = DateTime.Now.Ticks + "_" + pic.FileName.Split('/').Last();
					employee.Avatar = filename;
                    string path = Server.MapPath("~/Content/Uploads/Avatars");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    pic.SaveAs(path + "\\" + filename);
                }
                else
                {
					employee.Avatar = null;
                }

				employee.CreatedDate = DateTime.Now;              
                db.Employees.Add(employee);
                db.SaveChanges();

				data.Password = MySecurity.Encrypt(Password);				
				var acc = AutoMapper.Mapper.Map<Account>(data);
				acc.EmployeeId = employee.Id;
				acc.AccountType = employee.LevelId.Value;
				acc.IsActive = true;                  
                db.Accounts.Add(acc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

		
			ViewBag.CityId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten", employee.CityId);
            ViewBag.DistrictId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == employee.CityId), "Id", "Ten", employee.DistrictId);
            ViewBag.WardId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == employee.DistrictId), "Id", "Ten", employee.WardId);
            ViewBag.ChucvuId = new SelectList(db.DM_Chucvus, "Id", "Chucvu", employee.ChucvuId);
            ViewBag.HocviId = new SelectList(db.DM_Hocvis, "Id", "tenHocvi", employee.HocviId);
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", employee.KhoaphongId);
            ViewBag.NghenghiepId = new SelectList(db.DM_Nghenghieps, "Id", "Nghenghiep", employee.NghenghiepId);
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "GioiTinh", employee.GenderId);
			return View(employee);
        }

        // GET: Admin/Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

			ViewBag.CityId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten", employee.CityId);
			ViewBag.DistrictId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == employee.CityId), "Id", "Ten", employee.DistrictId);
			ViewBag.WardId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == employee.DistrictId), "Id", "Ten", employee.WardId);
			ViewBag.ChucvuId = new SelectList(db.DM_Chucvus, "Id", "Chucvu", employee.ChucvuId);
            ViewBag.HocviId = new SelectList(db.DM_Hocvis, "Id", "tenHocvi", employee.HocviId);
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", employee.KhoaphongId);
            ViewBag.NghenghiepId = new SelectList(db.DM_Nghenghieps, "Id", "Nghenghiep", employee.NghenghiepId);
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
			ViewBag.GenderId = new SelectList(db.Genders, "Id", "GioiTinh", employee.GenderId);
			return View(employee);
        }

        // POST: Admin/Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Email,Address,LevelId,IsActive,KhoaphongId,NghenghiepId,ChucvuId,HocviId,CreatedDate,CityId,DistrictId,WardId,Avatar,GenderId,Birthday")] Employee employee, HttpPostedFileBase pic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                if (pic != null)
                {
                    string filename = DateTime.Now.Ticks + "_" + pic.FileName.Split('/').Last();
                    string path = Server.MapPath("~/Content/Uploads/Avatars");
                    if (System.IO.File.Exists(path + "\\" + employee.Avatar))
                    {
                        System.IO.File.Delete(path + "\\" + employee.Avatar);
                    }
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    pic.SaveAs(path + "\\" + filename);
					employee.Avatar = filename;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

			ViewBag.CityId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0"), "Id", "Ten", employee.CityId);
			ViewBag.DistrictId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == employee.CityId), "Id", "Ten", employee.DistrictId);
			ViewBag.WardId = new SelectList(db.DM_Donvihanhchinhs.Where(x => x.ParentId == employee.DistrictId), "Id", "Ten", employee.WardId);
			ViewBag.ChucvuId = new SelectList(db.DM_Chucvus, "Id", "Chucvu", employee.ChucvuId);
            ViewBag.HocviId = new SelectList(db.DM_Hocvis, "Id", "tenHocvi", employee.HocviId);
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", employee.KhoaphongId);
            ViewBag.NghenghiepId = new SelectList(db.DM_Nghenghieps, "Id", "Nghenghiep", employee.NghenghiepId);
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
			ViewBag.GenderId = new SelectList(db.Genders, "Id", "GioiTinh", employee.GenderId);
			return View(employee);
        }

        // GET: Admin/Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return PartialView(employee);
        }

        // POST: Admin/Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var acc = db.Accounts.SingleOrDefault(x => x.EmployeeId == id);
            if (acc != null)
            {
                db.Accounts.Remove(acc);
            }
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            string path = Server.MapPath("~/Content/Uploads/Avatars");
            if (System.IO.File.Exists(path + "\\" + employee.Avatar))
            {
                System.IO.File.Delete(path + "\\" + employee.Avatar);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
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
