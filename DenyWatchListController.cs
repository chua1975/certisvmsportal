using CertisVMS.Bll.Factory;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Model.Search;
using CertisVMS.Model.Constant;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMS.Model.DbModels;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.ApiClient.VMS.Api;
using CertisVMS.Bll.ViewModels;
using CertisVMS.ApiClient.VMS;

namespace CertisVMSPortal.Controllers
{
    public class DenyWatchListController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(DenyWatchListController));
        public const string tableName = "TblDenyWatchList";

        [ActionFilterController(TableName = tableName)]
        [CheckAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        //[CheckAuthorize]
        //Get Deny/Watch List -- Datatable
        public ActionResult GetJsonData(int draw, int start, int length)
        {
            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchDenyWatchListViewModel>>(VMSApiClient.Api_Enrollment + "/api/denywatchlist/search", reqJson, out error);
            var dt = DataTableData<SearchDenyWatchListViewModel>.CreateFor(
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


        //Add Deny/Watch List by ProfileID
        //heckAuthorize]
        public string AddDenyWatchList(DenyWatchListViewModel reqList)
        {
            string error;
            var reqJson = JsonConvert.SerializeObject(reqList);
            var data = VMSApiClient.Post<GeneralApiResult>(VMSApiClient.Api_Enrollment + $"/api/denywatchlist/create", reqJson, out error);
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


        //Delete Deny/Watch List by ProfileID
       //[CheckAuthorize]
        public string DeleteDenyWatchList(long id)
        {
            string error;
            var data = VMSApiClient.Delete<GeneralApiResult>(VMSApiClient.Api_Enrollment + $"/api/denywatchlist/delete/{id}", out error);
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