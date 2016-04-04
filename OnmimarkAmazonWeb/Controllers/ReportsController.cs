using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmnimarkAmazon.Models;
using Startbutton.Web.ExtensionMethods;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace OmnimarkAmazonWeb.Controllers
{
    public class ReportsController : _BaseController
    {
        public JsonResult Report(string ViewName, object model, Dictionary<string, object> RtnData = null)
        {
            if (RtnData == null)
                RtnData = new Dictionary<string, object>();

            ViewBag.RtnData = RtnData;

            RtnData["html"] = Startbutton.Web.Library.RenderPartialViewToString(ControllerContext, ViewData, TempData, ViewName, model);

            return new JsonResult()
            {
                Data = RtnData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }
        public ActionResult Index()
        {
            ViewBag.Title = "Reports";
            return View();
        }
        public ActionResult StaleInventory()
        {
            ViewBag.Title = "Stale Inventory Report";
            return View();
        }
        public ActionResult StaleInventoryReport(Nullable<int> Days)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            int days = Days == null ? 30 : (int)Days;

            DateTime XDaysAgo = DateTime.Today.AddDays(-days);
            
            var model = SQLFunctions.KnownASINsWithInventoryAndSales(null, XDaysAgo, days, false).Where(ka => ka.StockQty != null && ka.StockQty - ka.StockQtyInbound > 0).OrderByDescending(ka => (((decimal)ka.StockQty - ka.StockQtyInbound) / (decimal)((ka.QtySold == null ? .00001 : (ka.QtySold == 0 ? .00001 : (double)ka.QtySold)) * 100)));
            
            Dictionary<string, object> RtnData = new Dictionary<string, object>();

            RtnData["Days"] = days;

            return Report(Action, model, RtnData);

        }
        public ActionResult BuyBoxChange()
        {
            ViewBag.Title = "Buy Box Percentage Change Report";
            return View();
        }
        public ActionResult BuyBoxChangeReport()
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            var model = db.Database.SqlQuery<BuyBoxPercentageChangeReportRecord>("GetReportBuyBoxPercentageChange");

            return Report(Action, model);

        }
        public ActionResult SalesChange()
        {
            ViewBag.Title = "Sales Change Report";
            return View();
        }
        public ActionResult SalesChangeReport()
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            var model = db.Database.SqlQuery<SalesChangeReportRecord>("GetReportSalesChange");

            return Report(Action, model);

        }
        public ActionResult OutPriced()
        {
            ViewBag.Title = "Out Priced Report";
            return View();
        }
        public ActionResult OutPricedReport()
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            var model = db.OutPriceds.OrderByDescending(op => op.AmazonFulfilled).ThenByDescending(op => op.StockQty).ThenBy(op => op.CompetitorMinPrice / op.OurMinPrice);

            return Report(Action, model);

        }
        public ActionResult SpecialPriceSales()
        {
            ViewBag.Title = "Special Price Sales Report";
            return View();
        }
        public ActionResult SpecialPriceSalesReport(Nullable<DateTime> StartDate, Nullable<DateTime> EndDate)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            if (EndDate == null)
                EndDate = DateTime.Today;

            if (StartDate == null)
                StartDate = ((DateTime)EndDate).AddDays(-30);

            var model = db.SpecialPriceSalesByDays.Where(sps => sps.PurchaseDate >= StartDate && sps.PurchaseDate <= EndDate).GroupBy(sps => sps.ASIN).ToList().Select(g => new { ASIN = g.Key, Qty = g.Sum(sps => sps.Qty), Title = g.Min(sps => sps.Title), SpecialPrice = g.Min(sps => sps.SpecialPrice) }.ToExpando());

            Dictionary<string, object> RtnData = new Dictionary<string, object>();

            RtnData["StartDate"] = ((DateTime)StartDate).ToString("MM/dd/yyyy");
            RtnData["EndDate"] = ((DateTime)EndDate).ToString("MM/dd/yyyy");

            return Report(Action, model, RtnData);

        }
        public ActionResult Profitability()
        {
            ViewBag.Title = "ASIN Profitability Report";
            return View();
        }
        public ActionResult ProfitabilityReport(Nullable<DateTime> StartDate, Nullable<DateTime> EndDate)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            if (EndDate == null)
                EndDate = DateTime.Today;

            if (StartDate == null)
                StartDate = ((DateTime)EndDate).AddDays(-30);

            int Days = (int)(((DateTime)EndDate).ToOADate() - ((DateTime)StartDate).ToOADate()) + 1;

            var model = SQLFunctions.ProfitabilityReport((DateTime)StartDate, Days).Where(pr => pr.QtySold > 0).OrderByDescending(pr => pr.Profit).ThenByDescending(pr => pr.Sales);

            Dictionary<string, object> RtnData = new Dictionary<string, object>();

            RtnData["StartDate"] = ((DateTime)StartDate).ToString("MM/dd/yyyy");
            RtnData["EndDate"] = ((DateTime)EndDate).ToString("MM/dd/yyyy");

            return Report(Action, model, RtnData);

        }
        public ActionResult InventoryValue()
        {
            ViewBag.Title = "Inventory Value Report";
            return View();
        }
        public ActionResult InventoryValueReport(Nullable<DateTime> StartDate, Nullable<DateTime> EndDate, bool USAOnly = false)
        {
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            if (StartDate == null)
                StartDate = DateTime.Today.AddDays(-30);

            if (EndDate == null)
                EndDate = DateTime.Today;

            double Days = ((DateTime)EndDate).ToOADate() - ((DateTime)StartDate).ToOADate();

            var Locations = db.ProductInventoryLocations.Where(x => x.Name == "Orlando").ToList();
            var Stores = db.AmazonAccounts.Where(s => s.Enabled == true).OrderBy(s => s.DisplaySeq).ToList();

            Random r = new Random();
            int rn = r.Next();

            DataTable dt = Startbutton.Library.ExecuteSQLToDataTable("Main", @"
                exec ProductsWithInventory 'pwi" + rn.ToString() + @"'
                exec ProductSalesAmazonStores '" + StartDate.ToString() + "', " + Days.ToString() + @", 'psas" + rn.ToString() + @"'
                select * from pwi" + rn.ToString() + @" pwi left join psas" + rn.ToString() + @" psas on  pwi.ID = psas.ID
                drop table pwi" + rn.ToString() + @"
                drop table psas" + rn.ToString() + @"
            ");

            Dictionary<string, object> RtnData = new Dictionary<string, object>();
            List<InventoryReportRecord> model = new List<InventoryReportRecord>();

            decimal TotalInventoryValue = 0;

            foreach (DataRow row in dt.Rows)
            {
                InventoryReportRecord ir = new InventoryReportRecord();
                ir.ProductID = (Guid)row["ID"];
                ir.Name = (string)row["Name"];

                Nullable<decimal> Cost = null;

                if (row["Cost"] != DBNull.Value)
                    Cost = (decimal)row["ActualCost"];

                ir.ActualCost = Cost;

                decimal Total = 0;

                foreach (var loc in Locations)
                    if (row["Qty" + loc.ShortCode] == DBNull.Value)
                        ir.Qtys.Add(loc.ShortCode, null);
                    else
                    {
                        decimal Qty = (decimal)row["Qty" + loc.ShortCode];
                        ir.Qtys.Add(loc.ShortCode, Qty);
                        Total += Qty;

                        if (Cost != null)
                        {
                            if (!RtnData.ContainsKey("TotalValue" + loc.ShortCode))
                                RtnData["TotalValue" + loc.ShortCode] = (decimal)0;

                            RtnData["TotalValue" + loc.ShortCode] = ((decimal)RtnData["TotalValue" + loc.ShortCode]) + (((decimal)row["Qty" + loc.ShortCode]) * (decimal)Cost);
                        }


                    }

                foreach (var s in Stores)
                    if (row["AmazonQty" + s.CharID] == DBNull.Value)
                        ir.Qtys.Add("Amz" + s.CharID, null);
                    else
                    {
                        decimal Qty = (decimal)row["AmazonQty" + s.CharID];
                        ir.Qtys.Add("Amz" + s.CharID, Qty);
                        Total += Qty;

                        if (Cost != null)
                        {
                            if (!RtnData.ContainsKey("TotalValue" + "Amz" + s.CharID))
                                RtnData["TotalValue" + "Amz" + s.CharID] = (decimal)0;

                            RtnData["TotalValue" + "Amz" + s.CharID] = ((decimal)RtnData["TotalValue" + "Amz" + s.CharID]) + (((decimal)row["AmazonQty" + s.CharID]) * (decimal)Cost);
                        }
                    }

                ir.Qtys.Add("Total", Total);
                if (row["TotalQtySold"] == DBNull.Value)
                    ir.Qtys.Add("TotalQuantitySold", (decimal)0);
                else
                    ir.Qtys.Add("TotalQuantitySold", (decimal)row["TotalQtySold"]);

                if (row["Cost"] == DBNull.Value)
                    ir.TotalValue = null;
                else
                {
                    ir.TotalValue = Total * Cost;
                    TotalInventoryValue += (decimal)ir.TotalValue;
                }

                model.Add(ir);

            }

            dt.Dispose();
            dt = null;

            RtnData["StartDate"] = ((DateTime)StartDate).ToString("MM/dd/yyyy");
            RtnData["EndDate"] = ((DateTime)EndDate).ToString("MM/dd/yyyy");
            RtnData["Days"] = Days;
            RtnData["USAOnly"] = USAOnly;
            RtnData["TotalInventoryValue"] = "$" + TotalInventoryValue.ToString("###,###,###,###.00");

            ViewBag.Locations = Locations;
            ViewBag.Stores = Stores;

            return Report(Action, model.OrderByDescending(m => m.TotalValue), RtnData);

        }
        public ActionResult OrderProfitability()
        {
            ViewBag.Title = "Profitability by Order Report";
            return View();
        }
        public ActionResult OrderProfitabilityReport(Nullable<DateTime> StartDate, Nullable<DateTime> EndDate)
        {

            if (EndDate == null)
                EndDate = DateTime.Today;

            if (StartDate == null)
                StartDate = ((DateTime)EndDate).AddDays(-7);
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;

            //var model = db.Database.SqlQuery<OrderProfitabilityRecord>("select * from OrderProfitabilityWithLineItems where PurchaseDate >= '" + StartDate.ToString() + "' and PurchaseDate <= '" + (((DateTime)EndDate).AddDays(1)).ToString() + "' order by PurchaseDate desc").ToList();
            var model = db.Database.SqlQuery<OrderProfitabilityRecord>("GetOrderProfitabilityWithLineItems'" + StartDate.ToString() + "','" + (((DateTime)EndDate).AddDays(1)).ToString() + "'").ToList();
            foreach (var item in model)
            {
                if (item.Cost != null)
                {
                    item.NetNet = (decimal)item.AmazonNet - (decimal)item.Cost;
                    item.Margin = item.NetNet / item.OrderTotal;
                }

            }

            Dictionary<string, object> RtnData = new Dictionary<string, object>();

            RtnData["StartDate"] = ((DateTime)StartDate).ToString("MM/dd/yyyy");
            RtnData["EndDate"] = ((DateTime)EndDate).ToString("MM/dd/yyyy");

            return Report(Action, model, RtnData);

        }
        public ActionResult OrderProfitabilityByVendor()
        {
            ViewBag.Title = "OrderProfitabilityByVendor";
            return View();
        }
        public ActionResult OrderProfitabilityByVendorReport(Guid? VendorID, Guid? ProductID, string ASIN, DateTime? StartDate, DateTime? EndDate)
        {

            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
            Func<GetOrderProfitabilityByVendorRecord, bool> predicate = null;
            Func<GetOrderProfitabilityByVendorRecord, bool> func2 = null;
            Func<GetOrderProfitabilityByVendorRecord, bool> func3 = null;
            if (EndDate == null)
            {
                EndDate = new DateTime?(DateTime.Today);
            }
            if (StartDate == null)
            {
                StartDate = new DateTime?(EndDate.Value.AddDays(-7.0));
            }
            Dictionary<string, object> rtnData = new Dictionary<string, object>();
            rtnData["StartDate"] = StartDate.Value.ToString("MM/dd/yyyy");
            rtnData["EndDate"] = EndDate.Value.ToString("MM/dd/yyyy");
            if ((VendorID != null || ProductID != null) && !string.IsNullOrEmpty(ASIN))
            {
                throw new Exception("Only ProductID/VendorID or ASIN may be set.");
            }
            List<GetOrderProfitabilityByVendorRecord> source = new List<GetOrderProfitabilityByVendorRecord>();
            if (VendorID != null || !string.IsNullOrEmpty(ASIN))
            {
                rtnData["VendorID"] = VendorID;
                rtnData["ProductID"] = ProductID;
                rtnData["ASIN"] = ASIN;
                if (string.IsNullOrEmpty(ASIN))
                {
                    if (ProductID == null)
                    {
                        if (predicate == null)
                        {
                            predicate = delegate(GetOrderProfitabilityByVendorRecord gop)
                            {
                                DateTime orderDate = gop.OrderDate;
                                DateTime? startDate = StartDate;
                                if (!(startDate != null ? (orderDate >= startDate.GetValueOrDefault()) : false))
                                {
                                    return false;
                                }
                                DateTime time2 = gop.OrderDate;
                                DateTime? endDate = EndDate;
                                if (endDate == null)
                                {
                                    return false;
                                }
                                return time2 <= endDate.GetValueOrDefault();
                            };
                        }
                        source = db.Database.SqlQuery<GetOrderProfitabilityByVendorRecord>("GetOrderProfitabilityByVendor '" + VendorID.ToString() + "'", new object[0]).ToList().Where(predicate).ToList();
                    }
                    else
                    {
                        if (func2 == null)
                        {
                            func2 = delegate(GetOrderProfitabilityByVendorRecord gop)
                            {
                                DateTime orderDate = gop.OrderDate;
                                DateTime? startDate = StartDate;
                                if (!(startDate != null ? (orderDate >= startDate.GetValueOrDefault()) : false))
                                {
                                    return false;
                                }
                                DateTime time2 = gop.OrderDate;
                                DateTime? endDate = EndDate;
                                if (endDate == null)
                                {
                                    return false;
                                }
                                return time2 <= endDate.GetValueOrDefault();
                            };
                        }
                        source = db.Database.SqlQuery<GetOrderProfitabilityByVendorRecord>("GetOrderProfitability '" + VendorID.ToString() + "', '" + ProductID.ToString() + "'", new object[0]).Where(func2).ToList();
                    }
                    if (source.Count > 0)
                    {
                        rtnData["Vendor"] = source.First().Vendor;
                    }
                    else
                    {
                        rtnData["Vendor"] = db.Vendors.Single(v => (v.ID == VendorID)).Name;
                    }
                }
                else
                {
                    if (func3 == null)
                    {
                        func3 = delegate(GetOrderProfitabilityByVendorRecord gop)
                        {
                            DateTime orderDate = gop.OrderDate;
                            DateTime? startDate = StartDate;
                            if (!(startDate != null ? (orderDate >= startDate.GetValueOrDefault()) : false))
                            {
                                return false;
                            }
                            DateTime time2 = gop.OrderDate;
                            DateTime? endDate = EndDate;
                            if (endDate == null)
                            {
                                return false;
                            }
                            return time2 <= endDate.GetValueOrDefault();
                        };
                    }
                    source = db.Database.SqlQuery<GetOrderProfitabilityByVendorRecord>("GetOrderProfitabilityByASIN '" + ASIN.ToString() + "'", new object[0]).Where(func3).ToList();
                }
            }
            foreach (GetOrderProfitabilityByVendorRecord record in source)
            {
                decimal cost = record.Cost;
                record.NetNet = record.AmazonNet - record.Cost;
                if (record.OrderTotal == (decimal)0)
                {
                    record.Margin = (decimal)0;
                }
                else
                {
                    record.Margin = record.NetNet / record.OrderTotal;
                }
            }
            rtnData["Totals"] = from d in source
                                group d by 1 into g
                                select new { OrderTotal = g.Sum(d => d.OrderTotal), Cost = g.Sum(d => d.Cost), NetNet = g.Sum(d => d.NetNet), Margin = g.Sum(d => d.NetNet) / g.Sum(d => d.OrderTotal) };
            return this.Report(Action, source, rtnData);
        }
    }
}
    

