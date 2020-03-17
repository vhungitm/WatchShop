using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WatchShop.Controllers
{
    public class BaseController : Controller
    {
        protected void SetAlert(string message, AlertType type)
        {
            TempData["AlertMessage"] = message;
            if (type == AlertType.Success)
            {
                TempData["AlertType"] = "alert-success";
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