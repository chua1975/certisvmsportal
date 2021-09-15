using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.Search;
using CertisVMSApi.Interface.Models;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using Newtonsoft.Json;
using ResponseHelper = CertisVMSApi.Interface.Models.ResponseHelper;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class DeviceController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(DeviceController));
        public const string tableName = "TblFRDeviceGroup";

        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllData(int draw, int start, int length)
        {

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchFRDeviceModel>>(VMSApiClient.Api_Config + "/api/device/search", reqJson, out error);
            var dt = DataTableData<SearchFRDeviceModel>.CreateFor(
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


        [HttpPost]
        public ActionResult UpdateFRDeviceDirection(TblFRDeviceDirection model)
        {

            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/device/updateDirection", reqJson, out error);
            if (!isSuccess)
            {
                return Json(ResponseHelper.ReturnJson(ResponseHelper.SYSTEM_ERROR));
            }
            else
            {
                return Json(ResponseHelper.ReturnJson(ResponseHelper.SUCCESS));
            }
        }



        [HttpGet]
        public ActionResult SyncDevice(string password)
        {

            var reqJson = JsonConvert.SerializeObject(password);
            string error;
            var response = VMSApiClient.TryGetOne<ResponseApiGeneral>(VMSApiClient.Api_AccessControl + $"/api/senseNebula/sync/{password}", out error);
            if (!response.IsSuccess)
            {
                return Json(ResponseHelper.ReturnJson(ResponseHelper.SYSTEM_ERROR));
            }
            else
            {
                return Json(ResponseHelper.ReturnJson(ResponseHelper.SUCCESS));
            }
        }

        [ActionFilterController(TableName = tableName)]
        public ActionResult Features()
        {
            return View();
        }

        public ActionResult GetDeviceFeatures(int draw, int start, int length)
        {

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchFRDeviceModel>>(VMSApiClient.Api_Config + "/api/device/devicefeature/search", reqJson, out error);
            var dt = DataTableData<SearchFRDeviceModel>.CreateFor(
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

        [HttpPost]
        public ActionResult UpdateDeviceFeature(SearchFRDeviceModel model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/device/devicefeature/updateDeviceFeature", reqJson, out error);
            if (!isSuccess)
            {
                return Json(ResponseHelper.ReturnJson(ResponseHelper.SYSTEM_ERROR));
            }
            else
            {
                return Json(ResponseHelper.ReturnJson(ResponseHelper.SUCCESS));
            }

        }


        public ActionResult List()
        {
            return View();
        }


        public ActionResult GetAllData2(int draw, int start, int length)
        {

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchFRDeviceModel>>(VMSApiClient.Api_Config + "/api/device/search2", reqJson, out error);
            var dt = DataTableData<SearchFRDeviceModel>.CreateFor(
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