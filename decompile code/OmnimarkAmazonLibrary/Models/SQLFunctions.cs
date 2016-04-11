using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Reflection;

namespace OmnimarkAmazon.Models
{
    public class SalesReportRecord
    {
        public Guid AmazonAccountID;
        public string ASIN;
        public decimal QtySold;
    }

    public class ProfitabilityReportRecord
    {
        public string ASIN { get; set; }
        public string Title { get; set; }
        public decimal QtySold { get; set; }
        public decimal QtySoldFiveStar { get; set; }
        public decimal QtySoldVitality { get; set; }
        public decimal QtySoldAdmarkia { get; set; }
        public decimal QtySoldPlatinumHealth { get; set; }
        public decimal QtySoldAmazonCanada { get; set; }
        public decimal QtySoldFrogPond { get; set; }
        public decimal QtySoldBrandzilla { get; set; }
        public decimal Sales { get; set; }
        public decimal SalesFiveStar { get; set; }
        public decimal SalesVitality { get; set; }
        public decimal SalesAdmarkia { get; set; }
        public decimal SalesPlatinumHealth { get; set; }
        public decimal SalesAmazonCanada { get; set; }
        public decimal SalesFrogPond { get; set; }
        public decimal SalesBrandzilla { get; set; }
        public decimal ProductCost { get; set; }
        public bool MissingCosts { get; set; }
        public decimal AveragePrice { get; set; }
        public Nullable<decimal> AverageProfitPerItem { get; set; }
        public Nullable<decimal> Profit { get; set; }
    }

    public class OrderProfitabilityRecord
    {
        public Guid AmazonAccountID { get; set; }
        public string AmazonAccountName { get; set; }
        public string AmazonOrderID { get; set; }
        public Nullable<int> Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal? Cost { get; set; }
        public decimal AmazonNet { get; set; }
        public string MissingProductAssociations { get; set; }
        public decimal NetNet { get; set; }
        public decimal Margin { get; set; }
        public int LineItemCount { get; set; }
        public string ASIN { get; set; }
        public string Title { get; set; }
        public decimal? LineItemQty { get; set; }
        public Int64 LineItemNumber { get; set; }
        public int? FulfillmentChannel { get; set; }
    }

    public class GetOrderProfitabilityByVendorRecord
    {
        //public DateTime OrderDate { get; set; }
        //public string Store{ get; set; }
        //public string OrderID { get; set; }
        //public string OrderLineNumber { get; set; }
        //public string ASIN { get; set; }
        //public string SKU { get; set; }
        //public string ASINTitle { get; set; }
        //public Nullable<decimal> Qty { get; set; }
        //public int? FulfillmentChannel { get; set; }
        //public decimal OrderTotal { get; set; }
        //public decimal AmazonNet { get; set; }
        //public decimal? Cost { get; set; }
        //public decimal NetNet { get; set; }
        //public decimal Margin { get; set; }   



        public decimal AmazonNet { get; set; }
        public string ASIN { get; set; }
        public string ASINTitle { get; set; }
        public decimal Cost { get; set; }
        public int FulfillmentChannel { get; set; }
        public decimal Margin { get; set; }
        public decimal NetNet { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderID { get; set; }
        public int OrderLineNumber { get; set; }
        public decimal OrderTotal { get; set; }
        public string Product { get; set; }
        public Guid ProductID { get; set; }
        public decimal Qty { get; set; }
        public string SKU { get; set; }
        public string Store { get; set; }
        public Guid StoreID { get; set; }
        public string Vendor { get; set; }
        public Guid VendorID { get; set; }
    }

    public class BuyBoxPercentageChangeReportRecord
    {
        public string ASIN { get; set; }
        public decimal BuyBox30Day { get; set; }
        public decimal BuyBox7Day { get; set; }
        public string ChangeRatio { get; set; }
        public string Title { get; set; }
    }

    public class SalesChangeReportRecord
    {
        public string ASIN { get; set; }
        public decimal Sales30Day { get; set; }
        public decimal Sales7Day { get; set; }
        public string ChangeRatio { get; set; }
        public string Title { get; set; }
    }

