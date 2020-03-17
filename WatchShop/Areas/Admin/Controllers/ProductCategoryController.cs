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
    public class ProductCategoryController : BaseController
    {
        [HasCredential(RoleID = "VIEW_PRODUCT_CATEGORY")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductCategoryDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(result);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_PRODUCT_CATEGORY")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_PRODUCT_CATEGORY")]
        public ActionResult Create(ProductCategory Entity)
        {
            if (ModelState.IsValid)
            {
                Entity.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Username.ToString();  // Người tạo
                Entity.CreatedDate = DateTime.Now;  // Thời gian tạo

                var dao = new ProductCategoryDao();
                if (dao.Insert(Entity))
                {
                    SetAlert("Thêm mới danh mục sản phẩm thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm mới danh mục sản phẩm không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_PRODUCT_CATEGORY")]
        public ActionResult Edit(long id)
        {
            var dao = new ProductCategoryDao();
            var result = dao.GetByID(id);

            if (result != null)
            {
                return View(result);
            }
            else return Redirect("/404/Index.html");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_PRODUCT_CATEGORY")]
        public ActionResult Edit(ProductCategory Entity)
        {
            if (ModelState.IsValid)
            {
                Entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Username.ToString(); // Người cập nhật
                Entity.ModifiedDate = DateTime.Now; // Thời gian cập nhật

                var dao = new ProductCategoryDao();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật danh mục sản phẩm thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Cập nhật danh mục sản phẩm không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HasCredential(RoleID = "DELETE_PRODUCT_CATEGORY")]
        public ActionResult Delete(long id)
        {
            var dao = new ProductCategoryDao();
            if (dao.Delete(id))
            {
                SetAlert("Xóa danh mục sản phẩm thành công!", AlertType.Success);
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
            var result = new ProductCategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpPost]
        public JsonResult ChangeShowOnHome(long id)
        {
            var result = new ProductCategoryDao().ChangeShowOnHome(id);
            return Json(new
            {
                status = result
            });
        }
    }
}