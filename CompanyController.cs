using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Model.Search;
using CertisVMS.Bll.ViewModels;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertisVMS.Bll.QueryCache;

namespace CertisVMSPortal.Controllers
{
    //[CheckAuthorize]
    public class CompanyController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(RoleController));
        public const string tableName = "TblCompany";

        // GET: Company
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
            var data = VMSApiClient.Search<DataTableResponse<SearchCompanyViewModel>>(VMSApiClient.Api_Config + "/api/company/search", reqJson, out error);
            var dt = DataTableData<SearchCompanyViewModel>.CreateFor(
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
        public ActionResult SearchCompany(string searchTerm)
        {
            var draw = 100;
            var start = 0;
            var length = 100;
            string error;
            var req = DataTableReqFactory.Create(this, draw, start, length);
            if (string.IsNullOrEmpty(req.search)) req.search = searchTerm;
            var reqJson = JsonConvert.SerializeObject(req);

            var data = VMSApiClient.Search<DataTableResponse<SearchCompanyViewModel>>(VMSApiClient.Api_Config + "/api/company/search", reqJson, out error);

            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }


            var listCompanyNames = data.Data.Select(x => new
            {
                id = x.CompanyName,
                name = x.CompanyName
            }).ToList();

            return Json(listCompanyNames.ToList(), JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult Create()
        {
            var model = new CompanyViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CompanyViewModel model)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(model);
            var IsSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + "/api/company/create", reqJson, out error);

            if (!IsSuccess)
            {
                this.ShowMessage(false, error);
                _log.Error(error);
                return RedirectToAction("Index");
            }
            else
            {

                this.ShowMessage(true);
                CacheFetch.AddOrUpdateLogoCache(string.Empty);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [ActionFilterController(TableName = tableName)]
        public ActionResult Edit(long id)
        {
            this.ComputePermission();
            string error;
            var data = VMSApiClient.TryGetOne<CompanyViewModel>(VMSApiClient.Api_Config + $"/api/company/edit/{id}", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }

            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(CompanyViewModel model)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(model);
            var ret = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + "/api/company/edit", reqJson, out error);

            if (!ret)
            {
                _log.Error(error);
                this.ShowMessage(false, error);
                return View(model);
            }
            else
            {
                this.ShowMessage(true);
                CacheFetch.AddOrUpdateLogoCache(string.Empty);
                return RedirectToAction("Index");
            }
        }

        [ActionFilterController(TableName = tableName)]
        public ActionResult Details(long id)
        {
            string error;
            var data = VMSApiClient.TryGetOne<CompanyViewModel>(VMSApiClient.Api_Config + $"/api/company/detail/{id}", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }

            return View(data);
        }

        public ActionResult CheckCompanyNameExist(CompanyViewModel com)
        {
            var reqJson = JsonConvert.SerializeObject(com);
            string error;
            var data = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/company/checkCompanyNameExist", reqJson, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult CreateCompanyIfNotExist(CompanyViewModel com)
        {
            if (string.IsNullOrWhiteSpace(com.CompanyName)) { return Json(false); }

            var reqJson = JsonConvert.SerializeObject(com);
            string error;
            var IsSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/company/createCompanyIfnotExist", reqJson, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            return Json(IsSuccess, JsonRequestBehavior.AllowGet);
        }
    }

}