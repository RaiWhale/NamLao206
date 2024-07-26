using NamLao206.Models;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    public class PicturesController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/Pictures
        public ActionResult Index()
        {
			ViewBag.Title = "Hình ảnh -";
			var pictures = db.Pictures.Include(p => p.DM_PhongBans1);
            return View(pictures.ToList());
        }

        // GET: Admin/Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Admin/Pictures/Create
        public ActionResult Create()
        {
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa");
            return PartialView();
        }

        // POST: Admin/Pictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Url,KhoaphongId")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Pictures.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", picture.KhoaphongId);
            return PartialView(picture);
        }

        // GET: Admin/Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", picture.KhoaphongId);
            return PartialView(picture);
        }

        // POST: Admin/Pictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Url,KhoaphongId")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KhoaphongId = new SelectList(db.DM_PhongBans, "Id", "TenKhoa", picture.KhoaphongId);
            return PartialView(picture);
        }

        // GET: Admin/Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return PartialView(picture);
        }

        // POST: Admin/Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
            string path = Server.MapPath("~/Content/Uploads/KhoaPhongs") + "\\" + picture.KhoaphongId + "\\" + picture.Url;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
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
