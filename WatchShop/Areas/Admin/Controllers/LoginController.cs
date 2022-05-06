using System.Web.Mvc;
using Model.Dao;
using Model;
using Common;
using Model.EF;

namespace WatchShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];

            if (user != null && user.GroupID == CommonConstants.ADMIN_GROUP)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(Login model)
        {
            if (ModelState.IsValid)
            {
                model.GroupID = CommonConstants.ADMIN_GROUP;

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
            return RedirectToAction("Index", "Home");
        }
    }
}