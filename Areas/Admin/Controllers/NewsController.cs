using PagedList;
using NamLao206.Models;
using NamLao206.Models.ViewModels;
using NamLao206.Utils;
using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();
        int pageSize = 10;
        // GET: Admin/News
        public ActionResult Index(int? page, string search)
        {
			ViewBag.Title = "Tin tức -";
			IQueryable<News> news; //model chinh        

            news = db.News.OrderByDescending(x => x.uutien).ThenByDescending(x => x.DateUp);
            if (search != null && search.Trim() != "")
            {
                news = news.Where(s => s.Title.Trim().ToLower().Contains(search.Trim().ToLower()));
            }
            ViewBag.search = search;
            //Paging
            int pageNumber = page ?? 1;
            return View(news.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return PartialView(news);
        }

        // GET: Admin/News/Create

        public ActionResult Create()
        {
            ViewBag.Title = "Đăng tin";
            ViewBag.cosoId = new SelectList(db.Transports, "Id", "Coso");
            ViewBag.AdminId = new SelectList(db.Administrators, "Id", "AdminName");
            ViewBag.TopicId = new SelectList(db.Topics.Where(x => x.NhomNews != 1), "Id", "TopicName");
            ViewBag.SubMenuId = new SelectList(db.SubMenus, "Id", "subMenuName");
            return View();
        }

        public ActionResult Create2()
        {
            ViewBag.Title = "Đăng tin";
            ViewBag.cosoId = new SelectList(db.Transports, "Id", "Coso");
            ViewBag.AdminId = new SelectList(db.Administrators, "Id", "AdminName");
            ViewBag.TopicId = new SelectList(db.Topics.Where(x => x.NhomNews != 1), "Id", "TopicName");
            ViewBag.SubMenuId = new SelectList(db.SubMenus, "Id", "subMenuName");
            return View();
        }
        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Summary,Details,TopicId,AdminId,Views,Picture,SubMenuId,cosoId,TitleChange,Author")] News news, HttpPostedFileBase pic, string DateUp)
        {
            
            if (ModelState.IsValid)
            {
                int acc_id = int.Parse(User.Identity.Name);
                string filename = "";
                if (pic != null)
                {
                    filename = DateTime.Now.Ticks + "_" + pic.FileName.Split('/').Last();
                    string path = Server.MapPath("~/Content/Uploads/News");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    pic.SaveAs(path + "\\" + filename);
                }
				news.DateUp = DateTime.ParseExact(DateUp, "dd-MM-yyyy", CultureInfo.InvariantCulture);
				news.TitleChange = MySecurity.RemoveDiacritics(news.Title);
                news.uutien = false;
                news.Duyet = true;
                news.Picture = filename;
                news.AdminId = acc_id;               
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cosoId = new SelectList(db.Transports, "Id", "Coso", news.cosoId);
            ViewBag.AdminId = new SelectList(db.Administrators, "Id", "AdminName", news.AdminId);
            ViewBag.TopicId = new SelectList(db.Topics.Where(x => x.NhomNews != 1), "Id", "TopicName", news.TopicId);
            ViewBag.SubMenuId = new SelectList(db.SubMenus, "Id", "subMenuName", news.SubMenuId);
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public ActionResult Edit(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page;
			ViewBag.cosoId = new SelectList(db.Transports, "Id", "Coso", news.cosoId);
            ViewBag.AdminId = new SelectList(db.Administrators, "Id", "AdminName", news.AdminId);
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName", news.TopicId);
            ViewBag.SubMenuId = new SelectList(db.SubMenus.Where(x => x.TopicId == news.TopicId), "Id", "subMenuName", news.SubMenuId);
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Summary,Details,TopicId,AdminId,Views,Picture,SubMenuId,cosoId,uutien,Duyet,Author")] News news, HttpPostedFileBase pic, string DateUp, int? page)
        {
            if (ModelState.IsValid)
            {
                int acc_id = int.Parse(User.Identity.Name);
                news.AdminId = acc_id;
                if (pic != null)
                {
                    string filename = DateTime.Now.Ticks + "_" + pic.FileName.Split('/').Last();
                    string path = Server.MapPath("~/Content/Uploads/News");
                    if (System.IO.File.Exists(path + "\\" + news.Picture))
                    {
                        System.IO.File.Delete(path + "\\" + news.Picture);
                    }

                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    pic.SaveAs(path + "\\" + filename);
                    news.Picture = filename;
                }
				news.DateUp = DateTime.ParseExact(DateUp, "dd-MM-yyyy", CultureInfo.InvariantCulture);
				news.TitleChange = MySecurity.RemoveDiacritics(news.Title);
                news.DateModified = DateTime.Now;
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();			
				return RedirectToAction("Index", new { page = page });
            }
            ViewBag.cosoId = new SelectList(db.Transports, "Id", "Coso", news.cosoId);
            ViewBag.AdminId = new SelectList(db.Administrators, "Id", "AdminName", news.AdminId);
            ViewBag.TopicId = new SelectList(db.Topics.Where(x => x.NhomNews != 1), "Id", "TopicName", news.TopicId);
            ViewBag.SubMenuId = new SelectList(db.SubMenus.Where(x => x.TopicId == news.TopicId), "Id", "subMenuName", news.SubMenuId);
            return View(news);
        }

        // GET: Admin/News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return PartialView(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            string path = Server.MapPath("~/Content/Uploads/News");
            if (System.IO.File.Exists(path + "\\" + news.Picture))
            {
                System.IO.File.Delete(path + "\\" + news.Picture);
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
