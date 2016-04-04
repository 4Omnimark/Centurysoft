using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OmnimarkAmazonWeb.Models
{
    public class AddKeywords
    {
        [Required(ErrorMessage="Please Enter Keyword")]
        public string keyword { get; set; }
    }
}