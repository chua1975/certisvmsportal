using System.Web.Mvc;
using System;
using System.Collections.Generic;
using log4net;
using CertisVMSPortal.Filters;
using CertisVMS.Model.Search;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Model.Constant.ErrorMessageKey;
using Newtonsoft.Json;
using CertisVMS.ApiClient;

namespace CertisVMSPortal.Controllers
{

    [CheckAuthorize]
    public class PublicHolidayController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(PublicHolidayController));
        public const string tableName = "TblPublicHoliday";

        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreatePublicHoliday(SearchPublicHolidayViewModel model)
        {
 

            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/publicHoliday/create", reqJson, out error);
            if (isSuccess)
            {
                this.ShowMessage(true);
                return RedirectToAction("Index");
            }
            else
            {
                this.ShowMessage(false, error);
                return View(model);
            }
        }


        [HttpPost]
        public ActionResult UpdatePublicHoliday(SearchPublicHolidayViewModel model)
        {
 
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/publicHoliday/edit", reqJson, out error);
            if (isSuccess)
            {
                return Json(new
                {
                    IsSuccess = true,
                    message = CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess),
                });
            }
            else
            {
                return Json(new
                {
                    IsSuccess = false,
                    message = CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed),
                });
            }
        }


        [HttpPost]
        public ActionResult DeletePublicHoliday(SearchPublicHolidayViewModel model)
        {

            string error;
            var isSuccess = VMSApiClient.Delete<bool>(
                VMSApiClient.Api_Config+ $"/api/publicHoliday/delete/{model.Id}",  out error);
            if (isSuccess)
            {
                return Json(new
                {
                    IsSuccess = true,
                    message = CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess),
                });
            }
            else
            {
                return Json(new
                {
                    IsSuccess = false,
                    message = CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed),
                });
            }
        }


        [HttpGet]
        public ActionResult GetJsonData(DateTime start, DateTime end)
        {
 

            var reqJson = JsonConvert.SerializeObject(new {
                start = start,
                end = end
            });
            string error;
            var list = VMSApiClient.Post<List<Event>>(VMSApiClient.Api_Config + $"/api/publicHoliday/search", reqJson, out error);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}