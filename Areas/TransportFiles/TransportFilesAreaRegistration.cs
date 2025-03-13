using System.Web.Mvc;

namespace NamLao206.Areas.TransportFiles
{
    public class TransportFilesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TransportFiles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TransportFiles_default",
                "TransportFiles/{controller}/{action}/{id}",
                new { controller = "TransportFiles", action = "HopThuDen", id = UrlParameter.Optional },
                 new string[] { "NamLao206.Areas.TransportFiles.Controllers" }
            );
        }
    }
}