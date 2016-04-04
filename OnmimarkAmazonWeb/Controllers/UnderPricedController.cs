using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OmnimarkAmazonWeb.Controllers
{
    public class UnderPricedController : _BaseController
    {

        public ActionResult Index()
        {

            var model = db.UnderPricedView.OrderBy(upv => upv.ASIN).ThenBy(upv => upv.StoreName);

            return View(model);

        }

        public ActionResult HideUntilUpdate(string ASIN, Guid AcctID)
        {
            db.AmazonInventories.Single(ai => ai.ASIN == ASIN && ai.AmazonAccountID == AcctID).UnderPricedIgnoreUntilUpdate = DateTime.Now;
            db.SaveChanges();

            return Content("1");
        }

        public ActionResult HideFor30Days(string ASIN, Guid AcctID)
        {
            db.AmazonInventories.Single(ai => ai.ASIN == ASIN && ai.AmazonAccountID == AcctID).UnderPricedIgnoreUntil = DateTime.Now.AddDays(30);
            db.SaveChanges();

            return Content("1");
        }

    }

}
