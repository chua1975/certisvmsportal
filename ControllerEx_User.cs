using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using CertisVMS.Bll.Helpers;
using CertisVMS.Bll.ViewModels;
using CertisVMS.Model.Constant;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.DbModels;
using CertisVMSPortal.Helpers;

namespace CertisVMSPortal
{
    public static partial class ControllerEx
    {
        public static void SetLoginUser(this Controller controller, TblUser user, TblUserControlPermission permission)
        {
            controller.HttpContext.Session[SessionKeys.LoginUser] = user;

            if (permission != null)
            {
                controller.HttpContext.Session[SessionKeys.UserPermissionControl] = permission;
            }
        }

        public static TblUser GetLoginUser(this Controller controller)
		{
            ////TODO get from redis
			var shouldNotNull = controller.HttpContext.Session[SessionKeys.LoginUser] as TblUser;
			if (shouldNotNull == null)
			{
				return new TblUser();
			}

			return shouldNotNull;
		}


        /// <summary>
        /// User Control Permission : canLock/canResetPassword
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static TblUserControlPermission GetUserControlerPermission(this Controller controller)
        {
            var shouldNotNull = controller.HttpContext.Session[SessionKeys.UserPermissionControl] as TblUserControlPermission;
            if (shouldNotNull == null)
            {
                return new TblUserControlPermission();
            }

            return shouldNotNull;
        }


        public static void SetTemplate(this Controller controller, string themeName)
        {
            if (!string.IsNullOrEmpty(themeName))
            {
                controller.HttpContext.Session[SessionKeys.Template] = themeName;
            }
            else
            {
                controller.HttpContext.Session[SessionKeys.Template] = "White";
            }
        }

        public static void SetUserFunctions(this Controller controller, IList<TblFunction> functionNames)
		{
			controller.HttpContext.Session[SessionKeys.UserFunctions] = functionNames;
		}

		public static List<TblFunction> GetUserFunctions(this Controller controller)
		{
			return controller.HttpContext.Session[SessionKeys.UserFunctions] as List<TblFunction>;
		}

        public static void SetUserFunctionMenu(this Controller controller, IList<FunctionMenuItem> menu)
        {
            controller.HttpContext.Session[SessionKeys.FunctionMenu] = menu;
        }

        public static IList<FunctionMenuItem> GetUserFunctionMenu(this Controller controller)
        {
            var list = controller.HttpContext.Session[SessionKeys.FunctionMenu] as IList<FunctionMenuItem>;
            return list;
        }

        public static string httpWithJson(string url, string jsonParam, string method = "POST", int maxRetry = 10)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
            {
                return true;
            };

            _log.Debug($"================");
            _log.Debug($"'{url}'");
            _log.Debug($"================");

            var count = 0;
            while (count < maxRetry)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = method;
                    request.ContentType = "application/json";

                    if (method != "GET")
                    {
                        var byteData = Encoding.UTF8.GetBytes(jsonParam);
                        var length = byteData.Length;
                        request.ContentLength = length;
                        var writer = request.GetRequestStream();
                        writer.Write(byteData, 0, length);
                        writer.Close();
                    }

                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"))
                        .ReadToEnd();
                    return responseString;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message.ToLower();
                    if (msg.Contains("unable") && msg.Contains("connect") && msg.Contains("remote"))
                    {
                        count++;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        _log.Error(ex);
                        break;
                    }
                }
            }

            return "";
        }
    }
}