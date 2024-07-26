using NamLao206.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class SubMenusController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();

        // GET: Admin/SubMenus
        public ActionResult Index()
        {
			ViewBag.Title = "Menu phụ -";
			var subMenus = db.SubMenus.Include(s => s.Topic);
            return View(db.SubMenus.ToList());
        }


        // GET: Admin/SubMenus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubMenu subMenu = db.SubMenus.Find(id);
            if (subMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName");
            return PartialView(subMenu);
        }

        // GET: Admin/SubMenus/Create
        public ActionResult Create(int? id, int? topicId)
        {
            var subMenu = new SubMenu()
            {
                TopicId = topicId != null ? topicId.Value : 0,
                Id = id != null ? id.Value : 0
            };

            ViewBag.subMenuId = new SelectList(db.SubMenus, "Id", "subMenuName", new { id = id, topicId = topicId });
            return PartialView(subMenu);
        }

        // POST: Admin/SubMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,subMenuName,ParentId,TopicId,UserId")] SubMenu subMenu, int? id, int topicId)
        {
            if (ModelState.IsValid)
            {
                subMenu.ParentId = id != null ? id.Value : 0;
                subMenu.TopicId = topicId;
                subMenu.UserId = int.Parse(User.Identity.Name);
                db.SubMenus.Add(subMenu);
                db.SaveChanges();
                return RedirectToAction("Index", "Topics", id = subMenu.TopicId);
            }

            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName", subMenu.TopicId);
            return View(subMenu);
        }

        // GET: Admin/SubMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubMenu subMenu = db.SubMenus.Find(id);
            if (subMenu == null)
            {
                return HttpNotFound();
            }
            var user = db.Administrators.Find(int.Parse(User.Identity.Name));
            if (subMenu.UserId != user.Id && !user.AdminLevel.AdminLevelName.Equals("Admin"))
            {
                return PartialView("Error");
            }
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName", subMenu.TopicId);
            return PartialView(subMenu);
        }

        // POST: Admin/SubMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,subMenuName,ParentId,TopicId,UserId")] SubMenu subMenu)
        {
            if (ModelState.IsValid)
            {
                var user = db.Administrators.Find(int.Parse(User.Identity.Name));


                var sub = db.SubMenus.Find(subMenu.Id);

                if (sub.UserId != user.Id && !user.AdminLevel.AdminLevelName.Equals("Admin"))
                {
                    return Content("Ban khong co quyen chinh sua");
                }
                sub.subMenuName = subMenu.subMenuName;
                sub.TopicId = subMenu.TopicId;
                sub.UserId = user.Id;
                //category.UserId = int.Parse(User.Identity.Name);///Truong hop admin edit thi lay admin gan vao
                //db.Entry(subMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Topics");
            }
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName", subMenu.TopicId);
            return PartialView(subMenu);
        }

        // GET: Admin/SubMenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubMenu subMenu = db.SubMenus.Find(id);
            if (subMenu == null)
            {
                return HttpNotFound();
            }
            return PartialView(subMenu);
        }

        // POST: Admin/SubMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            SubMenu subMenu = db.SubMenus.Find(id);
            var subInSub = db.SubMenus.Where(x => x.ParentId == subMenu.Id);
            if (subInSub.Count() > 0 && subMenu.ParentId == 0)
            {
                ViewBag.Message = "Có Menu con không thể xóa";
            }
            else
            {
                db.SubMenus.Remove(subMenu);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Topics", new { message = ViewBag.Message });
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
