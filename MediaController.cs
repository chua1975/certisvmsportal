using CertisVMS.Bll.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CertisVMSPortal.Controllers
{
    public class MediaController : Controller
    {
        ILog _log = LogManager.GetLogger(typeof(CaseController));

        // GET: Media
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _liveView(string url)
        {
            var model = new MediaViewModel()
            {
                URL = url
            };

            return PartialView(model);
        }

      /*  public ActionResult _liveView(MediaViewModel model)
        { 
           
            return PartialView("_liveView",model);
        }
        */
    }
}