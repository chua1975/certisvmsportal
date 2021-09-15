using System;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMS.ApiClient.VMS.Api.Fr;
using CertisVMSPortal.Models.Mobile;
using log4net;
using Newtonsoft.Json;

namespace CertisVMSPortal.Controllers
{
    public class ReqFRCrop
    {
        public string base64 { get; set; }
    }
    public class RepFRCrop
    {
        public string base64 { get; set; }
    }
    public class PortalFRController : Controller
    {
        private ILog _log = LogManager.GetLogger(typeof(PortalFRController));

        public ActionResult CropFace(ReqFRCrop req)
        {
            return Json(new RepFRCrop()
            {
                base64 = req.base64
            });
            //_log.Debug($"fr crop face 1");
            //this.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //this.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            //this.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
            //this.Response.AddHeader("Access-Control-Max-Age", "1800");//30 min

            //var bytes = Convert.FromBase64String(req.base64);
            //var newImg = EmguHelper.ResizeImage(bytes, 0.5);
            //var newBase64 = Convert.ToBase64String(newImg);
            //_log.Debug($"fr crop face 2");
            //var face64 = FRGateway.CropFaceBase64(newBase64);
            //var parsedStr = ConstFR.ParseBase64Prefix(face64);
            //var resp = new RepFRCrop()
            //{
            //    base64 = parsedStr[1]
            //};

            //return Json(resp);
        }

        [System.Web.Http.HttpPut]
        public ActionResult UpdateVisitorPhoto(long id)
        {
 
            string error;
            var ret = VMSApiClient.Put<GeneralResponse>(
                VMSApiClient.Api_AccessControl + $"/api/fr/updateVisitorPhoto/{id}", "", out error);
            return Json(ret);
        }

        [System.Web.Http.HttpPost]
        public ActionResult OneToOneBase64(string base64_1, string base64_2)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(new RequestFrOneOne()
            {
                base64_1 = base64_1,
                base64_2 = base64_2
            });
            var ret = VMSApiClient.Post<double>(VMSApiClient.Api_AccessControl + $"/api/fr/oneToOne", reqJson, out error);
            return Json(new { score = ret });
        }


        [System.Web.Http.HttpPost]
        public ActionResult OneToOneDeepFace(RequestFrOneOne req)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(req);
            var responseObj = VMSApiClient.Post<ResponseCompareFace>(VMSApiClient.Api_AccessControl + $"/api/fr/oneToOneDeepFace", reqJson, out error);
            var responseJson = Json(responseObj);
            _log.Debug($"OneToOneDeepFace >> {responseJson}");           
            return responseJson;
        }

       

        [System.Web.Http.HttpPost]
        public ActionResult OneToOne()
        {
            //_log.Debug($"one to one 1");
            var files = Request.Files;
            if (files.Count == 2)
            {
                _log.Debug($"one to one 2");
                var file1 = files[0];
                byte[] image1 = new byte[file1.ContentLength];
                file1.InputStream.Read(image1, 0, image1.Length);
                var b64_1 = Convert.ToBase64String(image1);

                _log.Debug($"one to one 3");
                var file2 = files[1];
                byte[] image2 = new byte[file2.ContentLength];
                file2.InputStream.Read(image2, 0, image2.Length);
                var b64_2 = Convert.ToBase64String(image2);

                string error;
                var reqJson = JsonConvert.SerializeObject(new RequestFrOneOne()
                {
                    base64_1 = b64_1,
                    base64_2 = b64_2
                });
                var ret = VMSApiClient.Post<double>(VMSApiClient.Api_AccessControl + $"/api/fr/oneToOne", reqJson, out error);

                return Json(new {score = ret});
            }

            return Json(new { score = 0 });

        }


        public ActionResult CheckQuality(ReqFRCrop req)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(req);
            var ret = VMSApiClient.Post<bool>(VMSApiClient.Api_AccessControl + $"/api/fr/quality", reqJson,out error);
            return Json(ret);
        }


        public string CheckQualitySenseDLC(ReqFRCrop req)
        {
           /// return true.ToString();

            try
            {
                string error;
                var reqJson = JsonConvert.SerializeObject(req);
                var ret = VMSApiClient.Post<bool>(VMSApiClient.Api_AccessControl + $"/api/fr/quality", reqJson, out error);
                return ret.ToString(); ;
            }catch(Exception ex)
            {
                _log.Error(ex);
                return string.Empty;
            }
        }
    }
}