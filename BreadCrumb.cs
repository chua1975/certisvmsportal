using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertisVMSPortal.Models
{
    public class BreadCrumb
    {
        public IList<BreadCrumbItem> Middle { get; set; }
        public BreadCrumbItem Last { get; set; }

        public BreadCrumb(IList<BreadCrumbItem> middle, BreadCrumbItem last)
        {
            Middle = middle;
            Last = last;
        }
    }

    public class BreadCrumbItem
    {
        public string Link { get; set; }
        public string Text  { get; set; }
    }
}