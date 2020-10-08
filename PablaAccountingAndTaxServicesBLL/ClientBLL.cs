using PablaAccountingAndTaxServicesDLL.DataAccess;
using PablaAccountingAndTaxServicesDLL.Interface;
using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PablaAccountingAndTaxServicesBLL
{
    public class ClientBLL
    {
        IClient client;
        public ClientBLL()
        {
            client = new PablaAccountingAndTaxServicesDLL.DataAccess.SQLClient();
        }
        public List<tblUser> selectClientList()
        {
            return client.selectClientList();
        }
        //public List<tblClientDocument> GetPersonName(int ClientId)
        //{
        //    return client.GetPersonName(ClientId);
        //}
        public void AddNewClient(ClientEntity clientEntity)
        {
            client.AddNewClient(clientEntity);
        }
        public ClientEntity GetAllClient(int ClientId)
        {
            return client.GetAllClient(ClientId);
        }
        public List<tbl_RequestedDocument> GetRequest(int ClientId)
        {
            return client.GetRequest(ClientId);
        }
        public void UpdateClient(ClientEntity clientEntity)
        {
            client.UpdateClient(clientEntity);
        }
        public void UpdateClientPassword(int UserId, string Password, string ConfirmPassword)
        {
            client.UpdateClientPassword(UserId, Password, ConfirmPassword);
        }
        public void UpdateCredential(int ClientId, string Email,string EncryPassword)
        {
            client.UpdateCredential(ClientId, Email, EncryPassword);
        }
        public void DeleteClient(int UserId)
        {
            client.DeleteClient(UserId);
        }

        public void DeleteRequest(int UserId)
        {
            client.DeleteRequest(UserId);
        }
        public List<tblClientDocument> selectAllDocumentForClient(int clientId)
        {
            return client.selectAllDocumentForClient(clientId);
        }
        public void Savedocuments(FileUploadEntity fileUploadEntity)
        {
            client.Savedocuments(fileUploadEntity);
        }
        public List<tblClientDocument> SearchDocumentByQuery(int UserId, string PersonName, string DocumentType, string Year,string Monthly)
        {
            return client.SearchDocumentByQuery(UserId, PersonName, DocumentType, Year, Monthly);
        }
        public void RequestDocumentByClient(int UserId, string DocumentType, string Year, string PersonName, string Description,string OtherDocuments, string Months,string PeriodTime )
        {
            client.RequestDocumentByClient(UserId, DocumentType,Year,PersonName,Description, OtherDocuments, Months, PeriodTime);
         
        }
        //public List<tblClientDocument> selectClientname(int ClientId)
        //{
        //    return client.selectClientname(ClientId);
        //}
        public void SaveFilePersonalTax(FilePersonalTaxEntity filePersonalTaxEntity)
        {
            client.SaveFilePersonalTax(filePersonalTaxEntity);
        }
        public void InsertDocumentType(string DocumentType)
        {
            client.InsertDocumentType(DocumentType);
        }

    }
}
