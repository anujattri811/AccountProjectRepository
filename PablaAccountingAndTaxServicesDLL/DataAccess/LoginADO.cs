using PablaAccountingAndTaxServicesEntity;
using System.Data.Entity;
using System.Linq;

namespace PablaAccountingAndTaxServicesDLL.DataAccess
{
    public partial class SQLLogin
    {
        PablaAccountsEntities pablaAccountsEntities = new PablaAccountsEntities();
        public LoginEntity CheckLogin(string UserName, string Password)
        {
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserName == UserName || x.Email== UserName && x.Password == Password && x.IsDeleted == false && x.RoleId== 1).FirstOrDefault();
            LoginEntity loginEntity = new LoginEntity();
            if (result != null)
            {
                loginEntity.FirstName = result.FirstName;
                loginEntity.LastName = result.LastName;
                loginEntity.UserId = result.UserId;
                
            }
            return loginEntity;
        }
        public LoginEntity CheckClientLogin(string Username, string Password)
        {
            LoginEntity loginEntity = new LoginEntity();
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserName == Username && x.Password == Password && x.RoleId != 1 && x.IsDeleted == false).FirstOrDefault();
            if (result != null)
            {
                loginEntity.FirstName = result.FirstName;
                loginEntity.LastName = result.LastName;
                loginEntity.UserId = result.UserId;
                loginEntity.Email = result.Email;
                loginEntity.Isactive = result.Isactive;
                
            }
            return loginEntity;
        }
        public LoginEntity CheckClientactive(string Username, string Password)
        {
            LoginEntity loginEntity = new LoginEntity();
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserName == Username && x.Password == Password && x.RoleId != 1 && x.IsDeleted == false && x.Isactive ==false).FirstOrDefault();
            if (result != null)
            {
                loginEntity.FirstName = result.FirstName;
                loginEntity.LastName = result.LastName;
                loginEntity.UserId = result.UserId;
                loginEntity.Email = result.Email;
                loginEntity.Isactive = result.Isactive;

            }
            return loginEntity;
        }
        public LoginEntity ForgetPassword(string Email,int RoleId)
        {
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserName == Email && x.RoleId == RoleId && x.IsDeleted == false && x.Isactive == false).FirstOrDefault();
            LoginEntity loginEntity = new LoginEntity();
            if (result != null)
            {
                loginEntity.UserId = result.UserId;
                loginEntity.FirstName = result.FirstName;
                loginEntity.LastName = result.LastName;
                loginEntity.Password = result.Password;
                loginEntity.Isactive = result.Isactive;
            }
            return loginEntity;
        }
      
    }
}
