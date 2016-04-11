using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OmnimarkAmazon.Models
{
    [MetadataType(typeof(ReceivedShipmentAdMetaData))]
    public partial class ReceivedShipment
    {
        class ReceivedShipmentAdMetaData
        {
            [Required]
            [Display(Name = "Receipt Date")]
            public DateTime Date { get; set; }

            [Required]
            [Display(Name = "Vendor")]
            public string VendorID { get; set; }
        }
    }
}
