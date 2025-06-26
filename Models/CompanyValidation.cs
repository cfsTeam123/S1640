using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S1640.Models
{
    public class CompanyValidation
    {
        public int MTransNo { get; set; }
        public string CompCode { get; set; }
        public string CompName { get; set; }
        public string Natureofwork { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string State { get; set; }
        public Nullable<short> StateCode { get; set; }
        public string PhoneNo { get; set; }
        public string GeneralEmail { get; set; }
        public string SalesEmail { get; set; }
        public string PurchaseEmail { get; set; }
        public string Website { get; set; }
        public string GSTIN { get; set; }
        public string PAN { get; set; }
        public string CIN { get; set; }
        public Nullable<System.DateTime> FromYear { get; set; }
        public Nullable<System.DateTime> ToYear { get; set; }
        public Nullable<byte> LockCounter { get; set; }
        public string LockStatus { get; set; }
        public Nullable<short> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<short> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string DeleteStatus { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountNo { get; set; }
        public string MICRCode { get; set; }
        public string SalPhoneNo { get; set; }
        public string PurPhoneNo { get; set; }
        public string City { get; set; }
        public Nullable<int> PinCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}