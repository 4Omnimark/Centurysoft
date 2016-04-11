using System;

namespace OmnimarkAmazon.Models
{
    public class MoveInventoryFromInboundFBAShipmentRec
    {
        public string ShipmentID { get; set; }
        public Guid ProductID { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public string ProductName { get; set; }
        public string AmazonAccountName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string SKU { get; set; }
        public string ASIN { get; set; }
        public Nullable<Int32> SKUQty { get; set; }
        public Nullable<decimal> QuantityShipped { get; set; }
        public decimal Unit { get; set; }
        public Nullable<decimal> TotalShip { get; set; }

             
    }
}
