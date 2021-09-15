using System.Web.Mvc;
using CertisVMSPortal.Filters;
using log4net;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class MapController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(MapController));
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Indoor()
        {
            return View();
        }
    }
}