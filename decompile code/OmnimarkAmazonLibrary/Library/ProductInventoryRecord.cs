using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmnimarkAmazon.Models
{
    public class InventoryReportRecord
    {
        public Guid ProductID;
        public Nullable<decimal> ActualCost;
        public string Name;
        public Dictionary<string, Nullable<decimal>> Qtys = new Dictionary<string, Nullable<decimal>>();
        public Nullable<decimal> TotalValue;
    }


}
