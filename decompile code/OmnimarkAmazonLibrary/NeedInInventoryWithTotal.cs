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
    
    public partial class NeedInInventoryWithTotal
    {
        public string ASIN { get; set; }
        public System.Guid AmazonAccountID { get; set; }
        public System.Guid ProductID { get; set; }
        public System.Guid VendorID { get; set; }
        public Nullable<decimal> QtySold { get; set; }
        public Nullable<decimal> AmazonStockQty { get; set; }
        public Nullable<decimal> NeedInInventory { get; set; }
        public decimal OnOrder { get; set; }
        public string Description { get; set; }
        public string AccountName { get; set; }
        public string ProductName { get; set; }
        public string VendorName { get; set; }
        public decimal NeedInInventoryTotal { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public Nullable<decimal> FBAQty { get; set; }
    }
}
