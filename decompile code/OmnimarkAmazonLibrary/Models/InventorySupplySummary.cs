using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmnimarkAmazon.Models
{
    public class InventorySupplySummary
    {
        public string ASIN;
        public decimal TotalSupplyQuantity;
        public decimal InStockSupplyQuantity;
        public IEnumerable<FBAInventoryServiceMWS.Model.InventorySupply> SKUs;
    }
}
