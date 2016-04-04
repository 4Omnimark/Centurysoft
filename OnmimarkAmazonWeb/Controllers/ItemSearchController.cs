using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazonWeb.Controllers
{
    public class ItemSearchController : _BaseController
    {
        public ActionResult Index()
        {
            if (Request.Form["Action"] != null && Request.Form["Action"] != "")
            {
                string Action = Request.Form["Action"];

                string[] a = Action.Split('=');

                string[] b = a[0].Split('_');

                string Command = b[0];
                string ASIN = b[1];
                string Value = Request.Form[a[0]];

                KnownASIN rec = db.KnownASINs.Single(ka => ka.ASIN == ASIN);

                if (Command == "Reviewed")
                    if (Value != null)
                        rec.Reviewed = DateTime.Now;
                    else
                        rec.Reviewed = null;

                if (Command == "Filtered")
                    if (Value != null)
                        rec.Filtered = true;
                    else
                        rec.Filtered = false;

                db.SaveChanges();


            }

            //IEnumerable<OmnimarkAmazon.Models.KnownASIN> model = db.KnownASINs.Where(ka => ka.SearchTerm != null && ka.OurProduct == false).OrderBy(ka => ka.SearchTerm);
                IEnumerable<OmnimarkAmazon.Models.KnownASIN> model;
            if (Request.Form["cbShowFiltered"] == null && Request.Form["cbShowReviewed"] == null)
            {
                model = db.KnownASINs.Where(ka => ka.SearchTerm != null && ka.OurProduct == false && ka.Filtered==false && ka.Reviewed==null).OrderBy(ka => ka.SearchTerm).Take(1000);
            }
            else
            {
                model = db.KnownASINs.Where(ka => ka.SearchTerm != null && ka.OurProduct == false).OrderBy(ka => ka.SearchTerm).Take(1000);
                if (Request.Form["cbShowFiltered"] != null)
                    model = model.Where(ka => ka.Filtered == true);

                if (Request.Form["cbShowReviewed"] != null)
                    model = model.Where(ka => ka.Reviewed != null);
            }


            //if (Request.Form["cbShowFiltered"] == null)
            //    model = model.Where(ka => ka.Filtered ==  false);

            //if (Request.Form["cbShowReviewed"] == null)
            //    model = model.Where(ka => ka.Reviewed == null);


            return View(model.ToList());
        }

        public ActionResult AddKnownASIN()
        {
            ViewBag.MarketPlaces = new SelectList(db.Countries.Where(c => c.AmazonMarketPlaceID != null).OrderBy(c => c.CountryName), "AmazonMarketPlaceID", "CountryName");
            return View();
        }
        
        [HttpPost]
        public ActionResult AddKnownASIN(string MarketPlaceID, object FormInput)
        {
            if (Request.Form["ASIN"] != null)
            {
                KnownASIN ka = new KnownASIN();
                ka.ASIN = Request.Form["ASIN"];
                ka.TimeStamp = DateTime.Now;
                ka.Reviewed = DateTime.Now;
                ka.Filtered = false;
                ka.MarketPlaceID = MarketPlaceID;

                db.KnownASINs.Add(ka);
                db.SaveChanges();
            }

            return View("ASINAdded");
        }

    }
}
