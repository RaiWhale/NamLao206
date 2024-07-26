using NamLao206.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class DM_AdminListUpItemController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/DM_AdminListUpItem
        public ActionResult Index()
        {
			ViewBag.Title = "List Admin -";
			return View(db.DM_AdminListUpItem.ToList());
        }

        // GET: Admin/DM_AdminListUpItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_AdminListUpItem dM_AdminListUpItem = db.DM_AdminListUpItem.Find(id);
            if (dM_AdminListUpItem == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_AdminListUpItem);
        }

        // GET: Admin/DM_AdminListUpItem/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/DM_AdminListUpItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemName,Name,NhomItemId")] DM_AdminListUpItem dM_AdminListUpItem)
        {
            if (ModelState.IsValid)
            {
                var itemname = db.DM_AdminListUpItem.Where(x => x.ItemName.Equals(dM_AdminListUpItem.ItemName)).SingleOrDefault();

                if (itemname != null)
                {
                    ViewBag.Message = "Đã tồn tại bảng";
                    return RedirectToAction("Index");
                }
                else
                {
                    db.DM_AdminListUpItem.Add(dM_AdminListUpItem);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return PartialView(dM_AdminListUpItem);

        }

        // GET: Admin/DM_AdminListUpItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_AdminListUpItem dM_AdminListUpItem = db.DM_AdminListUpItem.Find(id);
            if (dM_AdminListUpItem == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_AdminListUpItem);
        }

        // POST: Admin/DM_AdminListUpItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemName,Name,NhomItemId")] DM_AdminListUpItem dM_AdminListUpItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dM_AdminListUpItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(dM_AdminListUpItem);
        }

        // GET: Admin/DM_AdminListUpItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_AdminListUpItem dM_AdminListUpItem = db.DM_AdminListUpItem.Find(id);
            if (dM_AdminListUpItem == null)
            {
                return HttpNotFound();
            }
            return PartialView(dM_AdminListUpItem);
        }

        // POST: Admin/DM_AdminListUpItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_AdminListUpItem dM_AdminListUpItem = db.DM_AdminListUpItem.Find(id);
            db.DM_AdminListUpItem.Remove(dM_AdminListUpItem);
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
