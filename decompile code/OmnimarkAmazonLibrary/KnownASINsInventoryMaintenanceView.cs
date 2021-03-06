//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OmnimarkAmazon.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KnownASINsInventoryMaintenanceView
    {
        public string ASIN { get; set; }
        public string SearchTerm { get; set; }
        public string Title { get; set; }
        public bool Filtered { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> LastSeen { get; set; }
        public Nullable<System.DateTime> Reviewed { get; set; }
        public bool OurProduct { get; set; }
        public Nullable<System.Guid> AmazonAccountID { get; set; }
        public Nullable<decimal> AmazonStockQty { get; set; }
        public Nullable<System.DateTime> AmazonStockTimeStamp { get; set; }
        public string MarketPlaceID { get; set; }
        public Nullable<System.DateTime> LastScrape { get; set; }
        public Nullable<System.DateTime> LastScrapeTry { get; set; }
        public Nullable<System.DateTime> FirstScrapeFail { get; set; }
        public Nullable<System.DateTime> LastScrapeFail { get; set; }
        public Nullable<int> ScrapeFailCount { get; set; }
        public bool GoneFromAmazon { get; set; }
        public Nullable<decimal> OurLowestPrice { get; set; }
        public Nullable<decimal> OurMostRecentUSListingPrice { get; set; }
        public Nullable<System.DateTime> OurMostRecentUSListingOpenDate { get; set; }
        public Nullable<System.Guid> OurMostRecentUSListingAccountID { get; set; }
        public string ImageName { get; set; }
        public string MergedIntoASIN { get; set; }
    }
}
