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

        #region client_login
        [HttpGet]
        public ActionResult Index()
        {
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
            ViewBag.TotalDocument = result;

            return View(model);
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
    }
}