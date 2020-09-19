﻿using PablaAccountingAndTaxServices.CommanClass;
using PablaAccountingAndTaxServicesBLL;
using PablaAccountingAndTaxServicesDLL.DataAccess;
using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}