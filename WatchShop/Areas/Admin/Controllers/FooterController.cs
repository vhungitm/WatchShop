using System;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using WatchShop.Utils;

namespace WatchShop.Areas.Admin.Controllers
{
    public class FooterController : BaseController
    {
        [HttpGet]
        [HasCredential(RoleID = "VIEW_FOOTER")]
        public ActionResult Index()
        {
            var result = new FooterDao().GetFooter();
            return View(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "VIEW_FOOTER")]
        public ActionResult Index(Footer Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new FooterDao();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật Footer thành công!", AlertType.Success);
                }
                else
                {
                    SetAlert("Cập nhật Footer không thành công!", AlertType.Error);
                }
            }
            return View();

        }
	}
}