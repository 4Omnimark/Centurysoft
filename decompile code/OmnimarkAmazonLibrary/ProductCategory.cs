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
    
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            this.ProductTags = new HashSet<ProductTag>();
            this.Products = new HashSet<Product>();
        }
    
        public System.Guid ID { get; set; }
        public Nullable<int> DisplaySeq { get; set; }
        public string Name { get; set; }
        public System.DateTime TimeStamp { get; set; }
    
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
