using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PablaAccountingAndTaxServicesEntity
{
    public class FileUploadEntity
    {
        public int UserId { get; set; }
        public string PersonName { get; set; }

        public string DocumentType{ get; set; }
        public HttpPostedFileBase UploadFile { get; set; }
        public string year { get; set; }
       
        
        public string Extension { get; set; }
        public string DocumentName { get; set; }
       

       

    }
}
