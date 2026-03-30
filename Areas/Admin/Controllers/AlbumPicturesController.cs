using NamLao206.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class AlbumPicturesController : Controller
    {
        private namlao206_websiteEntities db = new namlao206_websiteEntities();
        private static string UploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", "Albums");
        // GET: Admin/AlbumPictures
        public ActionResult Index(int? id, string message, string search)
        {
            ViewBag.Title = "Album -";
            //var AlbumPictures = db.AlbumPictures.Include(h => h.Album);
            IQueryable<Album> album;
            album = db.Albums.OrderByDescending(x => x.Id);


            ViewBag.album = db.Albums.OrderByDescending(x => x.Id).ToList();
            ViewBag.albumpicture = db.AlbumPictures.ToList();
            ViewBag.cat = id;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            return View(album);
        }

        // GET: Admin/AlbumPictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumPicture albumPicture = db.AlbumPictures.Find(id);
            if (albumPicture == null)
            {
                return HttpNotFound();
            }
            return PartialView(albumPicture);
        }

        // GET: Admin/AlbumPictures/Create
        public ActionResult Create()
        {
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "AlbumName");
            return PartialView();
        }

        // POST: Admin/AlbumPictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Id,PictureName,AlbumId,ten,mota,IsActive,Url")] AlbumPicture albumPicture)
        {
           
            if (Request.Files.Count > 0)
            {
                int file_count = 0;
                string dir = UploadPath + "\\" + albumPicture.AlbumId + "\\";
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
                            albumPicture.IsActive = true;
                            albumPicture.PictureName = filename;
                            db.AlbumPictures.Add(albumPicture);
                            await db.SaveChangesAsync();
                            file_count++;
                        }
                    }
                    catch { }
                }
                ViewBag.Message = "Thêm mới thành công!";
                return RedirectToAction("Index", "AlbumPictures", new { message = ViewBag.Message });
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "AlbumName", albumPicture.AlbumId);
            return PartialView(albumPicture);
        }

        // GET: Admin/AlbumPictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumPicture albumPicture = db.AlbumPictures.Find(id);
            if (albumPicture == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "Id", "AlbumName", albumPicture.AlbumId);
            return PartialView(albumPicture);
        }

        // POST: Admin/AlbumPictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PictureName,AlbumId,ten,mota,IsActive,Url")] AlbumPicture albumPicture)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    int file_count = 0;
                    string dir = UploadPath + "\\" + albumPicture.AlbumId + "\\";
                    if (System.IO.File.Exists(dir + albumPicture.PictureName)) System.IO.File.Delete(dir + albumPicture.PictureName);
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
                                albumPicture.PictureName = filename;

                                file_count++;
                            }
                        }
                        catch { }
                    }
                }
                db.Entry(albumPicture).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ViewBag.Message = "Sửa thành công!";

                return RedirectToAction("Index", "AlbumPictures", new { message = ViewBag.Message, Id = albumPicture.AlbumId });
            }
            ViewBag.Message = "Đã xảy ra lỗi nhập liệu!";
            return RedirectToAction("Index", new { message = ViewBag.Message });
        }

        // GET: Admin/AlbumPictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumPicture albumPicture = db.AlbumPictures.Find(id);
            if (albumPicture == null)
            {
                return HttpNotFound();
            }
            return PartialView(albumPicture);
        }

        // POST: Admin/AlbumPictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AlbumPicture albumPicture = db.AlbumPictures.Find(id);
       
            string path = UploadPath + "\\" + albumPicture.AlbumId + "\\" + albumPicture.PictureName;
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            db.AlbumPictures.Remove(albumPicture);
            await db.SaveChangesAsync();
            ViewBag.Message = "Xóa thành công!";
            return RedirectToAction("Index","AlbumPictures", new { message = ViewBag.Message, Id = albumPicture.AlbumId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // POST: Admin/SanPham/DeletePicture/5
        [HttpPost]
        public ActionResult DeletePicture(int id)
        {
            try
            {
                AlbumPicture pic = db.AlbumPictures.Find(id);
                db.AlbumPictures.Remove(pic);
                db.SaveChanges();
                string file = UploadPath + "\\" + pic.AlbumId + "\\" + pic.PictureName;
                if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
                return Content("Ok");
            }
            catch
            {
                return Content("Error");
            }
        }
    }
}
