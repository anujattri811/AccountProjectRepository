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
}
