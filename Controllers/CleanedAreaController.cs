using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S1640.Models;
using ZXing.QrCode.Internal;

namespace S1373.Controllers
{
    public class CleanedAreaController : Controller
    {
        // GET: CleanedArea
        S1640Entities conn = new S1640Entities();
        public ActionResult AddEdit()
        {
            S1640Entities db= new S1640Entities();
            InwardValidation inward= new InwardValidation();    
            return View(inward);
        }
        //Fetching data from barcode scann only cleaned data will shown
        [HttpGet]
        public ActionResult FetchBin(string barcode)
        {
            S1640Entities db = new S1640Entities();
            
                Int32 mUserNo = Convert.ToInt32(Session["Userid"]);
                DateTime docdate = DateTime.Now;
                var record = db.InawardTables.Where(s => s.BarCode == barcode && s.Status == "Clean").FirstOrDefault();
                var TempDataExist = db.TempTables.Where(s => s.BinCode == barcode && s.Status == "Loaded").FirstOrDefault();
                if (TempDataExist == null)
                {
                    if (record != null)
                    {
                        var TempData = db.SP_TempTable(record.MTransNo, docdate, barcode, "NA", 0, mUserNo, docdate, "Loaded", "Clean", record.BinCondition, record.BinFillStatus);
                        var data = db.TempTables.Where(s => s.BinCode == barcode && s.Status == "Loaded").FirstOrDefault();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("NotFound", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Exist", JsonRequestBehavior.AllowGet);
                }
        }
        //Deleting Temptable data 
        public ActionResult DtlDelete(int MTransNo ,string Barcode)
        {
            S1640Entities db = new S1640Entities();
            var record = db.TempTables.Where(s => s.BinCode == Barcode && s.InwardNo==MTransNo ).FirstOrDefault();
            if (record != null)
            {
                db.TempTables.Remove(record);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public class MyItem
        {
            public string MTransNo { get; set; }
            public bool IsChecked { get; set; }
        }
        [HttpPost]
        public JsonResult SaveData(List<MyItem> data)
        {
            S1640Entities db = new S1640Entities();
            InwardValidation inward = new InwardValidation();
            Int32 mUserNo = Convert.ToInt32(Session["Userid"]);
            DateTime docdate = DateTime.Now;
            try
            {
                foreach (var item in data)
                {
                    int mTransNo = Convert.ToInt32(item.MTransNo);
                    // Example: Find and update your entity
                    var record = db.TempTables.Where(s => s.MTransNo == mTransNo).FirstOrDefault();
                    if (record != null)
                    {
                        var TempData = db.SP_Livestock(record.MTransNo, docdate, record.BinCode, "NA", 0, mUserNo, docdate, "Loaded", "Clean", record.BinCondition, record.BinFillStatus);
                    }
                    db.TempTables.Remove(record);
                    db.SaveChanges();
                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch { }
            return Json("Error", JsonRequestBehavior.AllowGet);  
        }
    }
}