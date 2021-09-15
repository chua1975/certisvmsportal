
//using CertisVMS.Model.DbModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;

//namespace CertisVMSPortal.Controllers
//{
//    public class ActionFilterController : ActionFilterAttribute
//    {
//        public string TableName { get; set; }
//        public string Description { get; set; }
//        public string ActionType { get; set; }

//        public Dictionary<string, string> EntityReadMap = new Dictionary<string, string>() {
//            {"TblShowerAccesse", "Shower Access" },
//            {"TblQrCodeRecord", "QRCode Records" },
//            {"ReportVisitation", "Visitation Report" },
//            {"ReportOverStay", "Overstay Report" },
//            {"ReportMovement", "Movement Report" },
//            {"TblSenseNebulaRecord","Access Records" },
//            {"ApprovalPendingList","Approval Pending List" },
//            {"ApprovalRejectList","Approval Reject List" },
//        };

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            CertisVMS.Bll.UserAudit.UserAuditHelper.ModuleName = TableName;
//        }

//        public override void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            ////Action执行之后
//            if (filterContext.IsChildAction)
//            {
//                return;
//            }
//            var moduleName = "";
//            if (!EntityReadMap.ContainsKey(TableName))
//            {
//                if (!CertisVMS.Bll.UserAudit.UserAuditHelper.EntityNameMaps.ContainsKey(TableName))
//                {
//                    return;
//                }
//                else
//                {
//                    moduleName = CertisVMS.Bll.UserAudit.UserAuditHelper.EntityNameMaps.FirstOrDefault(e => e.Key == TableName).Value;
//                }
//            }
//            else
//            {
//                moduleName = EntityReadMap.FirstOrDefault(e => e.Key == TableName).Value;
//            }
//            var user = filterContext.HttpContext.Session["LoginUser"];
//            if (user != null)
//            {
//                TblUser loginUser = filterContext.HttpContext.Session["LoginUser"] as TblUser;
//                var id = "";
//                if (filterContext.RouteData.Values.ContainsKey("id"))
//                {
//                    id = filterContext.RouteData.Values["id"].ToString();
//                }

//                var action = "";
//                if (filterContext.RouteData.Values.ContainsKey("action"))
//                {
//                    action = filterContext.RouteData.Values["action"].ToString();
//                }
//                var desc = $"{action} page.";
//                if (!string.IsNullOrWhiteSpace(Description))
//                {
//                    desc = Description;
//                }
//                var actionType = "Read";
//                if (!string.IsNullOrWhiteSpace(ActionType))
//                {
//                    actionType = ActionType;
//                }

//                var audit = new TblUserAudit
//                {
//                    TableName = TableName,
//                    ModuleName = moduleName,
//                    CreatedBy = loginUser.UserID,
//                    DateCreated = DateTime.Now,
//                    ActionType = actionType,
//                    Description = desc
//                };
//                if (!string.IsNullOrWhiteSpace(id))
//                {
//                    audit.EntityID = id;
//                }

//                var individualName = "";
//                long lid;
//                if (!string.IsNullOrWhiteSpace(id) && long.TryParse(id, out lid))
//                {
//                    var staffBo = new StaffService();
//                    var visitorBo = new VisitorService();
//                    switch (TableName)
//                    {
//                        case "TblRegisteredStaff":
//                            var rstaff = staffBo.GetStaffByRegisteredStaffID(lid);
//                            if (rstaff != null)
//                            {
//                                individualName = rstaff.StaffName;
//                            }
//                            break;
//                        case "TblRegisteredVisitor":
//                            var rvisitor = visitorBo.GetVisitorByVisitationId(lid);
//                            if (rvisitor != null)
//                            {
//                                individualName = rvisitor.VisitorName;
//                            }
//                            break;
//                        case "TblVisitor":
//                            var visitor = visitorBo.GetById(lid);
//                            if (visitor != null)
//                            {
//                                individualName = visitor.VisitorName;
//                            }
//                            break;
//                        case "TblStaff":
//                            var staff = staffBo.GetById(lid);
//                            if (staff != null)
//                            {
//                                individualName = staff.StaffName;
//                            }
//                            break;
//                    }
//                }
//                if (!string.IsNullOrWhiteSpace(individualName))
//                {
//                    audit.FieldName = individualName;
//                }
//                CertisVMS.Bll.UserAudit.UserAuditBo.Add(audit);
//            }
//        }
//    }
//}