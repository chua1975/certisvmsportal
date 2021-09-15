using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMSPortal.Filters;
using Newtonsoft.Json;

namespace CertisVMSPortal.Controllers
{
    //TODO Phase2
    [CheckAuthorize]
    public class FloorPlanController : Controller
	{
        [HttpPost]
		public ActionResult AddOrUpdate(string modelStr)
		{
            var reqJson = modelStr;
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/floorPlan/edit", reqJson, out error);
            return Json(isSuccess);
        }
	}
}