using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CertisVMSPortal
{
	public static partial class ControllerEx
	{
		/// <summary>
		/// Data table helper
		/// </summary>

		public static string SearchText(this Controller controller)
		{
			string search = controller.Request["search[value]"];
			return search;
		}

		public static int SearchStatus(this Controller controller)
		{
			int searchStatus = -1;
			int.TryParse(controller.Request["status"], out searchStatus);

			return searchStatus;
		}
        public static void GetDataTablePara(this Controller obj, 
            out string search, out string sortColumn, out string sortDirection, out DateTime? startDate,out DateTime? endDate)
        {
            GetDataTablePara(obj, out search, out sortColumn, out sortDirection);

            obj.GetDataTableDateRange(out startDate, out endDate);
            if (startDate.HasValue)
            {
                startDate = startDate.Value.Date;
            }
            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date;
            }

        }
        public static void GetDataTablePara(this Controller obj, out string search, out string sortColumn, out string sortDirection)
		{
			search = "";
			sortColumn = "";
			sortDirection = "";

			search = obj.Request[DataTableQueryString.Searching];

			if (obj.Request[DataTableQueryString.OrderingColumn] != null)
			{
				sortColumn = obj.Request[DataTableQueryString.OrderingColumn].ToString();

				sortColumn = obj.Request["columns[" + sortColumn + "][name]"];
			}

			if (obj.Request[DataTableQueryString.OrderingDir] != null)
			{
				sortDirection = obj.Request[DataTableQueryString.OrderingDir];
			}

        }

        public static void GetDataTableDateRange(this Controller obj, out DateTime? start, out DateTime? end)
		{
            start = null;
			end = null;

			var strStart = obj.Request["dateFrom"];
			DateTime dtStart;
			if (!string.IsNullOrEmpty(strStart) && DateTime.TryParseExact(strStart, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtStart))
			{
				start = dtStart;
			}

			var strEnd = obj.Request["dateTo"];
			DateTime dtEnd;
			if (!string.IsNullOrEmpty(strEnd) && DateTime.TryParseExact(strEnd, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEnd))
			{
				end = dtEnd.AddDays(1).AddTicks(-1);
			}
		}
		public static string SortColumn(this Controller controller)
		{
			string sortColumn = "";
			if (controller.Request["order[0][column]"] != null)
			{
				var columnName = controller.Request["order[0][column]"];
				sortColumn = controller.Request["columns[" + columnName + "][name]"];
			}

			return sortColumn;
		}

		public static string SortDirection(this Controller controller)
		{
			string sortDirection = "asc";
			if (controller.Request["order[0][dir]"] != null)
			{
				sortDirection = controller.Request["order[0][dir]"];
			}

			return sortDirection;
		}


	}
}