using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;
using Startbutton.ExtensionMethods;
using MvcPaging;
using System.Configuration;
using OmnimarkAmazon;

namespace OmnimarkAmazonWeb.Controllers
{

    public class PurchasingController : _BaseController
    {
        const int NUM_PRODUCTS = 70;

        #region Receiving

        public ActionResult Received(int? Page, string SortBy, bool? SortDescending, Nullable<Guid> ProductID)
        {
            ViewBag.Title = "Received Products";

            IQueryable<ReceivedProduct> model = null;

            if (ProductID == null)
                model = db.ReceivedProducts.OrderByDescending(rp => rp.ReceivedShipment.Date).ThenByDescending(rp => rp.TimeStamp);
            else
                model = db.ReceivedProducts.Where(rp => rp.ProductID == ProductID).OrderByDescending(rp => rp.ReceivedShipment.Date).ThenByDescending(rp => rp.TimeStamp);

            int currentPageIndex = Page.HasValue ? Page.Value - 1 : 0;
            var option = new PagingOption { Page = currentPageIndex, PageSize = 15, SortBy = SortBy, SortDescending = SortDescending };

            return View(model.ToPagedList(option));
        }

        void ReceivedShipmentDropDowns(string SelectedStatus)
        {
            
            ViewBag.ReceivedStatus = new SelectList(db.RecievedShipmentStatuses.OrderBy(s => s.DisplaySeq).ThenBy(s => s.Name), "ID", "Name", SelectedStatus);
        }


        void FillReceivedShipmentLines(List<ReceivedProduct> rslines)
        {
            while (rslines.Count < NUM_PRODUCTS)
                rslines.Add(new ReceivedProduct());
         
        }

        List<ReceivedProduct> ValidateReceivedShipment(ReceivedShipment Rec)
        {
            List<Guid> lines_to_delete = new List<Guid>();
            return ValidateReceivedShipment(Rec, ref lines_to_delete);
        }

        List<ReceivedProduct> ValidateReceivedShipment(ReceivedShipment Rec, ref List<Guid> lines_to_delete)
        {

            List<ReceivedProduct> ReceivedProducts = new List<ReceivedProduct>();

            for (int x = 0; x < NUM_PRODUCTS; x++)
            {
                ReceivedProduct rp = new ReceivedProduct();

                if (!Request.Form["ReceivedProductSearch" + x.ToString()].IsNullOrNullString())
                {
                    if (Request.Form["ProductID" + x.ToString()].IsNullOrNullString())
                        ModelState.AddModelError("ReceivedProductSearch" + x.ToString(), "Unrecognized Product");
                    else
                        rp.ProductID = Guid.Parse(Request.Form["ProductID" + x.ToString()]);

                    if (Request.Form["ReceivedProductQty" + x.ToString()].IsNullOrNullString())
                        ModelState.AddModelError("ReceivedProductQty" + x.ToString(), "Invalid Quantity");

                    if (!Request.Form["ReceivedProductID" + x.ToString()].IsNullOrNullString())
                        rp.ID = Guid.Parse(Request.Form["ReceivedProductID" + x.ToString()]);
                  
                }
                else // no product - check for deleted line item
                    if (!Request.Form["ReceivedProductID" + x.ToString()].IsNullOrNullString())
                    {
                        Guid id = Guid.Parse(Request.Form["ReceivedProductID" + x.ToString()]);

                        if (id != Guid.Empty)
                            lines_to_delete.Add(id);
                    }

                if (!Request.Form["ReceivedProductQty" + x.ToString()].IsNullOrNullString())
                {
                    decimal Qty = 0;

                    if (!decimal.TryParse(Request.Form["ReceivedProductQty" + x.ToString()], out Qty))
                        ModelState.AddModelError("ReceivedProductQty" + x.ToString(), "Invalid Quantity");
                    else if (Qty == 0)
                        ModelState.AddModelError("ReceivedProductQty" + x.ToString(), "Invalid Quantity");
                    else
                        rp.Qty = Qty;
                }

                rp.TrackingNumber = Request.Form["TrackingNumber" + x.ToString()];

                ReceivedProducts.Add(rp);
            }

            if (ReceivedProducts.Count(rp => rp.ProductID != Guid.Empty) == 0)
                ModelState.AddModelError("", "No Line Items Entered!");

            return ReceivedProducts;
        }

