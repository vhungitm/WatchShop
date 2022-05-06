using System;
using System.Web.Mvc;
using Model;
using Model.EF;
using Model.Dao;
using Facebook;
using System.Configuration;
using Common;

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
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                model.Username = model.Email;

                if (dao.CheckUsername(model.Phone))
                {
                    ViewBag.Error = "Số điện thoại đã tồn tại";
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ViewBag.Error = "Email đã tồn tại";
                }
                else
                {
                    var MemberGroupId = new UserGroupDao().FindByID("MEMBER").ID;

                    var user = new User();
                    user.Username = model.Username;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.GroupID = MemberGroupId;
                    user.Name = model.Name;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;

                    if (dao.Insert(user))
                    {
                        ViewBag.Success = "Đăng ký thành công!";
                    }
                    else
                    {
                        ViewBag.Error = "Đăng ký không thành công!";
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
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                model.GroupID = CommonConstants.MEMBER_GROUP;

                var dao = new UserDao();
                var result = dao.Login(model);

                if (result.Status)
                {
                    var user = new UserDao().GetByUsername(model.Username);
                    var listCredentials = dao.GetListCredential(user.Username);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, user);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View();
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

                Session.Add(CommonConstants.USER_SESSION, user);
            }
            return Redirect("/");
        }
	}
}