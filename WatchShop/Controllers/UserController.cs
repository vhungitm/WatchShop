using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShop.Models;
using Model.EF;
using Model.Dao;
using BotDetect.Web.Mvc;
using WatchShop.Common;
using Facebook;
using System.Configuration;
namespace WatchShop.Controllers
{
    public class UserController : Controller
    {
        public Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUsername(model.Username))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.Username = model.Username;
                    user.Password = Common.Encryptor.MD5Hash(model.Password);
                    user.Name = model.Name;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;

                    if (dao.Insert(user))
                    {
                        ViewBag.Success = "Đăng ký thành công!";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("","Đăng ký không thành công!");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.Username, Encryptor.MD5Hash(model.Password));
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
                    return Redirect("/");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản này đang bị khóa!");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Sai mật khẩu!");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic resutl = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = resutl.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;

                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string username = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.Username = username;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.Status = true;
                user.CreatedDate = DateTime.Now;
                
                var resultInsertUser = new UserDao().InsertForFaceBook(user);

                var userSession = new UserLogin();

                userSession.UserID = user.ID;
                userSession.Username = user.Username;
                userSession.Name = user.Name;

                Session.Add(CommonConstants.USER_SESSION, userSession);
            }
            return Redirect("/");
        }
	}
}