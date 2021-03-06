﻿using PablaAccountingAndTaxServices.CommanClass;
using PablaAccountingAndTaxServicesBLL;
using PablaAccountingAndTaxServicesDLL.DataAccess;
using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PablaAccountingAndTaxServices.Controllers
{
    public class AdminController : Controller
    {
        ClientEntity clientEntity = new ClientEntity();
        LoginBLL loginBLL = new LoginBLL();
        ClientBLL clientBLL = new ClientBLL();
        EncryDecry encryDecry = new EncryDecry();
        CustomMethod customMethod = new CustomMethod();

        PablaAccountsEntities pablaAccountsEntities = new PablaAccountsEntities();
        public AdminController()
        {
            var DocumentList = pablaAccountsEntities.tblDocumentTypes.Where(x => x.IsDeleted == false).Select(x => x.DocumentType).ToList();
            var SearchDocumentList = pablaAccountsEntities.tblDocumentTypes.Where(x => x.IsDeleted == false).Select(x => x.DocumentType).ToList();
            DocumentList.Add("Other");
            IEnumerable<SelectListItem> selectDocumentList = from Document in DocumentList
                                                             select new SelectListItem
                                                             {
                                                                 Text = Convert.ToString(Document),
                                                                 Value = Convert.ToString(Document)
                                                             };
            IEnumerable<SelectListItem> selectSearchDocumentList = from Documents in SearchDocumentList
                                                                   select new SelectListItem
                                                                   {
                                                                       Text = Convert.ToString(Documents),
                                                                       Value = Convert.ToString(Documents)
                                                                   };
            ViewBag.DocumentList = new SelectList(selectDocumentList, "Text", "Value");
            ViewBag.SearchDocumentList = new SelectList(selectSearchDocumentList, "Text", "Value");
        }

        #region Admin Login

        [HttpGet]
        public ActionResult admin_login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = Convert.ToString(TempData["Message"]);
            }
            return View();
        }
        [HttpPost]
        public ActionResult admin_login(string UserName, string Password)
        {
            var EncryPassword = encryDecry.EncryptPassword(Password);
            var result = loginBLL.CheckLogin(UserName, EncryPassword);
            if (result.FirstName == null && result.LastName == null)
            {
                TempData["Message"] = "You have entered incorrect Username and Password.";
                return RedirectToAction("admin_login");
            }
            else
            {
                Session["FirstName"] = result.FirstName;
                Session["LastName"] = result.LastName;
                Session["AdminUserId"] = result.UserId;
                return RedirectToAction("admin_dashboard");
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
            var Clientdata = pablaAccountsEntities.tblUsers.Where(x => x.Email == Email && x.IsDeleted == false).FirstOrDefault();
            if (Clientdata == null)
            {
                TempData["Msg"] = "This email does not exist. ";
                return RedirectToAction("Admin_forgotpassword");
            }
            else
            {
                var result = loginBLL.ForgetPassword(Email, 1);
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
                string emailText = "<tr><td><br/>You have requested to get your password. This is your password below:-</br></br></td></tr>";
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
        [HttpGet]

        #endregion 
        public ActionResult admin_dashboard()
        {
            var clients = clientBLL.selectClientList().Count;
            ViewBag.ClientList = clients;
            var requestedDocuments = pablaAccountsEntities.tbl_RequestedDocument.Where(x => x.IsDeleted == false && x.IsDeclined == false && x.IsUploaded == false).ToList().Count;
            ViewBag.RequestedDocuments = requestedDocuments;
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
                if (TempData["Success"] != null)
                {
                    ViewBag.Success = Convert.ToString(TempData["Success"]);
                }
                var result = clientBLL.selectClientList().OrderByDescending(x => x.UserId).ToList();
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
                var ExistClient = pablaAccountsEntities.tblUsers.Where(x => x.CorporateAccessNumber == clientEntity.CorporateAccessNumber && x.IsDeleted == false).FirstOrDefault();
                if (ExistClient == null)
                {
                    TempData["Success"] = "2";
                    clientBLL.AddNewClient(clientEntity);
                }
                else
                {
                    ViewBag.ExistClient = "1";
                    return View();
                }

            }
            return RedirectToAction("client");
        }

        public ActionResult GeneratePassword(int ClientId = 0, string Email = "", string FirstName = "", string LastName = "", string CorporateAccessNumber = "", string DateOfBirth = "")
        {
            Random r = new Random();
            int rInt = r.Next(1, 10);
            var Pass = "";
            var UserName = "";
            if (FirstName.Length >= 3)
            {
                if(CorporateAccessNumber.Length >= 7)
                {
                    var corpno = CorporateAccessNumber.Substring(0, 5);
                    UserName = FirstName.Substring(0, 3) + corpno;
                    Pass = FirstName.Substring(0, 3) + rInt + DateOfBirth.Split('/')[2] + "@";
                }
                else
                {
                    
                    UserName = FirstName.Substring(0, 3) + CorporateAccessNumber;
                    Pass = FirstName.Substring(0, 3) + rInt + DateOfBirth.Split('/')[2] + "@";
                }
            }
            else
            {
                if (CorporateAccessNumber.Length >= 7)
                {
                    var corpno = CorporateAccessNumber.Substring(0, 5);
                    UserName = FirstName + corpno;
                    Pass = FirstName + rInt + DateOfBirth.Split('/')[2] + "@";
                }
                else
                {

                    UserName = FirstName + CorporateAccessNumber;
                    Pass = FirstName + rInt + DateOfBirth.Split('/')[2] + "@";
                }
                //UserName = FirstName + CorporateAccessNumber.Substring(0, 5);
                //Pass = FirstName + rInt + DateOfBirth.Split('/')[2] + "@";
            }
            
            //Pass = FirstName.ToLower() + DateOfBirth.Split('/')[2] + "@" + rInt;
            var EncryPassword = encryDecry.EncryptPassword(Pass);
            var Decrypt = encryDecry.DecryptPassword(EncryPassword);
            clientBLL.UpdateCredential(ClientId, UserName, EncryPassword);
            SendCredential(ClientId, UserName, FirstName, LastName, Decrypt, Email);
            TempData["Message"] = "1";
            return RedirectToAction("client_view", new { ClientId = ClientId });
        }

        public ActionResult SendCredentialToClient(int clientId = 0, string email = "", string firstName = "", string lastName = "", string UserName = "", string Password = "")
        {
            string htmlBody = "";
            string headerText = "Hi <b> " + firstName + " " + lastName + " ,</b>";
            string startTable = "<table>";
            string emailText = "<tr><td><br/>As per your request for credential, Your credentials are given as below:-</br></br></td></tr>";
            //emailText += "<tr><td>Name:<b> "  "</b></td></tr>";
            emailText += "<tr><td>UserName:<b> " + UserName + "</b></td></tr>";
            emailText += "<tr><td>Password:<b> " + Password + "</b></td></tr>";
            emailText += "<tr><td> You can login with above credentials from below link:-</td></tr>";
            emailText += "<tr><td><b>http://pablastax.com/Client/client_login </b></td></tr>";
            emailText += "<tr><td>Regards</td></tr>";
            emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
            string endTable = "<br/></table> </br> </br> Thanks";
            htmlBody = headerText + startTable + emailText + endTable;
            customMethod.SendEmail(email, "Credential Information", htmlBody, "");
            TempData["Message"] = "2";
            return RedirectToAction("client_view", new { ClientId = clientId });
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
        public ActionResult client_view(int ClientId = 0, string PersonNames = "", string DocumentTypes = "", string SearchYear = "", int UserId = 0, string SearchMonthly = "", string SearchQuaterly = "")
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = Convert.ToString(TempData["Message"]);
            }
            if (TempData["Status"] != null)
            {
                ViewBag.Status = Convert.ToString(TempData["Status"]);
            }
            var model = new ClientEntity();
            if (Session["FirstName"] == null && Session["LastName"] == null)
            {
                return RedirectToAction("admin_login");
            }
            else
            {
                List<tblClientDocument> result = new List<tblClientDocument>();
                if (PersonNames == "" && DocumentTypes == "" && SearchYear == "")
                {
                    result = clientBLL.selectAllDocumentForClient(ClientId);
                }
                else
                {
                    result = clientBLL.SearchDocumentByQuery(ClientId, PersonNames, DocumentTypes, SearchYear, SearchMonthly, SearchQuaterly);
                }

                model = clientBLL.GetAllClient(ClientId);
                model.Password = encryDecry.DecryptPassword(model.Password);
                var PersonList = pablaAccountsEntities.tblClientDocuments.Where(x => x.UserId == ClientId && x.IsDeleted == false).Select(x => x.PersonName).ToList();


                IEnumerable<SelectListItem> selectPersonList = from Person in PersonList
                                                               select new SelectListItem
                                                               {
                                                                   Text = Convert.ToString(Person),
                                                                   Value = Convert.ToString(Person)
                                                               };
                ViewBag.PersonName = new SelectList(selectPersonList, "Text", "Value");
                ViewBag.userid = model.UserId;
                ViewBag.ClientId = ClientId;
                ViewBag.TotalDocument = result;
            }
            return View("client_view", model);
        }


        public ActionResult DeleteClient(int UserId)
        {
            clientBLL.DeleteClient(UserId);
            TempData["Delete"] = "1";
            return RedirectToAction("client");
        }
        public ActionResult DeleteDocument(int DocumentId = 0, int ClientId = 0)
        {
            clientBLL.DeleteDocument(DocumentId);
            TempData["Delete"] = "1";
            return RedirectToAction("client_view", new { ClientId = ClientId });
        }

        [HttpPost]
        public ActionResult savedocuments(FileUploadEntity fileUploadEntity, string SendCompanyName)
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

            if (fileUploadEntity.Monthly != "")
            {
                fileUploadEntity.Quaterly = "";
            }
            if (fileUploadEntity.Quaterly != "")
            {
                fileUploadEntity.Monthly = "";
            }
            if (fileUploadEntity.DocumentType == "Other")
            {
                fileUploadEntity.DocumentType = fileUploadEntity.OtherDocuments;
            }
            clientBLL.Savedocuments(fileUploadEntity);
            clientBLL.InsertDocumentType(fileUploadEntity.DocumentType);
            //clientBLL.InsertDocumentType(fileUploadEntity.DocumentType);
            tblUser tbluser = pablaAccountsEntities.tblUsers.SingleOrDefault(b => b.UserId == fileUploadEntity.UserId);

            string htmlBody = "";
            string headerText = "Hi <b> " + tbluser.FirstName + " " + tbluser.LastName + " ,</b>";
            string startTable = "<table>";
            string emailText = "<tr><td><br/>A document has been uploaded for the person name  <b> " + fileUploadEntity.PersonName + " </b> of the company " + SendCompanyName + " with document type  <b> " + fileUploadEntity.DocumentType + " </b>. Please find an attachment below:-</br></br></td></tr>";
            emailText += "<tr><td>Regards</td></tr>";
            emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
            string endTable = "<br/></table> </br> </br> Thanks";
            htmlBody = headerText + startTable + emailText + endTable;
            string fullfilepath = path + fileName;
            customMethod.SendEmail(tbluser.Email, "Document Uploaded", htmlBody, fullfilepath);
            TempData["Message"] = "3";
            return RedirectToAction("client_view", new { ClientId = fileUploadEntity.UserId });
        }
        public bool SendCredential(int UserId, string UserName, string FirstName, string LastName, string Password, string Email)
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
                emailText += "<tr><td><b>http://pablastax.com/Client/client_login </b></td></tr>";
                emailText += "<tr><td>Regards</td></tr>";
                emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                customMethod.SendEmail(Email, "Account Information", htmlBody, "");
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
            if (TempData["Messsage"] != null)
            {
                ViewBag.Messsage = Convert.ToString(TempData["Messsage"]);
            }
            if (Session["FirstName"] == null && Session["LastName"] == null)
            {
                return RedirectToAction("admin_login");
            }
            //if (TempData["ApproveSuccess"] != null)
            //{
            //    ViewBag.ApproveSuccess = Convert.ToString(TempData["ApproveSuccess"]);
            //}
            //if (TempData["ApproveError"] != null)
            //{
            //    ViewBag.ApproveError = Convert.ToString(TempData["ApproveError"]);
            //}
            //dynamic model = new ExpandoObject();
            dynamic joinResult = (from d in pablaAccountsEntities.tbl_RequestedDocument
                                  from p in pablaAccountsEntities.tblUsers
                                  where d.RequestedBy == p.UserId
                                  select new
                                  {
                                      RequestDocumentId = d.RequestDocumentId,
                                      DocumentType = d.DocumentType,
                                      Other = d.Other,
                                      PersonName = d.PersonName,
                                      Year = d.Year,
                                      Description = d.Description,
                                      RequestedBy = p.FirstName + " " + p.LastName,
                                      RequestedById = p.UserId,
                                      CreatedOn = d.CreatedOn,
                                      IsDeleted = d.IsDeleted,
                                      IsApprooved = d.IsApprooved,
                                      IsDeclined = d.IsDeclined,
                                      IsUploaded = d.IsUploaded,
                                      Monthly = d.Monthly
                                  }).Where(x => x.IsDeleted == false && x.IsDeclined == false && x.IsUploaded == false).OrderByDescending(x => x.RequestDocumentId)
                                  .AsEnumerable()    //important to convert to Enumerable
                        .Select(c => c.ToExpando()); //convert to ExpandoObject;

            //List<tbl_RequestedDocument> rd = pablaAccountsEntities.tbl_RequestedDocument.Where(x => x.IsDeleted == false).OrderByDescending(x=>x.RequestDocumentId).ToList();
            //ViewBag.Result = joinResult;
            //model = joinResult;

            // Create a list of dynamic objects to form the view model
            // that has prettified rate code
            var forecastRates = new List<dynamic>();
            //forecastRates = joinResult.ToExpando();
            //foreach (var fr in joinResult)
            //{
            //    dynamic f = new ExpandoObject();

            //    f.RequestDocumentId = fr.RequestDocumentId;
            //    f.DocumentType = fr.DocumentType;
            //    f.Other = fr.Other;
            //    f.PersonName = fr.PersonName;
            //    f.Year = fr.Year;
            //    f.Description = fr.Description;
            //    f.RequestedBy = fr.RequestedBy;
            //    f.CreatedOn = fr.CreatedOn;
            //    f.IsDeleted = fr.IsDeleted;
            //    f.IsApprooved = fr.IsApprooved;
            //    f.IsDeclined = fr.IsDeclined;

            //    forecastRates.Add(f);
            //}



            return View(joinResult);
        }
        //public static ExpandoObject ToExpando(this object anonymousObject)
        //{
        //    IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
        //    IDictionary<string, object> expando = new ExpandoObject();
        //    foreach (var item in anonymousDictionary)
        //        expando.Add(item);
        //    return (ExpandoObject)expando;
        //}
        //public static ExpandoObject ToExpando(this object anonymousObject)
        //{
        //    IDictionary<string, object> expando = new ExpandoObject();
        //    foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
        //    {
        //        var obj = propertyDescriptor.GetValue(anonymousObject);
        //        expando.Add(propertyDescriptor.Name, obj);
        //    }

        //    return (ExpandoObject)expando;
        //}

        [HttpPost]
        public ActionResult approve_document(int requestedDocumentId, string Message)
        {
            //tbl_RequestedDocument rd = pablaAccountsEntities.tbl_RequestedDocument.Where(x => x.RequestDocumentId).ToList();
            var result = pablaAccountsEntities.tbl_RequestedDocument.SingleOrDefault(b => b.RequestDocumentId == requestedDocumentId);
            tblUser tbluser = new tblUser();
            if (result != null)
            {
                result.IsApprooved = true;
                pablaAccountsEntities.SaveChanges();
                tbluser = pablaAccountsEntities.tblUsers.SingleOrDefault(b => b.UserId == result.RequestedBy);
            }
            string htmlBody = "";
            string headerText = "Hi <b> " + tbluser.FirstName + " " + tbluser.LastName + " ,</b>";
            string startTable = "<table>";
            string emailText = "<tr><td><br/>You request for addition of document named as <b> " + result.DocumentName + " </b> of Document Type <b> " + result.DocumentType + " </b> has been successfully approved by our company. You will get a document attached with your email shortly. The message for approval of your document from company is as below:-</br></br></td></tr>";
            emailText += "<tr><td>" + Message + "</td></tr>";
            emailText += "<tr><td>Regards</td></tr>";
            emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
            string endTable = "<br/></table> </br> </br> Thanks";
            htmlBody = headerText + startTable + emailText + endTable;
            bool status = customMethod.SendEmail(tbluser.Email, "Document Approved", htmlBody, "");
            //if (status == "success")
            //{
            //    TempData["ApproveSuccess"] = "Document approved successfully";
            //}
            //else
            //{
            //    TempData["ApproveError"] = status;
            //}
            TempData["Messsage"] = "1";
            return RedirectToAction("requested_document", "admin");
        }
        [HttpPost]
        public ActionResult deny_document(int requestedDocumentId, string Reason)
        {
            //tbl_RequestedDocument rd = pablaAccountsEntities.tbl_RequestedDocument.Where(x => x.RequestDocumentId).ToList();
            var result = pablaAccountsEntities.tbl_RequestedDocument.SingleOrDefault(b => b.RequestDocumentId == requestedDocumentId);
            tblUser tbluser = new tblUser();
            if (result != null)
            {
                result.IsDeclined = true;
                pablaAccountsEntities.SaveChanges();
                tbluser = pablaAccountsEntities.tblUsers.SingleOrDefault(b => b.UserId == result.RequestedBy);
            }
            string htmlBody = "";
            string headerText = "Hi <b> " + tbluser.FirstName + " " + tbluser.LastName + " ,</b>";
            string startTable = "<table>";
            string emailText = "<tr><td><br/>You request for addition of document named as <b> " + result.DocumentName + " </b> of Document Type <b> " + result.DocumentType + " </b> has been declined by our company. The reason for decline is as below:-</br></br></td></tr>";
            emailText += "<tr><td>" + Reason + "</td></tr>";
            emailText += "<tr><td>Regards</td></tr>";
            emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
            string endTable = "<br/></table> </br> </br> Thanks";
            htmlBody = headerText + startTable + emailText + endTable;
            customMethod.SendEmail(tbluser.Email, "Document Declined", htmlBody, "");
            TempData["Messsage"] = "2";
            return RedirectToAction("requested_document", "admin");
        }

        public ActionResult add_document(int requestedDocumentId, string clientName)
        {
            FileUploadEntity fileuploadentity = new FileUploadEntity();
            var result = pablaAccountsEntities.tbl_RequestedDocument.SingleOrDefault(b => b.RequestDocumentId == requestedDocumentId);
            if (result != null)
            {

                fileuploadentity.UserId = (int)result.RequestedBy;
                fileuploadentity.PersonName = result.PersonName;
                fileuploadentity.year = result.Year;
                fileuploadentity.DocumentType = result.DocumentType;
                fileuploadentity.Other = result.Other;
                fileuploadentity.Monthly = result.Monthly;
                fileuploadentity.Periodending = result.Periodending;
                fileuploadentity.RequestedDocumentId = requestedDocumentId;
            }
            ViewBag.ClientName = clientName;
            return View(fileuploadentity);

        }
        [HttpPost]
        public ActionResult add_document(FileUploadEntity fileUploadEntity)
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
            if (fileUploadEntity.DocumentType == "Other")
            {
                fileUploadEntity.DocumentType = fileUploadEntity.Other;
            }
            clientBLL.Savedocuments(fileUploadEntity);
            clientBLL.InsertDocumentType(fileUploadEntity.DocumentType);
            var result = pablaAccountsEntities.tbl_RequestedDocument.SingleOrDefault(b => b.RequestDocumentId == fileUploadEntity.RequestedDocumentId);
            tblUser tbluser = new tblUser();
            if (result != null)
            {
                result.IsUploaded = true;
                pablaAccountsEntities.SaveChanges();
                tbluser = pablaAccountsEntities.tblUsers.SingleOrDefault(b => b.UserId == result.RequestedBy);
            }
            string htmlBody = "";
            string headerText = "Hi <b> " + tbluser.FirstName + " " + tbluser.LastName + " ,</b>";
            string startTable = "<table>";
            string emailText = "<tr><td><br/>As per Your request for addition of document named as <b> " + result.DocumentName + " </b> of Document Type <b> " + result.DocumentType + " </b> for <b> " + fileUploadEntity.PersonName + " </b> , We have uploaded a document, Please find an attachment below:-</br></br></td></tr>";
            emailText += "<tr><td>Regards</td></tr>";
            emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
            string endTable = "<br/></table> </br> </br> Thanks";
            htmlBody = headerText + startTable + emailText + endTable;
            string fullfilepath = path + fileName;
            customMethod.SendEmail(tbluser.Email, "Document Uploaded", htmlBody, fullfilepath);
            TempData["Messsage"] = "3";
            return RedirectToAction("requested_document", "admin");
        }
        [HttpGet]
        public ActionResult admin_changepassword(int UserId = 0)
        {
            var model = clientBLL.GetAllClient(UserId);
            ViewBag.Email = model.UserName;
            ViewBag.UserId = UserId;
            return View();
        }

        [HttpPost]
        public ActionResult admin_changepassword(int UserId, string OldPassword, string Password, string ConfirmPassword)
        {
            var result = pablaAccountsEntities.tblUsers.SingleOrDefault(b => b.UserId == UserId && b.IsDeleted == false);
            var Pass = result.Password;
            var DecryptPass = encryDecry.DecryptPassword(Pass);
            if (DecryptPass == OldPassword)
            {
                var EncryPassword = encryDecry.EncryptPassword(Password);
                var EncryConfirmPassword = encryDecry.EncryptPassword(ConfirmPassword);
                clientBLL.UpdateClientPassword(UserId, EncryPassword, EncryConfirmPassword);
                return RedirectToAction("admin_login");
            }
            else
            {
                TempData["Message"] = "Please enter correct old passsword";
                return RedirectToAction("admin_changepassword", new { UserId = UserId });
            }
           

        }

        public ActionResult BulkEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BulkEmail(string message)
        {
            var tbluser = pablaAccountsEntities.tblUsers.Where(x => x.IsDeleted == false).ToList();
            foreach (var item in tbluser)
            {
                string htmlBody = "";
                string headerText = "Hi <b> " + item.FirstName + " " + item.LastName + " ,</b>";
                string startTable = "<table>";
                string emailText = "<tr><td><br/>" + message + "</br></br></td></tr>";
                emailText += "<tr><td>Regards</td></tr>";
                emailText += "<tr><td><b>Pabla Accounting And Tax Services</b></td></tr>";
                string endTable = "<br/></table> </br> </br> Thanks";
                htmlBody = headerText + startTable + emailText + endTable;
                bool status = customMethod.SendEmail(item.Email, "Message From Admin", htmlBody, "");
            }
            ViewBag.Status = "Your message has been emailed to all your clients.";
            return View();
        }

        [HttpPost]
        public ActionResult SaveNotes(int clientId, string Notes)
        {
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserId == clientId && x.IsDeleted == false && x.Isactive == false ).SingleOrDefault();
            if (result != null)
            {
                result.Notes = Notes;
                pablaAccountsEntities.SaveChanges();
            }
            TempData["Status"] = "A note has been added successfully for this client.";
            return RedirectToAction("client_view", "Admin", new { ClientId = clientId, PersonNames = "", DocumentTypes = "", SearchYear = "", UserId = 0, SearchMonthly = "", SearchQuaterly = "" });
        }
        
        public ActionResult BlockeUser(int UserId= 0)
        {
            var client_data= pablaAccountsEntities.tblUsers.Where(x => x.UserId == UserId && x.IsDeleted == false).SingleOrDefault();
            client_data.Isactive = true;
            pablaAccountsEntities.SaveChanges();
            return RedirectToAction("client");
        }

      
        public ActionResult UnBlockUser(int UserId = 0)
        {
            var client_data = pablaAccountsEntities.tblUsers.Where(x => x.UserId == UserId && x.IsDeleted == false).SingleOrDefault();
            client_data.Isactive = false;
            pablaAccountsEntities.SaveChanges();
            return RedirectToAction("client");
        }
    }
}