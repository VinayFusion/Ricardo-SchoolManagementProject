using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SchoolManagementSystem.Common
{
    #region Edit By Dheeraj
    //public class SendEmail
    //{
    //    string _AppName = ConfigurationManager.AppSettings["First_Word_AppName"] + " " + ConfigurationManager.AppSettings["Second_Word_AppName"];

    //    public void Send(string receiverName, string subject, string ToEmail, string msg, string AttachFileName)
    //    {
    //        if (AttachFileName == "")
    //        {
    //            #region Send withiout Attachment

    //            string bodys = @"<html><body><div><p>Hi " + receiverName + @"</p>
    //            <p style='text-align:justify;'>" + msg + @"
    //            </p><br>
    //            <div>
    //            <span style='border-bottom:1px solid #0070AC; width:100%;Float: left;'><h3 style='border-bottom:2px solid #0070AC; Float:left; margin-bottom:0;'>" + _AppName + @"</h3></span>
    //            <div style='Float: left !important; margin-right: 10px;background-color:black;width:100%;text-align:center;'>
    //            </div>
    //            </div>
    //            </div></body></html>";

    //            MailMessage message2 = new MailMessage();
    //            SmtpClient smtpClient = new SmtpClient();

    //            MailAddress fromAddress = new MailAddress("", _AppName);
    //            message2.From = fromAddress;
    //            message2.To.Add(ToEmail);
    //            message2.Subject = subject;
    //            message2.IsBodyHtml = true;
    //            message2.Body = bodys;
    //            message2.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
    //            message2.Priority = MailPriority.High;
    //            AlternateView htmlMail = AlternateView.CreateAlternateViewFromString(bodys, null, "text/html");

    //            //LinkedResource myimage = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("/Content/images/logo1.png"));
    //            //myimage.ContentId = "companylogo";
    //            //htmlMail.LinkedResources.Add(myimage);

    //            //message2.AlternateViews.Add(htmlMail);
    //            //System.Net.Mail.Attachment attachment;
    //            //attachment = new System.Net.Mail.Attachment(filePath);
    //            //message2.Attachments.Add(attachment);

    //            //smtpClient.Host = "relay-hosting.secureserver.net";   //-- Donot change.
    //            smtpClient.Host = "smtp.gmail.com";   //-- Donot change.

    //            smtpClient.Port = 587; //--- Donot change
    //            smtpClient.EnableSsl = true;//--- Donot change
    //            smtpClient.UseDefaultCredentials = false;
    //            smtpClient.Credentials = new System.Net.NetworkCredential("", "");
    //            smtpClient.Send(message2);
    //            //return 1;
    //            #endregion

    //        }
    //    }

    //}

    #endregion

    public class SendEmail
    {
        string _AppName = ConfigurationManager.AppSettings["First_Word_AppName"] + " " + ConfigurationManager.AppSettings["Second_Word_AppName"];

        public void Send(string receiverName, string subject, string ToEmail, string msg, string AttachFileName)
        {
            if (AttachFileName == "")
            {
                #region Send withiout Attachment

                string bodys = @"<html><body><div><p>Hi " + receiverName + @"</p>
                <p style='text-align:justify;'>" + msg + @"
                </p><br>
                <div>
                <span style='border-bottom:1px solid #0070AC; width:100%;Float: left;'><h3 style='border-bottom:2px solid #0070AC; Float:left; margin-bottom:0;'>" + _AppName + @"</h3></span>
                <div style='Float: left !important; margin-right: 10px;background-color:black;width:100%;text-align:center;'>
                </div>
                </div>
                </div></body></html>";

                MailMessage message2 = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                // Enter Email Address here
                MailAddress fromAddress = new MailAddress("", _AppName);
                message2.From = fromAddress;
                message2.To.Add(ToEmail);
                message2.Subject = subject;
                message2.IsBodyHtml = true;
                message2.Body = bodys;
                message2.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                message2.Priority = MailPriority.High;
                AlternateView htmlMail = AlternateView.CreateAlternateViewFromString(bodys, null, "text/html");

                //LinkedResource myimage = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("/Content/images/logo1.png"));
                //myimage.ContentId = "companylogo";
                //htmlMail.LinkedResources.Add(myimage);

                //message2.AlternateViews.Add(htmlMail);
                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(filePath);
                //message2.Attachments.Add(attachment);

                //smtpClient.Host = "relay-hosting.secureserver.net";   //-- Donot change.
                smtpClient.Host = "smtp.gmail.com";   //-- Donot change.

                smtpClient.Port = 587; //--- Donot change
                smtpClient.EnableSsl = true;//--- Donot change
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("", "");
                smtpClient.Send(message2);
                //return 1;
                #endregion

            }
        }
    }
}