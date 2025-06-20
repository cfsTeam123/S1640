using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S1640.Models;
using ZXing.QrCode.Internal;

namespace S1373.Controllers
{
    public class CleaningAreaController : Controller
    {
       public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddEdit()
        {
            return View();
        }

        public class MyItem
        {
            public string MtransNo { get; set; }
            public bool IsChecked { get; set; }
        }
        [HttpPost]
        public JsonResult UpdateData(List<MyItem> data)
        {
            S1640Entities db = new S1640Entities();
            InwardValidation inward = new InwardValidation();
            try
            {
                foreach (var item in data)
                {
                    if (item.IsChecked == true)
                    {
                        Int32 mUserNo = Convert.ToInt32(Session["Userid"]);
                        int mTransNo = Convert.ToInt32(item.MtransNo);
                        int mUserNo1 = mTransNo > 0 ? mUserNo : 0;
                        // Example: Find and update your entity
                        //var record = db.InawardTables.Where(s => s.MTransNo == mTransNo).FirstOrDefault();
                        //if (record != null)
                        //{
                        //    string Status1 = mTransNo > 0 ? "Update" : "Insert";

                        //    db.SP_Inward(mTransNo, record.DocDate, record.DocDate2, record.BarCode, record.BinCondition, "Clean", record.BinFillStatus, record.CreatedBy, record.CreatedOn, "Clean", record.ModifiedBy, record.ModifiedOn, 0, Status1);
                        //}
                        var record = db.InawardTables.FirstOrDefault(x => x.MTransNo == mTransNo);
                        if (record != null)
                        {
                            record.BinWash = "Clean";
                            record.Status = "Clean";
                            record.ModifiedBy = mUserNo;
                            record.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }

                    }
                }
                db.SaveChanges();

                return Json(new { success = true, message = "Data updated successfully." });
            }
            catch
            {
                return Json(new { success = false, message = "Error while updating data." });
            }
        }



    }
}