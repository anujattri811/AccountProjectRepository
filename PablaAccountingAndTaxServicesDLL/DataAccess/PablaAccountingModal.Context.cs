﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PablaAccountingAndTaxServicesDLL.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PablaAccountsEntities : DbContext
    {
        public PablaAccountsEntities()
            : base("name=PablaAccountsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblClientDocument> tblClientDocuments { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tbl_RequestedDocument> tbl_RequestedDocument { get; set; }
    
        public virtual int usp_insertclient(string firstName, string lastName, string email, string dateOfBirth, string mobileNo, string companryName, string address, string city, string postalCode, string province, string country, string sIN, string gSTNumber, string wCB, string password, Nullable<int> roleId, string corporateAccessNumber)
        {
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var dateOfBirthParameter = dateOfBirth != null ?
                new ObjectParameter("DateOfBirth", dateOfBirth) :
                new ObjectParameter("DateOfBirth", typeof(string));
    
            var mobileNoParameter = mobileNo != null ?
                new ObjectParameter("MobileNo", mobileNo) :
                new ObjectParameter("MobileNo", typeof(string));
    
            var companryNameParameter = companryName != null ?
                new ObjectParameter("CompanryName", companryName) :
                new ObjectParameter("CompanryName", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var cityParameter = city != null ?
                new ObjectParameter("City", city) :
                new ObjectParameter("City", typeof(string));
    
            var postalCodeParameter = postalCode != null ?
                new ObjectParameter("PostalCode", postalCode) :
                new ObjectParameter("PostalCode", typeof(string));
    
            var provinceParameter = province != null ?
                new ObjectParameter("Province", province) :
                new ObjectParameter("Province", typeof(string));
    
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var sINParameter = sIN != null ?
                new ObjectParameter("SIN", sIN) :
                new ObjectParameter("SIN", typeof(string));
    
            var gSTNumberParameter = gSTNumber != null ?
                new ObjectParameter("GSTNumber", gSTNumber) :
                new ObjectParameter("GSTNumber", typeof(string));
    
            var wCBParameter = wCB != null ?
                new ObjectParameter("WCB", wCB) :
                new ObjectParameter("WCB", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var roleIdParameter = roleId.HasValue ?
                new ObjectParameter("RoleId", roleId) :
                new ObjectParameter("RoleId", typeof(int));
    
            var corporateAccessNumberParameter = corporateAccessNumber != null ?
                new ObjectParameter("CorporateAccessNumber", corporateAccessNumber) :
                new ObjectParameter("CorporateAccessNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_insertclient", firstNameParameter, lastNameParameter, emailParameter, dateOfBirthParameter, mobileNoParameter, companryNameParameter, addressParameter, cityParameter, postalCodeParameter, provinceParameter, countryParameter, sINParameter, gSTNumberParameter, wCBParameter, passwordParameter, roleIdParameter, corporateAccessNumberParameter);
        }
    
        public virtual int usp_updateclient(Nullable<long> userId, string firstName, string lastName, string dateOfBirth, string email, string mobileNo, string companyName, string address, string city, string postalCode, string province, string country, string sIN, string gSTNumber, string wCB, string corporateAccessNumber)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(long));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var dateOfBirthParameter = dateOfBirth != null ?
                new ObjectParameter("DateOfBirth", dateOfBirth) :
                new ObjectParameter("DateOfBirth", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var mobileNoParameter = mobileNo != null ?
                new ObjectParameter("MobileNo", mobileNo) :
                new ObjectParameter("MobileNo", typeof(string));
    
            var companyNameParameter = companyName != null ?
                new ObjectParameter("CompanyName", companyName) :
                new ObjectParameter("CompanyName", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var cityParameter = city != null ?
                new ObjectParameter("City", city) :
                new ObjectParameter("City", typeof(string));
    
            var postalCodeParameter = postalCode != null ?
                new ObjectParameter("PostalCode", postalCode) :
                new ObjectParameter("PostalCode", typeof(string));
    
            var provinceParameter = province != null ?
                new ObjectParameter("Province", province) :
                new ObjectParameter("Province", typeof(string));
    
            var countryParameter = country != null ?
                new ObjectParameter("Country", country) :
                new ObjectParameter("Country", typeof(string));
    
            var sINParameter = sIN != null ?
                new ObjectParameter("SIN", sIN) :
                new ObjectParameter("SIN", typeof(string));
    
            var gSTNumberParameter = gSTNumber != null ?
                new ObjectParameter("GSTNumber", gSTNumber) :
                new ObjectParameter("GSTNumber", typeof(string));
    
            var wCBParameter = wCB != null ?
                new ObjectParameter("WCB", wCB) :
                new ObjectParameter("WCB", typeof(string));
    
            var corporateAccessNumberParameter = corporateAccessNumber != null ?
                new ObjectParameter("CorporateAccessNumber", corporateAccessNumber) :
                new ObjectParameter("CorporateAccessNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_updateclient", userIdParameter, firstNameParameter, lastNameParameter, dateOfBirthParameter, emailParameter, mobileNoParameter, companyNameParameter, addressParameter, cityParameter, postalCodeParameter, provinceParameter, countryParameter, sINParameter, gSTNumberParameter, wCBParameter, corporateAccessNumberParameter);
        }
    
        public virtual int usp_insertclientdocument(Nullable<long> userId, string personName, string documentType, string year, string documentName, string discription, string other)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(long));
    
            var personNameParameter = personName != null ?
                new ObjectParameter("PersonName", personName) :
                new ObjectParameter("PersonName", typeof(string));
    
            var documentTypeParameter = documentType != null ?
                new ObjectParameter("DocumentType", documentType) :
                new ObjectParameter("DocumentType", typeof(string));
    
            var yearParameter = year != null ?
                new ObjectParameter("Year", year) :
                new ObjectParameter("Year", typeof(string));
    
            var documentNameParameter = documentName != null ?
                new ObjectParameter("DocumentName", documentName) :
                new ObjectParameter("DocumentName", typeof(string));
    
            var discriptionParameter = discription != null ?
                new ObjectParameter("Discription", discription) :
                new ObjectParameter("Discription", typeof(string));
    
            var otherParameter = other != null ?
                new ObjectParameter("Other", other) :
                new ObjectParameter("Other", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_insertclientdocument", userIdParameter, personNameParameter, documentTypeParameter, yearParameter, documentNameParameter, discriptionParameter, otherParameter);
        }
    
        public virtual ObjectResult<string> GetPersonName(Nullable<long> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GetPersonName", userIdParameter);
        }
    
        public virtual int GeneratePassword(Nullable<long> userId, string userName, string password)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(long));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GeneratePassword", userIdParameter, userNameParameter, passwordParameter);
        }
    
        public virtual int usp_insertRequestdocument(Nullable<long> userId, string documentType, string year, string personName, string description, string other)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(long));
    
            var documentTypeParameter = documentType != null ?
                new ObjectParameter("DocumentType", documentType) :
                new ObjectParameter("DocumentType", typeof(string));
    
            var yearParameter = year != null ?
                new ObjectParameter("Year", year) :
                new ObjectParameter("Year", typeof(string));
    
            var personNameParameter = personName != null ?
                new ObjectParameter("PersonName", personName) :
                new ObjectParameter("PersonName", typeof(string));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var otherParameter = other != null ?
                new ObjectParameter("Other", other) :
                new ObjectParameter("Other", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_insertRequestdocument", userIdParameter, documentTypeParameter, yearParameter, personNameParameter, descriptionParameter, otherParameter);
        }
    
        public virtual ObjectResult<string> Selectpersonname(Nullable<long> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("Selectpersonname", userIdParameter);
        }
    
        public virtual int ChangePassword(Nullable<int> userId, string password, string confirmPassword)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var confirmPasswordParameter = confirmPassword != null ?
                new ObjectParameter("ConfirmPassword", confirmPassword) :
                new ObjectParameter("ConfirmPassword", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ChangePassword", userIdParameter, passwordParameter, confirmPasswordParameter);
        }
    }
}
