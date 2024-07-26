using NamLao206.Models;
using NamLao206.Utils;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    public class AdministratorsController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/Administrators
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            ViewBag.Title = "Tài khoản Admin -";
            var administrators = db.Administrators.Include(a => a.AdminLevel);
            return View(administrators.ToList());
        }

        // GET: Admin/Administrators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // GET: Admin/Administrators/Create
        public ActionResult Create()
        {
            ViewBag.AdminLevelId = new SelectList(db.AdminLevels, "Id", "AdminLevelName");
            return PartialView();
        }

        // POST: Admin/Administrators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AdminName,LoginName,Password,Email,AdminLevelId,IsActive,Profile")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                administrator.Password = MySecurity.Encrypt(administrator.Password);
                db.Administrators.Add(administrator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminLevelId = new SelectList(db.AdminLevels, "Id", "AdminLevelName", administrator.AdminLevelId);
            return View(administrator);
        }

        // GET: Admin/Administrators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminLevelId = new SelectList(db.AdminLevels, "Id", "AdminLevelName", administrator.AdminLevelId);
            return PartialView(administrator);
        }

        // POST: Admin/Administrators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AdminName,LoginName,Password,Email,AdminLevelId,IsActive,Profile")] Administrator administrator, string NewPassword)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    if (MySecurity.Encrypt(NewPassword).Contains(administrator.Password))
                    {
                        ViewBag.Message = "Trùng mật khẩu cũ";
                    }
                    else
                    {
                        administrator.Password = MySecurity.Encrypt(NewPassword);
                    }
                }

                db.Entry(administrator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { message = ViewBag.Message });
            }
            ViewBag.AdminLevelId = new SelectList(db.AdminLevels, "Id", "AdminLevelName", administrator.AdminLevelId);
            return View(administrator);
        }

        // GET: Admin/Administrators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return PartialView(administrator);
        }

        // POST: Admin/Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Administrator administrator = db.Administrators.Find(id);
            db.Administrators.Remove(administrator);
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
