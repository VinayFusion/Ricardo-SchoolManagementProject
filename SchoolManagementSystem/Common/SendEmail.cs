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
        private string _From_EmailAddress = ConfigurationManager.AppSettings["From_EmailAddress"];
        private string _From_EmailPassword = ConfigurationManager.AppSettings["From_EmailPassword"];
        private string _Host_Email = ConfigurationManager.AppSettings["Email_Host"];
        private string _Port_Email = ConfigurationManager.AppSettings["Email_Port"];

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

        public void SendSchoolAccountCreatedEmail(string schoolName, string toEmail, string password)
        {
            string subject = "🎉Welcome to School Attendance System – Your School Account is Ready!";
            string body = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
      <meta charset='UTF-8' />
      <meta name='viewport' content='width=device-width, initial-scale=1' />
      <title>Welcome to School Attendance System</title>
    </head>
    <body style='margin:0;padding:0;background:#eef4f8;font-family:Segoe UI,Tahoma,sans-serif;'>

    <div style='max-width:650px;margin:40px auto;background:#fff;border-radius:16px;box-shadow:0 8px 30px rgba(0, 0, 0, 0.07);overflow:hidden;'>

      <div style='background:linear-gradient(135deg,#13334a,#00a8ff);color:white;padding:40px 30px;text-align:center;'>
        <h1 style='margin:0;font-size:28px;'>🎉 School Account Created</h1>
        <p style='margin-top:10px;font-size:16px;'>Welcome to School Attendance System Platform</p>
      </div>

      <div style='padding:30px;color:#333;'>
        <p style='font-size:16px;line-height:1.6;'>Hello <strong>{schoolName}</strong>,</p>
        <p style='font-size:16px;line-height:1.6;'>Your school account has been successfully created. Use the following credentials to access your dashboard:</p>

        <div style='background:#e8f8ff;border:1px solid #b3e5fc;padding:20px;margin:25px 0;border-radius:10px;'>
          <p style='margin:10px 0;font-size:15px;font-family:monospace;color:#065f8d;'><strong>Email:</strong> {toEmail}</p>
          <p style='margin:10px 0;font-size:15px;font-family:monospace;color:#065f8d;'><strong>Password:</strong> {password}</p>
          <p style='margin:10px 0;font-size:15px;font-family:monospace;color:#065f8d;'><strong>Login URL:</strong>http://localhost:65060/Login/Index</p>
        </div>

        <div style='text-align:center;margin:35px 0 25px;'>
          <a href='http://localhost:65060/Login/Index' style='display:inline-block;background:#13334a;color:white;padding:14px 32px;font-size:16px;font-weight:500;text-decoration:none;border-radius:8px;transition:background 0.3s ease;box-shadow:0 4px 12px rgba(52, 172, 224, 0.4);' target='_blank'>Login to Your Dashboard</a>
        </div>

        <p>If you face any issues, feel free to contact our support team.</p>
        <p>Regards,<br><strong>EduTrack Team</strong></p>
      </div>

      <div style='text-align:center;font-size:13px;color:#777;padding:20px;background:#f4f6f8;'>
        © 2025 EduTrack | <a href='http://localhost:65060/Login/Index' style='color:#13334a;text-decoration:none;'>Visit Website</a>
      </div>

    </div>
    </body>
    </html>";

            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            MailAddress fromAddress = new MailAddress(_From_EmailAddress, _AppName);
            message.From = fromAddress;
            message.To.Add(toEmail);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Priority = MailPriority.High;
            AlternateView htmlMail = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

            smtpClient.Host = _Host_Email;
            smtpClient.Port = 25;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(_From_EmailAddress, _From_EmailPassword);

            smtpClient.Send(message);
        }

    }
}