using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.ApiClient.VMS;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.Search;
using CertisVMS.Model.ViewModels;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertisVMSApi.Interface.Models;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class DeclarationController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(DeclarationController));
        public const string tableName = "TblDeclaration";

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
            var data = VMSApiClient.Search<DataTableResponse<SearchDeclarationViewModel>>(VMSApiClient.Api_Config + "/api/declaration/searchdeclaration", reqJson, out error);
            var dt = DataTableData<SearchDeclarationViewModel>.CreateFor(
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

        public ActionResult Create(long id)
        {
            string error;
            var data = VMSApiClient.TryGetOne<TblVisitor>(VMSApiClient.Api_Enrollment + $"/api/visitor/{id}", out error);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction("Index");
        }

        [ActionFilterController(TableName = tableName)]
        public ActionResult Details(long id)
        {
            string error;
            var data = VMSApiClient.TryGetOne<TblDeclaration>(VMSApiClient.Api_Config + $"/api/declaration/declaration/{id}", out error);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetDeclaration()
        {
            string error;
            var list = VMSApiClient.TryGetList<DeclarationModel>(VMSApiClient.Api_Config + "/api/declaration/getdeclaration", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GeDeclarationSubQues(long questionaireid)
        {
            string error;
            var list = VMSApiClient.TryGetList<DeclarationSubQuestionnaire>(VMSApiClient.Api_Config + "/api/declaration/" + questionaireid + "/getdeclarationsubques", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateDeclaration(long id, string dataStr)
        {
            string error;
            var ret = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + "/api/declaration/createdeclaration/" + id, dataStr, out error);

            if (ret)
            {
                return Json(new { status = true, msg = CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _log.Error(error);
                return Json(new { status = false, msg = CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed) }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult GetDeclarationDetails(long id)
        {
            string error;
            var list = VMSApiClient.TryGetList<CategoryModel>(VMSApiClient.Api_Config + "/api/declaration/declarationdetail/" + id, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [CheckAuthorize]
        public ActionResult Delete(long id)
        {

            string error;
            var data = VMSApiClient.Delete<bool>(VMSApiClient.Api_Config + $"/api/declaration/deleteDeclaration/{id}", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
                this.ShowMessage(false, error);
            }
            else
            {
                this.ShowMessage(true);
            }

            return RedirectToAction("Index");
        }

        [CheckAuthorize]
        public string BulkDelete(long[] idList)
        {
            var reqJson = JsonConvert.SerializeObject(new RequestDeleteDeclaration()
            {
                idList = idList
            });
            string error;
            //var reqJson = JsonConvert.SerializeObject(idList);
            var data = VMSApiClient.Post<GeneralApiResult>(VMSApiClient.Api_Config + $"/api/declaration/bulkDelete", reqJson, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
                this.ShowMessage(false, error);
                return error;
            }
            else
            {
                if (data.IsSuccess)
                {
                    this.ShowMessage(true, data.Message);
                }
                else
                {
                    this.ShowMessage(false, data.Message);
                }
                return data.Message;
            }
        }
    }
}