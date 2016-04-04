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
using System.Data.Entity.Infrastructure;
using System.Web.Caching;

namespace OmnimarkAmazonWeb.Controllers
{
   
    public class AmazonInventoryController : _BaseController
    {
      
        string RecType = "Inventory Item";
       
        bool FilterOnlyLowInventory(KnownASINWithInventoryAndSales aip, double SalesDays)
        {
            decimal StockQtyTotal = 0;

            if (aip.StockQtyBrandzilla != null)
                StockQtyTotal += (decimal)aip.StockQtyBrandzilla;

            if (aip.StockQtyFiveStar != null)
                StockQtyTotal += (decimal)aip.StockQtyFiveStar;

            if (aip.StockQtyVitality != null)
                StockQtyTotal += (decimal)aip.StockQtyVitality;

            if (aip.StockQtyAdmarkia != null)
                StockQtyTotal += (decimal)aip.StockQtyAdmarkia;

            if (aip.StockQtyNutramart != null)
                StockQtyTotal += (decimal)aip.StockQtyNutramart;

            if (aip.StockQtyEurozoneMarketplace != null)
                StockQtyTotal += (decimal)aip.StockQtyEurozoneMarketplace;

            if (aip.StockQtySuperQuick != null)
                StockQtyTotal += (decimal)aip.StockQtySuperQuick;

            if (SalesDays == 0)
                return false;

            if (aip.QtySold == null)
                return false;

            if ((double)aip.QtySold / SalesDays * 30 > (double)StockQtyTotal)
                return true;
            else
                return false;
        }

        public ActionResult UpdateInventory(string id, string store)
        {
            string Error = null;
            string LogText = "";
            decimal NewStock = 0;

            AmazonAccount Account = GetAmazonAccount(store);

            List<string> SKUs = Account.AmazonInventorySKUs.Where(ais => ais.ASIN == id).Select(ais => ais.SKU).ToList();

            if (SKUs.Count == 0)
                Error = "No Known SKUs";
            else
            {
                var rec = db.AmazonInventories.Single(ai => ai.AmazonAccountID == Account.ID && ai.ASIN == id);
                rec.AmazonInStockQty = 0;
                rec.AmazonStockQty = 0;
                rec.AmazonStockTimeStamp = DateTime.Now;

                var ToDelete = db.AmazonInventorySKUs.Where(ais => ais.ASIN == id && ais.AmazonAccountID == Account.ID).ToList();

                foreach (AmazonInventorySKU ais in ToDelete)
                {
                    ais.TotalQty = 0;
                    ais.InStockQty = 0;
                }

                IEnumerable<InventorySupplySummary> inventory = OmnimarkAmazon.Library.GetInventory(null, Account, (LineBreak, Line) => UpdateInventoryLog(LineBreak, Line, ref LogText), SKUs);

                OmnimarkAmazon.Library.UpdateInventory(db, Account, inventory, (LineBreak, Line) => UpdateInventoryLog(LineBreak, Line, ref LogText), false);
                db.SaveChanges();

                if (rec == null)
                    Error = "No Inventory Record Found!";
                else
                    NewStock = (decimal)rec.AmazonStockQty;
            }

            return Json(new { Stock = NewStock, Log = LogText, Error = Error }, JsonRequestBehavior.AllowGet);
        }

        void UpdateInventoryLog(bool LineBreak, string Line, ref string LogText)
        {
            LogText += Line;

            if (LineBreak)
                LogText += "<br />\n";
        }

        public ActionResult Index(Nullable<Guid> ShowStockFor, string OnlyNoProducts, string OnlyUSA, Nullable<DateTime> SalesStart, Nullable<DateTime> SalesEnd, Nullable<Guid> ProductID, string OnlyLowInventory, Nullable<Guid> VendorID, string ASIN, string SKU)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;

            bool QueryeBay = ShowStockFor == null && OnlyNoProducts == null && OnlyUSA == null && OnlyNoProducts == null && (ASIN == null  || ASIN == "") && (SKU == null || SKU == "") && Request.RequestType != "GET";
           
            db.Database.ExecuteSqlCommand("TryUpdateSalesPerDayByASINTable");
           
            InitViewBag(ActionType.List, RecType);
            ViewBag.Accounts = new SelectList(db.AmazonAccounts.OrderBy(aa => aa.Name), "ID", "Name");
            ViewBag.ShowStockFor = ShowStockFor;

            if (SalesStart == null)
                SalesStart = DateTime.Today.AddDays(-7);

            if (SalesEnd == null)
                SalesEnd = DateTime.Today;

            double SalesDays = ((DateTime)SalesEnd).ToOADate() - ((DateTime)SalesStart).ToOADate();
                       
            IQueryable<KnownASINWithInventoryAndSales> PreQueryModel = SQLFunctions.KnownASINsWithInventoryAndSales(ShowStockFor, (DateTime)SalesStart, (int)SalesDays, OnlyUSA.IsNullOrNullString() ? false : true).OrderByDescending(ka => ka.QtySold).ThenBy(ka => ka.Title);

            if (!ASIN.IsNullOrNullString())
            {
                if (db.KnownASINs.Count(k => k.ASIN == ASIN) == 0)
                    ViewBag.Message = "Unknown ASIN: " + ASIN;
                else
                {
                    PreQueryModel = PreQueryModel.Where(p => p.ASIN == ASIN);
                    ViewBag.ASIN = ASIN;
                }
            }

            if (!SKU.IsNullOrNullString())
            {
                AmazonInventorySKU sku = db.AmazonInventorySKUs.Where(k => k.SKU == SKU).FirstOrDefault();

                if (sku == null)
                    ViewBag.Message = "Unknown SKU: " + ASIN;
                else
                {
                    PreQueryModel = PreQueryModel.Where(p => p.ASIN == sku.ASIN);
                    ViewBag.SKU = SKU;
                }
            }

            if (!OnlyNoProducts.IsNullOrNullString())
            {
                PreQueryModel = PreQueryModel.Where(ka => ka.ProductCount == 0);
                ViewBag.OnlyNoProducts = true;
            }

           // (db as IObjectContextAdapter).ObjectContext.CommandTimeout = 1800;

            if (Request.RequestType == "GET")
                PreQueryModel = PreQueryModel.Where(m => m.ASIN == "Xawerf");