    public class InventoryValueReportRecord
    {
        public string Name { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public decimal StockQty { get; set; }
        public decimal StockQtyFiveStar { get; set; }
        public decimal StockQtyVitality { get; set; }
        public decimal StockQtyAdmarkia { get; set; }
        public decimal StockQtyPlatinumHealth { get; set; }
        public decimal StockQtyAmazonCanada { get; set; }
        public decimal StockQtyFrogPond { get; set; }
        public decimal StockQtyBrandzilla { get; set; }
        public decimal StockQtySuperQuick { get; set; }
        public decimal StockQtyNutramart { get; set; }
        public decimal StockQtyEurozoneMarketplace { get; set; }
        public decimal QtySold { get; set; }
        public decimal QtySoldUSA { get; set; }
        public decimal QtySoldFiveStar { get; set; }
        public decimal QtySoldVitality { get; set; }
        public decimal QtySoldAdmarkia { get; set; }
        public decimal QtySoldPlatinumHealth { get; set; }
        public decimal QtySoldAmazonCanada { get; set; }
        public decimal QtySoldFrogPond { get; set; }
        public decimal QtySoldBrandzilla { get; set; }
        public decimal QtySoldSuperQuick { get; set; }
        public decimal QtySoldNutramart { get; set; }
        public decimal QtySoldEurozoneMarketplace { get; set; }
        
    }

    public class InventoryManagementeBayRecord
    {
        public string ItemID { get; set; }
        public string Title { get; set; }
        public Nullable<decimal> Qty { get; set; }
    }

    public class KnownASINWithInventoryAndSales
    {
        public string ASIN { get; set; }
        public string Title { get; set; }
        public Nullable<decimal> StockQty { get; set; }
        public Nullable<decimal> StockQtyInbound { get; set; }
        public Nullable<DateTime> StockTimeStamp { get; set; }
        public Nullable<decimal> StockQtyFiveStar { get; set; }
        public Nullable<decimal> StockQtyInboundFiveStar { get; set; }
        public Nullable<DateTime> StockTimeStampFiveStar { get; set; }
        public Nullable<decimal> StockQtyVitality { get; set; }
        public Nullable<decimal> StockQtyInboundVitality { get; set; }
        public Nullable<DateTime> StockTimeStampVitality { get; set; }
        public Nullable<decimal> StockQtyAdmarkia { get; set; }
        public Nullable<decimal> StockQtyInboundAdmarkia { get; set; }
        public Nullable<DateTime> StockTimeStampAdmarkia { get; set; }

        public Nullable<decimal> StockQtyPlatinumHealth { get; set; }
        public Nullable<decimal> StockQtyInboundPlatinumHealth { get; set; }
        public Nullable<DateTime> StockTimeStampPlatinumHealth { get; set; }

        public Nullable<decimal> StockQtyAmazonCanada { get; set; }
        public Nullable<decimal> StockQtyInboundAmazonCanada { get; set; }
        public Nullable<DateTime> StockTimeStampAmazonCanada { get; set; }

        public Nullable<decimal> StockQtyFrogPond { get; set; }
        public Nullable<decimal> StockQtyInboundFrogPond { get; set; }
        public Nullable<DateTime> StockTimeStampFrogPond { get; set; }

