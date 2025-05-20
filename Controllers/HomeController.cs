using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S1640.Models;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.Mail;

namespace S1640.Controllers
{
    public class SetSessionController : Controller
    {
        public ActionResult SetVariable(string key, string value)
        {
            Session[key] = value;
            return this.Json(new { success = true });
        }
    }
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            S1640Entities conn = new S1640Entities();
            if (Session["Userid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");              
            }
        }
        public ActionResult LogOut()
        {
            S1640Entities conn = new S1640Entities();
            int userid = Convert.ToInt32(Session["Userid"]);
            var userlog = conn.UserLogDatas.Where(x => x.CreatedBy == userid).FirstOrDefault();
            if (userlog != null)
            {
                conn.UserLogDatas.Remove(userlog);
                conn.SaveChanges();
            }
            Session.Remove("Userid");
            Session.Remove("SubscID");
            Session.Remove("CompNo");
            Session.Remove("CompName");
            Session.Remove("CStateCode");
            Session.Remove("UserType");
            Session.Remove("CYear");
            Session.Remove("CompCode");
            Session.Remove("TimeZone");
            Session.Remove("CCurrency");
            Session.Remove("BranchType");
            return RedirectToAction("Login");
        }
        public ActionResult Login()
            {
            Session.Remove("Userid");
            Session.Remove("SubscID");
            Session.Remove("CompNo");
            Session.Remove("CompName");
            Session.Remove("CStateCode");
            Session.Remove("UserType");
            Session.Remove("CYear");
            Session.Remove("CompCode");
            Session.Remove("TimeZone");
            Session.Remove("CCurrency");
            Session.Remove("BranchType");
            ViewBag.wrongLogin = TempData["wrongLogin"] == null ? "" : TempData["wrongLogin"] as string;
            UserMaster User = new UserMaster();
            S1640Entities conn = new S1640Entities();
            return View(User);
        }
        public ActionResult ErrorMessage()
        {
            return View();
        }
        public ActionResult Forgot(int userno = 0)
        {
            if (userno != 0)
            {
                // Set the "Userid" session variable using MTransNo
                Session["Userid"] = userno;

                // Call the ChangePassword method and pass the user object

                if (Session["Userid"] != null)
                {

                    return PasswordReset();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                // Handle the case when the user is not found
                return View(); // Redirect to a specific action or handle appropriately
            }
        }
        public ActionResult GenerateCode(String EmailId)
        {
            S1640Entities conn = new S1640Entities();
            var Check_mail = (conn.UserMasters.Where(x => x.EmailId == EmailId && x.DeleteStatus == "N").OrderBy(x => x.EmailId).FirstOrDefault());
            if (Check_mail == null)
            {
                // Return a JSON object with the property name "random_number"
                return Json(new { random_number = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int min = 0;
                int max = 9999;

                Random rdm = new Random();
                int randomNumber = rdm.Next(min, max);

                // Format the random number as a string with leading zeros
                string formattedNumber = randomNumber.ToString("D4");
                var email = Check_mail.EmailId;
                var user = Check_mail.MTransNo.ToString();
                SendEmail(email, formattedNumber);
                // Return a JSON object with the property name "random_number"
                return Json(new { random_number = formattedNumber, EmailI = email, userno = user }, JsonRequestBehavior.AllowGet);
            }
        }
        private string SendEmail(string toEmail, string code)
        {
            try
            {
                // Use your own SMTP server settings
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "ims2000.v2@gmail.com";
                string smtpPassword = "xmiaxtnxcubimehm";

                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    // Create the email message
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress("cfspatil543@gmail.com"),
                        Subject = "Generated Code",
                        Body = $"Your generated code is: {code}"
                    };

                    // Add recipient
                    mailMessage.To.Add(toEmail);

                    smtpClient.Send(mailMessage);

                    // Send the email
                    return "success";
                }
            }
            catch (SmtpException smtpEx)
            {
                // Handle SMTP-specific exceptions
                string errorMessage = "SMTP Error occurred. ";

                // Include details from the exception
                errorMessage += $"Error code: {smtpEx.StatusCode}, ";
                errorMessage += $"Error message: {smtpEx.Message}, ";
                errorMessage += $"Inner exception: {smtpEx.InnerException?.Message}";
                return errorMessage;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return "Error occurred. Error details: " + ex.Message;
            }
        }

        public ActionResult PasswordReset(int userno = 0)
        {
            if (userno != 0)
            {
                // Set the "Userid" session variable using MTransNo
                Session["Userid"] = userno;

                // Call the ChangePassword method and pass the user object

                if (Session["Userid"] != null)
                {

                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else if (Session["Userid"] != null)
            {
                return View();
            }
            else
            {
                // Handle the case when the user is not found
                return RedirectToAction("Login", "Home"); // Redirect to a specific action or handle appropriately
            }
        }
        public ActionResult ChangePassword()
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
        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        [HttpPost]
        public ActionResult LoginCheck(UserMaster User)
        {
            S1640Entities conn = new S1640Entities();
            var userExists = conn.UserMasters.Where(x => x.UserId == User.UserId && x.LockStatus == "Y" && x.DeleteStatus == "N").FirstOrDefault();
            if (userExists != null)
            {
                string ip = Request.UserHostAddress;
                //var userlog = conn.UserLogDatas.Where(x => x.CreatedBy == userExists.MTransNo).FirstOrDefault();
                //if (userlog != null)
                //{
                //    if (ip != userlog.ClientIp)
                //    {
                //        TempData["wrongLogin"] = "This User is Already Logged In..!";
                //        return RedirectToAction("Login");
                //    }
                //}
                //else
                //{
                    UserLogData obj = new UserLogData();
                    obj.ClientIp = ip;
                    obj.LatLong = "";
                    obj.Location = "";
                    obj.CreatedBy = Convert.ToByte(userExists.MTransNo);
                    obj.UserType = userExists.UserType;
                    obj.DeleteStatus = "N";
                    obj.CreatedOn = DateTime.Now.Date;
                    conn.UserLogDatas.Add(obj);
                    conn.SaveChanges();
                //}
                string encVa = Encrypt(User.PW, "mskl-1sv4-xklq23");
                string dec = Decrypt(encVa, "mskl-1sv4-xklq23");
                if (userExists.PW.Equals(encVa))
                {
                    Session["Userid"] = userExists.MTransNo;
                    Session["UserType"] = userExists.UserType;
                    Session["SubscID"] = userExists.SubscID;
                    var Company = conn.Companies.FirstOrDefault();
                    Session["CompNo"] = Company.MTransNo;
                    Session["CompName"] = Company.CompName;
                    Session["CStateCode"] = Company.StateCode;
                    Session["CompCode"] = Company.CompCode;
                    Session["CYear"] = 2324;

                    //new code
                    var FYear = conn.Companies.Where(x => x.DeleteStatus == "N" && x.LockStatus == "Y").FirstOrDefault();

                    DateTime FromDate =Convert.ToDateTime(FYear.FromYear);
                    DateTime ToYaer = Convert.ToDateTime(FYear.ToYear);

                    int currentYear = FromDate.Year;
                    int NextYear = ToYaer.Year;

                    int lastTwoDigits = currentYear % 100;
                    int nextYearLastTwoDigits = NextYear % 100;

                    int financialYearRepresentation = int.Parse($"{lastTwoDigits:D2}{nextYearLastTwoDigits:D2}");

                    Session["FYear"] = financialYearRepresentation;

                    //new code

                    var user1 = conn.UserMasters.Where(x => x.MTransNo == userExists.MTransNo && x.UserType == "ADMIN").FirstOrDefault();
                    if (user1 != null)
                    {
                        if (user1.loginDate == null)
                        {
                            user1.loginDate = DateTime.Now.AddDays(-1);
                        }
                        if (user1.loginDate.Value.Date != DateTime.Now.Date)
                        {
                            //DAtabase backup
                            //util.BackUp();
                            //util.BackUp1();
                            //sendMail();
                        }
                    }
                    userExists.loginDate = DateTime.Now;
                    conn.Entry(userExists).State = System.Data.Entity.EntityState.Modified;
                    conn.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["wrongLogin"] = "Login id / Password does not match";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["wrongLogin"] = "login ID / Password does not match";
                return RedirectToAction("Login");
            }
        }
        public String UpdatePass(String[] DtlString)
        {
            DateTime TodayDate = DateTime.Now;
            S1640Entities db = new S1640Entities();
            string pwd = DtlString[0];
            Int32 mt = Convert.ToInt32(DtlString[1]);
            UserMaster obj = db.UserMasters.Where(x => x.MTransNo == mt).FirstOrDefault();
            string encVa = Encrypt(pwd, "mskl-1sv4-xklq23");
            string dec = Decrypt(encVa, "mskl-1sv4-xklq23");

            if (pwd == dec)
            {
                obj.PW = encVa;
                obj.ModifiedOn = TodayDate;
                Convert.ToByte(Session["Userid"]);
                db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return "Success";
            }
            else
            {
                return "Error";
            }
        }
       
        public string UpdateUserLog(String DtlString)
        {
            DateTime TodayDate = DateTime.Now;
            S1640Entities db = new S1640Entities();
            byte usId = 0;
            usId = Convert.ToByte(Session["Userid"]);
            DateTime dt = DateTime.Today;
            UserLogData usr = db.UserLogDatas.Where(x => x.CreatedBy == usId && x.CreatedOn > dt).FirstOrDefault();
            if (usr == null)
            {
                UserLogData obj = new UserLogData();
                string[] st = DtlString.Split('#');
                obj.ClientIp = st[0];
                obj.LatLong = st[1];
                obj.Location = st[2];
                obj.CreatedBy = usId;
                obj.UserType = Convert.ToString(Session["UserType"]);
                obj.DeleteStatus = "N";
                obj.CreatedOn = TodayDate;
                db.UserLogDatas.Add(obj);
                db.SaveChanges();
                return "saved";
            }
            else if (!usr.LatLong.Contains('~') || usr.ClientIp == "")
            {
                string[] st = DtlString.Split('#');
                usr.LatLong = st[0];
                usr.LatLong = st[1];
                usr.Location = st[2];
                db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return "updated";
            }
            else
            {
                return "present";
            }
        }
        public String SaveMsgDtl(
            string Messages,
            int Receiver)
        {
            S1640Entities db = new S1640Entities();
            EMSMsg obj = new EMSMsg();
            obj.Messages = Messages;
            obj.Sender = Convert.ToInt32(Session["Userid"]);
            obj.Receiver = Receiver;
            obj.MsgStatus = "UNRead";
            obj.SendOn = DateTime.Now;
            db.EMSMsgs.Add(obj);
            db.SaveChanges();
            return "" + obj.Receiver;
        }

        public String SaveMsgData(int MtraNo)
        {
            S1640Entities db = new S1640Entities();
            var obj = db.EMSMsgs.Where(x=>x.MTransNo == MtraNo).FirstOrDefault();
            obj.AckStatus = "Y";
            obj.MsgStatus = "Read";
            obj.AckDate = DateTime.Now;
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return "" + obj.Receiver;
        }
        public int GetmsgCount(int UserNo)
        {
            S1640Entities db = new S1640Entities();
            int msgcount = 0;
            var UserLisr = (from M in db.EMSMsgs where M.Receiver == UserNo && M.MsgStatus == "UNRead" select M.MTransNo).ToList();
            if (UserLisr != null)
            {
                if (UserLisr.Count > 0)
                {
                    msgcount = UserLisr.Count;
                }
                else
                {
                    msgcount = 0;
                }

            }
            else
            {
                msgcount = 0;
            }
            return msgcount;
        }
        public String GetMenu(String DtlString)
        {
            S1640Entities db = new S1640Entities();

            byte usId = Convert.ToByte(Session["Userid"]);

            string usrTy = Convert.ToString(Session["UserType"]);

            var userAuth = (from MO in db.Menulists
                            select new
                            {
                                ModuleName = MO.ModuleName,
                                ModuleLink = MO.ModuleLink,
                                ModulePage = MO.ModulePage
                            }).OrderBy(s => s.ModuleName).ToList();

            if (usrTy != "ADMIN")
            {
                userAuth = (from UA in db.UserAuthorizations
                            join MO in db.Menulists
                            on UA.ModuleNo equals MO.MTransNo
                            where UA.UserNo == usId
                            select new
                            {
                                ModuleName = MO.ModuleName,
                                ModuleLink = MO.ModuleLink,
                                ModulePage = MO.ModulePage
                            }).OrderBy(s => s.ModuleName).ToList();
            }
            string res = "";
            for (int i = 0; i < userAuth.Count; i++)
            {
                res += userAuth[i].ModuleName + "~" + userAuth[i].ModuleLink + "~" + userAuth[i].ModulePage + "$";
            }
            return res;
        }
    }
}