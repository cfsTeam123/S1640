using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S1640.Models
{
    public class InwardValidation
    {
        public int MTransNo { get; set; }
        public int DocNo { get; set; }
       
        public int InwardNo { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public Nullable<System.DateTime> DocDate2 { get; set; }
       
       // [Required(ErrorMessage = "Please Scan The Barcode")]
        public string BarCode { get; set; }
        //[Required(ErrorMessage = "Please select valid values from Dropdown")]
        public string BinCondition { get; set; }
        public string BinClean { get; set; }
        public string BinFillStatus { get; set; }
       // [Required(ErrorMessage = "Please Enter The Remarks")]
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Status { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}