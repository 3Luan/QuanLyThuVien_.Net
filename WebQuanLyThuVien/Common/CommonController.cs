using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace WebQuanLyThuVien.Common
{
    public class CommonController : Controller
    {
        // GET: Common
        private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
        private static string Email = ConfigurationManager.AppSettings["Email"];

        public static bool SendEmail(string name, string subject, string content, string toMail)
        {
            bool rs = false;

            try
            {
                MailMessage message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";   // Host name
                    smtp.Port = 587;    // Port number
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(Email, password)
                    {
                        UserName = Email,
                        Password = password
                    };
                }

                MailAddress fromAddress = new MailAddress(Email, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                rs = true;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rs = false;
            }

            return rs;
        }

    }
}