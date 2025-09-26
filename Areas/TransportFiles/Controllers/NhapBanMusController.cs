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
    public class NhapBanMusController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/NhapBanMus
        public async Task<ActionResult> Index()
        {
            var nhapBanMus = db.NhapBanMus.Include(n => n.Account).Include(n => n.DM_DonVis).Include(n => n.DocumentType).Include(n => n.Employee).Include(n => n.Employee1).Include(n => n.Employee2).Include(n => n.StatusProject).Include(n => n.Unit).Include(n => n.Unit1).Include(n => n.NhapBanMu1).Include(n => n.NhapBanMu2).Include(n => n.Supplier).Include(n => n.Team);
            return View(await nhapBanMus.ToListAsync());
        }

        // GET: TransportFiles/NhapBanMus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhapBanMu nhapBanMu = await db.NhapBanMus.FindAsync(id);
            if (nhapBanMu == null)
            {
                return HttpNotFound();
            }
            return View(nhapBanMu);
        }

        // GET: TransportFiles/NhapBanMus/Create
        public ActionResult Create()
        {
          
         
            ViewBag.LoaiHs = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName");
            ViewBag.KeToan_EMP_Id = new SelectList(db.Employees, "Id", "Name");
            ViewBag.NguoiPheDuyet_EMP_Id = new SelectList(db.Employees, "Id", "Name");
            ViewBag.TroLyKeHoach_EMP_Id = new SelectList(db.Employees, "Id", "Name");
            ViewBag.DanhGiaCLMu = new SelectList(db.StatusProjects, "Id", "StatusName");
            ViewBag.DonViTienTe_Id = new SelectList(db.Units, "Id", "UnitName");
            ViewBag.LoaiTK = new SelectList(db.Units, "Id", "UnitName");
            ViewBag.Id = new SelectList(db.NhapBanMus, "Id", "LoaiMu");
            ViewBag.Id = new SelectList(db.NhapBanMus, "Id", "LoaiMu");
            ViewBag.DoiTac_Id = new SelectList(db.Suppliers, "Id", "SupplierName");
            ViewBag.Team_Id = new SelectList(db.Teams, "Id", "TeamName");
            return PartialView();
        }

        // POST: TransportFiles/NhapBanMus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DonVi_Id,Team_Id,LoaiMu,NguoiNhap_ACC_Id,NgayNhap,NguoiPheDuyet_EMP_Id,TroLyKeHoach_EMP_Id,KhoiLuongThuVao,KhoiLuongTTTC,KhoiLuongThuVaoLast,KhoiLuongTTTCLast,LoaiHs,LoaiTK,DonGia,Note,SoDienThoai,KeToan_EMP_Id,KhoiLuongTTL,DanhGiaCLMu,DoiTac_Id,DonViTienTe_Id,NguoiCan_EMP_Id")] NhapBanMu nhapBanMu)
        {
            if (ModelState.IsValid)
            {
                db.NhapBanMus.Add(nhapBanMu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.NguoiNhap_ACC_Id = new SelectList(db.Accounts, "Id", "LoginName", nhapBanMu.NguoiNhap_ACC_Id);
            ViewBag.DonVi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", nhapBanMu.DonVi_Id);
            ViewBag.LoaiHs = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", nhapBanMu.LoaiHs);
            ViewBag.KeToan_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.KeToan_EMP_Id);
            ViewBag.NguoiPheDuyet_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.NguoiPheDuyet_EMP_Id);
            ViewBag.TroLyKeHoach_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.TroLyKeHoach_EMP_Id);
            ViewBag.DanhGiaCLMu = new SelectList(db.StatusProjects, "Id", "StatusName", nhapBanMu.DanhGiaCLMu);
            ViewBag.DonViTienTe_Id = new SelectList(db.Units, "Id", "UnitName", nhapBanMu.DonViTienTe_Id);
            ViewBag.LoaiTK = new SelectList(db.Units, "Id", "UnitName", nhapBanMu.LoaiTK);
            ViewBag.Id = new SelectList(db.NhapBanMus, "Id", "LoaiMu", nhapBanMu.Id);
            ViewBag.Id = new SelectList(db.NhapBanMus, "Id", "LoaiMu", nhapBanMu.Id);
            ViewBag.DoiTac_Id = new SelectList(db.Suppliers, "Id", "SupplierName", nhapBanMu.DoiTac_Id);
            ViewBag.Team_Id = new SelectList(db.Teams, "Id", "TeamName", nhapBanMu.Team_Id);
            return View(nhapBanMu);
        }

        // GET: TransportFiles/NhapBanMus/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhapBanMu nhapBanMu = await db.NhapBanMus.FindAsync(id);
            if (nhapBanMu == null)
            {
                return HttpNotFound();
            }
            ViewBag.NguoiNhap_ACC_Id = new SelectList(db.Accounts, "Id", "LoginName", nhapBanMu.NguoiNhap_ACC_Id);
            ViewBag.DonVi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", nhapBanMu.DonVi_Id);
            ViewBag.LoaiHs = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", nhapBanMu.LoaiHs);
            ViewBag.KeToan_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.KeToan_EMP_Id);
            ViewBag.NguoiPheDuyet_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.NguoiPheDuyet_EMP_Id);
            ViewBag.TroLyKeHoach_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.TroLyKeHoach_EMP_Id);
            ViewBag.DanhGiaCLMu = new SelectList(db.StatusProjects, "Id", "StatusName", nhapBanMu.DanhGiaCLMu);
            ViewBag.DonViTienTe_Id = new SelectList(db.Units, "Id", "UnitName", nhapBanMu.DonViTienTe_Id);
            ViewBag.LoaiTK = new SelectList(db.Units, "Id", "UnitName", nhapBanMu.LoaiTK);
            ViewBag.Id = new SelectList(db.NhapBanMus, "Id", "LoaiMu", nhapBanMu.Id);
            ViewBag.Id = new SelectList(db.NhapBanMus, "Id", "LoaiMu", nhapBanMu.Id);
            ViewBag.DoiTac_Id = new SelectList(db.Suppliers, "Id", "SupplierName", nhapBanMu.DoiTac_Id);
            ViewBag.Team_Id = new SelectList(db.Teams, "Id", "TeamName", nhapBanMu.Team_Id);
            return View(nhapBanMu);
        }

        // POST: TransportFiles/NhapBanMus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DonVi_Id,Team_Id,LoaiMu,NguoiNhap_ACC_Id,NgayNhap,NguoiPheDuyet_EMP_Id,TroLyKeHoach_EMP_Id,KhoiLuongThuVao,KhoiLuongTTTC,KhoiLuongThuVaoLast,KhoiLuongTTTCLast,LoaiHs,LoaiTK,DonGia,Note,SoDienThoai,KeToan_EMP_Id,KhoiLuongTTL,DanhGiaCLMu,DoiTac_Id,DonViTienTe_Id,NguoiCan_EMP_Id")] NhapBanMu nhapBanMu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhapBanMu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.NguoiNhap_ACC_Id = new SelectList(db.Accounts, "Id", "LoginName", nhapBanMu.NguoiNhap_ACC_Id);
            ViewBag.DonVi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", nhapBanMu.DonVi_Id);
            ViewBag.LoaiHs = new SelectList(db.DocumentTypes, "Id", "DocumentTypeName", nhapBanMu.LoaiHs);
            ViewBag.KeToan_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.KeToan_EMP_Id);
            ViewBag.NguoiPheDuyet_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.NguoiPheDuyet_EMP_Id);
            ViewBag.TroLyKeHoach_EMP_Id = new SelectList(db.Employees, "Id", "Name", nhapBanMu.TroLyKeHoach_EMP_Id);
            ViewBag.DanhGiaCLMu = new SelectList(db.StatusProjects, "Id", "StatusName", nhapBanMu.DanhGiaCLMu);
            ViewBag.DonViTienTe_Id = new SelectList(db.Units, "Id", "UnitName", nhapBanMu.DonViTienTe_Id);
            ViewBag.LoaiTK = new SelectList(db.Units, "Id", "UnitName", nhapBanMu.LoaiTK);
            ViewBag.Id = new SelectList(db.NhapBanMus, "Id", "LoaiMu", nhapBanMu.Id);
            ViewBag.Id = new SelectList(db.NhapBanMus, "Id", "LoaiMu", nhapBanMu.Id);
            ViewBag.DoiTac_Id = new SelectList(db.Suppliers, "Id", "SupplierName", nhapBanMu.DoiTac_Id);
            ViewBag.Team_Id = new SelectList(db.Teams, "Id", "TeamName", nhapBanMu.Team_Id);
            return View(nhapBanMu);
        }

        // GET: TransportFiles/NhapBanMus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhapBanMu nhapBanMu = await db.NhapBanMus.FindAsync(id);
            if (nhapBanMu == null)
            {
                return HttpNotFound();
            }
            return View(nhapBanMu);
        }

        // POST: TransportFiles/NhapBanMus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NhapBanMu nhapBanMu = await db.NhapBanMus.FindAsync(id);
            db.NhapBanMus.Remove(nhapBanMu);
            await db.SaveChangesAsync();
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