        public Nullable<decimal> StockQtyBrandzilla { get; set; }
        public Nullable<decimal> StockQtyInboundBrandzilla { get; set; }
        public Nullable<DateTime> StockTimeStampBrandzilla { get; set; }
        public Nullable<decimal> StockQtySuperQuick { get; set; }
        public Nullable<decimal> StockQtyInboundSuperQuick { get; set; }
        public Nullable<DateTime> StockTimeStampSuperQuick { get; set; }
        public Nullable<decimal> StockQtyNutramart { get; set; }
        public Nullable<decimal> StockQtyEurozoneMarketplace { get; set; }
        public Nullable<decimal> StockQtyInboundNutramart { get; set; }
        public Nullable<decimal> StockQtyInboundEurozoneMarketplace { get; set; }
        public Nullable<DateTime> StockTimeStampNutramart { get; set; }
        public Nullable<DateTime> StockTimeStampEurozoneMarketplace { get; set; }
        public Nullable<decimal> QtySold { get; set; }
        public Nullable<decimal> QtySoldUSA { get; set; }
        public Nullable<decimal> QtySoldFiveStar { get; set; }
        public Nullable<decimal> QtySoldVitality { get; set; }
        public Nullable<decimal> QtySoldAdmarkia { get; set; }
        public Nullable<decimal> QtySoldPlatinumHealth { get; set; }
        public Nullable<decimal> QtySoldAmazonCanada { get; set; }
        public Nullable<decimal> QtySoldFrogPond { get; set; }
        public Nullable<decimal> QtySoldBrandzilla { get; set; }
        public Nullable<decimal> QtySoldSuperQuick { get; set; }
        public Nullable<decimal> QtySoldNutramart { get; set; }
        public Nullable<decimal> QtySoldEurozoneMarketplace { get; set; }
        public int ProductCount { get; set; }
        public decimal ProductCost { get; set; }
        public bool MissingCosts { get; set; }
        public Nullable<bool> BrandzillaMissingFBAListing { get; set; }
        public Nullable<bool> FiveStarMissingFBAListing { get; set; }
        public Nullable<bool> VitalityMissingFBAListing { get; set; }
        public Nullable<bool> AdmarkiaMissingFBAListing { get; set; }
        public Nullable<bool> PlatinumHealthMissingFBAListing { get; set; }
        public Nullable<bool> AmazonCanadaMissingFBAListing { get; set; }
        public Nullable<bool> FrogPondMissingFBAListing { get; set; }
        public Nullable<bool> SuperQuickMissingFBAListing { get; set; }
        public string OnOrder { get; set; }
        public string Vendors { get; set; }
        public Nullable<int> PageViews { get; set; }
        public Nullable<decimal> BuyBoxPercentage { get; set; }
        public bool IseBayListing { get; set; }
    }

    public class KnownASINWithInventoryAndSales2
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string ASIN { get; set; }
        public string Title { get; set; }
        public Nullable<decimal> StockQtyFiveStar { get; set; }
        public Nullable<decimal> StockQtyBrandzilla { get; set; }
        public Nullable<decimal> StockQtyAdmarkia { get; set; }
        public Nullable<decimal> StockQtyAmazonCanada { get; set; }
        public Nullable<decimal> StockQtyFrogPond { get; set; }
        public Nullable<decimal> StockQtyPlatinumHealth { get; set; }
        public Nullable<decimal> StockQtySuperQuick { get; set; }
        public Nullable<decimal> StockQtyNutramart { get; set; }
        public Nullable<decimal> StockQtyEurozoneMarketplace { get; set; }
        public Nullable<decimal> QtySold { get; set; }
    }

    public class ImportedTrafficRec
    {
        public DateTime Date { get; set; }
        public Guid AmazonAccountID { get; set; }
        public int Count { get; set; }
        public int DisplaySeq { get; set; }
    }

    public class UnreconciledAmazonOrdersShippedFromOrlando
    {
        public string AmazonOrderID { get; set; }
        public Guid ProductID { get; set; }
        public decimal AdjustmentQty { get; set; }
    }

    public class eBayCompleteOrderLines
    {
        public string OrderID { get; set; }
        public string ItemID { get; set; }
        public Nullable<Guid> ProductID { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public int SellingManagerSalesRecordNumber { get; set; }
    }

    public class MissingFBASKU
    {
        public Guid AmazonAccountID { get; set; }
        public string SKU { get; set; }
    }

    public class GetProductInventoryMoveListFromInboundFBAShipmentChanges
    {
        public Guid ProductID { get; set; }
        public string SKU { get; set; }
        public decimal Qty { get; set; }
    }

