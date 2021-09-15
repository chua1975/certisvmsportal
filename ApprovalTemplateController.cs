using CertisVMS.Model.DbModels;
using log4net;
using System.Web.Mvc;
using CertisVMS.Bll.ViewModels;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using CertisVMS.Model.Constant.ErrorMessageKey;
using ResponseHelper = CertisVMSApi.Interface.Models.ResponseHelper;
using CertisVMS.Bll.QueryCache;
using CertisVMS.ApiClient.DataTable;
using Newtonsoft.Json;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.VMS.Api;
using System.Collections.Generic;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class ApprovalTemplateController : Controller
    {
        public const string tableName = "TblApprovalTemplate";
        ILog _log = LogManager.GetLogger(typeof(ApprovalTemplateController));
        //private ApprovalTemplateService approvalTemplateBo = new ApprovalTemplateService();


        // GET: ApprovalTemplate
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
            var data = VMSApiClient.Search<DataTableResponse<ApprovalTemplateViewModel>>(VMSApiClient.Api_Enrollment + "/api/approvalTemplate/search", reqJson, out error);
            var dt = DataTableData<ApprovalTemplateViewModel>.CreateFor(
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


        // GET: ApprovalTemplate/Details/5
        [ActionFilterController(TableName = tableName)]
        public ActionResult Details(long id)
        {
 

            string error;
            var data = VMSApiClient.TryGetOne<ApprovalTemplateViewModel>(VMSApiClient.Api_Enrollment + $"/api/approvalTemplate/{id}", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
                return Redirect("Error");
            }
            return View(data);
        }


        // GET: ApprovalTemplate/Create
        public ActionResult Create()
        {
            this.ComputePermission();
            var model = new ApprovalTemplateViewModel();
            return View(model);
        }


        // POST: ApprovalTemplate/Create
        [HttpPost]
        public ActionResult Create(RequestApprovalTemplate approvelTemplate)
        {
 
            string error;
            var reqJson = JsonConvert.SerializeObject(approvelTemplate);
            var data = VMSApiClient.Post<int>(VMSApiClient.Api_Enrollment + "/api/approvalTemplate/create", reqJson, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
                this.ShowMessage(false, error);
                return Json(new
                {
                    code = ResponseHelper.FAIL,
                    approvalTemplateId = 0,
                    message = error
                });
            }
            else
            {
                this.ShowMessage(true);
                return Json(new
                {
                    code = ResponseHelper.SUCCESS,
                    approvalTemplateId = data,
                    message = ""
                });
            }
        }

        // GET: ApprovalTemplate/Edit/5
        [ActionFilterController(TableName = tableName)]
        public ActionResult Edit(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<ApprovalTemplateViewModel>(
                VMSApiClient.Api_Enrollment + $"/api/approvalTemplate/Edit/{id}", out error);
            return View(ret);
        }

        // POST: ApprovalTemplate/Edit/5
        [HttpPost]
        public ActionResult Edit(RequestApprovalTemplate approvelTemplate)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(approvelTemplate);
            var ret = VMSApiClient.Post<bool>(VMSApiClient.Api_Enrollment + "/api/approvalTemplate/Edit", reqJson, out error);
            if (!ret)
            {
                _log.Error(error);
                this.ShowMessage(false, error);
                return Json(new
                {
                    code = ResponseHelper.FAIL,
                    message = CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed)
                });
            }
            else
            {
                this.ShowMessage(true);
                return Json(new
                {
                    code = ResponseHelper.SUCCESS,
                    message = CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess)
                });
            }
        }


    }
}
