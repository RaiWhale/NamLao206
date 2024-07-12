using NamLao206.Models;
using NamLao206.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Controllers
{
	public class HomeController : Controller
	{
		namlao206dbEntities db = new namlao206dbEntities();
		public ActionResult Trangchu()
		{
			ViewBag.Title = "Trang chủ -";


			ViewBag.news_hot = db.News.Where(x => x.Topic.NhomNews != 1 && x.Duyet == true).OrderByDescending(x => x.uutien).ThenByDescending(x => x.DateUp).Take(10).ToList();
			ViewBag.banner = db.Banners.Where(x => x.isActive == true).OrderByDescending(x => x.Id).Take(5).ToList();
			ViewBag.calrouselPicture = db.CalrouselPictures.OrderByDescending(x => x.Id).Take(20).ToList();
			
			return View();
		}
		public ActionResult Lienhe()
		{
			ViewBag.Title = "Liên hệ -";
			return View();
		}
		public ActionResult PhongBan(int khoaid)
		{
			//IQueryable<DM_Khoaphongs> khoaphongs;

			var id = db.DM_PhongBans.Where(x => x.Id == khoaid).FirstOrDefault();
			if (id != null)
			{
				ViewBag.Title = id.TenKhoa + " -";
				ViewBag.khoa = khoaid;
				ViewBag.nhomkhoa = db.DM_NhomPhongBans.ToList();
				ViewBag.KhoaPhong = db.DM_PhongBans.Where(x => x.DM_NhomPhongBans.ParentId == 1).ToList();
				return View(id);
			}
			else
			{
				return RedirectToAction("ErrorCustome");
			}
		}

		public ActionResult DetailGioiThieu(string newsid)
		{
			var nid = db.News.Where(x => x.TopicId.ToString().Equals(newsid) && x.Duyet == true).FirstOrDefault();
			if (nid == null)
			{
				return RedirectToAction("ErrorCustome");
			}
			ViewBag.Title = nid.Title;
			ViewBag.Summary = nid.Summary;
			ViewBag.urlPic = MySecurity.GetSrcPicture(nid.TopicId);
			ViewBag.KhoaPhong = db.DM_PhongBans.Where(x => x.DM_NhomPhongBans.ParentId == 1).ToList();
			return View(nid);
		}
	}
}
