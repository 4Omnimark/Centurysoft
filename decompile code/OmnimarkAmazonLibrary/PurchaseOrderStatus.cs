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
    
    public partial class PurchaseOrderStatus
    {
        public PurchaseOrderStatus()
        {
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
        }
    
        public string ID { get; set; }
        public string Name { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<int> DisplaySeq { get; set; }
        public string CID { get; set; }
        public string CName { get; set; }
    
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
