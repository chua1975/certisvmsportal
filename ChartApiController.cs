using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMSApi.Interface.Models;
using CertisVMSApi.Interface.Models.Chart;
using Newtonsoft.Json;

namespace CertisVMSPortal.Controllers.Api
{
    public class ChartApiController : Controller
    {
        [HttpGet]
        public ActionResult GetPeopleCountOfMonth(int year, int month)
        {
            string error;
            var json = VMSApiClient.TryGetOne<ChartReportData>(VMSApiClient.Api_Report + $"/api/chart/getpeoplecountofmonth/{year}/{month}", out error);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPeakTimeOfMonth(int year, int month)
        {
            string error;
            var json = VMSApiClient.TryGetOne<ChartReportData>(VMSApiClient.Api_Report + $"/api/chart/getpeaktimeofmonth/{year}/{month}", out error);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetProfileTypesCountByDay(string date)
        {
            string error;
            RequestGeneral req = new RequestGeneral { searchDateString = date };
            var reqJson = JsonConvert.SerializeObject(req);
            var json = VMSApiClient.Search<ChartReportDailyData>(VMSApiClient.Api_Report + $"/api/chart/getprofiletypescountbyday", reqJson, out error);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetMonthlyTenants(int year)
        {
            string error;
            var json = VMSApiClient.TryGetOne<ChartReportMonthlyTenants>(VMSApiClient.Api_Report + $"/api/chart/getmonthlytenants/{year}", out error);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetHourlyCharts(string date)
        {
            string error;
            RequestGeneral req = new RequestGeneral { searchDateString = date };
            var reqJson = JsonConvert.SerializeObject(req);
            var json = VMSApiClient.Search<ChartReportCommon>(VMSApiClient.Api_Report + $"/api/chart/gethourlycharts", reqJson, out error);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetTenantsByMonth(string date)
        {
            string error;
            RequestGeneral req = new RequestGeneral{ searchDateString = date };
            var reqJson = JsonConvert.SerializeObject(req);
            var json = VMSApiClient.Search<ChartReportCommon>(VMSApiClient.Api_Report + $"/api/chart/gettenantsbymonth", reqJson, out error);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetWeeklyCharts(string date)
        {
            string error;
            RequestGeneral req = new RequestGeneral { searchDateString = date };
            var reqJson = JsonConvert.SerializeObject(req);
            var json = VMSApiClient.Search<ChartReportCommon>(VMSApiClient.Api_Report + $"/api/chart/getweeklycharts", reqJson, out error);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}