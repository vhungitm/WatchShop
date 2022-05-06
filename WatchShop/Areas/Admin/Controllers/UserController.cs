using System;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using Common;
using WatchShop.Utils;

namespace WatchShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(result);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User Entity)
        {
            SetViewBag();

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.GetByPhone(Entity.Phone) != null)
                {
                    SetAlert("Số điện thoại này đã được sử dụng trước đó!", AlertType.Error);
                    return View();
                }

                if (dao.GetByEmail(Entity.Email) != null)
                {
                    SetAlert("Email này đã được đăng ký trước đó!", AlertType.Error);
                    return View();
                }

                Entity.Username = Entity.Phone;
                Entity.Password = Encryptor.MD5Hash(Entity.Password);
                Entity.CreatedBy = ((User)Session[CommonConstants.USER_SESSION]).Username.ToString();  // Người tạo
                Entity.CreatedDate = DateTime.Now;  // Thời gian tạo

                if (dao.Insert(Entity))
                {
                    SetAlert("Thêm người dùng thành công!", AlertType.Success);
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAlert("Thêm người dùng không thành công!", AlertType.Error);
                }
            }
            return View("Create");
        }


        [HttpGet]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(long id)
        {
            var Entity = new UserDao().GetById(id);
            
            SetViewBag(Entity.GroupID);
            Entity.Password = null;

            return View(Entity);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User Entity)
        {
            SetViewBag(Entity.GroupID);

            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                var user = dao.GetById(Entity.ID);
                var userCheckPhone = dao.GetByPhone(Entity.Phone);

                if (userCheckPhone != null && user.Phone != userCheckPhone.Phone)
                {
                    SetAlert("Số điện thoại này đã được người khác sử dụng!", AlertType.Error);
                    return View();
                }

                var userCheckEmail = dao.GetByEmail(Entity.Email);

                if (userCheckEmail != null && user.Email != userCheckEmail.Email)
                {
                    SetAlert("Email này đã được người khác sử dụng!", AlertType.Error);
                    return View();
                }

                Entity.Username = Entity.Phone;
                Entity.ModifiedBy = ((User)Session[CommonConstants.USER_SESSION]).Username.ToString();  // Người cập nhật
                Entity.ModifiedDate = DateTime.Now;  // Thời gian cập nhật
                Entity.Password = Entity.Password != null ? Encryptor.MD5Hash(Entity.Password) : null;

                var res = dao.Update(Entity);

                if (res)
                {
                    SetAlert("Cập nhật thông tin người dùng thành công!", AlertType.Success);
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAlert("Cập nhật thông tin người dùng không thành công!", AlertType.Error);
                }
                
            }

            return View("Edit");
        }

        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(long id)
        {
            var dao = new UserDao();
            dao.Delete(id);
            SetAlert("Xóa người dùng thành công!", AlertType.Success);
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var dao = new UserDao();
            var res = dao.ChangStatus(id);
            return Json(new
            {
                status = res
            });
        }
        public void SetViewBag()
        {
            var dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "ID", null);
        } 
        public void SetViewBag(string selectedId)
        {
            var dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "ID", selectedId);
        }
    }
}