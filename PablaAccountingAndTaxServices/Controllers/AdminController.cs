﻿using PablaAccountingAndTaxServices.CommanClass;
using PablaAccountingAndTaxServicesBLL;
using PablaAccountingAndTaxServicesDLL.DataAccess;
using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace PablaAccountingAndTaxServices.Controllers
{
    public class AdminController : Controller
    {
        ClientEntity clientEntity = new ClientEntity();
        LoginBLL loginBLL = new LoginBLL();
        ClientBLL clientBLL = new ClientBLL();
        EncryDecry encryDecry = new EncryDecry();

        PablaAccountsEntities pablaAccountsEntities = new PablaAccountsEntities();

        #region Admin Login

        [HttpGet]
        public ActionResult admin_login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult admin_login(string UserName, string Password)
        {
            var EncryPassword = encryDecry.EncryptPassword(Password);
            var result = loginBLL.CheckLogin(UserName, EncryPassword);
            if (result.FirstName == null && result.LastName == null)
            {
                ViewBag.Message = "You have entered incorrect Username and Password.";
                return RedirectToAction("admin_login");
            }
            else
            {
                Session["FirstName"] = result.FirstName;
                Session["LastName"] = result.LastName;
                return RedirectToAction("client");
            }

        }
        [HttpGet]
        public ActionResult Admin_forgotpassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Admin_forgotpassword(string Email = "")
        {
            var result = loginBLL.ForgetPassword(Email, 1);
            var Password = encryDecry.DecryptPassword(result.Password);
            SendForgetPasswordEmail(Email, result.FirstName, result.LastName, Password);
            return View();
        }
        public bool SendForgetPasswordEmail(string Email, string FirstName, string LastName, string Password)
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
        [HttpGet]

        #endregion 
        public ActionResult admin_dashboard()
        {
            return View();
        }
        [HttpGet]
        public ActionResult client()
        {
            if (Session["FirstName"] == null && Session["LastName"] == null)
            {
                return RedirectToAction("admin_login");
            }
            else
            {
                var result = clientBLL.selectClientList();
                ViewBag.ClientList = result;
            }
            return View();
        }
        [HttpGet]
        public ActionResult new_client()
        {
            if (Session["FirstName"] == null && Session["LastName"] == null)
            {
                return RedirectToAction("admin_login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult new_client(ClientEntity clientEntity)
        {
            if (Session["FirstName"] == null && Session["LastName"] == null)
            {
                return RedirectToAction("admin_login");
            }
            else
            {
                //Random r = new Random();
                //int rInt = r.Next(0, 10);
                //var Password = clientEntity.FirstName.Substring(0, 4) + clientEntity.MobileNo.Substring(clientEntity.MobileNo.Length - 4) + "@"+ rInt;
                //var EncryPassword = encryDecry.EncryptPassword(Password);
                //clientEntity.Password = EncryPassword;
                clientBLL.AddNewClient(clientEntity);
            }
            return RedirectToAction("client");
        }

        public ActionResult GeneratePassword(int ClientId = 0, string Email = "", string FirstName = "", string LastName = "", string MobileNo = "")
        {
            Random r = new Random();
            int rInt = r.Next(0, 10);
            var Pass = FirstName.Substring(0, 4) + MobileNo.Substring(MobileNo.Length - 4) + "@" + rInt;
            var EncryPassword = encryDecry.EncryptPassword(Pass);
            var Decrypt = encryDecry.DecryptPassword(EncryPassword);
            clientBLL.UpdateCredential(ClientId, Email, EncryPassword);
            SendCredential(ClientId, Email, FirstName, LastName, Decrypt);
            return RedirectToAction("client_view", new { ClientId = ClientId });
        }
        [HttpGet]
        public ActionResult modify_client(int ClientId = 0)
        {
            var model = new ClientEntity();
            if (Session["FirstName"] == null && Session["LastName"] == null)
            {
                return RedirectToAction("admin_login");
            }
            else
            {
                model = clientBLL.GetAllClient(ClientId);
                ViewBag.userid = model.UserId;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult modify_client(ClientEntity clientEntity)
        {

            if (Session["FirstName"] == null && Session["LastName"] == null)
            {
                return RedirectToAction("admin_login");
            }
            else
            {
                clientBLL.UpdateClient(clientEntity);
            }
            return RedirectToAction("client");
        }
        [HttpGet]
        public ActionResult client_view(int ClientId = 0, string PersonName = "", string DocumentType = "", string Year = "", int UserId = 0)
        {
            var model = new ClientEntity();
            if (Session["FirstName"] == null && Session["LastName"] == null)
            {
                return RedirectToAction("admin_login");
            }
            else
            {
                List<tblClientDocument> result = new List<tblClientDocument>();
                if (PersonName == "" && DocumentType == "" && Year == "")
                {
                    result = clientBLL.selectAllDocumentForClient(ClientId);
                }
                else
                {
                    result = clientBLL.SearchDocumentByQuery(ClientId, PersonName, DocumentType, Year);
                }

                model = clientBLL.GetAllClient(ClientId);
                model.Password = encryDecry.DecryptPassword(model.Password);
                var PersonList = pablaAccountsEntities.tblClientDocuments.Where(x => x.UserId == ClientId && x.IsDeleted == false).Select(x => x.PersonName).ToList();


                IEnumerable<SelectListItem> selectPersonList = from Person in PersonList
                                                               select new SelectListItem
                                                               {
                                                                   Text = Person.ToString(),
                                                                   Value = Person.ToString()
                                                               };
                ViewBag.PersonName = new SelectList(selectPersonList, "Text", "Value");
                ViewBag.userid = model.UserId;
                ViewBag.ClientId = ClientId;
                ViewBag.TotalDocument = result;
            }
            return View(model);
        }


        public ActionResult DeleteClient(int UserId)
        {
            clientBLL.DeleteClient(UserId);
            TempData["Delete"] = "1";
            return RedirectToAction("client");
        }
        [HttpPost]
        public ActionResult savedocuments(FileUploadEntity fileUploadEntity)
        {
            ClientBLL clientBLL = new ClientBLL();
            string path = Server.MapPath("~/Documents/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string DocumentName = Path.GetFileNameWithoutExtension(fileUploadEntity.UploadFile.FileName);
            string Extention = Path.GetExtension(fileUploadEntity.UploadFile.FileName).Replace(".", "");
            string Ext = Path.GetExtension(fileUploadEntity.UploadFile.FileName);
            string fileName = DocumentName + Ext;
            fileUploadEntity.UploadFile.SaveAs(path + fileName);
            fileUploadEntity.DocumentName = DocumentName;
            fileUploadEntity.Extension = Extention;
            clientBLL.Savedocuments(fileUploadEntity);

            return RedirectToAction("client_view", new { ClientId = fileUploadEntity.UserId });
        }
        public bool SendCredential(int UserId, string UserName, string FirstName, string LastName, string Password)
        {
            try
            {
                string htmlBody = "";
                string headerText = "Hi <b> " + FirstName + " " + LastName + " ,</b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>You account has been successfully created with our company. Your credentials are given as:-</br></br></td></tr>";
                //emailText += "<tr><td>Name:<b> "  "</b></td></tr>";
                emailText += "<tr><td>UserName:<b> " + UserName + "</b></td></tr>";
                emailText += "<tr><td>Password:<b> " + Password + "</b></td></tr>";
                emailText += "<tr><td> You can login with above credentials from below link:-</td></tr>";
                emailText += "<tr><td><b>http://pablaaccounts.globalroot.net/Client/client_login </b></td></tr>";
                emailText += "<tr><td>Regards</td></tr>";
                emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(UserName);
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
        public ActionResult DownloadFile(string Filename = "")
        {
            string filename = Filename;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "Documents\\" + filename;
            //byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            //string contentType = MimeMapping.GetMimeMapping(filepath);

            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = filename,
            //    Inline = true,
            //};

            //Response.AppendHeader("Content-Disposition", cd.ToString());

            //return File(filedata, contentType);
            return File(filepath, MediaTypeNames.Text.Plain, Filename);
        }

        public ActionResult FindClient()
        {

            return View();
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("admin_login");
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult requested_document()
        {
            List<tbl_RequestedDocument> rd = pablaAccountsEntities.tbl_RequestedDocument.Where(x => x.IsDeleted == false).ToList();
            return View(rd);
        }
        public ActionResult approve_document(int requestedDocumentId)
        {
            //tbl_RequestedDocument rd = pablaAccountsEntities.tbl_RequestedDocument.Where(x => x.RequestDocumentId).ToList();
            return RedirectToAction("requested_document", "admin");
        }
    }
}