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
                string a = "Hii";
                string Senderemail = ConfigurationManager.AppSettings["Senderemail"].ToString();
                string SenderPassword = ConfigurationManager.AppSettings["SenderPassword"].ToString();
                string emailserver = ConfigurationManager.AppSettings["emailserver"].ToString();
                string port = ConfigurationManager.AppSettings["port"].ToString();
                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress(Senderemail);
                Msg.To.Add(new MailAddress(To));
                Msg.Subject = Subject;
                Msg.Body = a;
                Msg.IsBodyHtml = true;
                if (attachedfile != "")
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(attachedfile);
                    Msg.Attachments.Add(attachment);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = emailserver;
                smtp.UseDefaultCredentials = false;
                NetworkCredential networkCredential = new NetworkCredential(Senderemail, SenderPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = networkCredential;
                smtp.Port = Convert.ToInt32(port);
                smtp.EnableSsl = false;
                smtp.Send(Msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //mail method to send email

        //MailMessage message = new MailMessage();
        //message.From = new MailAddress("your email address");

        //message.To.Add(new MailAddress("your recipient"));

        //message.Subject = "your subject";
        //message.Body = "content of your email";
        
        //SmtpClient client = new SmtpClient();
        //client.Send(message);
    }
}
