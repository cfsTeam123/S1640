//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace S1640.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LiveStockData
    {
        public int MTransNo { get; set; }
        public Nullable<int> InwardNo { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public string BinCode { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Status { get; set; }
        public string BinStatus { get; set; }
        public string BinCondition { get; set; }
        public string BinFillStatus { get; set; }
    }
}
