using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazonWeb.Controllers
{
    public class ProductsController : _BaseController
    {

        public static MvcHtmlString GetCategoryCheckBoxAttributes(ProductCategoryListing pcl)
        {
            return new MvcHtmlString(@" type=""checkbox"" id=""Category_" + pcl.ID.ToString() + @""" name=""Category_" + pcl.ID.ToString() + @""" " + (pcl.Value ? "checked" : ""));

        }

        public static MvcHtmlString GetTagCheckBoxAttributes(ProductTagListing ptl)
        {
            return new MvcHtmlString(@" type=""checkbox"" id=""Tag_" + ptl.ID.ToString() + @""" name=""Tag_" + ptl.ID.ToString() + @""" " + (ptl.Value ? "checked" : ""));

        }

        Type RecType = typeof(Product);

        public ActionResult Index()
        {
            InitViewBag(ActionType.List, RecType);

            return View("Index", db.ProductsView.OrderBy(p => p.Name).ToList());
        }

        public ActionResult Create()
        {
            InitViewBag(ActionType.Create, RecType);

            ViewBag.Categories = ProductCategoryListing.GetList(db);
            ViewBag.Tags = ProductTagListing.GetList(db);
            ViewBag.Vendors = VendorListing.GetList(db, null);

            return View("Edit", new ProductView());
        }

        public ActionResult Edit(Guid id)
        {
            InitViewBag(ActionType.Edit, RecType);

            var Rec = db.ProductView.Single(p => p.ID == id);

            ViewBag.Categories = ProductCategoryListing.GetList(Rec.Categories);
            ViewBag.Tags = ProductTagListing.GetList(Rec.Tags);
            ViewBag.Vendors = VendorListing.GetList(db, id);

            return View("Edit", Rec);
        }

        public ActionResult Delete(Guid id)
        {
            InitViewBag(ActionType.Delete, RecType);

            return View(db.Products.Single(p => p.ID == id));
        }

        [HttpPost]
        public ActionResult Create(ProductView Rec)
        {
            if (ModelState.IsValid)
            {
                Product NewRec = new Product();

                Startbutton.Library.SetMatchingMembers(NewRec, Rec);

                NewRec.ID = Guid.NewGuid();
                NewRec.TimeStamp = DateTime.Now;

                db.Products.Add(NewRec);

                ProductCategoryListing.SaveFormValuesToDB(db, NewRec, Request.Form);
                ProductTagListing.SaveFormValuesToDB(db, NewRec, Request.Form);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {

                InitViewBag(ActionType.Create, RecType);

                ViewBag.Categories = ProductCategoryListing.GetList(db, Request.Form);
                ViewBag.Tags = ProductTagListing.GetList(db, Request.Form);
                ViewBag.Vendors = VendorListing.GetList(db, null, Request.Form);

                return View("Edit", Rec);
            }
        }

        [HttpPost]
        public ActionResult Edit(Guid id, ProductView Rec)
        {
            if (ModelState.IsValid)
            {
                Product ExistingRec = db.Products.Single(p => p.ID == id);

                UpdateModel(ExistingRec);

                ProductCategoryListing.SaveFormValuesToDB(db, ExistingRec, Request.Form);
                ProductTagListing.SaveFormValuesToDB(db, ExistingRec, Request.Form);
                VendorListing.SaveFormValuesToDB(db, ExistingRec, Request.Form);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Categories = ProductCategoryListing.GetList(db, Request.Form);
                ViewBag.Tags = ProductTagListing.GetList(db, Request.Form);
                ViewBag.Vendors = VendorListing.GetList(db, id, Request.Form);


                return View("Edit", Rec);
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id, object x)
        {
            Product ToDelete = db.Products.Single(p => p.ID == id);

            ToDelete.Categories.Clear();
            ToDelete.Tags.Clear();

            db.Products.Remove(ToDelete);
            db.SaveChanges();

            return RedirectToAction("Index");


        }

        public ActionResult CheckPastPurchaseOrders(Guid ProductID, Guid VendorID)
        {
            var model = db.PurchaseOrderProductsVendors.Where(p => p.ProductID == ProductID && p.VendorID == VendorID).Select(p => new { p.POID, p.PONumber }).Distinct();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }

}
