//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class tblClientDocument
    {
        public long DocumentId { get; set; }
        public Nullable<long> UserId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string Discription { get; set; }
        public string Year { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string PersonName { get; set; }
        public string Other { get; set; }
        public string Periodending { get; set; }
        public string Monthly { get; set; }
    }
}
