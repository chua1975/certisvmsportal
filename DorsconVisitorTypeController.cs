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
    public class DorsconVisitorTypeController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(DorsconVisitorTypeController));
        public const string tableName = "TblDorsconVisitorType";

        [CheckAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CertisVMS.Bll.ViewModels.DorsconVisitorTypeViewModel model)
        {
            try
            {

                var reqJson = JsonConvert.SerializeObject(model);
                string error;
                var ret = VMSApiClient.Post<int>(VMSApiClient.Api_Config + $"/api/DorsconVisitorType/create", reqJson, out error);
                if (ret > 0)
                {
                    var refData = CacheFetch.RefreshDorsconVisitorType();
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
                _log.Error("Exit Dorsonc VisitorType", ex);
                return View("Error");
            }

        }


        public ActionResult Edit(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<DorsconVisitorTypeViewModel>(
                VMSApiClient.Api_Config + $"/api/DorsconVisitorType/edit/{id}", out error);
            return View(ret);
        }

        [HttpPost]
        public ActionResult Edit(CertisVMS.Bll.ViewModels.DorsconVisitorTypeViewModel model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var ret = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/DorsconVisitorType/edit", reqJson, out error);
            if (ret)
            {
                var refData = CacheFetch.RefreshDorsconVisitorType();
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                return RedirectToAction("Edit", new { id = model.ID });
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
            var ret = VMSApiClient.TryGetOne<DorsconVisitorTypeViewModel>(
                VMSApiClient.Api_Config + $"/api/DorsconVisitorType/detail/{id}", out error);

            return View(ret);
        }

        public ActionResult GetJsonData(int draw, int start, int length)
        {
            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchDorsconVisitorTypeViewModel>>(VMSApiClient.Api_Config + "/api/DorsconVisitorType/search", reqJson, out error);
            var dt = DataTableData<SearchDorsconVisitorTypeViewModel>.CreateFor(
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

        [AllowAnonymous]
        public ActionResult GetDorsconVisitorType(long id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<ResponseGetVisitorType>(VMSApiClient.Api_Config + $"/api/DorsconVisitorType/detail/{id}",
                out error);

            var notification = VMSApiClient.TryGetOne<string>(VMSApiClient.Api_Notification + $"/api/notification/GetByMessageType/{ConstVisitation.VisitorOverstayed}",
              out error);
            ret.Notification = notification;

            return Json(ret, JsonRequestBehavior.AllowGet);


        }


        //If Any visitor Type enable BioCheck, show Fingerprint Enrollement for all.
        public bool IsBioCheckEnable()
        {
            string error;
            var ret = VMSApiClient.TryGetOne<bool>(VMSApiClient.Api_Config + $"/api/DorsconVisitorType/bioCheckEnabled",
                out error);
            return ret;
        }
    }
}