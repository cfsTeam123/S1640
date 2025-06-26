using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S1640.Models;

namespace S1640.Controllers
{
    public class CompanyController : Controller
    {
        public ActionResult Index(int MTransNo = 0)
        {
            if (MTransNo > 0)
            {
                S1640Entities conn = new S1640Entities();
                ViewBag.LockStatus = new List<SelectListItem>(){ 
                new SelectListItem {Text="Inactive", Value="N", Selected=false},
                new SelectListItem {Text="Active", Value="Y", Selected=false},
                };
                //ViewBag.State = new SelectList(conn.OtherMasters.Where(x => x.MasterType == "State" && x.DeleteStatus == "N" && x.LockStatus == "Y").OrderBy(x => x.MasterName), "MTransNo", "MasterName");
                //ViewBag.State = new SelectList(conn.StateMasters.Where(x => x.DeleteStatus == "N" && x.LockStatus == "Y").OrderBy(x => x.State), "State", "State");
                //ViewBag.City = new SelectList(conn.OtherMasters.Where(x => x.MasterType == "City" && x.DeleteStatus == "N" && x.LockStatus == "Y").OrderBy(x => x.MasterName), "MasterName", "MasterName");
                //ViewBag.Country = new SelectList(conn.OtherMasters.Where(x => x.MasterType == "Country" && x.DeleteStatus == "N" && x.LockStatus == "Y").OrderBy(x => x.MasterName), "MasterName", "MasterName");
                Company CompanyModule = conn.Companies.FirstOrDefault();
                //StateMaster STC = conn.StateMasters.Where(x => x.State.Trim() == CompanyModule.State.Trim()).FirstOrDefault();
                //OtherMaster CTC = conn.OtherMasters.Where(x => x.MasterName.Trim() == CompanyModule.Country.Trim()).FirstOrDefault();
                CompanyValidation obj = new CompanyValidation();
                obj.MTransNo = CompanyModule.MTransNo;
                obj.CompCode = CompanyModule.CompCode;
                obj.CompName = CompanyModule.CompName;
                obj.Natureofwork = CompanyModule.Natureofwork;
                obj.Address1 = CompanyModule.Address1;
                obj.Address2 = CompanyModule.Address2;
                obj.Address3 = CompanyModule.Address3;
                obj.Country= CompanyModule.Country;
                //obj.CountryCode= CompanyModule.CountryCode;
                obj.State = CompanyModule.State;
                obj.StateCode = Convert.ToByte(CompanyModule.StateCode);
                //obj.City = CompanyModule.City;
                obj.PinCode = CompanyModule.PinCode;
                obj.PhoneNo = CompanyModule.PhoneNo;
                //obj.PurPhoneNo = CompanyModule.PurPhoneNo;
                //.SalPhoneNo = CompanyModule.SalPhoneNo;
                obj.GeneralEmail = CompanyModule.GeneralEmail;
                obj.PurchaseEmail = CompanyModule.PurchaseEmail;
                obj.SalesEmail = CompanyModule.SalesEmail;
                obj.Website = CompanyModule.Website;
                obj.GSTIN = CompanyModule.GSTIN;
                obj.PAN = CompanyModule.PAN;
                obj.CIN = CompanyModule.CIN;
                obj.FromYear =CompanyModule.FromYear;
                obj.ToYear = CompanyModule.ToYear;
                obj.BankName = CompanyModule.BankName;
                obj.BranchName = CompanyModule.BranchName;
                obj.IFSCCode = CompanyModule.IFSCCode;
                obj.AccountNo = CompanyModule.AccountNo;
                obj.MICRCode = CompanyModule.MICRCode;
                obj.LockStatus = CompanyModule.LockStatus;
                return View(obj);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(CompanyValidation CompanyModule)        {
           
            S1640Entities db = new S1640Entities();
            //StateMaster STC = db.StateMasters.Where(x => x.State.Trim() == CompanyModule.State.Trim()).FirstOrDefault();
            //OtherMaster CTC = db.OtherMasters.Where(x => x.MasterName.Trim() == CompanyModule.Country.Trim()).FirstOrDefault();
            Company obj = db.Companies.FirstOrDefault();
            obj.CompCode = CompanyModule.CompCode;
            obj.CompName = CompanyModule.CompName;
            obj.Natureofwork = CompanyModule.Natureofwork;
            obj.Address1 = CompanyModule.Address1;
            obj.Address2 = CompanyModule.Address2;
            obj.Address3 = CompanyModule.Address3;
            obj.Country = CompanyModule.Country;
            //obj.CountryCode = CTC.CountryCode ;
            obj.State = CompanyModule.State;
            //obj.StateCode = Convert.ToByte(STC.StateCode); 
           // obj.City = CompanyModule.City;
            obj.PinCode = CompanyModule.PinCode;
            obj.PhoneNo = CompanyModule.PhoneNo;
           // obj.PurPhoneNo = CompanyModule.PurPhoneNo;
           // obj.SalPhoneNo = CompanyModule.SalPhoneNo;
            obj.GeneralEmail = CompanyModule.GeneralEmail;
            obj.PurchaseEmail = CompanyModule.PurchaseEmail;
            obj.SalesEmail = CompanyModule.SalesEmail;
            obj.Website = CompanyModule.Website;
            obj.GSTIN = CompanyModule.GSTIN;
            obj.PAN = CompanyModule.PAN;
            obj.CIN = CompanyModule.CIN;
            obj.FromYear = CompanyModule.FromYear;
            obj.ToYear = CompanyModule.ToYear;
            obj.BankName = CompanyModule.BankName;
            obj.BranchName = CompanyModule.BranchName;
            obj.IFSCCode = CompanyModule.IFSCCode;
            obj.AccountNo = CompanyModule.AccountNo;
            obj.MICRCode = CompanyModule.MICRCode;
            obj.LockStatus = CompanyModule.LockStatus;
            obj.ModifiedBy = Convert.ToByte(Session["Userid"]);
            obj.ModifiedOn = DateTime.Now;
            Int32 Y = Convert.ToInt32(obj.LockCounter);
            byte Z = Convert.ToByte(Y + 1);
            obj.LockCounter = Z;
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Company", new { MTransNo = obj.MTransNo });
        }        
    }
}