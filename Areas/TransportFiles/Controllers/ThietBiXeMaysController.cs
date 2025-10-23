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
    public class ThietBiXeMaysController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/ThietBiXeMays
        public async Task<ActionResult> Index()
        {
            var thietBiXeMays = db.ThietBiXeMays.Include(t => t.DM_DonVis).Include(t => t.Employee);
            return View(await thietBiXeMays.ToListAsync());
        }

        // GET: TransportFiles/ThietBiXeMays/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBiXeMay thietBiXeMay = await db.ThietBiXeMays.FindAsync(id);
            if (thietBiXeMay == null)
            {
                return HttpNotFound();
            }
            return View(thietBiXeMay);
        }

        // GET: TransportFiles/ThietBiXeMays/Create
        public ActionResult Create()
        {
            ViewBag.DonVi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi");
            ViewBag.NguoiSuDung_Id = new SelectList(db.Employees, "Id", "Name");
            return View();
        }

        // POST: TransportFiles/ThietBiXeMays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DonVi_Id,NgayNhap,LoaiTrangThietBi,BienSo,NhanHieu,SoKhung,SoMay,NamSanXuat,XuatXu,NguoiSuDung_Id,TinhTrangKyThuat,HoSoPhapLy_Id,GhiChu,CreateDate,CreateUser_Id,IsActive")] ThietBiXeMay thietBiXeMay)
        {
            if (ModelState.IsValid)
            {
                db.ThietBiXeMays.Add(thietBiXeMay);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DonVi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", thietBiXeMay.DonVi_Id);
            ViewBag.NguoiSuDung_Id = new SelectList(db.Employees, "Id", "Name", thietBiXeMay.NguoiSuDung_Id);
            return View(thietBiXeMay);
        }

        // GET: TransportFiles/ThietBiXeMays/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBiXeMay thietBiXeMay = await db.ThietBiXeMays.FindAsync(id);
            if (thietBiXeMay == null)
            {
                return HttpNotFound();
            }
            ViewBag.DonVi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", thietBiXeMay.DonVi_Id);
            ViewBag.NguoiSuDung_Id = new SelectList(db.Employees, "Id", "Name", thietBiXeMay.NguoiSuDung_Id);
            return View(thietBiXeMay);
        }

        // POST: TransportFiles/ThietBiXeMays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DonVi_Id,NgayNhap,LoaiTrangThietBi,BienSo,NhanHieu,SoKhung,SoMay,NamSanXuat,XuatXu,NguoiSuDung_Id,TinhTrangKyThuat,HoSoPhapLy_Id,GhiChu,CreateDate,CreateUser_Id,IsActive")] ThietBiXeMay thietBiXeMay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thietBiXeMay).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DonVi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", thietBiXeMay.DonVi_Id);
            ViewBag.NguoiSuDung_Id = new SelectList(db.Employees, "Id", "Name", thietBiXeMay.NguoiSuDung_Id);
            return View(thietBiXeMay);
        }

        // GET: TransportFiles/ThietBiXeMays/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBiXeMay thietBiXeMay = await db.ThietBiXeMays.FindAsync(id);
            if (thietBiXeMay == null)
            {
                return HttpNotFound();
            }
            return View(thietBiXeMay);
        }

        // POST: TransportFiles/ThietBiXeMays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ThietBiXeMay thietBiXeMay = await db.ThietBiXeMays.FindAsync(id);
            db.ThietBiXeMays.Remove(thietBiXeMay);
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