        ReceivedShipment InitReceivedShipmentView(Startbutton.Web._BaseController.ActionType at, Guid id)
        {
            ReceivedShipment existing_shipment = db.ReceivedShipments.Single(rs => rs.ID == id);
            //ReceivedProduct existing_Product = db.ReceivedProducts.Single(rp => rp.ReceivedShipmentID == id);
            InitViewBag(at, "Received Shipment");

            List<ReceivedProduct> products = db.ReceivedProducts.Where(rp => rp.ReceivedShipmentID == id).ToList();
            FillReceivedShipmentLines(products);
            foreach(var pr in products)
            { 
           
               ReceivedShipmentDropDowns(pr.Status);
            }
            ViewBag.ReceivedProducts = products;

            return existing_shipment;
        }


        ActionResult ReceivedShipmentDefault(Startbutton.Web._BaseController.ActionType at, Guid id)
        {
            ReceivedShipment existing_shipment = InitReceivedShipmentView(at, id);
            return View("ReceivedShipment", existing_shipment);
        }

        public ActionResult CreateReceivedShipment()
        {
            InitViewBag(ActionType.Create, "Received Shipment");

            ReceivedShipmentDropDowns("S"); 
            ViewBag.ReceivedProducts = new List<ReceivedProduct>();
            FillReceivedShipmentLines(ViewBag.ReceivedProducts);

            ReceivedShipment model = new ReceivedShipment();
            model.Date = DateTime.Today;

            return View("ReceivedShipment", model);
        }

        public ActionResult EditReceivedShipment(Guid id)
        {
            return ReceivedShipmentDefault(ActionType.Edit, id);
        }

        [HttpPost]
        public ActionResult CreateReceivedShipment(ReceivedShipment Rec,ReceivedProduct rpd)
        {
            List<ReceivedProduct> ReceivedProducts = ValidateReceivedShipment(Rec);

            if (ModelState.IsValid)
            {
                Rec.ID = Guid.NewGuid();
                Rec.TimeStamp = DateTime.Now;

                db.ReceivedShipments.Add(Rec);

                foreach (ReceivedProduct rp in ReceivedProducts)
                {
                    if (rp.ProductID != Guid.Empty)
                    {
                        rp.ReceivedShipmentID = Rec.ID;
                        rp.ID = Guid.NewGuid();
                        rp.TimeStamp = Rec.TimeStamp;
                        rp.Status = rpd.Status;
                        db.ReceivedProducts.Add(rp);
                  
                     if(rp.Status!="D")
                     { 
                        ProductInventory pi = db.ProductInventories.Where(pix => pix.LocationID == Library.OrlandoLocationID && pix.ProductID == rp.ProductID ).FirstOrDefault();

                        if (pi == null)
                        {
                            pi = db.ProductInventories.Local.Where(pix => pix.LocationID == Library.OrlandoLocationID && pix.ProductID == rp.ProductID ).FirstOrDefault();

                            if (pi == null)
                            {
                                pi = new ProductInventory();
                                pi.LocationID = Library.OrlandoLocationID;
                                pi.ProductID = rp.ProductID;
                                pi.TimeStamp = DateTime.Now;
                                pi.UpdateTimeStamp = DateTime.Now;
                                pi.Qty = 0;
                                db.ProductInventories.Add(pi);
                            }
                        }

                        ProductInventoryAdjustment pia = new ProductInventoryAdjustment();
                        pia.ID = Guid.NewGuid();
                        pia.ProductID = rp.ProductID;
                        pia.LocationID = Library.OrlandoLocationID;
                        pia.ReasonID = Library.ItemReceivedReasonID;
                        pia.ReasonRef1 = rp.ID.ToString();
                        pia.ReasonRef2 = Rec.ID.ToString();
                        pia.TimeStamp = DateTime.Now;
                        pia.AdjustmentAmount = rp.Qty;
                        pia.OldQty = pi.Qty;
                        pia.NewQty = pi.Qty + rp.Qty;

                        db.ProductInventoryAdjustments.Add(pia);

                        pi.Qty += rp.Qty;
                     }

                    }

                }

                db.SaveChanges();

                ReceivedShipment existing_shipment = InitReceivedShipmentView(ActionType.DetailsForEmail, Rec.ID);

                Exception Ex = null;
                if (!OmnimarkAmazon.BLL.WebEmails.Send("/Views/Purchasing/ReceivedShipment.cshtml", Rec, this.ControllerContext, "Shipment Received", ConfigurationManager.AppSettings["NewReceivedShipmentNotificationList"], null, false, ref Ex))
                    return Redirect("/Purchasing/Received?errmsg=" + "Email Failed! " + Ex.Message);
                else
                    return Redirect("/Purchasing/Received");


            }
            else
            {
                InitViewBag(ActionType.Create, "Received Shipment");

                ReceivedShipmentDropDowns(rpd.Status);
                ViewBag.ReceivedProducts = ReceivedProducts;
                
                FillReceivedShipmentLines(ReceivedProducts);

                return View("ReceivedShipment", Rec);
            }
        }

