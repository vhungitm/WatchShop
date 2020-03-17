using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace WatchShop.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        [HasCredential(RoleID = "VIEW_FEEDBACK")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var result = new FeedbackDao().ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(result);
        }

        [HasCredential(RoleID = "VIEW_DETAIL_FEEDBACK")]
        public ActionResult View(long id)
        {
            var result = new FeedbackDao().GetByID(id);
            if (result != null)
            {
                return View(result);
            }
            return Redirect("/404/Index.html");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new FeedbackDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
	}
}