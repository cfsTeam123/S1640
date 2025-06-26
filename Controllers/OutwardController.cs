using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S1640.Models;
using ZXing.QrCode.Internal;

namespace S1640.Controllers
{
    public class OutwardController : Controller
    {

        // GET: Outward
        S1640Entities conn = new S1640Entities();
        public ActionResult AddEdit()
        {
            S1640Entities db = new S1640Entities();
            InwardValidation inward = new InwardValidation();
            return View(inward);
        }
        //Fetching data from barcode scann only cleaned data will shown
        [HttpGet]
        public ActionResult FetchBin(string barcode)
        {
            if (barcode == "")
                return View();
            S1640Entities db = new S1640Entities();
            Int32 mUserNo = Convert.ToInt32(Session["Userid"]);
            DateTime docdate = DateTime.Now;
            var record = db.LiveStockDatas.Where(s => s.BinCode == barcode && s.Status == "Loaded").FirstOrDefault();
            var InwardDataExist = db.InawardTables.Where(s => s.BarCode == barcode && s.Status == "Clean" && s.Remarks2 == "Inwarded").OrderByDescending(s => s.MTransNo).FirstOrDefault();
            if (record == null && InwardDataExist != null)
                return Json("Inwarded", JsonRequestBehavior.AllowGet);

            var TempDataExist = db.TempTables.Where(s => s.BinCode == barcode && s.Status == "Unloaded").FirstOrDefault();

            if (record != null && TempDataExist==null)
            {
                    var TempData = db.SP_TempTable(record.InwardNo, docdate, barcode, "NA", 0, mUserNo, docdate, "Unloaded", "Clean", record.BinCondition, record.BinFillStatus);
                    var data = db.TempTables.Where(s => s.BinCode == barcode && s.Status == "Unloaded").FirstOrDefault();
                    return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Exist", JsonRequestBehavior.AllowGet);
            }
        }
        //Deleting Temptable data 
        public ActionResult DtlDelete(int MTransNo, string Barcode)
        {
            S1640Entities db = new S1640Entities();
            var record = db.TempTables.Where(s => s.BinCode == Barcode && s.InwardNo == MTransNo).FirstOrDefault();
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
            public string Barcode { get; set; }
            public int InwardNo { get; set; }
            public bool IsChecked { get; set; }
        }
        [HttpPost]
        public JsonResult SaveData(List<MyItem> data)
        {
            S1640Entities db = new S1640Entities();
            InwardValidation inward = new InwardValidation();
            Int32 mUserNo = Convert.ToInt32(Session["Userid"]);
            DateTime docdate = DateTime.Now;
            var Createdon = DateTime.Now;
            try
            {
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        int InwardNo = Convert.ToInt32(item.InwardNo);
                        string Barcode = Convert.ToString(item.Barcode);
                        var LiveStockdata = db.LiveStockDatas.Where(s => s.InwardNo == InwardNo).FirstOrDefault();
                        var BarcodeMTransNo = db.BinMasters.Where(s => s.BarCode == LiveStockdata.BinCode && s.Status != "N").Select(s => s.MTransNo).FirstOrDefault();
                        db.SP_Transaction(0, LiveStockdata.InwardNo, docdate, LiveStockdata.BinCode, LiveStockdata.BinCondition, "Clean", LiveStockdata.BinFillStatus, mUserNo, Createdon, "Unloaded", null, null, BarcodeMTransNo);
                        // Example: Find and update your entity
                        var record = db.LiveStockDatas.Where(s => s.InwardNo == InwardNo || s.BinCode == Barcode).FirstOrDefault();
                        db.LiveStockDatas.Remove(record);
                        db.SaveChanges();
                        var tempdata = db.TempTables.Where(s => s.InwardNo == InwardNo).FirstOrDefault();
                        db.TempTables.Remove(tempdata);
                        db.SaveChanges();
                        var InwardData = db.InawardTables.Where(s => s.MTransNo == InwardNo).FirstOrDefault();
                        if (InwardData != null)
                        {
                            InwardData.Remarks2 = "Unloaded";
                            db.SaveChanges();
                        }
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
            }
            catch 
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}
