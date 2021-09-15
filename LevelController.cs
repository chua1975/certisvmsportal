using CertisVMS.Bll.ViewModels;
using log4net;
using System.Collections.Generic;
using System.Web.Mvc;
using ResponseHelper = CertisVMSApi.Interface.Models.ResponseHelper;
using CertisVMSPortal.Filters;
using Newtonsoft.Json;
using CertisVMS.ApiClient;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class LevelController : Controller
    {
        private ILog _log = LogManager.GetLogger(typeof(LevelController));


        [HttpPost]
        public ActionResult Create(ApprovalLevelViewModel model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var ret = VMSApiClient.Post<List<string>>(VMSApiClient.Api_Enrollment + $"/api/approvalLevel/create", reqJson, out error);
            if (!string.IsNullOrEmpty(error))
            {
                return Json(error);
            }
            else
            {
                return Json(ResponseHelper.ReturnJson(ResponseHelper.SUCCESS, ret.ToArray()));
            }
        }
    }
}