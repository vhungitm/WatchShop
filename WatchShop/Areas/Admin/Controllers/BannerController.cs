using System;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using WatchShop.Utils;

namespace WatchShop.Areas.Admin.Controllers
{
    public class BannerController : BaseController
    {
        [HasCredential(RoleID = "VIEW_BANNER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new BannerDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(result);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_BANNER")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_BANNER")]
        public ActionResult Create(Banner Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new BannerDao();
                if (dao.Insert(Entity))
                {
                    SetAlert("Thêm mới bìa quảng cáo thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm mới bìa quảng cáo không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_BANNER")]
        public ActionResult Edit(long id)
        {
            var dao = new BannerDao();
            var result = dao.GetByID(id);

            if (result != null)
            {
                return View(result);
            }
            else return Redirect("/404/Index.html");
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_BANNER")]
        public ActionResult Edit(Banner Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new BannerDao();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật bìa quảng cáo thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Cập nhật bìa quảng cáo không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HasCredential(RoleID = "DELETE_BANNER")]
        public ActionResult Delete(long id)
        {
            var dao = new BannerDao();
            if (dao.Delete(id))
            {
                SetAlert("Xóa bìa quảng cáo thành công!", AlertType.Success);
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
            var result = new BannerDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}