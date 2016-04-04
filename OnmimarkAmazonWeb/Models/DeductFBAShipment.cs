using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmnimarkAmazonWeb.Models
{
    public class DeductFBAShipment
    {
        public string ShipmentID { get; set; }
        public decimal Qty { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string AccountName { get; set; }
    }
}