        [HttpPost]
        public ActionResult EditReceivedShipment(Guid id, ReceivedShipment Rec,ReceivedProduct rpd)
        {
            List<Guid> lines_to_delete = new List<Guid>();

            List<ReceivedProduct> ReceivedProducts = ValidateReceivedShipment(Rec, ref lines_to_delete);

            if (ModelState.IsValid)
            {

                #region Delete Records
                if (lines_to_delete.Count > 0)
                {

                    foreach (Guid rp_id in lines_to_delete)
                        db.ReceivedProducts.Remove(db.ReceivedProducts.Single(rp => rp.ID == rp_id));

                    db.SaveChanges();

                }
                #endregion

                #region Update Records
                UpdateModel(db.ReceivedShipments.Single(rs => rs.ID == id));

                foreach (ReceivedProduct rp in ReceivedProducts)
                {
                    if (rp.ProductID != Guid.Empty)
                    {
                        if (rp.ID == Guid.Empty)
                        {
                            rp.ReceivedShipmentID = Rec.ID;
                            rp.ID = Guid.NewGuid();
                            rp.TimeStamp = DateTime.Now;
                            rp.Status = rpd.Status;
                            db.ReceivedProducts.Add(rp);
                        }
                        else
                        {
                            ReceivedProduct existing_rp = db.ReceivedProducts.Single(rpx => rpx.ID == rp.ID);
                            existing_rp.ProductID = rp.ProductID;
                            existing_rp.Qty = rp.Qty;
                            existing_rp.Status = rpd.Status;
                            existing_rp.TrackingNumber = rp.TrackingNumber;
                        }

                    }
                }
                #endregion

                db.SaveChanges();

                return Redirect("/Purchasing/Received");
            }
            else
            {
                InitViewBag(ActionType.Edit, "Received Shipment");

                ViewBag.ReceivedProducts = ReceivedProducts;
                FillReceivedShipmentLines(ViewBag.ReceivedProducts);

                return View("ReceivedShipment", Rec);
            }
        }

        public ActionResult ViewReceivedShipment(Guid id)
        {
            if (Request.HttpMethod == "POST")
                return Redirect("/Purchasing/Received");
            else
                return ReceivedShipmentDefault(ActionType.Details, id);
        }

