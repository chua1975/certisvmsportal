using CertisVMS.Bll.Factory;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using System;
using System.Web.Mvc;
using CertisVMS.Bll.QueryCache;
using Newtonsoft.Json;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Model.Search;
using CertisVMS.Bll.ViewModels;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class DisplayMessageController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(StaffTypeController));
        public const string tableName = "TblDisplayMessage";


        // GET: DisplayMessage
        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetJsonData(int draw, int start, int length)
        {

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchDisplayMessageViewModel>>(VMSApiClient.Api_Config + "/api/displayMessage/search", reqJson, out error);
            var dt = DataTableData<SearchDisplayMessageViewModel>.CreateFor(
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


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(CertisVMS.Bll.ViewModels.DisplayMessageViewModel model)
        {
 
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/displayMessage/create", reqJson, out error);
            if (isSuccess)
            {
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }


        [ActionFilterController(TableName = tableName)]
        public ActionResult Edit(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<DisplayMessageViewModel>(
                VMSApiClient.Api_Config + $"/api/displayMessage/edit/{id}", out error);
            return View(ret);
        }

        [HttpPost]
        public ActionResult Edit(CertisVMS.Bll.ViewModels.DisplayMessageViewModel model)
        {
 
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/displayMessage/edit", reqJson, out error);
            if (isSuccess)
            {
                var data = CacheFetch.RefreshDisplayMessage();
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
            var ret = VMSApiClient.TryGetOne<StaffTypeViewModel>(
                VMSApiClient.Api_Config + $"/api/displayMessage/detail/{id}", out error);
            return View(ret);
        }



        public ActionResult Delete(long id)
        {
            string error;
            var isSuccess = VMSApiClient.Delete<bool>(VMSApiClient.Api_Config + $"/api/displayMessage/delete/{id}", out error);
            if (isSuccess)
            {
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
            }
            else
            {
                this.ShowMessage(false);
            }
            return RedirectToAction("Index");
        }
    }
}