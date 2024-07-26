using NamLao206.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class BannersController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/Banners
        public ActionResult Index()
        {
			ViewBag.Title = "Banner -";
			return View(db.Banners.ToList());
        }

        // GET: Admin/Banners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return PartialView(banner);
        }

        // GET: Admin/Banners/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BannerName,Url,isActive")] Banner banner, HttpPostedFileBase pic)
        {
            if (ModelState.IsValid)
            {
                string filename = DateTime.Now.Ticks + "_" + pic.FileName.Split('/').Last();
                banner.Url = filename;
                banner.isActive = true;
                db.Banners.Add(banner);
                db.SaveChanges();

                string path = Server.MapPath("~/Content/Uploads/Banners") + "\\" + banner.Id;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                pic.SaveAs(path + "\\" + filename);


                return RedirectToAction("Index");
            }

            return PartialView(banner);
        }

        // GET: Admin/Banners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return PartialView(banner);
        }

        // POST: Admin/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BannerName,Url,isActive")] Banner banner, HttpPostedFileBase pic)
        {
            if (ModelState.IsValid)
            {

                if (banner.Url == null)
                {
                    string filename = DateTime.Now.Ticks + "_" + pic.FileName.Split('/').Last();
                    banner.Url = filename;
                    string path = Server.MapPath("~/Content/Uploads/Banners") + "\\" + banner.Id;
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    pic.SaveAs(path + "\\" + filename);

                }
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(banner);
        }

        // GET: Admin/Banners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return PartialView(banner);
        }

        // POST: Admin/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
            string path = Server.MapPath("~/Content/Uploads/Banners") + "\\" + banner.Id;
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
