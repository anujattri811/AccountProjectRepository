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
            var result= loginBLL.ForgetPassword(Email,0);
           var Password = encryDecry.DecryptPassword(result.Password);
            SendForgetPasswordEmail(Convert.ToInt32(result.UserId),Email,result.FirstName, result.LastName, Password);
            return View();
        }
        public bool SendForgetPasswordEmail(int UserId,string Email,string FirstName, string LastName, string Password)
        {
            try
            {
                string htmlBody = "";
                string headerText = "Hi <b> " + FirstName + " " + LastName + " ,</b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>You have requested to get your password this is your password below:-</br></br></td></tr>";
                emailText += "<tr><td>UserName:<b> " + Email + "</b></td></tr>";
                emailText += "<tr><td>Password:<b> " + Password + "</b></td></tr>";
                emailText += "<tr><td> You can Change Password from below link:-</td></tr>";
                emailText += "<tr><td><b>https://pablaaccounts.globalroot.net/Client/client_changepassword?UserId=" + UserId + "</b></td></tr>";
                emailText += "<tr><td>Regards</td></tr>";
                emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(Email);
                mailMessage.From = new MailAddress("Websiteindia2020@gmail.com");
                mailMessage.Subject = "Credential Information";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = htmlBody;
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "Websiteindia2020@gmail.com",
                    Password = "Sandeepanuj2020"
                };
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult client_dashboard()
        {
            var model = new ClientEntity();
            if (Session["UserId"] == null)
            {
                return RedirectToAction("client_login", "Client");
            }
            int ClientId = Convert.ToInt32(Session["UserId"]);
            List<tblClientDocument> result = new List<tblClientDocument>();
            result = clientBLL.selectAllDocumentForClient(ClientId);
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
                item.CreatedOn= Convert.ToDateTime(item.CreatedOn).AddMinutes(-570);
                item.CreatedOn = Convert.ToDateTime(item.CreatedOn).AddDays(7);
            }
            var date = DateTime.Now;
            ViewBag.CurrentDays = date;
            ViewBag.Request = resultList;
            ViewBag.TotalDocument = result;

            return View(model);
        }
        [HttpPost]
        public  ActionResult RequestDocumentByClient(int UserId = 0,string DocumentType="", string Year="", string PersonName="", string Description="",string OtherDocuments= "",string OtherPersonName="")
        {
            if(PersonName == "Other")
            {
                PersonName = OtherPersonName;
            }
            clientBLL.RequestDocumentByClient(UserId, DocumentType,Year,PersonName,Description, OtherDocuments);
            return RedirectToAction("client_dashboard","Client");
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
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add("Sahilattri740@gmail.com");
                mailMessage.From = new MailAddress("Websiteindia2020@gmail.com");
                mailMessage.Subject = "Contact Us";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = htmlBody;
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "Websiteindia2020@gmail.com",
                    Password = "Sandeepanuj2020"
                };
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpGet]
        public ActionResult RequestPhoneCall()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RequestPhoneCall(string FirstName = "", string LastName = "", string Email = "", string Phone = "", string ServiceType = "", string Comment = "")
        {
            SendRequestForPhoneCall(FirstName, LastName, Email, Phone, ServiceType, Comment);
            return View();
        }
        public bool SendRequestForPhoneCall(string FirstName, string LastName, string Email, string Phone, string ServiceType, string Comment)
        {
            try
            {
                string htmlBody = "";
                string headerText = "Hi,<b></b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>Someone has Requested a Phone Call With The Detail:-</br></br></td></tr>";
                emailText += "<tr><td>FirstName:<b> " + FirstName + "</b></td></tr>";
                emailText += "<tr><td>LastName:<b> " + LastName + "</b></td></tr>";
                emailText += "<tr><td>Email:<b> " + Email + "</b></td></tr>";
                emailText += "<tr><td>Phone:<b> " + Phone + "</b></td></tr>";
                emailText += "<tr><td>ServiceType:<b> " + ServiceType + "</b></td></tr>";
                emailText += "<tr><td>Comment:<b> " + Comment + "</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add("Sahilattri740@gmail.com");
                mailMessage.From = new MailAddress("Websiteindia2020@gmail.com");
                mailMessage.Subject = "Requested For a Phone Call";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = htmlBody;
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "Websiteindia2020@gmail.com",
                    Password = "Sandeepanuj2020"
                };
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

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
        public ActionResult FilePersonalTax(FilePersonalTaxEntity filePersonalTaxEntity)
        {
            clientBLL.SaveFilePersonalTax(filePersonalTaxEntity);
            SendFilePersonalTax(filePersonalTaxEntity.FirstName,filePersonalTaxEntity.LastName,filePersonalTaxEntity.Email,filePersonalTaxEntity.Phone);
            TempData["Success"] = "Your Data is Submitted Successfully";
            return RedirectToAction("FilePersonalTax");
        }
        public bool SendFilePersonalTax(string FirstName, string LastName, string Email, string Phone)
        {
            try
            {
                string htmlBody = "";
                string headerText = "Hi,<b></b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>Your Data is Submitted Successfully:-</br></br></td></tr>";
                emailText += "<tr><td>FirstName:<b> " + FirstName + "</b></td></tr>";
                emailText += "<tr><td>LastName:<b> " + LastName + "</b></td></tr>";
                emailText += "<tr><td>Email:<b> " + Email + "</b></td></tr>";
                emailText += "<tr><td>Phone:<b> " + Phone + "</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add("rs3551370@gmail.com");
                mailMessage.From = new MailAddress("Websiteindia2020@gmail.com");
                mailMessage.Subject = "File Personal Tax";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = htmlBody;
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "Websiteindia2020@gmail.com",
                    Password = "Sandeepanuj2020"
                };
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch
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
            ViewBag.UserId =UserId;
            return View();
        }

        [HttpPost]
        public ActionResult client_changepassword(int UserId=0, string Password= "", string ConfirmPassword= "")
        {
            var EncryPassword = encryDecry.EncryptPassword(Password);
            var EncryConfirmPassword = encryDecry.EncryptPassword(ConfirmPassword);
            clientBLL.UpdateClientPassword(UserId, EncryPassword, EncryConfirmPassword);
            return RedirectToAction("client_login");
        }


    }
}