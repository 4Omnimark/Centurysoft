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
    
    public partial class Vendor
    {
        public Vendor()
        {
            this.ReceivedShipments = new HashSet<ReceivedShipment>();
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
            this.Products = new HashSet<Product>();
        }
    
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string Phone { get; set; }
        public string ContactNameFirst { get; set; }
        public string ContactNameLast { get; set; }
        public string ContactNameMiddle { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string PostalCode { get; set; }
        public string StateOrProvince { get; set; }
    
        public virtual ICollection<ReceivedShipment> ReceivedShipments { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
