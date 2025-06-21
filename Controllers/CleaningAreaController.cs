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
                        DateTime? Modifiedon = mTransNo > 0 ? DateTime.Now : (DateTime?)null;
                        DateTime Createdon= DateTime.Now;
                        int mUserNo1 = mTransNo > 0 ? mUserNo : 0;
                        var record = db.InawardTables.FirstOrDefault(x => x.MTransNo == mTransNo);
                        if (record != null)
                        {
                            record.BinWash = "Clean";
                            record.Status = "Clean";
                            record.ModifiedBy = mUserNo;
                            record.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                        db.SP_Transaction(0, mTransNo, Createdon, record.BarCode, record.BinCondition, "Clean", record.BinFillStatus, mUserNo, Createdon, "Clean", mUserNo1, Modifiedon, record.DocNo);
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