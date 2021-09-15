using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CertisVMS.ApiClient;
using CertisVMS.Bll.QueryCache;
using CertisVMS.Bll.ViewModels;
using CertisVMS.Model.DbModels;
using CertisVMS.Model.Constant;
using CertisVMS.Model.Enum;
using CertisVMSApi.Interface.Models;
using Newtonsoft.Json;

namespace CertisVMSPortal.Helpers
{
    /// <summary>
    /// TODO this class data all should come from api 
    /// </summary>
    public static class DropDownHelper
    {
        private const string Please_Select = "Please Select";

        public static SelectList Site(string selectedValue = "")
        {

            //var sites = new SiteService().All();
            string error;
            var ret = VMSApiClient.TryGetOne<ResponseGetSites>(VMSApiClient.Api_Config + $"/api/sites/all", out error);
            var sites = ret.Data;
            var list = sites.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList IdType(string selectedValue = "")
        {
            return new SelectList(new string[] { ConstIDType.Phone, ConstIDType.Email, ConstIDType.FIN, ConstIDType.NRIC });
        }
        public static SelectList IdTypeForStaff(string selectedValue = "")
        {
            var list = MakeDropdownList(ConstCodeType.IdType, selectedValue)
                .Where(p => p.Text != ConstIDType.NO_ID).ToList();

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList Designations(string selectedValue = "")
        {
            return MakeDropdownList(ConstCodeType.Designation, selectedValue);
        }
        public static SelectList VisitorTypes(string selectedValue = "")
        {
            var data = CacheFetch.VisitorType();
            var list = data.Where(p => p.Status == ConstStatus.Active).Select(p => new SelectListItem { Value = p.VisitorTypeID.ToString(), Text = p.VisitorTypeName }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });

            if (string.IsNullOrEmpty(selectedValue))
            {
                var defaultType = data.Where(x => x.IsDefault == true)
                    .OrderByDescending(x => x.VisitorTypeID).Select(x => x.VisitorTypeID).FirstOrDefault();
                if (defaultType > 0) { selectedValue = defaultType.ToString(); }
            }

            return new SelectList(list, "Value", "Text", selectedValue);

        }

        public static SelectList Dorscon(string selectedValue = "")
        {
            var data = CacheFetch.Dorscon();
            var list = data.Select(p => new SelectListItem { Value = p.DorsconID.ToString(), Text = p.DorsconName }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });

            if (string.IsNullOrEmpty(selectedValue))
            {
                var defaultType = data.OrderBy(x => x.DorsconLevel).Select(x => x.DorsconID).FirstOrDefault();
                if (defaultType > 0) { selectedValue = defaultType.ToString(); }
            }

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList FunctionList(string selectedValue = "")
        {
            //var userBo = new UserBo();
            //var functionList = userBo.AllFunctions();
            string error;
            var functionList = VMSApiClient.TryGetList<TblFunction>(VMSApiClient.Api_Authentication + $"/api/functions/all", out error);


            var list = functionList.Select(p => new SelectListItem { Value = p.FunctionID.ToString(), Text = p.FunctionName }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList PurposeOfVisits(string selectedValue = "")
        {
            return MakeDropdownList(ConstCodeType.PurposeOfVisit, selectedValue);
        }

        private static SelectList MakeDropdownList(string codeType, string defaultValue)
        {

            var list = new List<SelectListItem>();

            //CodeService codeBo = new CodeService();
            string error;
            var codeList = VMSApiClient.TryGetList<TblCode>(VMSApiClient.Api_Config + $"/api/cache/code", out error);

            var listByType = codeList.Where(x => x.CodeType == codeType
                              && x.Status == ConstStatus.Active).ToList();

            //List<TblCode> codeList = codeBo.GetCodesByType(codeType);
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            if (listByType != null && listByType.Count > 0)
            {
                foreach (var code in listByType)
                {
                    SelectListItem item = new SelectListItem()
                    {
                        Value = code.CodeValue,
                        Text = code.CodeValue,
                        //Selected= code.CodeValue == defaultValue
                    };
                    if (defaultValue == item.Text)
                    {
                        defaultValue = item.Value;
                    }
                    list.Add(item);
                }
            }
            return new SelectList(list, "Value", "Text", defaultValue); ;
        }


        public static SelectList EntryTypes(string selectedValue = ConstVisitation.EntryType_Single)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = ConstVisitation.EntryType_Single, Text = ConstVisitation.EntryType_Single
                },
                new SelectListItem()
                {
                    Value = ConstVisitation.EntryType_Multiple, Text = ConstVisitation.EntryType_Multiple
                }
            };

            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public static SelectList BypassBio(string selectedValue = ConstStatus.Yes)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem() {Value = ConstStatus.Yes, Text = ConstStatus.YesDesc},
                new SelectListItem() {Value = ConstStatus.No, Text = ConstStatus.NoDesc}
            };

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList StaffTypes(string selectedValue = "")
        {
            var data = CacheFetch.StaffType();

            var list = data.Where(p => p.Status == ConstStatus.Active).Select(p => new SelectListItem { Value = p.StaffTypeID.ToString(), Text = p.StaffTypeName }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });

