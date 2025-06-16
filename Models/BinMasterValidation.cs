using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S1640.Models
{
    public class BinMasterValidation
    {
        public int MTransNo { get; set; }
        [Required(ErrorMessage = "Please Scan The Barcode")]
        public string BarCode { get; set; }
        [Required(ErrorMessage = "Please select valid values from Dropdown")]
        public string Status { get; set; }
        public string BarCodeImage { get; set; } // base64 image string
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}