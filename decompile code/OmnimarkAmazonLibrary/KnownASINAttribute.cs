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
    
    public partial class KnownASINAttribute
    {
        public string ASIN { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.DateTime> UpdateTimeStamp { get; set; }
    
        public virtual KnownASIN KnownASIN { get; set; }
    }
}
