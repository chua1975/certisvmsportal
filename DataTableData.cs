using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CertisVMS.ApiClient.DataTable;
using CertisVMS.Model.DbModels;

namespace CertisVMSPortal.Models
{
	public delegate IList<T> DataProvider1<T>(string userId, out int totalCount, int start = 0, int pageSize = 10, 
		string sortField = "", string sortDirection = "desc", string search = "");

    public delegate IList<T> DataProvider2<T>(string userId, out int totalCount, DateTime? startDate, DateTime? endDate,
           int start = 0, int pageSize = 10, string sortField = "", string sortDirection = "desc", string search = "");

    public delegate IList<T> DataProvider3<T>(string userId, out int totalCount, DateTime? startDate, DateTime? endDate,
           int start = 0, int pageSize = 10, string sortField = "", string sortDirection = "desc", Dictionary<string,string> searchMap = null);

    public static class DataTableReqFactory
    {
        public static DataTableRequest Create(Controller controller, int draw, int start, int length)
        {
            string search, sortColumn, sortDirection;
            controller.GetDataTablePara(out search, out sortColumn, out sortDirection);
            return new DataTableRequest(){
                draw = draw,
                start = start,
                pageSize = length,
                search = search,
                sortDirection = sortDirection,
                sortField = sortColumn};
        }
        public static DataTableRequest CreateWithDateRange(Controller controller, int draw,int start, int length)
        {
            string search, sortColumn, sortDirection;
            DateTime? startDate, endDate;
            controller.GetDataTablePara(out search, out sortColumn, out sortDirection, out startDate, out endDate);
            return new DataTableRequest()
            {
                draw = draw,
                start = start,
                pageSize = length,
                search = search,
                sortDirection = sortDirection,
                sortField = sortColumn,
                startDate = startDate,
                endDate = endDate
            };
        }
    }
    public class DataTableData<T>
	{
		public int draw { get; set; }
		public int recordsFiltered { get; set; }
		public IList<T> data { get; set; }

        public static DataTableData<T> CreateFor(Controller controller,
            int draw, 
            DataTableResponse<T> data)
        {
            var dataTableData = new DataTableData<T>
            {
                draw = draw
            };

            dataTableData.data = data.Data;
            dataTableData.recordsFiltered = data.Total;

            return dataTableData;
        }

        //public static DataTableData<T> CreateFor(Controller controller,
        //	TblUser loginUser, int draw, int start, int length,
        //	DataProvider1<T> dataProvider1,
        //          DataProvider2<T> dataProvider2 = null
        //          )
        //{
        //	string search, sortColumn, sortDirection;
        //	controller.GetDataTablePara(out search, out sortColumn, out sortDirection);

        //	var dataTableData = new DataTableData<T>
        //	{
        //		draw = draw
        //	};
        //	var recordsFiltered = 0;

        //	if (dataProvider1 != null)
        //	{
        //		int siteId = Convert.ToInt32(loginUser.SiteID);
        //		dataTableData.data = dataProvider1(loginUser.UserID,  out recordsFiltered, start, length, sortColumn, sortDirection, search.Trim());
        //		dataTableData.recordsFiltered = recordsFiltered;

        //		return dataTableData;
        //	}
        //          else if(dataProvider2 != null)
        //          {
        //              DateTime? startDate;
        //              DateTime? endDate;
        //              controller.GetDataTableDateRange(out startDate, out endDate);
        //              if (startDate.HasValue) 
        //              {
        //                  startDate = startDate.Value.Date;
        //              }
        //              if (endDate.HasValue)
        //              {
        //                  endDate = endDate.Value.Date;
        //              }

        //              if (startDate == null || endDate == null) { return new DataTableData<T>(); }

        //              int siteId = Convert.ToInt32(loginUser.SiteID);
        //              dataTableData.data = dataProvider2(loginUser.UserID, out recordsFiltered, startDate, endDate,start, length, sortColumn, sortDirection, search.Trim());
        //              dataTableData.recordsFiltered = recordsFiltered;
        //              return dataTableData;
        //          }
        //	////other version data providers...
        //	else
        //	{
        //		return new DataTableData<T>();
        //	}

        //}

        //      public static DataTableData<T> CreateFor(Controller controller,
        //          TblUser loginUser, int draw, int start, int length,
        //          DataProvider3<T> dataProvider3 = null, Dictionary<string, string> searchMap = null
        //          )
        //      {
        //          string search, sortColumn, sortDirection;
        //          controller.GetDataTablePara(out search, out sortColumn, out sortDirection);

        //          var dataTableData = new DataTableData<T>
        //          {
        //              draw = draw
        //          };
        //          var recordsFiltered = 0;
        //          DateTime? startDate;
        //          DateTime? endDate;
        //          controller.GetDataTableDateRange(out startDate, out endDate);
        //          if (startDate.HasValue)
        //          {
        //              startDate = startDate.Value.Date;
        //          }
        //          if (endDate.HasValue)
        //          {
        //              endDate = endDate.Value.Date.AddDays(1);
        //          }

        //          if (startDate == null || endDate == null) { return new DataTableData<T>(); }

        //          int siteId = Convert.ToInt32(loginUser.SiteID);
        //          searchMap.Add("search", search);
        //          dataTableData.data = dataProvider3(loginUser.UserID, out recordsFiltered, startDate, endDate, start, length, sortColumn, sortDirection, searchMap);
        //          dataTableData.recordsFiltered = recordsFiltered;
        //          return dataTableData;
        //      }
    }
}