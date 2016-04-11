using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmnimarkAmazon.Models
{
    public class RelatedProductCalcASIN
    {
        public Guid ProductID;
        public string ASIN;
        public Nullable<decimal> Ratio;
        public decimal CalculatedShipment;
        public decimal ProductQty;
        public decimal RelatedProductQty;
    }
}
