using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NamLao206.Models;
using NamLao206.Models.ViewModels;
using NamLao206.Controllers.API;
using PagedList;
namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
	public class DM_DonvihanhchinhsController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
		int pageSize = 10;
		// GET: Admin/DM_Donvihanhchinhs
		public ActionResult Index(int? page, string search)
        {
			ViewBag.Title = "Tin tức -";
			IQueryable<DM_Donvihanhchinhs> dvhc; //model chinh        

			dvhc = db.DM_Donvihanhchinhs.OrderByDescending(x => x.Id);
			if (search != null && search.Trim() != "")
			{
				dvhc = dvhc.Where(s => s.Ten.Trim().ToLower().Contains(search.Trim().ToLower()));
			}
			ViewBag.search = search;
			//Paging
			int pageNumber = page ?? 1;
			return View(dvhc.ToPagedList(pageNumber, pageSize));
		}

        // GET: Admin/DM_Donvihanhchinhs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Donvihanhchinhs dM_Donvihanhchinhs = db.DM_Donvihanhchinhs.Find(id);
            if (dM_Donvihanhchinhs == null)
            {
                return HttpNotFound();
            }
            return View(dM_Donvihanhchinhs);
        }

        // GET: Admin/DM_Donvihanhchinhs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DM_Donvihanhchinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ten,Ma,ParentId,CapId,IsActive")] DM_Donvihanhchinhs dM_Donvihanhchinhs)
        {
            if (ModelState.IsValid)
            {
                db.DM_Donvihanhchinhs.Add(dM_Donvihanhchinhs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dM_Donvihanhchinhs);
        }

        // GET: Admin/DM_Donvihanhchinhs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Donvihanhchinhs dM_Donvihanhchinhs = db.DM_Donvihanhchinhs.Find(id);
            if (dM_Donvihanhchinhs == null)
            {
                return HttpNotFound();
            }
            return View(dM_Donvihanhchinhs);
        }

        // POST: Admin/DM_Donvihanhchinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ten,Ma,ParentId,CapId,IsActive")] DM_Donvihanhchinhs dM_Donvihanhchinhs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_Donvihanhchinhs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dM_Donvihanhchinhs);
        }

        // GET: Admin/DM_Donvihanhchinhs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Donvihanhchinhs dM_Donvihanhchinhs = db.DM_Donvihanhchinhs.Find(id);
            if (dM_Donvihanhchinhs == null)
            {
                return HttpNotFound();
            }
            return View(dM_Donvihanhchinhs);
        }

        // POST: Admin/DM_Donvihanhchinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DM_Donvihanhchinhs dM_Donvihanhchinhs = db.DM_Donvihanhchinhs.Find(id);
            db.DM_Donvihanhchinhs.Remove(dM_Donvihanhchinhs);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
		public async Task<ActionResult> DVHC(Donvihanhchinh dvhc)
		{
			var model = await ApiController.GetDVHC();
            foreach (var item in model)
            {
                db.DM_Donvihanhchinhs.Add(new DM_Donvihanhchinhs
                {                
                    Ma = item.ma,
                    ParentId = item.chaId,
                    Ten = item.ten,
                    CapId = item.capId
                });
                db.SaveChanges();
            }
			return RedirectToAction("Index");
		}
		public async Task<ActionResult> QT(QuocTich qt)
		{
			var model = await ApiController.GetQT();
            foreach (var item in model)
            {
                db.Nationalities.Add(new Nationality
                {
                    Id = item.id,
                    Ma = item.ma,
                    QuocTich = item.ten,
                    tenKhac = item.tenKhac
                });
                db.SaveChanges();
            }
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