        public ActionResult DeleteReceivedShipment(Guid id)
        {
            if (Request.HttpMethod == "POST")
            {
                List<ReceivedProduct> products_to_delete = db.ReceivedProducts.Where(rp => rp.ReceivedShipmentID == id).ToList();

                foreach (ReceivedProduct product in products_to_delete)
                {
                    ProductInventory pi = db.ProductInventories.Where(pix => pix.LocationID == Library.OrlandoLocationID && pix.ProductID == product.ProductID).FirstOrDefault();

                    if (pi == null)
                    {
                        pi = new ProductInventory();
                        pi.LocationID = Library.OrlandoLocationID;
                        pi.ProductID = product.ProductID;
                        pi.TimeStamp = DateTime.Now;
                        pi.Qty = 0;
                        db.ProductInventories.Add(pi);
                    }

                    ProductInventoryAdjustment pia = new ProductInventoryAdjustment();
                    pia.ID = Guid.NewGuid();
                    pia.ProductID = product.ProductID;
                    pia.LocationID = Library.OrlandoLocationID;
                    pia.ReasonID = Library.ItemReceiptDeletedReasonID;
                    pia.ReasonRef1 = product.ID.ToString();
                    pia.ReasonRef2 = product.ReceivedShipmentID.ToString();
                    pia.TimeStamp = DateTime.Now;
                    pia.AdjustmentAmount = -product.Qty;
                    pia.OldQty = pi.Qty;
                    pia.NewQty = pi.Qty - product.Qty;

                    db.ProductInventoryAdjustments.Add(pia);

                    pi.Qty -= product.Qty;

                    db.ReceivedProducts.Remove(product);

                }

                db.ReceivedShipments.Remove(db.ReceivedShipments.Single(rs => rs.ID == id));

                db.SaveChanges();

                return Redirect("/Purchasing/Received");
            }
            else
                return ReceivedShipmentDefault(ActionType.Delete, id);
        }

        #endregion

        #region Purchase Orders

        public ActionResult PurchaseOrders(int? Page, string SortBy, bool? SortDescending, Nullable<Guid> ProductID, Nullable<Guid> VendorID, Nullable<bool> ShowClosed, Nullable<bool> ShowUnConfirmed)
        {
            ViewBag.Title = "Purchase Orders";

            IQueryable<PurchaseOrder> model;

            if (ProductID == null)
                model = db.PurchaseOrders.OrderBy(po => po.PurchaseOrderStatus.DisplaySeq).ThenByDescending(po => po.Date).ThenByDescending(po => po.TimeStamp);
            else
                model = db.PurchaseOrders.Where(po => po.Lines.Count(pol => pol.ProductID == ProductID) > 0).OrderBy(po => po.PurchaseOrderStatus.DisplaySeq).ThenByDescending(po => po.Date).ThenByDescending(po => po.TimeStamp);

            if (!(ShowClosed != null && (bool)ShowClosed))
                model = model.Where(po => po.Status != "C");

            if (ShowUnConfirmed != null && (bool)ShowUnConfirmed)
                model = model.Where(po => po.CStatus == "UN ");
            

            if (VendorID != null)
                model = model.Where(po => po.VendorID == VendorID);

            int currentPageIndex = Page.HasValue ? Page.Value - 1 : 0;
            var option = new PagingOption { Page = currentPageIndex, PageSize = 50, SortBy = SortBy, SortDescending = SortDescending };

            return View(model.ToPagedList(option));
        }

        List<PurchaseOrderLine> ValidatePurchaseOrder(PurchaseOrder Rec)
        {
            List<Guid> lines_to_delete = new List<Guid>();
            return ValidatePurchaseOrder(Rec, ref lines_to_delete);
        }

