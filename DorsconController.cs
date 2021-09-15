using CertisVMS.Bll.Factory;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Model.Search;
using CertisVMS.Model.Constant;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMS.Model.DbModels;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.ApiClient.VMS.Api;
using CertisVMS.Bll.ViewModels;

namespace CertisVMSPortal.Controllers
{
    public class DorsconController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(DorsconController));
        public const string tableName = "TblDorscon";

        // GET: Dorscon
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new DorsconViewModel();
            return View();
        }

        public ActionResult GetJsonData(int draw, int start, int length)
        {
            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;

            var data = VMSApiClient.Search<DataTableResponse<SearchDorsconViewModel>>(VMSApiClient.Api_Config + "/api/Dorscon/search", reqJson, out error);
            var dt = DataTableData<SearchDorsconViewModel>.CreateFor(
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

        [HttpPost]
        public ActionResult Create(CertisVMS.Bll.ViewModels.DorsconViewModel model)
        {
            try
            {
                var reqJson = JsonConvert.SerializeObject(model);
                string error;
                var ret = VMSApiClient.Post<int>(VMSApiClient.Api_Config + $"/api/Dorscon/create", reqJson, out error);
                if (ret > 0)
                {
                    var refData = CacheFetch.RefreshDorscon();
                    this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                    return RedirectToAction("Edit", new { id = ret });
                }
                else
                {
                    this.ShowMessage(false, error);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Exit Dorscon", ex);
                return View("Error");
            }

        }

        public ActionResult Edit(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<DorsconViewModel>(
                VMSApiClient.Api_Config + $"/api/Dorscon/edit/{id}", out error);
            return View(ret);
        }

        [HttpPost]
        public ActionResult Edit(CertisVMS.Bll.ViewModels.DorsconViewModel model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var ret = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/Dorscon/edit", reqJson, out error);
            if (ret)
            {
                var refData = CacheFetch.RefreshDorscon();
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                return RedirectToAction("Edit", new { id = model.DorsconID });
            }
            else
            {
                this.ShowMessage(false, error);
                return View(model);
            }
        }

        public ActionResult Details(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<DorsconViewModel>(
                VMSApiClient.Api_Config + $"/api/Dorscon/detail/{id}", out error);

            return View(ret);
        }

    }
}