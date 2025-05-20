using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            // Fetch list of barcodes for dropdown (used in both GET and POST actions)
            var barcodeList = conn.BinMasters
                                  .Where(s => s.Status == "Y")
                                  .OrderByDescending(s => s.MTransNo)
                                  .Select(s => new { s.MTransNo, s.BarCode })
                                  .ToList();

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
                ViewBag.BarCode = new SelectList(barcodeList, "BarCode", "BarCode");
                ViewBag.LockStatus = new List<SelectListItem>
                            {
                                new SelectListItem { Text = "Inactive", Value = "N" },
                                new SelectListItem { Text = "Active", Value = "Y" }
                            };
                ViewBag.BinCondition = new List<SelectListItem>
                            {
                                new SelectListItem { Text = "Good", Value = "Good" },
                                new SelectListItem { Text = "Broken", Value = "Broken" }
                            };
                ViewBag.BinWash = new List<SelectListItem>
                            {
                                new SelectListItem { Text = "Wash", Value = "Wash" },
                                new SelectListItem { Text = "UnWash", Value = "UnWash" }
                            };
                ViewBag.BinFillStatus = new List<SelectListItem>
                            {
                                new SelectListItem { Text = "Fill", Value = "Fill" },
                                new SelectListItem { Text = "Empty", Value = "Empty" }
                            };
            }
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
            //Move to Home Page
            if(model.DocNo==0 && model.BarCode == null)
            {
                return RedirectToAction("Index","Home");
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
                // Try to load the existing entity from the database
                var existingEntity = conn.InawardTables
                                        .FirstOrDefault(x => x.MTransNo == model.MTransNo);
                var transaction = conn.Transactions
                                 .FirstOrDefault(x => x.MTransNo == model.MTransNo);
                if (existingEntity != null)
                {
                    // Update the existing entity
                    existingEntity.BinCondition = model.BinCondition;
                    existingEntity.BinFillStatus = model.BinFillStatus;
                    existingEntity.BinWash = model.BinWash;
                    existingEntity.Remarks1 = model.Remarks1;
                    existingEntity.ModifiedOn = DateTime.Now;
                    existingEntity.ModifiedBy = Convert.ToByte(Session["Userid"]);
                    conn.SaveChanges();

                    transaction.BinCondition = model.BinCondition;
                    transaction.BinFillStatus = model.BinFillStatus;
                    transaction.BinWash = model.BinWash;
                    transaction.Remarks1 = model.Remarks1;
                    transaction.ModifiedOn = DateTime.Now;
                    transaction.ModifiedBy = Convert.ToByte(Session["Userid"]);
                    conn.SaveChanges();
                }
                else
                {
                    // If not found, handle the error (could be a delete or data issue)
                    ModelState.AddModelError("", "The record was not found.");
                    return View(model);
                }
            }
            else
            {
                // Insert a new record
                // Create a new InawardTable record
                InawardTable newRecord = new InawardTable
                {
                    DocNo = MTransNo,
                    BarCode = model.BarCode,
                    BinCondition = model.BinCondition,
                    BinFillStatus = model.BinFillStatus,
                    BinWash = model.BinWash,
                    Remarks1 = model.Remarks1,
                    CreatedBy = Convert.ToByte(Session["Userid"]),
                    DocDate = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    Status = "LOADED"
                };

                // Create a new Transaction record
                Transaction RR = new Transaction
                {
                    DocNo = MTransNo,
                    BarCode = model.BarCode,
                    BinCondition = model.BinCondition,
                    BinFillStatus = model.BinFillStatus,
                    BinWash = model.BinWash,
                    Remarks1 = model.Remarks1,
                    CreatedBy = Convert.ToByte(Session["Userid"]),
                    DocDate = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    Status = "LOADED"
                };

                // Add the new records to the context
                conn.InawardTables.Add(newRecord);
                conn.Transactions.Add(RR);
                // Save changes to the database
                conn.SaveChanges();
            }
            return RedirectToAction("AddEdit");
        }
    }
}
