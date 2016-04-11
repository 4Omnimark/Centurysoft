using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OmnimarkAmazon.Models
{
    [MetadataType(typeof(VendorMetaData))]
    public partial class Vendor
    {
        class VendorMetaData
        {
            [Required]
            [Display(Name = "Vendor Name")]
            public string Name { get; set; }
            
            [Display(Name = "Phone Number")]
            public string Phone { get; set; }

            [Display(Name = "Contact First Name")]
            public string ContactNameFirst { get; set; }

            [Display(Name = "Contact Middle Initial")]
            [StringLength(1)]
            public string ContactNameMiddle { get; set; }

            [Display(Name = "Contact Last Name")]
            public string ContactNameLast { get; set; }

            [Display(Name = "Address Line 1")]
            public string Address1 { get; set; }

            [Display(Name = "Address Line 2")]
            public string Address2 { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "State")]
            public string StateID { get; set; }

            [Display(Name = "State")]
            public string StateOrProvince { get; set; }

            [Display(Name = "Country")]
            public string CountryID { get; set; }

            [Display(Name = "Zip/Postal Code")]
            public string PostalCode { get; set; }

            [Display(Name = "Email")]
            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Not a valid email address")]
            public string Email { get; set; }

            [Display(Name = "Fax")]
            public string Fax { get; set; }

        }
    }
}