            if (string.IsNullOrEmpty(selectedValue))
            {
                var defaultType = data.Where(x => x.IsDefault == true)
                    .OrderByDescending(x => x.StaffTypeID).Select(x => x.StaffTypeID).FirstOrDefault();
                if (defaultType > 0) { selectedValue = defaultType.ToString(); }
            }

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList Projects(string selectedValue = "")
        {

            var data = CacheFetch.Project();
            var list = data.Where(p => p.Status == ConstStatus.Active)
                .Select(p => new SelectListItem
                {
                    Value = p.ProjectName.ToString(),
                    Text = p.ProjectName
                }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);
        }


        public static SelectList Departments(string selectedValue)
        {

            var list = new List<SelectListItem>();

            //CodeService codeBo = new CodeService();
            string error;
            var codeList = VMSApiClient.TryGetList<TblCode>(VMSApiClient.Api_Config + $"/api/cache/code", out error);

            codeList = codeList.Where(c => c.CodeType == ConstCodeType.Department).ToList();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            if (codeList != null && codeList.Count > 0)
            {
                foreach (var code in codeList)
                {
                    SelectListItem item = new SelectListItem()
                    {
                        Value = code.CodeID.ToString(),
                        Text = code.CodeValue,
                        //Selected= code.CodeValue == defaultValue
                    };

                    list.Add(item);
                }
            }
            return new SelectList(list, "Value", "Text", selectedValue); ;


        }

        public static SelectList Relationships(string selectedValue)
        {

            var list = MakeDropdownList("Relationship", selectedValue);

            return list;
        }


        public static SelectList ByPassBiometricReasons(string selectedValue = Please_Select)
        {
            var list = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = Please_Select,Value = ""}
                };
            string error;
            var codeList = VMSApiClient.TryGetList<TblCode>(VMSApiClient.Api_Config + $"/api/cache/code", out error);

