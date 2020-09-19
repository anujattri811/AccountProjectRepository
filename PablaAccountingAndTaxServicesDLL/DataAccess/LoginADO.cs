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
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserName == UserName && x.Password == Password && x.RoleId== 1).FirstOrDefault();
            LoginEntity loginEntity = new LoginEntity();
            if (result != null)
            {
                loginEntity.FirstName = result.FirstName;
                loginEntity.LastName = result.LastName;
            }
            return loginEntity;
        }
        public LoginEntity CheckClientLogin(string Username, string Password)
        {
            LoginEntity loginEntity = new LoginEntity();
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserName == Username && x.Password == Password && x.RoleId != 1).FirstOrDefault();
            if (result != null)
            {
                loginEntity.FirstName = result.FirstName;
                loginEntity.LastName = result.LastName;
                loginEntity.UserId = result.UserId;
            }
            return loginEntity;
        }
    }
}
