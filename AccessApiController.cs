using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Windows.Media.Imaging;
using CertisVMS.ApiClient;
using log4net;
using CertisVMS.Bll;
using CertisVMS.Bll.AccessControl;
using CertisVMS.Bll.Config;
using CertisVMS.Model.DbModels;
using SourceAFIS.Simple;
using CertisVMS.Bll.Helpers;
using CertisVMS.Model.Config;
using CertisVMSApi.Interface.Models;
using Newtonsoft.Json;
using CertisVMS.ApiClient.VMS.Api;

namespace CertisVMSPortal.Controllers.Api
{
    /// <summary>
    /// These apis are being used by HHT and Portal (when access control mode = AccessControl. config in TblSystemParameter)
    /// </summary>
    public class AccessApiController : ApiController
    {
        [HttpPost]
        [Route("api/ac/login")]
        public IHttpActionResult Login(RequestLogin login)
        {
            //UserBo userBo = new UserBo();
            //TblUser user = userBo.GetUserById(login.UserName);
            string error;
            var reqJson = JsonConvert.SerializeObject(login);
            var resp = VMSApiClient.Post<ResponseLogin>(VMSApiClient.Api_AccessControl + "/api/ac/login", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
         
            return Ok(resp);
        }

        [HttpGet]
        [Route("api/ac/sites")]
        public IHttpActionResult GetSites()
        {
           
            string error;
            var sites = VMSApiClient.TryGetOne<ResponseGetSites>(VMSApiClient.Api_Config + "/api/ac/sites", out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(sites);
        }






        [HttpPost]
        [Route("api/ac/checkVisit")]
        public IHttpActionResult CheckVisit(RequestCheckVisit request)
        {
            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            var resp = VMSApiClient.Post<ResponseCheckVisit>(VMSApiClient.Api_AccessControl + "/api/ac/checkVisit", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }


        [HttpPost]
        [Route("api/ac/movement")]
        public IHttpActionResult SaveMovement(RequestSaveMovement request)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            var resp = VMSApiClient.Post<ResponseCheckVisit>(VMSApiClient.Api_AccessControl + "/api/ac/movement", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }

        [HttpPost]
        [Route("api/ac/regFinger")]
        public IHttpActionResult RegFinger(RequestRegFingerPrint request)
        {
            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            var resp = VMSApiClient.Post<ResponseFingerPrint>(VMSApiClient.Api_AccessControl + "/api/ac/regFinger", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }


        public class RequestCompareFinger
        {
            public string Left { get; set; }
            public string Right { get; set; }
            public string Captured { get; set; }
            public string IC { get; set; }
        }

        public class ResponseCompareFinger
        {
            public float Score { get; set; }
            public bool IsSuccess { get; set; }
            public string ServerError { get; set; }
        }
        [HttpPost]
        [Route("api/ac/CompareFinger")]
        public IHttpActionResult CompareFinger(RequestCompareFinger request)
        {

            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            var resp = VMSApiClient.Post<ResponseCompareFinger>(VMSApiClient.Api_AccessControl + "/api/ac/CompareFinger", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }


        [HttpPost]
        [Route("api/ac/CompareFace")]
        public IHttpActionResult CompareFace(RequestCompareBiometric request)
        {
            //return Ok(new ResponseCompareBiometric()
            //{
            //    IsSuccess = true,
            //    Score = 10,
            //    ServerError = ""
            //}
            //);

            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            var resp = VMSApiClient.Post<ResponseCompareBiometric>(VMSApiClient.Api_AccessControl + "/api/ac/CompareFace", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }

        [HttpPost]
        [Route("api/ac/CompareIris")]
        public IHttpActionResult CompareIris(RequestCompareIris request)
        {
            return Ok(new ResponseCompareBiometric()
            {
                IsSuccess = true,
                Score = 0,
                ServerError = ""
            }
           );
            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            var resp = VMSApiClient.Post<ResponseCompareBiometric>(VMSApiClient.Api_AccessControl + "/api/ac/CompareIris", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }


        private ILog _log = LogManager.GetLogger(typeof(AccessApiController));
        [HttpPost]
        [Route("api/ac/regPass")]
        public IHttpActionResult RegPass(RequestVisitorPass request)
        {
            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            var resp = VMSApiClient.Post<ResponseRegVisitorPass>(VMSApiClient.Api_AccessControl + "/api/ac/regPass", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }

        [HttpPost]
        [Route("api/ac/createorupdatemobilevisit")]
        public IHttpActionResult CreateVisitation(ReqCreateVisitation request)
        {
            //return Ok(true);
            //
            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            _log.Info($"CreateVisitation para >> {reqJson}");
            var resp = VMSApiClient.Post<ResponseApiGeneral>(VMSApiClient.Api_Enrollment + "/api/visitor/createmobilevisit", reqJson,
                out error);
            _log.Error(error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }


        [HttpPost]
        [Route("api/ac/checkValidVisit")]
        public IHttpActionResult checkValidVisit(RequestCheckVisit request)
        {
            string error;
            var reqJson = JsonConvert.SerializeObject(request);
            var resp = VMSApiClient.Post<ResponseCheckVisit>(VMSApiClient.Api_AccessControl + "/api/ac/checkValidVisit", reqJson,
                out error);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }
            return Ok(resp);
        }



    }
}
