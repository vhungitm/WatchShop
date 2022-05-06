using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Common;
using Model.EF;

namespace WatchShop.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (User)Session[CommonConstants.USER_SESSION];
            if (session == null || session.GroupID != CommonConstants.ADMIN_GROUP)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }
        protected void SetAlert(string message, AlertType type)
        {
            TempData["AlertMessage"] = message;
            if (type == AlertType.Success)
            {
                TempData["AlertType"] = "alert-primary";
            }
            else if (type == AlertType.Warning)
            {
                TempData["AlertType"] = "alert-warrning";
            }
            else if (type == AlertType.Error)
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        protected enum AlertType
        {
            Success,
            Error,
            Warning,
        }
	}
}