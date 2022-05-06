using System.Linq;
using System.Web.Mvc;
using Model.Dao;
using System.Web;

namespace WatchShop.Controllers
{
    public class HomeController : Controller
    {

        // GET: /Home/
        public ActionResult Index()
        {
            var productDao = new ProductDao();

            int topFeatureProducts = 4;
            int topNewProducts = 8;
            int topContents = 4;

            ViewBag.listNewContents = new ContentDao().ListNewContent(topContents);
            ViewBag.listNewProducts = productDao.ListNewProduct(topNewProducts);
            ViewBag.listFeatureProducts = productDao.ListFeatureProduct(topFeatureProducts);
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult Banner()
        {
            var result = new BannerDao().ListAll().Where(x => x.Status == true).ToList();

            return PartialView(result);
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var result = new MenuDao().ListByGroupId(1);

            ViewBag.listProductCategory = new ProductCategoryDao().ListAll();
            return PartialView(result);
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }
	}
}