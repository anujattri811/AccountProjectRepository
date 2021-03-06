﻿using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PablaAccountingAndTaxServicesDLL.Interface
{
    public interface ILogin
    {
        LoginEntity CheckLogin(string UserName, string Password);
        LoginEntity ForgetPassword(string Email,int RoleId);
        LoginEntity CheckClientLogin(string Username, string Password);
        LoginEntity CheckClientactive(string Username, string Password);
    }
}
