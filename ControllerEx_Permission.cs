using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CertisVMS.Model.Constant;
using CertisVMSPortal.Helpers;

namespace CertisVMSPortal
{
	public partial class ControllerEx
	{
        /// <summary>
        /// After compute the result could be reused in page ViewBag.
        /// but in the end we need to remove the usage of ViewBag
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static Dictionary<ConstFunctions, bool> ComputePermission(this Controller controller)
        {
            var bag = controller.Session["PermissionBag"] as Dictionary<ConstFunctions, bool>;
            if (bag != null)
            {
                return bag;
            }

            bag = new Dictionary<ConstFunctions, bool>();
            var allFuncs = Enum.GetNames(typeof(ConstFunctions));
            foreach (var funcName in allFuncs)
            {
                var myFuncs = controller.GetUserFunctions();
                var hasAccess = myFuncs.Any(f => f.Description == funcName.ToString());

                ConstFunctions func;
                if (!Enum.TryParse(funcName, true, out func))
                {
                    _log.Error($"failed to parse function :{funcName}!!! description must be same to enum name");
                    continue;
                }

                
                if (!bag.ContainsKey(func))
                {
                    bag.Add(func, hasAccess);
                }
                else
                {
                    bag[func] = hasAccess;
                }
            }

            var loginUser = controller.GetLoginUser();
            bag[ConstFunctions.SuperAdmin] = loginUser.SuperAdmin == ConstStatus.Yes;

            controller.Session["PermissionBag"] = bag;
            return bag;
        }


		//public static bool CanBan(this Controller controller)
		//{
  //          var funcs = controller.MyFunctions();
		//	return funcs.Any(f=>f.Description == ConstFunctions.Ban.ToString());
		//}

  //      public static bool CanChangeDayEntryType(this Controller controller)
  //      {
  //          var funcs = controller.MyFunctions();
  //          return funcs.Any(f => f.Description == ConstFunctions.Allow_Day_Type_and_Entry_Type_Selection.ToString());
  //      }

  //      public static bool CanChangeScanningMode(this Controller controller)
  //      {
  //          var funcs = controller.MyFunctions();
  //          return funcs.Any(f => f.Description == ConstFunctions.Allow_Enable_and_Disable_QR_FR.ToString());
  //      }
    }
}