            IEnumerable<KnownASINWithInventoryAndSales> model = PreQueryModel.ToList();

            if (!OnlyUSA.IsNullOrNullString())
                model = model.OrderByDescending(m => m.QtySold).ThenBy(m => m.Title);

            if (ProductID != null)
            {
                List<string> ASINs = db.AmazonInventoryProducts.Where(aip => aip.ProductID == ProductID).Select(aip => aip.ASIN).ToList();

                model = model.Where(ka => ASINs.Contains(ka.ASIN));

                var pi = db.ProductInventories.Where(pix => pix.ProductID == ProductID && pix.LocationID == OmnimarkAmazon.Library.OrlandoLocationID).FirstOrDefault();

                if (pi == null)
                    ViewBag.MainWarehouseInventory = 0;
                else
                    ViewBag.MainWarehouseInventory = pi.Qty;
            }

            Vendor Vendor = null;

            if (VendorID != null)
            {
                SQLFunctions.UpdateProductVendors();

                Vendor = db.Vendors.Single(v => v.ID == VendorID);
                               
                List<Guid> VendorProducts = Vendor.Products.Select(p => p.ID).ToList();
                List<string> ASINs = db.AmazonInventoryProducts.Where(aip => VendorProducts.Contains(aip.Product.ID)).Select(aip => aip.ASIN).ToList();

                model = model.Where(ka => ASINs.Contains(ka.ASIN));
            }

            if (!OnlyLowInventory.IsNullOrNullString())
            {
                model = model.Where(ka => FilterOnlyLowInventory(ka, SalesDays));
                ViewBag.OnlyLowInventory = true;
            }

            if (!OnlyUSA.IsNullOrNullString())
                ViewBag.OnlyUSA = true;
            else
                ViewBag.OnlyUSA = false;

            // Set ViewBag stuff
            ViewBag.TotalInventoryValue = model.Sum(ka => ka.ProductCost * (ka.StockQty == null ? 0 : ka.StockQty));
            ViewBag.TotalInventoryInboundValue = model.Sum(ka => ka.ProductCost * (ka.StockQtyInbound == null ? 0 : ka.StockQtyInbound));

            if (model.Count() == 0)
                ViewBag.AnyCostMissing = false;
            else
                ViewBag.AnyCostMissing = model.Max(ka => ka.MissingCosts);

            ViewBag.SalesStart = SalesStart;
            ViewBag.SalesEnd = SalesEnd;

            if (SalesEnd == DateTime.Today)
                ViewBag.SalesDays = SalesDays.ToString();
            else
                ViewBag.SalesDays = "n/a";

            if (ProductID != null)
            {
                ViewBag.ProductID = ProductID;
                ViewBag.ProductName = db.Products.Single(p => p.ID == (Guid)ProductID).Name;
            }

            if (VendorID != null)
            {
                ViewBag.VendorID = VendorID;
                ViewBag.VendorName = Vendor.Name;
            }

            AmazonAccount BrandzillaAccount = GetAmazonAccount("bz");
            AmazonAccount FiveStarAccount = GetAmazonAccount("fs");
            AmazonAccount VitalityAccount = GetAmazonAccount("vp");
            AmazonAccount NutramartAccount = GetAmazonAccount("nm");
            AmazonAccount SuperQuickAccount = GetAmazonAccount("sq");
            AmazonAccount AdmarkiaAccount = GetAmazonAccount("am");
            AmazonAccount PlatinumHealth = GetAmazonAccount("ph");
            AmazonAccount AmazonCanada = GetAmazonAccount("ac");
            AmazonAccount FrogPond = GetAmazonAccount("fp");
            AmazonAccount EurozoneMarketplace = GetAmazonAccount("em");

