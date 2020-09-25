using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PablaAccountingAndTaxServicesEntity
{
    public class DocumentEntity
    {
        public int RequestDocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string Other { get; set; }
        public string PersonName { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public int RequestedBy { get; set; }
    }

    public class RequestedDocuments
    {
        public long RequestDocumentId { get; set; }
        public string DocumentType { get; set; }
        public string Other { get; set; }
        public string Person { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string RequestedBy { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsApprooved { get; set; }
        public Nullable<bool> IsDeclined { get; set; }
    }          
}
