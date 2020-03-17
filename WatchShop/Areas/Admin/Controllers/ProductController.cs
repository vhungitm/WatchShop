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
    public class ProductController : BaseController
    {
        [HasCredential(RoleID = "VIEW_PRODUCT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(result);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_PRODUCT")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_PRODUCT")]
        public ActionResult Create(Product Entity)
        {
            SetViewBag();
            if (ModelState.IsValid)
            {
                Entity.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Username.ToString();  // Người tạo
                Entity.CreatedDate = DateTime.Now;  // Thời gian tạo

                var dao = new ProductDao();
                if (dao.Insert(Entity))
                {
                    SetAlert("Thêm mới sản phẩm thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm mới sản phẩm không thành công!",AlertType.Error);
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit(long id)
        {
            var dao = new ProductDao();
            var result = dao.GetByID(id);
            
            SetViewBag(result.CategoryID);

            if (result != null)
            {
                return View(result);
            }
            else return Redirect("/404/Index.html");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit(Product Entity)
        {
            if (ModelState.IsValid)
            {
                Entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Username.ToString(); // Người cập nhật
                Entity.ModifiedDate = DateTime.Now; // Thời gian cập nhật

                var dao = new ProductDao();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật sản phẩm thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Cập nhật sản phẩm không thành công!", AlertType.Error);
                }
            }
            SetViewBag(Entity.CategoryID);
            return View();
        }

        [HasCredential(RoleID = "DELETE_PRODUCT")]
        public ActionResult Delete(long id)
        {
            var dao = new ProductDao();
            if (dao.Delete(id))
            {
                SetAlert("Xóa sản phẩm thành công!", AlertType.Success);
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/404/Index.html");
            }
        }

        // Thay đổi trạng thái
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}