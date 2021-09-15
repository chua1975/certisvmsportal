using CertisVMS.Model.DbModels;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using System.Web.Mvc;
using Newtonsoft.Json;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class ApprovalController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(VisitorController));
        public const string tableName = "TblRegisteredVisitor";


        // GET: Approval
        [ActionFilterController(TableName = "ApprovalPendingList", Description = "Approval Pending List")]
        public ActionResult Index()
        {
            return View();
        }

        [ActionFilterController(TableName = "ApprovalRejectList", Description = "Approval Reject List")]
        public ActionResult RejectList()
        {
            return View();
        }

        public ActionResult GetJsonApprovalData(int draw, int start, int length)
        {

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;

            var data = VMSApiClient.Search<DataTableResponse<CertisVMS.Model.DbModels.VisibleApprovalVisitation>>(VMSApiClient.Api_Enrollment + "/api/visitation/approved/all", reqJson, out error);
            var dt = DataTableData<CertisVMS.Model.DbModels.VisibleApprovalVisitation>.CreateFor(
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


        public ActionResult GetJsonRejectedData(int draw, int start, int length)
        {

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<VisibleApprovalVisitation>>(VMSApiClient.Api_Enrollment + "/api/visitation/rejected/all", reqJson, out error);
            var dt = DataTableData<VisibleApprovalVisitation>.CreateFor(
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
        public ActionResult SubmitApprovalResult(long rid,
            //long CurrentApprovalLevel,
            string CurrentApprovalLevelName,
            //string ApprovalRole,
            //string ApprovalResult,
            string CurrentResult,
            string ApprovalRemarks)
        {
   
            string error;
            var reqJson = JsonConvert.SerializeObject(new
            {
                rid = rid,
                CurrentApprovalLevelName = CurrentApprovalLevelName,
                CurrentResult = CurrentResult,
                ApprovalRemarks = ApprovalRemarks
            });
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Enrollment + "/api/visitation/approval/submit", reqJson, out error);
            if (!isSuccess)
            {
                _log.Error(error);
                this.ShowMessage(false, error);
            }
            else
            {
                this.ShowMessage(true);
            }
            return Json(new { state = isSuccess }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult SubmitRevertPendingResult(long rid, string ApprovalRemarks)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(new
            {
                rid = rid,
                ApprovalRemarks = ApprovalRemarks
            });
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Enrollment + "/api/visitation/approval/SubmitRevertPendingResult", reqJson, out error);
            if (!isSuccess)
            {
                _log.Error(error);
                this.ShowMessage(false, error);
            }
            else
            {
                this.ShowMessage(true);
            }
            return Json(new { state = isSuccess }, JsonRequestBehavior.AllowGet);

        }
    }
}