using NamLao206.Models;
using NamLao206.Utils;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Controllers
{
	public class HomeController : Controller
	{
		namlao206dbEntities db = new namlao206dbEntities();
		int pageSize = 0;
		public ActionResult Trangchu()
		{
            int pageNumber = 2; // Adjust this based on your current page (starts from 1)
            int pageSize = 10; // Number of items per page
            ViewBag.Title = "Trang chủ -";


			ViewBag.hot_News = db.News.Where(x => x.Topic.NhomNews != 1 && x.Duyet == true && x.uutien == true).OrderByDescending(x => x.DateUp).Take(10).ToList();
			ViewBag.hot_News_KhongUuTien =db.News.Where(x => x.Topic.NhomNews != 1 && x.TopicId != 13 && x.Duyet == true && x.uutien == false)
				.OrderByDescending(x => x.DateUp)
				.Take(pageSize).ToList();
            ViewBag.hot_News_KhongUuTienSkip = db.News.Where(x => x.Topic.NhomNews != 1 && x.TopicId != 13 && x.Duyet == true && x.uutien == false)
                .OrderByDescending(x => x.DateUp)
				.Skip((pageNumber - 1) * pageSize) // Skip based on page number
				.Take(5).ToList();


            ViewBag.banner = db.AlbumPictures.Where(x => x.AlbumId == 4).OrderByDescending(x => x.Id).ToList();		
			ViewBag.chuyenDoiSo_News = db.News.Where(x=>x.TopicId == 9 && x.Duyet == true).OrderByDescending(x => x.DateUp).Take(10).ToList();

			ViewBag.doingoaiNews = db.News.Where(x => x.TopicId == 14 && x.Duyet == true).OrderByDescending(x => x.DateUp).Take(10).ToList();
            ViewBag.anninh_News = db.News.Where(x => x.TopicId == 2 && x.Duyet == true).OrderByDescending(x => x.DateUp).Take(3).ToList();
			ViewBag.thau_News = db.News.Where(x => x.TopicId == 13 && x.Duyet == true).OrderByDescending(x => x.DateUp).Take(10).ToList();
            ViewBag.doanthe_News = db.News.Where(x => x.TopicId == 5 && x.Duyet == true).OrderByDescending(x => x.DateUp).Take(10).ToList();

            ViewBag.carousel_Chief = db.AlbumPictures.Where(x => x.AlbumId == 1).OrderByDescending(x => x.Id).Take(20).ToList(); /*AlbumId=1 => Ban lãnh đạo*/
            ViewBag.linhvuckinhdoanh = db.AlbumPictures.Where(x => x.AlbumId == 2).OrderByDescending(x => x.Id).Take(20).ToList(); /*AlbumId=2 => linhvuckinhdoanh*/
            return View();
		}
		public ActionResult Lienhe()
		{
			ViewBag.Title = "Liên hệ -";
			return View();
		}
		public ActionResult PhongBan(int khoaid)
		{
			//IQueryable<DM_PhongBans> khoaphongs;

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
		public ActionResult NhomNews(string topic, int? submenu, int? page, string search)
		{
			pageSize = 9;
			ViewBag.topic = db.Topics.ToList();
			IQueryable<News> newsid = db.News.Where(s => s.Duyet == true);

			if (!string.IsNullOrEmpty(search))
			{
				newsid = newsid.Where(s => s.Title.Trim().ToLower().Contains(search.Trim().ToLower())).OrderByDescending(x => x.DateUp);
			}
			else if (!string.IsNullOrEmpty(topic))
			{
				newsid = newsid.Where(x => x.TopicId.ToString().Equals(topic) && x.Duyet == true).OrderByDescending(x => x.DateUp);
				var topicname = db.Topics.Where(x => x.Id.ToString().Equals(topic)).FirstOrDefault();
				ViewBag.TopicName = topicname.TopicName;
			}
			else if (submenu != null)
			{
				newsid = newsid.Where(x => x.SubMenuId == submenu && x.Duyet == true).OrderByDescending(x => x.DateUp);
				var submenuName = db.SubMenus.Where(x => x.Id == submenu).FirstOrDefault();
				ViewBag.TopicName = submenuName.subMenuName;
			}

			//Paging
			int pageNumber = page ?? 1;

			ViewBag.Title = "Tin tức";
			ViewBag.search = search;
			ViewBag.topic = topic;
			ViewBag.submenu = submenu;
			ViewBag.topics = db.Topics.Where(x => x.NhomNews.Value == 2 || x.NhomNews.Value == 3).ToList();
			ViewBag.RecentNews = db.News.OrderByDescending(x => x.Id).Take(4).ToList();
			return View(newsid.ToPagedList(pageNumber, pageSize));
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
		public ActionResult NewsDetail(int? newsid, string title)
		{
			var nid = db.News.Where(x => x.Id == newsid && x.Duyet == true).SingleOrDefault();
			if (nid == null)
			{
				return RedirectToAction("ErrorCustome");
			}
			ViewBag.Title = nid.Title;
			ViewBag.Summary = nid.Summary;
			ViewBag.urlPic = MySecurity.GetSrcPicture(nid.TopicId);		
			ViewBag.latest_news = db.News.Where(x => x.SubMenuId == nid.SubMenuId).OrderByDescending(x => x.Id).Take(6).ToList();
			ViewBag.topics = db.Topics.Where(x => x.NhomNews.Value == 2 || x.NhomNews.Value == 3).ToList();
			ViewBag.RecentNews = db.News.Where(x => x.TopicId == nid.TopicId).OrderByDescending(x => x.Id).Take(8).ToList();
			return View(nid);
		}
	}
}
