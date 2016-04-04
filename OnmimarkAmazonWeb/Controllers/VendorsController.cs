using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazonWeb.Controllers
{
    public class VendorsController : _BaseController
    {
        public ActionResult Index()
        {
            return View(db.Vendors.OrderBy(v => v.Name));
        }

        void LoadListBoxes(Nullable<int> StateID, Nullable<int> CountryID)
        {
            ViewBag.StateSelectList = new SelectList(db.States.OrderBy(s => s.StateName), "FIPS", "StateName", StateID);
            ViewBag.CountrySelectList = new SelectList(db.Countries.OrderBy(c => c.CountryName), "Code", "CountryName", CountryID);
            ViewBag.SuppliersSelectList = new SelectList(db.suppliers.OrderBy(s => s.Company), "ID", "Company");
        }

        public ActionResult GetSupplier(Guid id)
        {
            supplier supplier = db.suppliers.Single(s => s.ID == id);

            int CountryID = db.Countries.Single(c => c.C2dig == supplier.TLD).Code;

            int StateID = 0;

            if (CountryID == 840)
                StateID = db.States.Single(s => s.Code == supplier.State).FIPS;

            return Json(new { supplier = supplier, CountryID = CountryID, StateID = StateID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VendorDefault(Vendor Vendor)
        {
            LoadListBoxes(Vendor.StateID, Vendor.CountryID);

            return View("Vendor", Vendor);
        }

        public ActionResult Create()
        {
            Vendor Vendor = new Vendor();

            InitViewBag(ActionType.Create, "Vendor");

            return VendorDefault(Vendor);
        }

        public ActionResult Edit(Guid id)
        {
            Vendor Vendor = db.Vendors.Single(v => v.ID == id);

            InitViewBag(ActionType.Edit, "Vendor", null, Vendor.Name);

            return VendorDefault(Vendor);
        }

        public ActionResult Details(Guid id)
        {
            Vendor Vendor = db.Vendors.Single(v => v.ID == id);

            InitViewBag(ActionType.Details, "Vendor", null, Vendor.Name);

            return VendorDefault(Vendor);
        }

        public ActionResult Delete(Guid id)
        {
            Vendor Vendor = db.Vendors.Single(v => v.ID == id);

            InitViewBag(ActionType.Delete, "Vendor", null, Vendor.Name);

            return VendorDefault(Vendor);
        }

        [HttpPost]
        public ActionResult Create(Vendor Rec)
        {
            if (ModelState.IsValid)
            {

                Rec.ID = Guid.NewGuid();
                Rec.TimeStamp = DateTime.Now;

                ToProper(Rec);

                db.Vendors.Add(Rec);

                db.SaveChanges();

                return BackToList();
            }
            else
            {
                InitViewBag(ActionType.Create, "Vendor");

                return VendorDefault(Rec);

            }
        }

        [HttpPost]
        public ActionResult Edit(Guid id, Vendor Rec)
        {
            if (ModelState.IsValid)
            {

                Vendor existing_vendor = db.Vendors.Single(v => v.ID == id);

                ToProper(Rec);

                UpdateModel(existing_vendor);

                db.SaveChanges();

                return BackToList();
            }
            else
            {
                InitViewBag(ActionType.Edit, "Vendor", null, Rec.Name);

                return VendorDefault(Rec);

            }
        }

        [HttpPost]
        public ActionResult Details(Guid id, Vendor Rec)
        {
            return BackToList();
        }

        [HttpPost]
        public ActionResult Delete(Guid id, Vendor Rec)
        {
            db.Vendors.Remove(db.Vendors.Single(v => v.ID == id));
            db.SaveChanges();

            return BackToList();
        }

        ActionResult BackToList()
        {
            return Redirect("/Vendors");
        }

        void ToProper(Vendor Rec)
        {
            Rec.ContactNameFirst = Startbutton.Library.ToProper(Rec.ContactNameFirst);
            Rec.ContactNameMiddle = Startbutton.Library.ToProper(Rec.ContactNameMiddle);
            Rec.ContactNameLast = Startbutton.Library.ToProper(Rec.ContactNameLast);
            Rec.Name = Startbutton.Library.ToProper(Rec.Name);
        }
    }
}
