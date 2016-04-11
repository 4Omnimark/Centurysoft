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
    
    public partial class PurchaseOrderLine
    {
        public System.Guid ID { get; set; }
        public System.Guid PurchaseOrderID { get; set; }
        public int LineNumber { get; set; }
        public System.Guid ProductID { get; set; }
        public decimal Qty { get; set; }
        public decimal Cost { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.Guid> ReceivedProductID { get; set; }
        public Nullable<decimal> ActualCost { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual ReceivedProduct ReceivedProduct { get; set; }
    }
}