using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Web.Mvc;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazon.BLL
{
    public static class WebEmails
    {
        public static bool Send(Entities db, string ViewPath, object Model, ControllerContext Context, string Subject, Guid UserID, bool SendAsync, ref Exception Ex)
        {
            User User = db.Users.Single(u => u.UserID == UserID);
            return Send(ViewPath, Model, Context, Subject, User.aspnet_Users.aspnet_Membership.Email, User, SendAsync, ref Ex);
        }

        public static bool Send(string ViewPath, object Model, ControllerContext Context, string Subject, string Email, User User, bool SendAsync, ref Exception Ex)
        {
            SmtpClient SmtpServer = new SmtpClient("dedrelay.secureserver.net");

            bool rtn = false;

            string body = Startbutton.Web.Library.RenderViewToString(ViewPath, "_LayoutForEmail", Model, Context, false);

            MailMessage msg = null;

            if (User != null)
                msg = new MailMessage(new MailAddress("systememail@revupcommerce.com", "RevUpCommerce Automailer"), new MailAddress(Email, User.NameFirst + " " + User.NameLast));
            else
            {
                msg = new MailMessage();
                msg.From = new MailAddress("systememail@revupcommerce.com", "RevUpCommerce Automailer");

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
            //SmtpServer.Port = 25;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("systememail@revupcommerce.com", "D8e#37!");
            //SmtpServer.EnableSsl = false;
            //SmtpServer.Send(msg);
           rtn = Startbutton.Library.SendMail(msg, ref Ex, SendAsync);

            return rtn;

        }

    }

}