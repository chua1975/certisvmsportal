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
using System.Web;


namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class EmailTemplateController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(EmailTemplateController));
        public const string tableName = "TblEmailTemplate";

        [ActionFilterController(TableName = tableName)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new EmailTemplateViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CertisVMS.Bll.ViewModels.EmailTemplateViewModel model)
        {
            try
            {
                var footerImage = GetBase64FromImage(model.FooterImage);
                if (!string.IsNullOrEmpty(footerImage))
                {
                    model.Footer = footerImage;
                }

                var logoImage = GetBase64FromImage(model.LogoImage);
                if (!string.IsNullOrEmpty(logoImage))
                {
                    model.Logo = logoImage;
                }
                model.FooterImage = null;
                model.LogoImage = null;
                var reqJson = JsonConvert.SerializeObject(model);
                string error;
                var isSuccess = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/EmailTemplate/create", reqJson, out error);
                if (isSuccess)
                {
                    this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
                    return RedirectToAction("Index");
                }
                else
                {
                    this.ShowMessage(false, error);
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Exit Create EmailTemplate", ex);
                return View("Error");
            }
        }           

        [ActionFilterController(TableName = tableName)]
        public ActionResult Edit(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<EmailTemplateViewModel>(
                VMSApiClient.Api_Config + $"/api/EmailTemplate/edit/{id}", out error);
            return View(ret);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CertisVMS.Bll.ViewModels.EmailTemplateViewModel model)
        {
            try
            {
                var footerImage = GetBase64FromImage(model.FooterImage);
                if (!string.IsNullOrEmpty(footerImage))
                {
                    model.Footer = footerImage;
                }

                var logoImage = GetBase64FromImage(model.LogoImage);
                if (!string.IsNullOrEmpty(logoImage))
                {
                    model.Logo = logoImage;
                }
                model.FooterImage = null;
                model.LogoImage = null;
                var reqJson = JsonConvert.SerializeObject(model);
                string error;
                var isSuccess = VMSApiClient.Put<bool>(VMSApiClient.Api_Config + $"/api/EmailTemplate/edit", reqJson, out error);
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
            catch (Exception ex)
            {
                _log.Error("Exit Edit EmailTemplate", ex);
                return View("Error");
            }
        }

        private string GetBase64FromImage(HttpPostedFileBase image)
        {
            if (image == null) { return string.Empty; }
            var fs = image.InputStream;
            var br = new System.IO.BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            return base64String;
        }

        [ActionFilterController(TableName = tableName)]
        public ActionResult Details(long? id)
        {
            string error;
            var ret = VMSApiClient.TryGetOne<EmailTemplateViewModel>(
                VMSApiClient.Api_Config + $"/api/EmailTemplate/detail/{id}", out error);
            return View(ret);
        }


        public ActionResult GetJsonData(int draw, int start, int length)
        {
            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var data = VMSApiClient.Search<DataTableResponse<SearchEmailTemplateViewModel>>(VMSApiClient.Api_Config + "/api/EmailTemplate/search", reqJson, out error);
            var dt = DataTableData<SearchEmailTemplateViewModel>.CreateFor(
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

        public ActionResult Delete(long id)
        {
            string error;
            var isSuccess = VMSApiClient.Delete<bool>(VMSApiClient.Api_Config + $"/api/EmailTemplate/delete/{id}", out error);
            if (isSuccess)
            {
                this.ShowMessage(true, CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess));
            }
            else
            {
                this.ShowMessage(false);
            }
            return RedirectToAction("Index");
        }
    }
}