        List<PurchaseOrderLine> ValidatePurchaseOrder(PurchaseOrder Rec, ref List<Guid> lines_to_delete)
        {
            List<PurchaseOrderLine> polines = new List<PurchaseOrderLine>();

            if (Rec.VendorID == Guid.Empty)
                ModelState.AddModelError("VendorID", "Vendor required");

            for (int x = 0; x < NUM_PRODUCTS; x++)
            {
                PurchaseOrderLine poline = new PurchaseOrderLine();

                if (!Request.Form["ProductSearch" + x.ToString()].IsNullOrNullString())
                {
                    if (Request.Form["ProductID" + x.ToString()].IsNullOrNullString())
                        ModelState.AddModelError("ProductSearch" + x.ToString(), "Unrecognized Product");
                    else
                        poline.ProductID = Guid.Parse(Request.Form["ProductID" + x.ToString()]);

                    if (Request.Form["LineItemQty" + x.ToString()].IsNullOrNullString())
                        ModelState.AddModelError("LineItemQty" + x.ToString(), "Invalid Quantity");

                    if (!Request.Form["LineItemID" + x.ToString()].IsNullOrNullString())
                        poline.ID = Guid.Parse(Request.Form["LineItemID" + x.ToString()]);

                }
                else // no product - check for deleted line item
                    if (!Request.Form["LineItemID" + x.ToString()].IsNullOrNullString())
                    {
                        Guid poline_id = Guid.Parse(Request.Form["LineItemID" + x.ToString()]);

                        if (poline_id != Guid.Empty)
                            lines_to_delete.Add(poline_id);
                    }

                if (!Request.Form["LineItemQty" + x.ToString()].IsNullOrNullString())
                {
                    decimal Qty = 0;

                    if (!decimal.TryParse(Request.Form["LineItemQty" + x.ToString()], out Qty))
                        ModelState.AddModelError("LineItemQty" + x.ToString(), "Invalid Quantity");
                    else if (Qty == 0)
                        ModelState.AddModelError("LineItemQty" + x.ToString(), "Invalid Quantity");
                    else
                        poline.Qty = Qty;
                }

                if (!Request.Form["LineItemCost" + x.ToString()].IsNullOrNullString())
                {
                    decimal Cost = 0;

                    if (!decimal.TryParse(Request.Form["LineItemCost" + x.ToString()], out Cost))
                        ModelState.AddModelError("LineItemCost" + x.ToString(), "Invalid Cost");
                    else if (Cost == 0)
                        ModelState.AddModelError("LineItemCost" + x.ToString(), "Invalid Cost");
                    else
                        poline.Cost = Cost;
                }

                if (!Request.Form["LineItemActualCost" + x.ToString()].IsNullOrNullString())
                {
                    decimal ActualCost = 0;

                    if (!decimal.TryParse(Request.Form["LineItemActualCost" + x.ToString()], out ActualCost))
                        ModelState.AddModelError("LineItemActualCost" + x.ToString(), "Invalid Cost");
                    else if (ActualCost == 0)
                        ModelState.AddModelError("LineItemActualCost" + x.ToString(), "Invalid Cost");
                    else
                        poline.ActualCost = ActualCost;
                }

                if (!Request.Form["LineItemReceiptNumber" + x.ToString()].IsNullOrNullString())
                {
                    decimal ReceiptNumber = 0;

                    if (!decimal.TryParse(Request.Form["LineItemReceiptNumber" + x.ToString()], out ReceiptNumber))
                        ModelState.AddModelError("LineItemReceiptNumber" + x.ToString(), "Invalid Receipt Number");
                    else if (ReceiptNumber == 0)
                        ModelState.AddModelError("LineItemReceiptNumber" + x.ToString(), "Invalid Receipt Number");
                    else
                    {
                        ReceivedProduct rp = db.ReceivedProducts.Where(rpx => rpx.Number == ReceiptNumber).FirstOrDefault();

                        if (rp == null)
                            ModelState.AddModelError("LineItemReceiptNumber" + x.ToString(), "Receipt Number does not Exist");
                        else
                            poline.ReceivedProductID = rp.ID;
                    }
                }

                polines.Add(poline);
            }

            if (polines.Count(pol => pol.ProductID != Guid.Empty) == 0)
                ModelState.AddModelError("", "No Line Items Entered!");

            return polines;

        }

        void LoadPurchaseOrderDropDowns(string SelectedStatus, Nullable<Guid> SelectedPaymentType,string SelectedCStatus)
        {
            ViewBag.Statuses = new SelectList(db.PurchaseOrderStatuses.OrderBy(s => s.DisplaySeq).ThenBy(s => s.Name), "ID", "Name", SelectedStatus);
            ViewBag.PaymentTypes = new SelectList(db.PurchaseOrderPaymentTypes.OrderBy(pt => pt.Name), "ID", "Name", SelectedPaymentType);
            ViewBag.CStatuses = new SelectList(db.PurchaseOrderConfirm_Status.OrderBy(s => s.CName), "CID", "CName", SelectedCStatus);
        }

        void FillPurchaseOrderLines(List<PurchaseOrderLine> polines)
        {
            while (polines.Count < NUM_PRODUCTS)
                polines.Add(new PurchaseOrderLine());

        }

