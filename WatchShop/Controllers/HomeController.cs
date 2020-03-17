using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace WatchShop.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var productDao = new ProductDao();

            int top = 6;
            ViewBag.listNewContents = new ContentDao().ListNewContent(3);
            ViewBag.listNewProducts = productDao.ListNewProduct(top);
            ViewBag.listFeatureProducts = productDao.ListFeatureProduct(top);
            return View();
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var result = new MenuDao().ListByGroupId(1);

            ViewBag.listProductCategory = new ProductCategoryDao().ListAll();
            return PartialView(result);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public PartialViewResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }
	}
}