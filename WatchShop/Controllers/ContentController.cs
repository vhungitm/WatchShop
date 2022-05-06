using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;

namespace WatchShop.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        public ActionResult Detail(long id)
        {
            var dao = new ContentDao();
            var model = dao.GetByID(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return Redirect("/404/Index.html");
            }
        }

        [ChildActionOnly]
        public PartialViewResult NavContent()
        {
            var dao = new ContentDao();
            ViewBag.listNewContent = dao.ListNewContent(10);
            ViewBag.listTopHotContent = dao.ListTopHotContent(10);
            return PartialView();
        }
	}
}