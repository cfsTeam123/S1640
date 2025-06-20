using Newtonsoft.Json;
using S1640.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace S1640.Controllers
{
    public class BinMasterController : Controller
    {
        // GET: BinMaster
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddEdit( int MTransNo=0)
        {
            S1640Entities conn = new S1640Entities();
            BinMasterValidation binMaster = new BinMasterValidation();
            ViewBag.Status = new List<SelectListItem>()
            {
                new SelectListItem {Text="Inactive", Value="N", Selected=false},
                new SelectListItem {Text="Active", Value="Y", Selected=true},
            };
            if (MTransNo > 0)
            {
                S1640Entities db = new S1640Entities();
                BinMaster RS = db.BinMasters.Where(x => x.MTransNo == MTransNo).FirstOrDefault();
                binMaster.BarCode = RS.BarCode;
                binMaster.Status = RS.Status;
                binMaster.MTransNo=RS.MTransNo;
                return View(binMaster);
            }
            else
            {
                return View(binMaster);
            }
        }
        [HttpPost]
        public ActionResult AddEdit( BinMasterValidation BinMaster, string MainSubmit, string Status ,int MTransNo)
        {
            if (MTransNo>0)
            {
                S1640Entities db = new S1640Entities();
                BinMaster RS = db.BinMasters.Where(x => x.MTransNo == MTransNo).FirstOrDefault();
                RS.ModifiedBy = Convert.ToByte(Session["Userid"]);
                RS.ModifiedOn = DateTime.Today;
                RS.Status = Status;
                db.BinMasters.Add(RS);
                db.Entry(RS).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
               
            }
            else
            {
                S1640Entities db = new S1640Entities();
                var binMaster = new BinMaster
                {
                    BarCode = BinMaster.BarCode,
                    CreatedOn = DateTime.Now,
                    CreatedBy = Convert.ToByte(Session["Userid"]),
                    Status = "Y",
                };
                db.BinMasters.Add(binMaster);
                db.SaveChanges();
                var mTransNo = binMaster.MTransNo;
               
            }
            return RedirectToAction("Index", "BinMaster");
        }
        // Method to generate barcode
        public JsonResult GenerateBarcode()
        {
                try
                {
                    S1640Entities db = new S1640Entities();
                    // Get the last used barcode from the database
                    var lastBarcode = db.BinMasters.OrderByDescending(b => b.BarCode).FirstOrDefault();
                    // Start from SR5001 if there are no existing barcodes
                    int barcodeNumber = 5001;
                    if (lastBarcode != null)
                    {
                        // Extract the numeric part of the last barcode (after 'SR')
                        var lastBarcodeNumber = int.Parse(lastBarcode.BarCode.Substring(2));
                        barcodeNumber = lastBarcodeNumber + 1;
                    }
                    // Generate the barcode
                    string newBarcode = "SR" + barcodeNumber;
                    // Return the generated barcode and the MTransNo back to the AddEdit
                    return Json(new { success = true, barcode = newBarcode });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = ex.Message });
                }
        }
        public ActionResult DtlDelete(int MTransNo = 0)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            S1640Entities db = new S1640Entities();
            BinMaster RS = db.BinMasters.Where(x => x.MTransNo == MTransNo).FirstOrDefault();
            if (RS != null)
            {
                RS.Status = "Y";
                db.Entry(RS).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            // Redirect back to the "AddEdit" page with the MTransNo
            return RedirectToAction("AddEdit", "BinMaster", new { MTransNo = RS.MTransNo });
        }
    }
}