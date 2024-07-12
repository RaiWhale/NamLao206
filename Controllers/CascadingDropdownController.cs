using NamLao206.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PhusannhiHospital.Controllers
{
    public class CascadingDropdownController : Controller
    {
        private namlao206dbEntities db = new namlao206dbEntities();
        // GET: CascadingDropdown
        public JsonResult GetCityList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<DM_Donvihanhchinhs> CityList = db.DM_Donvihanhchinhs.Where(x => x.ParentId == "0").ToList();
            return Json(CityList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDistrictList1()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<DM_Donvihanhchinhs> DistrictList = db.DM_Donvihanhchinhs.Where(x => x.ParentId == "7237").ToList();
            return Json(DistrictList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDistrictList(string sourceId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<DM_Donvihanhchinhs> DistrictList = db.DM_Donvihanhchinhs.Where(x => x.ParentId == sourceId).ToList();
            return Json(DistrictList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWardList(string sourceId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<DM_Donvihanhchinhs> WardList = db.DM_Donvihanhchinhs.Where(x => x.ParentId == sourceId).ToList();
            return Json(WardList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSubmenusList(int? sourceId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SubMenu> SubmenusList = db.SubMenus.Where(x => x.TopicId == sourceId).ToList();
            return Json(SubmenusList, JsonRequestBehavior.AllowGet);

        }
    }
}