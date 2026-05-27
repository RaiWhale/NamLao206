using System.Web.Mvc;

namespace NamLao206.Areas.KhachSan
{
    public class KhachSanAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "KhachSan";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KhachSan_default",
                "KhachSan/{controller}/{action}/{id}",
                 new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                 new string[] { "NamLao206.Areas.KhachSan.Controllers" }
            );
        }
    }
}