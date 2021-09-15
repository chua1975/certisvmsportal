using System;
using CertisVMSPortal.Filters;
using log4net;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using System.Configuration;
using CertisVMS.Bll.ViewModels;
using CertisVMS.Model.Search;
using System.Collections.Generic;

namespace CertisVMSPortal.Controllers
{
    [CheckAuthorize]
    public class DashboardNotchController : Controller
    {
        [ActionFilterController(TableName = "AttendanceTotal_Notch")]
        // GET: Notch
        public ActionResult Index()
        {
            string api_dashboard = ConfigurationManager.AppSettings["api_Dashboard"];
            ViewBag.api_dashboard = api_dashboard;

            string error;
            var ret = VMSApiClient.TryGetOne<HomeViewModel>(
                VMSApiClient.Api_Report + $"/api/home/getdata", out error);

            IList<SearchCategoryCountViewModel> Lists = new List<SearchCategoryCountViewModel>();

            var sCategoryList = ret.AccessData.StaffTypes;
            //var vCategoryList = ret.AccessData.VisitorTypes;
            IList<SearchCategoryCountViewModel> vCategoryList = new List<SearchCategoryCountViewModel>();
            vCategoryList = ret.AccessData.VisitorTypes;
            foreach (var s in sCategoryList)
            {
                foreach (var v in vCategoryList)
                {
                    if (s.Category.Equals(v.Category))
                    {
                        //if Staff Category = Visitor Category; 
                        //Staff TypeCount = Satff Type Count + Visitor TypeCount;
                        //delet Visitor Category
                        s.TypeCount = s.TypeCount + v.TypeCount;
                    }
                }
                Lists.Add(s);
            }

            foreach (var v in vCategoryList)
            {
                bool flag = true;
                foreach (var s in sCategoryList)
                {
                    if (s.Category.Equals(v.Category))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    Lists.Add(v);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                var CategoryName = "CategoryName" + i;
                ViewData[CategoryName] = "";
            }
            for (int i = 0; i < Lists.Count; i++)
            {
                var CategoryName = "CategoryName" + i;
                ViewData[CategoryName] = Lists[i].Category;
            }

            ret.AccessData.VisitorTypes = Lists;

            return View(ret);
        }
    }
}