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
    
    public partial class InboundFBAShipmentProductInventoryAdjustment
    {
        public System.Guid ID { get; set; }
        public string InboundFBAShipmentID { get; set; }
        public Nullable<System.Guid> ProductInventoryAdjustmentBatchID { get; set; }
        public System.DateTime TimeStamp { get; set; }
    
        public virtual InboundFBAShipment InboundFBAShipment { get; set; }
        public virtual ProductInventoryAdjustmentBatch ProductInventoryAdjustmentBatch { get; set; }
    }
}
