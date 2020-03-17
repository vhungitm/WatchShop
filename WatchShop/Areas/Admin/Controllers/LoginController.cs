using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShop.Areas.Admin.Models;
using Model.Dao;
using WatchShop.Common;

namespace WatchShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.Username, Encryptor.MD5Hash(model.Password), true);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else if (result == 1)
                {
                    var user = new UserDao().GetByUsername(model.Username);
                    var userSession = new UserLogin();

                    userSession.UserID = user.ID;
                    userSession.Username = user.Username;
                    userSession.Name = user.Name;
                    userSession.GroupID = user.GroupID;

                    var listCredentials = dao.GetListCredential(model.Username);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản này đang bị khóa!");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Sai mật khẩu!");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Không có quyền truy cập!");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng!");
                }
            }
            return View("Index");
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}