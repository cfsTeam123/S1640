using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S1640.Models;

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
            S1640Entities db = new S1640Entities();
            Int32 mUserNo = Convert.ToInt32(Session["Userid"]);
            DateTime docdate = DateTime.Now;
            var record = db.LiveStockDatas.Where(s => s.BinCode == barcode && s.Status == "loaded").FirstOrDefault();
            if (record != null)
            {
                    var TempData = db.SP_TempTable(record.MTransNo, docdate, barcode, "NA", 0, mUserNo, docdate, "Unloaded", "Clean", record.BinCondition, record.BinFillStatus);
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
                    string Barcode = Convert.ToString(item.Barcode);
                    // Example: Find and update your entity
                    var record = db.LiveStockDatas.Where(s => s.MTransNo == mTransNo || s.BinCode== Barcode).FirstOrDefault();
                    db.LiveStockDatas.Remove(record);
                    db.SaveChanges();
                    var tempdata = db.TempTables.Where(s => s.MTransNo == mTransNo).FirstOrDefault();
                    db.TempTables.Remove(tempdata);
                    db.SaveChanges();
                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch { }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
    }
}
