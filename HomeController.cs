using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMS.Bll.ViewModels;
using CertisVMS.Model.Constant;
using CertisVMS.Model.Search;
using CertisVMSPortal.Filters;
using log4net;

namespace CertisVMSPortal.Controllers
{
    //[Authorize]
    [CheckAuthorize]
    public class HomeController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(HomeController));

        [ActionFilterController(TableName = "TblStaff", Description = "Dashboard page")]
        public ActionResult Index()
        {
           
            this.ComputePermission();
            string error;
            var ret = VMSApiClient.TryGetOne<HomeViewModel>(
                VMSApiClient.Api_Report + $"/api/home/getdata", out error);

            var enabledMap = this.GetUserFunctions().FirstOrDefault(x => x.FunctionName == ConstFunctions.IndoorMap.ToString());
            if (enabledMap != null)
            {
                ret.EnabledMap = true;
            }

            return View(ret);
        }

        [ActionFilterController(TableName = "TblPortalNotifyLog", ActionType = "Update", Description = "Acknowlege Notification")]
        public ActionResult AcknowlegeNotification(long id)
        {
       
            string error;
            var isSuccess = VMSApiClient.Post<bool>(
                VMSApiClient.Api_Notification + $"/api/notification/ack/{id}", "", out error);
            if (isSuccess)
            {
                this.ShowMessage(true);
            }
            else
            {
                this.ShowMessage(false, error);
            }
            return RedirectToAction("Index");
        }


        public ActionResult OnPremVistors()
        {
           
            string error;
            var ret = VMSApiClient.TryGetList<SearchVisitationViewModel>(
                VMSApiClient.Api_Report + $"/api/home/OnPremVistors", out error);
            return View("List", ret);
        }

        public ActionResult OnPremStaffs()
        {
            
            string error;
            var ret = VMSApiClient.TryGetList<SearchVisitationViewModel>(
                VMSApiClient.Api_Report + $"/api/home/OnPremStaffs", out error);
            return View("List", ret);
        }

        public ActionResult OverstayedVisitors()
        {
            string error;
            var ret = VMSApiClient.TryGetList<SearchVisitationViewModel>(
                VMSApiClient.Api_Report + $"/api/home/OverstayedVisitors", out error);
            return View("List", ret);
        }

        public ActionResult OverstayedStaffs()
        {
            string error;
            var ret = VMSApiClient.TryGetList<SearchVisitationViewModel>(
                VMSApiClient.Api_Report + $"/api/home/OverstayedStaffs", out error);
            return View("List", ret);
        }

    }
}