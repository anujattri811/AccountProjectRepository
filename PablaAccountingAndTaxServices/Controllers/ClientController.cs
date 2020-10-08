using PablaAccountingAndTaxServices.CommanClass;
using PablaAccountingAndTaxServicesBLL;
using PablaAccountingAndTaxServicesDLL.DataAccess;
using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PablaAccountingAndTaxServices.Controllers
{
    public class ClientController : Controller
    {
        LoginBLL loginBLL = new LoginBLL();
        EncryDecry encryDecry = new EncryDecry();
        ClientBLL clientBLL = new ClientBLL();
        PablaAccountsEntities pablaAccountsEntities = new PablaAccountsEntities();
        CustomMethod customMethod = new CustomMethod();
        public ClientController()
        {
            var DocumentList = pablaAccountsEntities.tblDocumentTypes.Where(x => x.IsDeleted == false).Select(x => x.DocumentType).ToList();
            DocumentList.Add("Other");
            IEnumerable<SelectListItem> selectDocumentList = from Document in DocumentList
                                                             select new SelectListItem
                                                             {
                                                                 Text = Convert.ToString(Document),
                                                                 Value = Convert.ToString(Document)
                                                             };
            ViewBag.DocumentList = new SelectList(selectDocumentList, "Text", "Value");
        }

        #region client_login
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.IMG = "Home";
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("client_login", "Client");
        }
        public ActionResult clients_login()
        {
            return View();
        }
        public ActionResult client_login()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Client_Dashboard", "Client");
            }
            return View();
        }
        [HttpPost]
        public ActionResult client_login(tblUser tbluser)
        {
            var EncryPassword = encryDecry.EncryptPassword(tbluser.Password);
            var result = loginBLL.CheckClientLogin(tbluser.UserName, EncryPassword);
            if (result.FirstName == null && result.LastName == null)
            {
                ViewBag.Message = "You have entered incorrect Username and Password.";
                return View();
            }
            else
            {
                Session["UserId"] = result.UserId;
                Session["FirstName"] = result.FirstName;
                Session["LastName"] = result.LastName;
                Session["Email"] = result.Email;
                return RedirectToAction("Client_Dashboard", "Client");
            }
        }
        [HttpGet]
        public ActionResult client_forgotpassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult client_forgotpassword(string Email = "")
        {
            var Clientdata = pablaAccountsEntities.tblUsers.Where(x => x.Email == Email && x.IsDeleted == false).FirstOrDefault();
            if (Clientdata == null)
            {
                TempData["MSG"] = "This email does not exist. ";
                return RedirectToAction("client_forgotpassword");
            }
            else
            {
                var result = loginBLL.ForgetPassword(Email, 0);
                var Password = encryDecry.DecryptPassword(result.Password);
                SendForgetPasswordEmail(Convert.ToInt32(result.UserId), Email, result.FirstName, result.LastName, Password);
                TempData["MSG"] = " Your Username and Password has been sent successfully to your email.";
                return View();
            }

        }
        public bool SendForgetPasswordEmail(int UserId, string Email, string FirstName, string LastName, string Password)
        {
            try
            {
                string htmlBody = "";
                string headerText = "Hi <b> " + FirstName + " " + LastName + " ,</b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>You have requested to get your password this is your password below:-</br></br></td></tr>";
                emailText += "<tr><td>UserName:<b> " + Email + "</b></td></tr>";
                emailText += "<tr><td>Password:<b> " + Password + "</b></td></tr>";
                emailText += "<tr><td>Regards</td></tr>";
                emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                customMethod.SendEmail(Email, "Credential Information", htmlBody, "");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult client_dashboard(string PersonName = "", string SearchDocumentType = "", string SearchYear = "", string SearchMonthly="", int UserId = 0)
        {
            var model = new ClientEntity();
            if (Session["UserId"] == null)
            {
                return RedirectToAction("client_login", "Client");
            }
            int ClientId = Convert.ToInt32(Session["UserId"]);
            List<tblClientDocument> result = new List<tblClientDocument>();

            if (PersonName != "" && SearchDocumentType != "")
            {
                result = clientBLL.SearchDocumentByQuery(ClientId, PersonName, SearchDocumentType, SearchYear, SearchMonthly);
                ViewBag.Search = "1";
            }
            else
            {
                result = clientBLL.selectAllDocumentForClient(ClientId);
                ViewBag.Search = "0";
            }
            model = clientBLL.GetAllClient(ClientId);
            var PersonList = pablaAccountsEntities.tblClientDocuments.Where(x => x.UserId == ClientId && x.IsDeleted == false).Select(x => x.PersonName).Distinct().ToList();
            PersonList.Add("Other");
            IEnumerable<SelectListItem> selectPersonList = from Person in PersonList
                                                           select new SelectListItem
                                                           {

                                                               Text = Convert.ToString(Person),
                                                               Value = Convert.ToString(Person)
                                                           };
            ViewBag.PersonName = new SelectList(selectPersonList, "Text", "Value");
            var resultList = clientBLL.GetRequest(ClientId);
            foreach (var item in resultList)
            {
                item.CreatedOn = Convert.ToDateTime(item.CreatedOn).AddMinutes(-570);
                item.CreatedOn = Convert.ToDateTime(item.CreatedOn).AddDays(7);
            }
            var date = DateTime.Now;
            ViewBag.CurrentDays = date;
            ViewBag.Request = resultList;
            ViewBag.TotalDocument = result;

            return View(model);
        }
        [HttpPost]
        public ActionResult RequestDocumentByClient(int UserId = 0, string DocumentType = "", string Year = "", string PersonName = "", string Description = "", string OtherDocuments = "", string OtherPersonName = "", string PeriodTime = "", string Months = "")
        {
            if (PersonName == "Other")
            {
                PersonName = OtherPersonName;
            }
            clientBLL.RequestDocumentByClient(UserId, DocumentType, Year, PersonName, Description, OtherDocuments, Months, PeriodTime);
            SendRequestDocumentEmail(DocumentType, Year, PersonName, Description, OtherDocuments, PeriodTime, Months);
            return RedirectToAction("client_dashboard", "Client");
        }
        public bool SendRequestDocumentEmail(string DocumentType, string Year, string PersonName, string Description, string OtherDocuments, string PeriodTime, string Months)
        {
            try
            {
                string htmlBody = "";
                string headerText = "Hi,<b></b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>Someone has Requested a document. Here is the information given below:-</br></br></td></tr>";
                emailText += "<tr><td>DocumentType:<b> " + DocumentType + "</b></td></tr>";
                emailText += "<tr><td>Year:<b> " + Year + "</b></td></tr>";
                emailText += "<tr><td>Name:<b> " + PersonName + "</b></td></tr>";
                emailText += "<tr><td>Description:<b> " + Description + "</b></td></tr>";
                emailText += "<tr><td>OtherDocuments:<b> " + OtherDocuments + "</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                customMethod.SendEmail("Tax@pablastax.com", "Request Document", htmlBody, "");
                //MailMessage mailMessage = new MailMessage();
                //mailMessage.To.Add("sahilattri740@gmail.com");
                //mailMessage.From = new MailAddress("Websiteindia2020@gmail.com");
                //mailMessage.Subject = "Request Document";
                //mailMessage.IsBodyHtml = true;
                //mailMessage.Body = htmlBody;
                //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                //smtpClient.Port = 587;
                //smtpClient.Credentials = new System.Net.NetworkCredential()
                //{
                //    UserName = "Websiteindia2020@gmail.com",
                //    Password = "Sandeepanuj2020"
                //};
                //smtpClient.EnableSsl = false;
                //smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(string Name = "", string Email = "", string Phone = "", string Subject = "", string Message = "")
        {

            SendContactUsEmail(Name, Email, Phone, Subject, Message);
            TempData["Message"] = "Your Request has been Sumitted";
            return View();
        }
        public bool SendContactUsEmail(string Name, string Email, string Phone, string Subject, string Message)
        {
            try
            {
                string htmlBody = "";
                string headerText = "Hi,<b></b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>Someone has Contact You With The Detail:-</br></br></td></tr>";
                emailText += "<tr><td>Name:<b> " + Name + "</b></td></tr>";
                emailText += "<tr><td>Email:<b> " + Email + "</b></td></tr>";
                emailText += "<tr><td>Phone:<b> " + Phone + "</b></td></tr>";
                emailText += "<tr><td>Subject:<b> " + Subject + "</b></td></tr>";
                emailText += "<tr><td>Message:<b> " + Message + "</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                customMethod.SendEmail("Tax@pablastax.com", "Contact Us", htmlBody, "");
                //MailMessage mailMessage = new MailMessage();
                //mailMessage.To.Add("rs3551370@gmail.com");
                //mailMessage.From = new MailAddress("Websiteindia2020@gmail.com");
                //mailMessage.Subject = "Contact Us";
                //mailMessage.IsBodyHtml = true;
                //mailMessage.Body = htmlBody;
                //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                //smtpClient.Port = 587;
                //smtpClient.Credentials = new System.Net.NetworkCredential()
                //{
                //    UserName = "Websiteindia2020@gmail.com",
                //    Password = "Sandeepanuj2020"
                //};
                //smtpClient.EnableSsl = false;
                //smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public ActionResult RequestPhoneCall()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult RequestPhoneCall(string FirstName = "", string LastName = "", string Email = "", string Phone = "", string ServiceType = "", string Comment = "")
        //{
        //    string htmlBody = "";
        //    string headerText = "Hi,<b></b>";
        //    string startTable = "<table>";
        //    string emailText = "<tr><td><br/>Someone has Requested a Phone Call With The Detail:-</br></br></td></tr>";
        //    emailText += "<tr><td>FirstName:<b> " + FirstName + "</b></td></tr>";
        //    emailText += "<tr><td>LastName:<b> " + LastName + "</b></td></tr>";
        //    emailText += "<tr><td>Email:<b> " + Email + "</b></td></tr>";
        //    emailText += "<tr><td>Phone:<b> " + Phone + "</b></td></tr>";
        //    emailText += "<tr><td>ServiceType:<b> " + ServiceType + "</b></td></tr>";
        //    emailText += "<tr><td>Comment:<b> " + Comment + "</b></td></tr>";
        //    string endTable = "<br/></table> </br> </br> Thanks";
        //    htmlBody = headerText + startTable + emailText + endTable;
        //    //SendRequestForPhoneCall(FirstName, LastName, Email, Phone, ServiceType, Comment);
        //    bool status = customMethod.SendEmail("Sahilattri740@gmail.com", "Contact Us", htmlBody, "");
        //    return View();
        //}
        //public bool SendRequestForPhoneCall(string FirstName, string LastName, string Email, string Phone, string ServiceType, string Comment)
        //{
        //    try
        //    {

        //        MailMessage mailMessage = new MailMessage();
        //        mailMessage.To.Add("Sahilattri740@gmail.com");
        //        mailMessage.From = new MailAddress("Websiteindia2020@gmail.com");
        //        mailMessage.Subject = "Requested For a Phone Call";
        //        mailMessage.IsBodyHtml = true;
        //        mailMessage.Body = htmlBody;
        //        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        //        smtpClient.Port = 587;
        //        smtpClient.Credentials = new System.Net.NetworkCredential()
        //        {
        //            UserName = "Websiteindia2020@gmail.com",
        //            Password = "Sandeepanuj2020"
        //        };
        //        smtpClient.EnableSsl = true;
        //        smtpClient.Send(mailMessage);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult DeleteRequest(int UserId)
        {
            clientBLL.DeleteRequest(UserId);
            TempData["Delete"] = "1";
            return RedirectToAction("client_dashboard");
        }
        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult services()
        {
            return View();
        }
        public ActionResult FilePersonalTax()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FilePersonalTax(FilePersonalTaxEntity filePersonalTaxEntity, string NewClient, string ReturningClient)
        {
            if (NewClient == "true")
            {
                filePersonalTaxEntity.IsExiting = false;
            }
            else
            {
                filePersonalTaxEntity.IsExiting = true;
            }
            clientBLL.SaveFilePersonalTax(filePersonalTaxEntity);
            bool status = SendFilePersonalTax(filePersonalTaxEntity.FirstName, filePersonalTaxEntity.LastName, filePersonalTaxEntity.Email, filePersonalTaxEntity.Phone, filePersonalTaxEntity.SIN, filePersonalTaxEntity.DateOfBirth, filePersonalTaxEntity.MaritalStatus, filePersonalTaxEntity.Sex, filePersonalTaxEntity.CurrentAddress, filePersonalTaxEntity.City, filePersonalTaxEntity.Province, filePersonalTaxEntity.PostalCode, filePersonalTaxEntity.SpouseFirstName, filePersonalTaxEntity.SpouseMiddleName, filePersonalTaxEntity.SpouseLastName, filePersonalTaxEntity.SpouseDateOfBirth, filePersonalTaxEntity.SpouseSIN, filePersonalTaxEntity.Children1Name, filePersonalTaxEntity.Children1DateOfBirth, filePersonalTaxEntity.Children2Name, filePersonalTaxEntity.Children2DateOfBirth, filePersonalTaxEntity.Children3Name, filePersonalTaxEntity.Children3DateOfBirth, filePersonalTaxEntity.Entrydatetime, filePersonalTaxEntity.Entrydatetime1);
            if (status == true)
            {
                TempData["Success"] = "Your Data is Submitted Successfully. We will get back to you soon.";
            }
            else
            {
                TempData["Error"] = "Something went wrong while submitting. Please try after some time.";
            }

            return RedirectToAction("FilePersonalTax");
        }
        public bool SendFilePersonalTax(string FirstName, string LastName, string Email, string Phone, string SIN, string DateOfBirth, string MaritalStatus, string Sex, string CurrentAddress, string City, string Province, string PostalCode, string SpouseFirstName, string SpouseMiddleName, string SpouseLastName, string SpouseDateOfBirth, string SpouseSIN, string Children1Name, string Children1DateOfBirth, string Children2Name, string Children2DateOfBirth, string Children3Name, string Children3DateOfBirth, string Entrydatetime, string Entrydatetime1)
        {
            try
            {
                string htmlBody = "";
                string headerText = "Hi,<b></b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>Someone has filed a Tax Online. Here is the information below:-</br></br></td></tr>";
                emailText += "<tr><td>First Name:<b> " + FirstName + "</b></td></tr>";
                emailText += "<tr><td>Last Name:<b> " + LastName + "</b></td></tr>";
                emailText += "<tr><td>Email:<b> " + Email + "</b></td></tr>";
                emailText += "<tr><td>SIN:<b> " + SIN + "</b></td></tr>";
                emailText += "<tr><td>Date Of Birth:<b> " + DateOfBirth + "</b></td></tr>";
                emailText += "<tr><td>Marital Status:<b> " + MaritalStatus + "</b></td></tr>";
                emailText += "<tr><td>Sex:<b> " + Sex + "</b></td></tr>";
                emailText += "<tr><td>Current Address:<b> " + CurrentAddress + "</b></td></tr>";
                emailText += "<tr><td>City:<b> " + City + "</b></td></tr>";
                emailText += "<tr><td>Province:<b> " + Province + "</b></td></tr>";
                emailText += "<tr><td>PostalCode:<b> " + PostalCode + "</b></td></tr>";
                emailText += "<tr><td>Spouse First Name:<b> " + SpouseFirstName + "</b></td></tr>";
                emailText += "<tr><td>Spouse Middle Name:<b> " + SpouseMiddleName + "</b></td></tr>";
                emailText += "<tr><td>Spouse Last Name:<b> " + SpouseLastName + "</b></td></tr>";
                emailText += "<tr><td>Spouse Date Of Birth:<b> " + SpouseDateOfBirth + "</b></td></tr>";
                emailText += "<tr><td>Spouse SIN:<b> " + SpouseSIN + "</b></td></tr>";
                emailText += "<tr><td>Children1 Name:<b> " + Children1Name + "</b></td></tr>";
                emailText += "<tr><td>Children1 DateOfBirth:<b> " + Children1DateOfBirth + "</b></td></tr>";
                emailText += "<tr><td>Children2 Name:<b> " + Children2Name + "</b></td></tr>";
                emailText += "<tr><td>Children2 DateOfBirth:<b> " + Children2DateOfBirth + "</b></td></tr>";
                emailText += "<tr><td>Children3 Name:<b> " + Children3Name + "</b></td></tr>";
                emailText += "<tr><td>Children3 DateOfBirth:<b> " + Children3DateOfBirth + "</b></td></tr>";
                emailText += "<tr><td>Children3 DateOfBirth:<b> " + Entrydatetime + "</b></td></tr>";
                emailText += "<tr><td>Children3 DateOfBirth:<b> " + Entrydatetime1 + "</b></td></tr>";
                emailText += "<tr><td>Phone:<b> " + Phone + "</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                customMethod.SendEmail("Tax@pablastax.com", "File Personal Tax", htmlBody, "");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet]
        public ActionResult update_clientinfo(int ClientId = 0)
        {
            var model = new ClientEntity();
            model = clientBLL.GetAllClient(ClientId);
            ViewBag.userid = model.UserId;
            return View(model);
        }
        [HttpPost]
        public ActionResult update_clientinfo(ClientEntity clientEntity)
        {
            clientBLL.UpdateClient(clientEntity);
            return RedirectToAction("client_dashboard");
        }
        [HttpGet]
        public ActionResult client_changepassword(int UserId = 0)
        {
            ViewBag.Email = Session["Email"];
            ViewBag.UserId = UserId;
            return View();
        }

        [HttpPost]
        public ActionResult client_changepassword(int UserId = 0, string Password = "", string ConfirmPassword = "")
        {
            var EncryPassword = encryDecry.EncryptPassword(Password);
            var EncryConfirmPassword = encryDecry.EncryptPassword(ConfirmPassword);
            clientBLL.UpdateClientPassword(UserId, EncryPassword, EncryConfirmPassword);
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("client_login");
        }


    }
}