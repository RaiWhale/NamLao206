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
    public class ThietBi_DonViController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();

        // GET: TransportFiles/ThietBi_DonVi
        public async Task<ActionResult> Index()
        {
            var thietBi_DonVi = db.ThietBi_DonVi.Include(t => t.Account).Include(t => t.Account1).Include(t => t.DM_DonVis).Include(t => t.DM_PhongBans).Include(t => t.DM_ThietBi);
            return View(await thietBi_DonVi.ToListAsync());
        }

        // GET: TransportFiles/ThietBi_DonVi/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBi_DonVi thietBi_DonVi = await db.ThietBi_DonVi.FindAsync(id);
            if (thietBi_DonVi == null)
            {
                return HttpNotFound();
            }
            return View(thietBi_DonVi);
        }

        // GET: TransportFiles/ThietBi_DonVi/Create
        public ActionResult Create()
        {
            ViewBag.NguoiCapNhat_Id = new SelectList(db.Accounts, "Id", "LoginName");
            ViewBag.NguoiTao_Id = new SelectList(db.Accounts, "Id", "LoginName");
            ViewBag.Donvi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi");
            ViewBag.Phongban_id = new SelectList(db.DM_PhongBans, "Id", "TenKhoa");
            ViewBag.Thietbi_Id = new SelectList(db.DM_ThietBi, "Id", "BienSo");
            return View();
        }

        // POST: TransportFiles/ThietBi_DonVi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Thietbi_Donvi_Id,Thietbi_Id,Donvi_Id,Phongban_id,SoLuong,SoLuong_ThucTe,SoLuong_Hong,NgayTao,NguoiTao_Id,NgayCapNhat,NguoiCapNhat_Id,IsActive")] ThietBi_DonVi thietBi_DonVi)
        {
            if (ModelState.IsValid)
            {
                db.ThietBi_DonVi.Add(thietBi_DonVi);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.NguoiCapNhat_Id = new SelectList(db.Accounts, "Id", "LoginName", thietBi_DonVi.NguoiCapNhat_Id);
            ViewBag.NguoiTao_Id = new SelectList(db.Accounts, "Id", "LoginName", thietBi_DonVi.NguoiTao_Id);
            ViewBag.Donvi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", thietBi_DonVi.Donvi_Id);
            ViewBag.Phongban_id = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", thietBi_DonVi.Phongban_id);
            ViewBag.Thietbi_Id = new SelectList(db.DM_ThietBi, "Id", "BienSo", thietBi_DonVi.Thietbi_Id);
            return View(thietBi_DonVi);
        }

        // GET: TransportFiles/ThietBi_DonVi/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBi_DonVi thietBi_DonVi = await db.ThietBi_DonVi.FindAsync(id);
            if (thietBi_DonVi == null)
            {
                return HttpNotFound();
            }
            ViewBag.NguoiCapNhat_Id = new SelectList(db.Accounts, "Id", "LoginName", thietBi_DonVi.NguoiCapNhat_Id);
            ViewBag.NguoiTao_Id = new SelectList(db.Accounts, "Id", "LoginName", thietBi_DonVi.NguoiTao_Id);
            ViewBag.Donvi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", thietBi_DonVi.Donvi_Id);
            ViewBag.Phongban_id = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", thietBi_DonVi.Phongban_id);
            ViewBag.Thietbi_Id = new SelectList(db.DM_ThietBi, "Id", "BienSo", thietBi_DonVi.Thietbi_Id);
            return View(thietBi_DonVi);
        }

        // POST: TransportFiles/ThietBi_DonVi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Thietbi_Donvi_Id,Thietbi_Id,Donvi_Id,Phongban_id,SoLuong,SoLuong_ThucTe,SoLuong_Hong,NgayTao,NguoiTao_Id,NgayCapNhat,NguoiCapNhat_Id,IsActive")] ThietBi_DonVi thietBi_DonVi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thietBi_DonVi).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.NguoiCapNhat_Id = new SelectList(db.Accounts, "Id", "LoginName", thietBi_DonVi.NguoiCapNhat_Id);
            ViewBag.NguoiTao_Id = new SelectList(db.Accounts, "Id", "LoginName", thietBi_DonVi.NguoiTao_Id);
            ViewBag.Donvi_Id = new SelectList(db.DM_DonVis, "Id", "TenDonVi", thietBi_DonVi.Donvi_Id);
            ViewBag.Phongban_id = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", thietBi_DonVi.Phongban_id);
            ViewBag.Thietbi_Id = new SelectList(db.DM_ThietBi, "Id", "BienSo", thietBi_DonVi.Thietbi_Id);
            return View(thietBi_DonVi);
        }

        // GET: TransportFiles/ThietBi_DonVi/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThietBi_DonVi thietBi_DonVi = await db.ThietBi_DonVi.FindAsync(id);
            if (thietBi_DonVi == null)
            {
                return HttpNotFound();
            }
            return View(thietBi_DonVi);
        }

        // POST: TransportFiles/ThietBi_DonVi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ThietBi_DonVi thietBi_DonVi = await db.ThietBi_DonVi.FindAsync(id);
            db.ThietBi_DonVi.Remove(thietBi_DonVi);
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
