using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;

namespace OmnimarkAmazon.Models
{
    [MetadataType(typeof(SearchTermMetaData))]
    public partial class SearchTerm
    {
        class SearchTermMetaData
        {
            [Required]
            [Display(Name = "Search Term")]
            [StringLength(100, ErrorMessage = "Search Term must not exceed 100 characters!")]
            public string Term { get; set; }
        }
    }
}