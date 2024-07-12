using System.Web.Mvc;

namespace PhusannhiHospital.Controllers
{
    public class LoginRedirectController : Controller
    {
        // GET: LoginRedirect
        public ActionResult Trangchu(string returnURL)
        {
            string[] d = returnURL.Split('/');
            switch (d[1])
            {
                case "Admin": return Redirect("~/Admin/Account/Login");
                //break;
                case "QuanLyNhanVien": return Redirect("~/Login/Login");
                //break;
                case "tkyt": return Redirect("~/Login/Login");
                //break;
                case "Baocao": return Redirect("~/Login/Login");
                //break;
                case "Login": return Redirect("~/Login/Login");
                //break;
                case "HenTiemVaccin": return Redirect("~/Login/Login");
                //break;
                case "TraCuuXetNghiem": return Redirect("~/Login/Login");
            }
            return HttpNotFound();
        }
    }
}