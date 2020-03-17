using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using WatchShop.Common;
using PagedList;
using WatchShop.Areas.Admin.Models;

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
                if (dao.GetByUsername(Entity.Username) != null)
                {
                    SetAlert("Tài khoản này đã tồn tại", AlertType.Error);
                    return View("Create");
                }
                var EnCryptedMd5Pas = Encryptor.MD5Hash(Entity.Password);
                Entity.Password = EnCryptedMd5Pas;
                Entity.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Username.ToString();  // Người tạo
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
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Edit(long id)
        {
            var dao = new UserDao().GetById(id);
            SetViewBag(dao.GroupID);
            return View(dao);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                Entity.Password = Encryptor.MD5Hash(Entity.Password); // Mã hóa mật khẩu
                Entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).Username.ToString();  // Người cập nhật
                Entity.ModifiedDate = DateTime.Now;  // Thời gian cập nhật

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
            SetViewBag(Entity.GroupID);
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