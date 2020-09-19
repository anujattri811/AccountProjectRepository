using PablaAccountingAndTaxServicesDLL.Interface;
using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PablaAccountingAndTaxServicesBLL
{
    public class LoginBLL
    {
        ILogin ilogin;
        public LoginBLL()
        {
            ilogin = new PablaAccountingAndTaxServicesDLL.DataAccess.SQLLogin();
        }
        public LoginEntity CheckLogin(string UserName, string Password)
        {
            return ilogin.CheckLogin(UserName, Password);
        }
        public LoginEntity CheckClientLogin(string Username, string Password)
        {
            return ilogin.CheckClientLogin(Username, Password);
        }
    }
}


