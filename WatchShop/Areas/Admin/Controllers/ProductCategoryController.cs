using System;
using System.Linq;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using WatchShop.Utils;
using Common;

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
            SetViewBag();
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_PRODUCT_CATEGORY")]
        public ActionResult Create(ProductCategory Entity)
        {
            SetViewBag(Entity.ID, Entity.ParentID);

            if (ModelState.IsValid)
            {                
                var dao = new ProductCategoryDao();

                Entity.MetaTitle = StringFormat.formatToLink(Entity.Name);
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
                SetViewBag(result.ID, result.ParentID);
                return View(result);
            }
            else return Redirect("/404/Index.html");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_PRODUCT_CATEGORY")]
        public ActionResult Edit(ProductCategory Entity)
        {
            SetViewBag(Entity.ID, Entity.ParentID);

            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();

                Entity.ModifiedBy = ((User)Session[CommonConstants.USER_SESSION]).Username.ToString(); // Người cập nhật
                Entity.ModifiedDate = DateTime.Now; // Thời gian cập nhật
                Entity.MetaTitle = StringFormat.formatToLink(Entity.Name);

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

        public void SetViewBag(long? id = null, long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            var categories = dao.ListAll();
            var firstCategory = new ProductCategory();

            firstCategory.Name = "Chọn danh mục cấp trên";
            categories.Insert(0, firstCategory);

            if (id != null)
            {
                categories = categories.Where(x => x.ID != id).ToList();
            }

            ViewBag.ParentID = new SelectList(categories, "ID", "Name", selectedId);
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