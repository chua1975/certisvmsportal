using CertisVMS.ApiClient;
using CertisVMS.ApiClient.DataTable;
using CertisVMSPortal.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertisVMS.Model.DbModels;
using CertisVMSPortal.Filters;
using CertisVMSPortal.Helpers;
using CertisVMS.Bll.Helpers;
using CertisVMS.Model.Enum;
using CertisVMS.Bll.ViewModels;
using CertisVMS.Bll.Config;
using CertisVMS.Model.Config;

namespace CertisVMSPortal.Controllers
{
    public class CaseController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(CaseController));

        // GET: Case
        public ActionResult Index()
        {

            return View();

        }
        public ActionResult CaseListPopup()
        {

            return View();

        }
        

        public ActionResult RenderCaseliveview(CaseView Model)
        {
             
            string error;
            var data = VMSApiClient.TryGetList<TblCamera>(VMSApiClient.Api_Case + $"/api/master/equipmentcamerapair?eqtag="+Model.EquipmentTag, out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            List<string> list = new List<string>();
            var livestreamServer = VMSConfig.Get<string>(VMSKey.LIVEVIEW_WS);
            if (data != null)
            {
                foreach (var item in data)
                {
                    var url = livestreamServer + item.Id;
                    list.Add(url);
                }
            }            

            ViewBag.RTSurls = list;
            return PartialView("_Caseliveview");
        }

        public ActionResult DetailPopup(int? id)
        {

            if (!id.HasValue && TempData["id"] != null)
            {
                id = TempData["id"].ToString().ParseToNullableInt();

            }

            string error;
            var data = VMSApiClient.TryGetOne<CaseView>(VMSApiClient.Api_Case + $"/api/case/detail/{id}", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }


            var mediaData = VMSApiClient.TryGetList<TblMedia>(VMSApiClient.Api_Case + $"/api/case/{id}/media", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            IList<Tuple<long, string, string>> media = new List<Tuple<long, string, string>>();
            var mediaFolder = AppConfig.MediaFolder();
            foreach (var item in mediaData)
            {
                var url = VMSApiClient.api_media + $"/api/media/{item.Id}";
                media.Add(new Tuple<long, string, string>(item.Id, url, item.FileType));
            }

            //  media.Add(new Tuple<int, string,string>(5, "http://localhost:8503/api/media/capture/5", "image/jpeg"));
            //  media.Add(new Tuple<int, string,string>(5, "http://localhost:8503/api/media/capture/6", "video/mp4"));

            ViewBag.Medias = media;

            return View(data);


        }

        public ActionResult Details(int? id)
        {

            if (!id.HasValue && TempData["id"] != null)
            {
                id = TempData["id"].ToString().ParseToNullableInt();

            }

            string error;
            var data = VMSApiClient.TryGetOne<CaseView>(VMSApiClient.Api_Case + $"/api/case/detail/{id}", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }


            var mediaData = VMSApiClient.TryGetList<TblMedia>(VMSApiClient.Api_Case + $"/api/case/{id}/media", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
            }
            IList<Tuple<long, string, string>> media = new List<Tuple<long, string, string>>();
            var mediaFolder = AppConfig.MediaFolder();
            foreach (var item in mediaData)
            {
                var url = VMSApiClient.api_media + $"/api/media/{item.Id}";
                media.Add(new Tuple<long, string, string>(item.Id, url, item.FileType));
            }
          
          //  media.Add(new Tuple<int, string,string>(5, "http://localhost:8503/api/media/capture/5", "image/jpeg"));
          //  media.Add(new Tuple<int, string,string>(5, "http://localhost:8503/api/media/capture/6", "video/mp4"));

            ViewBag.Medias = media;

            return View(data);


        }


        public ActionResult GetCaseData(int draw, int start, int length)
        {

            var req = DataTableReqFactory.Create(this, draw, start, length);
            var reqJson = JsonConvert.SerializeObject(req);
            string error;

            var data = VMSApiClient.Search<DataTableResponse<CaseView>>(VMSApiClient.Api_Case + "/api/case/opencases", reqJson, out error);
            var dt = DataTableData<CaseView>.CreateFor(
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

        private bool UpdateCaseStatus(CaseView casedata, int status)
        {
            string error;
          /*  var casedata = VMSApiClient.TryGetOne<CaseView>(VMSApiClient.Api_Case + $"/api/case/detail/{model.Id}", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _log.Error(error);
                return false;
            }
            */
            casedata.StatusCode = status;
            var reqJson = JsonConvert.SerializeObject(casedata);
            var ret = VMSApiClient.Put<bool>(VMSApiClient.Api_Case + "/api/case/edit", reqJson, out error);
            if (!ret)
            {
                _log.Error(error);
                this.ShowMessage(false, error.GetErrorMessageFromJson());               
                return false;


            }
            else
            {
                TempData["id"] = casedata.Id;
                this.ShowMessage(true);
                return true;

            }

        }

        private bool UpdateCase(CaseView casedata)
        {
            /* string error;
             var casedata = VMSApiClient.TryGetOne<CaseView>(VMSApiClient.Api_Case + $"/api/case/detail/{model.Id}", out error);
             if (!string.IsNullOrWhiteSpace(error))
             {
                 _log.Error(error);
                 return false;
             }*/
            string error;
            string EventTypeId = Request.Form["EventType"].ToString();
            if (EventTypeId != null && Convert.ToString(casedata.EventTypeId) != EventTypeId)
                casedata.EventTypeId = Convert.ToInt32(EventTypeId);

            string Status = Request.Form["Status"].ToString();
            if (Status != null && Convert.ToString(casedata.StatusCode) != Status)
                casedata.StatusCode = Convert.ToInt32(Status);


            var reqJson = JsonConvert.SerializeObject(casedata);
            var ret = VMSApiClient.Put<bool>(VMSApiClient.Api_Case + "/api/case/edit", reqJson, out error);
            if (!ret)
            {
                _log.Error(error);
                this.ShowMessage(false, error.GetErrorMessageFromJson());
             
                return false;


            }
            else
            {
                TempData["id"] = casedata.Id;
                this.ShowMessage(true);
                return true;

            }


        }

        [CheckAuthorize]
        [HttpPost]
        public ActionResult Edit(string submitButton, CaseView model)
        {
            switch (submitButton)
            {
                case "Save":
                    if (UpdateCase(model))
                        return RedirectToAction("Details", new { id = model.Id });
                    return new EmptyResult();
                case "SaveandClose":
                    if (UpdateCase(model))
                        return RedirectToAction("Index");
                    return new EmptyResult();
                case "Reopen":
                    if (UpdateCaseStatus(model, (int)CaseStatus.Open))
                        return RedirectToAction("Details", new { id = model.Id });
                    return new EmptyResult();
                case "Resolve":
                    if (UpdateCaseStatus(model, (int)CaseStatus.Resolved))
                        return RedirectToAction("Details", new { id = model.Id });
                    return new EmptyResult();
                case "Close":
                    if (UpdateCaseStatus(model, (int)CaseStatus.Closed))
                        return RedirectToAction("Details", new { id = model.Id });
                    return new EmptyResult();


                default:
                    return new EmptyResult();
            }



        }

        [CheckAuthorize]
        [HttpPost]
        public ActionResult Edit1(string submitButton, CaseView model)
        {
            switch (submitButton)
            {
                case "Save":
                    if (UpdateCase(model))
                        return RedirectToAction("DetailPopup", new { id = model.Id });
                    return new EmptyResult();
                case "SaveandClose":
                    if (UpdateCase(model))
                        return RedirectToAction("DetailPopup", new { id = model.Id });
                    return new EmptyResult();
                case "Reopen":
                    if (UpdateCaseStatus(model, (int)CaseStatus.Open))
                        return RedirectToAction("DetailPopup", new { id = model.Id });
                    return new EmptyResult();
                case "Resolve":
                    if (UpdateCaseStatus(model, (int)CaseStatus.Resolved))
                        return RedirectToAction("DetailPopup", new { id = model.Id });
                    return new EmptyResult();
                case "Close":
                    if (UpdateCaseStatus(model, (int)CaseStatus.Closed))
                        return RedirectToAction("DetailPopup", new { id = model.Id });
                    return new EmptyResult();


                default:
                    return new EmptyResult();
            }



        }
    }
}