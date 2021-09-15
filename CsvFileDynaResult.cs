using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsvHelper;

namespace CertisVMSPortal.Models
{
	public class CsvFileDynaResult : FileResult 
	{
		private IList<string> _header;
        private IList<IList<string>> _rows;
		private const int MAXROWS = 1000;
		public CsvFileDynaResult(IList<string> headers, IList<IList<string>> rows)
			: base("text/csv")
        {
            _header = headers;
            _rows = rows;
        }

		protected override void WriteFile(HttpResponseBase response)
		{
			var outPutStream = response.OutputStream;
			response.AddHeader("content-disposition", "attachment; filename=" + "export.csv");
			using (var streamWriter = new StreamWriter(outPutStream, System.Text.Encoding.UTF8))
			using (var writer = new CsvWriter(streamWriter))
			{
                var len = _header.Count;

                for (var i = 0; i < len; i++)
                {
                    writer.WriteField(_header[i]);
                }
                writer.NextRecord();

                var rows = _rows.Count;
				for (int i = 0;i<rows;i++)
                {
                    var r = _rows[i].Count;
                    for (int j = 0; j < r; j++)
                    {
                        var v = _rows[i][j];
                        writer.WriteField(v);
                    }
                    writer.NextRecord();
                }
            }
		}
    }
}
