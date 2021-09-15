using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CertisVMSPortal
{
	public static partial class ControllerEx
	{
		public static string UploadFileToBase64(this Controller controller, string key)
		{
			if (controller.Request.Files.Count == 0)
			{
				return string.Empty;
			}

			var file = controller.Request.Files[key];
			if (file == null)
			{
				return string.Empty;
			}


			byte[] image = new byte[file.ContentLength];
			file.InputStream.Read(image, 0, image.Length);

			var base64 = Convert.ToBase64String(image);

			return base64;
		}

		public static byte[] GetFileRequestAsByteArray(this Controller controller, string key)
		{
			var file = controller.Request.Files[key];
			if (file == null)
			{
				return new byte[0];
			}


			byte[] image = new byte[file.ContentLength];
			file.InputStream.Read(image, 0, image.Length);

			return image;
		}


	}
}