        void CheckUpdateProductCost(Guid ProductID, decimal Cost, decimal ActualCost)
        {

            Product product = null;

            if (Cost !=0 || ActualCost != 0)
                product = db.Products.Single(p => p.ID == ProductID);

            if (Cost != 0)
            {
                if (product.Cost != Cost)
                    product.Cost = Cost;
            }

            if (ActualCost != 0)
            {
                if (product.ActualCost != ActualCost)
                    product.ActualCost = ActualCost;
            }
        }
        
        public ActionResult CreatePurchaseOrder()
        {
            InitViewBag(ActionType.Create, "Purchase Order");
            LoadPurchaseOrderDropDowns("O", null, "CNF");

            List<PurchaseOrderLine> polines = new List<PurchaseOrderLine>();

            FillPurchaseOrderLines(polines);

            ViewBag.PurchaseOrderLines = polines;

            PurchaseOrder model = new PurchaseOrder();
            model.Date = DateTime.Today;

            return View("PurchaseOrder", model);
        }

        [HttpPost]
        public ActionResult CreatePurchaseOrder(PurchaseOrder Rec)
        {
            List<PurchaseOrderLine> polines = ValidatePurchaseOrder(Rec);

            if (ModelState.IsValid)
            {
                #region Create Records
                Rec.ID = Guid.NewGuid();
                Rec.TimeStamp = DateTime.Now;

                db.PurchaseOrders.Add(Rec);

                int Number = 0;

                foreach (PurchaseOrderLine poline in polines)
                {
                    if (poline.ProductID != Guid.Empty)
                    {
                        poline.PurchaseOrderID = Rec.ID;
                        poline.ID = Guid.NewGuid();
                        poline.TimeStamp = Rec.TimeStamp;
                        poline.LineNumber = ++Number;
                        db.PurchaseOrderLines.Add(poline);

                        CheckUpdateProductCost(poline.ProductID, poline.Cost, (decimal)poline.ActualCost);
                    }
                }

                #endregion

                CalculateTotalCost(Rec, polines);

                db.Database.ExecuteSqlCommand("UpdateProductVendors");

                db.SaveChanges();

                InitPoView(ActionType.DetailsForEmail, Rec.ID);

                Exception Ex = null;
                if (!OmnimarkAmazon.BLL.WebEmails.Send("/Views/Purchasing/PurchaseOrder.cshtml", Rec, this.ControllerContext, "New PO #" + Rec.Number.ToString(), ConfigurationManager.AppSettings["NewPurchaseOrderNotificationList"], null, false, ref Ex))
                    return Redirect("/Purchasing/PurchaseOrders?errmsg=" + "Email Failed! " + Ex.Message);
                else
                    return Redirect("/Purchasing/PurchaseOrders");
            }
            else
            {
                InitViewBag(ActionType.Create, "Purchase Order");
                LoadPurchaseOrderDropDowns(Rec.Status, Rec.PaymentTypeID,Rec.CStatus);
                ViewBag.PurchaseOrderLines = polines;

                return View("PurchaseOrder", Rec);
            }
        }

        PurchaseOrder InitPoView(Startbutton.Web._BaseController.ActionType at, Guid id)
        {
            PurchaseOrder existing_po = db.PurchaseOrders.Single(po => po.ID == id);

            InitViewBag(at, "Purchase Order", null, "#" + existing_po.Number.ToString());

            LoadPurchaseOrderDropDowns(existing_po.Status, existing_po.PaymentTypeID,existing_po.CStatus);

            List<PurchaseOrderLine> polines = db.PurchaseOrderLines.Where(pol => pol.PurchaseOrderID == id).OrderBy(pol => pol.LineNumber).ToList();

            FillPurchaseOrderLines(polines);

            ViewBag.PurchaseOrderLines = polines;

            return existing_po;

        }

        ActionResult PurchaseOrderDefault(Startbutton.Web._BaseController.ActionType at, Guid id)
        {

            PurchaseOrder existing_po = InitPoView(at, id);

            return View("PurchaseOrder", existing_po);
        }

        public ActionResult EditPurchaseOrder(Guid id)
        {
            return PurchaseOrderDefault(ActionType.Edit, id);
        }

        public ActionResult ViewPurchaseOrder(Guid id)
        {
            if (Request.HttpMethod == "POST")
                return Redirect("/Purchasing/PurchaseOrders");
            else
                return PurchaseOrderDefault(ActionType.Details, id);
        }

