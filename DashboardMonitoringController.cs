using System;
using CertisVMSPortal.Filters;
using log4net;
using System.Web.Mvc;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class DashboardMonitoringController : Controller
    {
        [ActionFilterController(TableName = "Dashboard_Monitoring")]
        // GET: DashboardMonitoring
        public ActionResult Index()
        {
            return View();
        }
    }
}