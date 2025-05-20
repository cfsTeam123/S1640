using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S1640.Models
{
    public class ReportListValidation
    {
       
        public string BarCode { get; set; }
        public string BinCondition { get; set; }
        public string BinWash { get; set; }
        public string BinFillStatus { get; set; }
        public string InwardNo { get; set; }
        public string BinCode { get; set; }
        public string BinStatus { get; set; }
        public string UserId { get; set; }

        public string BinDetails { get; set; }
        //public string OutwardReport { get; set; }
        //public string LiveStock { get; set; }

        public string ReportName { get; set; }
        public string ReportUrl { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }



        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
       
    }
}