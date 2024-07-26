using NamLao206.Models;
using NamLao206.Models.ViewModels;
using NamLao206.Utils;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace NamLao206.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();


        public ActionResult Index()
        {
            ViewBag.Title = "Tài khoản -";
            return View(db.Accounts.ToList());
        }

        // GET: Admin/Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return PartialView(account);
        }
        // POST: Admin/Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountType,EmployeeId,LoginName,Password,ActivateCode,Coso,Token,IsActive")] Account account, string rePassword, string rePassword2)
        {
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(rePassword))
                {
                    account.Password = MySecurity.Encrypt(rePassword);
                }

                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(account);
        }
        // GET: Admin/Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(LoginVM data)
        {
            data.Password = MySecurity.Encrypt(data.Password);
            var acc = db.Administrators.SingleOrDefault(x => x.LoginName.Equals(data.LoginName) && x.Password.Equals(data.Password));
            if (acc != null)
            {
                if (acc.IsActive)
                {
                    FormsAuthentication.SetAuthCookie(acc.Id.ToString(), false);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.Message = "Tài khoản đã bị khóa!";
                }
            }
            else
            {
                ViewBag.Message = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
            return View();
        }


        // GET: Admin/Account
        public ActionResult UpdateProfile()
        {
            int acc_id = int.Parse(User.Identity.Name);
            Administrator acc = db.Administrators.Find(acc_id);
            if (acc == null)
            {
                return HttpNotFound();
            }
            return View(acc);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
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