//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OmnimarkAmazon.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RelatedProduct
    {
        public System.Guid ProductID { get; set; }
        public string ASIN { get; set; }
        public System.Guid RelatedProductID { get; set; }
        public string RelatedProductName { get; set; }
        public decimal RelatedProductQty { get; set; }
        public decimal ProductQty { get; set; }
    }
}
