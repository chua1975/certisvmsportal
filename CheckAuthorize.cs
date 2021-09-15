using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.Constant;
using CertisVMS.Model.DbModels;
using CertisVMSPortal.Helpers;

namespace CertisVMSPortal.Filters
{
    public class CheckAuthorize : AuthorizeAttribute
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
            
            if (httpContext.Session[SessionKeys.UserFunctions] == null)
            {
                return false;
            }


            ////check user access to controller
            var functions = httpContext.Session[SessionKeys.UserFunctions] as List<TblFunction>;
            string controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();


            TblUser loginUser = httpContext.Session[SessionKeys.LoginUser] as TblUser;
            if (loginUser != null && loginUser.SuperAdmin == ConstStatus.Yes ||
                functions != null && functions.Any(f => f.ControllerName?.ToUpper() == controller.ToUpper()))
            {
                return true;
            }

            return false;
        }

    }
}