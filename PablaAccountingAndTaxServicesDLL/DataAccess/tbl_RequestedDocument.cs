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
    
    public partial class tbl_RequestedDocument
    {
        public long RequestDocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string Other { get; set; }
        public string PersonName { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public Nullable<int> RequestedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsApprooved { get; set; }
        public Nullable<bool> IsDeclined { get; set; }
        public Nullable<bool> IsUploaded { get; set; }
    }
}
