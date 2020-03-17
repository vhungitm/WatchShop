using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Web.Routing;

namespace WatchShop
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (Common.UserLogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION];
            if (session == null)
            {
                return false;
            }

            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(session.Username);

            if (privilegeLevels.Contains(this.RoleID) || session.GroupID == CommonConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = (Common.UserLogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION];
            if (session != null)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
                };
            }
        }
        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            var credentials = (List<string>)HttpContext.Current.Session[Common.CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}