        //public ActionResult CopyPurchaseOrder(Guid id)
        //{
        //    PurchaseOrder CopyFrom = db.PurchaseOrders.Single(po => po.ID == id);
        //    PurchaseOrder CopyTo = new PurchaseOrder();

        //    Startbutton.Library.SetMatchingMembers(CopyTo, CopyFrom, new string[] { "Lines", "Vendor", "VendorReference", "PurchaseOrderStatusReference", "PaymentType", "PaymentTypeReference", "EntityKey" });

        //    CopyTo.ID = Guid.NewGuid();
        //    CopyTo.TimeStamp = DateTime.Now;

        //    db.PurchaseOrders.Add(CopyTo);

        //    foreach (PurchaseOrderLine CopyLineFrom in db.PurchaseOrderLines.Where(polx => polx.PurchaseOrderID == id))
        //    {
        //        PurchaseOrderLine CopyLineTo = new PurchaseOrderLine();
        //        Startbutton.Library.SetMatchingMembers(CopyLineTo, CopyLineFrom, new string[] { "Product", "ProductReference", "PurchaseOrder", "PurchaseOrderReference", "ReceivedProduct", "ReceivedProductReference", "PurchaseOrderID", "ReceivedProductID" });

        //        CopyLineTo.EntityKey = null;
        //        CopyLineTo.PurchaseOrderID = CopyTo.ID;
        //        CopyLineTo.ID = Guid.NewGuid();
        //        CopyLineTo.TimeStamp = DateTime.Now;

        //        db.PurchaseOrderLines.Add(CopyLineTo);

        //    }

        //    db.SaveChanges();

        //    CopyTo = db.PurchaseOrders.Single(po => po.ID == CopyTo.ID);

        //    Exception Ex = null;
        //    OmnimarkAmazon.BLL.Emails.Send("http://" + Startbutton.Web.Library.CurrentHost + "/Purchasing/ViewPurchaseOrderForEmail/" + CopyTo.ID.ToString(), "New PO #" + CopyTo.Number.ToString() + " - Copied from #" + CopyFrom.Number.ToString(), ConfigurationManager.AppSettings["NewPurchaseOrderNotificationList"], null, false, ref Ex);

        //    return RedirectToAction("EditPurchaseOrder", new { id = CopyTo.ID });

            
        //}

        public ActionResult DeletePurchaseOrder(Guid id)
        {
            if (Request.HttpMethod == "POST")
            {
                List<PurchaseOrderLine> lines_to_delete = db.PurchaseOrderLines.Where(pol => pol.PurchaseOrderID == id).ToList();

                foreach (PurchaseOrderLine poline in lines_to_delete)
                    db.PurchaseOrderLines.Remove(poline);

                db.PurchaseOrders.Remove(db.PurchaseOrders.Single(po => po.ID == id));

                db.SaveChanges();

                return Redirect("/Purchasing/PurchaseOrders");
            }
            else
                return PurchaseOrderDefault(ActionType.Delete, id);
        }

