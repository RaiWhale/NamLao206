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
                case "TransportFiles": return Redirect("~/Login/Login");
                //break;
                case "News": return Redirect("~/Login/Login");

            }
            return HttpNotFound();
        }
    }
}