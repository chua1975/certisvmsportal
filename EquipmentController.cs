using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertisVMS.Model.DbModels;
using CertisVMSPortal.Filters;

namespace CertisVMSPortal.Controllers
{
	//TODO Phase2
    [CheckAuthorize]
    public class EquipmentController : Controller
	{
		[HttpPost, Route("equipment")]
		public ActionResult AddOrUpdate(TblEquipment obj)
		{
			throw new NotImplementedException();
		}
	}
}