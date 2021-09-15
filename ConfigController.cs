using System;
using CertisVMS.Model.DbModels;
using CertisVMSPortal.Filters;
using log4net;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using CertisVMS.Bll.QueryCache;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class ConfigController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(ConfigController));

        public const string tableName = "TblCode";


        // GET: Config
        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllCodes()
        {
            string error;
            var data = VMSApiClient.TryGetList<TblCode>(VMSApiClient.Api_Config + $"/api/code/all", out error);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllCodeTypes()
        {
            string error;
            var data = VMSApiClient.TryGetList<string>(VMSApiClient.Api_Config + $"/api/codeType/all", out error);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DoCreate(TblCode model)
        {

            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/code/create", reqJson, out error);
            if (isSuccess)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        public ActionResult DeleteCode(TblCode model)
        {
            string error;
            var isSuccess = VMSApiClient.Delete<bool>(VMSApiClient.Api_Config + $"/api/code/delete/{model.CodeID}", out error);
            if (isSuccess)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
            //throw new NotSupportedException();
        }



        public ActionResult UpdateCode(TblCode model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/code/update", reqJson, out error);
            if (isSuccess)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }


        public ActionResult GetTblCodeByTypeAndValue(TblCode model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = !VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/code/checkCodeExist", reqJson, out error);
            return Json(isSuccess);
        }


        public ActionResult GetAllSystemParameters()
        {
            //var data = CacheFetch.SystemParameter().OrderBy(x => x.ParamKey).ToList();
            //return Json(data, JsonRequestBehavior.AllowGet);

            string error;
            var data = VMSApiClient.TryGetList<TblSystemParameter>(VMSApiClient.Api_Config + $"/api/setting/all", out error);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoSystemParameterCreate(TblSystemParameter model)
        {

            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/setting/create", reqJson, out error);
            if (isSuccess)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }


        public ActionResult UpdateSystemParameter(TblSystemParameter model)
        {

            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/setting/update", reqJson, out error);

            if (isSuccess)
            {
                //Update Cache if update successful
                var data = CacheFetch.RefreshSystemParameter();
            }

            return Json(isSuccess);
        }


        public ActionResult CheckKeyExists(TblSystemParameter model)
        {
            var reqJson = JsonConvert.SerializeObject(model);
            string error;
            var exist = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/setting/checkKeyExist", reqJson,
                out error);
            return Json(!exist);
        }


        public string GetFingerPrintApi_Reg()
        {

            string error;
            var isSuccess = VMSApiClient.TryGetOne<string>(VMSApiClient.Api_Config + $"/api/setting/fingerprintRegUrl",
                out error);
            return isSuccess;
        }

    }
}