    public class SQLFunctions : DataContext
    {
        public SQLFunctions() : base(Startbutton.Library.GetConnectionString("Main"))
        {
            CommandTimeout = 1800;
        }

        public static IQueryable<SalesReportRecord> SalesReport(DateTime StartDate, int Days)
        {
            return (new SQLFunctions()).DoSalesReport(StartDate, Days);
        }

        [Function(Name = "dbo.SalesReport", IsComposable = true)]
        IQueryable<SalesReportRecord> DoSalesReport([Parameter(DbType = "DateTime")] DateTime StartDate, [Parameter(DbType = "int")] int Days)
        {
            return this.CreateMethodCallQuery<SalesReportRecord>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), StartDate, Days);
        }

        public static IQueryable<ProfitabilityReportRecord> ProfitabilityReport(DateTime StartDate, int Days)
        {
            return (new SQLFunctions()).DoProfitabilityReport(StartDate, Days);
        }

        [Function(Name = "dbo.ProfitabilityReport", IsComposable = true)]
        IQueryable<ProfitabilityReportRecord> DoProfitabilityReport([Parameter(DbType = "DateTime")] DateTime StartDate, [Parameter(DbType = "int")] int Days)
        {
            return this.CreateMethodCallQuery<ProfitabilityReportRecord>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), StartDate, Days);
        }

        public static IQueryable<KnownASINWithInventoryAndSales> KnownASINsWithInventoryAndSales(Nullable<Guid> AmazonAccountID, DateTime StartDate, int Days, bool USAOnly)
        {
            return (new SQLFunctions()).DoKnownASINsWithInventoryAndSales(AmazonAccountID, StartDate, Days, USAOnly);
        }

        [Function(Name = "dbo.KnownASINsWithInventoryAndSales", IsComposable = true)]
        IQueryable<KnownASINWithInventoryAndSales> DoKnownASINsWithInventoryAndSales([Parameter(DbType = "uniqueidentifier")] Nullable<Guid> AmazonAccountID, [Parameter(DbType = "DateTime")] DateTime StartDate, [Parameter(DbType = "int")] int Days, [Parameter(DbType = "bit")] bool USAOnly)
        {
            return this.CreateMethodCallQuery<KnownASINWithInventoryAndSales>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), AmazonAccountID, StartDate, Days, USAOnly);
        }

        public static IQueryable<KnownASINWithInventoryAndSales2> KnownASINsWithInventoryAndSales2(DateTime StartDate, int Days)
        {
            return (new SQLFunctions()).DoKnownASINsWithInventoryAndSales2(StartDate, Days);
        }

        [Function(Name = "dbo.KnownASINsWithInventoryAndSales2", IsComposable = true)]
        IQueryable<KnownASINWithInventoryAndSales2> DoKnownASINsWithInventoryAndSales2([Parameter(DbType = "DateTime")] DateTime StartDate, [Parameter(DbType = "int")] int Days)
        {
            return this.CreateMethodCallQuery<KnownASINWithInventoryAndSales2>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), StartDate, Days);
        }

        public static IQueryable<InventoryValueReportRecord> InventoryValueReport(DateTime StartDate, int Days, bool USAOnly)
        {
            return (new SQLFunctions()).DoInventoryValueReport(StartDate, Days, USAOnly);
        }

        [Function(Name = "dbo.ProductsWithInventoryAndSales", IsComposable = true)]
        IQueryable<InventoryValueReportRecord> DoInventoryValueReport([Parameter(DbType = "DateTime")] DateTime StartDate, [Parameter(DbType = "int")] int Days, [Parameter(DbType = "bit")] bool USAOnly)
        {
            return this.CreateMethodCallQuery<InventoryValueReportRecord>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), StartDate, Days, USAOnly);
        }

        public static IQueryable<int> UpdateProductVendors()
        {
            return (new SQLFunctions()).DoUpdateProductVendors();
        }

        [Function(Name = "dbo.UpdateProductVendors", IsComposable = true)]
        IQueryable<int> DoUpdateProductVendors()
        {
            return this.CreateMethodCallQuery<int>(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
        }


    }
}
