using System.Web.Mvc;
using CertisVMSPortal.Filters;
using log4net;
using CertisVMS.Bll.QueryCache;
using CertisVMSPortal.Models;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMS.ApiClient;
using CertisVMS.Bll.ViewModels;
using Newtonsoft.Json;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Model.Search;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class NotificationController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(NotificationController));
        public const string tableName = "TblNotificationConfig";

        // GET: Site
        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionFilterController(TableName = tableName)]
        public ActionResult Edit(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<NotificationViewModel>(
                VMSApiClient.Api_Notification + $"/api/notification/edit/{id}", out error);
            return View(ret);
        }
        
        [HttpPost]
        public ActionResult Edit(CertisVMS.Bll.ViewModels.NotificationViewModel model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Notification + $"/api/notification/edit", reqJson, out error);
            if (isSuccess)
            {
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                return RedirectToAction("Index");
            }
            else
            {
                this.ShowMessage(false, error);
                return View(model);
            }
        }


        [ActionFilterController(TableName = tableName)]
        public ActionResult Details(long? id)
        {
         
            string error;
            var ret = VMSApiClient.TryGetOne<NotificationViewModel>(
                VMSApiClient.Api_Notification + $"/api/notification/details/{id}", out error);
            return View(ret);
        }       



        public ActionResult GetJsonData(int draw, int start, int length)
        {
            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchNotificationViewModel>>(VMSApiClient.Api_Notification + "/api/notification/search", reqJson, out error);
            var dt = DataTableData<SearchNotificationViewModel>.CreateFor(
                this, draw, data);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            var result = new JsonResult()
            {
                Data = dt,
                MaxJsonLength = int.MaxValue, // !important
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return result;
        }
    }
}