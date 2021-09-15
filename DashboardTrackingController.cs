using System;
using CertisVMSPortal.Filters;
using log4net;
using System.Web.Mvc;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class DashboardTrackingController : Controller
    {
        [ActionFilterController(TableName = "Dashboard_Tracking")]
        // GET: DashboardTracking
        public ActionResult Index()
        {
            return View();
        }
    }
}