using CertisVMS.Model.DbModels;
using CertisVMSPortal.Models;
using log4net;
using Newtonsoft.Json;
using System.Web.Mvc;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMSPortal.Filters;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Bll.PositionMap;
using CertisVMS.Model.Search;

namespace CertisVMSPortal.Controllers
{

    public class PositionController : Controller
	{
		ILog _log = LogManager.GetLogger(typeof(PositionController));
        public const string tableName = "TblPosition";

        // GET: Position
        [CheckAuthorize]
        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            this.ComputePermission();
            return View();
		}

        [CheckAuthorize]
        public ActionResult AddGetPositionById(string id)
		{
 
            string error;
            var ret = VMSApiClient.TryGetOne<PositionParentVm>(
                VMSApiClient.Api_Config + $"/api/position/add/{id}", out error);
            return Json(ret);
        }


        [CheckAuthorize]
        public ActionResult EditGetPositionById(string Id)
		{

            string error;
            var ret = VMSApiClient.TryGetOne<PositionParentVm>(
                VMSApiClient.Api_Config + $"/api/position/edit/{Id}", out error);
            return Json(ret);
        }

        [CheckAuthorize]
        public bool SavePosition(string modelStr)
		{

            var reqJson = modelStr;
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/position/save", reqJson, out error);
            return isSuccess;
        }


        [CheckAuthorize]
        [CheckLoginStatus]
        public string DeletePositionById(int id)
		{

            string error;
            var isSuccess = VMSApiClient.Delete<bool>(VMSApiClient.Api_Config + $"/api/position/delete/{id}", out error);
            return error;
        }


		public ActionResult GetDeviceData(int draw, int start, int length)
		{

            var req = DataTableReqFactory.Create(this, draw, start, length);
            req.sortField = "DeviceSN";
            req.sortDirection = "asc";
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchDeviceViewModel>>(VMSApiClient.Api_Config + "/api/positionDevice/search", reqJson, out error);
            var dt = DataTableData<SearchDeviceViewModel>.CreateFor(
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


        [CheckAuthorize]
        [CheckLoginStatus]
        public string SaveDevice(string deviceIdList, int id)
		{

            var reqJson = JsonConvert.SerializeObject(new {
                deviceIdList = deviceIdList,
                id = id
            });
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/positionDevice/save", reqJson, out error);
            if (isSuccess)
            {
                var data = new
                {
                    IsSuccess = true,
                    Message = CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess)
                };
                return JsonConvert.SerializeObject(data);
            }
            else
            {
                var data = new
                {
                    IsSuccess = false,
                    Message = CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed)
                };
                return JsonConvert.SerializeObject(data);
            }
        }

        [CheckLoginStatus]
        public string GetPositionAuthorityData(string checkNodesString)
		{
            var reqJson = JsonConvert.SerializeObject(new {
                checkNodesString = checkNodesString
            });
            string error;
            var ret = VMSApiClient.Search<string>(VMSApiClient.Api_Config + $"/api/position/GetPositionAuthorityData", reqJson,
                out error);
            return ret;
        }


        [CheckAuthorize]
        [CheckLoginStatus]
        public string GetFloorplans(int? id)
		{

            string error;
            var data = VMSApiClient.TryGetList<TblFloorPlan>(VMSApiClient.Api_Config + $"/api/floorPlan/{id}", out error);
            return JsonConvert.SerializeObject(data); 
        }


        [CheckAuthorize]
        [CheckLoginStatus]
        public ActionResult GetEquipmentData(int draw, int start, int length)
		{

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<TblEquipment>>(VMSApiClient.Api_Config + "/api/equipment/search", reqJson, out error);
            var dt = DataTableData<TblEquipment>.CreateFor(
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

        [CheckAuthorize]
        [HttpPost]
        public ActionResult AddOrUpdateEquipment(string modelStr)
        {
            string error;
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/equipment/create", modelStr, out error);
            return Json(isSuccess);
        }

        
        [CheckAuthorize]
        [HttpPost]
        public ActionResult DeleteEquipment(long id)
        {
            string error;
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/equipment/delete/{id}", "", out error);
            return Json(isSuccess);
        }

        [CheckAuthorize]
        [HttpPost]
		public ActionResult UpdateEquipmentPosition(string setToCur, string unsetFromCur, int positionId)
		{
            var reqJson = JsonConvert.SerializeObject(new {
                setToCur = setToCur,
                unsetFromCur = unsetFromCur,
                positionId = positionId
            });
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/equipmentPosition/edit", reqJson, out error);
            return Json(isSuccess);
        }


        [CheckAuthorize]
        [CheckLoginStatus]
        [HttpPost]
        public ActionResult UpdateSchindlerId(string schindlerId, int positionId)
        {
            var reqJson = JsonConvert.SerializeObject(new
            {
                schindlerId = schindlerId,
                positionId = positionId
            });
            string error;
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/schindlerId/save", reqJson, out error);
            return Json(isSuccess);
        }
    }
}