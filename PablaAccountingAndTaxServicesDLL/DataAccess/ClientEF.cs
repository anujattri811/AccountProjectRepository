using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PablaAccountingAndTaxServicesDLL.DataAccess
{
    public partial class SQLClient
    {
        PablaAccountsEntities pablaAccountsEntities = new PablaAccountsEntities();
        public List<tblUser> selectClientList()
        {
            var result = pablaAccountsEntities.tblUsers.Where(x => x.IsDeleted == false && x.RoleId != 1).ToList();
            return result;
        }
        public void AddNewClient(ClientEntity clientEntity)
        {
            pablaAccountsEntities.usp_insertclient(clientEntity.FirstName, clientEntity.LastName, clientEntity.Email, clientEntity.DateOfBirth, clientEntity.MobileNo, clientEntity.CompanyName, clientEntity.Address, clientEntity.City, clientEntity.PostalCode, clientEntity.Province, clientEntity.Country, clientEntity.SIN, clientEntity.GSTNumber, clientEntity.WCB, clientEntity.Password, clientEntity.RoleId);
        }
        public ClientEntity GetAllClient(int ClientId)
        {
            ClientEntity clientEntity = new ClientEntity();
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserId == ClientId).SingleOrDefault();
            clientEntity.UserId = Convert.ToInt32(result.UserId);
            clientEntity.FirstName = result.FirstName;
            clientEntity.LastName = result.LastName;
            clientEntity.Email = result.Email;
            clientEntity.DateOfBirth = result.DateOfBirth;
            clientEntity.MobileNo = result.MobileNo;
            clientEntity.CompanyName = result.CompanyName;
            clientEntity.Address = result.Address;
            clientEntity.City = result.City;
            clientEntity.PostalCode = result.PostalCode;
            clientEntity.Province = result.Province;
            clientEntity.Country = result.Country;
            clientEntity.SIN = result.SIN;
            clientEntity.GSTNumber = result.GSTNumber;
            clientEntity.WCB = result.WCB;
            clientEntity.IsPassword = result.IsPassword;
            clientEntity.UserName = result.UserName;
            clientEntity.Password = result.Password;
            return clientEntity;
        }
        public void UpdateClient(ClientEntity clientEntity)
        {
            pablaAccountsEntities.usp_updateclient(clientEntity.UserId, clientEntity.FirstName, clientEntity.LastName, clientEntity.DateOfBirth, clientEntity.Email, clientEntity.MobileNo, clientEntity.CompanyName, clientEntity.Address, clientEntity.City, clientEntity.PostalCode, clientEntity.Province, clientEntity.Country, clientEntity.SIN, clientEntity.GSTNumber, clientEntity.WCB);
        }
        public void UpdateCredential(int ClientId, string Email, string EncryPassword)
        {
            pablaAccountsEntities.GeneratePassword(ClientId, Email, EncryPassword);
        }
        public void DeleteClient(int UserId)
        {
            var result = pablaAccountsEntities.tblUsers.Where(x => x.UserId == UserId).SingleOrDefault();
            result.IsDeleted = true;
            pablaAccountsEntities.SaveChanges();
        }

        public List<tblClientDocument> selectAllDocumentForClient(int clientId)
        {
            return pablaAccountsEntities.tblClientDocuments.Where(x => x.UserId == clientId).ToList();
        }
        public void Savedocuments(FileUploadEntity fileUploadEntity)
        {
            pablaAccountsEntities.usp_insertclientdocument(fileUploadEntity.UserId, fileUploadEntity.PersonName, fileUploadEntity.DocumentType, fileUploadEntity.year, fileUploadEntity.DocumentName,fileUploadEntity.Extension);

        }
        public List<tblClientDocument> SearchDocumentByQuery(int UserId, string PersonName, string DocumentType, string Year)
        {
            return pablaAccountsEntities.tblClientDocuments.Where(x => x.UserId == UserId && x.PersonName == PersonName && x.DocumentType == DocumentType && x.Year == Year).ToList();
        }

    }
}
