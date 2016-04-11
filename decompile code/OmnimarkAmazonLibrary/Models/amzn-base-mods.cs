using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.XML
{
    public partial class Price
    {
        public Price()
        {
        }

        public Price(string SKU, decimal Price)
        {
            this.SKU = SKU;
            var c = new Amazon.XML.OverrideCurrencyAmount();
            c.currency = Amazon.XML.BaseCurrencyCodeWithDefault.USD;
            c.Value = Price;
            this.StandardPrice = c;
        }

    }

    public partial class OverrideCurrencyAmount
    {
        public OverrideCurrencyAmount()
        {
        }

        public OverrideCurrencyAmount(decimal Value)
        {
            this.Value = Value;
            this.currency = BaseCurrencyCodeWithDefault.USD;
        }

    }
}
