using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S1640.Models;

namespace S1640.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        S1640Entities conn = new S1640Entities();
        public ActionResult Index()
        {
            ReportListValidation obj = new ReportListValidation();
            //    CFS_Entities conn = new CFS_Entities();
            ViewBag.BarCode = new SelectList(conn.BinMasters.Where(x => x.Status == "Y").OrderBy(x => x.BarCode), "BarCode", "BarCode");
            ViewBag.DocDate = new SelectList(conn.LiveStockDatas.Where(x => x.Status == "N").OrderBy(x => x.DocDate), "DocDate", "DocDate");
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
            ViewBag.Status = new List<SelectListItem>(){
                    new SelectListItem {Text="All", Value="ALL", Selected=true},
                    new SelectListItem {Text="LOADED", Value="LOADED", Selected=false},
                    new SelectListItem {Text="UNLOADED", Value="UNLOADED", Selected=false},
                };
            obj.FromDate = DateTime.Now;
            obj.ToDate = DateTime.Now;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Index(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Reports", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BarCode = ReportListModule.BarCode;
            obj.BinCondition = ReportListModule.BinCondition;
            obj.BinStatus = ReportListModule.BinStatus;
            obj.Status = ReportListModule.Status;
            obj.InwardNo = ReportListModule.InwardNo;
            obj.DocDate = ReportListModule.DocDate;
            obj.BinCode = ReportListModule.BinCode;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate;
            //obj.OutwardReport = ReportListModule.OutwardReport;
            //obj.LiveStock = ReportListModule.LiveStock;

            if (ReportListModule.ReportName == "BinDetailsPrint")
            {
                return RedirectToAction("BinDetailsPrint", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "BinDetailsList")
            {
                return RedirectToAction("BinDetailsList", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "InawardReportPrint")
            {
                return RedirectToAction("InawardReportPrint", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "InawardReportList")
            {
                return RedirectToAction("InawardReportList", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "OutwardReportPrint")
            {
                return RedirectToAction("OutwardReportPrint", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "OutwardReportList")
            {
                return RedirectToAction("OutwardReportList", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "LiveStockPrint")
            {
                return RedirectToAction("LiveStockPrint", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "LiveStockList")
            {
                return RedirectToAction("LiveStockList", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "CleaningAreaList")
            {
                return RedirectToAction("CleaningAreaList", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "CleaningAreaPrint")
            {
                return RedirectToAction("CleaningAreaPrint", "Reports", obj);
            } 
            else if (ReportListModule.ReportName == "CleanedAreaList")
            {
                return RedirectToAction("CleanedAreaList", "Reports", obj);
            }
            else if (ReportListModule.ReportName == "CleanedAreaPrint")
            {
                return RedirectToAction("CleanedAreaPrint", "Reports", obj);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult BinDetailsPrint(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();

            obj.BarCode = ReportListModule.BarCode;
            //obj.PartNo = ReportListModule.PartNo;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            //obj.ItemStatus = ReportListModule.ItemStatus;
            //obj.WareHouse = ReportListModule.WareHouse;

            return View(obj);
        }
        public ActionResult BinDetailsList(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BarCode = ReportListModule.BarCode;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);

            return View(obj);
        }

        public ActionResult InawardReportPrint(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BarCode = ReportListModule.BarCode;
            obj.BinCondition = ReportListModule.BinCondition;
            obj.BinWash = ReportListModule.BinWash;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.Status = ReportListModule.Status;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate;
            return View(obj);
        }
        public ActionResult InawardReportList(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BarCode = ReportListModule.BarCode;
            obj.BinCondition = ReportListModule.BinCondition;
            obj.BinWash = ReportListModule.BinWash;
            obj.Status = ReportListModule.Status;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate;
            return View(obj);
        }

        public ActionResult OutwardReportPrint(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BarCode = ReportListModule.BarCode;
            obj.BinCondition = ReportListModule.BinCondition;
            obj.BinWash = ReportListModule.BinWash;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.Status = ReportListModule.Status;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            return View(obj);
        }
        public ActionResult OutwardReportList(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BarCode = ReportListModule.BarCode;
            obj.BinCondition = ReportListModule.BinCondition;
            obj.BinWash = ReportListModule.BinWash;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.Status = ReportListModule.Status;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            return View(obj);
        }
        public ActionResult LiveStockPrint(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BarCode = ReportListModule.BarCode;
            obj.DocDate = ReportListModule.DocDate;
            obj.BinCode = ReportListModule.BinCode;
            obj.Remarks = ReportListModule.Remarks;
            obj.UserId = ReportListModule.UserId;
            obj.Status = ReportListModule.Status;
            obj.BinStatus = ReportListModule.BinStatus;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            return View(obj);
        }
        public ActionResult LiveStockList(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BinCondition = ReportListModule.BinCondition;
            obj.DocDate = ReportListModule.DocDate;
            obj.BarCode = ReportListModule.BarCode;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.BinWash = ReportListModule.BinWash;
            obj.Status = ReportListModule.Status;
            obj.BinStatus = ReportListModule.BinStatus;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            return View(obj);
        }
        public ActionResult CleaningAreaList(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BinCondition = ReportListModule.BinCondition;
            obj.DocDate = ReportListModule.DocDate;
            obj.BarCode = ReportListModule.BarCode;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.BinWash = ReportListModule.BinWash;
            obj.Status = ReportListModule.Status;
            obj.BinStatus = ReportListModule.BinStatus;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            return View(obj);
        }
        public ActionResult CleaningAreaPrint(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BinCondition = ReportListModule.BinCondition;
            obj.DocDate = ReportListModule.DocDate;
            obj.BarCode = ReportListModule.BarCode;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.BinWash = ReportListModule.BinWash;
            obj.Status = ReportListModule.Status;
            obj.BinStatus = ReportListModule.BinStatus;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            return View(obj);
        } public ActionResult CleanedAreaList(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BinCondition = ReportListModule.BinCondition;
            obj.DocDate = ReportListModule.DocDate;
            obj.BarCode = ReportListModule.BarCode;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.BinWash = ReportListModule.BinWash;
            obj.Status = ReportListModule.Status;
            obj.BinStatus = ReportListModule.BinStatus;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            return View(obj);
        }
        public ActionResult CleanedAreaPrint(ReportListValidation ReportListModule)
        {
            //if (Session["EmployeeCode"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            ReportListValidation obj = new ReportListValidation();
            obj.BinCondition = ReportListModule.BinCondition;
            obj.DocDate = ReportListModule.DocDate;
            obj.BarCode = ReportListModule.BarCode;
            obj.BinFillStatus = ReportListModule.BinFillStatus;
            obj.BinWash = ReportListModule.BinWash;
            obj.Status = ReportListModule.Status;
            obj.BinStatus = ReportListModule.BinStatus;
            obj.FromDate = ReportListModule.FromDate;
            obj.ToDate = ReportListModule.ToDate?.AddDays(1);
            return View(obj);
        }

    }
}