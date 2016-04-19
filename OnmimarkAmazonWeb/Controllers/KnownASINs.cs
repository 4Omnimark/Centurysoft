using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazonWeb.Controllers
{
    public class KnownASINsController : _BaseController
    {

        Type RecType = typeof(KnownASIN);

        public ActionResult Index()
        {
            InitViewBag(ActionType.List, RecType);

            return View("Index", db.KnownASINs.OrderBy(ka => ka.Title).ToList());
        }

        public ActionResult Create()
        {
            InitViewBag(ActionType.Create, RecType);

            return View("Edit", new KnownASIN());
        }

        public ActionResult Edit(string ASIN)
        {
            InitViewBag(ActionType.Edit, RecType);

            return View("Edit", db.KnownASINs.Single(ka => ka.ASIN == ASIN));
        }

        public ActionResult Delete(string ASIN)
        {
            InitViewBag(ActionType.Delete, RecType);

            return View(db.KnownASINs.Single(ka => ka.ASIN == ASIN));
        }

        [HttpPost]
        public ActionResult Create(KnownASIN NewRec)
        {
            if (ModelState.IsValid)
            {
                NewRec.TimeStamp = DateTime.Now;
                
                db.KnownASINs.Add(NewRec);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {

                InitViewBag(ActionType.Create, RecType);

                return View("Edit", NewRec);
            }
        }

        [HttpPost]
        public ActionResult Edit(string ASIN, Product Rec)
        {
            if (ModelState.IsValid)
            {
                UpdateModel(db.KnownASINs.Single(ka => ka.ASIN == ASIN));

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                InitViewBag(ActionType.Edit, RecType);

                return View("Edit", Rec);
            }
        }

        [HttpPost]
        public ActionResult Delete(string ASIN, object x)
        {
            db.KnownASINs.Remove(db.KnownASINs.Single(ka => ka.ASIN == ASIN));
            db.SaveChanges();

            return RedirectToAction("Index");


        }
    }

}
