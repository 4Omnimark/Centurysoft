using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceProcess;
using Startbutton.Web.HtmlHelperExtensions;

namespace OmnimarkAmazonWeb.Controllers
{
    public class HomeController : _BaseController
    {
        public ActionResult Index()
        {
            ViewBag.LastKnownOrderPurchaseDate = db.AmazonOrders.Max(ao => ao.PurchaseDate).ToString("M/d/yyyy HH:mm:ss");
            UkListing.UKOmnimarkEntities uk =new UkListing.UKOmnimarkEntities();
            ViewBag.UKProhibition = uk.ServiceStatus.Where(x => x.ServiceName == "ProhibitedService").Select(x => x.LastUpdatedTimestamp).SingleOrDefault();
            ServiceController sc = new ServiceController("OmnimarkAmazonService");

          
            try
            {
                ViewBag.ServiceStatus = sc.Status.ToString();
            }
            catch (Exception Ex)
            {
                ViewBag.ServiceStatus = "Service Not Found!";
            }

            ViewBag.AmazonAccounts = db.AmazonAccounts.OrderBy(ao => ao.Name);

            return View();
        }

        public ActionResult RunTool(string id, string ru, string parms)
        {
            ViewBag.ToolName = id;
            ViewBag.ReturnURL = ru;
            ViewBag.ToolPath = HtmlHelper.GetAppPath() + "/Tools/" + id + ".aspx";

            if (parms != null)
                ViewBag.ToolPath += "?" + parms;
            
            return View();
        }

        public ActionResult TriggerError()
        {
            var x = db.KnownASINs.Single(ka => ka.ASIN == "sdfgasdfg");

            return View();
        }

    }
}
