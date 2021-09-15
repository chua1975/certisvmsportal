using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CertisVMS.ApiClient;
using CertisVMS.Model.DbModels;
using CertisVMS.Model;
using CertisVMS.Model.Enum;

using CertisVMSApi.Interface.Models;
using CertisVMSApi.Interface.Models.Map;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CertisVMSPortal.Controllers.Api
{
    [RoutePrefix("api/map")]
    public class MapApiController : ApiController
    {
        private ILog _log = LogManager.GetLogger(typeof(MapApiController));

        [HttpGet, Route("buildings")]
        public IEnumerable<IdName> GetBuildingList()
        {
            string error;
            var data = VMSApiClient.TryGetList<IdName>(VMSApiClient.Api_Config + "/api/map/buildings", out error);
            return data;
        }

        [HttpGet, Route("buildings/{buildingId}/floors")]
        public IEnumerable<IdName> GetFloorsByBuilding(int buildingId)
        {
            string error;
            var data = VMSApiClient.TryGetList<IdName>(VMSApiClient.Api_Config + $"/api/map/buildings/{buildingId}/floors", out error);
            return data;
        }

        [HttpGet, Route("floors/{floorId}")]
        public TblFloorPlan GetFloor(int floorId)
        {
            string error;
            var data = VMSApiClient.TryGetOne<TblFloorPlan>(VMSApiClient.Api_Config + $"/api/map/floors/{floorId}", out error);
            return data;
        }

        [HttpGet, Route("floors/{floorId}/cameras")]
        public IHttpActionResult GetFloorCameras(int floorId)
        {
            string error;
            var data = VMSApiClient.TryGetOne<JObject>(VMSApiClient.Api_Config + $"/api/map/floors/{floorId}/cameras", out error);
            return Json(data);
        }

        [HttpGet, Route("floors/{floorId}/tablets")]
        public IHttpActionResult GetFloorTablets(int floorId)
        {
            string error;
            var data = VMSApiClient.TryGetOne<JObject>(VMSApiClient.Api_Config + $"/api/map/floors/{floorId}/tablets", out error);
            return Json(data);
        }

        [HttpGet, Route("floors/{floorId}/doors")]
        public IHttpActionResult GetFloorDoors(int floorId)
        {
            string error;
            var data = VMSApiClient.TryGetOne<JObject>(VMSApiClient.Api_Config + $"/api/map/floors/{floorId}/doors", out error);
            return Json(data);
        }

        [HttpGet, Route("floors/{floorId}/gantries")]
        public IHttpActionResult GetFloorGantries(int floorId)
        {
            string error;
            var data = VMSApiClient.TryGetOne<JObject>(VMSApiClient.Api_Config + $"/api/map/floors/{floorId}/gantries", out error);
            return Json(data);
        }

        [HttpGet, Route("floors/{floorId}/lifts")]
        public IHttpActionResult GetFloorLifts(int floorId)
        {
            string error;
            var data = VMSApiClient.TryGetOne<JObject>(VMSApiClient.Api_Config + $"/api/map/floors/{floorId}/lifts", out error);
            return Json(data);
        }

        [HttpGet, Route("floors/{floorId}/intercoms")]
        public IHttpActionResult GetFloorintercoms(int floorId)
        {
            string error;
            var data = VMSApiClient.TryGetOne<JObject>(VMSApiClient.Api_Config + $"/api/map/floors/{floorId}/intercoms", out error);
            return Json(data);
        }


        [HttpGet, Route("floors/{floorId}/image")]
        public HttpResponseMessage GetFloorImage(int floorId)
        {
            string error;
            var data = VMSApiClient.GetRaw(VMSApiClient.Api_Config + $"/api/map/floors/{floorId}/image", out error);
            return data;

        }

        [HttpPost, Route("updateTagCoord")]
        public IHttpActionResult UpdateCoordinates(RequestUpdateCoord req)
        {
 
            var reqJson = JsonConvert.SerializeObject(req);
            string error;
            var ret = VMSApiClient.Post<bool>(VMSApiClient.Api_Config + $"/api/map/updateTagCoord",reqJson, out  error);
            if (!ret)
            {
                _log.Error($"failed to update coordinate .");
            }
            return Ok();
        }

        
    }
}
