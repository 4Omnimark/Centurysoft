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
    
    public partial class User
    {
        public System.Guid UserID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Phone { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
    }
}
