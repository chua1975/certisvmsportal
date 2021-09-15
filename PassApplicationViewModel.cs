using CertisVMS.Bll.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertisVMSPortal.Models
{
    public class PassApplicationViewModel
    {
        public PassApplicationViewModel()
        {
            //Set hard coded values
            Id = 0;
            Nationality = "SINGAPORE";
            CountryOfBirth = "SINGAPORE";
            PermitExpiryDate = DateTime.Now.AddYears(5);
            DOB = DateTime.Now.AddYears(-18);
            PassType = "SEASONAL PASS";
            RankID = 17;
            Sex = "MALE";
            WorkFlowStage = -1;
            SelectedSources = new string[] { "1", "2" };
            MyProfile = new UserProfile()
            {
                CompanyId = 1,
                CompanyName = "MONETARY AUTHORITY OF SINGAPORE",
                Role = "AS"
            };
        }

        public long Id { get; set; }

        public string NRIC { get; set; }

        public string ApplicantName { get; set; }

        public string ApplicantAliaName { get; set; }

        public string Nationality { get; set; }

        public string CountryOfBirth { get; set; }

        public DateTime? DOB { get; set; }

        public string Sex { get; set; }

        public string Email { get; set; }

        public string Religion { get; set; }

        public string Organisation { get; set; }

        public string Department { get; set; }

        public string JobDesignation { get; set; }

        public string ApplicationStatus { get; set; }

        public string ReviewStatus { get; set; }

        public string Description { get; set; }

        public string PassType { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public long SubmissionId { get; set; }

        public string ImageBase64_1 { get; set; }
        public byte[] Image1 { get; set; }
        public string ImageBase64_2 { get; set; }
        public byte[] Image2 { get; set; }
        public string ImageBase64_3 { get; set; }
        public byte[] Image3 { get; set; }

        public string CompanyID { get; set; }

        public UserProfile MyProfile { get; set; }

        public List<string> AccessGroupList { get; set; }

        public string AccessGroup { get; set; }

        public string JobRoles { get; set; }

        public List<string> JobRolesList { get; set; }

        public DateTime? PermitExpiryDate { get; set; }

        public string HomeAddress { get; set; }

        public long RankID { get; set; }

        public int WorkFlowStage { get; set; } = -1;

        public String FinalStage { get; set; }

        public IEnumerable<string> SelectedSources { get; set; }

    }

    public class UserProfile
    {
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public string Role { get; set; }
    }

    public static class PassApplicationEx
    {
        public static PassApplicationViewModel SetVisitorData(this PassApplicationViewModel model, VisitorViewModel visitor)
        {
            model.NRIC = visitor.FIN;
            model.ApplicantName = visitor.VisitorName;
            //model.MyProfile.CompanyName = visitor.CompanyName;
            model.ValidFrom = DateTime.Now;
            model.ValidTo = DateTime.Now.AddYears(1);
            model.PermitExpiryDate = DateTime.Now.AddYears(2);
            model.ImageBase64_1 = visitor.Photo;
            return model;
        }

        public static PassApplicationViewModel SetStaffData(this PassApplicationViewModel model, StaffViewModel staff)
        {
            model.NRIC = staff.FIN;
            model.ApplicantName = staff.StaffName;
            //model.MyProfile.CompanyName = staff.Company;
            model.ValidFrom = DateTime.Now;
            model.ValidTo = DateTime.Now.AddYears(1);
            model.PermitExpiryDate = DateTime.Now.AddYears(2);
            model.ImageBase64_1 = staff.Photo;
            return model;
        }
    }
}
   