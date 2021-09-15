using System;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Bll.Config;
using CertisVMS.Bll.ViewModels;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.Config;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.Enum;
using CertisVMSPortal.Filters;
using log4net;
using CertisVMSPortal.Models;
using Newtonsoft.Json;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class AccessController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(AccessController));
        public const string tableName = "TblRegisteredVisitorCheck";

        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            string error;
            var model = VMSApiClient.TryGetOne<AccessViewModel>(VMSApiClient.Api_Report + $"/api/summary/count",
                out error);

            //model.ApiRegFinger = VMSConfig.Get<string>(VMSKey.FingerPrintApi_Reg);
            //model.ApiCompareFinger = VMSConfig.Get<string>(VMSKey.FingerPrintApi_Compare);
            //model.NeedChangePass = VMSConfig.Get<bool>(VMSKey.AccessControl_ChangePass);
            var modeStr = VMSConfig.Get<string>(VMSKey.AccessControl_Mode);

            AcMode mode;
            if (!Enum.TryParse(modeStr, true, out mode))
            {
                return Content("Failed to parse mode. check configuration");
            }

            if (mode == AcMode.AccessControl)
            {
                return View("Scan", model);
            }

            if (mode == AcMode.CheckIn)
            {
                return View(model);
            }

            return Content("Failed to parse mode. check configuration");
        }

        public ActionResult GetJsonData(int draw, int start, int length)
        {
            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<VisibleCheck>>(VMSApiClient.Api_Report + "/api/check/search", reqJson, out error);
            var dt = DataTableData<VisibleCheck>.CreateFor(
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