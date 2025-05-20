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
        
        public ActionResult AddEdit(int MTransNo = 0)
        {
            S1640Entities conn = new S1640Entities();
            InwardValidation RS = new InwardValidation();
            // Fetch list of barcodes for dropdown (used in both GET and POST actions)
            var barcodeList = conn.InawardTables
                                  .Where(s => s.Status == "LOADED")
                                  .OrderByDescending(s => s.MTransNo)
                                  .Select(s => new { s.MTransNo, s.BarCode })
                                  .ToList();

          
            if (MTransNo > 0)
            {
                // Get the existing data for the specified MTransNo
                InawardTable InwardModule = conn.InawardTables.FirstOrDefault(x => x.MTransNo == MTransNo);
                if (InwardModule != null)
                {
                    RS.MTransNo = MTransNo;
                }
                return Json(conn.InawardTables.Where(x => x.MTransNo == MTransNo).FirstOrDefault(), JsonRequestBehavior.AllowGet);

            }

            // Set dropdown lists for the form (both for creating and editing)
            ViewBag.BarCode = new SelectList(barcodeList, "MTransNo", "BarCode");
            return View(RS);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult AddEdit(InwardValidation model)
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (model.DocNo == 0 && model.BarCode == null)
            {
                return RedirectToAction("Index", "Home");
            }
            S1640Entities conn = new S1640Entities();
            InwardValidation n = new InwardValidation();
            // To fetch the MTransNo from BinMaster (based on BarCode)
            int? MTransNo = conn.BinMasters
                                .Where(x => x.BarCode.Trim() == model.BarCode.Trim())
                                .Select(x => x.MTransNo)
                                .FirstOrDefault();
            if (model.MTransNo > 0)
            {
                // load the existing entity from the database
                var existingEntity = conn.InawardTables.FirstOrDefault(x => x.MTransNo == model.MTransNo);
                if (existingEntity != null)
                {
                    // Update the existing entity
                    existingEntity.DocDate2 = DateTime.Now;
                    existingEntity.Remarks2 = model.Remarks2;
                    existingEntity.Status = "UNLOADED";
                    existingEntity.ModifiedOn = DateTime.Now;
                    existingEntity.ModifiedBy = Convert.ToByte(Session["Userid"]);
                    conn.SaveChanges();
                }
                else
                {
                    // If not found, handle the error (could be a delete or data issue)
                    ModelState.AddModelError("", "The record was not found.");
                    return View(model);
                }
                    Transaction RR = new Transaction
                    {
                        DocNo = existingEntity.DocNo,
                        BarCode = existingEntity.BarCode,
                        BinCondition = model.BinCondition,
                        BinFillStatus = model.BinFillStatus,
                        BinWash = model.BinWash,
                        Remarks1 = model.Remarks2,
                        CreatedBy = Convert.ToByte(Session["Userid"]),
                        DocDate = DateTime.Now,
                        CreatedOn = DateTime.Now,
                        Status = "UNLOADED"
                    };
                    conn.Transactions.Add(RR);
                    // Save changes to the database
                    conn.SaveChanges();
            }
            else
            {
                return RedirectToAction("AddEdit");
            }
            return RedirectToAction("AddEdit");
        }
        public JsonResult GetData(int Barcode)
        {
            using (S1640Entities conn = new S1640Entities())
            {
                // Fetch data based on MTransNo (Barcode is assumed to map to MTransNo)
                var data = conn.InawardTables.Where(s => s.MTransNo == Barcode).ToList();
                // Return the data as JSON, explicitly allowing GET requests
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
