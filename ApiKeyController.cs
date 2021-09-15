using CertisVMS.Bll.Factory;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.Constant;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using System;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Bll.ViewModels;
using CertisVMS.Model.Search;
using Newtonsoft.Json;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class ApiKeyController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(ApiKeyController));
        public const string tableName = "TblApiKey";

        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CertisVMS.Bll.ViewModels.ApiKeyViewModel model)
        {
            try
            {
                var reqJson = JsonConvert.SerializeObject(model);
                string error;
                var ret = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/apikey/create", reqJson, out error);
                if (ret)
                {
                    this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                }
                else
                {
                    this.ShowMessage(false, CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed));
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _log.Error("Exit Create ApiKey", ex);
                return View("Error");
            }
        }

        [ActionFilterController(TableName = tableName)]
        public ActionResult Edit(long? id)
        {
            string error;
            var apiKey = VMSApiClient.TryGetOne<ApiKeyViewModel>(VMSApiClient.Api_Config + $"/api/apikey/edit/{id}", out error);
            if (apiKey != null)
            {
                return View(apiKey);
            }
            else
            {
                this.ShowMessage(false, CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed));
            }

            return Redirect("Error");
        }

        [HttpPost]
        public ActionResult Edit(CertisVMS.Bll.ViewModels.ApiKeyViewModel model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var ret = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/apikey/edit", reqJson, out error);
            if (ret)
            {
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
            }
            else
            {
                this.ShowMessage(false, CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed));
            }
            return RedirectToAction("Index");
        }

        [ActionFilterController(TableName = tableName)]
        public ActionResult Details(long? id)
        {
            string error;
            var apiKey = VMSApiClient.TryGetOne<SearchApiKeyViewModel>(VMSApiClient.Api_Config + $"/api/apikey/detail/{id}", out error);
            return View(apiKey);

        }

        public ActionResult GetJsonData(int draw, int start, int length)
        {
            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchApiKeyViewModel>>(VMSApiClient.Api_Config + "/api/apikey/search", reqJson, out error);
            var dt = DataTableData<SearchApiKeyViewModel>.CreateFor(
                this, draw, data);


            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }

            return new JsonResult()
            {
                Data = dt,
                MaxJsonLength = int.MaxValue, // !important
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}