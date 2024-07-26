using NamLao206.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class DM_HocvisController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/DM_Hocvis
        public ActionResult Index()
        {
            ViewBag.Title = "Học vị -";
            return View(db.DM_Hocvis.ToList());
        }

        // GET: Admin/DM_Hocvis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Hocvis dM_Hocvis = db.DM_Hocvis.Find(id);
            if (dM_Hocvis == null)
            {
                return HttpNotFound();
            }
            return View(dM_Hocvis);
        }

        // GET: Admin/DM_Hocvis/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/DM_Hocvis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,tenHocvi")] DM_Hocvis dM_Hocvis)
        {
            if (ModelState.IsValid)
            {
                db.DM_Hocvis.Add(dM_Hocvis);
                db.SaveChanges();
                return RedirectToAction("Index");


            }

            return PartialView(dM_Hocvis);
        }

        // GET: Admin/DM_Hocvis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Hocvis dM_Hocvis = db.DM_Hocvis.Find(id);
            if (dM_Hocvis == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_Hocvis);
        }

        // POST: Admin/DM_Hocvis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,tenHocvi")] DM_Hocvis dM_Hocvis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_Hocvis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(dM_Hocvis);
        }

        // GET: Admin/DM_Hocvis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_Hocvis dM_Hocvis = db.DM_Hocvis.Find(id);
            if (dM_Hocvis == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_Hocvis);
        }

        // POST: Admin/DM_Hocvis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_Hocvis dM_Hocvis = db.DM_Hocvis.Find(id);
            db.DM_Hocvis.Remove(dM_Hocvis);
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
