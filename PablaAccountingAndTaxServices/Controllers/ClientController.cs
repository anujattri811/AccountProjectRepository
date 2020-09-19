using PablaAccountingAndTaxServices.CommanClass;
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

        #region client_login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult clients_login()
        {
            return View();
        }
        public ActionResult client_login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult client_login(tblUser tbluser)
        {
            var EncryPassword = encryDecry.EncryptPassword(tbluser.Password);
            var result = loginBLL.CheckClientLogin(tbluser.UserName, EncryPassword);
            if(result.FirstName==null && result.LastName == null)
            {
                ViewBag.Message = "You have entered incorrect Username and Password.";
                return RedirectToAction("client_login","Client");
            }
            else
            {
                Session["FirstName"] = result.FirstName;
                Session["LastName"] = result.LastName;
                return RedirectToAction("Client_Dashboard","Client");
            }
        }
        
        public ActionResult Client_Dashboard()
        {
            return View();
        }
        #endregion
    }
}