using System;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using Common;
using WatchShop.Utils;

namespace WatchShop.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        [HasCredential(RoleID = "VIEW_CONTENT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(result);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_CONTENT")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_CONTENT")]
        public ActionResult Create(Content Entity)
        {
            if (ModelState.IsValid)
            {
                Entity.CreatedBy = ((User)Session[CommonConstants.USER_SESSION]).Username.ToString();
                Entity.CreatedDate = DateTime.Now;
                Entity.MetaTitle = StringFormat.formatToLink(Entity.Name);

                var dao = new ContentDao();
                if (dao.Insert(Entity))
                {
                    SetAlert("Thêm mới tin tức thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm mới tin tức không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Edit(long id)
        {
            var dao = new ContentDao();
            var result = dao.GetByID(id);

            if (result != null)
            {
                return View(result);
            }
            else return Redirect("/404/Index.html");
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Edit(Content Entity)
        {
            if (ModelState.IsValid)
            {
                Entity.ModifiedBy = ((User)Session[CommonConstants.USER_SESSION]).Username.ToString();
                Entity.ModifiedDate = DateTime.Now;
                Entity.MetaTitle = StringFormat.formatToLink(Entity.Name);

                var dao = new ContentDao();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật tin tức thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Cập nhật tin tức không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HasCredential(RoleID = "DELETE_CONTENT")]
        public ActionResult Delete(long id)
        {
            var dao = new ContentDao();
            if (dao.Delete(id))
            {
                SetAlert("Xóa tin tức thành công!", AlertType.Success);
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
            var result = new ContentDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}