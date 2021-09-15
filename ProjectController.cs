using CertisVMS.Model.DbModels;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using System;
using System.Web.Mvc;
using CertisVMS.Bll.QueryCache;
using CertisVMS.ApiClient;
using Newtonsoft.Json;
using CertisVMS.Bll.ViewModels;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Model.Search;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class ProjectController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(ProjectController));
        public const string tableName = "TblProject";

        // GET: Project
        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {
            CertisVMS.Bll.ViewModels.ProjectViewModel model = new CertisVMS.Bll.ViewModels.ProjectViewModel();
            return View(model);
        }


        [HttpPost]
        public ActionResult Create(CertisVMS.Bll.ViewModels.ProjectViewModel model)
        {

            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/project/create", reqJson, out error);
            if (isSuccess)
            {
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                return RedirectToAction("Index");
            }
            else
            {
                this.ShowMessage(false, error);
                return View(model);
            }
        }

        //TODO why this id allows null ? change it 
        [ActionFilterController(TableName = tableName)]
        public ActionResult Edit(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<ProjectViewModel>(
                VMSApiClient.Api_Config + $"/api/project/edit/{id}", out error);
            return View(ret);
        }


        [HttpPost]
        public ActionResult Edit(CertisVMS.Bll.ViewModels.ProjectViewModel model)
        {

            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/project/edit", reqJson, out error);
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                this.ShowMessage(false, error);
                return View(model);
            }
        }
        
        public ActionResult GetJsonData(int draw, int start, int length)
        {
            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchProjectViewModel>>(VMSApiClient.Api_Config + "/api/project/search", reqJson, out error);
            var dt = DataTableData<SearchProjectViewModel>.CreateFor(
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