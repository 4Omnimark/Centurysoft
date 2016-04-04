using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Amazon.XML
{
    public class PriceWeb
    {
        [Required]
        public string SKU { get; set; }

        [Required]
        public Nullable<decimal> Price { get; set; }

        public PriceWeb()
        {
        }

        public PriceWeb(string SKU, Nullable<decimal> Price)
        {
            this.SKU = SKU;
            this.Price = Price;
        }
    }
}