using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc; // or Microsoft.AspNetCore.Mvc for .NET Core


namespace S1640.Models
{
    public class BarCodeViewValidation
    {
        public string Barcode { get; set; }              // Use string if it's a barcode string or path
        public BinMasterValidation BinMaster { get; set; }
    }
}