        [HttpPost]
        public ActionResult EditPurchaseOrder(Guid id, PurchaseOrder Rec)
        {
            PurchaseOrder existing_po = db.PurchaseOrders.Single(po => po.ID == id);
            List<Guid> lines_to_delete = new List<Guid>();

            List<PurchaseOrderLine> polines = ValidatePurchaseOrder(Rec, ref lines_to_delete);

            if (ModelState.IsValid)
            {

                int Number = 0;

                #region Delete Records
                if (lines_to_delete.Count > 0)
                {

                    foreach (Guid poline_id in lines_to_delete)
                        db.PurchaseOrderLines.Remove(db.PurchaseOrderLines.Single(pol => pol.ID == poline_id));

                    db.SaveChanges();

                    // renumber lines

                    foreach (PurchaseOrderLine poline in db.PurchaseOrderLines.Where(pol => pol.PurchaseOrderID == id).OrderBy(pol => pol.LineNumber))
                        poline.LineNumber = ++Number;

                    db.SaveChanges();
                }
                #endregion

                #region Update Records
                string errmsg = null;

                if (Rec.Status == "I" && existing_po.Status != "I")
                {
                    ViewBag.PurchaseOrderNumber = existing_po.Number;
                    Exception Ex = null;
                    if (!OmnimarkAmazon.BLL.WebEmails.Send("/Views/Purchasing/PurchaseOrderIssue.cshtml", Rec, this.ControllerContext, "Issue with PO #" + existing_po.Number.ToString(), ConfigurationManager.AppSettings["NewPurchaseOrderNotificationList"], null, false, ref Ex))
                        errmsg = "Email Failed! " + Ex.Message;
                }

                UpdateModel(existing_po);

                CalculateTotalCost(existing_po, polines);

                Number = 0;

                foreach (PurchaseOrderLine poline in polines)
                {
                    if (poline.ProductID != Guid.Empty)
                    {
                        if (poline.ID == Guid.Empty)
                        {
                            poline.PurchaseOrderID = Rec.ID;
                            poline.ID = Guid.NewGuid();
                            poline.TimeStamp = DateTime.Now;
                            poline.LineNumber = ++Number;
                            db.PurchaseOrderLines.Add(poline);
                        }
                        else
                        {
                            PurchaseOrderLine existing_line = db.PurchaseOrderLines.Single(pol => pol.ID == poline.ID);
                            existing_line.ProductID = poline.ProductID;
                            existing_line.Qty = poline.Qty;
                            existing_line.Cost = poline.Cost;
                            existing_line.ActualCost = poline.ActualCost;
                            existing_line.ReceivedProductID = poline.ReceivedProductID;
                            Number++;
                        }

                        CheckUpdateProductCost(poline.ProductID, poline.Cost, (decimal)poline.ActualCost);

                    }
                }
                #endregion

                db.Database.ExecuteSqlCommand("UpdateProductVendors");

                db.SaveChanges();

                return Redirect("/Purchasing/PurchaseOrders" + (errmsg == null ? "" : "?errmsg=" + Server.UrlEncode(errmsg)));
            }
            else
            {
                InitViewBag(ActionType.Edit, "Purchase Order", null, "#" + existing_po.Number.ToString());
                LoadPurchaseOrderDropDowns(Rec.Status, Rec.PaymentTypeID,Rec.CStatus);
                FillPurchaseOrderLines(polines);

                ViewBag.PurchaseOrderLines = polines;

                return View("PurchaseOrder", Rec);
            }
        }

        public ActionResult VendorLookup(string term)
        {
            var VendorList = db.Vendors.Where(p => p.Name.Contains(term)).OrderBy(p => p.Name).Select(p => new { label = p.Name, value = p.ID }).ToList();

            VendorList.Insert(0, new { label = "-- New Vendor --", value = Guid.Empty });
            return Json(VendorList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VendorFilterLookup(string term)
        {
            var VendorList = db.Vendors.Where(p => p.Name.Contains(term)).OrderBy(p => p.Name).Select(p => new { label = p.Name, value = p.ID }).ToList();

            return Json(VendorList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewVendor(string Name)
        {
            Vendor NewVendor = db.Vendors.Where(v => v.Name == Name).FirstOrDefault();

            if (NewVendor == null)
            {
                NewVendor = new Vendor();
                NewVendor.ID = Guid.NewGuid();
                NewVendor.Name = Name;
                NewVendor.TimeStamp = DateTime.Now;
                db.Vendors.Add(NewVendor);
                db.SaveChanges();
            }

            return Json(new { NewID = NewVendor.ID }, JsonRequestBehavior.AllowGet);

        }

        void CalculateTotalCost(PurchaseOrder po, List<PurchaseOrderLine> polines)
        {
            po.TotalCost = 0;
            po.TotalActualCost = 0;
            po.MissingCosts = false;

            foreach (PurchaseOrderLine poline in polines)
            {
                if (poline.ProductID != Guid.Empty)
                {
                    po.TotalCost += poline.Cost * poline.Qty;
                    po.TotalActualCost += poline.ActualCost * poline.Qty;

                    if (poline.Cost == 0 || poline.ActualCost == 0)
                        po.MissingCosts = true;
                }
            }

        }

        #endregion

    }
}
