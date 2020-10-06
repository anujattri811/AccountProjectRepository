using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PablaAccountingAndTaxServicesEntity
{
   public class FilePersonalTaxEntity
    {
        public int FilePersonalTaxId { get; set; }
        public bool IsExiting { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SIN { get; set; }
        public string DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string MaritalStatus { get; set; }
        public string Sex { get; set; }
        public string CurrentAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseMiddleName { get; set; }
        public string SpouseLastName { get; set; }
        public string SpouseSIN { get; set; }
        public string SpouseDateOfBirth { get; set; }
        public string Children1Name { get; set; }
        public string Children1DateOfBirth { get; set; }
        public string Entrydatetime1 { get; set; }
        public string Entrydatetime { get; set; }
        public string Children2Name { get; set; }
        public string Children2DateOfBirth { get; set; }
        public string Children3Name { get; set; }
        public string Children3DateOfBirth { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        
        
    }
}
