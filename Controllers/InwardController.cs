using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using S1640.Models;

namespace S1640.Controllers
{
    public class InwardController : Controller
    {
        // GET: Inward/AddEdit
        public ActionResult AddEdit(int MTransNo = 0)
        {
            S1640Entities conn = new S1640Entities();
            InwardValidation RS = new InwardValidation();
            ViewBag.BinCondition = new List<SelectListItem> {   new SelectListItem { Text = "Good", Value = "Good",Selected=true },
                                                                new SelectListItem { Text = "Broken", Value = "Broken" }                 };
            ViewBag.BinClean = new List<SelectListItem>     {   new SelectListItem { Text = "Clean", Value = "Clean",Selected=true },
                                                                new SelectListItem { Text = "Unclean", Value = "Unclean" }               };
            ViewBag.BinFillStatus = new List<SelectListItem>{   new SelectListItem { Text = "Fill", Value = "Fill" },
                                                                new SelectListItem { Text = "Empty", Value = "Empty" ,Selected=true}     };
            if (MTransNo > 0)
            {
                InawardTable InwardModule = conn.InawardTables.FirstOrDefault(x => x.MTransNo == MTransNo);
                if (InwardModule != null)
                {
                    RS.MTransNo = MTransNo;
                }
                return Json(conn.InawardTables.Where(x => x.MTransNo == MTransNo).FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }
            else
            {
               
            }
            return View(RS);
        }
        //To save the data Once the opertaor scan the barcode of bin
        [HttpPost]
        public ActionResult AddData(int MTransNo, string condition, string binclean, string binfillstatus, string barcode)
        {
            S1640Entities db = new S1640Entities();
            Int32 mSubscID = Convert.ToInt32(Session["SubscID"]);
            Int32 mUserNo = Convert.ToInt32(Session["Userid"]);

            string Status1 = MTransNo > 0 ? "Update" : "Insert"; 
            int mUserNo1 = MTransNo > 0 ? mUserNo : 0;
            DateTime? Modifiedon = MTransNo > 0 ? DateTime.Now : (DateTime?)null;

            var DocDate = DateTime.Now;
            var DocDate2 = DateTime.Now;
            var Createdon = DateTime.Now;
            var Status = binclean == "Clean" ? "Clean" : "Unclean";
            var BarcodeExist=db.InawardTables.Where(s=>s.BarCode==barcode).FirstOrDefault();
            int mTransNo = MTransNo > 0 ? MTransNo : 0;
            if (BarcodeExist == null || Status1== "Update")
            {
                var barcodeExists = db.BinMasters.FirstOrDefault(s => s.BarCode == barcode);
                if (barcodeExists != null)
                {
                    try
                    {
                        db.SP_Inward(mTransNo, DocDate, DocDate2, barcode,condition,binclean, binfillstatus, mUserNo, Createdon, Status, mUserNo1, Modifiedon, 0, Status1);
                    }
                    catch
                    {
                    }
                    //int generatedTransNo = (int)mTransNo.Value;
                    // You can return this value if needed
                }
                else
                {
                    return Json("NotExist", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
              
                return Json("Exist", JsonRequestBehavior.AllowGet);
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

    }
}
