using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Common;
using Model.Dao;
using Model.EF;
using WatchShop.Utils;

namespace WatchShop.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        [HttpGet]
        [HasCredential(RoleID = "VIEW_CONTACT")]
        public ActionResult Index()
        {
            var result = new ContactDAO().GetContact();
            return View(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "VIEW_CONTACT")]
        public ActionResult Index(Contact Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContactDAO();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật thông tin liên hệ thành công!", AlertType.Success);
                }
                else
                {
                    SetAlert("Cập nhật thông tin liên hệ không thành công!", AlertType.Error);
                }
            }
            return View();

        }
    }
}