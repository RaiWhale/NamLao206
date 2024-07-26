using NamLao206.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class DM_ChucvusController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/DM_Chucvus
        public ActionResult Index()
        {
			ViewBag.Title = "Chức vụ -";
			return View(db.DM_Chucvus.ToList());
        }

        // GET: Admin/DM_Chucvus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Chucvus dM_Chucvus = db.DM_Chucvus.Find(id);
            if (dM_Chucvus == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_Chucvus);
        }

        // GET: Admin/DM_Chucvus/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/DM_Chucvus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Chucvu")] DM_Chucvus dM_Chucvus)
        {
            if (ModelState.IsValid)
            {
                db.DM_Chucvus.Add(dM_Chucvus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView(dM_Chucvus);
        }

        // GET: Admin/DM_Chucvus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Chucvus dM_Chucvus = db.DM_Chucvus.Find(id);
            if (dM_Chucvus == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_Chucvus);
        }

        // POST: Admin/DM_Chucvus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Chucvu")] DM_Chucvus dM_Chucvus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_Chucvus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(dM_Chucvus);
        }

        // GET: Admin/DM_Chucvus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Chucvus dM_Chucvus = db.DM_Chucvus.Find(id);
            if (dM_Chucvus == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_Chucvus);
        }

        // POST: Admin/DM_Chucvus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_Chucvus dM_Chucvus = db.DM_Chucvus.Find(id);
            db.DM_Chucvus.Remove(dM_Chucvus);
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