            ViewBag.BrandzillaUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == BrandzillaAccount.ID).LastInventoryUpdateStart;
            ViewBag.FiveStarUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == FiveStarAccount.ID).LastInventoryUpdateStart;
            ViewBag.VitalityUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == VitalityAccount.ID).LastInventoryUpdateStart;
            ViewBag.NutramartUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == NutramartAccount.ID).LastInventoryUpdateStart;
            ViewBag.SuperQuickUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == SuperQuickAccount.ID).LastInventoryUpdateStart;
            ViewBag.AdmarkiaUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == AdmarkiaAccount.ID).LastInventoryUpdateStart;
            ViewBag.PlatinumHealthUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == PlatinumHealth.ID).LastInventoryUpdateStart;
            ViewBag.AmazonCanadaUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == AmazonCanada.ID).LastInventoryUpdateStart;
            ViewBag.FrogPondUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == FrogPond.ID).LastInventoryUpdateStart;
            ViewBag.EurozoneMarketplaceTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == EurozoneMarketplace.ID).LastInventoryUpdateStart;
            var FinalModel = model.ToList();

            if (QueryeBay)
            {

                List<string> ItemIDs = null;

                if (ProductID != null)
                    ItemIDs = db.eBayItemsProducts.Where(eip => eip.ProductID == ProductID).Select(aip => aip.ItemID).ToList();

                IEnumerable<InventoryManagementeBayRecord> FinaleBayData;

                var eBayData = db.Database.SqlQuery<InventoryManagementeBayRecord>(@"
                    select ei.ItemID, ei.Title, sum(eol.qty) as Qty from eBayItems ei 
                    left join eBayOrderLines eol on eol.ItemID = ei.ItemID
                    left join eBayOrders eo on eol.OrderID = eo.ID
                    where eo.Created is null or (eo.Created > '" + SalesStart.ToString() + @"' and eo.Created < '" + SalesEnd.ToString() + @"')
                    group by ei.ItemID, ei.Title
                ");

                if (ProductID != null)
                    FinaleBayData = eBayData.Where(ka=>ItemIDs.Contains(ka.ItemID)).ToList();
                else
                    FinaleBayData = eBayData.ToList();


                FinalModel.AddRange(FinaleBayData.Select(e => new KnownASINWithInventoryAndSales { IseBayListing = true, ASIN = e.ItemID, Title = e.Title, QtySold = e.Qty }));

            }

            return View(FinalModel);
        }

        public ActionResult IndexNew()
        {
            InitViewBag(ActionType.List, RecType);
            ViewBag.Accounts = new SelectList(db.AmazonAccounts.OrderBy(aa => aa.Name), "ID", "Name");
            return View();
        }

        public ActionResult KnownASINsWithInventoryAndSales2()
        {
            List<KnownASINWithInventoryAndSales2> model = new List<KnownASINWithInventoryAndSales2>();

            ViewBag.SalesStart = DateTime.Today.AddDays(-7);
            ViewBag.SalesEnd = DateTime.Today;

            return View(model);
        }

        [HttpPost]
        public ActionResult KnownASINsWithInventoryAndSales2(Guid? ProductID, DateTime? SalesStart, DateTime? SalesEnd)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            if (ProductID == null && SalesStart == null && SalesEnd == null)


                if (SalesStart == null)
                    SalesStart = DateTime.Today.AddDays(-7);

            if (SalesEnd == null)
                SalesEnd = DateTime.Today;

            double SalesDays = ((DateTime)SalesEnd).ToOADate() - ((DateTime)SalesStart).ToOADate();

            IQueryable<KnownASINWithInventoryAndSales2> model = SQLFunctions.KnownASINsWithInventoryAndSales2((DateTime)SalesStart, (int)SalesDays).Where(ka => ka.ProductID == ProductID || ProductID == null);


            ViewBag.SalesStart = SalesStart;
            ViewBag.SalesEnd = SalesEnd;

            if (ProductID != null)
            {
                ViewBag.ProductID = ProductID;
                ViewBag.ProductName = db.Products.Single(p => p.ID == (Guid)ProductID).Name;
            }

            return View(model);
        }

        public ActionResult KnownASINsWithInventoryAndSales(Nullable<Guid> ShowStockFor, string OnlyNoProducts, string OnlyUSA, Nullable<DateTime> SalesStart, Nullable<DateTime> SalesEnd, Nullable<Guid> ProductID, string OnlyLowInventory, Nullable<Guid> VendorID, string ASIN, string SKU)
        {
            Dictionary<string, object> RtnData = new Dictionary<string, object>();
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            db.Database.ExecuteSqlCommand("TryUpdateSalesPerDayByASINTable");

            InitViewBag(ActionType.List, RecType);
            RtnData["ShowStockFor"] = ShowStockFor;

            if (SalesStart == null)
                SalesStart = DateTime.Today.AddDays(-7);

            if (SalesEnd == null)
                SalesEnd = DateTime.Today;

            double SalesDays = ((DateTime)SalesEnd).ToOADate() - ((DateTime)SalesStart).ToOADate();

            IQueryable<KnownASINWithInventoryAndSales> PreQueryModel = SQLFunctions.KnownASINsWithInventoryAndSales(ShowStockFor, (DateTime)SalesStart, (int)SalesDays, OnlyUSA.IsNullOrNullString() ? false : true).OrderByDescending(ka => ka.QtySold).ThenBy(ka => ka.Title);

            if (!ASIN.IsNullOrNullString())
            {
                if (db.KnownASINs.Count(k => k.ASIN == ASIN) == 0)
                    RtnData["Message"] = "Unknown ASIN: " + ASIN;
                else
                {
                    PreQueryModel = PreQueryModel.Where(p => p.ASIN == ASIN);
                    RtnData["ASIN"] = ASIN;
                }
            }

            if (!SKU.IsNullOrNullString())
            {
                AmazonInventorySKU sku = db.AmazonInventorySKUs.Where(k => k.SKU == SKU).FirstOrDefault();

                if (sku == null)
                    RtnData["Message"] = "Unknown SKU: " + ASIN;
                else
                {
                    PreQueryModel = PreQueryModel.Where(p => p.ASIN == sku.ASIN);
                    RtnData["SKU"] = SKU;
                }
            }

            if (!OnlyNoProducts.IsNullOrNullString())
            {
                PreQueryModel = PreQueryModel.Where(ka => ka.ProductCount == 0);
                RtnData["OnlyNoProducts"] = true;
            }

            IEnumerable<KnownASINWithInventoryAndSales> model = PreQueryModel.ToList();

            if (ProductID != null)
            {
                List<string> ASINs = db.AmazonInventoryProducts.Where(aip => aip.ProductID == ProductID).Select(aip => aip.ASIN).ToList();

                model = model.Where(ka => ASINs.Contains(ka.ASIN));
            }

            Vendor Vendor = null;

            if (VendorID != null)
            {
                SQLFunctions.UpdateProductVendors();

                Vendor = db.Vendors.Single(v => v.ID == VendorID);
                              
                List<Guid> VendorProducts = Vendor.Products.Select(p => p.ID).ToList();
                List<string> ASINs = db.AmazonInventoryProducts.Where(aip => VendorProducts.Contains(aip.Product.ID)).Select(aip => aip.ASIN).ToList();

                model = model.Where(ka => ASINs.Contains(ka.ASIN));
            }

            if (!OnlyLowInventory.IsNullOrNullString())
            {
                model = model.Where(ka => FilterOnlyLowInventory(ka, SalesDays));
                RtnData["OnlyLowInventory"] = true;
            }

            if (!OnlyUSA.IsNullOrNullString())
            {
                RtnData["OnlyUSA"] = true;
            }
                       
            RtnData["TotalInventoryValue"] = model.Sum(ka => ka.ProductCost * (ka.StockQty == null ? 0 : ka.StockQty));

            if (model.Count() == 0)
                RtnData["AnyCostMissing"] = false;
            else
                RtnData["AnyCostMissing"] = model.Max(ka => ka.MissingCosts);

            RtnData["SalesStart"] = SalesStart;
            RtnData["SalesEnd"] = SalesEnd;

            if (SalesEnd == DateTime.Today)
                RtnData["SalesDays"] = SalesDays.ToString();
            else
                RtnData["SalesDays"] = "n/a";

            if (ProductID != null)
            {
                RtnData["ProductID"] = ProductID;
                RtnData["ProductName"] = db.Products.Single(p => p.ID == (Guid)ProductID).Name;
            }

            if (VendorID != null)
            {
                RtnData["VendorID"] = VendorID;
                RtnData["VendorName"] = Vendor.Name;
            }

            AmazonAccount BrandzillaAccount = GetAmazonAccount("bz");
            AmazonAccount FiveStarAccount = GetAmazonAccount("fs");
            AmazonAccount VitalityAccount = GetAmazonAccount("vp");
            AmazonAccount NutramartAccount = GetAmazonAccount("nm");
            AmazonAccount SuperQuickAccount = GetAmazonAccount("sq");
            AmazonAccount AdmarkiaAccount = GetAmazonAccount("am");
            AmazonAccount PlatinumHealth = GetAmazonAccount("ph");
            AmazonAccount AmazonCanada = GetAmazonAccount("ac");
            AmazonAccount FrogPond = GetAmazonAccount("fp");
            AmazonAccount EurozoneMarketplace = GetAmazonAccount("em");

            ViewBag.AdmarkiaUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == AdmarkiaAccount.ID).LastInventoryUpdateStart;
            ViewBag.PlatinumHealthUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == PlatinumHealth.ID).LastInventoryUpdateStart;
            ViewBag.AmazonCanadaUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == AmazonCanada.ID).LastInventoryUpdateStart;
            ViewBag.FrogPondUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == FrogPond.ID).LastInventoryUpdateStart;
            ViewBag.BrandzillaUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == BrandzillaAccount.ID).LastInventoryUpdateStart;
            ViewBag.FiveStarUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == FiveStarAccount.ID).LastInventoryUpdateStart;
            ViewBag.VitalityUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == VitalityAccount.ID).LastInventoryUpdateStart;
            ViewBag.SuperQuickUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == SuperQuickAccount.ID).LastInventoryUpdateStart;
            ViewBag.NutramartUpdateTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == NutramartAccount.ID).LastInventoryUpdateStart;
            ViewBag.EurozoneMarketplaceTimeStamp = db.AmazonAccounts.Single(aa => aa.ID == EurozoneMarketplace.ID).LastInventoryUpdateStart;

            RtnData["html"] = Startbutton.Web.Library.RenderPartialViewToString(this.ControllerContext, ViewData, TempData, "KnownASINsWithInventoryAndSales", model);

            return new JsonResult()
            {
                Data = RtnData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        KnownASINWithInventoryAndSales KnownASINToInventoryAndSales(KnownASIN ka)
        {
            KnownASINWithInventoryAndSales i = new KnownASINWithInventoryAndSales();

            i.Title = ka.Title;
            i.ASIN = ka.ASIN;

            return i;
        }

        public ActionResult Products(string id)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            KnownASIN ka = db.KnownASINs.Single(kax => kax.ASIN == id);

            ViewBag.Title = "Inventory Maintenance for " + ka.Title;
            ViewBag.ProductsInventory = db.AmazonInventories.Where(ai => ai.ASIN == id);
            ViewBag.AssociatedProducts = db.AmazonInventoryProducts.Where(aip => aip.ASIN == id);
            ViewBag.ASIN = id;

            List<Country> Countries = new List<Country>();

            foreach (AmazonInventory ai in ka.AmazonInventories)
                if (!Countries.Contains(ai.AmazonAccount.Country))
                    Countries.Add(ai.AmazonAccount.Country);

            ViewBag.ListingCountries = Countries;

            return View(ka);
        }

        public ActionResult ProductLookup(string term, Guid? VendorID)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            Func<Product, bool> predicate = null;
            if (VendorID == null)
            {
                return base.Json(from p in db.Products
                                 where p.Name.Contains(term)
                                 orderby p.Name
                                 select new { label = p.Name, value = p.ID }, JsonRequestBehavior.AllowGet);
            }
            Vendor vendor = db.Vendors.Single(v => v.ID == VendorID);
            if (predicate == null)
            {
                predicate = delegate(Product p)
                {
                    if (term != null)
                    {
                        return p.Name.Contains(term);
                    }
                    return true;
                };
            }
            return base.Json(from p in vendor.Products.Where(predicate)
                             orderby p.Name
                             select new { label = p.Name, value = p.ID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductLookupWithCost(string term)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            var products = db.Products.Where(p => p.Name.Contains(term)).OrderBy(p => p.Name).ToList().Select(p => new { label = p.Name, value = p.ID.ToString() + "," + p.Cost.ToString() + "," + p.ActualCost.ToString() });

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VendorLookup(string term, Guid? ProductID)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            Func<Vendor, bool> predicate = null;
            if (ProductID == null)
            {
                return base.Json(from p in db.Vendors
                                 where p.Name.Contains(term)
                                 orderby p.Name
                                 select new { label = p.Name, value = p.ID }, JsonRequestBehavior.AllowGet);
            }
            Product product = db.Products.Single(p => p.ID == ProductID);
            if (predicate == null)
            {
                predicate = p => p.Name.Contains(term);
            }
            return base.Json(from p in product.Vendors.Where(predicate)
                             orderby p.Name
                             select new { label = p.Name, value = p.ID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StockDetails(string id, string store)
        {

            AmazonAccount Account = GetAmazonAccount(store);

            string StoreName = Account.Name;

            return Json(new { StoreName, Stock = db.AmazonInventorySKUs.Where(ais => ais.AmazonAccountID == Account.ID && ais.ASIN == id).Select(ais => new { ais.SKU, ais.InStockQty, ais.TotalQty }) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAssociatedProduct(string ASIN)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            KnownASIN ka = db.KnownASINs.Single(kax => kax.ASIN == ASIN);

            ViewBag.Title = "Create Associated Products for " + ka.Title;
            ViewBag.KnownASIN = ka;

            return View("AssociatedProduct", new AmazonInventoryProduct());
        }

        [HttpPost]
        public ActionResult CreateAssociatedProduct(string ASIN, AmazonInventoryProduct NewRec)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            if (ModelState.IsValid)
            {
                NewRec.ASIN = ASIN;
                NewRec.TimeStamp = DateTime.Now;
                db.AmazonInventoryProducts.Add(NewRec);
                db.SaveChanges();

                return RedirectToAction("Products", new { id = ASIN });
            }

            KnownASIN ka = db.KnownASINs.Single(kax => kax.ASIN == ASIN);

            ViewBag.Title = "Create Associated Products for " + ka.Title;
            ViewBag.KnownASIN = ka;

            return View("AssociatedProduct", NewRec);
        }

        public ActionResult EditAssociatedProduct(string ASIN, Guid ProductID)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            KnownASIN ka = db.KnownASINs.Single(kax => kax.ASIN == ASIN);

            ViewBag.Title = "Edit Associated Products for " + ka.Title;
            ViewBag.KnownASIN = ka;

            AmazonInventoryProduct aip = db.AmazonInventoryProducts.Single(p => p.ASIN == ASIN && p.ProductID == ProductID);

            ViewBag.ProductName = aip.Product.Name;

            return View("AssociatedProduct", aip);
        }

        [HttpPost]
        public ActionResult EditAssociatedProduct(string ASIN, Guid ProductID, AmazonInventoryProduct Rec)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            ProductID = Guid.Parse(Request.QueryString["ProductID"]);

            AmazonInventoryProduct aip = db.AmazonInventoryProducts.Single(p => p.ASIN == ASIN && p.ProductID == ProductID);

            if (ModelState.IsValid)
            {
                UpdateModel(aip);
                db.SaveChanges();

                return RedirectToAction("Products", new { id = ASIN });
            }

            KnownASIN ka = db.KnownASINs.Single(kax => kax.ASIN == ASIN);

            ViewBag.Title = "Edit Associated Products for " + ka.Title;
            ViewBag.KnownASIN = ka;

            ViewBag.ProductName = db.AmazonInventoryProducts.Single(p => p.ASIN == ASIN && p.ProductID == Rec.ProductID).Product.Name;

            return View("AssociatedProduct", Rec);
        }

        public ActionResult DeleteAssociatedProduct(string ASIN, Guid ProductID)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            db.AmazonInventoryProducts.Remove(db.AmazonInventoryProducts.Single(p => p.ASIN == ASIN && p.ProductID == ProductID));
            db.SaveChanges();

            return RedirectToAction("Products", new { id = ASIN });
        }

        public ActionResult FBACalc(Nullable<DateTime> SalesStart, Nullable<DateTime> SalesEnd, Nullable<Guid> ProductID, string Qty)
        {
            #region Set ViewBag stuff
            if (SalesStart == null)
                SalesStart = DateTime.Today.AddDays(-7);

            if (SalesEnd == null)
                SalesEnd = DateTime.Today;

            double SalesDays = ((DateTime)SalesEnd).ToOADate() - ((DateTime)SalesStart).ToOADate();

            ViewBag.SalesStart = SalesStart;
            ViewBag.SalesEnd = SalesEnd;

            if (SalesEnd == DateTime.Today)
                ViewBag.SalesDays = SalesDays.ToString();
            else
                ViewBag.SalesDays = "n/a";

            if (ProductID != null)
            {
                ViewBag.ProductID = ProductID;
                ViewBag.ProductName = db.Products.Single(p => p.ID == (Guid)ProductID).Name;
            }

            decimal QtyDec = 0;

            if (decimal.TryParse(Qty, out QtyDec))
                ViewBag.Qty = QtyDec;
            #endregion

            #region Prepare RelatedProducts and set ViewBag stuff

            List<RelatedProductCalc> RelatedProductCalcs = new List<RelatedProductCalc>();
            List<RelatedProductCalcASIN> RelatedProductCalcASINs = new List<RelatedProductCalcASIN>();

            foreach (var RelatedProduct in db.RelatedProducts.Where(rp => rp.ProductID == ProductID).Select(rp => new { RelatedProductID = rp.RelatedProductID, RelatedProductName = rp.RelatedProductName }).Distinct())
            {
                string val = Request.Form["RelatedProduct_" + RelatedProduct.RelatedProductID.ToString()];

                #region Build RelatedProductCalc
                RelatedProductCalc rpc = new RelatedProductCalc();

                rpc.ProductID = RelatedProduct.RelatedProductID;
                rpc.ProductName = RelatedProduct.RelatedProductName;

                if (val == null)
                    rpc.QtyAvailable = 0;
                else
                    decimal.TryParse(val, out rpc.QtyAvailable);

                RelatedProductCalcs.Add(rpc);

                #endregion

                #region Build RelatedProductCalcASINs for this Product

                IEnumerable<RelatedProduct> ASINs = db.RelatedProducts.Where(rp => rp.ProductID == ProductID && rp.RelatedProductID == RelatedProduct.RelatedProductID);

                foreach (RelatedProduct ASIN in ASINs)
                {

                    RelatedProductCalcASIN rpca = new RelatedProductCalcASIN();

                    rpca.ProductID = rpc.ProductID;
                    rpca.ASIN = ASIN.ASIN;
                    rpca.ProductQty = ASIN.ProductQty;
                    rpca.RelatedProductQty = ASIN.RelatedProductQty;

                    if (QtyDec != 0)
                        rpca.Ratio = rpc.QtyAvailable / QtyDec * ASIN.ProductQty / ASIN.RelatedProductQty;

                    RelatedProductCalcASINs.Add(rpca);
                }
                #endregion

            }

            ViewBag.RelatedProductCalcs = RelatedProductCalcs;
            ViewBag.RelatedProductCalcASINs = RelatedProductCalcASINs;

            #endregion

            List<ShippingCalcResult> Results = new List<ShippingCalcResult>();

            #region Build Results Model

            if (ProductID != null && QtyDec != 0)
            {
                #region Initialize stuff

                decimal TotalSales = 0;

                IEnumerable<AmazonInventoryProduct> ASINs = db.AmazonInventoryProducts.Where(aip => aip.ProductID == ProductID);

                IEnumerable<SalesReportRecord> Sales = SQLFunctions.SalesReport((DateTime)SalesStart, (int)SalesDays);

                #endregion

                #region  loop 1 - populate initial records
                foreach (AmazonInventoryProduct ASIN in ASINs)
                {
                    foreach (AmazonInventory i in db.AmazonInventories.Where(ai => ai.ASIN == ASIN.ASIN))
                    {

                        ShippingCalcResult scr = new ShippingCalcResult(db);

                        SalesReportRecord srr = Sales.Where(srrx => srrx.ASIN == ASIN.ASIN && srrx.AmazonAccountID == i.AmazonAccountID).FirstOrDefault();

                        scr.ASIN = ASIN.ASIN;
                        scr.AmazonAccountID = i.AmazonAccountID;
                        scr.QtyPerASIN = ASIN.Qty;
                        scr.ProductDescription = i.KnownASIN.Title;

                        if (srr == null)
                            scr.QtySold = 0;
                        else
                            scr.QtySold = srr.QtySold * ASIN.Qty;

                        scr.RelatedProducts = db.RelatedProducts.Where(rp => rp.ProductID == ProductID && rp.ASIN == ASIN.ASIN);

                        Results.Add(scr);
                        TotalSales += scr.QtySold;
                    }
                }
                #endregion

                #region loop 2 - Calculate shipment and check related product limitations

                int TotalCalculatedShipment = 0;
                int LoopNumber = 0;
                bool TryAgain = true;
                decimal GoalTotalCalculatedShipment = QtyDec;
                decimal TotalSalesLessFixed = TotalSales;

                while (TryAgain)
                {

                    TotalCalculatedShipment = 0;

                    foreach (ShippingCalcResult scr in Results)
                    {
                        #region set CalculatedShipment
                        if (scr.Fixed == null) // if shipment amount not fixed, calculate it
                        {
                            if (TotalSalesLessFixed > 0) // no divide by zero
                                scr.PercentageOfSales = scr.QtySold / TotalSalesLessFixed;
                            else
                                scr.PercentageOfSales = 0;

                            // calculate shipment rounded down
                            scr.CalculatedShipment = decimal.Floor((decimal)scr.PercentageOfSales * GoalTotalCalculatedShipment / scr.QtyPerASIN) * scr.QtyPerASIN;
                        }
                        else
                            scr.CalculatedShipment = (decimal)scr.Fixed;

                        TotalCalculatedShipment += (int)scr.CalculatedShipment;
                        #endregion
                    }

                    #region increment proper records with leftover from round down
                    while (TotalCalculatedShipment != QtyDec)
                    {
                        ShippingCalcResult TheChosenOne = Results.Where(r => r.Fixed == null && r.QtyPerASIN == 1).OrderByDescending(r => r.PercentageOfSales * GoalTotalCalculatedShipment - r.CalculatedShipment).FirstOrDefault();

                        if (TheChosenOne == null)
                            break;

                        TheChosenOne.CalculatedShipment++;
                        TotalCalculatedShipment++;
                    }
                    #endregion

                    LoopNumber++;

                    if (LoopNumber == 2)
                        break;

                    #region Check for related product limitations

                    foreach (RelatedProductCalc rpc in RelatedProductCalcs)
                    {

                        var join = from rpca in RelatedProductCalcASINs
                                   join r in Results on rpca.ASIN equals r.ASIN
                                   where rpca.ProductID == rpc.ProductID
                                   select new { rpca, r };

                        decimal TotalPercentageOfSales = join.Sum(rec => (decimal)rec.r.PercentageOfSales);

                        GoalTotalCalculatedShipment = QtyDec;
                        TotalSalesLessFixed = TotalSales;
                        TryAgain = false;

                        foreach (var item in join)
                        {
                            #region write debug record
                            ViewBag.RelatedProductCalcsJoin += @"
                            <tr>
                                <td>" + LoopNumber.ToString() + @"</td>
                                <td>" + rpc.ProductName + @"</td>
                                <td>" + item.r.AmazonAccount.Name + @"</td>
                                <td>" + item.rpca.ASIN + @"</td>
                                <td>" + item.rpca.Ratio + @"</td>
                                <td>" + item.r.CalculatedShipment + @"</td>
                            </tr>
                        ";
                            #endregion

                            if (item.rpca.Ratio < 1) // if less of related product is available than needed
                            {
                                #region Calculate Max
                                //int NewMax = (int)decimal.Floor((decimal)item.rpca.Ratio * item.r.CalculatedShipment);

                                int NewMax = (int)(decimal.Floor((decimal)item.r.PercentageOfSales / TotalPercentageOfSales * rpc.QtyAvailable / item.rpca.RelatedProductQty) * item.rpca.ProductQty);

                                if (item.r.Max == null || NewMax < item.r.Max)
                                {
                                    item.r.Max = NewMax;
                                    item.r.Fixed = NewMax;
                                    //item.r.PercentageOfSales = null;
                                    GoalTotalCalculatedShipment -= NewMax;
                                    TotalSalesLessFixed -= item.r.QtySold;

                                    TryAgain = true;
                                }
                                #endregion
                            }
                        }

                        if (TryAgain == true) // if we did a calculation
                        {
                            #region increment proper records with leftover from round down
                            while (join.Sum(j => j.r.Max) != rpc.QtyAvailable)
                            {
                                var TheChosenOne = join.Where(j => j.rpca.RelatedProductQty == 1).OrderByDescending(j => ((decimal)j.r.PercentageOfSales / TotalPercentageOfSales * rpc.QtyAvailable) - j.r.Max).FirstOrDefault();

                                if (TheChosenOne == null)
                                    break;

                                TheChosenOne.r.Max += TheChosenOne.rpca.ProductQty;
                                TheChosenOne.r.Fixed += TheChosenOne.rpca.ProductQty;
                                GoalTotalCalculatedShipment -= TheChosenOne.rpca.ProductQty;
                            }
                            #endregion
                        }

                    }

                    #endregion

                }
                #endregion

            }
            #endregion

            return View(Results);
        }

        public ActionResult NeedInInventory(Nullable<int> CountryID, Nullable<Guid> VendorID, Nullable<Guid> ProductID, Nullable<Guid> AccountID, string Sort,String ForceRefresh)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            TimeSpan slidingExpiration = TimeSpan.FromMinutes(60.0);
            
            if (CountryID == null)
            {
                var Model = db.NeedInInventoryWithTotals
                    .Where(n => n.AmazonStockQty < n.QtySold && n.NeedInInventoryTotal > n.OnOrder)
                    .Select(n => new { n.CountryID, n.CountryName }).Distinct().OrderBy(n => n.CountryName)
                    .FromCache(System.Web.Caching.CacheItemPriority.Normal, slidingExpiration,(ForceRefresh!=null))
                    .ToList().Select(n => new { n.CountryID, n.CountryName }.ToExpando());

                ViewBag.SubView = "SelectCountry";
                ViewBag.Model = Model;
                if (ForceRefresh != null)
                {
                    ViewBag.Message = "Force Refresh Done";
                }
                return View();
            }
            else if (VendorID == null)
            {
                var qry = db.NeedInInventoryWithTotals
                   .Where(n => n.CountryID == CountryID)
                   .Select(n => new { n.VendorID, n.VendorName, n.ProductID, n.ProductName, n.NeedInInventory, n.OnOrder, n.FBAQty })
                   .GroupBy(n => new { n.VendorID, n.VendorName, n.ProductID, n.ProductName })
                   .Select(n => new { n.Key.VendorID, n.Key.VendorName, n.Key.ProductID, n.Key.ProductName, NeedInInventory = n.Sum(x => x.NeedInInventory), OnOrder = n.Average(x => x.OnOrder), FBAQty = n.Sum(x => x.FBAQty) })
                   .Where(n => n.NeedInInventory - n.OnOrder > 0)
                   .GroupBy(n => new { n.VendorID, n.VendorName, n.ProductID, n.ProductName })
                   .Select(n => new { n.Key.VendorID, n.Key.ProductID, n.Key.ProductName, n.Key.VendorName, NeedInInventory = n.Sum(x => x.NeedInInventory), OnOrder = n.Sum(x => x.OnOrder), FBAQty = n.Sum(x => x.FBAQty) })
                   .GroupJoin(db.ProductInventories.Where(pi => pi.LocationID == OmnimarkAmazon.Library.OrlandoLocationID), n => n.ProductID, pi => pi.ProductID, (n, pi) => new { n, pi })
                   .SelectMany(j => j.pi.DefaultIfEmpty(), (j, pi) => new { j.n, pi })
                   .OrderByDescending(j => j.n.NeedInInventory - j.n.OnOrder - (j.pi == null ? (decimal)0 : j.pi.Qty))
                   .GroupBy(j => new { j.n.VendorID, j.n.VendorName })
                   .FromCache(System.Web.Caching.CacheItemPriority.Normal, slidingExpiration, (ForceRefresh != null))
                   .Select(n => new { n.Key.VendorID, n.Key.VendorName, NeedInInventory = n.Sum(x => x.n.NeedInInventory), OnOrder = n.Sum(x => x.n.OnOrder), FBAQty = n.Sum(x => x.n.FBAQty), Qty = n.Sum(x => x.pi == null ? (decimal)0 : x.pi.Qty) })
                   .ToList();

                if (Sort == null || Sort == "Needed")
                    ViewBag.Model = qry.OrderByDescending(n => n.NeedInInventory - n.OnOrder-n.Qty).Select(n => new { n.VendorID, n.VendorName, n.NeedInInventory, n.OnOrder, n.FBAQty, n.Qty }.ToExpando());
                else
                    ViewBag.Model = qry.OrderBy(n => n.VendorName).Select(n => new { n.VendorID, n.VendorName, n.NeedInInventory, n.OnOrder,n.Qty, n.FBAQty }.ToExpando());

                ViewBag.SubView = "SelectVendor";
                ViewBag.CountryName = db.Countries.Single(c => c.Code == CountryID).CountryName;
                ViewBag.CountryID = CountryID;

                if (ForceRefresh != null)
                {
                    ViewBag.Message = "Force Refresh Done";
                }


                return View();
            }
            else if (ProductID == null)
            {
                var Model = db.NeedInInventoryWithTotals
                    .Where(n => n.VendorID == VendorID && n.CountryID == CountryID)
                    .Select(n => new { n.ProductID, n.ProductName, n.NeedInInventory, n.OnOrder, n.FBAQty })
                    .GroupBy(n => new { n.ProductID, n.ProductName })
                    .Select(n => new { n.Key.ProductID, n.Key.ProductName, FBAQty = n.Sum(x => x.FBAQty), NeedInInventory = n.Sum(x => x.NeedInInventory), OnOrder = n.Average(x => x.OnOrder) })
                    .GroupJoin(db.ProductInventories.Where(pi => pi.LocationID == OmnimarkAmazon.Library.OrlandoLocationID), n => n.ProductID, pi => pi.ProductID, (n, pi) => new { n, pi })
                    .SelectMany(j => j.pi.DefaultIfEmpty(), (j, pi) => new { j.n, pi })
                    .OrderByDescending(j => j.n.NeedInInventory - j.n.OnOrder - (j.pi == null ? (decimal)0 : j.pi.Qty))
                    .FromCache(System.Web.Caching.CacheItemPriority.Normal, slidingExpiration, (ForceRefresh != null))
                    .ToList()
                    .Select(j => new { j.n.ProductID, j.n.ProductName, j.n.NeedInInventory, j.n.OnOrder, Qty = j.pi == null ? (decimal)0 : j.pi.Qty, j.n.FBAQty }.ToExpando());

                ViewBag.SubView = "SelectProduct";
                ViewBag.Model = Model;
                ViewBag.CountryName = db.Countries.Single(c => c.Code == CountryID).CountryName;
                ViewBag.CountryID = CountryID;
                ViewBag.VendorName = db.Vendors.Single(v => v.ID == VendorID).Name;
                ViewBag.VendorID = VendorID;
                if (ForceRefresh != null)
                {
                    ViewBag.Message = "Force Refresh Done";
                }
                return View();
            }
            else if (AccountID == null)
            {
                var Model = db.NeedInInventoryWithTotals
                    .Where(n => n.VendorID == VendorID && n.ProductID == ProductID && n.CountryID == CountryID)
                    .GroupBy(n => new { n.AmazonAccountID, n.AccountName })
                    .Select(n => new { n.Key.AmazonAccountID, n.Key.AccountName, NeedInInventory = n.Sum(x => x.NeedInInventory) }).OrderBy(n => n.AccountName)
                    .FromCache(System.Web.Caching.CacheItemPriority.Normal, slidingExpiration, (ForceRefresh != null))
                    .ToList()
                    .Select(n => new { n.AmazonAccountID, n.AccountName, n.NeedInInventory }.ToExpando());

                ViewBag.SubView = "SelectAccount";
                ViewBag.Model = Model;
                ViewBag.CountryName = db.Countries.Single(c => c.Code == CountryID).CountryName;
                ViewBag.CountryID = CountryID;
                ViewBag.VendorName = db.Vendors.Single(v => v.ID == VendorID).Name;
                ViewBag.VendorID = VendorID;
                ViewBag.ProductName = db.Products.Single(p => p.ID == ProductID).Name;
                ViewBag.ProductID = ProductID;
                var OnOrder = db.OnOrders.Where(oo => oo.ProductID == ProductID).FirstOrDefault();

                ViewBag.OnOrder = OnOrder == null ? 0 : OnOrder.OnOrder1;
                if (ForceRefresh != null)
                {
                    ViewBag.Message = "Force Refresh Done";
                }
                return View();
            }
            else
            {
                var Model = db.NeedInInventoryWithTotals
                    .Where(n => n.VendorID == VendorID && n.ProductID == ProductID && n.AmazonAccountID == AccountID && n.CountryID == CountryID)
                    .FromCache(System.Web.Caching.CacheItemPriority.Normal, slidingExpiration, (ForceRefresh != null))
                    .ToList()
                    .Select(n => new { n.ASIN, n.QtySold, n.AmazonStockQty, n.Description }.ToExpando());

                ViewBag.SubView = "ViewASINs";
                ViewBag.Model = Model;
                ViewBag.CountryName = db.Countries.Single(c => c.Code == CountryID).CountryName;
                ViewBag.CountryID = CountryID;
                ViewBag.VendorName = db.Vendors.Single(v => v.ID == VendorID).Name;
                ViewBag.VendorID = VendorID;
                ViewBag.ProductName = db.Products.Single(p => p.ID == ProductID).Name;
                ViewBag.ProductID = ProductID;

                var OnOrder = db.OnOrders.Where(oo => oo.ProductID == ProductID).FirstOrDefault();

                ViewBag.OnOrder = OnOrder == null ? 0 : OnOrder.OnOrder1;
                ViewBag.AccountName = db.AmazonAccounts.Single(a => a.ID == AccountID).Name;
                ViewBag.AccountID = AccountID;
                if (ForceRefresh != null)
                {
                    ViewBag.Message = "Force Refresh Done";
                }

                return View();
            }
        }
        

   
        public ActionResult UpdateTitle(string id, string Title)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            db.KnownASINs.Single(ka => ka.ASIN == id).Title = Title;
            db.SaveChanges();

            return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SOSInventoryExport()
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            var model = db.SOSInventoryChangeReports.OrderByDescending(sicr => sicr.TimeStamp);

            return View(model);
        }

        public ActionResult CreateSOSInventoryExport()
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            SOSInventoryChangeReport r = new SOSInventoryChangeReport();
            r.ID = Guid.NewGuid();
            r.TimeStamp = DateTime.Now;

            db.SOSInventoryChangeReports.Add(r);

            db.SaveChanges();

            db.Database.ExecuteSqlCommand(@"
                update AmazonOrders set SOSInventoryChangeReportID='" + r.ID.ToString() + @"' where SOSInventoryChangeReportID is null and Status=3 and FulfillmentChannel=0 
                and LastStatusChangeNoticed > '1/6/2014'
            ");

            db.SaveChanges();

            return RedirectToAction("SOSInventoryExport");
        }

        public ActionResult GetSOSInventoryExport(Guid id)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            var ier = db.SOSInventoryChangeReports.Single(sicrx => sicrx.ID == id);

            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=SOS_Inventory_Change_Report_" + ier.TimeStamp.ToString("yyyyMMdd") + ".csv");

            var recs = db.Database.SqlQuery<string>(@"
                select '""' + p.Name + '"",' + convert(nvarchar(max), convert(numeric(9,0),Qty)) as ExportRow
                from SOSInventoryChangeReportView irv 
                join Products p on irv.ProductID = p.ID
                where SOSInventoryChangeReportID='" + id.ToString() + @"'
            ");

            StringBuilder sb = new StringBuilder();

            foreach (var rec in recs)
            {
                sb.Append(rec);
                sb.Append("\n");
            }

            return Content(sb.ToString());
        }

        class ASINsWithNoProductAssociationsRec
        {
            public Guid AmazonAccountID { get; set; }
            public string ASIN { get; set; }
            public DateTime RelevantDate { get; set; }
            public int CountryID { get; set; }
            public bool NotKnown { get; set; }
        }

        public ActionResult MissingProductAssociations()
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            var recs = db.Database.SqlQuery<ASINsWithNoProductAssociationsRec>("select * from ASINsWithNoProductAssociations").ToList();

            string Log = "";

            foreach (ASINsWithNoProductAssociationsRec rec in recs.Where(r => r.NotKnown))
            {
               // Amazon.AWS.ItemLookupResponseItemsItem item = OmnimarkAmazon.Library.ItemLookup(rec.CountryID, rec.ASIN, (br, txt) => Log += txt + (br ? "<br />" : ""));

                KnownASIN ka = new KnownASIN();
                ka.ASIN = rec.ASIN;
                ka.Filtered = false;
                ka.TimeStamp = DateTime.Now;
                ka.Reviewed = ka.TimeStamp;
                ka.OurProduct = true;
                ka.MarketPlaceID = db.Countries.Single(c => c.Code == rec.CountryID).AmazonMarketPlaceID;

                //if (item != null && item.ItemAttributes.Title != null)
                //    ka.Title = item.ItemAttributes.Title;

                db.KnownASINs.Add(ka);
            }

            db.SaveChanges();

            ViewBag.NewASINLog = Log;

            return View(recs.OrderBy(r => r.RelevantDate).Select(r => r.ASIN));
        }
    }

    public class MyExpando : DynamicObject
    {

        private Dictionary<string, object> _members =
                new Dictionary<string, object>();

        public MyExpando(Dictionary<string, object> Members)
        {
            _members = Members;
        }

        /// <summary>
        /// When a new property is set, 
        /// add the property name and value to the dictionary
        /// </summary>     
        public override bool TrySetMember
             (SetMemberBinder binder, object value)
        {
            if (!_members.ContainsKey(binder.Name))
                _members.Add(binder.Name, value);
            else
                _members[binder.Name] = value;

            return true;
        }

        /// <summary>
        /// When user accesses something, return the value if we have it
        /// </summary>      
        public override bool TryGetMember
               (GetMemberBinder binder, out object result)
        {
            if (_members.ContainsKey(binder.Name))
            {
                result = _members[binder.Name];
                return true;
            }
            else
            {
                return base.TryGetMember(binder, out result);
            }
        }

        /// <summary>
        /// If a property value is a delegate, invoke it
        /// </summary>     
        public override bool TryInvokeMember
           (InvokeMemberBinder binder, object[] args, out object result)
        {
            if (_members.ContainsKey(binder.Name)
                      && _members[binder.Name] is Delegate)
            {
                result = (_members[binder.Name] as Delegate).DynamicInvoke(args);
                return true;
            }
            else
            {
                return base.TryInvokeMember(binder, args, out result);
            }
        }


        /// <summary>
        /// Return all dynamic member names
        /// </summary>
        /// <returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _members.Keys;
        }


    }
}
