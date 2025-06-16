using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S1640.Models;

namespace S1640.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if (Session["Userid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult AddEdit()
        {
            S1640Entities conn = new S1640Entities();
           // ViewBag.Department = new SelectList(conn.OtherMasters.Where(x => x.DeleteStatus == "N" && x.LockStatus == "Y" && x.MasterType == "Employee Department").OrderBy(x => x.MasterName), "MasterName", "MasterName");
             ViewBag.UserType = new List<SelectListItem>(){
                new SelectListItem {Text="ADMIN", Value="ADMIN", Selected=true},
              //  new SelectListItem {Text="HOD", Value="HOD", Selected=false},
                new SelectListItem {Text="OPERATOR", Value="OPERATOR", Selected=false},
                };
            ViewBag.LockStatus = new List<SelectListItem>(){
                new SelectListItem {Text="Inactive", Value="N", Selected=true},
                new SelectListItem {Text="Active", Value="Y", Selected=false},
                };
            Int32 MTransNo = Convert.ToInt32(Session["MTransNo"]);
            Session.Remove("MTransNo");
            if (MTransNo > 0)
            {                
                UserMaster UserMasterModule = conn.UserMasters.Where(x => x.MTransNo == MTransNo).FirstOrDefault();
                UserValidation obj = new UserValidation();
                obj.MTransNo = UserMasterModule.MTransNo;
                obj.UserId = UserMasterModule.UserId == null ? null : UserMasterModule.UserId.Trim();
                string dec = HomeController.Decrypt(UserMasterModule.PW, "mskl-1sv4-xklq23");
                obj.PW = dec;
                obj.RePW = dec;
                obj.UserName = UserMasterModule.UserName == null ? null : UserMasterModule.UserName.Trim();
                obj.UserType = UserMasterModule.UserType == null ? null : UserMasterModule.UserType.Trim();
                obj.LockStatus = UserMasterModule.LockStatus;
                return View(obj);
            }
            else
            {
                UserValidation UserMasterModule = new UserValidation();
                return View(UserMasterModule);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult AddEdit(UserValidation UserMasterModule)
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            DateTime TodayDate = DateTime.Now;
            Int32 mSubscID = Convert.ToInt32(Session["SubscID"]);
            Int32 mUserNo = Convert.ToInt32(Session["Userid"]);
            S1640Entities db = new S1640Entities();
           
            //Password Encrapted
            string encVa = HomeController.Encrypt(UserMasterModule.PW, "mskl-1sv4-xklq23");
            if (UserMasterModule.MTransNo > 0)
            {
                S1640Entities conn = new S1640Entities();
                UserMaster RS = conn.UserMasters.Where(x => x.MTransNo == UserMasterModule.MTransNo).FirstOrDefault();
                RS.UserName = UserMasterModule.UserName;
                RS.UserId = UserMasterModule.UserId;
                RS.PW = encVa;
                RS.ModifiedBy = Convert.ToByte(Session["Userid"]);
                RS.ModifiedOn = DateTime.Today;
                RS.DeleteStatus = "N";
                RS.LockStatus = UserMasterModule.LockStatus;
                conn.Entry(RS).State = System.Data.Entity.EntityState.Modified;
                conn.SaveChanges();
            }
            else
            {
                S1640Entities conn = new S1640Entities();
                UserMaster obj = new UserMaster();
                obj.UserName = UserMasterModule.UserName;
                obj.UserId = UserMasterModule.UserId;
                obj.SubscID = Convert.ToSByte(mSubscID);
                obj.PW = encVa;
                obj.UserType = UserMasterModule.UserType;
                obj.CreatedBy = Convert.ToByte(Session["Userid"]);
                obj.CreatedOn = DateTime.Now;
                obj.DeleteStatus = "N";
                obj.LockStatus = UserMasterModule.LockStatus;
                conn.UserMasters.Add(obj);
                conn.SaveChanges();
            }
            return RedirectToAction("Index");
         
        }
        public JsonResult RecordSerch(String searchStr = "")
        {
            Int32 mSubscID = Convert.ToInt32(Session["SubscID"]);
            S1640Entities conn = new S1640Entities();
            var AllList = conn.UserMasters.Where(x => x.DeleteStatus == "N").OrderByDescending(x => x.MTransNo).Take(12).ToList();
            if (searchStr != "")
            {
                AllList = conn.UserMasters.Where(x => x.DeleteStatus == "N").OrderBy(x => x.UserName).ThenBy(x => x.UserType).ToList();
                string[] result = searchStr.Split(' ');
                foreach (String str in result)
                {
                    String mStr = str.Trim().ToUpper();
                    if (mStr.Length > 0)
                    {
                        AllList = AllList.Where(x => x.DeleteStatus == "N" && (x.UserName.ToUpper().Contains(mStr) || x.UserType.ToUpper().Contains(mStr))).OrderBy(x => x.UserName).ThenBy(x => x.UserType).ToList();
                    }
                }
            }
            return Json(AllList, JsonRequestBehavior.AllowGet);
        }
        public String RecDelete(int MTransNo = 0, String mReason = "")
        {
            S1640Entities db = new S1640Entities();
            UserMaster obj3 = db.UserMasters.Where(x => x.MTransNo == MTransNo).FirstOrDefault();
            if (obj3 != null)
            {
                //obj3.DeleteReason = mReason;
                obj3.DeleteStatus = "Y";
                db.Entry(obj3).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return "Delete";
        }
        public JsonResult UserIdCheck(string UserId)
        {
            S1640Entities conn = new S1640Entities();
            bool exists = conn.UserMasters.Any(s => s.UserId == UserId);
            return Json(exists, JsonRequestBehavior.AllowGet);
        }
    }
}