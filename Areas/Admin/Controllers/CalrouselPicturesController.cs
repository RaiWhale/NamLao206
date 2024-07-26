using NamLao206.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class CalrouselPicturesController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/CalrouselPictures
        public ActionResult Index()
        {
            ViewBag.Title = "Gallery Calrousel -";
            return View(db.CalrouselPictures.OrderByDescending(x => x.Id).ToList());
        }

        // GET: Admin/CalrouselPictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalrouselPicture calrouselPicture = db.CalrouselPictures.Find(id);
            if (calrouselPicture == null)
            {
                return HttpNotFound();
            }
            return PartialView(calrouselPicture);
        }

        // GET: Admin/CalrouselPictures/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/CalrouselPictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Url")] CalrouselPicture calrouselPicture)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    int file_count = 0;
                    string dir = Server.MapPath("~/Content/Uploads/CalrouselPicture");
                    if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        try
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            if (!string.IsNullOrEmpty(file.FileName))
                            {
                                string filename = DateTime.Now.Ticks + "_" + file.FileName.Split('/').Last();
                                file.SaveAs(dir + "//" + filename);

                                db.CalrouselPictures.Add(new CalrouselPicture
                                {
                                    Url = filename,
                                });

                                file_count++;
                            }
                        }
                        catch { }
                    }
                    if (file_count > 0)
                    {
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            return PartialView(calrouselPicture);
        }

        // GET: Admin/CalrouselPictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalrouselPicture calrouselPicture = db.CalrouselPictures.Find(id);
            if (calrouselPicture == null)
            {
                return HttpNotFound();
            }
            return PartialView(calrouselPicture);
        }

        // POST: Admin/CalrouselPictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Url")] CalrouselPicture calrouselPicture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calrouselPicture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(calrouselPicture);
        }

        // GET: Admin/CalrouselPictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalrouselPicture calrouselPicture = db.CalrouselPictures.Find(id);

            if (calrouselPicture == null)
            {
                return HttpNotFound();
            }
            return PartialView(calrouselPicture);
        }

        // POST: Admin/CalrouselPictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalrouselPicture calrouselPicture = db.CalrouselPictures.Find(id);
            db.CalrouselPictures.Remove(calrouselPicture);
            db.SaveChanges();
            string path = Server.MapPath("~/Content/Uploads/CalrouselPicture");
            if (System.IO.File.Exists(path + "\\" + calrouselPicture.Url))
            {
                System.IO.File.Delete(path + "\\" + calrouselPicture.Url);
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
