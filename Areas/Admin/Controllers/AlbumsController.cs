﻿using NamLao206.Models;
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
    public class AlbumsController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/Albums
        public ActionResult Index()
        {
			ViewBag.Title = "Albums -";
			return View(db.Albums.ToList());
        }

        // GET: Admin/Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return PartialView(album);
        }

        // GET: Admin/Albums/Create
        public ActionResult Create(int? id)
        {
            var album = new Album()
            {
                ParentId = id != null ? id.Value : 0
            };
            return PartialView(album);
        }

        // POST: Admin/Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,AlbumName,ParentId")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                if (Request.Files.Count > 0)
                {
                    int file_count = 0;
                    string dir = Server.MapPath("~/Content/Uploads/AlbumPictures") + "\\" + album.Id + "\\";
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
                                db.AlbumPictures.Add(new AlbumPicture
                                {

                                    PictureName = filename,
                                    AlbumId = album.Id
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
                return RedirectToAction("Index", "AlbumPictures");
            }
            return PartialView(album);
        }

        // GET: Admin/Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return PartialView(album);
        }

        // POST: Admin/Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AlbumName,ParentId")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "AlbumPictures");
            }
            return PartialView(album);
        }

        // GET: Admin/Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return PartialView(album);
        }

        // POST: Admin/Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            string path = Server.MapPath("~/Content/Uploads/AlbumPictures") + "\\" + album.Id;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "AlbumPictures");
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