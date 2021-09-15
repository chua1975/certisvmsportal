using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.ApiClient.VMS.Api;
using CertisVMS.Bll.ViewModels;
using CertisVMS.Model.Constant;
using CertisVMS.Model.Search;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using Newtonsoft.Json;
using System.Web.Mvc;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Model.Constant.ErrorMessageKey;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class DorsconStaffTypeController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(DorsconStaffTypeController));
        public const string tableName = "TblDorsconStaffType";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CertisVMS.Bll.ViewModels.DorsconStaffTypeViewModel model)
        {

            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var ret = VMSApiClient.Post<int>(VMSApiClient.Api_Config + $"/api/DorsconStaffType/create", reqJson, out error);
            if (ret > 0)
            {
                var refData = CacheFetch.RefreshDorsconStaffType();
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                return RedirectToAction("Edit", new { id = ret });
            }
            else
            {
                this.ShowMessage(false, error);
                return View(model);
            }
        }


        public ActionResult Edit(long? id)
        {

            string error;
            var ret = VMSApiClient.TryGetOne<DorsconStaffTypeViewModel>(
                VMSApiClient.Api_Config + $"/api/DorsconStaffType/edit/{id}", out error);
            return View(ret);
        }


        [HttpPost]
        public ActionResult Edit(CertisVMS.Bll.ViewModels.DorsconStaffTypeViewModel model)
        {


            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var ret = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/DorsconStaffType/edit", reqJson, out error);
            if (ret)
            {
                var refData = CacheFetch.RefreshDorsconStaffType();
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
            var ret = VMSApiClient.TryGetOne<DorsconStaffTypeViewModel>(
                VMSApiClient.Api_Config + $"/api/DorsconStaffType/detail/{id}", out error);
            return View(ret);
        }



        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetStaffType(long id)
        {

            string error;
            var ret = VMSApiClient.TryGetOne<ResponseGetStaffType>(VMSApiClient.Api_Config + $"/api/DorsconStaffType/detail/{id}",
                out error);

            var notification = VMSApiClient.TryGetOne<string>(VMSApiClient.Api_Notification + $"/api/notification/GetByMessageType/{ConstVisitation.VisitorOverstayed}",
              out error);
            ret.Notification = notification;

            return Json(ret, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetJsonData(int draw, int start, int length)
        {

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchDorsconStaffType>>(VMSApiClient.Api_Config + "/api/DorsconStaffType/search", reqJson, out error);
            var dt = DataTableData<SearchDorsconStaffType>.CreateFor(
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