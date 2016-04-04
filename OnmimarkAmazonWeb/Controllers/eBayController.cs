using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;
using Startbutton.ExtensionMethods;
using Startbutton.Web.ExtensionMethods;
using System.Dynamic;
using System.Web.Routing;
using System.Text;
using Monty.Linq;

namespace OmnimarkAmazonWeb.Controllers
{
    public class eBayController : _BaseController
    {

        public ActionResult Products(string id)
        {

            if (id == null)
            {
                var model = db.eBayItems.OrderBy(ei => ei.Title);
                
                return View("eBayItemsList", model);
            }
            else
            {

                eBayItem ei = db.eBayItems.Single(kax => kax.ItemID == id);

                ViewBag.Title = "Inventory Maintenance for " + ei.Title;
                ViewBag.AssociatedProducts = db.eBayItemsProducts.Where(aip => aip.ItemID == id);
                ViewBag.ItemID = id;

                return View(ei);
            }
        }

        public ActionResult CreateAssociatedProduct(string ItemID)
        {
            eBayItem ei = db.eBayItems.Single(kax => kax.ItemID == ItemID);

            ViewBag.Title = "Create Associated Products for " + ei.Title;
            ViewBag.eBayItem = ei;

            return View("AssociatedProduct", new eBayItemsProduct());
        }

        [HttpPost]
        public ActionResult CreateAssociatedProduct(string ItemID, eBayItemsProduct NewRec)
        {
            if (ModelState.IsValid)
            {
                NewRec.ItemID = ItemID;
                NewRec.TimeStamp = DateTime.Now;
                db.eBayItemsProducts.Add(NewRec);
                db.SaveChanges();

                return RedirectToAction("Products", new { id = ItemID });
            }

            eBayItem ei = db.eBayItems.Single(kax => kax.ItemID == ItemID);

            ViewBag.Title = "Create Associated Products for " + ei.Title;
            ViewBag.eBayItem = ei;

            return View("AssociatedProduct", NewRec);
        }

        public ActionResult EditAssociatedProduct(string ItemID, Guid ProductID)
        {
            eBayItem ei = db.eBayItems.Single(kax => kax.ItemID == ItemID);

            ViewBag.Title = "Edit Associated Products for " + ei.Title;
            ViewBag.eBayItem = ei;

            eBayItemsProduct eip = db.eBayItemsProducts.Single(p => p.ItemID == ItemID && p.ProductID == ProductID);

            ViewBag.ProductName = eip.Product.Name;

            return View("AssociatedProduct", eip);
        }

        [HttpPost]
        public ActionResult EditAssociatedProduct(string ItemID, Guid ProductID, AmazonInventoryProduct Rec)
        {
            ProductID = Guid.Parse(Request.QueryString["ProductID"]);

            eBayItemsProduct aip = db.eBayItemsProducts.Single(p => p.ItemID == ItemID && p.ProductID == ProductID);

            if (ModelState.IsValid)
            {
                UpdateModel(aip);
                db.SaveChanges();

                return RedirectToAction("Products", new { id = ItemID });
            }

            eBayItem ei = db.eBayItems.Single(kax => kax.ItemID == ItemID);

            ViewBag.Title = "Edit Associated Products for " + ei.Title;
            ViewBag.eBayItem = ei;

            ViewBag.ProductName = db.eBayItemsProducts.Single(p => p.ItemID == ItemID && p.ProductID == Rec.ProductID).Product.Name;

            return View("AssociatedProduct", Rec);
        }

        public ActionResult DeleteAssociatedProduct(string ItemID, Guid ProductID)
        {
            db.eBayItemsProducts.Remove(db.eBayItemsProducts.Single(p => p.ItemID == ItemID && p.ProductID == ProductID));
            db.SaveChanges();

            return RedirectToAction("Products", new { id = ItemID });
        }

    }

}
