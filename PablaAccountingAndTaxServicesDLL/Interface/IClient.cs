using PablaAccountingAndTaxServicesDLL.DataAccess;
using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PablaAccountingAndTaxServicesDLL.Interface
{
   public interface IClient
    {
        List<tblUser> selectClientList();
        void AddNewClient(ClientEntity clientEntity);
        void UpdateClient(ClientEntity clientEntity);
        void UpdateCredential(int UserId,string Email,string EncryPassword);
        ClientEntity GetAllClient(int userId);
        void DeleteClient(int UserId);
        List<tblClientDocument> selectAllDocumentForClient(int clientId);
        void Savedocuments(FileUploadEntity fileUploadEntity);

        //List<tblClientDocument> GetPersonName(int ClientId);
        List<tblClientDocument> SearchDocumentByQuery(int ClientId, string PersonName, string DocumentType, string Year);
    }
}
