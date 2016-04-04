using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OmnimarkAmazonWeb.Models
{
   public class UkProhibitedKeywords
    {
       [Required(ErrorMessage = "Please Enter Keyword")]
       public string keyname { get; set; }
    }
}
