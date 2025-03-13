using NamLao206.Models;
using NamLao206.Models.ViewModels;
using NamLao206.Utils;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NamLao206.Controllers
{
    public class LoginController : Controller
    {
        namlao206dbEntities db = new namlao206dbEntities();

        [Authorize]
        public ActionResult UpdateProfile()
        {
            var acc = db.Accounts.Find(int.Parse(User.Identity.Name));
            if (acc == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                //if (acc.AccountType == 2 || acc.AccountType == 3)
                //{
                    var user = db.Employees.Where(x => x.Id == acc.EmployeeId).SingleOrDefault();
                    var data = AutoMapper.Mapper.Map<UpdateProfileVM>(user);
                    ViewBag.Chucvu = user.DM_Chucvus.Chucvu;
                    return View(data);
                //}
                //if (acc.AccountType == 1)
                //{
                //    var user = db.Customers.Where(x => x.Id == acc.EmployeeId).FirstOrDefault();
                //    var data = AutoMapper.Mapper.Map<UpdateProfileVM>(user);
                //    ViewBag.Chucvu = "";
                //    return View(data);
                //}
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult UpdateProfile(UpdateProfileVM data)
        {
            ViewBag.Title = "Cập nhật";
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdatePassword(string CurrentPassword, string NewPassword, string RetypeNewPassword)
        {
            int acc_id = int.Parse(User.Identity.Name);
            Account acc = db.Accounts.Find(acc_id);
            if (acc == null)
            {
                return HttpNotFound();
            }
            try
            {
                if (!string.IsNullOrEmpty(CurrentPassword))
                {
                    if (!acc.Password.Equals(MySecurity.Encrypt(CurrentPassword)))
                    {
                        ViewBag.Message = "Mật khẩu hiện tại không đúng!";
                        return View(acc);
                    }
                    if (string.IsNullOrEmpty(NewPassword) || NewPassword.Length < 3)
                    {
                        ViewBag.Message = "Mật khẩu mới không hợp lệ!";
                        return View(acc);
                    }
                    if (!NewPassword.Equals(RetypeNewPassword))
                    {
                        ViewBag.Message = "Mật khẩu mới không trùng khớp!";
                        return View(acc);
                    }
                    acc.Password = MySecurity.Encrypt(NewPassword);
                }
                db.SaveChanges();
                ViewBag.Message = "Cập nhật thành công!";
            }
            catch (Exception ex) { ViewBag.Message = ex.Message; }
            return View(acc);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Title = "Đăng nhập";
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(LoginVM data)
        {
            data.Password = MySecurity.Encrypt(data.Password);
            var acc = db.Accounts.SingleOrDefault(x => x.LoginName.Equals(data.LoginName) && x.Password.Equals(data.Password));
            if (acc != null)
            {
                if (acc.IsActive)
                {
                    FormsAuthentication.SetAuthCookie(acc.Id.ToString(), false);
                    if(acc.AccountType == 3)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    return RedirectToAction("Trangchu", "Home");
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

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UpdateProfile");
            };
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM data, HttpPostedFileBase pic)
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login","Login");
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