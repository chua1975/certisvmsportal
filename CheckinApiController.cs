using System;
using System.Web.Http;
using log4net;
using CertisVMS.Bll;
using CertisVMS.Bll.AccessControl;
using CertisVMS.Model.DbModels;
using System.Web.Http.Results;
using CertisVMS.ApiClient;
using CertisVMS.Bll.Helpers;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Model;
using CertisVMS.Model.Constant.ErrorMessageKey;
using CertisVMSApi.Interface.Models;
using CertisVMSPortal.Filters;
using Newtonsoft.Json;

namespace CertisVMSPortal.Controllers.Api
{
    /// <summary>
    /// These Apis are for checkin page (when access control mode = CheckIn. config in TblSystemParameter)
    /// </summary>
    public class CheckinApiController : ApiController
    {
        private ILog _log = LogManager.GetLogger(typeof(AccessApiController));

        [HttpPost]
        [Route("api/checkin/validate")]
        public IHttpActionResult CheckInValidate(RequestCheckVisit request)
        {

            var json = JsonConvert.SerializeObject(request);
            string error;
            var ret = VMSApiClient.Post<ResponseCheckVisit>(VMSApiClient.Api_AccessControl + "/ac/checkin/validate", json, out error);

            return Ok(ret);
        }

        /// <summary>
        /// TODO confirm always visitor ?
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/checkin/updatePic")]
        public IHttpActionResult UpdateVistorPic(RequestVisitorPic request)
        {

            var json = JsonConvert.SerializeObject(request);
            string error;
            var ret = VMSApiClient.Post<ResponseCheckVisit>(VMSApiClient.Api_AccessControl + "/ac/checkin/updatePic", json, out error);

            return Ok(ret);
        }

        [HttpPost]
        [Route("api/checkin/validateQr")]
        public IHttpActionResult QrCodeCheck(RequestCodeCheck request)
        {

            var json = JsonConvert.SerializeObject(request);
            string error;
            var ret = VMSApiClient.Post<ResponseCheckVisitRecord>(VMSApiClient.Api_AccessControl + "/ac/checkin/validateQr", json, out error);

            return Ok(ret);
        }

        [HttpPost]
        [Route("api/checkin/saveState")]
        public IHttpActionResult SaveRegVisitorCheckedIn(RequestVisitorPic request)
        {
            var json = JsonConvert.SerializeObject(request);
            string error;
            var ret = VMSApiClient.Post<ResponseCheckVisit>(VMSApiClient.Api_AccessControl + "/ac/checkin/saveState", json, out error);

            return Ok(ret);
        }
        
        

    }
}
