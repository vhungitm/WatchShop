using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using WatchShop.Common;
namespace WatchShop.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        [HasCredential(RoleID = "VIEW_CONTACT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContactDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(result);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_CONTACT")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]

        [HasCredential(RoleID = "ADD_CONTACT")]
        public ActionResult Create(Contact Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContactDao();
                if (dao.Insert(Entity))
                {
                    SetAlert("Thêm mới thông tin liên lạc thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm mới thông tin liên lạc không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_CONTACT")]
        public ActionResult Edit(long id)
        {
            var dao = new ContactDao();
            var result = dao.GetByID(id);

            if (result != null)
            {
                return View(result);
            }
            else return Redirect("/404/Index.html");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_CONTACT")]
        public ActionResult Edit(Contact Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContactDao();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật thông tin liên lạc thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Cập nhật thông tin liên lạc không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HasCredential(RoleID = "DELETE_CONTACT")]
        public ActionResult Delete(long id)
        {
            var dao = new ContactDao();
            if (dao.Delete(id))
            {
                SetAlert("Xóa thông tin liên lạc thành công!", AlertType.Success);
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/404/Index.html");
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ContactDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}