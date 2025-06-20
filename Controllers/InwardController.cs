using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
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
            if (barcode == "") 
                return View();

            var DocDate = DateTime.Now;
            var DocDate2 = DateTime.Now;
            var Createdon = DateTime.Now;
            var Status = binclean == "Clean" ? "Clean" : "Unclean";
            var BarcodeExist=db.InawardTables.Where(s=>s.BarCode==barcode ).FirstOrDefault();
           // int  mTransNo = MTransNo > 0 ? MTransNo : 0;
            if (BarcodeExist == null || Status1== "Update")
            {
                var BarcodeMTransNo = db.BinMasters.Where(s => s.BarCode == barcode && s.Status != "N").Select(s=>s.MTransNo).FirstOrDefault();
                var barcodeExists = db.BinMasters.FirstOrDefault(s => s.BarCode == barcode && s.Status != "N");
                if (barcodeExists != null)
                {
                    try
                    {
                       // System.Data.Entity.Core.Objects.ObjectParameter mID = new System.Data.Entity.Core.Objects.ObjectParameter("MTransNo", typeof(Int32));
                        var mTransNo = new ObjectParameter("MTransNo", 0); // treated as input-output
                        db.SP_Inward(mTransNo, DocDate, DocDate2, barcode,condition,binclean, binfillstatus, mUserNo, Createdon, Status, mUserNo1, Modifiedon, BarcodeMTransNo, Status1);
                        var mtr = Convert.ToInt32(mTransNo.Value);
                        db.SP_Transaction(0, mtr, DocDate,barcode,condition,binclean, binfillstatus, mUserNo, Createdon, Status, mUserNo1, Modifiedon, BarcodeMTransNo);
                    }
                    catch
                    {
                    }
                    //int generatedTransNo = (int)mTransNo.Value;
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
        public static int GetInwardNo(int InwardNo)
        {
            using (var db = new S1640Entities())
            {
                // Input parameter
                var inputParam = new SqlParameter("@InputInwardNo", InwardNo);

                // Output parameter
                var outputParam = new SqlParameter
                {
                    ParameterName = "@MTransNo",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                // Call the stored procedure
                db.Database.ExecuteSqlCommand(
                    "EXEC SP_GetInwardNo @InputInwardNo, @MTransNo OUTPUT",
                    inputParam, outputParam
                );

                // Return the output value
                return (int)outputParam.Value;
            }
        }

    }
}