            var records = codeList.Where(c => c.CodeType == ConstCodeType.BypassBiometricReason)
                                       .Where(x => x.Status == ConstStatus.Active)
                                       .Select(x => new SelectListItem() { Text = x.CodeValue, Value = x.CodeValue }).ToList();
            list.AddRange(records);

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList ApprovalLevels(string selectedValue = "")
        {
            var data = CacheFetch.ApprovalLevel();

            var list = data.Select(p => new SelectListItem { Value = p.ID.ToString() + "|" + p.ApprovalRole + "|" + p.ApprovalBy, Text = p.LevelName }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].Value != "")
                {
                    var listSplit = list[i].Value.Split('|');
                    list[i].Value = listSplit[0] + "|" + GetRoleAndApprovalNames(listSplit[1], listSplit[2]);
                }
            }
            return new SelectList(list, "Value", "Text", selectedValue);
        }


        public static string GetRoleAndApprovalNames(string roleId, string approvalBys)
        {
            //return new ApprovalLevelService().GetRoleAndApprovalNames(roleId, approvalBys);
            string error;
            var req = new ApprovalLevelViewModel() { ApprovalRole = roleId, ApprovalBy = approvalBys };
            var reqJson = JsonConvert.SerializeObject(req);
            var approvalData = VMSApiClient.Post<string>(
                VMSApiClient.Api_Enrollment +
               $"/api/approvalLevel/byRoleAndApprovalBy", reqJson, out error);
            return approvalData;
        }

        public static SelectList Roles(string selectedValue = "")
        {
            string error;
            var allRoles =
                VMSApiClient.TryGetList<TblRole>(VMSApiClient.Api_Authentication + $"/api/roles/all", out error);

            var list = allRoles.Select(p => new SelectListItem { Value = p.RoleID.ToString(), Text = p.RoleName }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);
        }


        public static SelectList SelectDefaultFloor(this IList<int> floorIds, int? selectedValue)
        {
            var list = floorIds.Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() }).ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "0",
                Text = Please_Select
            });
            if (!selectedValue.HasValue)
            {
                selectedValue = 0;
            }
            return new SelectList(list, "Value", "Text", selectedValue.ToString());
        }



        public static SelectList MFATypes(string selectedValue = "")
        {
            var data = from MFAMode d in Enum.GetValues(typeof(MFAMode))
                       select new SelectListItem()
                       {
                           Value = (d).ToString(),
                           Text = d.GetDisplayName()
                       };

            var list = data.ToList();
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });

            return new SelectList(list, "Value", "Text", selectedValue);

        }

        public static SelectList QuestionnaireCategory(string selectedValue = "")
        {
            string error;
            var allDatas =
                VMSApiClient.TryGetList<TblQuestionaireCategory>(VMSApiClient.Api_Config + $"/api/questionnaires/allquestionnairecategory", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.QuestionaireCatName }).ToList();
            }
            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList SubAnswerCodeType(string selectedValue = "")
        {
            string error;
            var allDatas =
                VMSApiClient.TryGetList<string>(VMSApiClient.Api_Config + $"/api/SubQuestionnaireAnswerCode/allSubAnswerCodeType", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p, Text = p }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList Priority(string selectedValue = "")
        {
   
            string error;
            var allDatas =
               VMSApiClient.TryGetList<TblPriorityLevel>(VMSApiClient.Api_Case + $"/api/master/priority", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p.PriorityLevel.ToString(), Text = p.Name }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);

        }

        public static SelectList EventType(string selectedValue = "")
        {

            string error;
            var allDatas =
               VMSApiClient.TryGetList<TblEventType>(VMSApiClient.Api_Case + $"/api/master/eventtype", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);

        }

        public static SelectList EventSubType(string selectedValue = "")
        {

            string error;
            var allDatas =
               VMSApiClient.TryGetList<TblEventSubType>(VMSApiClient.Api_Case + $"/api/master/eventsubtype", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);

        }

        public static SelectList SLAConfig(string selectedValue = "")
        {

            string error;
            var allDatas =
               VMSApiClient.TryGetList<TblSLAConfig>(VMSApiClient.Api_Case + $"/api/master/slaconfig", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);

        }


        public static SelectList CaseStatus(string selectedValue = "")
        {

            string error;
            var allDatas =
               VMSApiClient.TryGetList<int, string>(VMSApiClient.Api_Case + $"/api/master/casestatus", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p.Key.ToString(), Text = p.Value.ToString() }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);

        }
        public static SelectList CaseType(string selectedValue = "")
        {

            string error;
            var allDatas =
               VMSApiClient.TryGetList<TblCaseType>(VMSApiClient.Api_Case + $"/api/master/casetype", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);

        }

        public static SelectList ReportingChannel(string selectedValue = "")
        {

            string error;
            var allDatas =
               VMSApiClient.TryGetList<TblReportingChannel>(VMSApiClient.Api_Case + $"/api/master/reportingchannel", out error);
            var list = new List<SelectListItem>();
            if (allDatas != null)
            {
                list = allDatas.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            }

            list.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = Please_Select
            });
            return new SelectList(list, "Value", "Text", selectedValue);

        }
    }
}