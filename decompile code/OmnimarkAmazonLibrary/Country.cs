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
    
    public partial class Country
    {
        public Country()
        {
            this.Vendors = new HashSet<Vendor>();
            this.AmazonAccounts = new HashSet<AmazonAccount>();
        }
    
        public int Code { get; set; }
        public string CountryName { get; set; }
        public string C2dig { get; set; }
        public string C3dig { get; set; }
        public string AmazonMarketPlaceID { get; set; }
        public string AWSAccessKeyID { get; set; }
        public string AWSSecretAccessKey { get; set; }
        public string ServiceURL { get; set; }
        public string OrderServiceURL { get; set; }
        public string ProductsServiceURL { get; set; }
        public string InventoryServiceURL { get; set; }
        public string AWSServiceURL { get; set; }
        public string AWSXMLNamespace { get; set; }
        public string DefaultItemDetailsURL { get; set; }
        public string DateTimeFormat { get; set; }
        public string DefaultURL { get; set; }
        public string CurrencySymbolPrefix { get; set; }
    
        public virtual ICollection<Vendor> Vendors { get; set; }
        public virtual ICollection<AmazonAccount> AmazonAccounts { get; set; }
    }
}
