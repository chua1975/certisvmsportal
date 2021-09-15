using System.Web;
using System.Web.Mvc;
using CertisVMSPortal.Helpers;

namespace CertisVMSPortal.Filters
{
    public class CheckLoginStatusAttribute: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/UserAccount/Login");  
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {            
            if (httpContext.Session[SessionKeys.LoginUser] == null)
            {
                return false;
            }

	        return true;
        }

    }
}