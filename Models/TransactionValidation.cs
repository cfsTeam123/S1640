using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S1640.Models
{
    public class TransactionValidation
    {
        public int MTransNo { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public string BarCode { get; set; }
        public string BinCondition { get; set; }
        public string BinWash { get; set; }
        public string BinFillStatus { get; set; }
        public string Remarks1 { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Status { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> DocNo { get; set; }
    }
}