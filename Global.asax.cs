using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CertisVMS.Bll;
using CertisVMS.Bll.AccessControl;
using CertisVMSPortal.Helpers;

namespace CertisVMSPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //init in other services
            //CertisVMS.Bll.UserAudit.UserAuditHelper.InitDbViewModelMaps();
            //CertisVMS.Bll.UserAudit.BaseViewHelper.SetViewModuleKeyMaps();

            //Temporary
            if (System.IO.File.Exists(@"C:\Projects\Certis-VMS\1_FrontEnd\Certis.VMS.Portal\bin\CertisVMS.ConfigCache.dll"))
            {
                // If file found, delete it    
                System.IO.File.Delete(@"C:\Projects\Certis-VMS\1_FrontEnd\Certis.VMS.Portal\bin\CertisVMS.ConfigCache.dll");
            }

            log4net.Config.XmlConfigurator.Configure();
            MvcHandler.DisableMvcResponseHeader = true;
            //FacePP_SyncAccessData.Start();
            // OverstayBatchJob.Run(10*1000,20);

        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
                app.Context.Response.Headers.Remove("X-AspNetMvc-Version");
            }
        }

    }
}
