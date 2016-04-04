using System;
//using System;
//using System;
//using System.Web;
using System.Web.Mvc;
//using System.Configuration;
//using System.Net;
using System.IO;
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;


using System.Collections.Generic;
using System.Linq;

namespace OmnimarkAmazonWeb.Controllers
{
    class AuthorizeIPAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //Get users IP Address 
            //string ipAddress = filterContext.HttpContext.Request.UserHostAddress;
            //string ipAddress = "192.168.2.159";
            //string hostName = Dns.GetHostName();
            //IPHostEntry ipentry = Dns.GetHostEntry(hostName);
            string ipAddress;
            ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipAddress == "" || ipAddress == null)
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];



            if (!IsIpAddressValid(ipAddress.Trim()))
            {
                //Send back a HTTP Status code of 403 Forbidden  
                filterContext.Result = new HttpStatusCodeResult(403);
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary> 
        /// Compares an IP address to list of valid IP addresses attempting to 
        /// find a match 
        /// </summary> 
        /// <param name="ipAddress">String representation of a valid IP Address</param> 
        /// <returns></returns> 
        public static bool IsIpAddressValid(string ipAddress)
        {
            //Split the users IP address into it's 4 octets (Assumes IPv4) 
            string[] incomingOctets = ipAddress.Trim().Split(new char[] { '.' });

            //Get the valid IP addresses from the web.config 
            string addresses =
              Convert.ToString(ConfigurationManager.AppSettings["AuthorizeIPAddresses"]);

            //Store each valid IP address in a string array 
            string[] validIpAddresses = addresses.Trim().Split(new char[] { ',' });

            //Iterate through each valid IP address 
            foreach (var validIpAddress in validIpAddresses)
            {
                //Return true if valid IP address matches the users 
                if (validIpAddress.Trim() == ipAddress)
                {
                    return true;
                }

                //Split the valid IP address into it's 4 octets 
                string[] validOctets = validIpAddress.Trim().Split(new char[] { '.' });

                bool matches = true;

                //Iterate through each octet 
                for (int index = 0; index < validOctets.Length; index++)
                {
                    //Skip if octet is an asterisk indicating an entire 
                    //subnet range is valid 
                    if (validOctets[index] != "*")
                    {
                        if (validOctets[index] != incomingOctets[index])
                        {
                            matches = false;
                            break; //Break out of loop 
                        }
                    }
                }

                if (matches)
                {
                    return true;
                }
            }

            //Found no matches 
            return false;
        }
    }
}
