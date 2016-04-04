using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazonWeb.Controllers
{
    public class SearchTermsController : _BaseController
    {
        private string _ViewPath = "~/Views/SearchTerms/";
        private string _Layout = "~/Views/Shared/_Layout.cshtml";

        public ActionResult Index()
        {
            var qry = db.SearchTerms.OrderBy(st => st.Term);

            return View(qry);
        }

        public ActionResult Create()
        {
            InitViewBag(ActionType.Create, "Search Term");
            return View(_ViewPath + "Edit.cshtml", _Layout, new SearchTerm());
        }

        [HttpPost]
        public ActionResult Create(SearchTerm Rec)
        {
            if (ModelState.IsValid)
            {
                Rec.TimeStamp = DateTime.Now;
                Rec.ID = Guid.NewGuid();
                db.SearchTerms.Add(Rec);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                InitViewBag(ActionType.Create, "Search Term");
                return View(_ViewPath + "Edit.cshtml", _Layout, new SearchTerm());
            }
        }

        public ActionResult Delete(Guid id)
        {
            var qry = db.SearchTerms.Single(st => st.ID == id);

            return View(qry);
        }

        [HttpPost]
        public ActionResult Delete(SearchTerm Rec)
        {
            var qry = db.SearchTerms.Single(st => st.ID == Rec.ID);
            db.SearchTerms.Remove(qry);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
