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
    public class DM_PhongBansController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/DM_PhongBans
        public ActionResult Index()
        {
            ViewBag.Title = "Khoa / Phòng ban -";
            var DM_PhongBans = db.DM_PhongBans.Include(d => d.DM_NhomPhongBans).Include(d => d.Picture);
            return View(DM_PhongBans.ToList());
        }

        // GET: Admin/DM_PhongBans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_PhongBans DM_PhongBans = db.DM_PhongBans.Find(id);
            if (DM_PhongBans == null)
            {
                return HttpNotFound();
            }
            return PartialView(DM_PhongBans);
        }

        // GET: Admin/DM_PhongBans/Create
        public ActionResult Create()
        {
            ViewBag.NhomKhoaId = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa");
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Url");
            return View();
        }

        // POST: Admin/DM_PhongBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenKhoa,ChucNang,NhomKhoaId,PictureId,Description")] DM_PhongBans DM_PhongBans)
        {
            if (ModelState.IsValid)
            {

                db.DM_PhongBans.Add(DM_PhongBans);
                db.SaveChanges();
                if (Request.Files.Count > 0)
                {
                    int file_count = 0;
                    string dir = Server.MapPath("~/Content/Uploads/KhoaPhongs") + "\\" + DM_PhongBans.Id + "\\";
                    if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        try
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            if (!string.IsNullOrEmpty(file.FileName))
                            {
                                string filename = DateTime.Now.Ticks + "_" + file.FileName.Split('/').Last();
                                file.SaveAs(dir + filename);
                                db.Pictures.Add(new Picture
                                {
                                    Url = filename,
                                    KhoaphongId = DM_PhongBans.Id
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
                    return RedirectToAction("Index");
                }
            }

            ViewBag.NhomKhoaId = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa", DM_PhongBans.NhomKhoaId);
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Url", DM_PhongBans.PictureId);
            return View(DM_PhongBans);
        }

        // GET: Admin/DM_PhongBans/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_PhongBans DM_PhongBans = db.DM_PhongBans.Find(id);
            if (DM_PhongBans == null)
            {
                return HttpNotFound();
            }
            ViewBag.NhomKhoaId = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa", DM_PhongBans.NhomKhoaId);
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Url", DM_PhongBans.PictureId);
            return View(DM_PhongBans);
        }

        // POST: Admin/DM_PhongBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenKhoa,ChucNang,NhomKhoaId,PictureId,Description")] DM_PhongBans DM_PhongBans)
        {
            if (ModelState.IsValid)
            {
                db.Entry(DM_PhongBans).State = EntityState.Modified;
                db.SaveChanges();
                if (Request.Files.Count > 0)
                {
                    int file_count = 0;
                    string dir = Server.MapPath("~/Content/Uploads/KhoaPhongs") + "\\" + DM_PhongBans.Id + "\\";
                    if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        try
                        {
                            HttpPostedFileBase file = Request.Files[i];
                            if (!string.IsNullOrEmpty(file.FileName))
                            {
                                string filename = DateTime.Now.Ticks + "_" + file.FileName.Split('/').Last();
                                file.SaveAs(dir + filename);
                                db.Pictures.Add(new Picture
                                {
                                    Url = filename,
                                    KhoaphongId = DM_PhongBans.Id
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
            ViewBag.NhomKhoaId = new SelectList(db.DM_NhomPhongBans, "Id", "Nhomkhoa", DM_PhongBans.NhomKhoaId);
            ViewBag.PictureId = new SelectList(db.Pictures, "Id", "Url", DM_PhongBans.PictureId);
            return View(DM_PhongBans);
        }

        // GET: Admin/DM_PhongBans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_PhongBans DM_PhongBans = db.DM_PhongBans.Find(id);
            if (DM_PhongBans == null)
            {
                return HttpNotFound();
            }
            return PartialView(DM_PhongBans);
        }

        // POST: Admin/DM_PhongBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DM_PhongBans DM_PhongBans = db.DM_PhongBans.Find(id);
            db.DM_PhongBans.Remove(DM_PhongBans);
            string path = Server.MapPath("~/Content/Uploads/KhoaPhongs") + "\\" + DM_PhongBans.Id;
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
