using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OmnimarkAmazonWeb.Models
{
    public class UKListingModel
    {
        public String Category { get; set; }
        public String filename { get; set; }
        public Nullable<decimal> rownumber { get; set; }
       [Required(ErrorMessage = "Please Enter Value in this Field.")]
        public double PriceValue { get; set; }
       public bool chkresult { get; set; }
    }
}