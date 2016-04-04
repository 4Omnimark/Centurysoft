using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OmnimarkAmazon.Models
{
    [MetadataType(typeof(AutomaticPriceChangeWebMetaData))]
    public class AutomaticPriceChangeWeb : AutomaticPriceChange
    {

        [Required]
        public Nullable<int> TimeSpanCount { get; set; }

        [Required]
        public Nullable<int> TimeSpanSpanType { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yyyy}")]
        public new Nullable<DateTime> TempPriceStartDateTime { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yyyy}")]
        public new Nullable<DateTime> TempPriceEndDateTime { get; set; }

        [Required]
        public string TempPriceStartTime { get; set; }

        [Required]
        public string TempPriceEndTime { get; set; }

        [Required]
        public new Nullable<decimal> TempPrice { get; set; }

        [Required]
        public new Nullable<decimal> RegularPrice { get; set; }

        public new Nullable<Guid> AmazonAccountID { get; set; }

        class AutomaticPriceChangeWebMetaData
        {
            [Required]
            public string SKU { get; set; }

        }
    }
}