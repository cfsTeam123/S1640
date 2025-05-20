using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using S1640.Models;


namespace S1640.Controllers
{
    public class UserAuthController : Controller
    {
        // GET: UserAuthorization        
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
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            DateTime TodayDate = DateTime.Now;
            Int32 MTransNo = Convert.ToInt32(Session["MTransNo"]);
            Session.Remove("MTransNo");   

            S1640Entities conn = new S1640Entities();           
            ViewBag.UserNo = new SelectList(conn.UserMasters.Where(x => x.DeleteStatus == "N" && x.LockStatus == "Y" && x.UserType != "ADMIN").OrderBy(x => x.UserId), "MTransNo", "UserName");
            ViewBag.CUserNo = new SelectList(conn.UserMasters.Where(x => x.DeleteStatus == "N" && x.LockStatus == "Y" && x.UserType != "ADMIN").OrderBy(x => x.UserId), "MTransNo", "UserName");
            //var Dept = (from CM in conn.Menulists
            //            select new
            //            {
            //                Department = CM.Department,
            //            }).Distinct().OrderBy(x => x.Department).ToList();
           // ViewBag.Dept = new SelectList(Dept, "Department", "Department");
            ViewBag.AuthType = new List<SelectListItem>(){
                new SelectListItem {Text="No Authorization", Value="N", Selected=false},
                new SelectListItem {Text="Read Only", Value="R", Selected=false},
                new SelectListItem {Text="Full Authorization", Value="F", Selected=false}
                };
            if (MTransNo > 0)
            {
                UserAuthorization UserAuthModule = conn.UserAuthorizations.Where(x => x.MTransNo == MTransNo).FirstOrDefault();
                UserAuthorizationValidation obj = new UserAuthorizationValidation();
                obj.UserNo = UserAuthModule.UserNo;
                //obj.Dept = UserAuthModule.Dept.Trim();
                if (obj.AuthType!=null)
                {
                    obj.AuthType = "F";
                }                
                return View(obj);
            }
            else
            {
                UserAuthorizationValidation UserAuthModule = new UserAuthorizationValidation();                
                return View(UserAuthModule);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult AddEdit(UserAuthorizationValidation UserAuthModule, string MainSubmit, string Add)
        {
            
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (!string.IsNullOrEmpty(Add))
                {
                    S1640Entities db = new S1640Entities();                    
                    UserAuthorization obj = db.UserAuthorizations.Where(x => x.UserNo == UserAuthModule.UserNo /*&& x.Dept == UserAuthModule.Dept*/).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["MTransNo"] = obj.MTransNo;
                        return RedirectToAction("AddEdit");
                    }
                    else
                    {
                        if (UserAuthModule.CUserNo > 0)
                        {
                            List<UserAuthorization> obj3 = db.UserAuthorizations.Where(x => x.UserNo == UserAuthModule.CUserNo /*&& x.Dept == UserAuthModule.Dept*/).ToList();
                            foreach (UserAuthorization Sub in obj3)
                            {
                                UserAuthorization obj4 = new UserAuthorization();
                                obj4.UserNo = UserAuthModule.UserNo;
                                //obj4.Dept = UserAuthModule.Dept;
                                obj4.ModuleNo = Sub.ModuleNo;
                                obj4.AuthType = Sub.AuthType;
                                db.UserAuthorizations.Add(obj4);
                                db.SaveChanges();
                                Session["MTransNo"] = obj4.MTransNo;                                
                            }
                            return RedirectToAction("AddEdit");
                        }
                        else
                        {
                            UserAuthorization objD = new UserAuthorization();
                            objD.UserNo = UserAuthModule.UserNo;
                            //objD.Dept = UserAuthModule.Dept;
                            objD.ModuleNo = 0;
                            objD.AuthType = "";
                            db.UserAuthorizations.Add(objD);
                            db.SaveChanges();
                            Session["MTransNo"] = objD.MTransNo;
                            return RedirectToAction("AddEdit");
                        }                        
                    }      
                    
                }
                else
                {
                    return RedirectToAction("Index");
                }                    
            }
        }
        public String SaveDtl(int UserNo,
            string Dept,
            String DtlString,
            String Status)
        {
            int MTransNo = 0;
            S1640Entities db = new S1640Entities();           
            string strText = DtlString;
            string[] arrayStock = strText.Split('~');
            foreach (string str in arrayStock)
            {
                string[] arrayValues = str.Split('^');
                try
                {
                    if (arrayValues[3]== "1")
                    {                               
                        if (Convert.ToInt32(arrayValues[2]) == 0)
                        {
                            UserAuthorization objD = new UserAuthorization();                           
                            objD.UserNo = UserNo;
                            //objD.Dept = Dept;
                            objD.ModuleNo = Convert.ToInt32(arrayValues[0]);
                            objD.AuthType = arrayValues[1]; 
                            db.UserAuthorizations.Add(objD);
                            db.SaveChanges();
                            MTransNo = objD.MTransNo;
                        }
                        else
                        {
                            MTransNo = Convert.ToInt32(arrayValues[2]);
                            UserAuthorization objD = db.UserAuthorizations.Where(x => x.MTransNo == MTransNo).FirstOrDefault();
                            objD.AuthType = arrayValues[1];
                            db.Entry(objD).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }
            }
            Session["MTransNo"] = MTransNo;
            return "" + MTransNo;
        }        
        public ActionResult DtlDelete(int MTransNo = 0)
        {
            S1640Entities db = new S1640Entities();
            UserAuthorization obj = db.UserAuthorizations.Where(x => x.MTransNo == MTransNo).FirstOrDefault();
            obj.MTransNo = MTransNo;
            db.UserAuthorizations.Remove(obj);
            db.SaveChanges();
            Session["MTransNo"] = obj.UserNo;
            return RedirectToAction("AddEdit");
        }
    }
}