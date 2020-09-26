using PablaAccountingAndTaxServicesEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PablaAccountingAndTaxServices.CommanClass
{
    public class CustomMethod
    {
        public bool SendEmail(string To, string Subject, string Body, string attachedfile)
        {
            try
            {
                string Senderemail = ConfigurationManager.AppSettings["Senderemail"].ToString();
                string SenderPassword = ConfigurationManager.AppSettings["SenderPassword"].ToString();
                string emailserver = ConfigurationManager.AppSettings["emailserver"].ToString();
                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress(Senderemail);
                Msg.To.Add(To);
                Msg.Subject = Subject;
                Msg.Body = Body;
                Msg.IsBodyHtml = true;
                if (attachedfile != "")
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(attachedfile);
                    Msg.Attachments.Add(attachment);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = emailserver;
                NetworkCredential networkCredential = new NetworkCredential(Senderemail, SenderPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(Msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
