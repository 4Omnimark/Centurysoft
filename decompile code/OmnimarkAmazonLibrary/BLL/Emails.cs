using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazon.BLL
{
    public static class Emails
    {
        public static bool Send(Entities db, string EmailURL, string Subject, Guid UserID, bool SendAsync, ref Exception Ex)
        {
            User User = db.Users.Single(u => u.UserID == UserID);
            return Send(EmailURL, Subject, User.aspnet_Users.aspnet_Membership.Email, User, SendAsync, ref Ex);
        }

        public static bool Send(string EmailURL, string Subject, string Email, User User, bool SendAsync, ref Exception Ex)
        {
            bool rtn = false;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(EmailURL);

            request.CookieContainer = new CookieContainer();
            //request.CookieContainer.Add(Startbutton.Library.GetCookieCollectionFromRequest(ControllerContext.HttpContext));
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception Exx)
            {
                throw (new Exception("EMAIL GEN ERROR: " + Exx.Message + " ON:\n\n" + EmailURL));
            }

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string body = reader.ReadToEnd();

            //PreMailer.Net.PreMailer pm = new PreMailer.Net.PreMailer();
            //body = pm.MoveCssInline(body, true);

            MailMessage msg = null;

            if (User != null)
                msg = new MailMessage(new MailAddress("automailer@amazon.revupcommerce.com", "RevUpCommerce Automailer"), new MailAddress(Email, User.NameFirst + " " + User.NameLast));
            else
            {
                msg = new MailMessage();
                msg.From = new MailAddress("automailer@amazon.revupcommerce.com", "RevUpCommerce Automailer");

                string[] a = Email.Split(';');

                for (int x = 0; x < a.Length; x++)
                    msg.To.Add(new MailAddress(a[x]));
            }

            msg.Subject = Subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            if (ConfigurationManager.AppSettings["BCCAddress"] != null)
                msg.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["BCCAddress"]));

            if (Ex == null)
                Ex = new Exception();

            rtn = Startbutton.Library.SendMail(msg, ref Ex, SendAsync);

            return rtn;


        }

    }
}