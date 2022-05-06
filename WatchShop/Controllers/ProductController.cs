using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace WatchShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: /Product/
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var result = new ProductDao().ListAllPaging("", page, pageSize, 1);

            return View(result);
        }

        public PartialViewResult ProductCategoryMenu()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }

        public ActionResult ProductCategory(long categoryId, int page = 1, int pageSize = 10)
        {
            var category = new ProductCategoryDao().GetByID(categoryId);
            ViewBag.Category = category;

            var productDao = new ProductDao();
            var model = productDao.GetByCategoryId(categoryId, page, pageSize);
            return View(model);
        }

        public ActionResult Detail(long id)
        {
            var result = new ProductDao().GetByID(id);
            @ViewBag.Category = new ProductCategoryDao().GetByID(result.CategoryID);
            return View(result);
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string keyword, int page = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var model = new ProductDao().ListAllPaging(keyword, page, pageSize);

                ViewBag.Keyword = keyword;
                return View(model);
            }

            return View();
        }
	}
}