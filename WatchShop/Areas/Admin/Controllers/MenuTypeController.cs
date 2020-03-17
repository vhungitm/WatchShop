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
    public class MenuTypeController : BaseController
    {
        [HasCredential(RoleID = "VIEW_MENU_TYPE")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new MenuTypeDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(result);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_MENU_TYPE")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_MENU_TYPE")]
        public ActionResult Create(MenuType Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new MenuTypeDao();
                if (dao.Insert(Entity))
                {
                    SetAlert("Thêm mới loại menu thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm mới loại menu không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_MENU_TYPE")]
        public ActionResult Edit(int id)
        {
            var dao = new MenuTypeDao();
            var result = dao.GetByID(id);

            if (result != null)
            {
                return View(result);
            }
            else return Redirect("/404/Index.html");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_MENU_TYPE")]
        public ActionResult Edit(MenuType Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new MenuTypeDao();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật loại menu thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Cập nhật loại menu không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HasCredential(RoleID = "DELETE_MENU_TYPE")]
        public ActionResult Delete(int id)
        {
            var dao = new MenuTypeDao();
            if (dao.Delete(id))
            {
                SetAlert("Xóa loại menu thành công!", AlertType.Success);
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/404/Index.html");
            }
        }
    }
}