using CertisVMS.Model.Constant.ErrorMessageKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertisVMS.Bll.QueryCache;

namespace CertisVMSPortal
{
	public static partial class ControllerEx
	{
		public class UIKeys
		{
			public const string IsSuccess = "IsSuccess";
			public const string IsError = "IsError";
			public const string Message = "Message";
		}

		public class ActiveStatus
		{
			public const string Active = "Active";
			public const string Deactive = "Deactive";
		}

        public class ConstString
        {
            public const string Stars = "****";
        }

        public static void ShowMessage(this Controller controller, bool isSuccess, string msg = "")
		{
			if (isSuccess)
			{
				controller.TempData[UIKeys.IsSuccess] = true;
                controller.TempData[UIKeys.Message] = CacheFetch.DisplayMessage(MessageKeyVMS.OperationSuccess);
            }
			else
			{
				controller.TempData[UIKeys.IsError] = true;
				controller.TempData[UIKeys.Message] = CacheFetch.DisplayMessage(MessageKeyVMS.OperationFailed);
            }

			if (msg != string.Empty)
			{
				controller.TempData[UIKeys.Message] = msg;
			}
		}

	}
}