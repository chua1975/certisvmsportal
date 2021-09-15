//#define ALWAYS_TRUE


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using CertisLicenseLib;
using CertisVMS.ApiClient;
using CertisVMS.Bll.AccessControl;

using CertisVMS.Model;
using CertisVMS.Model.Config;
using CertisVMS.Model.DbModels;
using CertisVMS.WebsocketClient;
using CertisVMSApi.Interface.Models;
using CertisVMSApi.Interface.Models.FacePP;
using log4net;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace CertisVMSPortal.Controllers.Api
{
    public class FacePPApiController : ApiController
    {
        private ILog _log = LogManager.GetLogger(typeof(FacePPApiController));

 

        [HttpGet]
        [Route("api/facepp/sync")]
        public IHttpActionResult SyncDevice(string password)
        {
            try
            {
                if (password.ToLower() != "cta")
                {
                    return Ok("invalid password");
                }

                //FacePP_KolaClient.SyncDevices();
                //return Ok(new ResponseApiGeneral() { IsSuccess = true });
                string error;
                var ret = VMSApiClient.TryGetOne<ResponseApiGeneral>(VMSApiClient.Api_AccessControl + $"/api/facepp/sync/{password}", out error);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseApiGeneral() { ErrorMessage = ex.Message, IsSuccess = false });
            }
        }


    }
}