using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmnimarkAmazon.Models
{
    public class ShippingCalcResult
    {
        public Guid AmazonAccountID;
        public string ASIN;
        public decimal QtySold;
        public decimal QtyPerASIN;
        public decimal CalculatedShipment;
        public Nullable<decimal> PercentageOfSales;
        public IEnumerable<RelatedProduct> RelatedProducts;
        public Nullable<decimal> Max;
        public Nullable<decimal> Fixed;
        public string ProductDescription;

        Entities db;
        AmazonAccount _AmazonAccount;

        public ShippingCalcResult(Entities db)
        {
            this.db = db;
        }

        public AmazonAccount AmazonAccount
        {
            get
            {
                if (_AmazonAccount == null)
                    _AmazonAccount = db.AmazonAccounts.Single(aa => aa.ID == AmazonAccountID);

                return _AmazonAccount;
            }
        }
    }
}
