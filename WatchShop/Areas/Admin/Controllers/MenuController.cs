using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using WatchShop.Utils;

namespace WatchShop.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        [HasCredential(RoleID = "VIEW_MENU")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new MenuDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(result);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_MENU")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_MENU")]
        public ActionResult Create(Menu Entity)
        {
            SetViewBag();

            if (ModelState.IsValid)
            {
                var dao = new MenuDao();
                if (dao.Insert(Entity))
                {
                    SetAlert("Thêm mới menu thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm mới menu không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_MENU")]
        public ActionResult Edit(int id)
        {
            var dao = new MenuDao();
            var result = dao.GetByID(id);

            SetViewBag(result.TypeID);
            if (result != null)
            {
                return View(result);
            }
            else return Redirect("/404/Index.html");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_MENU")]
        public ActionResult Edit(Menu Entity)
        {
            SetViewBag(Entity.TypeID);
            if (ModelState.IsValid)
            {
                var dao = new MenuDao();
                if (dao.Update(Entity))
                {
                    SetAlert("Cập nhật menu thành công!", AlertType.Success);
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Cập nhật menu không thành công!", AlertType.Error);
                }
            }
            return View();
        }

        [HasCredential(RoleID = "DELETE_MENU")]
        public ActionResult Delete(int id)
        {
            var dao = new MenuDao();
            if (dao.Delete(id))
            {
                SetAlert("Xóa menu thành công!", AlertType.Success);
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/404/Index.html");
            }
        }

        // DropdownList
        public void SetViewBag(long? selectedID = null)
        {
            var dao = new MenuTypeDao();
            ViewBag.TypeID = new SelectList(dao.ListAll(), "ID", "Name", selectedID);
        }

        // Thay dổi trạng thái
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new MenuDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}